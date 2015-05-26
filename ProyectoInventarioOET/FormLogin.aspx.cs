using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;

namespace ProyectoInventarioOET
{
    /*
     * Código de funcionamiento de la interfaz de login.
     * Controla las operaciones en la interfaz y comunica con la controladora de seguridad.
     */
    public partial class FormLogin : System.Web.UI.Page
    {
        //Atributos
        static ControladoraSeguridad controladora; // Controladora para obtener datos de usuario

        /*
         * Maneja las acciones que se ejecutan cuando se carga la página
         * En este caso, unicamente crea la controladora
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                controladora = new ControladoraSeguridad();
            }
        }

        /*
         * Maneja el trámite de inicio de sesión una vez el usuario introduce sus datos
         */
        protected void iniciarSesion(object sender, EventArgs e)
        {
            // Consulta al usuario
            EntidadUsuario usuario = controladora.consultarUsuario(UserName.Text, Password.Text);

            if(usuario != null)
            {
                // Si me retorna un usuario valido

                // Hacer el usuario logueado visible a todas los modulos
                (this.Master as SiteMaster).Usuario = usuario;
                // Redirigir a pagina de seleccion de bodega
                Response.Redirect("FormLoginBodega.aspx");
            }
            else
            {
                // Si no me retorna un usuario valido, advertir
                mostrarMensaje();
            }
        }

        /*
         * Muestra un mensaje de advertencia, en caso de usuario/contraseña incorrectos
         */
        protected void mostrarMensaje()
        {
            mensajeAlerta.Attributes["class"] = "alert alert-danger alert-dismissable fade in";
            labelTipoAlerta.Text = "Error:";
            labelAlerta.Text = "Nombre de usuario o contraseña inválidos";
            mensajeAlerta.Visible = true;
        }
    }
}