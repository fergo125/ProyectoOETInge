using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.App_Code.Modulo_Traslados;

namespace ProyectoInventarioOET
{
    public partial class FormTraslados : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };

        // Atributos
        private static Boolean seConsulto = false;                              // True si se consulto y se debe visitar la base de datos
        private static Object[] idArrayTraslados;                               // Array de llaves que no se muestran en el grid de consultas
        private static Object[] idArrayProductos;                               // Array de llaves que no se muestran en el grid de productos
        private static Object[] idArrayAgregarProductos;                        // Array de llaves que no se muestran en el grid de agregar productos
        private static DataTable tablaAgregarProductos;                         // Tabla en memoria de los productos agregables
        private static DataTable tablaProductos;                                // Tabla en memoria de los productos agregados
        private static int modo = (int)Modo.Inicial;                            // Modo actual de interfaz
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Controladora de datos generales
        private static ControladoraTraslado controladoraTraslados;                 // Controladora del modulo ajustes
        private static EntidadTraslado trasladoConsultado;                         // El ajuste mostrado en pantalla

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
                        // cargarBodegas
                        // setDatosConsultados();

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
                    tituloAccionTraslados.InnerText = "Seleccione un ajuste";
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

                case (int)Modo.Modificacion: //insertar
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
                    habilitarCampos(true);
                    gridViewProductos.Columns[1].Visible = true;
                    break;

                case (int)Modo.Consultado://consultado, pero con los espacios bloqueados
                    botonAgregar.Visible = false;
                    FieldsetTraslados.Visible = true;
                    botonAceptarTraslado.Visible = false;
                    botonCancelarTraslado.Visible = false;
                    tituloAccionTraslados.InnerText = "Ajuste seleccionado";
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
                    //llenarGrid();
                    break;

                default:
                    // Algo salio mal
                    break;
            }
        }

        /*
         * Limpia los campos editables
         */
        protected void limpiarCampos()
        {
            //Clear dropDown bodegas
            //vaciarGridProductos();
            inputNotas.Text = "";
        }

        /*
         * Habilita o desabilita los campos editables
         */
        protected void habilitarCampos(bool habilitar)
        {
            this.inputNotas.Enabled = habilitar;
            this.dropDownBodegaEntrada.Disabled = !habilitar;
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
            int indiceNuevoAjuste = -1;
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
                        /*
                        if (ajusteConsultado != null && (fila[0].Equals(ajusteConsultado.Codigo)))
                        {
                            indiceNuevoAjuste = i;
                        }
                         */
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

                DataTable productos = controladoraTraslados.consultarProductosTrasferibles((this.Master as SiteMaster).LlaveBodegaSesion, "PITAN129012015101713605001");

                if (productos.Rows.Count > 0)
                {
                    idArrayAgregarProductos = new Object[productos.Rows.Count];
                    foreach (DataRow fila in productos.Rows)
                    {
                        idArrayAgregarProductos[i] = fila[0];
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
         * Esto pasa la interfaz al modo de crear traslados.
         */
        protected void botonRealizarTraslado_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            llenarGridAgregarProductos();
            //vaciarGridProductos();

            //cargarTipos();
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
            DataTable prueba = controladoraTraslados.consultarTraslados("PITAN129012015101713605001", false);
            int y = 9+6;
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
         * Método que maneja la selección de un ajuste en el grid de productos.
         */
        protected void gridViewProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewTraslados.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArrayTraslados[Convert.ToInt32(e.CommandArgument) + (this.gridViewTraslados.PageIndex * this.gridViewTraslados.PageSize)]);
                    controladoraTraslados.consultarTraslado(codigo);
                    
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormTraslados.aspx");
                    break;
            }
        }

        /*
         * Este método confirma las transacciones de traslados.
         */
        protected void botonAceptarTraslado_ServerClick(object sender, EventArgs e)
        {
            
        }

        /*
         * Método que maneja la aceptar la cancelación.
         * Elimina datos y reinicia la interfaz.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            //vaciarGridAjustes();
            //modo = (int)Modo.Inicial;
            //cambiarModo();
            //limpiarCampos();
            //ajusteConsultado = null;
        }

        /*
         * Método que consulta los traslados, dependiendo del tipo seleccionado
         */
        protected void botonTipoConsulta_ServerClick(object sender, EventArgs e)
        {
            llenarGrid(dropDownConsultas.SelectedValue == "Entradas");
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de consultar.
         */
        protected void gridViewTraslados_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewTraslados.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArrayTraslados[Convert.ToInt32(e.CommandArgument) + (this.gridViewTraslados.PageIndex * this.gridViewTraslados.PageSize)]);
                    trasladoConsultado  = controladoraTraslados.consultarTraslado(codigo);
                    controladoraTraslados.acertarTraslado(trasladoConsultado);
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
            //llenarGrid();
            this.gridViewTraslados.PageIndex = e.NewPageIndex;
            this.gridViewTraslados.DataBind();
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de agregar productos.
         */
        protected void gridViewAgregarProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            /*
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
                    List<Object> temp = new List<Object>(idArrayProductos);
                    temp.Add(idArrayAgregarProductos[indice]);
                    idArrayProductos = temp.ToArray();

                    temp = new List<Object>(idArrayAgregarProductos);
                    temp.RemoveAt(indice);
                    idArrayAgregarProductos = temp.ToArray();

                    //Response.Redirect("FormTraslados.aspx");
                    break;
            }
            */
        }

        protected void gridViewAgregarProductos_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGridAgregarProductos();
            this.gridViewAgregarProductos.PageIndex = e.NewPageIndex;
            this.gridViewAgregarProductos.DataBind();
        }
    }
}