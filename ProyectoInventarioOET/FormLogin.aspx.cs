using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Seguridad;

namespace ProyectoInventarioOET
{
    public partial class FormLogin : System.Web.UI.Page
    {
        static ControladoraSeguridad controladora;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                controladora = new ControladoraSeguridad();
            }
        }

        protected void iniciarSesion(object sender, EventArgs e)
        {
            EntidadUsuario usuario = controladora.consultarUsuario(UserName.Text, Password.Text);
        }
    }
}