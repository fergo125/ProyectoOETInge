using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.App_Code;

namespace ProyectoInventarioOET
{
    public partial class FormSeguridad : System.Web.UI.Page
    {

        enum Modo { Inicial, InicialPerfil, InicialUsuario, ConsultaPerfil, InsercionPerfil, ModificacionPerfil, ConsultaUsuario, InsercionUsuario, AsociarUsuario, ConsultadoUsuario };
       // Atributos
        private static int modo = (int)Modo.Inicial;                    // Modo actual de la pagina
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static ControladoraSeguridad controladoraSeguridad;
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //Controladora para consultar las estaciones
        private static EntidadUsuario usuarioConsultado;                        //Entidad que almacena la cuenta consultada
        private static Boolean seConsulto = false;                              //Bandera que revisa si ya se consulto o no                       //???
        private static Object[] idArray;                                //Array de ids para almacenar los usuarios
        
        protected void Page_Load(object sender, EventArgs e)
        {
                mensajeAlerta.Visible = false;
            
                ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
                labelAlerta.Text = "";

                if (!IsPostBack)
                {
                    controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                    controladoraSeguridad = new ControladoraSeguridad();
                  //Seguridad
                    permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Gestion de bodegas");
                    if (permisos == "000000")
                        Response.Redirect("~/ErrorPages/404.html");
                    //mostrarBotonesSegunPermisos();

          
                    /*EJEMPLO PARA EL BLOPA YO HICE DOS METODS COOOOOORRERRRRRRLO!
                     *  i) consultarCuentas que trae informacion previa(idUsuario, Nombre, Perfil, Estado) de todas las cuentas que existen en el sistema 
                     *  ii) consultarCuenta(String id) que devuelve la Entidad de Usuario con todas las cosas que existen en la BD de un usuario especifico, esta entidad contiene
                     *       una matriz  de permisos que despliega los permisos de dicho usuario en cada interfaz
                     */
                    /*controladoraSeguridad = new ControladoraSeguridad();
                    entidadConsultada = controladoraSeguridad.consultarCuenta("3"); //Recibe el id (Seg_usuario)
                    this.gridPermisos.DataSource = entidadConsultada.MatrizPermisos;
                    this.gridPermisos.DataBind();  //IMPORTANTE!! ASI VIENEN LOS PERMISOS DE UN USUARIO
                    this.inputNombre.Value = entidadConsultada.Nombre;
                    this.gridCuentas.DataSource = controladoraSeguridad.consultarCuentas(); // Recordadr no desplegar el idUsuario!! Yo lo hice xq era un ejemplo de que funka 
                    this.gridCuentas.DataBind();*/
                     if (!seConsulto)
                    {
                        modo = (int)Modo.Inicial;
                    }
                    else
                    {
                        if (usuarioConsultado == null)
                        {
                            mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la bodega.");
                        }
                        else
                        {

                            cargarEstaciones();
                            cargarAnfitriones();
                            cargarEstados();

                            seConsulto = false;
                        }
                    }
                }
                cambiarModo();
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
                case (int)Modo.ConsultaPerfil:
                    FieldsetGrid.Visible = true;
                    FieldsetPerfil.Visible = false;
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
            cargarEstaciones();
            cargarAnfitriones();
            cargarEstados();
            cambiarModo();
        }

        // Consulta los usuarios
        protected void botonConsultarUsuario_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.ConsultaUsuario;
            cambiarModo();
        }

        // Consulta perfiles
        protected void botonConsultarPerfil_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.ConsultaPerfil;
            cambiarModo();
        }

        //Carga las estaciones al combobox
        protected void cargarEstaciones()
        {
            DropDownListEstacion.Items.Clear();
            DropDownListEstacion.Items.Add(new ListItem("", null));
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            foreach (DataRow fila in estaciones.Rows)
            {
                DropDownListEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        //Metodo que muestra el resultado de la accion
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {

            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        }


        protected void botonAceptarUsuario_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";

            if (modo == (int)Modo.InsercionUsuario)
            {
                codigoInsertado = crearUsuario();

                if (codigoInsertado != "")
                {
                    operacionCorrecta = true;
                    usuarioConsultado = controladoraSeguridad.consultarUsuario(codigoInsertado);
                    modo = (int)Modo.ConsultadoUsuario;
                    habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }

        //Metodo que realiza la insercion de un nuevo usuario en la base de datos
        protected String crearUsuario()
        {
            String codigo = "";
            Object[] usuario = obtenerDatosCuenta();
            String[] error = controladoraSeguridad.insertarUsuario(usuario);
            codigo = Convert.ToString(error[3]);
            mostrarMensaje(error[0], error[1], error[2]);
            if (error[0].Contains("success"))
            {
                llenarGrid();
            }
            else
            {
                codigo = "";
                modo = (int)Modo.InsercionUsuario;
            }

            return codigo;
        }


        //Metodo que habilita o deshabilita los campos de usuario
        protected void habilitarCampos(bool habilitar)
        {
            this.inputUsuario.Disabled = !habilitar;
            this.inputNombre.Disabled = !habilitar;
            this.inputPassword.Disabled = !habilitar;
            this.inputPasswordConfirm.Disabled = !habilitar;
            this.DropDownListEstacion.Enabled = habilitar;
            this.inputDescripcion.Disabled = !habilitar;
            this.DropDownListAnfitriona.Enabled = habilitar ;
            this.DropDownListEstado.Enabled = habilitar;
            this.inputDescuentoMaximo.Disabled = !habilitar;
        }

        // Metodo que llena el grid de cuentas consultadas
        protected void llenarGrid()
        {
            DataTable tabla = tablaUsuarios();
            int indiceNuevoUsuario = -1;
            int i = 0;

            try
            {
                // Cargar usuarios
                Object[] datos = new Object[4];
                DataTable usuarios = controladoraSeguridad.consultarUsuarios();
                if (usuarios.Rows.Count > 0)
                {
                    idArray = new Object[usuarios.Rows.Count];
                    foreach (DataRow fila in usuarios.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[3].ToString();
                        datos[2] = fila[4].ToString();
                        datos[3] = fila[5].ToString();
                        tabla.Rows.Add(datos);
                        if (usuarioConsultado != null && (fila[0].Equals(usuarioConsultado.Codigo)))
                        {
                            indiceNuevoUsuario = i;
                        }
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = "-";
                    tabla.Rows.Add(datos);
                    mostrarMensaje("warning", "Atención: ", "No existen bodegas en la base de datos.");
                }

                this.gridViewGeneral.DataSource = tabla;
                this.gridViewGeneral.DataBind();
            }
            catch (Exception e)
            {
                //mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
    


        }


        protected Object[] obtenerDatosCuenta()
        {
            Object[] datos = new Object[10];
            datos[0] = 0;
            datos[1] = this.inputUsuario.Value;
            datos[2] = this.inputPassword.Value;
            datos[3] = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            datos[4] = this.inputDescripcion.Value;
            datos[5] = this.DropDownListEstacion.SelectedValue;
            datos[6] = this.DropDownListAnfitriona.SelectedValue;
            datos[7] = this.inputNombre.Value;
            datos[8] = this.DropDownListEstado.SelectedValue;
            datos[9] = this.inputDescuentoMaximo.Value;
            return datos;
        }

        //Tabla de consulta de cuentas
        protected DataTable tablaUsuarios()
        {
            DataTable tabla = new DataTable();
            //A Carlos le toca implementarlo
            return tabla;
        }

        // Tabla de consulta de perfiles
        protected DataTable tablaPerfiles()
        {
            DataTable tabla = new DataTable();
            // Toca implementarlo
            return tabla;
        }

        //Carga los anfitriones de la base de datos
        protected void cargarAnfitriones()
        {

            DropDownListAnfitriona.Items.Clear();
            DropDownListAnfitriona.Items.Add(new ListItem("", null));
            DataTable anfitriones = controladoraDatosGenerales.consultarAnfitriones();
            foreach (DataRow fila in anfitriones.Rows)
            {
                DropDownListAnfitriona.Items.Add(new ListItem(fila[2].ToString(), fila[0].ToString()));
            }
        }

        //Carga los posibles estados de una cuenta
        protected void cargarEstados()
        {

            DropDownListEstado.Items.Clear();
            DropDownListEstado.Items.Add(new ListItem("", null));
            DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
            foreach (DataRow fila in estados.Rows)
            {
                DropDownListEstado.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }

    }
}