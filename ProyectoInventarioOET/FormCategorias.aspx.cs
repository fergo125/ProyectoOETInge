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
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static int modo = (int)Modo.Inicial;                    //???
        private static int resultadosPorPagina; //wtf?
        private static Object[,] idArray;                              //???
        private static ControladoraCategorias controladoraCategorias;   //???
        private static EntidadCategoria categoriaConsultada;            //???
        private static bool seConsulto = false;                         //???
        private static ControladoraDatosGenerales controladoraDatosGenerales;
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.

        /*
         * ???
         */
        protected void Page_Load(object sender, EventArgs e)
        {
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
         * ???
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
                comboBoxEstadosActividades.Enabled = false;
                cargarEstados();


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
                comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
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
                comboBoxEstadosActividades.Enabled = true;
                comboBoxEstadosActividades.Visible = true;
                cargarEstados();

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
                inputNombre.Visible = false;
                comboBoxEstadosActividades.Visible = false;
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
                comboBoxEstadosActividades.Enabled = false;

            }
            //aplicarPermisos();// se aplican los permisos del usuario para el acceso a funcionalidades
        }

        /*
         * ???
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
         * ???
         */
        protected void botonRedireccionProductos_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("FormProductos.aspx");
        }

        /*
         * ???
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
         * ???
         */
        protected void gridViewCategorias_CambioPagina(object sender, GridViewPageEventArgs e)
        {
            llenarGrid();
            this.gridViewCategorias.PageIndex = e.NewPageIndex;
            this.gridViewCategorias.DataBind();
        }

        /*
         * ???
         */
        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
            this.comboBoxEstadosActividades.SelectedIndex = 0;
        }

        /*
         * ???
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
         * ???
         */
        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
            deshabilitarCampos();
        }

        /*
         * ???
         */
        protected void botonAgregarCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            limpiarCampos();
            llenarGrid();
            irAModo();
        }

        /*
         * ???
         */
        protected void botonModificacionCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            irAModo();
        }

        /*
         * ???
         */
        protected void botonConsultaCategoria_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Consulta;
            //limpiarCampos();
            llenarGrid();
            irAModo();
        }

        /*
         * ???
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        /*
         * ???
         */
        protected void setDatosConsultados()
        {
            this.inputNombre.Value = categoriaConsultada.Descripcion;
            this.comboBoxEstadosActividades.SelectedValue = Convert.ToString(categoriaConsultada.Estado);
        }

        /*
         * ???
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
                        Object[] datos2 = new Object[2] { datos[1], Convert.ToInt16(datos[2].ToString()) == 1 ? "Activo" : "inactivo" };
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
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * ???
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
                    //comboBoxEstadosActividades.SelectedValue = categoriaConsultada.Estado;
                    seConsulto = true;
                    mostrarMensaje("success", "Exito", "Categoria Agregada");
                    cargarEstados();
                    setDatosConsultados();

                }
                if(codigoInsertado == "repetido"){
                    mostrarMensaje("warning", "Alerta", "La categoria insertada ya existe");
                    operacionCorrecta = false;
                }
                if (codigoInsertado == "")
                {
                    codigoModificado = "";
                    mostrarMensaje("Warning", "Alerta", "No se pudo agregar la categoria");
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
                    mostrarMensaje("success", "Exito", "Categoria Modificada");
                    cargarEstados();
                    setDatosConsultados();

                }
                if (codigoModificado == "repetido")
                {
                    mostrarMensaje("warning", "Alerta", "La categoria modificada ya existe");
                    operacionCorrecta = false;
                }
                if (codigoModificado == "")
                {
                    codigoModificado = "";
                    mostrarMensaje("Warning", "Alerta", "No se pudo modficar");
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
         * ???
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
         * ???
         */
        private void habilitarCampos(bool p)
        {
            camposCategoria.Disabled = !p;
        }

        /*
         * ???
         */
        private string insertar()
        {

            String codigo = "";
            String res = "";
            String nombre = this.inputNombre.Value.ToString();
            if (!nombreRepetido(nombre))
            {
                String[] error = controladoraCategorias.insertarDatos(nombre);

                codigo = Convert.ToString(error[3]);
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
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            limpiarCampos();
            categoriaConsultada = null;
            irAModo();
        }
        private bool nombreRepetido(String nombre)
        {
            for (int i = 0; i < idArray.Length/3; ++i )
            {
                if (String.Equals(idArray[i, 1].ToString(), nombre))
                    return true;
            }
            return false;
        }

        protected void mostrarBotonesSegunPermisos()
        {
            botonConsultaCategoria.Visible = (permisos[5] == '1');
            botonAgregarCategoria.Visible = (permisos[4] == '1');
            botonModificacionCategoria.Visible = (permisos[3] == '1');
            comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
        }
    }
}