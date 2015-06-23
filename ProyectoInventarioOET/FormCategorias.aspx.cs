using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Categorias;
using ProyectoInventarioOET.App_Code;

namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormCategorias : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado }; //Sirve para controlar los modos de la interfaz
        //Atributos
        private static int modo = (int)Modo.Inicial;                    //Almacena el modo actual de la interfaz
        private static Object[,] idArray;                              //Almacena los datos de todas las categorias almacenadas 
        private static ControladoraCategorias controladoraCategorias;   //Comunica a la interfaz con la base de datos.
        private static EntidadCategoria categoriaConsultada;            //La categoria que se consulto actualmente
        private static bool seConsulto = false;                         // Me dice si hay una entidad consultada
        private static ControladoraDatosGenerales controladoraDatosGenerales;
        private static String permisos = "000000";                       // Permisos utilizados para el control de seguridad.

        /*
         * Maneja las acciones que se ejecutan cuando se carga la página, establecer el modo de operación, 
         * cargar elementos de la interfaz, gestión de seguridad.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            //Elementos visuales
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster

            if (!IsPostBack)
            {
                controladoraCategorias = new ControladoraCategorias();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Gestion de actividades");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");

                // Esconder botones
                mostrarBotonesSegunPermisos();

                if (!seConsulto)
                {
                    modo = 0;
                }
                else
                {
                    if (categoriaConsultada == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la categoria.");
                    }
                    else
                    {
                        cargarEstados();
                        setDatosConsultados();
                        llenarGrid();
                        seConsulto = false;
                    }
                }
            }
            irAModo();
        }

        /*
         * Cambia el modo de la pantalla activando/desactivando o mostrando/ocultando elementos de acuerdo con la 
         * acción que se va a realizar.
         */
        protected void irAModo()
        {
            if (modo == (int)Modo.Consulta)
            { 
                bloqueBotones.Visible = false;
                bloqueBotones.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = true;
                camposCategoria.Visible = false;
                camposCategoria.Disabled = true;
                gridViewCategorias.Visible = true;
                tituloGrid.Visible = true;
                comboBoxEstadosActividades.Enabled = false;
                textoObligatorioBodega.Visible = false;
                mensajeAlerta.Visible = false;
                tituloAccion.InnerText = "Seleccione una opción";

            }
            else if (modo == (int)Modo.Modificacion)
            { 
                bloqueBotones.Visible = true;
                bloqueBotones.Disabled = false;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = false;
                camposCategoria.Visible = true;
                inputNombre.Disabled = false;
                gridViewCategorias.Visible = false;
                tituloGrid.Visible = false;
                comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
                textoObligatorioBodega.Visible = true;
                tituloAccion.InnerText = "Cambie los datos";
                mensajeAlerta.Visible = false;
            }
            else if (modo == (int)Modo.Insercion)
            {
                bloqueBotones.Visible = true;
                bloqueBotones.Disabled = false;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = false;
                camposCategoria.Visible = true;
                inputNombre.Disabled = false;
                inputNombre.Visible = true;
                gridViewCategorias.Visible = false;
                tituloGrid.Visible = false;
                comboBoxEstadosActividades.Enabled = true;
                comboBoxEstadosActividades.Visible = true;
                textoObligatorioBodega.Visible = true;
                tituloAccion.InnerText = "Ingrese datos";
                mensajeAlerta.Visible = false;


            }
            else if (modo == (int)Modo.Inicial)
            { 
                bloqueBotones.Visible = false;
                bloqueBotones.Disabled = true;
                botonModificacionCategoria.Disabled = true;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = false;
                camposCategoria.Visible = false;
                inputNombre.Visible = false;
                gridViewCategorias.Visible = false;
                tituloGrid.Visible = false;
                inputNombre.Visible = false;
                comboBoxEstadosActividades.Visible = false;
                textoObligatorioBodega.Visible = false;
                tituloAccion.InnerText = "Seleccione una categoria";
                mensajeAlerta.Visible = false;
            }
            else if (modo == (int)Modo.Consultado)
            { 
                bloqueBotones.Visible = false;
                bloqueBotones.Disabled = true;
                botonModificacionCategoria.Disabled = false;
                botonAgregarCategoria.Disabled = false;
                botonConsultaCategoria.Disabled = true;
                camposCategoria.Visible = true;
                camposCategoria.Disabled = true;
                inputNombre.Disabled = true;
                gridViewCategorias.Visible = true;
                tituloGrid.Visible = true;
                comboBoxEstadosActividades.Enabled = false;
                textoObligatorioBodega.Visible = false;
                tituloAccion.InnerText = "Categoria seleccionada";
                mensajeAlerta.Visible = false;
            }
            //aplicarPermisos();// se aplican los permisos del usuario para el acceso a funcionalidades
        }


        /*
         * Construye la tabla que se va a utilizar para mostrar la información de las categorias.
         */
        protected DataTable tablaCategorias()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            return tabla;
        }
        protected void cargarEstados()
        {
            comboBoxEstadosActividades.Items.Clear();
            comboBoxEstadosActividades.Items.Add(new ListItem("", null));
            DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
            foreach (DataRow fila in estados.Rows)
            {
                comboBoxEstadosActividades.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }
        /*
         * boton que redirecciona a la interfaz de productos.
         */
        protected void botonRedireccionProductos_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("FormProductos.aspx");
        }

        /*
         * Evento que maneja el cambio de página en la tabla de categorias.
         */
        protected void gridViewCategorias_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewCategorias.Rows[Convert.ToInt32(e.CommandArgument)];
                    Object[] entidad = new Object[3];
                    entidad[0] = idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCategorias.PageIndex * gridViewCategorias.PageSize),0];
                    entidad[1] = idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCategorias.PageIndex * gridViewCategorias.PageSize), 1];
                    entidad[2] = idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCategorias.PageIndex * gridViewCategorias.PageSize), 2];
                    categoriaConsultada = new EntidadCategoria(entidad);
                    seConsulto = true;
                    modo = 4;
                    Response.Redirect("FormCategorias.aspx");
                    break;
            }
        }

        /*
        * Evento que maneja el cambio de página en la tabla de categorias.
        */
        protected void gridViewCategorias_CambioPagina(object sender, GridViewPageEventArgs e)
        {
            llenarGrid();
            this.gridViewCategorias.PageIndex = e.NewPageIndex;
            this.gridViewCategorias.DataBind();
        }

        /*
         * Metodo que limpia los campos de los campos de la interfaz
         */
        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
            comboBoxEstadosActividades.SelectedValue = null;
        }

        /*
         * Deshabilita los campos de la interfaza
         */
        protected void deshabilitarCampos()
        {
            this.inputNombre.Disabled = true;
        }
        /*protected void habilitarCampos(bool cambio)
        {
            this.inputNombre.Disabled = cambio;
        }*/


        /*
         * Regresa la interfaz a modo inicial en caso de darse una cancelación de una acción (inserción o modificación).
         */
        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {
        }

        /*
         * Maneja el evento de presionar el botón para agregar una actividad, cambiando el modo de la pantalla.
         */
        protected void botonAgregarCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
          
            llenarGrid();
            irAModo();
            limpiarCampos();
            cargarEstados();
        }

        /*
         * Maneja el evento de presionar el botón para modificar una categoria, cambiando el modo de la pantalla.
         */
        protected void botonModificacionCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            irAModo();
        }

        /*
         * Maneja el evento de presionar el botón para consultar una categoria, cambiando el modo de la pantalla.
         */
        protected void botonConsultaCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Consulta;
            //limpiarCampos();
            llenarGrid();
            cargarEstados();
            irAModo();
        }

        /*
          * Muestra el mensaje que da el resultado de las transacciones que se efectúan.
          */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        }

        /*
         * Pone la información de la categoria consultada en los campos correspondientes.
         */
        protected void setDatosConsultados()
        {
            this.inputNombre.Value = categoriaConsultada.Descripcion;
            this.comboBoxEstadosActividades.SelectedValue = Convert.ToString(categoriaConsultada.Estado);
        }

        /*
         * Llena la tabla con las categorias almacenadas en la base de datos.
         */
        protected void llenarGrid()
        {
            DataTable tabla = tablaCategorias();
            int indiceNuevaCategoria = -1;
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[3];
                DataTable categorias = controladoraCategorias.consultarCategorias();

                if (categorias.Rows.Count > 0)
                {
                    idArray = new Object[categorias.Rows.Count,3];
                    foreach (DataRow fila in categorias.Rows)
                    {
                        idArray[i,0] = datos[0] = fila[0].ToString();
                        idArray[i,1] = datos[1] = fila[1].ToString();
                        idArray[i, 2] = datos[2] = fila[2];
                        Object[] datos2 = new Object[2] { datos[1], Convert.ToInt16(datos[2].ToString()) == 1 ? "Activo" : "Inactivo" };
                        tabla.Rows.Add(datos2);
                        

                        if (categoriaConsultada != null && (fila[0].Equals(categoriaConsultada.Nombre)))
                        {
                            indiceNuevaCategoria = i;
                        }
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridViewCategorias.DataSource = tabla;
                this.gridViewCategorias.DataBind();
    
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta:", "No hay conexión a la base de datos.");
            }
        }

        /*
         * Metodo para el evento de click de boton aceptar.
         */
        protected void botonAceptarCategoria_ServerClick(object sender, EventArgs e)
        {
            String codigoModificado ="";
            String codigoInsertado = "";
            bool operacionCorrecta = true;

            if (modo == 2)
            {
                codigoInsertado = insertar();

                if (codigoInsertado == "success")
                {
                    modo = (int)Modo.Consultado;
                    comboBoxEstadosActividades.SelectedValue = Convert.ToString(categoriaConsultada.Estado);
                    seConsulto = true;
                    mostrarMensaje("success", "Éxito:", "Categoría agregada al sistema.");
                    cargarEstados();

                    llenarGrid();
                    irAModo();

                }
                if(codigoInsertado == "repetido"){
                    mostrarMensaje("warning", "Error:", "La categoría insertada ya existe en el sistema.");
                    operacionCorrecta = false;
                }
                if (codigoInsertado == "")
                {
                    codigoModificado = "";
                    mostrarMensaje("Warning", "Error:", "Categoría no agregada, intente nuevamente.");
                    operacionCorrecta = false;
                }
            }
            else if (modo == 3)
            {
                codigoModificado = modificar();

                if (codigoModificado == "success")
                {
                    modo = (int)Modo.Consultado;
                    seConsulto = true;
                    mostrarMensaje("success", "Éxito:", "Categoria modificada en el sistema.");
                    cargarEstados();
                    setDatosConsultados();

                }
                if (codigoModificado == "repetido")
                {
                    mostrarMensaje("warning", "Error:", "La categoría modificada ya existe en el sistema.");
                    operacionCorrecta = false;
                }
                if (codigoModificado == "")
                {
                    codigoModificado = "";
                    mostrarMensaje("Warning", "Error:", "Categoría no modificada, intente nuevamente.");
                    operacionCorrecta = false;
                }
            }
            /*
            if (operacionCorrecta)
            {
                irAModo();
            }*/
        }

        /*
         * Metodo que controla el modificar en 
         */
        private String modificar()
        {
            String res = "";
            String nombre= this.inputNombre.Value;
            String[] datosCat = {nombre,categoriaConsultada.Nombre,comboBoxEstadosActividades.SelectedValue};
            
            //if (!nombreRepetido(nombre))
           // {
                String[] error = controladoraCategorias.modificarDatos(categoriaConsultada, datosCat);
                if (error[0].Contains("success"))// si fue exitoso
                {
                    llenarGrid();
                    res = "success";
                }
                else
                {
                    res = "";
                    modo = (int)Modo.Modificacion;
                }
            //}
            //else
           // {
               // res = "repetido";
            //}
            return res;
         
        }

        /*
         * Metodo que habilita los campos de la interfaz
         */
        private void habilitarCampos(bool p)
        {
            camposCategoria.Disabled = !p;
        }

        /*
         * metodo que controla el insertar en la interfaz
         */
        private string insertar()
        {

            String codigo = "";
            String res = "";
            String nombre = this.inputNombre.Value.ToString();
            String estado = comboBoxEstadosActividades.SelectedValue;
            if (!nombreRepetido(nombre)){
            
                String[] error = controladoraCategorias.insertarDatos(nombre,estado);

                codigo = Convert.ToString(error[2]);
                categoriaConsultada = controladoraCategorias.consultarCategoria(codigo);
                if (error[0].Contains("success"))
                {
                    llenarGrid();
                    res = "success";

                }
                else
                {
                    codigo = "";
                    modo = (int)Modo.Insercion;
                }
            }
            else
                res = "repetido";
            return res;
        }
        /*
         *Metodo que controla el aceptar en el modal
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            limpiarCampos();
            categoriaConsultada = null;
            seConsulto = false;
            irAModo();

        }
        /*
         * Metodo que chequea si el nombre nuevo que se va a ingresar esta repetido.
         */
        private bool nombreRepetido(String nombre)
        {
            for (int i = 0; i < idArray.Length/3; ++i )
            {
                if (String.Equals(idArray[i, 1].ToString(), nombre))
                    return true;
            }
            return false;
        }
        /*
         * Metodo que administra los permisos en la interfaz.
         */
        protected void mostrarBotonesSegunPermisos()
        {
            botonConsultaCategoria.Visible = (permisos[5] == '1');
            botonAgregarCategoria.Visible = (permisos[4] == '1');
            botonModificacionCategoria.Visible = (permisos[3] == '1');
            comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
        }
    }
}