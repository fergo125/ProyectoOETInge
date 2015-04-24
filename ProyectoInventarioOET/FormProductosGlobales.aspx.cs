using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales;
using ProyectoInventarioOET.App_Code;


namespace ProyectoInventarioOET
{
    public partial class FormProductosGlobales : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        private static int modo = (int) Modo.Inicial;
        private static int idProducto = 0; //Sirve para estar en modo consulta
        private static int resultadosPorPagina;
        private static Object[] idArray;
        private static ControladoraDatosGenerales controladoraDatosGenerales;
        private static ControladoraProductosGlobales controladora; 

        protected void Page_Load(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
        }


        protected void cargarEstaciones()
        {
            inputEstacion.Items.Clear();
            inputEstacion.Items.Add(new ListItem("", null));
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            foreach (DataRow fila in estaciones.Rows)
            {
                inputEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }


        protected void cargarEstados()
        {
            inputEstado.Items.Clear();
            inputEstado.Items.Add(new ListItem("", null));
            DataTable estados = controladoraDatosGenerales.consultarEstados();
            foreach (DataRow fila in estados.Rows)
            {
                inputEstado.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }

        protected DataTable tablaProductosGlobales()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Costo en colones";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Costo en dolares";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estacion";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            return tabla;

            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster

        }
        protected void llenarGrid()
        {
            DataTable tabla = tablaProductosGlobales();
            int indiceNuevoProducto = -1;
            int i = 0;

            try
            {
                // Cargar proyectos
                Object[] datos = new Object[3];
                DataTable bodegas = new DataTable(); //quitar una vez que ya está la controladora
                //bodegas = controladora.consultarBodegas();

                if (bodegas.Rows.Count > 0)
                {
                    idArray = new Object[bodegas.Rows.Count];
                    foreach (DataRow fila in bodegas.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[6].ToString();
                        datos[2] = fila[7].ToString();
                        tabla.Rows.Add(datos);
                        /* if (bodegaConsultada != null && (fila[0].Equals(bodegaConsultada.Identificador)))
                         {
                             indiceNuevaBodega = i;
                         }*/
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridViewProductosGlobales.DataSource = tabla;
                this.gridViewProductosGlobales.DataBind();
                /* if (bodegaConsultada != null)
                 {
                     GridViewRow filaSeleccionada = this.gridViewProyecto.Rows[indiceNuevoProyecto];
                 }*/
            }

            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        


        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Attributes.Remove("hidden");
        }


        protected void cambiarModo()
        {
            switch (modo)
            {///Probar si aun se pueden mostrar los campos con el JS********************
                case (int)Modo.Inicial:
                    limpiarCampos();
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = true;
                    habilitarCampos(false);
                    this.gridViewProductosGlobales.Visible = false;///********************
                    this.botonAceptarProducto.Visible = false;///******************
                    this.botonCancelarProducto.Visible = false;///******************          
                    //this. =controladoraDatosGenerales.consultarEstaciones;                              ///

                    break;
                case (int)Modo.Insercion: //insertar
                    habilitarCampos(true);
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    //this.gridViewActividades.Visible = false;///********************
                    break;
                case (int)Modo.Modificacion: //modificar
                    habilitarCampos(true);
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    //this.gridViewActividades.Visible = false;///********************
                    break;
                case (int)Modo.Consulta://consultar
                    limpiarCampos();
                    habilitarCampos(false);
                    this.botonAceptarProducto.Visible = false;///******************
                    this.botonCancelarProducto.Visible = false;///******************
                    this.botonModificacionProductos.Disabled = true;//**********************
                    //this.gridViewActividades.Visible = true;///********************
                    break;
                case (int)Modo.Consultado://consultada una actividad
                    habilitarCampos(false);
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = false;
                    this.botonAceptarProducto.Visible = false;///******************
                    this.botonCancelarProducto.Visible = false;///****************** 
                    //this.gridViewActividades.Visible = false;///********************///

                    break;
                default:

                    break;
            }
        }


        /* METODOS DE INTERFAZ RUTINARIOS
         * Limpiar pantalla, Habilitar campos          */

        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
            this.inputCodigo.Value = "";
            this.inputCodigoBarras.Value = "";
        }

        protected void habilitarCampos(bool resp)
        {
            this.inputNombre.Disabled = !resp;
            this.inputCodigo.Disabled = !resp;
            this.inputCodigoBarras.Disabled = !resp;
            this.inputUnidades.Enabled = resp;
            this.inpuCategoria.Enabled = resp;
            this.inputEstado.Enabled = resp;
        }


        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        protected void botonAgregarProductos_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            //cargarEstados();
        }

        protected void botonModificacionProductos_ServerClick(object sender, EventArgs e)
        {

        }

        protected void botonConsultaProductos_ServerClick(object sender, EventArgs e)
        {

        }

        protected void gridViewProductosGlobales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewProductosGlobales.Rows[Convert.ToInt32(e.CommandArgument)];
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewProductosGlobales.PageIndex * resultadosPorPagina)]);
                    //consultarActividad(codigo);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormActividades.aspx");
                    break;
            }
        }

        protected void gridViewProductosGlobales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gridViewProductosGlobales.PageIndex = e.NewPageIndex;
            this.gridViewProductosGlobales.DataBind();
        }



    }
}