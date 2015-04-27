using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Seguridad;

namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormLogin : System.Web.UI.Page
    {
        //Atributos
        static ControladoraSeguridad controladora;

        /*
         * ???
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                controladora = new ControladoraSeguridad();
            }
        }

        /*
         * ???
         */
        protected void iniciarSesion(object sender, EventArgs e)
        {
            EntidadUsuario usuario = controladora.consultarUsuario(UserName.Text, Password.Text);
            if(usuario != null)
            {
                // Asignar a global
                (this.Master as SiteMaster).Usuario = usuario;
                // Redirigir a pagina principal
                Response.Redirect("Default.aspx");
            }
            else
            {
                // Mostrar mensaje
                mostrarMensaje();
            }
        }

        /*
         * ???
         */
        protected void mostrarMensaje()
        {
            mensajeAlerta.Attributes["class"] = "alert alert-danger alert-dismissable fade in";
            labelTipoAlerta.Text = "Error";
            labelAlerta.Text = "Nombre de usuario o contraseña inválidos";
            mensajeAlerta.Visible = true;
        }
    }
}