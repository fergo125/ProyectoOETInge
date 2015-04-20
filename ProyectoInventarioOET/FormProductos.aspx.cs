using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoInventarioOET
{
    public partial class FormProductos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true);
        }

        protected void botonAgregarProductos_ServerClick(object sender, EventArgs e)
        {

        }

        protected void botonRedireccionCategorias_ServerClick(object sender, EventArgs e)
        {
            //Server.Transfer("FormCategorias.aspx");
            Response.Redirect("FormCategorias.aspx");
        }
    }
}