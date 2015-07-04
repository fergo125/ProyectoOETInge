using System;
using System.Collections.Generic;
//using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;

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
        private static EntidadUsuario usuarioLogueado;                      //Instancia que almacena la información del usuario conectado
        private static String llaveBodegaSesion;                            //Llave de la bodega local utilizada en la sesion
        private static String nombreBodegaSesion;                           //Nombre de la bodega local utilizada en la sesion

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
                this.linkNombreUsuarioLogueado.InnerText = usuarioLogueado.Nombre + " (" + usuarioLogueado.Perfil + ") " + nombreBodegaSesion;
                this.linkIniciarSesion.Visible = false;
                this.linkNombreUsuarioLogueado.Visible = true;
                esconderLinks(llaveBodegaSesion == null); //Sólo si ya inició sesión y ya escogió bodega se muestran los links para las partes del sistema
            }
        }

        /*
         * Setter y getter para el atributo de usuarioLogueado.
         */
        public EntidadUsuario Usuario
        {
            get { return usuarioLogueado; }
            set { usuarioLogueado = value; }
        }

        /*
         * Setter y getter para el atributo llaveBodegaSesion
         */
        public String LlaveBodegaSesion
        {
            get { return llaveBodegaSesion; }
            set { llaveBodegaSesion = value; }
        }
        /*
         * Setter y getter para el atributo nombreBodegaSesion
         */
        public String NombreBodegaSesion
        {
            get { return nombreBodegaSesion; }
            set { nombreBodegaSesion = value; }
        }

        /*
         * Para simular el cierre de sesión esconde todos los elementos y borra el usuario que estaba conectado antes.
         * Muestra de nuevo el enlace para iniciar sesión.
         */
        public void cerrarSesion(object sender, EventArgs e)
        {
            usuarioLogueado = null;
            llaveBodegaSesion = null;
            nombreBodegaSesion = null;
            esconderLinks(true);
            Response.Redirect("Default.aspx");
        }

        /*
         * ???
         */
        public void cambiarSesion(String nombre, String perfil, String nombreBodegaSesion)
        {
            this.linkNombreUsuarioLogueado.InnerText = nombre + " (" + perfil + ") " + nombreBodegaSesion;
        }


        //Importante:
        //Para el codigoPerfilUsuario (que se usa un poco hard-coded), los números son:
        //1. Administrador global
        //2. Administrador local
        //3. Supervisor
        //4. Vendedor

        /*
         * Usado cuando se inicia o cierra sesión, al iniciar sesión vuelve a todos los links visibles excepto al de iniciar sesión,
         * al cerrar sesión los esconde, excepto el de iniciar sesión, el cual muestra.
         */
        protected void esconderLinks(bool esconder)
        {
            this.linkIniciarSesion.Visible = esconder;
            this.linkNombreUsuarioLogueado.Visible = !esconder;
            //this.linkCambiarSesion.Visible = !esconder;

            //TODO arreglar esto para que no sea hard coded***
            this.linkFormProductos.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2"));
                this.linkFormProductos1.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2"));
                this.linkFormProductos2.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2"));
            this.linkFormBodegas.Visible = !esconder;
                this.linkFormBodegas1.Visible = !esconder;
                this.linkFormBodegas2.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2"));
                this.linkFormBodegas3.Visible = !esconder;
            this.linkFormInventario.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2" || usuarioLogueado.CodigoPerfil == "3"));
                this.linkFormInventario1.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2" || usuarioLogueado.CodigoPerfil == "3"));
                this.linkFormInventario2.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2" || usuarioLogueado.CodigoPerfil == "3"));
                this.linkFormInventario3.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2" || usuarioLogueado.CodigoPerfil == "3"));
            this.linkFormVentas.Visible = !esconder;
            this.linkFormAdministracion.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2" || usuarioLogueado.CodigoPerfil == "3"));
                this.linkFormAdministracion1.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1" || usuarioLogueado.CodigoPerfil == "2" || usuarioLogueado.CodigoPerfil == "3"));
                this.linkFormAdministracion2.Visible = (!esconder && (usuarioLogueado.CodigoPerfil == "1"));
            this.linkFormAbout.Visible = (!esconder);
        }

        /*
         * Método para obtener permisos del usuario logueado
         * Se hace para tener un único punto de acceso a esto desde cada Interfaz
         */
        public String obtenerPermisosUsuarioLogueado(String nombreInterfaz)
        {
            ControladoraSeguridad cs = new ControladoraSeguridad();
            if (usuarioLogueado != null)
                return cs.consultarPermisosUsuario(usuarioLogueado.LlavePerfil, nombreInterfaz);
            else
                return "000000";
        }



        /*
         * Método no usado.
         */
        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }
    }
}