using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

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

        // SOLO PARA PRUEBAS
        protected void testGrid()
        {
            DataTable tabla2 = tablaCatalogoLocal();
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
            this.gridViewCatalogoLocal.DataSource = tabla2;
            this.gridViewCatalogoLocal.DataBind();

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

        // Seleccion de bodega, aqui deberia cargarse la nueva bodega a la tabla
        protected void DropDownListBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        // protocol epsilon chi tau

    }
}