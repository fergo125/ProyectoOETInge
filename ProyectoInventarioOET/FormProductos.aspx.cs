using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoInventarioOET
{
    public partial class FormProductos : System.Web.UI.Page
    {
        private static int resultadosPorPagina;
        private static Object[] idArray;

        protected void Page_Load(object sender, EventArgs e)
        {
            testGrid();
        }

        protected void gridViewBodegas_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewBodegas.Rows[Convert.ToInt32(e.CommandArgument)];
                    int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * resultadosPorPagina)]);
                    break;
            }
        }

        protected void gridViewBodegas_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            this.gridViewBodegas.PageIndex = e.NewPageIndex;
            this.gridViewBodegas.DataBind();
        }

        protected void testGrid()
        {

            DataTable tabla = tablaBodegas();
            DataTable tabla2 = tablaCatalogoLocal();

            for (int i = 1; i < 5; i++)
            {
                Object[] datos = new Object[3];
                datos[0] = i * 2;
                datos[1] = i * 3;
                datos[2] = i * 4;
                tabla.Rows.Add(datos);
            }

            for (int i = 1; i < 5; i++)
            {
                Object[] datos2 = new Object[5];
                datos2[0] = i * 2;
                datos2[1] = i * 3;
                datos2[2] = i * 4;
                datos2[3] = i * 5;
                datos2[4] = i * 6;
                tabla2.Rows.Add(datos2);
            }

            this.gridViewBodegas.DataSource = tabla;
            this.gridViewBodegas.DataBind();

        }


        protected void llenarGrid()
        {
            DataTable tabla = tablaBodegas();
            int indiceNuevaBodega = -1;
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

                this.gridViewBodegas.DataSource = tabla;
                this.gridViewBodegas.DataBind();
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

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estación";
            tabla.Columns.Add(columna);

            return tabla;
        }

        protected DataTable tablaCatalogoLocal()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Precio";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Cantidad";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Mínimo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Máximo";
            tabla.Columns.Add(columna);

            return tabla;

            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster

        }

        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Attributes.Remove("hidden");
        }

        protected void gridViewCatalogoLocal_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewBodegas.Rows[Convert.ToInt32(e.CommandArgument)];
                    int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * resultadosPorPagina)]);
                    break;
            }
        }

        protected void gridViewCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
        }

        protected void botonRedireccionCategorias_ServerClick(object sender, EventArgs e)
        {
            //Server.Transfer("FormCategorias.aspx");
            Response.Redirect("FormCategorias.aspx");
        }
    }
}