using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;

namespace ProyectoInventarioOET
{
    public partial class FormSeguridad : System.Web.UI.Page
    {

        enum Modo { Inicial, InicialPerfil, InicialUsuario, ConsultaPerfil, InsercionPerfil, ModificacionPerfil, ConsultaUsuario, InsercionUsuario, AsociarUsuario };
        ControladoraSeguridad controladora;
        EntidadUsuario entidadConsultada;
        // Atributos
        private static int modo = 0;                    // Modo actual de la pagina

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*EJEMPLO PARA EL BLOPA YO HICE DOS METODS COOOOOORRERRRRRRLO!
                 *  i) consultarCuentas que trae informacion previa(idUsuario, Nombre, Perfil, Estado) de todas las cuentas que existen en el sistema 
                 *  ii) consultarCuenta(String id) que devuelve la Entidad de Usuario con todas las cosas que existen en la BD de un usuario especifico, esta entidad contiene
                 *       una matriz  de permisos que despliega los permisos de dicho usuario en cada interfaz
                 */
                controladora = new ControladoraSeguridad();
                entidadConsultada = controladora.consultarCuenta("3"); //Recibe el id (Seg_usuario)
                this.gridPermisos.DataSource = entidadConsultada.MatrizPermisos;
                this.gridPermisos.DataBind();  //IMPORTANTE!! ASI VIENEN LOS PERMISOS DE UN USUARIO
                this.inputNombre.Value = entidadConsultada.Nombre;
                this.gridCuentas.DataSource = controladora.consultarCuentas(); // Recordadr no desplegar el idUsuario!! Yo lo hice xq era un ejemplo de que funka 
                this.gridCuentas.DataBind();


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
                    FieldsetGrid.Visible = false;
                    break;
                case (int)Modo.InicialPerfil:
                    FieldsetBotonesPerfiles.Visible = true;
                    FieldsetBotonesUsuarios.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetGrid.Visible = false;
                    break;
                case (int)Modo.InicialUsuario:
                    FieldsetBotonesUsuarios.Visible = true;
                    FieldsetBotonesPerfiles.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetGrid.Visible = false;
                    break;

                case (int)Modo.ConsultaUsuario:
                    FieldsetUsuario.Visible = false;
                    FieldsetGrid.Visible = true;
                    FieldsetBotones.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    break;
                case (int)Modo.InsercionUsuario:
                    FieldsetUsuario.Visible = true;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = true;
                    FieldsetGrid.Visible = false;
                    break;
                case (int)Modo.AsociarUsuario:
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = true;
                    FieldsetBotones.Visible = true;
                    FieldsetGrid.Visible = false;
                    break;
            }
        }

        /*
         * Llena el árbol de permisos, con ningún permiso si es para creación, o con los permisos de un perfil si es para modificación.
         * Para determinar la situación, se fija si es nulo o no el parámetro que recibe.
         * TODO: que reciba una EntidadPerfil como parámetro
         */
        protected void llenarArbol()
        {
            //Agregar los permisos para cada subinterfaz
            TreeNode raiz = new TreeNode();
            raiz.Text = "Interfaces";
            raiz.Value = "Raiz";
            for(short i=0; i<5; ++i)
            {
                TreeNode interfaz = new TreeNode();
                switch(i)
                {
                    case 1:
                        interfaz.Text = "Productos";
                        interfaz.Value = "Pro";
                        break;
                    case 2:
                        interfaz.Text = "Bodegas";
                        interfaz.Value = "Bod";
                        break;
                    case 3:
                        interfaz.Text = "Inventario";
                        interfaz.Value = "Inv";
                        break;
                    case 4:
                        interfaz.Text = "Ventas";
                        interfaz.Value = "Ven";
                        break;
                    case 5:
                        interfaz.Text = "Administración";
                        interfaz.Value = "Adm";
                        break;
                    default:
                        break;
                }
                raiz.ChildNodes.Add(interfaz);
            }
            TreeNode subinterfaz = new TreeNode();
            subinterfaz.Text = "Interfaces";
            subinterfaz.Value = "Root";
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

        // Consulta los usuarios
        protected void botonConsultarUsuario_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.ConsultaUsuario;
            cambiarModo();
        }
    }
}