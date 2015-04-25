using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Categorias;

namespace ProyectoInventarioOET
{
    public partial class FormCategorias : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        private static int modo = (int)Modo.Inicial;
        private static int idCategoria = 0; //Sirve para estar en modo consulta
        private static int resultadosPorPagina;
        private static Object[] idArray;
        private static ControladoraCategorias controladoraCategorias;
        private static EntidadCategoria categoriaConsultada;
        private static bool seConsulto = false;

        protected void Page_Load(object sender, EventArgs e)
        {
           if (!IsPostBack)
            {
                
                    controladoraCategorias = new ControladoraCategorias();

                    if (!seConsulto)
                    {
                        modo = 0;
                    }
                    else{
                        if (categoriaConsultada == null)
                        {
                            mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la categoria.");
                        }
                        else
                        {
                            setDatosConsultados();

                            seConsulto = false;
                        }
                    }
            }
           irAModo();
        }
        protected void irAModo()
        {
            if (modo == (int)Modo.Consulta)
            { // el modo 0 se usa para resetear la interfaz
                bloqueBotones.Visible = true;
                bloqueBotones.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = true;
                camposCategoria.Visible = false;
                gridViewCategorias.Visible = true;
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
            }

            //aplicarPermisos();// se aplican los permisos del usuario para el acceso a funcionalidades
        }
        protected void testGrid()
        {

            DataTable tabla = tablaCategorias();
            for (int i = 1; i < 5; i++)
            {
                Object[] datos2 = new Object[2];
                datos2[0] = i * 2;
                datos2[1] = i * 3;
                tabla.Rows.Add(datos2);
            }

            this.gridViewCategorias.DataSource = tabla;
            this.gridViewCategorias.DataBind();

        }

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
            columna.ColumnName = "Descripción";
            tabla.Columns.Add(columna);

            return tabla;
        }

        protected void botonRedireccionProductos_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("FormProductos.aspx");
        }

        protected void gridViewCategorias_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewCategorias.Rows[Convert.ToInt32(e.CommandArgument)];
                    int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCategorias.PageIndex * resultadosPorPagina)]);
                    break;
            }
        }

        protected void gridViewCategorias_CambioPagina(object sender, GridViewPageEventArgs e)
        {

        }
        
        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
        }
        protected void deshabilitarCampos()
        {
            this.inputNombre.Disabled = true;
        }
        /*protected void habilitarCampos(bool cambio)
        {
            this.inputNombre.Disabled = cambio;
        }*/
        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
            deshabilitarCampos();
        }

        protected void botonAgregarCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            irAModo();
        }

        protected void botonModificacionCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            irAModo();
        }

        protected void botonConsultaCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Consulta;
            testGrid();
            irAModo();
        }

        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible= true;
        }
        protected void setDatosConsultados()
        {
            this.inputNombre.Value = categoriaConsultada.Descripcion;
        }
        protected void llenarGrid()
        {
            DataTable tabla = tablaCategorias();
            int indiceNuevaBodega = -1;
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[2];
                DataTable bodegas = controladoraCategorias.consultarCategorias();

                if (bodegas.Rows.Count > 0)
                {
                    idArray = new Object[bodegas.Rows.Count];
                    foreach (DataRow fila in bodegas.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[3].ToString();
                        tabla.Rows.Add(datos);
                        if (categoriaConsultada != null && (fila[0].Equals(categoriaConsultada.Nombre)))
                        {
                            indiceNuevaBodega = i;
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
                    GridViewRow filaSeleccionada = this.gridViewCategorias.Rows[indiceNuevaBodega];
                }
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }
    }

}