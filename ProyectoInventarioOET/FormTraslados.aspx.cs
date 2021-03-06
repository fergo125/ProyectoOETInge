﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Traslados;
using ProyectoInventarioOET.Modulo_Ajustes;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_ProductosLocales;

namespace ProyectoInventarioOET
{
    public partial class FormTraslados : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };

        // Atributos
        private static Boolean seConsulto = false;                              // True si se consulto y se debe visitar la base de datos
        private static Object[] idArrayTraslados;                               // Array de llaves que no se muestran en el grid de consultas
        private static Object[] idArrayProductosOrigen;                         // Array de llaves que no se muestran en el grid de productos, de bodega origen
        private static Object[] idArrayProductosDestino;                        // Array de llaves que no se muestran en el grid de productos, de bodega destino
        private static Object[] idArrayAgregarProductosOrigen;                  // Array de llaves que no se muestran en el grid de agregar productos, de bodega origen
        private static Object[] idArrayAgregarProductosDestino;                 // Array de llaves que no se muestran en el grid de agregar productos, de bodega destino
        private static DataTable tablaAgregarProductos;                         // Tabla en memoria de los productos agregables
        private static DataTable tablaProductos;                                // Tabla en memoria de los productos agregados
        private static int modo = (int)Modo.Inicial;                            // Modo actual de interfaz
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Controladora de datos generales
        private static ControladoraTraslado controladoraTraslados;              // Controladora del modulo traslados
        private static ControladoraProductoLocal controladoraProductoLocal;     // Controladora de los productos locales de una bodega
        private static EntidadTraslado trasladoConsultado;                      // El traslado mostrado en pantalla
        private static bool tipoConsulta;                                       // True si se esta viendo entradas, false si salidas
        private static String stringBusqueda;                                   // String que se está siendo buscado
        private static ArrayList trasladosGuardados;                            // Guarda temporalmente los valores de los campos editables del grid de productos

        protected void Page_Load(object sender, EventArgs e)
        {
            //Elementos visuales
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
            mensajeAlerta.Visible = false;

            if (!IsPostBack)
            {
                labelAlerta.Text = "";

                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraTraslados = new ControladoraTraslado();
                controladoraProductoLocal = new ControladoraProductoLocal();
                controladoraTraslados.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraProductoLocal.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                trasladosGuardados = new ArrayList();

                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Traslados de inventario");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                mostrarBotonesSegunPermisos();


                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (trasladoConsultado == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar el traslado.");
                    }
                    else
                    {
                        cargarBodegas();
                        cargarEstados();
                        setDatosConsultados();

                        seConsulto = false;
                    }
                }
            }
            cambiarModo();
        }

        /*
         * Maneja la activación y desactivación de objetos dependiendo de la operación a realizar por el usuario
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial: //modo inicial
                    limpiarCampos();
                    botonAgregar.Visible = false;
                    FieldsetTraslados.Visible = false;
                    botonAceptarTraslado.Visible = false;
                    botonCancelarTraslado.Visible = false;
                    tituloAccionTraslados.InnerText = "Seleccione una opción";
                    botonRealizarTraslado.Disabled = false;
                    botonModificarTraslado.Disabled = true;
                    botonConsultarTraslado.Disabled = false;
                    tituloGridProductos.Visible = false;
                    tituloGridConsulta.Visible = false;
                    gridViewTraslados.Visible = false;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = false;
                    fieldsetConsulta.Visible = false;
                    habilitarCampos(false);
                    break;

                case (int)Modo.Insercion: //insertar
                    botonAgregar.Visible = true;
                    FieldsetTraslados.Visible = true;
                    botonAceptarTraslado.Visible = true;
                    botonCancelarTraslado.Visible = true;
                    tituloAccionTraslados.InnerText = "Ingrese datos";
                    botonRealizarTraslado.Disabled = true;
                    botonModificarTraslado.Disabled = true;
                    botonConsultarTraslado.Disabled = false;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = false;
                    gridViewTraslados.Visible = false;
                    gridViewProductos.Enabled = true;
                    gridViewProductos.Visible = true;
                    fieldsetConsulta.Visible = false;
                    fieldsetEstado.Visible = false;
                    habilitarCampos(true);
                    foreach (DataControlField col in gridViewProductos.Columns)
                        col.Visible = true;
                    break;

                case (int)Modo.Consulta://consultar
                    botonAgregar.Visible = false;
                    FieldsetTraslados.Visible = false;
                    botonAceptarTraslado.Visible = false;
                    botonCancelarTraslado.Visible = false;
                    tituloAccionTraslados.InnerText = "Seleccione un traslado";
                    botonRealizarTraslado.Disabled = false;
                    botonModificarTraslado.Disabled = true;
                    botonConsultarTraslado.Disabled = false;
                    tituloGridProductos.Visible = false;
                    //tituloGridConsulta.Visible = true;
                    //gridViewTraslados.Visible = true;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = false;
                    fieldsetConsulta.Visible = true;
                    habilitarCampos(false);
                    break;

                case (int)Modo.Modificacion: //modificar
                    botonAgregar.Visible = false;
                    FieldsetTraslados.Visible = true;
                    botonAceptarTraslado.Visible = true;
                    botonCancelarTraslado.Visible = true;
                    tituloAccionTraslados.InnerText = "Cambie los datos";
                    botonRealizarTraslado.Disabled = true;
                    botonModificarTraslado.Disabled = true;
                    botonConsultarTraslado.Disabled = false;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = false;
                    gridViewTraslados.Visible = false;
                    gridViewProductos.Enabled = true;
                    gridViewProductos.Visible = true;
                    fieldsetConsulta.Visible = false;
                    fieldsetEstado.Visible = true;
                    dropDownEstado.Enabled = true;
                    habilitarCampos(false);
                    foreach (DataControlField col in gridViewProductos.Columns)
                        col.Visible = false;
                    break;

                case (int)Modo.Consultado://consultado, pero con los espacios bloqueados
                    botonAgregar.Visible = false;
                    FieldsetTraslados.Visible = true;
                    botonAceptarTraslado.Visible = false;
                    botonCancelarTraslado.Visible = false;
                    tituloAccionTraslados.InnerText = "Traslado seleccionado";
                    botonRealizarTraslado.Disabled = false;
                    botonModificarTraslado.Disabled = !(trasladoConsultado.BodegaDestino == (this.Master as SiteMaster).NombreBodegaSesion && trasladoConsultado.Estado == "En Proceso");
                    botonConsultarTraslado.Disabled = false;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = true;
                    gridViewTraslados.Visible = true;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = true;
                    fieldsetConsulta.Visible = false;
                    fieldsetEstado.Visible = true;
                    dropDownEstado.Enabled = false;
                    habilitarCampos(false);
                    foreach (DataControlField col in gridViewProductos.Columns)
                        col.Visible = false;
                    /*
                    if( tipoConsulta )
                    {
                        dropDownConsultas.Items.FindByValue("Salidas").Selected = false;
                        dropDownConsultas.Items.FindByValue("Entradas").Selected = true;
                    }
                        
                    else
                    {
                        dropDownConsultas.Items.FindByValue("Entradas").Selected = false;
                        dropDownConsultas.Items.FindByValue("Salidas").Selected = true;
                    }
                     */   
                    llenarGrid(tipoConsulta);
                    break;

                default:
                    // Algo salio mal
                    break;
            }
        }

        /*
         * Toma la entidad consultada y carga su información en la interfaz
         */
        protected void setDatosConsultados()
        {
            this.outputBodegaSalida.Value = trasladoConsultado.BodegaOrigen;

            // Setear Bodega Destino
            this.dropDownBodegaEntrada.SelectedItem.Selected = false;
            this.dropDownBodegaEntrada.Items.FindByText(trasladoConsultado.BodegaDestino).Selected = true;

            // Setear Estado
            this.dropDownEstado.SelectedItem.Selected = false;
            this.dropDownEstado.Items.FindByText(trasladoConsultado.Estado).Selected = true;

            this.outputUsuario.Value = trasladoConsultado.Usuario;
            this.outputFecha.Value = trasladoConsultado.Fecha.ToString();
            this.inputNotas.Text = trasladoConsultado.Notas;

            // Manejo grid
            DataTable tabla = tablaProductoConsulta();
            Object[] datos = new Object[4];
            if (trasladoConsultado.Detalles.Count > 0)
            {
                foreach (EntidadDetalles elemento in trasladoConsultado.Detalles)
                {
                    datos[0] = elemento.NombreProducto;
                    datos[1] = elemento.Codigo;
                    datos[2] = elemento.Cambio;
                    datos[3] = elemento.Unidades;
                    tabla.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                datos[2] = 0;
                datos[3] = "-";
                tabla.Rows.Add(datos);
            }


            gridViewProductos.DataSource = tabla;
            gridViewProductos.DataBind();
        }

        /*
         * Limpia los campos editables
         */
        protected void limpiarCampos()
        {
            dropDownBodegaEntrada.Items.Clear();
            vaciarGridProductos();
            inputNotas.Text = "";
        }

        /*
         * Limpia el grid de consulta
         */
        protected void vaciarGridTraslados()
        {
            DataTable tablaLimpia = null;
            gridViewTraslados.DataSource = tablaLimpia;
            gridViewTraslados.DataBind();
        }

        /*
         * Habilita o desabilita los campos editables
         */
        protected void habilitarCampos(bool habilitar)
        {
            this.inputNotas.Enabled = habilitar;
            this.dropDownBodegaEntrada.Enabled = habilitar;
            gridViewProductos.Enabled = habilitar;
            // Habilitar/Desabilitar botones de grid
        }

        /*
         * Actualiza el contenido del mensaje y lo hace visible
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {

            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        }

        /*
         * Método auxiliar que viaja a la base de datos y maneja la consulta de ajustes
         */
        protected void consultarTraslado(String id)
        {
            seConsulto = true;
            try
            {
                trasladoConsultado = controladoraTraslados.consultarTraslado(id);
                modo = (int)Modo.Consultado;
            }
            catch
            {
                trasladoConsultado = null;
                modo = (int)Modo.Inicial;
            }
            cambiarModo();
        }

        /*
         * Retorna la información del traslado como un array de objetos
         */
        protected Object[] obtenerDatosTraslado()
        {
            Object[] datos = new Object[10];
            datos[0] = "";
            datos[1] = this.outputFecha.Value;
            datos[2] = (this.Master as SiteMaster).Usuario.Nombre;
            datos[3] = (this.Master as SiteMaster).Usuario.Codigo;
            datos[4] = this.inputNotas.Text;
            datos[5] = (this.Master as SiteMaster).LlaveBodegaSesion;
            datos[6] = dropDownBodegaEntrada.SelectedValue;
            datos[7] = (this.Master as SiteMaster).NombreBodegaSesion;
            datos[8] = dropDownBodegaEntrada.SelectedItem.Text;
            datos[9] = 0;
            return datos;
        }

        /*
         * Maneja la inserción de un nuevo traslado
         */
        protected String insertar()
        {
            String codigo = "";
            Object[] traslado = obtenerDatosTraslado();
            EntidadTraslado nuevo = new EntidadTraslado(traslado);
            DataTable productoDeBodega;
            bool alerta = false;
            double saldoNuevo;

            // Agregar detalles a entidad
            int i = 0;
            try
            {
                if( nuevo.IdBodegaDestino == nuevo.IdBodegaOrigen )
                    throw new EvaluateException();
                if (idArrayProductosDestino.Count() < 1)
                    throw new NoNullAllowedException();

                foreach (DataRow row in tablaProductos.Rows)
                {
                    if (((TextBox)gridViewProductos.Rows[i].FindControl("textTraslados")).Text == "")
                        throw new NullReferenceException();
                    Double cantAjuste = Double.Parse(((TextBox)gridViewProductos.Rows[i].FindControl("textTraslados")).Text);
                    if( cantAjuste <= 0 )
                        throw new InvalidConstraintException();

                    traslado = new Object[6];
                    traslado[0] = traslado[1] = traslado[3] = "";
                    traslado[2] = cantAjuste;
                    traslado[4] = idArrayProductosOrigen[i];
                    traslado[5] = idArrayProductosDestino[i];

                    productoDeBodega = controladoraProductoLocal.consultarMinimoMaximoProductoEnBodega(idArrayProductosOrigen[i].ToString());
                    saldoNuevo = Convert.ToDouble(productoDeBodega.Rows[0][2].ToString()) - cantAjuste;
                    if (saldoNuevo < 0)
                        throw new ArgumentException();
                    alerta |= cantAjuste <= Convert.ToDouble(productoDeBodega.Rows[0][0].ToString()) || cantAjuste >= Convert.ToDouble(productoDeBodega.Rows[0][1].ToString());

                    nuevo.agregarDetalle(traslado);
                    ++i;
                }


                String[] error = controladoraTraslados.insertarTraslado(nuevo);

                codigo = Convert.ToString(error[3]);
                if (error[0].Contains("success"))
                {
                    llenarGrid(false);
                    tipoConsulta = false;
                    if (alerta)
                    {
                        error[0] = "warning";
                        error[2] += "\nUno o más productos han salido de sus límites permitidos (nivel máximo o mínimo), revise el catálogo local.";
                    }
                }
                else
                {
                    codigo = "";
                    modo = (int)Modo.Insercion;
                }
                mostrarMensaje(error[0], error[1], error[2]);
            }
            catch (NoNullAllowedException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "No puede realizarse un traslado sin productos");
            }
            catch (ArgumentException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "Uno o más productos quedarían en saldo negativo si realiza el traslado");
            }
            catch (InvalidConstraintException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "Está tratando de trasladar 0 instancias de un producto");
            }
            catch (NullReferenceException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "Introducir cantidad a transferir");
            }
            catch (EvaluateException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "No se puede transferir a la misma bodega");
            }
            return codigo;
        }

        /*
         * Maneja el cambio de estado de un traslado 
         */
        protected bool modificar()
        {
            Boolean res = true;
            String[] error = {"warning", "Alerta: ", "No hubo cambios"};

            if (dropDownEstado.SelectedValue == "1")
                error = controladoraTraslados.acertarTraslado(trasladoConsultado);
            else
                if (dropDownEstado.SelectedValue == "-1")
                    error = controladoraTraslados.rechazarTraslado(trasladoConsultado);
                else
                    res = false;
            trasladoConsultado.Estado = dropDownEstado.SelectedValue;
            mostrarMensaje(error[0], error[1], error[2]);

            if (res && (error[0].Contains("success") || error[0].Contains("warning") ))// si fue exitoso
            {
                //llenarGrid(true);
                modo = (int)Modo.Consultado;
            }
            else
            {
                modo = (int)Modo.Modificacion;
            }
            return res;
        }

        /*
         * Crea una datatable en el formato del grid de consultas
         */
        protected DataTable tablaTraslados()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Bodega Origen";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Bodega Destino";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.DateTime");
            columna.ColumnName = "Fecha y Hora";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Encargado";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Crea una datatable en el formato del grid de productos en ajustes
         */
        protected DataTable tablaProducto()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Código";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Cantidad Actual";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Unidad Métrica";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Crea una datatable en el formato del grid de productos en ajustes, cuando son consultados
         */
        protected DataTable tablaProductoConsulta()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Código";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Cantidad Transferida";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Unidad Métrica";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Crea una datatable en el formato del grid de agregar productos
         */
        protected DataTable tablaAgregarProducto()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Código";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Cantidad Actual";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Unidad Métrica";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Minimo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Maximo";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Viaja a la base de datos y obtiene los datos de consulta
         */
        protected void llenarGrid(bool entrada)
        {
            DataTable tabla = tablaTraslados();
            int indiceNuevoTraslado = -1;
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[5];

                DataTable traslados = controladoraTraslados.consultarTraslados((this.Master as SiteMaster).LlaveBodegaSesion, entrada);
                //EntidadTraslado p = controladoraTraslados.consultarTraslado("1111");
                
                
                if (traslados.Rows.Count > 0)
                {
                    idArrayTraslados = new Object[traslados.Rows.Count];
                    foreach (DataRow fila in traslados.Rows)
                    {
                        idArrayTraslados[i] = fila[0];
                        datos[0] = fila[5];
                        datos[1] = fila[6];
                        datos[2] = fila[8];
                        datos[3] = fila[1];
                        datos[4] = fila[2];
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = Convert.ToDateTime("01/01/1997");
                    datos[4] = "-";
                    tabla.Rows.Add(datos);
                    mostrarMensaje("warning", "Atención: ", "No existen traslados de ese tipo.");
                }

                this.gridViewTraslados.DataSource = tabla;
                this.gridViewTraslados.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * Limpia el grid de productos
         */
        protected void vaciarGridProductos()
        {
            DataTable tablaLimpia = tablaProducto();

            Object[] datos = new Object[4];
            datos[0] = "-";
            datos[1] = "-";
            datos[2] = "0";
            datos[3] = "-";
            tablaLimpia.Rows.Add(datos);

            gridViewProductos.Visible = false;
            tituloGridProductos.Visible = false;

            gridViewProductos.DataSource = tablaLimpia;
            gridViewProductos.DataBind();

            idArrayProductosOrigen = new Object[0];
            idArrayProductosDestino = new Object[0];
            tablaProductos = tablaProducto();
        }

        /*
         * Carga datos del grid de productos agregables
         */
        protected void llenarGridAgregarProductos(String query)
        {

            DataTable tabla = tablaAgregarProducto();
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[6];

                DataTable productos = controladoraTraslados.consultarProductosTrasferibles((this.Master as SiteMaster).LlaveBodegaSesion, dropDownBodegaEntrada.SelectedValue, query);

                if (productos.Rows.Count > 0)
                {
                    idArrayAgregarProductosOrigen = new Object[productos.Rows.Count];
                    idArrayAgregarProductosDestino = new Object[productos.Rows.Count];
                    foreach (DataRow fila in productos.Rows)
                    {
                        if( !idArrayProductosOrigen.Contains(fila[6]) )
                        {
                            idArrayAgregarProductosOrigen[i] = fila[6];
                            idArrayAgregarProductosDestino[i] = fila[7];
                            datos[0] = fila[1].ToString();
                            datos[1] = fila[2].ToString();
                            datos[2] = Convert.ToDouble(fila[3].ToString());
                            datos[3] = fila[8].ToString();
                            datos[4] = Convert.ToDouble(fila[4].ToString());
                            datos[5] = Convert.ToDouble(fila[5].ToString());
                            tabla.Rows.Add(datos);
                            i++;
                        }
                    }
                    if( idArrayAgregarProductosOrigen[0] == null )
                    {
                        datos[0] = "-";
                        datos[1] = "-";
                        datos[2] = "0";
                        datos[3] = "-";
                        datos[4] = "0";
                        datos[5] = "0";
                        tabla.Rows.Add(datos);
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "0";
                    datos[3] = "-";
                    datos[4] = "0";
                    datos[5] = "0";
                    tabla.Rows.Add(datos);
                    if( query == "" )
                        mostrarMensaje("warning", "Atención: ", "No existen productos en la bodega actual.");
                }

                this.gridViewAgregarProductos.DataSource = tabla;
                this.gridViewAgregarProductos.DataBind();
                tablaAgregarProductos = tabla;
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }

        }

        /*
         * Maneja el cargado de estados a memoria
         */
        protected void cargarEstados()
        {
            this.dropDownEstado.Items.Clear();
            this.dropDownEstado.Items.Add(new ListItem("En Proceso", "0"));
            this.dropDownEstado.Items.Add(new ListItem("Rechazado", "-1"));
            this.dropDownEstado.Items.Add(new ListItem("Aceptado", "1"));
        }

        /*
         * Maneja el cargado de bodegas a memoria, para seleccionar la bodega de destino
         */
        protected void cargarBodegas()
        {
            DataTable bodegas = controladoraTraslados.consultarBodegas((this.Master as SiteMaster).Usuario.Codigo,(this.Master as SiteMaster).Usuario.CodigoPerfil);
            if (bodegas.Rows.Count > 0)
            {
                this.dropDownBodegaEntrada.Items.Clear();
                foreach (DataRow fila in bodegas.Rows)
                {
                    this.dropDownBodegaEntrada.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                }
            }
        }

        /*
         * Guarda los datos de los campos que son borrados al cambiar el dataGrid
         */
        protected void guardarDatos()
        {
            int i = 0;
            trasladosGuardados.Clear();
            foreach (GridViewRow row in gridViewProductos.Rows)
            {
                String valor = ((TextBox)gridViewProductos.Rows[i].FindControl("textTraslados")).Text; // Caso en que no se especifico un ajuste
                valor = valor.Equals("") ? "-99" : valor;
                try
                {
                    trasladosGuardados.Add(Double.Parse(valor));
                }
                catch
                {
                    trasladosGuardados.Add(-99.0);
                }
                ++i;
            }
        }

        /*
         * Repone los datos de los campos que son borrados al cambiar el dataGrid
         */
        private void reponerDatos()
        {
            int i = 0;
            foreach (double current in trasladosGuardados)
            {
                ((TextBox)gridViewProductos.Rows[i].FindControl("textTraslados")).Text = current.ToString().Equals("-99") ? "" : current.ToString(); // Caso en que no se especifico un ajuste
                ++i;
            }
        }

        /*
         * Selección de bodega, esta invalida los productos anteriores.
         */
        protected void dropDownBodegaEntrada_SelectedIndexChanged(object sender, EventArgs e)
        {
            stringBusqueda = "";
            vaciarGridProductos();
            llenarGridAgregarProductos(stringBusqueda);
        }

        /*
         * Esto pasa la interfaz al modo de crear traslados.
         */
        protected void botonRealizarTraslado_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            stringBusqueda = "";
            cambiarModo();
            limpiarCampos();
            cargarBodegas();
            cargarEstados();
            llenarGridAgregarProductos(stringBusqueda);
            vaciarGridProductos();

            gridViewProductos.Visible = false;

            if ((this.Master as SiteMaster).Usuario != null)
                outputUsuario.Value = (this.Master as SiteMaster).Usuario.Nombre;
            outputBodegaSalida.Value = (this.Master as SiteMaster).NombreBodegaSesion;
            outputFecha.Value = DateTime.Now.ToString();
        }

        /*
         * Esto pasa la interfaz al modo de modificar traslados.
         */
        protected void botonModificarTraslado_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            cambiarModo();
            // Agregar cambio de estado
        }

        /*
         * Esto pasa la interfaz al modo de consulta.
         */
        protected void botonConsultarTraslado_ServerClick(object sender, EventArgs e)
        {
            //DataTable prueba = controladoraTraslados.consultarTraslados((this.Master as SiteMaster).LlaveBodegaSesion, false);
            outputBodegaConsulta.Value = (this.Master as SiteMaster).NombreBodegaSesion;
            modo = (int)Modo.Consulta;
            cambiarModo();

            // No enseñar una tabla vacia
            tituloGridConsulta.Visible = false;
            gridViewTraslados.Visible = false;
            DataTable tabla = tablaTraslados();
            Object[] datos = new Object[5];
            datos[0] = "-";
            datos[1] = "-";
            datos[2] = "-";
            datos[3] = Convert.ToDateTime("01/01/1997");
            datos[4] = "-";
            tabla.Rows.Add(datos);
            this.gridViewTraslados.DataSource = tabla;
            this.gridViewTraslados.DataBind();
        }

        /*
         * Método que maneja la selección de un traslado en el grid de productos.
         */
        protected void gridViewProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            guardarDatos();
            if (idArrayProductosOrigen != null && idArrayProductosOrigen.Count() > 0)
            {
                switch (e.CommandName)
                {
                    case "Select":
                        int indice = Convert.ToInt32(e.CommandArgument);

                        // Eliminar vieja tupla de grid
                        trasladosGuardados.RemoveAt(indice);
                        tablaProductos.Rows[indice].Delete();
                        gridViewProductos.DataSource = tablaProductos;
                        gridViewProductos.DataBind();
                        reponerDatos();

                        // Actualizar listas de Ids
                        List<Object> temp = new List<Object>(idArrayProductosOrigen);
                        temp.RemoveAt(indice);
                        idArrayProductosOrigen = temp.ToArray();

                        temp = new List<Object>(idArrayProductosDestino);
                        temp.RemoveAt(indice);
                        idArrayProductosDestino = temp.ToArray();

                        if (idArrayProductosOrigen.Count() < 1)
                            vaciarGridProductos();

                        // Volver a cargar el grid, para obtener los datos del producto
                        llenarGridAgregarProductos(barraDeBusqueda.Value);

                        break;
                }
            }
        }

        protected void gridViewTraslados_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            // Intitialize TableCell list
            List<TableCell> columns = new List<TableCell>();
            int i = 0;
            foreach (DataControlField column in gridViewProductos.Columns)
            {
                if (i == 0)
                {
                    //Get the first Cell /Column
                    TableCell cell = row.Cells[1];
                    // Then Remove it after
                    row.Cells.Remove(cell);
                    //And Add it to the List Collections
                    columns.Add(cell);

                }
                i++;

            }

            // Add cells
            row.Cells.AddRange(columns.ToArray());
         }

        /*
         * Este método confirma las transacciones de traslados.
         */
        protected void botonAceptarTraslado_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";

            if (modo == (int)Modo.Insercion)
            {
                codigoInsertado = insertar();

                if (codigoInsertado != "")
                {
                    operacionCorrecta = true;
                    //trasladoConsultado = controladoraTraslados.consultarTraslado(codigoInsertado);
                    //modo = (int)Modo.Consultado;
                    modo = (int)Modo.Inicial;
                    //habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == (int)Modo.Modificacion)
            {
                operacionCorrecta = modificar();
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }

        /*
         * Método que maneja la aceptar la cancelación.
         * Elimina datos y reinicia la interfaz.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            vaciarGridTraslados();
            modo = (int)Modo.Inicial;
            cambiarModo();
            limpiarCampos();
            trasladoConsultado = null;
        }

        /*
         * Método que, al cambiar el valor seleccionado del dropDown, limpia el grid.
         */
        protected void dropDownConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridViewTraslados.Visible = false;
            tituloGridConsulta.Visible = false;
            FieldsetTraslados.Visible = false;
            FieldsetGridProductos.Visible = false;
            modo = (int)Modo.Consulta;
        }

        /*
         * Método que consulta los traslados, dependiendo del tipo seleccionado
         */
        protected void botonTipoConsulta_ServerClick(object sender, EventArgs e)
        {
            llenarGrid(dropDownConsultas.SelectedValue == "Entrantes");
            tipoConsulta = dropDownConsultas.SelectedValue == "Entrantes";
            gridViewTraslados.Visible = true;
            tituloGridConsulta.Visible = true;
        }

        /*
         * Método que maneja la selección de un traslado en el grid de consultar.
         */
        protected void gridViewTraslados_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            if (idArrayTraslados != null && idArrayTraslados.Count() > 0)
            {
                switch (e.CommandName)
                {
                    case "Select":
                        GridViewRow filaSeleccionada = this.gridViewTraslados.Rows[Convert.ToInt32(e.CommandArgument)];
                        //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                        String codigo = Convert.ToString(idArrayTraslados[Convert.ToInt32(e.CommandArgument) + (this.gridViewTraslados.PageIndex * this.gridViewTraslados.PageSize)]);
                        consultarTraslado(codigo);
                        //controladoraTraslados.acertarTraslado(trasladoConsultado);
                        modo = (int)Modo.Consultado;
                        Response.Redirect("FormTraslados.aspx");
                        break;
                }
            }
            
        }

        /*
         * Método que maneja el cambio de páginas en el grid de consultar
         */
        protected void gridViewTraslados_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid(dropDownConsultas.SelectedValue == "Entrantes");
            this.gridViewTraslados.PageIndex = e.NewPageIndex;
            this.gridViewTraslados.DataBind();
        }

        /*
         * Maneja la búsqueda para el usuario.
         */
        protected void botonBuscar_OnClick(object sender, EventArgs e)
        {
            stringBusqueda = barraDeBusqueda.Value;
            llenarGridAgregarProductos(stringBusqueda);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "test", "mifuncion() ;", true);
        }
        

        /*
         * Método que maneja la selección de un traslado en el grid de agregar productos.
         */
        protected void gridViewAgregarProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            if (idArrayAgregarProductosDestino != null && idArrayAgregarProductosDestino.Count() > 0)
            {
                switch (e.CommandName)
                {
                    case "Select":
                        int indice = Convert.ToInt32(e.CommandArgument) + (this.gridViewAgregarProductos.PageIndex * this.gridViewAgregarProductos.PageSize);
                        DataRow seleccionada = tablaAgregarProductos.Rows[indice];

                        // Sacamos datos pertinentes del producto
                        Object[] datos = new Object[4];
                        datos[0] = seleccionada["Nombre"];
                        datos[1] = seleccionada["Código"];
                        datos[2] = seleccionada["Cantidad Actual"];
                        datos[3] = seleccionada["Unidad Métrica"];

                        // Hacerla visible
                        gridViewProductos.Visible = true;
                        tituloGridProductos.Visible = true;

                        // Agregar nueva tupla a tabla
                        tablaProductos.Rows.Add(datos);
                        guardarDatos();
                        gridViewProductos.DataSource = tablaProductos;
                        gridViewProductos.DataBind();
                        reponerDatos();

                        // Eliminar vieja tupla de grid
                        tablaAgregarProductos.Rows[Convert.ToInt32(e.CommandArgument) + (this.gridViewAgregarProductos.PageIndex * this.gridViewAgregarProductos.PageSize)].Delete();
                        gridViewAgregarProductos.DataSource = tablaAgregarProductos;
                        gridViewAgregarProductos.DataBind();

                        // Actualizar listas de Ids
                        List<Object> temp = new List<Object>(idArrayProductosOrigen);
                        temp.Add(idArrayAgregarProductosOrigen[indice]);
                        idArrayProductosOrigen = temp.ToArray();

                        temp = new List<Object>(idArrayProductosDestino);
                        temp.Add(idArrayAgregarProductosDestino[indice]);
                        idArrayProductosDestino = temp.ToArray();

                        temp = new List<Object>(idArrayAgregarProductosOrigen);
                        temp.RemoveAt(indice);
                        idArrayAgregarProductosOrigen = temp.ToArray();

                        temp = new List<Object>(idArrayAgregarProductosDestino);
                        temp.RemoveAt(indice);
                        idArrayAgregarProductosDestino = temp.ToArray();

                        //Response.Redirect("FormTraslados.aspx");
                        break;
                }
            }
        }

        protected void gridViewAgregarProductos_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGridAgregarProductos(stringBusqueda);
            this.gridViewAgregarProductos.PageIndex = e.NewPageIndex;
            this.gridViewAgregarProductos.DataBind();
        }

        protected void mostrarBotonesSegunPermisos()
        {
            this.botonConsultarTraslado.Visible = (permisos[5] == '1');
            this.botonRealizarTraslado.Visible = (permisos[4] == '1');
            this.botonModificarTraslado.Visible = (permisos[3] == '1');
        }


    }
}