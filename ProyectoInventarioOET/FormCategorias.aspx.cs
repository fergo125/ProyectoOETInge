using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Categorias;
using ProyectoInventarioOET.App_Code;

namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormCategorias : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static int modo = (int)Modo.Inicial;                    //???
        private static int resultadosPorPagina; //wtf?
        private static Object[][] idArray;                              //???
        private static ControladoraCategorias controladoraCategorias;   //???
        private static EntidadCategoria categoriaConsultada;            //???
        private static bool seConsulto = false;                         //???
        private static ControladoraDatosGenerales controladoraDatosGenerales;
        /*
         * ???
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                controladoraCategorias = new ControladoraCategorias();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                if (!seConsulto)
                {
                    modo = 0;
                }
                else
                {
                    if (categoriaConsultada == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la categoria.");
                    }
                    else
                    {
                        cargarEstados();
                        setDatosConsultados();
                        llenarGrid();
                        seConsulto = false;
                    }
                }
            }
            irAModo();
        }

        /*
         * ???
         */
        protected void irAModo()
        {
            if (modo == (int)Modo.Consulta)
            { // el modo 0 se usa para resetear la interfaz
                bloqueBotones.Visible = false;
                bloqueBotones.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = true;
                camposCategoria.Visible = false;
                gridViewCategorias.Visible = true;
                comboBoxEstadosActividades.Enabled = false;
            }
            else if (modo == (int)Modo.Modificacion)
            { // se desea insertar
                bloqueBotones.Visible = true;
                bloqueBotones.Disabled = false;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = true;
                camposCategoria.Visible = true;
                gridViewCategorias.Visible = false;
                comboBoxEstadosActividades.Enabled = false;
            }
            else if (modo == (int)Modo.Insercion)
            { //modificar
                bloqueBotones.Visible = true;
                bloqueBotones.Disabled = false;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = false;
                camposCategoria.Visible = true;
                gridViewCategorias.Visible = false;
                comboBoxEstadosActividades.Enabled = true;
                cargarEstados();
            }
            else if (modo == (int)Modo.Inicial)
            { // eliminar
                bloqueBotones.Visible = false;
                bloqueBotones.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = false;
                camposCategoria.Visible = false;
                gridViewCategorias.Visible = false;
                comboBoxEstadosActividades.Enabled = false;
            }
            else if (modo == (int)Modo.Consultado)
            { // eliminar
                bloqueBotones.Visible = false;
                bloqueBotones.Disabled = true;
                botonModificacionCategoria.Disabled = false;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = true;
                camposCategoria.Visible = true;
                camposCategoria.Disabled = true;
                inputNombre.Disabled = true;
                gridViewCategorias.Visible = true;
                comboBoxEstadosActividades.Enabled = false;
            }
            //aplicarPermisos();// se aplican los permisos del usuario para el acceso a funcionalidades
        }

        /*
         * ???
         */
        protected DataTable tablaCategorias()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            return tabla;
        }
        protected void cargarEstados()
        {
            comboBoxEstadosActividades.Items.Clear();
            comboBoxEstadosActividades.Items.Add(new ListItem("", null));
            DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
            foreach (DataRow fila in estados.Rows)
            {
                comboBoxEstadosActividades.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }
        /*
         * ???
         */
        protected void botonRedireccionProductos_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("FormProductos.aspx");
        }

        /*
         * ???
         */
        protected void gridViewCategorias_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewCategorias.Rows[Convert.ToInt32(e.CommandArgument)];
                    Object[] entidad = new Object[3];
                    entidad[0] = idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCategorias.PageIndex * resultadosPorPagina)][0];
                    entidad[1] = idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCategorias.PageIndex * resultadosPorPagina)][1];
                    entidad[2] = idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCategorias.PageIndex * resultadosPorPagina)][2];
                    categoriaConsultada = new EntidadCategoria(entidad);
                    seConsulto = true;
                    modo = 4;
                    Response.Redirect("FormCategorias.aspx");
                    break;
            }
        }

        /*
         * ???
         */
        protected void gridViewCategorias_CambioPagina(object sender, GridViewPageEventArgs e)
        {

        }

        /*
         * ???
         */
        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
        }

        /*
         * ???
         */
        protected void deshabilitarCampos()
        {
            this.inputNombre.Disabled = true;
        }
        /*protected void habilitarCampos(bool cambio)
        {
            this.inputNombre.Disabled = cambio;
        }*/

        /*
         * ???
         */
        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
            deshabilitarCampos();
        }

        /*
         * ???
         */
        protected void botonAgregarCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            irAModo();
        }

        /*
         * ???
         */
        protected void botonModificacionCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            irAModo();
        }

        /*
         * ???
         */
        protected void botonConsultaCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Consulta;
            llenarGrid();
            irAModo();
        }

        /*
         * ???
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        /*
         * ???
         */
        protected void setDatosConsultados()
        {
            this.inputNombre.Value = categoriaConsultada.Descripcion;
            this.comboBoxEstadosActividades.SelectedValue = Convert.ToString(categoriaConsultada.Estado);
        }

        /*
         * ???
         */
        protected void llenarGrid()
        {
            DataTable tabla = tablaCategorias();
            int indiceNuevaCategoria = -1;
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[3];
                DataTable categorias = controladoraCategorias.consultarCategorias();

                if (categorias.Rows.Count > 0)
                {
                    idArray = new Object[categorias.Rows.Count][];
                    foreach (DataRow fila in categorias.Rows)
                    {
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = fila[2];
                        Object[] datos2 = new Object[2] { datos[1], Convert.ToInt16(datos[2].ToString()) == 1 ? "Activo" : "inactivo" };
                        tabla.Rows.Add(datos2);
                        idArray[i] = datos;

                        if (categoriaConsultada != null && (fila[0].Equals(categoriaConsultada.Nombre)))
                        {
                            indiceNuevaCategoria = i;
                        }
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridViewCategorias.DataSource = tabla;
                this.gridViewCategorias.DataBind();
                if (categoriaConsultada != null)
                {
                    GridViewRow filaSeleccionada = this.gridViewCategorias.Rows[indiceNuevaCategoria];
                }
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * ???
         */
        protected void botonAceptarCategoria_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";

            if (modo == 2)
            {
                codigoInsertado = insertar();

                if (codigoInsertado != "")
                {
                    operacionCorrecta = true;
                    categoriaConsultada = controladoraCategorias.consultarCategoria(codigoInsertado);
                    modo = (int)Modo.Consultado;
                    habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == 3)
            {
                operacionCorrecta = modificar();
            }
            if (operacionCorrecta)
            {
                irAModo();
            }
        }

        /*
         * ???
         */
        private bool modificar()
        {
            bool res = true;
            String[] datosCat = new String[3]{inputNombre.Value,categoriaConsultada.Nombre,"1"};
            String[] error = controladoraCategorias.modificarDatos(categoriaConsultada, datosCat);
            mostrarMensaje(error[0], error[1], error[2]);

            if (error[0].Contains("success"))// si fue exitoso
            {
                llenarGrid();
                categoriaConsultada = controladoraCategorias.consultarCategoria(categoriaConsultada.Nombre);
                modo = (int)Modo.Consulta;
            }
            else
            {
                res = false;
                modo = (int)Modo.Modificacion;
            }
            return res;
            
        }

        /*
         * ???
         */
        private void habilitarCampos(bool p)
        {
            camposCategoria.Disabled = !p;
        }

        /*
         * ???
         */
        private string insertar()
        {

            String codigo = "";

            String[] error = controladoraCategorias.insertarDatos(inputNombre.Value.ToString());

            codigo = Convert.ToString(error[3]);
            mostrarMensaje(error[0], error[1], error[2]);
            if (error[0].Contains("success"))
            {
                llenarGrid();
            }
            else
            {
                codigo = "";
                modo = (int)Modo.Insercion;
            }

            return codigo;
        }
    }
}