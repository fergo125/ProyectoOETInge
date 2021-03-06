﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_Bodegas;

namespace ProyectoInventarioOET
{
    public partial class FormSeguridad : System.Web.UI.Page
    {

        enum Modo { Inicial, InicialPerfil, InicialUsuario, ConsultaPerfil, InsercionPerfil, ModificarPerfil, ConsultaUsuario, InsercionUsuario, ModificarUsuario, ConsultadoUsuario, ConsultadoPerfil };
       // Atributos
        private static int modo = (int)Modo.Inicial;                    // Modo actual de la pagina
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static ControladoraSeguridad controladoraSeguridad;
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //Controladora para consultar las estaciones
        private static ControladoraBodegas controladoraBodegas;
        private static EntidadUsuario usuarioConsultado;                        //Entidad que almacena la cuenta consultada
        private static EntidadPerfil perfilConsultado;                          //Entidad que almacena el perfil consultado
        private static Boolean seConsulto = false;                              //Bandera que revisa si ya se consulto o no                       //???
        private static Object[] idArray;                                //Array de ids para almacenar los usuarios
        private static String perfilSeleccionado="";                                //Array de ids para almacenar los usuarios
        private static Object[] idArrayPerfiles;                        //Array de ids para almacenar los perfiles
        private static Object[] idBodegas;                                //Array de ids para almacenar los usuarios
        private static DataTable tablaCuentas;
        private static DataTable tablaPerfiles;                         // Datatable para almacenar los perfiles de consulta
        private static DataTable bodegasEstacion;
        private static String argumentoSorteo = "";
        private static bool boolSorteo = false;
        private static DataTable tablaOrdenable;
        
        /*
         * ???
         */
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
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Seguridad");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                //mostrarBotonesSegunPermisos();

                    if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (usuarioConsultado == null && perfilConsultado == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar.");
                    }
                    else
                    {

                        cargarEstaciones();
                        cargarAnfitriones();
                        cargarEstados();
                        cargarPerfiles();
                        cargarNivelesPerfil();
                        if (usuarioConsultado != null)
                            setDatosCuenta();
                        if (perfilConsultado != null)
                            setDatosPerfil();
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
                    FieldsetUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    tituloAccionForm.InnerText = "";
                    FieldsetConsultarPerfil.Visible = false;

                    break;

                case (int)Modo.InicialPerfil:
                    ArbolPermisos.Enabled = false;
                    FieldsetBotonesPerfiles.Visible = true;
                    FieldsetBotonesUsuarios.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    this.botonModificarUsuario.Disabled = true;
                    tituloAccionForm.InnerText = "";
                    FieldsetConsultarPerfil.Visible = false;
                    botonConsultarPerfil.Disabled = false;
                    botonCrearPerfil.Disabled = false;
                    botonModificarPerfil.Disabled = true;
                    break;

                case (int)Modo.InicialUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetBotonesUsuarios.Visible = true;
                    FieldsetBotonesPerfiles.Visible = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    this.botonModificarUsuario.Disabled = true;
                    tituloAccionForm.InnerText = "";
                    FieldsetConsultarPerfil.Visible = false;
                    break;

                case (int)Modo.ConsultaPerfil:
                    ArbolPermisos.Enabled = false;
                    FieldsetGrid.Visible = true;
                    FieldsetPerfil.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    FieldsetConsultarPerfil.Visible = true;
                    FieldsetBotonesPerfiles.Visible = true;
                    botonConsultarPerfil.Disabled = true;
                    botonCrearPerfil.Disabled = false;
                    botonModificarPerfil.Disabled = true;
                    break;

                case (int)Modo.ConsultaUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetGrid.Visible = true;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGridCuentas.Visible = true;
                    FieldsetPerfil.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    this.botonModificarUsuario.Disabled = true;
                    FieldsetConsultarPerfil.Visible = false;
                    break;

                case (int)Modo.InsercionUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetUsuario.Visible = true;

                    inputPassword.Visible = true;
                    inputPasswordConfirm.Visible = true;
                    labelInputPassword.Visible = true;
                    labelInputPasswordConfirm.Visible = true;   
                    FieldsetBotones.Visible = true;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfilCreacion.Visible = false;
                    FieldsetConsultarPerfil.Visible = false;

                    this.labelCheckboxCambiaPassword.Visible = false;
                    this.checkboxCambiaPassword.Visible = false;
                    break;

                case (int)Modo.ModificarUsuario:
                    ArbolPermisos.Enabled = false;
                    tituloAccionForm.InnerText = "Modifique la información del usuario.";
                    FieldsetUsuario.Visible = true;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = true;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    gridViewBodegas.Enabled = true;
                    this.inputFecha.Disabled = true;
                    FieldsetConsultarPerfil.Visible = false;
                    this.inputPassword.Visible = true;
                    this.inputPassword.Disabled = true;
                    this.inputPasswordConfirm.Visible = true;
                    this.inputPasswordConfirm.Disabled = true;
                    this.labelInputPassword.Visible = true;
                    this.labelInputPasswordConfirm.Visible = true;
                    this.labelCheckboxCambiaPassword.Visible = true;
                    this.checkboxCambiaPassword.Visible = true;
                    this.botonModificarUsuario.Disabled = true;
                    break;

                case (int)Modo.InsercionPerfil:
                    tituloAccionForm.InnerText = "Ingrese los datos para el nuevo perfil";
                    ArbolPermisos.Enabled = true;
                    FieldsetUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfil.Visible = true;
                    FieldsetPerfilCreacion.Visible = true;
                    botonConsultarPerfil.Disabled = false;
                    botonCrearPerfil.Disabled = true;
                    botonModificarPerfil.Disabled = true;
                    botonAceptarCreacionPerfil.Visible = true;
                    botonCancelarCreacionPerfil.Visible = true;
                    FieldsetConsultarPerfil.Visible = false;
                    habilitarCamposPerfil(true);
                    break;

                case (int)Modo.ModificarPerfil:
                    tituloAccionForm.InnerText = "Cambie los datos para del perfil";
                    FieldsetBotonesPerfiles.Visible = true;
                    botonConsultarPerfil.Disabled = false;
                    botonCrearPerfil.Disabled = false;
                    botonModificarPerfil.Disabled = true;
                    botonAceptarCreacionPerfil.Visible = true;
                    botonCancelarCreacionPerfil.Visible = true;
                    FieldsetConsultarPerfil.Visible = true;
                    FieldsetConsultarPerfil.Visible = false;
                    habilitarCamposPerfil(true);
                    break;

                case (int)Modo.ConsultadoUsuario:
                    ArbolPermisos.Enabled = false;
                    FieldsetUsuario.Visible = true;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGrid.Visible = false;
                    inputPassword.Visible = false; inputPasswordConfirm.Visible = false;
                    labelInputPassword.Visible = false; labelInputPasswordConfirm.Visible = false;                 
                    FieldsetGridCuentas.Visible = false;
                    FieldsetPerfil.Visible = false;
                    this.botonModificarUsuario.Disabled = false;
                    this.botonModificarUsuario.Visible = true;
                    this.FieldsetBotonesUsuarios.Visible = true;
                    this.botonModificarUsuario.Visible = true;
                    FieldsetConsultarPerfil.Visible = false;

                    this.labelCheckboxCambiaPassword.Visible = false;
                    this.checkboxCambiaPassword.Visible = false;
                    break;

                case (int)Modo.ConsultadoPerfil:
                    tituloAccionForm.InnerText = "Datos del Perfil";
                    ArbolPermisos.Enabled = false;
                    FieldsetUsuario.Visible = false;
                    FieldsetBotones.Visible = false;
                    FieldsetBotonesModificar.Visible = false;
                    FieldsetGrid.Visible = false;
                    FieldsetGridCuentas.Visible = false;
                    FieldsetBotonesPerfiles.Visible = true;
                    botonConsultarPerfil.Disabled = true;
                    botonCrearPerfil.Disabled = false;
                    botonModificarPerfil.Disabled = (perfilConsultado != null && perfilConsultado.Nivel == 1);
                    FieldsetPerfil.Visible = true;
                    FieldsetPerfilCreacion.Visible = true;
                    botonAceptarCreacionPerfil.Visible = false;
                    botonCancelarCreacionPerfil.Visible = false;
                    FieldsetConsultarPerfil.Visible = true;
                    habilitarCamposPerfil(false);
                    llenarGridPerfiles();
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
        protected String[] manejarPermisosArbol(bool obtener, String[] permisosConsultados)
        {
            String[] permisos = (obtener ? new String[11] : permisosConsultados); //hay 11 subinterfaces
            short iterador = 0;
            foreach(TreeNode interfaz in ArbolPermisos.Nodes[0].ChildNodes) //hijos de la raíz
            {
                foreach(TreeNode subinterfaz in interfaz.ChildNodes) //hijos de la interfaz
                {
                    if(obtener)
                    {
                        permisos[iterador] = (dropDownListCrearPerfilNivel.SelectedValue == "1" ? "111" : "000");
                        permisos[iterador] += (subinterfaz.ChildNodes[2].Checked ? "1" : "0"); //modificar
                        permisos[iterador] += (subinterfaz.ChildNodes[1].Checked ? "1" : "0"); //insertar
                        permisos[iterador] += (subinterfaz.ChildNodes[0].Checked ? "1" : "0"); //consultar
                    }
                    else
                    {
                        subinterfaz.ChildNodes[2].Checked = (permisos[iterador][3] == '1' ? true : false); //modificar
                        subinterfaz.ChildNodes[1].Checked = (permisos[iterador][4] == '1' ? true : false); //insertar
                        subinterfaz.ChildNodes[0].Checked = (permisos[iterador][5] == '1' ? true : false); //consultar
                    }
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
            limpiarCamposPerfil();
            cambiarModo();
            llenarArbol();
            cargarNivelesPerfil();
        }

        /*
         * Invocada cuando se da click al botón "Guardar" debajo de los campos de creación de perfil.
         */
        protected void botonAceptarCreacionPerfil_ServerClick(object sender, EventArgs e)
        {
            //if(textBoxCrearPerfilNombre.Value == null)
            //{
            //    mostrarMensaje("warning", "Atención: ", "Ingrese un nombre para el nuevo perfil.");
            //    return;
            //}
            //primero revisar que el nombre no sea repetido
            if (controladoraSeguridad.consultarPerfil(textBoxCrearPerfilNombre.Value) != null && !(perfilConsultado != null && perfilConsultado.Nombre == textBoxCrearPerfilNombre.Value) ) //ya existe
            {
                mostrarMensaje("danger", "Error: ", "Ese nombre ya pertenece a otro perfil existente.");
                return;
            }
            //segundo intentar crear el nuevo perfil
            String[] resultado = new String[3];
            if( modo == (int)Modo.InsercionPerfil )
                resultado = controladoraSeguridad.insertarPerfil(textBoxCrearPerfilNombre.Value, Convert.ToInt32(dropDownListCrearPerfilNivel.SelectedValue), manejarPermisosArbol(true, null));
            if (modo == (int)Modo.ModificarPerfil)
                resultado = modificar();
            mostrarMensaje(resultado[0], resultado[1], resultado[2]);

            if(resultado[0].Contains("success"))
            {
                llenarGridPerfiles();
                modo = (int)Modo.ConsultadoPerfil;
                cambiarModo();
            }
        }

        protected String[] modificar()
        {
            Object[] datos = new Object[3];
            datos[0] = textBoxCrearPerfilNombre.Value;
            datos[1] = Convert.ToInt32(dropDownListCrearPerfilNivel.SelectedValue);
            datos[2] = manejarPermisosArbol(true, null);
            EntidadPerfil nueva = new EntidadPerfil(datos);

            return controladoraSeguridad.modificarPerfil(perfilConsultado.Nombre, nueva);
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

        /*
         * Cambia la interfa a modo de modificacion de perfiles
         */
        protected void botonModificarPerfil_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.ModificarPerfil;
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
            limpiarCampos();
            habilitarCampos(true);
            modo = (int)Modo.InsercionUsuario;
            cargarEstaciones();
            cargarAnfitriones();
            cargarEstados();
            cargarPerfiles();
            inputFecha.Disabled = true;
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
            llenarGridPerfiles();
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
                if (operacionCorrecta)
                modo = (int)Modo.Inicial;
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }


        protected void botonModificarCuentaUsuario_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;

            if (modo == (int)Modo.ModificarUsuario)
            {
                operacionCorrecta = modificarUsuario();
                if (operacionCorrecta)
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
            String perfilSeleccionadoB = DropDownListPerfilConsulta.SelectedValue;
            if (!controladoraSeguridad.nombreUsuarioRepetido(usuario[1].ToString()))
            {
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
                    if (((CheckBox)gridViewBodegas.Rows[i].FindControl("checkBoxBodegas")).Checked)
                {
                    String llaveBodega = controladoraBodegas.consultarLlaveBodega(fila.Cells[1].Text, DropDownListEstacion.SelectedValue);
                    res = controladoraSeguridad.asociarABodega(codigo, llaveBodega, DropDownListEstacion.SelectedValue);

                }
                i++;
                
            }
            res = controladoraSeguridad.asociarPerfilNuevoUsuario(codigo, perfilSeleccionadoB);
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

            
            }
            else
            {
                mostrarMensaje("warning", "Alerta", "El nombre de usuario especificado ya existe, por favor revise los datos.");
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
            Boolean usuarioCambio = !(usuarioConsultado.Usuario.Equals(usuario[1]));
            Boolean repetido = false;

            if (usuarioCambio) 
            { 
                repetido = controladoraSeguridad.nombreUsuarioRepetido(usuario[1].ToString());
            }

            if (!repetido)
            {
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
                String perfil = DropDownListPerfilConsulta.SelectedItem.Text;
                String[] error = controladoraSeguridad.modificarUsuario(usuario, listadoBodegas, perfil);
                String[] mensaje = new String[3];
                if (!"".Equals(this.inputPassword.Value.ToString().Trim()))
                {
                    mensaje = controladoraSeguridad.modificarContrasena(usuarioConsultado.Codigo, this.inputPassword.Value);
                }
                else 
                {
                    mensaje[0] = "success";                
                }

                mostrarMensaje(error[0], error[1], error[2]);
                if (error[0].Contains("success") & mensaje[0].Contains("success"))
                {
                    exito = true;
                }
            }
            else
            {
                mostrarMensaje("warning", "Alerta", "El nombre de usuario especificado ya existe, por favor revise los datos.");
                exito = false;
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
            this.gridViewBodegas.Enabled = habilitar;
        }

        /*
         * Metodo que habilita o deshabilita los campos de perfiles
         */
        protected void habilitarCamposPerfil(bool habilitar)
        {
            this.textBoxCrearPerfilNombre.Disabled = !habilitar;
            this.dropDownListCrearPerfilNivel.Enabled = habilitar;
            this.PanelArbolPermisos.Enabled = true;
            this.ArbolPermisos.Enabled = habilitar;
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
        }

        protected void limpiarCamposPerfil()
        {
            this.textBoxCrearPerfilNombre.Value = "";
        }

        /*
         * Metodo que llena el grid de cuentas consultadas
         */
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
                tablaOrdenable = tablaCuentas;
            }
            catch (Exception e)
            {
            }
        }
    

        /*
         * Metodo que llena el grid de perfiles
         * Viaja a la base de datos y carga los datos a memoria
         */
        protected void llenarGridPerfiles()
        {
            tablaPerfiles = crearTablaPerfiles();
            int indiceNuevoPerfil = -1;
            int i = 0;

            try
            {
                // Cargar usuarios
                Object[] datos = new Object[2];
                DataTable perfiles = controladoraSeguridad.consultarPerfiles();
                if (perfiles.Rows.Count > 0)
                {
                    idArrayPerfiles = new Object[perfiles.Rows.Count];
                    foreach (DataRow fila in perfiles.Rows)
                    {
                        idArrayPerfiles[i] = fila[1];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[3].ToString();
                        tablaPerfiles.Rows.Add(datos);
                        if (perfilConsultado != null && (fila[0].Equals(perfilConsultado.Nombre)))
                        {
                            indiceNuevoPerfil = i;
                        }
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    tablaPerfiles.Rows.Add(datos);
                    mostrarMensaje("warning", "Atención: ", "No existen perfiles en la base de datos.");
        }

                this.gridViewConsultaPerfiles.DataSource = tablaPerfiles;
                this.gridViewConsultaPerfiles.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
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
            this.inputFecha.Value = Convert.ToDateTime(usuarioConsultado.FechaCreacion).Date.ToString("dd/MM/yyyy"); 
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

        /*
         * Despleiga la información en la pantalla de la entidad perfil consultada
         */
        protected void setDatosPerfil()
        {
            textBoxCrearPerfilNombre.Value = perfilConsultado.Nombre;
            dropDownListCrearPerfilNivel.SelectedValue = perfilConsultado.Nivel.ToString();
            llenarArbol();
            manejarPermisosArbol(false, perfilConsultado.Permisos);
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
        protected DataTable crearTablaPerfiles()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Int32");
            columna.ColumnName = "Nivel";
            tabla.Columns.Add(columna);

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
         * Método auxiliar que viaja a la base de datos y maneja la consulta de cuentas
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
         * Método auxiliar que viaja a la base de datos y maneja la consulta de perfiles
         */
        protected void consultarPerfil(String id)
        {
            seConsulto = true;
            try
            {
                perfilConsultado = controladoraSeguridad.consultarPerfil(id);
                modo = (int)Modo.ConsultadoPerfil;
            }
            catch
            {
                perfilConsultado = null;
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


        /*
         * Procedimiento invocado cuando se selecciona uno de los perfiles para consultar su información.
         */
        protected void gridViewConsultaPerfiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            {
                switch (e.CommandName)
                {
                    case "Select":

                        GridViewRow filaSeleccionada = this.gridViewConsultaPerfiles.Rows[Convert.ToInt32(e.CommandArgument)];
                        String codigo = Convert.ToString(idArrayPerfiles[Convert.ToInt32(e.CommandArgument) + (this.gridViewConsultaPerfiles.PageIndex * this.gridViewConsultaPerfiles.PageSize)]);
                        consultarPerfil(codigo);
                        Response.Redirect("FormSeguridad.aspx");
                        break;
                }
            }
        }

        /*
         * Procedimiento invocado cuando se cambia la página de la tabla con los pefiles.
         */
        protected void gridViewConsultaPerfiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gridViewConsultaPerfiles.PageIndex = e.NewPageIndex;
            this.gridViewConsultaPerfiles.DataSource = tablaPerfiles;
            this.gridViewConsultaPerfiles.DataBind();
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

        protected void dropDownListCrearPerfilNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            perfilSeleccionado = DropDownListPerfilConsulta.SelectedValue;
        }

        /*
         * Evento invocado cuando se da click al checkbox para cambiar la contraseña,
         * habilita y deshabilita los campos para ingresar la contraseña nueva.
         */
        protected void checkboxCambiaPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                this.inputPassword.Disabled = false;
                this.inputPasswordConfirm.Disabled = false;
            }
            else {
                this.inputPassword.Disabled = true;
                this.inputPasswordConfirm.Disabled = true;            
            }
        }

        protected void gridViewCuentas_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == argumentoSorteo)
            {
                if (boolSorteo == true)
                    boolSorteo = false;
                else
                    boolSorteo = true;
            }
            else //New Column clicked so the default sort direction will be incorporated
                boolSorteo = false;

            argumentoSorteo = e.SortExpression; //Update the sort column
            BindGrid(argumentoSorteo, boolSorteo);
        }


        /*
 * Auxiliar para ordenar grid
 */
        public void BindGrid(string sortBy, bool inAsc)
        {
            agregarID();
            DataView aux = new DataView(tablaOrdenable);
            aux.Sort = sortBy + " " + (inAsc ? "DESC" : "ASC"); //Ordena
            tablaOrdenable = aux.ToTable();
            actualizarIDs();
            gridViewCuentas.DataSource = tablaOrdenable;
            gridViewCuentas.DataBind();
        }

        public void agregarID()
        {
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "id";
            tablaOrdenable.Columns.Add(columna);
            int i = 0;
            foreach (DataRow fila in tablaOrdenable.Rows)
            {
                fila[3] = idArray[i];
                i++;
            }
        }

        public void actualizarIDs()
        {
            int i = 0;
            foreach (DataRow fila in tablaOrdenable.Rows)
            {
                idArray[i] = fila[3];
                i++;
            }
            tablaOrdenable.Columns.Remove("id");
        }

    }
}