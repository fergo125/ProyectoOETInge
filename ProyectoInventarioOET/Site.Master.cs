using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Seguridad;

namespace ProyectoInventarioOET
{
    /* 
     * Clase SiteMaster, presente en toda página, contiene toda la página y presenta la barra de navegación junto con el pie de página.
     * Al ser omnipresente se le asocia el identificar al usuario que está conectado y usando el sistema, por lo que se relaciona
     * estrechamente con las labores de seguridad.
     */
    public partial class SiteMaster : MasterPage
    {
        //Atributos
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";          //Tokens usados para protección contra ataques XSRF
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";    //Tokens usados para protección contra ataques XSRF
        private string _antiXsrfTokenValue;                                 //Tokens usados para protección contra ataques XSRF
        private static EntidadUsuario usuarioLogueado;              //Instancia que almacena la información del usuario conectado

        /*
         * Código de inicialización, aparentemente se ejecuta sólo una vez.
         */
        protected void Page_Init(object sender, EventArgs e)
        {
            // El código siguiente ayuda a proteger frente a ataques XSRF
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Utilizar el token Anti-XSRF de la cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generar un nuevo token Anti-XSRF y guardarlo en la cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }
            Page.PreLoad += master_Page_PreLoad;
        }

        /*
         * Pre carga del SiteMaster, maneja los tokens.
         */
        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer token Anti-XSRF
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validar el token Anti-XSRF
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Error de validación del token Anti-XSRF.");
                }
            }
        }

        /*
         * Método Page_Load del SiteMaster, por ahora se encarga de mostrar los elementos correctos al usuario dependiendo de
         * si está conectado o no.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            if(usuarioLogueado != null)
            {
                this.linkNombreUsuarioLogueado.InnerText = usuarioLogueado.Nombre + " (" + usuarioLogueado.Perfil + ")";
                this.linkIniciarSesion.Visible = false;
                this.linkNombreUsuarioLogueado.Visible = true;
                this.linkCerrarSesion.Visible = true;
                //TODO poner los links como invisibles por default para que sólo puedan usarse después de iniciar sesión
                //TODO pesonalizar cuáles links serán visibles para cada perfil
                this.linkFormProductos.Visible = true;
                this.linkFormBodegas.Visible = true;
                this.linkFormInventario.Visible = true;
                this.linkFormVentas.Visible = true;
                this.linkFormAdministracion.Visible = true;
            }
        }

        /*
         * Método no usado.
         */
        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

        /*
         * Para simular el cierre de sesión esconde todos los elementos y borra el usuario que estaba conectado antes.
         * Muestra de nuevo el enlace para iniciar sesión.
         */
        protected void cerrarSesion(object sender, EventArgs e)
        {
            usuarioLogueado = null;
            this.linkIniciarSesion.Visible = true;
            this.linkNombreUsuarioLogueado.Visible = false;
            this.linkCerrarSesion.Visible = false;
            this.linkFormProductos.Visible = false;
            this.linkFormBodegas.Visible = false;
            this.linkFormInventario.Visible = false;
            this.linkFormVentas.Visible = false;
            this.linkFormAdministracion.Visible = false;
        }

        /*
         * Setter y getter para el atributo de usuarioLogueado.
         */
        public EntidadUsuario Usuario
        {
            get { return usuarioLogueado; }
            set { usuarioLogueado = value; }
        }
    }
}