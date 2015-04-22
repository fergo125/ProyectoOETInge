using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoInventarioOET
{
    public partial class FormCategorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void botonRedireccionProductos_ServerClick(object sender, EventArgs e)
        {
            //Server.Transfer("FormProductos.aspx");
            Response.Redirect("FormProductos.aspx");
        }

        protected void gridViewCategorias_Seleccion(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gridViewCategorias_CambioPagina(object sender, GridViewPageEventArgs e)
        {

        }

        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {

        }


    }
}