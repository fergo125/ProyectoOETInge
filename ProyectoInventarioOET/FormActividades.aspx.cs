using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Actividades;
using ProyectoInventarioOET.App_Code;


namespace ProyectoInventarioOET
{
    public partial class FormActividades : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion };
        private int modo;
        private static int resultadosPorPagina;
        private static Object[] idArray;
        private static ControladoraDatosGenerales controladoraDatosGenerales;
        private static EntidadActividad actividadConsultada;
        private static ControladoraActividades controladoraActividades;
        private static Boolean seConsulto = false;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraActividades = new ControladoraActividades();

                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (actividadConsultada == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la actividad.");
                    }
                    else
                    {
                        cargarEstados();
                        //cargarAnfitriones();
                        //cargarEstaciones();
                        //setDatosConsultados();

                        seConsulto = false;
                    }
                }
            }
            cambiarModo();

        }

        protected void gridViewActividades_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewActividades.Rows[Convert.ToInt32(e.CommandArgument)];
                    int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewActividades.PageIndex * resultadosPorPagina)]);
                    break;
            }
        }

        protected void gridViewActividades_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            this.gridViewActividades.PageIndex = e.NewPageIndex;
            this.gridViewActividades.DataBind();
        }

        protected void testGrid()
        {

            DataTable tabla = tablaActividades();

            for (int i = 1; i < 5; i++)
            {
                Object[] datos = new Object[3];
                datos[0] = i * 2;
                datos[1] = i * 3;
                datos[2] = i * 4;
                tabla.Rows.Add(datos);
            }


            this.gridViewActividades.DataSource = tabla;
            this.gridViewActividades.DataBind();


        }


        protected void llenarGrid()
        {
            DataTable tabla = tablaActividades();
            int indiceNuevaActividad = -1;
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[3];
                //DataTable bodegas = controladoraActividades.consultarActividades();

               /* if (bodegas.Rows.Count > 0)
                {
                    idArray = new Object[bodegas.Rows.Count];
                    foreach (DataRow fila in bodegas.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[2].ToString();                       
                        tabla.Rows.Add(datos);
                        /*if (bodegaConsultada != null && (fila[0].Equals(bodegaConsultada.Identificador)))
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
                    tabla.Rows.Add(datos);
                }
    */
                this.gridViewActividades.DataSource = tabla;
                this.gridViewActividades.DataBind();
                /* if (bodegaConsultada != null)
                 {
                     GridViewRow filaSeleccionada = this.gridViewProyecto.Rows[indiceNuevoProyecto];
                 }*/
            }

            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }


        protected DataTable tablaActividades()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Descripción";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            return tabla;
        }

        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Attributes.Remove("hidden");
        }

        protected void botonAceptarActividad_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";
            String[] resultado = new String[4];

            if (modo == (int)Modo.Insercion)
            {
                resultado = controladoraActividades.insertarDatos("LOQUESEA", inputDescripcionActividad.Value, Int32.Parse(this.comboBoxEstadosActividades.SelectedValue));
                codigoInsertado = resultado[3];

                if (codigoInsertado != "")
                {
                    operacionCorrecta = true;
                    actividadConsultada = controladoraActividades.consultarActividad(codigoInsertado);
                    modo = (int)Modo.Consulta;
                    habilitarCampos(false);
                    mostrarMensaje(resultado[0], resultado[1], resultado[2]);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == (int)Modo.Modificacion)
            {
                //operacionCorrecta = modificar();
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }

        }

        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {

        }

        protected void botonConsultaActividades_ServerClick(object sender, EventArgs e)
        {

        }

        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {

        }

        protected void habilitarCampos(bool habilitar)
        {
            this.inputDescripcionActividad.Disabled = !habilitar;
            this.comboBoxEstadosActividades.Enabled = habilitar;
        }

        protected void limpiarCampos()
        {
            this.inputDescripcionActividad.Value = "";
            this.comboBoxEstadosActividades.SelectedValue = null;
        }

        protected void cargarEstados()
        {
            comboBoxEstadosActividades.Items.Clear();
            comboBoxEstadosActividades.Items.Add(new ListItem("", null));
            DataTable estados = controladoraDatosGenerales.consultarEstados();
            foreach (DataRow fila in estados.Rows)
            {
                comboBoxEstadosActividades.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }


        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial:
                    limpiarCampos();
                    botonAgregarActividades.Disabled = false;
                    botonModificacionActividades.Disabled = true;
                    habilitarCampos(false);
                    break;
                case (int)Modo.Insercion: //insertar
                    habilitarCampos(true);
                    botonAgregarActividades.Disabled = true;
                    botonModificacionActividades.Disabled = true;
                    break;
                case (int)Modo.Modificacion: //modificar
                    habilitarCampos(true);
                    botonAgregarActividades.Disabled = true;
                    botonModificacionActividades.Disabled = true;

                    break;
                case (int)Modo.Consulta://consultar
                    habilitarCampos(false);
                    break;

                default:

                    break;
            }
        }

        protected void botonAgregarActividades_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            cargarEstados();
        }

        protected String insertar()
        {
            //String codigo = "";
            //Object[] bodega = obtenerDatosBodega();

            //String[] error = controladoraBodegas.insertarDatos(bodega);

            //codigo = Convert.ToString(error[3]);
            //mostrarMensaje(error[0], error[1], error[2]);
            //if (error[0].Contains("success"))
            //{
            //    llenarGrid();
            //}
            //else
            //{
            //    codigo = "";
            //    modo = 1;
            //}

            return "hola";
        }

    }
}