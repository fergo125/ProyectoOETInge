using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.App_Code.Módulo_Ajustes;
using ProyectoInventarioOET.Módulo_Seguridad;


namespace ProyectoInventarioOET
{
    public partial class FormAjustes : System.Web.UI.Page
    {

        enum Modo { Inicial, Consulta, Insercion, Consultado };

        // Atributos
        private static int resultadosPorPagina;                                 // Resultados por página que presenta el grid de consulta
        private static Boolean seConsulto = false;                              // True si se consulto y se debe visitar la base de datos
        private static Object[] idArray;                                        // Array de llaves que no se muestran en el grid de consultas
        private static int modo = (int)Modo.Inicial;                            // Modo actual de interfaz
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Controladora de datos generales
        private static ControladoraAjustes controladoraAjustes;                 // Controladora del modulo ajustes


        // DataTable bodegas = controladoraBodegas.consultarBodegasDeEstacion(idEstacion);

        /*
         * Método llamado cada vez que se carga la página.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            mensajeAlerta.Visible = false;

            if (!IsPostBack)
            {
                labelAlerta.Text = "";

                controladoraAjustes = new ControladoraAjustes();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;

                /*
                //Seguridad
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Gestion de bodegas");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                mostrarBotonesSegunPermisos();
                */

                /*
                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (bodegaConsultada == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la bodega.");
                    }
                    else
                    {
                        cargarEstados();
                        cargarAnfitriones();
                        cargarEstaciones();
                        cargarIntenciones();
                        setDatosConsultados();

                        seConsulto = false;
                    }
                }
                */
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
                    botonAgregar.Disabled = true;
                    FieldsetAjustes.Visible = false;
                    botonAceptarAjustes.Visible = false;
                    botonCancelarAjustes.Visible = false;
                    tituloAccionAjustes.InnerText = "Seleccione una opción";
                    botonRealizarAjuste.Disabled = false;
                    botonConsultarAjustes.Disabled = false;
                    tituloGridProductos.Visible = false;
                    tituloGridConsulta.Visible = false;
                    gridViewAjustes.Enabled = false;
                    gridViewProductos.Enabled = false;
                    habilitarCampos(false);
                    break;

                case (int)Modo.Insercion: //insertar
                    botonAgregar.Disabled = false;
                    FieldsetAjustes.Visible = true;
                    botonAceptarAjustes.Visible = true;
                    botonCancelarAjustes.Visible = true;
                    tituloAccionAjustes.InnerText = "Ingrese datos";
                    botonRealizarAjuste.Disabled = true;
                    botonConsultarAjustes.Disabled = false;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = false;
                    gridViewAjustes.Enabled = false;
                    gridViewProductos.Enabled = true;
                    habilitarCampos(true);
                    break;

                case (int)Modo.Consulta://consultar
                    botonAgregar.Disabled = true;
                    FieldsetAjustes.Visible = false;
                    botonAceptarAjustes.Visible = false;
                    botonCancelarAjustes.Visible = false;
                    tituloAccionAjustes.InnerText = "Seleccione un ajuste";
                    botonRealizarAjuste.Disabled = false;
                    botonConsultarAjustes.Disabled = true;
                    tituloGridProductos.Visible = false;
                    tituloGridConsulta.Visible = true;
                    gridViewAjustes.Enabled = true;
                    gridViewProductos.Enabled = false;
                    habilitarCampos(false);
                    break;

                case (int)Modo.Consultado://consultado, pero con los espacios bloqueados
                    botonAgregar.Disabled = true;
                    FieldsetAjustes.Visible = true;
                    botonAceptarAjustes.Visible = false;
                    botonCancelarAjustes.Visible = false;
                    tituloAccionAjustes.InnerText = "Ajuste seleccionado";
                    botonRealizarAjuste.Disabled = false;
                    botonConsultarAjustes.Disabled = true;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = true;
                    gridViewAjustes.Enabled = true;
                    gridViewProductos.Enabled = true;
                    habilitarCampos(false);
                    llenarGrid();
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
            dropdownTipo.SelectedValue = null;
            vaciarGridProductos();
        }

        /*
         * Habilita o desabilita los campos editables
         */
        protected void habilitarCampos(bool habilitar)
        {
            this.dropdownTipo.Enabled = habilitar;
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
         * Viaja a la base de datos y obtiene los datos de consulta
         */
        protected void llenarGrid()
        {
            // Por implementar
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


                DataTable productos = controladoraAjustes.consultarProductosDeBodega("Bodega Actual");

                if (productos.Rows.Count > 0)
                {
                    idArray = new Object[productos.Rows.Count];
                    foreach (DataRow fila in productos.Rows)
                    {
                        idArray[i] = fila[1];
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = Convert.ToInt32(fila[2].ToString());
                        datos[3] = Convert.ToInt32(fila[3].ToString());
                        datos[4] = Convert.ToInt32(fila[4].ToString());
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
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
            
        }

        /*
         * Crea una datatable en el formato del grid de consultas
         */
        protected DataTable tablaAjustes()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Tipo de Ajuste";
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
            columna.DataType = System.Type.GetType("System.Int32");
            columna.ColumnName = "Cantidad Actual";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Int32");
            columna.ColumnName = "Ajuste";
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
            columna.DataType = System.Type.GetType("System.Int32");
            columna.ColumnName = "Cantidad Actual";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Int32");
            columna.ColumnName = "Minimo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Int32");
            columna.ColumnName = "Maximo";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Esto pasa la interfaz al modo de crear ajustes.
         */
        protected void botonRealizarAjuste_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            llenarGridAgregarProductos();
            // Creo que falta cargar cosas
        }

        /*
         * Esto pasa la interfaz al modo de consulta.
         */
        protected void botonConsultarAjustes_ServerClick(object sender, EventArgs e)
        {
            llenarGrid();
            modo = (int)Modo.Consulta;
            cambiarModo();
        }

        /*
         * Limpia el grid de consulta
         */
        protected void vaciarGridAjustes()
        {
            DataTable tablaLimpia = null;
            gridViewAjustes.DataSource = tablaLimpia;
            gridViewAjustes.DataBind();
        }

        /*
         * Limpia el grid de productos
         */
        protected void vaciarGridProductos()
        {
            DataTable tablaLimpia = null;
            gridViewProductos.DataSource = tablaLimpia;
            gridViewProductos.DataBind();
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de consultar.
         */
        protected void gridViewAjustes_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            /*
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewAjustes.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * this.gridViewBodegas.PageSize)]);
                    consultarBodega(codigo);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormBodegas.aspx");
                    break;
            }*/
        }

        /*
         * Método que maneja el cambio de páginas en el grid de consultar
         */
        protected void gridViewAjustes_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            /*
            llenarGrid();
            this.gridViewAjustes.PageIndex = e.NewPageIndex;
            this.gridViewAjustes.DataBind();
            */
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de productos.
         */
        protected void gridViewProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            /*
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewAjustes.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * this.gridViewBodegas.PageSize)]);
                    consultarBodega(codigo);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormBodegas.aspx");
                    break;
            }*/
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
                    GridViewRow filaSeleccionada = this.gridViewAjustes.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * this.gridViewBodegas.PageSize)]);
                    consultarBodega(codigo);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormBodegas.aspx");
                    break;
            }*/
        }


        /*
         * Este método confirma inserción de ajustes.
         */
        protected void botonAceptarAjustes_ServerClick(object sender, EventArgs e)
        {

        }

        /*
         * Método que maneja la aceptar la cancelación.
         * Elimina datos y reinicia la interfaz.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            vaciarGridAjustes();
            modo = (int)Modo.Inicial;
            cambiarModo();
            limpiarCampos();
            //ajusteConsultado = null;
        }

        protected void gridViewAgregarProductos_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGridAgregarProductos();
            this.gridViewAgregarProductos.PageIndex = e.NewPageIndex;
            this.gridViewAgregarProductos.DataBind();
        }
    }
}