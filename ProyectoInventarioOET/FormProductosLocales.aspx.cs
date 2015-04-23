using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoInventarioOET
{
    public partial class FormProductosLocales : System.Web.UI.Page
    {

        private static int resultadosPorPagina;
        private static Object[] idArray;

        protected void Page_Load(object sender, EventArgs e)
        {
            testGrid();
        }

        protected void gridViewCatalogoLocal_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewCatalogoLocal.Rows[Convert.ToInt32(e.CommandArgument)];
                    int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCatalogoLocal.PageIndex * resultadosPorPagina)]);
                    break;
            }
        }

        protected void gridViewCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            this.gridViewCatalogoLocal.PageIndex = e.NewPageIndex;
            this.gridViewCatalogoLocal.DataBind();
        }
        protected void testGrid()
        {
            DataTable tabla = tablaBodegas();
            for (int i = 1; i < 20; i++)
            {
                Object[] datos2 = new Object[2];
                datos2[0] = i * 21;
                datos2[1] = i * 35;
                tabla.Rows.Add(datos2);
            }
            this.gridViewCatalogoLocal.DataSource = tabla;
            this.gridViewCatalogoLocal.DataBind();
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

    }
}