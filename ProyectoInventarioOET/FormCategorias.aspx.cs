using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoInventarioOET
{
    public partial class FormCategorias : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion };
        private static int modo = (int)Modo.Inicial;
        private static int idCategoria = 0; //Sirve para estar en modo consulta
        private static int resultadosPorPagina;
        private static Object[] idArray;

        protected void Page_Load(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;

            testGrid();
        }
        protected void irAModo()
        {
            if (modo == (int)Modo.Consulta)
            { // el modo 0 se usa para resetear la interfaz
                botonAceptar.Disabled = true;
                botonCancelar.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = true;
                botonConsultaCategoria.Disabled = true;
                habilitarCampos(true);
            }
            else if (modo == (int)Modo.Modificacion)
            { // se desea insertar
                botonAceptar.Disabled = true;
                botonCancelar.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = true;
                botonConsultaCategoria.Disabled = true;

            }
            else if (modo == (int)Modo.Insercion)
            { //modificar
                botonAceptar.Disabled = false;
                botonCancelar.Disabled = false;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = true;
                botonConsultaCategoria.Disabled = false;
                habilitarCampos(false);
            }
            else if (modo == (int)Modo.Inicial)
            { // eliminar
                botonAceptar.Disabled = true;
                botonCancelar.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = false;
                habilitarCampos(true);
            }

            //aplicarPermisos();// se aplican los permisos del usuario para el acceso a funcionalidades
        }
        protected void testGrid()
        {

            DataTable tabla = tablaBodegas();
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

        protected DataTable tablaBodegas()
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
        protected void habilitarCampos(bool cambio)
        {
            this.inputNombre.Disabled = cambio;
        }
        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
            deshabilitarCampos();
        }

    }
}