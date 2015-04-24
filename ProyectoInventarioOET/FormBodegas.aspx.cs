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
    public partial class FormBodegas : System.Web.UI.Page
    {

        private static int resultadosPorPagina;
        private static EntidadBodega bodegaConsultada;
        private static ControladoraBodegas controladoraBodegas;
        private static ControladoraBDEstados controladoraEstados;
        private static ControladoraBDEstaciones controladoraEstaciones;
        private static ControladoraBDAnfitrionas controladoraAnfitriones;
        private static Boolean seConsulto = false;
        private static Object[] idArray;
        private static int modo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                    controladoraBodegas = new ControladoraBodegas();
                    controladoraEstados = new ControladoraBDEstados();
                    controladoraAnfitriones = new ControladoraBDAnfitrionas();
                    controladoraEstaciones = new ControladoraBDEstaciones();

                    if (!seConsulto)
                    {
                        modo = 0;
                    }
                    else{
                        if (bodegaConsultada == null)
                        {
                            mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la bodega.");
                        }
                        else
                        {
                            cargarEstados();
                            cargarAnfitriones();
                            cargarEstaciones();
                            setDatosConsultados();

                            seConsulto = false;
                        }
                    }
            }
            cambiarModo();
            
        }

        protected void cambiarModo()
        {
            switch (modo)
            {
                case 0:
                    limpiarCampos();
                    botonAgregarBodega.Disabled = false;
                    botonModificarBodega.Disabled = true;
                    habilitarCampos(false);
                    break;
                case 1: //insertar
                    habilitarCampos(true);
                    botonAgregarBodega.Disabled = true;
                    botonModificarBodega.Disabled = true;
                    break;
                case 2: //modificar
                    habilitarCampos(true);
                    botonAgregarBodega.Disabled = true;
                    botonModificarBodega.Disabled = true;

                    break;
                case 3://consultar
                    habilitarCampos(false);
                    break;

                default:
                    // Algo salio mal
                    break;
            }
        }


        protected void gridViewBodegas_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName) 
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewBodegas.Rows[Convert.ToInt32(e.CommandArgument)];
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * resultadosPorPagina)]);
                    consultarBodega(codigo);
                    Response.Redirect("FormBodegas.aspx");
                    break;
            }
        }

        protected void gridViewBodegas_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            this.gridViewBodegas.PageIndex = e.NewPageIndex;
            this.gridViewBodegas.DataBind();
        }

        protected void testGrid()
        {

            DataTable tabla = tablaBodegas();
            DataTable tabla2 = tablaCatalogoLocal();

            for (int i = 1; i < 5; i++)
            {
                Object[] datos = new Object[2];
                datos[0] = i * 2;
                datos[1] = i * 3;
                tabla.Rows.Add(datos);
            }

            for (int i = 1; i < 5; i++)
            {
                Object[] datos2 = new Object[5];
                datos2[0] = i * 2;
                datos2[1] = i * 3;
                datos2[2] = i * 4;
                datos2[3] = i * 5;
                datos2[4] = i * 6;
                tabla2.Rows.Add(datos2);
            }

            this.gridViewBodegas.DataSource = tabla;
            this.gridViewBodegas.DataBind();
        
        
        }


        protected void llenarGrid()
        {
                DataTable tabla = tablaBodegas();
                int indiceNuevaBodega = -1;
                int i = 0;

                try
                {
                    // Cargar bodegas
                    Object[] datos = new Object[2];
                    DataTable bodegas = controladoraBodegas.consultarBodegas();

                    if (bodegas.Rows.Count > 0)
                    {
                        idArray = new Object[bodegas.Rows.Count];
                        foreach (DataRow fila in bodegas.Rows)
                        {
                            idArray[i] = fila[0];
                            datos[0] = fila[1].ToString();
                            datos[1] = fila[3].ToString();
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
                        tabla.Rows.Add(datos);
                    }

                    this.gridViewBodegas.DataSource = tabla;
                    this.gridViewBodegas.DataBind();
                    if (bodegaConsultada != null)
                    {
                        GridViewRow filaSeleccionada = this.gridViewBodegas.Rows[indiceNuevaBodega];
                    }
                }

                catch (Exception e)
                {
                    mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
                }
        }

        protected void vaciarGridBodegas()
        {
            DataTable tablaLimpia = null;
            gridViewBodegas.DataSource = tablaLimpia;
            gridViewBodegas.DataBind();
        }



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

            return tabla;
        }

        protected DataTable tablaCatalogoLocal()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Precio";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Cantidad";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Mínimo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Máximo";
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

        protected void gridViewCatalogoLocal_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewBodegas.Rows[Convert.ToInt32(e.CommandArgument)];
                    int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * resultadosPorPagina)]);
                    break;
            }
        }

        protected void botonAceptarBodega_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";

            if (modo == 1)
            {
                codigoInsertado = insertar();

                if (codigoInsertado != "")
                {
                    operacionCorrecta = true;
                    bodegaConsultada = controladoraBodegas.consultarBodega(codigoInsertado);
                    modo = 3;
                    habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == 2)
            {
                operacionCorrecta = modificar();
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }

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
                modo = 1;
            }

            return codigo;
        }

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
                modo = 3;
            }
            else
            {
                res = false;
                modo = 2;
            }


            return res;
        }

        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            vaciarGridBodegas();
            modo = 0;
            cambiarModo();
            limpiarCampos();;
            bodegaConsultada = null;
        }

        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {
        }

        protected void botonConsultarBodega_consultarBodegas(object sender, EventArgs e)
        {
            llenarGrid();
        }


        protected void consultarBodega(String id)
        {
            seConsulto = true;
            try
            {
                bodegaConsultada = controladoraBodegas.consultarBodega(id);
                modo = 3;
            }
            catch
            {
                bodegaConsultada = null;
                modo = 0;
            }
            cambiarModo();
        }

        protected Object[] obtenerDatosBodega()
        {
            Object[] datos = new Object[5];
            datos[0] = 0;
            datos[1] = this.inputNombre.Value;
            datos[2] = this.comboBoxEmpresa.SelectedValue;
            datos[3] = this.comboBoxEstacion.SelectedValue;
            datos[4] = this.dropdownEstado.SelectedValue;
            return datos;
        }

        protected void cargarEstados()
        {
            dropdownEstado.Items.Clear();
            dropdownEstado.Items.Add(new ListItem("", null));
            DataTable estados = controladoraEstados.consultarEstados();
            foreach (DataRow fila in estados.Rows)
            {
                dropdownEstado.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }

        protected void cargarAnfitriones()
        {
            comboBoxEmpresa.Items.Clear();
            comboBoxEmpresa.Items.Add(new ListItem("", null));
            DataTable anfitriones = controladoraAnfitriones.consultarAnfitriones();
            foreach (DataRow fila in anfitriones.Rows)
            {
                comboBoxEmpresa.Items.Add(new ListItem(fila[2].ToString(), fila[0].ToString()));
            }
        }

        protected void cargarEstaciones()
        {
            comboBoxEstacion.Items.Clear();
            comboBoxEstacion.Items.Add(new ListItem("", null));
            DataTable estaciones = controladoraEstaciones.consultarEstaciones();
            foreach (DataRow fila in estaciones.Rows)
            {
                comboBoxEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        protected void setDatosConsultados()
        {
            this.inputNombre.Value = bodegaConsultada.Nombre;
            this.comboBoxEstacion.SelectedValue = bodegaConsultada.Estacion;
            this.comboBoxEmpresa.SelectedValue = bodegaConsultada.Anfitriona;
            this.dropdownEstado.SelectedValue = Convert.ToString(bodegaConsultada.Estado);
        }

        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
            this.comboBoxEstacion.SelectedValue = null;
            this.comboBoxEmpresa.SelectedValue = null;
            this.dropdownEstado.SelectedValue = null;
        }

        protected void habilitarCampos(bool habilitar)
        {
            this.inputNombre.Disabled = !habilitar;
            this.comboBoxEmpresa.Enabled = habilitar;
            this.comboBoxEstacion.Enabled = habilitar;
            this.dropdownEstado.Enabled = habilitar;
        }

        protected void botonAgregarBodega_ServerClick(object sender, EventArgs e)
        {
            modo = 1;
            cambiarModo();
            limpiarCampos();
            cargarEstados();
            cargarAnfitriones();
            cargarEstaciones();
        }

        protected void botonModificarBodega_ServerClick(object sender, EventArgs e)
        {
            modo = 2;
            cambiarModo();
        }

        

    }
}