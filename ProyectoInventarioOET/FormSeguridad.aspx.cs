using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.Modulo_Bodegas;

namespace ProyectoInventarioOET
{
    public partial class FormSeguridad : System.Web.UI.Page
    {

        enum Modo { Inicial, InicialPerfil, InicialUsuario, ConsultaPerfil, InsercionPerfil, ModificacionPerfil, ConsultaUsuario, InsercionUsuario, ModificarUsuario, ConsultadoUsuario };
       // Atributos
        private static int modo = (int)Modo.Inicial;                    // Modo actual de la pagina
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static ControladoraSeguridad controladoraSeguridad;
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //Controladora para consultar las estaciones
        private static ControladoraBodegas controladoraBodegas;
        private static EntidadUsuario usuarioConsultado;                        //Entidad que almacena la cuenta consultada
        private static Boolean seConsulto = false;                              //Bandera que revisa si ya se consulto o no                       //???
        private static Object[] idArray;                                //Array de ids para almacenar los usuarios
        private static Object[] idBodegas;                                //Array de ids para almacenar los usuarios
        private static DataTable tablaCuentas;
        private static DataTable bodegasEstacion;
        
        protected void Page_Load(object sender, EventArgs e)
        {
                mensajeAlerta.Visible = false;
            
                ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab();", true); //para que quede marcada la página seleccionada en el sitemaster
                labelAlerta.Text = "";

                if (!IsPostBack)
                {
                    controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                    controladoraSeguridad = new ControladoraSeguridad();
                    controladoraBodegas = new ControladoraBodegas();
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
                            cargarPerfiles();
                            setDatosCuenta();
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
                    ArbolPermisos.Enabled = false;
                    FieldsetBotonesPerfiles.Visible = false;
                    FieldsetBotonesUsuarios.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    tituloAccionForm.InnerText = "";
                    break;

                case (int)Modo.InicialPerfil:
                    ArbolPermisos.Enabled = false;
                    FieldsetBotonesPerfiles.Visible = true;
                    FieldsetBotonesUsuarios.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    this.botonModificarUsuario.Disabled = true;
                    tituloAccionForm.InnerText = "";
                    break;

                case (int)Modo.InicialUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetBotonesUsuarios.Visible = true;
                    FieldsetBotonesPerfiles.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    this.botonModificarUsuario.Disabled = true;
                    tituloAccionForm.InnerText = "";
                    break;

                case (int)Modo.ConsultaPerfil:
                    ArbolPermisos.Enabled = false;
                    FieldsetGrid.Visible = true;
                    FieldsetPerfil.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    break;

                case (int)Modo.ConsultaUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetGrid.Visible = true;
                    FieldsetBotones.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetGridCuentas.Visible = true;
                    FieldsetPerfil.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    this.botonModificarUsuario.Disabled = true;
                    break;

                case (int)Modo.InsercionUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetUsuario.Visible = true;

                    inputPassword.Visible = true;
                    inputPasswordConfirm.Visible = true;
                    labelInputPassword.Visible = true;
                    labelInputPasswordConfirm.Visible = true;
                    
                    DropDownListPerfilConsulta.Visible = false; labelDropDownListPerfilConsulta.Visible = false;
                    inputFecha.Visible = false; labelInputFecha.Visible = false;

                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = true;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    break;

                case (int)Modo.ModificarUsuario:
                    ArbolPermisos.Enabled = false;
                    tituloAccionForm.InnerText = "Modifique la información del usuario.";
                    FieldsetUsuario.Visible = true;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = true;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    gridViewBodegas.Enabled = true;
                    this.inputFecha.Disabled = true;
                    break;

                case (int)Modo.InsercionPerfil:
                    tituloAccionForm.InnerText = "Ingrese los datos para el nuevo perfil";
                    ArbolPermisos.Enabled = true;
                    FieldsetUsuario.Visible = false;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfil.Visible = true;
                    FieldsetPerfilCreacion.Visible = true;
                    break;
                //case (int)Modo.AsociarUsuario:
                //    FieldsetUsuario.Visible = false;
                //    FieldsetAsociarUsuario.Visible = true;
                //    FieldsetBotones.Visible = true;
                //    FieldsetGrid.Visible = false;
                //    FieldsetGridCuentas.Visible = false;
                //    FieldsetPerfilCreacion.Visible = false;
                //    break;
                case (int)Modo.ConsultadoUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetUsuario.Visible = true;
                    FieldsetAsociarUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetGrid.Visible = false;

                    inputPassword.Visible = false; inputPasswordConfirm.Visible = false;
                    labelInputPassword.Visible = false; labelInputPasswordConfirm.Visible = false;
                    DropDownListPerfilConsulta.Visible = true; labelDropDownListPerfilConsulta.Visible = true;
                    inputFecha.Visible = true; labelInputFecha.Visible = true;
                    
                    this.gridViewBodegas.Enabled = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfil.Visible = false;
                    this.botonModificarUsuario.Disabled = false;
                    this.FieldsetBotonesUsuarios.Visible = true;
                    this.botonModificarUsuario.Visible = true;
                    break;
            }
        }

        /*
         * Llena el árbol de permisos, con el texto necesario, el consultar o llenar los checkboxes es en otra función.
         */
        protected void llenarArbol()
        {
            //Agregar los permisos para cada subinterfaz
            ArbolPermisos.Nodes.Clear();
            TreeNode raiz = new TreeNode();
            raiz.Text = "Interfaces";
            raiz.Value = "RAI";
            for(short i=1; i<=5; ++i)
            {
                TreeNode interfaz = new TreeNode();
                switch(i)
                {
                    case 1: interfaz.Text = "Productos";        interfaz.Value = "PRO"; break;
                    case 2: interfaz.Text = "Bodegas";          interfaz.Value = "BOD"; break;
                    case 3: interfaz.Text = "Inventario";       interfaz.Value = "INV"; break;
                    case 4: interfaz.Text = "Ventas";           interfaz.Value = "VEN"; break;
                    case 5: interfaz.Text = "Administración";   interfaz.Value = "ADM"; break;
                    default: break;
                }
                for(short s=1; s<=3; ++s)
                {
                    TreeNode subinterfaz = new TreeNode();
                    switch(i)
                    {
                        case 1: //Productos
                            switch(s)
                            {
                                case 1: subinterfaz.Text = "Catálogo general de productos";     subinterfaz.Value = "CGP"; break;
                                case 2: subinterfaz.Text = "Categorías de productos";           subinterfaz.Value = "CDP"; ++s; break;
                                default: break;
                            }
                            break;
                        case 2: //Bodegas
                            switch(s)
                            {
                                case 1: subinterfaz.Text = "Catálogos de productos en bodegas"; subinterfaz.Value = "CPB"; break;
                                case 2: subinterfaz.Text = "Gestión de bodegas";                subinterfaz.Value = "GDB"; break;
                                case 3: subinterfaz.Text = "Gestión de actividades";            subinterfaz.Value = "GDA"; break;
                                default: break;
                            }
                            break;
                        case 3: //Inventario
                            switch(s)
                            {
                                case 1: subinterfaz.Text = "Entradas de inventario";            subinterfaz.Value = "EDI"; break;
                                case 2: subinterfaz.Text = "Ajustes de inventario";             subinterfaz.Value = "ADI"; break;
                                case 3: subinterfaz.Text = "Traslados de inventario";           subinterfaz.Value = "TDI"; break;
                                default: break;
                            }
                            break;
                        case 4: //Ventas
                            switch(s)
                            {
                                case 1: subinterfaz.Text = "Facturación";                       subinterfaz.Value = "FDV"; ++s; ++s; break;
                                default: break;
                            }
                            break;
                        case 5: //Administración
                            switch(s)
                            {
                                case 1: subinterfaz.Text = "Reportes";                          subinterfaz.Value = "RDC"; break;
                                case 2: subinterfaz.Text = "Seguridad";                         subinterfaz.Value = "SDS"; ++s; break;
                                default: break;
                            }
                            break;
                        default: break;
                    }
                    for(short p=1; p<=3; ++p)
                    {
                        TreeNode permisos = new TreeNode();
                        switch(p)
                        {
                            case 1: permisos.Text = " Consultar";   permisos.Value = "C"; break;
                            case 2: permisos.Text = " Crear";       permisos.Value = "I"; break;
                            case 3: permisos.Text = " Modificar";   permisos.Value = "M"; break;
                            default: break;
                        }
                        subinterfaz.ChildNodes.Add(permisos);
                    }
                    interfaz.ChildNodes.Add(subinterfaz);
                }
                raiz.ChildNodes.Add(interfaz);
                interfaz.Expand();
            }
            ArbolPermisos.Nodes.Add(raiz);
            raiz.Expand();
        }

        /*
         * 
         */
        protected void cargarNivelesPerfil()
        {
            dropDownListCrearPerfilNivel.Items.Clear();
            dropDownListCrearPerfilNivel.Items.Add(new ListItem("Normal (no supervisa nada)", "4"));
            dropDownListCrearPerfilNivel.Items.Add(new ListItem("Medio (supervisa vendedores)", "3"));
            dropDownListCrearPerfilNivel.Items.Add(new ListItem("Alto (supervisa bodegas)", "2"));
            dropDownListCrearPerfilNivel.Items.Add(new ListItem("Máximo (supervisa estaciones)", "1"));
        }

        /*
         * Forma un arreglo de strings que representan 
         */
        protected String[] obtenerPermisosArbol()
        {
            String[] permisos = new String[11]; //hay 11 subinterfaces
            short iterador = 0;
            foreach(TreeNode interfaz in ArbolPermisos.Nodes[0].ChildNodes) //hijos de la raíz
            {
                foreach(TreeNode subinterfaz in interfaz.ChildNodes) //hijos de la interfaz
                {
                    permisos[iterador] = (dropDownListCrearPerfilNivel.SelectedValue == "1" ? "111" : "000");
                    permisos[iterador] += (subinterfaz.ChildNodes[2].Checked ? "1" : "0"); //modificar
                    permisos[iterador] += (subinterfaz.ChildNodes[1].Checked ? "1" : "0"); //insertar
                    permisos[iterador] += (subinterfaz.ChildNodes[0].Checked ? "1" : "0"); //consultar
                    ++iterador;
                }
            }
            return permisos;
        }

        /*
         * Muestra los campos necesarios para crear un perfil de seguridad, incluyendo el árbol de permisos.
         */
        protected void botonCrearPerfil_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.InsercionPerfil;
            cambiarModo();
            llenarArbol();
            cargarNivelesPerfil();
        }

        /*
         * Invocada cuando se da click al botón "Guardar" debajo de los campos de creación de perfil.
         */
        protected void botonAceptarCreacionPerfil_ServerClick(object sender, EventArgs e)
        {
            if(textBoxCrearPerfilNombre.Value == null)
            {
                mostrarMensaje("warning", "Atención: ", "Ingrese un nombre para el nuevo perfil.");
                return;
            }
            //primero revisar que el nombre no sea repetido
            if (controladoraSeguridad.consultarPerfil(textBoxCrearPerfilNombre.Value) != null) //ya existe
            {
                mostrarMensaje("danger", "Error: ", "Ese nombre ya pertenece a otro perfil existente.");
                return;
            }
            //segundo intentar crear el nuevo perfil
            String[] resultado = controladoraSeguridad.insertarPerfil(textBoxCrearPerfilNombre.Value, Convert.ToInt32(dropDownListCrearPerfilNivel.SelectedValue), obtenerPermisosArbol());
            mostrarMensaje(resultado[0], resultado[1], resultado[2]);
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
        protected void botonModificarUsuario_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.ModificarUsuario;
            habilitarCampos(true);
            cambiarModo();
        }

        // Confirmación del modal de cancelación
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
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
            cargarPerfiles();
            cambiarModo();
        }

        // Consulta los usuarios
        protected void botonConsultarUsuario_ServerClick(object sender, EventArgs e)
        {
            llenarGrid();
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


        /*
         * Carga las estaciones al combobox
        */
        protected void cargarPerfiles()
        {
            DropDownListPerfilConsulta.Items.Clear();
            DropDownListPerfilConsulta.Items.Add(new ListItem("", null));
            DataTable perfiles = controladoraSeguridad.consultarPerfiles();
            foreach (DataRow fila in perfiles.Rows)
            {
                DropDownListPerfilConsulta.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        /*Metodo que muestra el resultado de la accion*/
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {

            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        }

        /*Metodo que acepta el usuario*/
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
                    usuarioConsultado = controladoraSeguridad.consultarCuenta(codigoInsertado);
                    modo = (int)Modo.ConsultadoUsuario;
                    habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == (int)Modo.ModificarUsuario)
            {
                operacionCorrecta = modificarUsuario();
                modo = (int)Modo.Inicial;
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }

        /*Metodo que realiza la insercion de un nuevo usuario en la base de datos*/
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

            ControladoraBodegas controladoraBodegas = new ControladoraBodegas();
            int i = 0;
            String[] res = new String[3];
            foreach (GridViewRow fila in gridViewBodegas.Rows)
            {
                if(((CheckBox)gridViewBodegas.Rows[i].FindControl("checkBoxBodegas")).Checked)
                {
                    String llaveBodega = controladoraBodegas.consultarLlaveBodega(fila.Cells[1].Text, DropDownListEstacion.SelectedValue);
                    res = controladoraSeguridad.asociarABodega(codigo, llaveBodega, DropDownListEstacion.SelectedValue);

                }
                i++;
                
            }
            mostrarMensaje(res[0], res[1], res[2]);
            if (res[0].Contains("success"))
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

        /*
         * Metodo que realiza la modificación de un usuario en la base de datos          
         */
        protected Boolean modificarUsuario()
        {
            Boolean exito = false;
            Object[] usuario = obtenerDatosCuenta();
            usuario[0] = usuarioConsultado.Codigo;
            List<String> listadoBodegas = new List<String>();

            int i = 0;
            foreach (GridViewRow fila in gridViewBodegas.Rows)
            {
                if (((CheckBox)gridViewBodegas.Rows[i].FindControl("checkBoxBodegas")).Checked)
                {
                    String llaveBodega = controladoraBodegas.consultarLlaveBodega(fila.Cells[1].Text, DropDownListEstacion.SelectedValue);
                    listadoBodegas.Add(llaveBodega);
                }
                i++;
            }

            String[] error = controladoraSeguridad.modificarUsuario(usuario, listadoBodegas);
            mostrarMensaje(error[0], error[1], error[2]);
            if (error[0].Contains("success"))
            {
                exito = true;
            }
            return exito;
        }


        /*Metodo que habilita o deshabilita los campos de usuario*/
        protected void habilitarCampos(bool habilitar)
        {
            this.inputUsuario.Disabled = !habilitar;
            this.inputNombre.Disabled = !habilitar;
            this.inputPassword.Disabled = !habilitar;
            this.inputPasswordConfirm.Disabled = !habilitar;
            this.inputFecha.Disabled = !habilitar;
            this.DropDownListEstacion.Enabled = habilitar;
            this.inputDescripcion.Disabled = !habilitar;
            this.DropDownListAnfitriona.Enabled = habilitar ;
            this.DropDownListEstado.Enabled = habilitar;
            this.inputDescuentoMaximo.Disabled = !habilitar;
            this.DropDownListPerfilConsulta.Enabled = habilitar;
        }

        protected void limpiarCampos() 
        {
            this.inputUsuario.Value = "";
            this.inputNombre.Value = "";
            this.inputPassword.Value = "";
            this.inputPasswordConfirm.Value = "";
            this.inputFecha.Value = "";
            this.DropDownListEstacion.SelectedValue = "";
            this.inputDescripcion.Value = "";
            this.DropDownListAnfitriona.SelectedValue = "";
            this.DropDownListEstado.SelectedValue = "";
            this.inputDescuentoMaximo.Value = "";
            this.DropDownListPerfilConsulta.SelectedValue = "";
        }

        /*
         * Metodo que llena el grid de cuentas consultadas*/
        protected void llenarGrid()
        {
            tablaCuentas = tablaUsuarios();
            int indiceNuevoUsuario = -1;
            int i = 0;

            try
            {
                // Cargar usuarios
                Object[] datos = new Object[3];
                DataTable usuarios = controladoraSeguridad.consultarUsuarios();
                if (usuarios.Rows.Count > 0)
                {
                    idArray = new Object[usuarios.Rows.Count];
                    foreach (DataRow fila in usuarios.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[2].ToString();
                        datos[2] = fila[3].ToString();
                        tablaCuentas.Rows.Add(datos);
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
                    tablaCuentas.Rows.Add(datos);
                    mostrarMensaje("warning", "Atención: ", "No existen bodegas en la base de datos.");
                }

                this.gridViewCuentas.DataSource = tablaCuentas;
                this.gridViewCuentas.DataBind();
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


        protected void setDatosCuenta()
        {
            this.inputUsuario.Value = usuarioConsultado.Usuario;
            this.inputFecha.Value = usuarioConsultado.FechaCreacion.ToString().Substring(0,10); // Ver porque no funciona           
            this.DropDownListEstacion.SelectedIndex = DropDownListEstacion.Items.IndexOf(DropDownListEstacion.Items.FindByText(usuarioConsultado.DescripcionEstacion));
            this.DropDownListAnfitriona.SelectedIndex = DropDownListAnfitriona.Items.IndexOf(DropDownListAnfitriona.Items.FindByText(usuarioConsultado.DescripcionAnfitriona));
            this.DropDownListEstado.SelectedIndex = DropDownListEstado.Items.IndexOf(DropDownListEstado.Items.FindByText(usuarioConsultado.DescripcionEstado));
            this.DropDownListPerfilConsulta.SelectedIndex = DropDownListPerfilConsulta.Items.IndexOf(DropDownListPerfilConsulta.Items.FindByText(usuarioConsultado.Perfil));
            this.inputNombre.Value = usuarioConsultado.Nombre;
            this.inputDescripcion.Value = usuarioConsultado.Descripcion;
            this.inputDescuentoMaximo.Value = ""+usuarioConsultado.DescuentoMaximo;
            ControladoraBodegas controladoraBodegas = new ControladoraBodegas();
            bodegasEstacion = controladoraBodegas.consultarBodegasDeEstacion(usuarioConsultado.IdEstacion);
            DataTable aux = crearTablaBodegas();
            this.gridViewBodegas.DataSource = aux;
            this.gridViewBodegas.DataBind();
            foreach (DataRow fila in usuarioConsultado.Bodegas.Rows) {
                int i = 0;
                foreach (DataRow fila2 in aux.Rows)
                {
                    if (fila[1].ToString() == fila2[0].ToString()) {
                        ((CheckBox)gridViewBodegas.Rows[i].FindControl("checkBoxBodegas")).Checked = true;
                    }
                    i++;
                }
            }
            habilitarCampos(false);
        }

        private DataTable crearTablaBodegas()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);
            Object[] datos = new Object[1];
            try
            {
                if (bodegasEstacion.Rows.Count > 0)
                {
                    idArray = new Object[bodegasEstacion.Rows.Count];
                    int i = 0;
                    foreach (DataRow fila in bodegasEstacion.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                return tabla;
            }
            catch {
                datos[0] = "";
                tabla.Rows.Add(datos);
                return tabla;
            }
        }


        //Tabla de consulta de cuentas
        protected DataTable tablaUsuarios()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Perfil";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

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


        /*
         * Método auxiliar que viaja a la base de datos y maneja la consulta de ajustes
         */
        protected void consultarCuenta(String id)
        {
            seConsulto = true;
            try
            {
                usuarioConsultado = controladoraSeguridad.consultarCuenta(id);
                modo = (int)Modo.ConsultadoUsuario;
            }
            catch
            {
                usuarioConsultado = null;
                modo = (int)Modo.Inicial;
            }
            cambiarModo();
        }

        /*
         * Procedimiento invocado cuando se selecciona uno de los usuarios para consultar su información.
         */
        protected void gridViewCuentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            {
                switch (e.CommandName)
                {
                    case "Select":

                        GridViewRow filaSeleccionada = this.gridViewCuentas.Rows[Convert.ToInt32(e.CommandArgument)];
                        String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCuentas.PageIndex * this.gridViewCuentas.PageSize)]);
                        consultarCuenta(codigo);
                        Response.Redirect("FormSeguridad.aspx");  
                        break;
                }
            }
        }

        /*
         * Procedimiento invocado cuando se cambia la página de la tabla con los usuarios.
         */
        protected void gridViewCuentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gridViewCuentas.PageIndex = e.NewPageIndex;
            this.gridViewCuentas.DataSource = tablaCuentas;
            this.gridViewCuentas.DataBind();
        }


        protected void checkBoxBodegas_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownListEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ControladoraBodegas controladoraBodegas = new ControladoraBodegas();
            bodegasEstacion = controladoraBodegas.consultarBodegasDeEstacion(DropDownListEstacion.SelectedValue);
            this.gridViewBodegas.DataSource = crearTablaBodegas();
            this.gridViewBodegas.DataBind();
        }


        /*
         * Función invocada cuando se colapsa un nodo, por ahora no hace nada pero DEBE DEJARSE AQUÍ para que el árbol siga colapsando, por alguna razón.
         */
        protected void ArbolPermisos_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        {
        }

    }
}