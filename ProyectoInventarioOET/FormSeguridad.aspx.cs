using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoInventarioOET
{
    public partial class FormSeguridad : System.Web.UI.Page
    {

        enum Modo { Inicial, InicialPerfil, InicialUsuario, ConsultaPerfil, InsercionPerfil, ModificacionPerfil, ConsultaUsuario, InsercionUsuario, AsociarUsuario };
        // Atributos
        private static int modo = 0;                    // Modo actual de la pagina

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                modo = (int)Modo.Inicial;
                cambiarModo();
            }
        }

         /*
         * Realiza los cambios de modo que determinan que se puede ver y que no.
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial:
                    FieldsetBotonesPerfiles.Visible = false;
                    FieldsetBotonesUsuarios.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    break;
                case (int)Modo.InicialPerfil:
                    FieldsetBotonesPerfiles.Visible = true;
                    FieldsetBotonesUsuarios.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    break;
                case (int)Modo.InicialUsuario:
                    FieldsetBotonesUsuarios.Visible = true;
                    FieldsetBotonesPerfiles.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    break;


                case (int)Modo.InsercionUsuario:
                    FieldsetUsuario.Visible = true;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = true;
                    break;
                case (int)Modo.AsociarUsuario:
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = true;
                    FieldsetBotones.Visible = true;
                    break;
            }
        }

        // Seleccion de administracion de perfiles
        protected void botonPerfiles_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.InicialPerfil;
            cambiarModo();
        }

        // Seleccion de administracion de usuarios
        protected void botonUsuarios_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.InicialUsuario;
            cambiarModo();
        }

        // Seleccion de asociacion de usuarios a perfil
        protected void botonAsociarPerfil_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.AsociarUsuario;
            cambiarModo();
        }

        // Confirmación del modal de cancelación
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        // Crear usuario
        protected void botonCrearUsuario_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.InsercionUsuario;
            cambiarModo();
        }
    }
}