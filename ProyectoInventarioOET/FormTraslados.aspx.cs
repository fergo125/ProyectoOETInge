using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.App_Code.Modulo_Traslados;
using ProyectoInventarioOET.App_Code.Modulo_Ajustes;
using ProyectoInventarioOET.Modulo_Seguridad;

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
        private static EntidadTraslado trasladoConsultado;                      // El traslado mostrado en pantalla
        private static bool tipoConsulta;                                       // True si se esta viendo entradas, false si salidas

        protected void Page_Load(object sender, EventArgs e)
        {
            //Elementos visuales
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
            mensajeAlerta.Visible = false;

            if (!IsPostBack)
            {
                labelAlerta.Text = "";

                controladoraTraslados = new ControladoraTraslado();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;

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
                    gridViewProductos.Columns[1].Visible = false;
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
                    habilitarCampos(true);
                    gridViewProductos.Columns[1].Visible = true;
                    break;

                case (int)Modo.Consulta://consultar
                    botonAgregar.Visible = false;
                    FieldsetTraslados.Visible = false;
                    botonAceptarTraslado.Visible = false;
                    botonCancelarTraslado.Visible = false;
                    tituloAccionTraslados.InnerText = "Seleccione un traslado";
                    botonRealizarTraslado.Disabled = false;
                    botonModificarTraslado.Disabled = true;
                    botonConsultarTraslado.Disabled = true;
                    tituloGridProductos.Visible = false;
                    tituloGridConsulta.Visible = true;
                    gridViewTraslados.Visible = true;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = false;
                    fieldsetConsulta.Visible = true;
                    habilitarCampos(false);
                    gridViewProductos.Columns[1].Visible = false;
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
                    botonTipoConsulta.Disabled = false;
                    habilitarCampos(false);
                    gridViewProductos.Columns[1].Visible = true;
                    break;

                case (int)Modo.Consultado://consultado, pero con los espacios bloqueados
                    botonAgregar.Visible = false;
                    FieldsetTraslados.Visible = true;
                    botonAceptarTraslado.Visible = false;
                    botonCancelarTraslado.Visible = false;
                    tituloAccionTraslados.InnerText = "Traslado seleccionado";
                    botonRealizarTraslado.Disabled = false;
                    botonModificarTraslado.Disabled = false;
                    botonConsultarTraslado.Disabled = true;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = true;
                    gridViewTraslados.Visible = true;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = true;
                    fieldsetConsulta.Visible = true;
                    habilitarCampos(false);
                    gridViewProductos.Columns[1].Visible = false;
                    botonTipoConsulta.Disabled = true;
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
            this.dropDownBodegaEntrada.SelectedItem.Selected = false;
            dropDownBodegaEntrada.Items.FindByText(trasladoConsultado.BodegaDestino).Selected = true;
            this.outputUsuario.Value = trasladoConsultado.Usuario;
            this.outputFecha.Value = trasladoConsultado.Fecha.ToString();
            this.inputNotas.Text = trasladoConsultado.Notas;

            // Manejo grid
            DataTable tabla = tablaProductoConsulta();
            Object[] datos = new Object[3];
            if (trasladoConsultado.Detalles.Count > 0)
            {
                foreach (EntidadDetalles elemento in trasladoConsultado.Detalles)
                {
                    datos[0] = elemento.NombreProducto;
                    datos[1] = elemento.Codigo;
                    datos[2] = elemento.Cambio;
                    tabla.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                datos[2] = 0;
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


            // Agregar detalles a entidad
            int i = 0;
            foreach (DataRow row in tablaProductos.Rows)
            {
                Double cantAjuste = Double.Parse(((TextBox)gridViewProductos.Rows[i].FindControl("textTraslados")).Text);

                traslado = new Object[6];
                traslado[0] = traslado[1] = traslado[3] = "";
                traslado[2] = cantAjuste;
                traslado[4] = idArrayProductosOrigen[i];
                traslado[5] = idArrayProductosDestino[i];

                nuevo.agregarDetalle(traslado);
                ++i;
            }


            String[] error = controladoraTraslados.insertarTraslado(nuevo);

            codigo = Convert.ToString(error[3]);
            mostrarMensaje(error[0], error[1], error[2]);
            if (error[0].Contains("success"))
            {
                llenarGrid(false);
                tipoConsulta = false;
            }
            else
            {
                codigo = "";
                modo = (int)Modo.Insercion;
            }

            return codigo;
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
            columna.ColumnName = "Fecha";
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
            columna.ColumnName = "Codigo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Cantidad Actual";
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
            columna.ColumnName = "Codigo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Ajuste de cambio";
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
            columna.ColumnName = "Codigo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Cantidad Actual";
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

            Object[] datos = new Object[3];
            datos[0] = "-";
            datos[1] = "-";
            datos[2] = "0";
            tablaLimpia.Rows.Add(datos);

            gridViewProductos.DataSource = tablaLimpia;
            gridViewProductos.DataBind();

            idArrayProductosOrigen = new Object[0];
            idArrayProductosDestino = new Object[0];
            tablaProductos = tablaProducto();
        }

        /*
         * Carga datos del grid de productos agregables
         */
        protected void llenarGridAgregarProductos()
        {

            DataTable tabla = tablaAgregarProducto();
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[5];

                DataTable productos = controladoraTraslados.consultarProductosTrasferibles((this.Master as SiteMaster).LlaveBodegaSesion, dropDownBodegaEntrada.SelectedValue);

                if (productos.Rows.Count > 0)
                {
                    idArrayAgregarProductosOrigen = new Object[productos.Rows.Count];
                    idArrayAgregarProductosDestino = new Object[productos.Rows.Count];
                    foreach (DataRow fila in productos.Rows)
                    {
                        idArrayAgregarProductosOrigen[i] = fila[6];
                        idArrayAgregarProductosDestino[i] = fila[7];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[2].ToString();
                        datos[2] = Convert.ToDouble(fila[3].ToString());
                        datos[3] = Convert.ToDouble(fila[4].ToString());
                        datos[4] = Convert.ToDouble(fila[5].ToString());
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "0";
                    datos[3] = "0";
                    datos[4] = "0";
                    tabla.Rows.Add(datos);
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
         * Selección de bodega, esta invalida los productos anteriores.
         */
        protected void dropDownBodegaEntrada_SelectedIndexChanged(object sender, EventArgs e)
        {
            vaciarGridProductos();
            llenarGridAgregarProductos();
        }

        /*
         * Esto pasa la interfaz al modo de crear traslados.
         */
        protected void botonRealizarTraslado_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            cargarBodegas();
            llenarGridAgregarProductos();
            vaciarGridProductos();

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
            DataTable prueba = controladoraTraslados.consultarTraslados((this.Master as SiteMaster).LlaveBodegaSesion, false);
            modo = (int)Modo.Consulta;
            cambiarModo();

            // Enseñar una tabla vacia
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
            /*
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewTraslados.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArrayTraslados[Convert.ToInt32(e.CommandArgument) + (this.gridViewTraslados.PageIndex * this.gridViewTraslados.PageSize)]);      
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormTraslados.aspx");
                    break;
            }*/
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
                    trasladoConsultado = controladoraTraslados.consultarTraslado(codigoInsertado);
                    modo = (int)Modo.Consultado;
                    habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == (int)Modo.Modificacion)
            {
                //operacionCorrecta = modificar();
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
         * Método que consulta los traslados, dependiendo del tipo seleccionado
         */
        protected void botonTipoConsulta_ServerClick(object sender, EventArgs e)
        {
            llenarGrid(dropDownConsultas.SelectedValue == "Entradas");
            tipoConsulta = dropDownConsultas.SelectedValue == "Entradas";
        }

        /*
         * Método que maneja la selección de un traslado en el grid de consultar.
         */
        protected void gridViewTraslados_Seleccion(object sender, GridViewCommandEventArgs e)
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

        /*
         * Método que maneja el cambio de páginas en el grid de consultar
         */
        protected void gridViewTraslados_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid(dropDownConsultas.SelectedValue == "Entradas");
            this.gridViewTraslados.PageIndex = e.NewPageIndex;
            this.gridViewTraslados.DataBind();
        }

        /*
         * Método que maneja la selección de un traslado en el grid de agregar productos.
         */
        protected void gridViewAgregarProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    int indice = Convert.ToInt32(e.CommandArgument) + (this.gridViewAgregarProductos.PageIndex * this.gridViewAgregarProductos.PageSize);
                    DataRow seleccionada = tablaAgregarProductos.Rows[indice];

                    // Sacamos datos pertinentes del producto
                    Object[] datos = new Object[3];
                    datos[0] = seleccionada["Nombre"];
                    datos[1] = seleccionada["Codigo"];
                    datos[2] = seleccionada["Cantidad Actual"];

                    // Agregar nueva tupla a tabla
                    tablaProductos.Rows.Add(datos);
                    gridViewProductos.DataSource = tablaProductos;
                    gridViewProductos.DataBind();

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

        protected void gridViewAgregarProductos_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGridAgregarProductos();
            this.gridViewAgregarProductos.PageIndex = e.NewPageIndex;
            this.gridViewAgregarProductos.DataBind();
        }
    }
}