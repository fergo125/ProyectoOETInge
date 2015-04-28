﻿using System;
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
    /*
     * Controla las operaciones en la interfaz y comunica con la controladora.
     */
    public partial class FormActividades : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static int modo = (int)Modo.Inicial;                            // Establece el modo inicial de la pantalla.
        private static Object[] idArray;                                        // Usado en consultas de actividades
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Obtiene datos generales (estados)
        private static EntidadActividad actividadConsultada;                    // Almacena la actividad que se consultó (o acaba de agregar o modificar)
        private static ControladoraActividades controladoraActividades;         // Comunica con la base de datos.
        private static Boolean seConsulto = false;                              // Bandera para saber si hubo consulta de una actividad.
        private static String permisos = "000000";                              // Permisos utilizados para el control de
                                                                                // seguridad.

        /*
         * Maneja las acciones que se ejecutan cuando se carga la página, establecer el modo de operación, 
         * cargar elementos de la interfaz, gestión de seguridad.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            mensajeAlerta.Visible = false;
            if (!IsPostBack)
            {
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraActividades = new ControladoraActividades();
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Gestion de actividades");

                // Esconder botones
                esconderBotonesSegunPermisos();

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
                        setDatosConsultados();
                        seConsulto = false;
                    }
                }

            }
            cambiarModo();
        }

        /*
         * Pide a la controladora consultar la información de una actividad a partir del código de esta.
         */
        protected void consultarActividad(String codigo)
        {
            seConsulto = true;
            try
            {
                actividadConsultada = controladoraActividades.consultarActividad(codigo);
                modo = (int)Modo.Consulta;
            }
            catch
            {
                actividadConsultada = null;
                modo = (int)Modo.Inicial;
            }
            cambiarModo();
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información de las actividades.
         */
        protected DataTable tablaActividades()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Código Interno";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Llena la tabla con las actividades almacenadas en la base de datos.
         */
        protected void llenarGrid()
        {
            DataTable tabla = tablaActividades();
            int indiceNuevaActividad = -1;
            int i = 0;

            try
            {
                // Cargar actividades
                Object[] datos = new Object[3];
                DataTable actividades = controladoraActividades.consultarActividades();

                if (actividades.Rows.Count > 0)
                {
                    idArray = new Object[actividades.Rows.Count];
                    foreach (DataRow fila in actividades.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[0].ToString();
                        if (fila[2].ToString().Equals("0"))
                        {
                            datos[2] = "Inactivo";
                        }
                        else if (fila[2].ToString().Equals("1"))
                        {
                            datos[2] = "Activo";
                        }
                        else
                        {
                            datos[2] = fila[2].ToString();
                        }

                        tabla.Rows.Add(datos);
                        if (actividadConsultada != null && (fila[0].Equals(actividadConsultada.Codigo)))
                        {
                            indiceNuevaActividad = i;
                        }
                        i++;
                    }
                }
                    // No hay actividades almacenadas.
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridViewActividades.DataSource = tabla;
                this.gridViewActividades.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Actividades.");
            }
        }






        // Control de elementos de la interfaz.

        /*
         * Muestra el mensaje que da el resultado de las transacciones que se efectuan.
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        /*
         * Habilita los campos de la interfaz para una inserción/modificación.
         */
        protected void habilitarCampos(bool habilitar)
        {
            this.inputDescripcionActividad.Disabled = !habilitar;
            comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
        }

        /*
         * Remueve la información de los campos de la interfaz.
         */
        protected void limpiarCampos()
        {
            this.inputDescripcionActividad.Value = "";
            this.comboBoxEstadosActividades.SelectedValue = null;
        }

        /*
         * Carga los posibles estados para una actividad en el 'comboBox' establecido para esto.
         */
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
         * Pone la información de la actividad consultada en los campos correspondientes.
         */
        protected void setDatosConsultados()
        {
            this.inputDescripcionActividad.Value = actividadConsultada.Descripcion;
            this.comboBoxEstadosActividades.SelectedValue = actividadConsultada.Estado.ToString();
            this.codigoInternoActividad.Value = actividadConsultada.Codigo;
        }

        /*
         * Cambia el modo de la pantalla activando/desactivando o mostrando/ocultando elementos de acuerdo con la 
         * acción que se va a realizar.
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial:
                    limpiarCampos();
                    habilitarCampos(false);
                    this.FieldsetActividad.Visible = false;
                    this.botonAgregarActividades.Disabled = false;
                    this.botonModificacionActividades.Disabled = true;
                    this.labelTextoObligatorioActividad.Visible = false;
                    this.gridViewActividades.Visible = false;
                    this.botonAceptarActividad.Visible = false;
                    this.botonCancelarActividad.Visible = false;    
                    this.codigoInternoActividad.Visible = false;
                    this.labelCodigoInterno.Visible = false;
                    this.codigoInternoActividad.Disabled = true;
                    this.tituloBienvenidaActividades.Visible = true;
                    break;
                case (int)Modo.Insercion: //insertar
                    habilitarCampos(true);
                    this.botonAgregarActividades.Disabled = true;
                    this.botonModificacionActividades.Disabled = true;
                    this.FieldsetActividad.Visible = true;
                    this.labelTextoObligatorioActividad.Visible = true;
                    this.gridViewActividades.Visible = false;
                    this.botonAceptarActividad.Visible = true;
                    this.botonCancelarActividad.Visible = true;  
                    this.codigoInternoActividad.Visible = false;
                    this.labelCodigoInterno.Visible = false;
                    this.tituloBienvenidaActividades.Visible = false;
                    break;
                case (int)Modo.Modificacion: //modificar
                    habilitarCampos(true);
                    llenarGrid();
                    this.botonAgregarActividades.Disabled = true;
                    this.botonModificacionActividades.Disabled = true;
                    this.FieldsetActividad.Visible = true;
                    this.labelTextoObligatorioActividad.Visible = true;
                    this.gridViewActividades.Visible = true;
                    this.botonAceptarActividad.Visible = true;
                    this.botonCancelarActividad.Visible = true;
                    this.codigoInternoActividad.Visible = false;
                    this.labelCodigoInterno.Visible = false;
                    this.tituloBienvenidaActividades.Visible = false;
                    break;
                case (int)Modo.Consulta://consultar
                    limpiarCampos();
                    habilitarCampos(false);
                    this.botonAgregarActividades.Disabled = false;
                    this.FieldsetActividad.Visible = false;
                    this.labelTextoObligatorioActividad.Visible = false;
                    this.botonAceptarActividad.Visible = false;
                    this.botonCancelarActividad.Visible = false;
                    this.botonModificacionActividades.Disabled = true;
                    this.gridViewActividades.Visible = true;
                    this.tituloBienvenidaActividades.Visible = false;
                    break;
                case (int)Modo.Consultado://consultada una actividad
                    habilitarCampos(false);
                    llenarGrid();
                    this.FieldsetActividad.Visible = true;
                    this.botonAgregarActividades.Disabled = false;
                    this.botonModificacionActividades.Disabled = false;
                    this.labelTextoObligatorioActividad.Visible = false;
                    this.botonAceptarActividad.Visible = false;
                    this.botonCancelarActividad.Visible = false;
                    this.gridViewActividades.Visible = true;
                    this.codigoInternoActividad.Visible = true;
                    this.labelCodigoInterno.Visible = true;
                    this.codigoInternoActividad.Disabled = true;
                    this.tituloBienvenidaActividades.Visible = false;
                    break;
                default:

                    break;
            }
        }





        // Control de eventos que son desencadenados desde la interfaz.

        /*
         * Maneja el evento de presionar el botón para agregar una actividad, cambiando el modo de la pantalla.
         */
        protected void botonAgregarActividades_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            cargarEstados();
        }

        /*
         * Maneja el evento de presionar el botón para modificar una actividad, cambiando el modo de la pantalla.
         */
        protected void botonModificacionActividades_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            cambiarModo();
        }

        /*
         * Regresa la interfaz a modo inicial en caso de darse una cancelación de una acción (inserción o modificación).
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        /*
         * Maneja el evento al presionar el botón de consultas, mostrando la tabla de actividades.
         */
        protected void botonConsultaActividades_ServerClick(object sender, EventArgs e)
        {
            llenarGrid();
            modo = (int)Modo.Consulta;
            cambiarModo();
        }

        /*
         * Responde al evento de confirmar una desactivación.
         */
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {

        }

        /*
         * Inicia la inserción o modificación de una actividad al haberse presionado el botón de Aceptar en el formulario,
         * chequea que no haya repetición en el nombre de la actividad con respecto a alguna almacenada en la base de datos.
         */
        protected void botonAceptarActividad_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";
            String[] resultado = new String[4];

            if (modo == (int)Modo.Insercion)
            {
                String nombreNuevo = this.inputDescripcionActividad.Value.ToString();
                EntidadActividad repetida = controladoraActividades.consultarActividadPorNombre(nombreNuevo);

                if (repetida == null )
                {
                    resultado = controladoraActividades.insertarDatos(nombreNuevo, Int32.Parse(this.comboBoxEstadosActividades.SelectedValue.ToString()));
                    codigoInsertado = resultado[3];

                    if (codigoInsertado != "" && resultado[1].Equals("Éxito"))
                    {
                        operacionCorrecta = true;
                        actividadConsultada = controladoraActividades.consultarActividad(codigoInsertado);
                        modo = (int)Modo.Consultado;
                        habilitarCampos(false);
                        mostrarMensaje(resultado[0], resultado[1], resultado[2]);
                    }
                    else
                        operacionCorrecta = false;

                    setDatosConsultados();
                }
                else
                {
                    mostrarMensaje("warning", "Alerta", "El nombre de la actividad corresponde a una existente, por favor ingrese otro nombre.");
                    operacionCorrecta = false;
                }
            }
            else if (modo == (int)Modo.Modificacion)
            {

                String nombreNuevo = this.inputDescripcionActividad.Value.ToString();
                EntidadActividad repetida = controladoraActividades.consultarActividadPorNombre(nombreNuevo);

                if (repetida == null)
                {
                    resultado = controladoraActividades.modificarDatos(actividadConsultada, nombreNuevo, Int32.Parse(this.comboBoxEstadosActividades.SelectedValue.ToString()));

                    if (resultado[1] == "Éxito")
                    {
                        codigoInsertado = actividadConsultada.Codigo;
                        operacionCorrecta = true;
                        actividadConsultada = controladoraActividades.consultarActividad(codigoInsertado);
                        modo = (int)Modo.Consultado;
                        habilitarCampos(false);
                        mostrarMensaje(resultado[0], resultado[1], resultado[2]);
                    }
                    else
                        operacionCorrecta = false;

                    setDatosConsultados();
                }
                else
                {
                    mostrarMensaje("warning", "Alerta", "El nombre de la actividad corresponde a una existente, por favor ingrese otro nombre.");
                    operacionCorrecta = false;
                }
            }

            if (operacionCorrecta)
            {
                cambiarModo();
            }

        }

        /*
         * Se dispara cuando se selecciona una actividad para consultar su información.
         */
        protected void gridViewActividades_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewActividades.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewActividades.PageIndex * resultadosPorPagina)]);
                    String codigo = filaSeleccionada.Cells[2].Text.ToString();
                    consultarActividad(codigo);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormActividades.aspx");
                    break;
            }
        }

        /*
         * Evento que maneja el cambio de página en la tabla de actividades.
         */
        protected void gridViewActividades_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid();
            this.gridViewActividades.PageIndex = e.NewPageIndex;
            this.gridViewActividades.DataBind();
        }

        protected void esconderBotonesSegunPermisos()
        {
            comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
            botonModificacionActividades.Visible = (permisos[3] == '1');
            botonAgregarActividades.Visible = (permisos[4] == '1');
            botonConsultaActividades.Visible = (permisos[5] == '1');
        }
    }
}