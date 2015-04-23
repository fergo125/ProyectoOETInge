using System;
using System.Collections.Generic;
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


    }
}