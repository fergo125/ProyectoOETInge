using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Bodegas;
using ProyectoInventarioOET.App_Code;

namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormBodegas : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static int resultadosPorPagina; //wtf?
        private static EntidadBodega bodegaConsultada;                          //???
        private static ControladoraBodegas controladoraBodegas;                 //???
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //???
        private static Boolean seConsulto = false;                              //???
        private static Object[] idArray;                                        //???
        private static int modo = (int)Modo.Inicial;                            //???
        private static bool mensajeMostrado = false; //wtf?
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.


        /*
         * ???
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            mensajeAlerta.Visible = false;
          
            if (!IsPostBack)
            {
                    labelAlerta.Text = "";
               
                controladoraBodegas = new ControladoraBodegas();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                //Seguridad
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Gestion de bodegas");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                mostrarBotonesSegunPermisos();

                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (bodegaConsultada == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la bodega.");
                    }
                    else
                    {
                        cargarEstados();
                        cargarAnfitriones();
                        cargarEstaciones();
                        cargarIntenciones();
                        setDatosConsultados();

                        seConsulto = false;
                    }
                }
            }
            cambiarModo();
        }

        /*
         * ???
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial:
                    limpiarCampos();
                    botonAgregarBodega.Disabled = false;
                    FieldsetBodegas.Visible = false;
                    textoObligatorioBodega.Visible = false;
                    botonModificarBodega.Disabled = true;
                    botonAceptarBodega.Visible = false;
                    botonCancelarBodega.Visible = false;
                    tituloAccionBodegas.InnerText = "Seleccione una opción";
                    textoObligatorioBodega.Visible = false;
                    botonConsultarBodega.Disabled = false;
                    habilitarCampos(false);
                    break;
                case (int)Modo.Insercion: //insertar
                    gridViewBodegas.Visible = false;
                    tituloGrid.Visible = false;
                    FieldsetBodegas.Visible = true;
                    textoObligatorioBodega.Visible = true;
                    habilitarCampos(true);
                    botonAgregarBodega.Disabled = true;
                    botonModificarBodega.Disabled = true;
                    botonConsultarBodega.Disabled = false;
                    botonAceptarBodega.Visible = true;
                    textoObligatorioBodega.Visible = true;
                    tituloAccionBodegas.InnerText = "Ingrese datos";
                    botonCancelarBodega.Visible = true;
                    break;
                case (int)Modo.Modificacion: //modificar
                    gridViewBodegas.Visible = false;
                    tituloGrid.Visible = false;
                    FieldsetBodegas.Visible = true;
                    textoObligatorioBodega.Visible = true;
                    habilitarCampos(true);
                    llenarGrid();
                    botonAgregarBodega.Disabled = true;
                    botonModificarBodega.Disabled = true;
                    botonConsultarBodega.Disabled = false;
                    botonAceptarBodega.Visible = true;
                    textoObligatorioBodega.Visible = true;
                    tituloAccionBodegas.InnerText = "Cambie los datos";
                    botonCancelarBodega.Visible = true;

                    break;
                case (int)Modo.Consulta://consultar
                    gridViewBodegas.Visible = true;
                    tituloGrid.Visible = true;
                    FieldsetBodegas.Visible = false;
                    textoObligatorioBodega.Visible = false;
                    botonAgregarBodega.Disabled = false;
                    botonConsultarBodega.Disabled = true;
                    botonAceptarBodega.Visible = false;
                    tituloAccionBodegas.InnerText = "Seleccione una bodega";
                    textoObligatorioBodega.Visible = false;
                    botonCancelarBodega.Visible = false;
                    habilitarCampos(false);
                    break;
                case (int)Modo.Consultado://consultado, pero con los espacios bloqueados
                    gridViewBodegas.Visible = true;
                    tituloGrid.Visible = true;
                    FieldsetBodegas.Visible = true;
                    textoObligatorioBodega.Visible = true;
                    botonAgregarBodega.Disabled = false;
                    botonModificarBodega.Disabled = false;
                    botonConsultarBodega.Disabled = false;
                    botonAceptarBodega.Visible = false;
                    tituloAccionBodegas.InnerText = "Bodega seleccionada";
                    textoObligatorioBodega.Visible = false;
                    botonCancelarBodega.Visible = false;
                    habilitarCampos(false);
                    llenarGrid();
                    break;
                default:
                    // Algo salio mal
                    break;
            }
        }

        /*
         * 
         */
        protected void mostrarBotonesSegunPermisos()
        {
            botonConsultarBodega.Visible = (permisos[5] == '1');
            botonAgregarBodega.Visible = (permisos[4] == '1');
            botonModificarBodega.Visible = (permisos[3] == '1');
            dropdownEstado.Enabled = (permisos[2] == '1');
        }

        /*
         * ???
         */
        protected void gridViewBodegas_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName) 
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewBodegas.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * this.gridViewBodegas.PageSize)]);
                    consultarBodega(codigo);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormBodegas.aspx");
                    break;
            }
        }

        /*
         * ???
         */
        protected void gridViewBodegas_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid();
            this.gridViewBodegas.PageIndex = e.NewPageIndex;
            this.gridViewBodegas.DataBind();
        }

        /*
         * ???
         */
        protected void llenarGrid()
        {
                DataTable tabla = tablaBodegas();
                int indiceNuevaBodega = -1;
                int i = 0;

                try
                {
                    // Cargar bodegas
                    Object[] datos = new Object[4];
                    DataTable bodegas = controladoraBodegas.consultarBodegas();

                    if (bodegas.Rows.Count > 0)
                    {
                        idArray = new Object[bodegas.Rows.Count];
                        foreach (DataRow fila in bodegas.Rows)
                        {
                            idArray[i] = fila[0];
                            datos[0] = fila[1].ToString();
                            datos[1] = fila[3].ToString();
                            datos[2] = fila[4].ToString();
                            datos[3] = fila[5].ToString();
                            tabla.Rows.Add(datos);
                            if (bodegaConsultada != null && (fila[0].Equals(bodegaConsultada.Codigo)))
                            {
                                indiceNuevaBodega = i;
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

                    this.gridViewBodegas.DataSource = tabla;
                    this.gridViewBodegas.DataBind();
                }
                catch (Exception e)
                {
                    mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
                }
        }

        /*
         * ???
         */
        protected void vaciarGridBodegas()
        {
            DataTable tablaLimpia = null;
            gridViewBodegas.DataSource = tablaLimpia;
            gridViewBodegas.DataBind();
        }

        /*
         * ???
         */
        protected DataTable tablaBodegas()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estación";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Intención de uso";
            tabla.Columns.Add(columna);

            return tabla;
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
        protected void botonAceptarBodega_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";

            if (modo == (int)Modo.Insercion)
            {
                codigoInsertado = insertar();

                if (codigoInsertado != "")
                {
                    operacionCorrecta = true;
                    bodegaConsultada = controladoraBodegas.consultarBodega(codigoInsertado);
                    modo = (int)Modo.Consultado;
                    habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == (int)Modo.Modificacion)
            {
                operacionCorrecta = modificar();
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }

        /*
         * ???
         */
        protected String insertar()
        {
            String codigo = "";
            Object[] bodega = obtenerDatosBodega();

            String[] error = controladoraBodegas.insertarDatos(bodega);

            codigo = Convert.ToString(error[3]);
            mostrarMensaje(error[0], error[1], error[2]);
            if (error[0].Contains("success"))
            {
                llenarGrid();
            }
            else
            {
                codigo = "";
                modo = (int)Modo.Insercion;
            }

            return codigo;
        }

        /*
         * ???
         */
        protected Boolean modificar()
        {
            Boolean res = true;

            Object[] bodega = obtenerDatosBodega();
            String id = bodegaConsultada.Codigo;
            bodega[0] = id;
            String[] error = controladoraBodegas.modificarDatos(bodegaConsultada, bodega);
            mostrarMensaje(error[0], error[1], error[2]);

            if (error[0].Contains("success"))// si fue exitoso
            {
                llenarGrid();
                bodegaConsultada = controladoraBodegas.consultarBodega(bodegaConsultada.Codigo);
                modo = (int)Modo.Consulta;
            }
            else
            {
                res = false;
                modo = (int)Modo.Modificacion;
            }
            return res;
        }

        /*
         * ???
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            vaciarGridBodegas();
            modo = (int)Modo.Inicial;
            cambiarModo();
            limpiarCampos();
            bodegaConsultada = null;
        }

        /*
         * ???
         */
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {
        }

        /*
         * ???
         */
        protected void botonConsultarBodega_consultarBodegas(object sender, EventArgs e)
        {
            llenarGrid();
            modo = (int)Modo.Consulta;
            cambiarModo();
        }

        /*
         * ???
         */
        protected void consultarBodega(String id)
        {
            seConsulto = true;
            try
            {
                bodegaConsultada = controladoraBodegas.consultarBodega(id);
                modo = (int)Modo.Consulta;
            }
            catch
            {
                bodegaConsultada = null;
                modo = (int)Modo.Inicial;
            }
            cambiarModo();
        }

        /*
         * ???
         */
        protected Object[] obtenerDatosBodega()
        {
            Object[] datos = new Object[6];
            datos[0] = 0;
            datos[1] = this.inputNombre.Value;
            datos[2] = this.comboBoxEmpresa.SelectedValue;
            datos[3] = this.comboBoxEstacion.SelectedValue;
            datos[4] = this.dropdownEstado.SelectedValue;
            datos[5] = this.comboBoxIntencion.SelectedValue;
            return datos;
        }

        /*
         * ???
         */
        protected void cargarEstados()
        {
            dropdownEstado.Items.Clear();
            dropdownEstado.Items.Add(new ListItem("", null));
            DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
            foreach (DataRow fila in estados.Rows)
            {
                dropdownEstado.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }

        /*
         * ???
         */
        protected void cargarIntenciones()
        {
            comboBoxIntencion.Items.Clear();
            comboBoxIntencion.Items.Add(new ListItem("", null));
            DataTable intenciones = controladoraDatosGenerales.consultarIntenciones();
            foreach (DataRow fila in intenciones.Rows)
            {
                comboBoxIntencion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        /*
         * ???
         */
        protected void cargarAnfitriones()
        {
            comboBoxEmpresa.Items.Clear();
            comboBoxEmpresa.Items.Add(new ListItem("", null));
            DataTable anfitriones = controladoraDatosGenerales.consultarAnfitriones();
            foreach (DataRow fila in anfitriones.Rows)
            {
                comboBoxEmpresa.Items.Add(new ListItem(fila[2].ToString(), fila[0].ToString()));
            }
        }

        /*
         * ???
         */
        protected void cargarEstaciones()
        {
            comboBoxEstacion.Items.Clear();
            comboBoxEstacion.Items.Add(new ListItem("", null));
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            foreach (DataRow fila in estaciones.Rows)
            {
                comboBoxEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        /*
         * ???
         */
        protected void setDatosConsultados()
        {
            this.inputNombre.Value = bodegaConsultada.Nombre;
            this.comboBoxEstacion.SelectedValue = bodegaConsultada.Estacion;
            this.comboBoxEmpresa.SelectedValue = bodegaConsultada.Anfitriona;
            this.dropdownEstado.SelectedValue = Convert.ToString(bodegaConsultada.Estado);
            this.comboBoxIntencion.SelectedValue = Convert.ToString(bodegaConsultada.IntencionUso);
        }

        /*
         * ???
         */
        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
            this.comboBoxEstacion.SelectedValue = null;
            this.comboBoxEmpresa.SelectedValue = null;
            this.dropdownEstado.SelectedValue = null;
            this.comboBoxIntencion.SelectedValue = null;
        }

        /*
         * ???
         */
        protected void habilitarCampos(bool habilitar)
        {
            this.inputNombre.Disabled = !habilitar;
            this.comboBoxEmpresa.Enabled = habilitar;
            this.comboBoxEstacion.Enabled = habilitar;
            this.dropdownEstado.Enabled = habilitar && (permisos[2] == '1');
            this.comboBoxIntencion.Enabled = habilitar;
        }

        /*
         * ???
         */
        protected void botonAgregarBodega_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            cargarEstados();
            cargarAnfitriones();
            cargarEstaciones();
            cargarIntenciones();
        }

        /*
         * ???
         */
        protected void botonModificarBodega_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            cambiarModo();
        }
    }
}