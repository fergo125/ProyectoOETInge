using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Entradas;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.Modulo_Seguridad;


namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormEntradas : System.Web.UI.Page
    {
        enum Modo { Inicial, BusquedaFactura, SeleccionFactura, SeleccionProductos,  EntradaConsultada, SeleccionEntrada }; // Sirve para controlar los modos de la interfaz
        //Atributos

        private static int modo = (int)Modo.Inicial;                            // Almacena el modo actual de la interfaz
        private static ControladoraEntradas controladoraEntradas;               // Comunica con la base de datos.
        private static Object[] idArrayFactura;                                        // Almacena identificadores de entradas
        private static Object[] idArrayEntrada; 
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Obtiene datos generales (estados)
        private static EntidadEntrada entradaConsultada;                    // Almacena la entrada que se consultó (o acaba de agregar)
        private static Boolean seConsulto = false;                              // Bandera para saber si hubo consulta de una actividad.
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static String bodegaDeTrabajo;
        private static String facturaBuscada;
        private static EntidadFactura facturaConsultada;
        private static ControladoraSeguridad controladoraSeguridad;


        protected void Page_Load(object sender, EventArgs e)
        {
            controladoraEntradas = new ControladoraEntradas();
            //bodegaDeTrabajo = (this.Master as SiteMaster).LlaveBodegaSesion;

            //llenarGrid();

            mensajeAlerta.Visible = false;
            if (!IsPostBack)
            {
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraEntradas = new ControladoraEntradas();
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Gestion de entradas");
                if (permisos == "000000")
                    //Response.Redirect("~/ErrorPages/404.html");

                // Esconder botones
                mostrarBotonesSegunPermisos();

                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (entradaConsultada == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la actividad.");
                    }
                    else
                    {
                        seConsulto = false;
                        CompletarDatosFactura(facturaConsultada);
                    }
                }

            }
            cambiarModo();
        }

        protected void mostrarBotonesSegunPermisos()
        {
            //botonConsultaActividades.Visible = (permisos[5] == '1');
            //botonAgregarActividades.Visible = (permisos[4] == '1');
            //botonModificacionActividades.Visible = (permisos[3] == '1');
            //comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información de las actividades.
         */
        protected DataTable tablaEntradas()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Entrada";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Factura";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Responsable";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Fecha de Realización";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
 * Construye la tabla que se va a utilizar para mostrar la información de las Facturas.
 */

        protected DataTable tablaFacturas()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Factura";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Orden de Compra";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Fecha de Pago";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Tipo de Pago";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información del detalle de las Facturas entrantes.
         */
        protected DataTable tablaFacturaDetallada()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Producto";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Cantidad";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Costo Unitario";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información de como se va construyendo la factura según los artículos recibidos.
         */
        protected DataTable tablaResultadosBusqueda()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);
            //columna = new DataColumn();
            //columna.DataType = System.Type.GetType("System.String");
            //columna.ColumnName = "Código Interno";
            //tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Llena la tabla con las actividades almacenadas en la base de datos.
         */
        protected void llenarGridEntradas()
        {
            DataTable tabla = tablaEntradas();
            int indiceNuevaActividad = -1;
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[4];
                bodegaDeTrabajo = "CRO44452";
                DataTable entradas = controladoraEntradas.consultarEntradas(bodegaDeTrabajo);

                if (entradas.Rows.Count > 0)
                {
                    idArrayEntrada = new Object[entradas.Rows.Count];
                    foreach (DataRow fila in entradas.Rows)
                    {
                        idArrayEntrada[i] = fila[0];
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = fila[2].ToString();
                        datos[3] = fila[4].ToString();


                        tabla.Rows.Add(datos);
                        //if (entradaConsultada != null && (fila[0].Equals(entradaConsultada.Codigo)))
                        //{
                        //    indiceNuevaActividad = i;
                        //}
                        i++;
                    }
                }
                // No hay entradas almacenadas.
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = "-";
                    mostrarMensaje("warning", "Alerta", "No hay entradas almacenadas.");
                    tabla.Rows.Add(datos);
                }

                this.gridViewEntradas.DataSource = tabla;
                this.gridViewEntradas.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Entradas.");
            }
        }

        /*
         * Llena la tabla con las actividades almacenadas en la base de datos.
         */
        protected void llenarGridFacturas()
        {
            DataTable tabla = tablaFacturas();
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[4];
                DataTable facturas = controladoraEntradas.buscarFacturas(facturaBuscada);

                if (facturas.Rows.Count > 0)
                {
                    idArrayFactura = new Object[facturas.Rows.Count];
                    foreach (DataRow fila in facturas.Rows)
                    {
                        idArrayFactura[i] = fila[0];
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = fila[2].ToString();
                        datos[3] = fila[4].ToString();


                        tabla.Rows.Add(datos);
                        //if (entradaConsultada != null && (fila[0].Equals(entradaConsultada.Codigo)))
                        //{
                        //    indiceNuevaActividad = i;
                        //}
                        i++;
                    }
                }
                // No hay entradas almacenadas.
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = "-";
                    mostrarMensaje("warning", "Alerta", "No hay Facturas disponibles en este momento.");
                    tabla.Rows.Add(datos);
                }

                this.gridViewFacturas.DataSource = tabla;
                this.gridViewFacturas.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Facturas.");
            }
        }

        /*
         * Llena la tabla con las actividades almacenadas en la base de datos.
         */
        protected void llenarGrid()
        {
            DataTable tabla = tablaFacturas();
            int indiceNuevaActividad = -1;
            //int i = 0;

            try
            {
                // Cargar actividades
                //Object[] datos = new Object[3];
                Object[] datos = new Object[4];
                //DataTable actividades = controladoraActividades.consultarActividades();
                //idArray = new Object[actividades.Rows.Count];
                for (int i = 0; i < 3; i++)
                {
                    //idArray[i] = fila[0];
                    datos[0] = 55;
                    datos[1] = 55;
                    datos[2] = 55;
                    datos[3] = 55;
                    //datos[1] = fila[0].ToString();
                    //if (fila[2].ToString().Equals("0"))
                    //{
                    //    datos[2] = "Inactivo";
                    //}
                    //else if (fila[2].ToString().Equals("1"))
                    //{
                    //    datos[2] = "Activo";
                    //}
                    //else
                    //{
                    //    datos[2] = fila[2].ToString();
                    //}
                    //if (fila[2].ToString().Equals("0"))
                    //{
                    //    datos[1] = "Inactivo";
                    //}
                    //else if (fila[2].ToString().Equals("1"))
                    //{
                    //    datos[1] = "Activo";
                    //}
                    //else
                    //{
                    //    datos[1] = fila[2].ToString();
                    //}

                    tabla.Rows.Add(datos);
                    //if (actividadConsultada != null && (fila[0].Equals(actividadConsultada.Codigo)))
                    //{
                    //    indiceNuevaActividad = i;
                    //}
                    //i++;
                }


                this.gridViewFacturas.DataSource = tabla;
                this.gridViewFacturas.DataBind();
            }
            catch (Exception e)
            {
                //mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Facturas.");
            }
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
        }

        protected void botonConsultaEntradas_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();            
            modo = (int)Modo.SeleccionEntrada;
            cambiarModo();
            llenarGridEntradas();
        }

        protected void gridViewEntradas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            modo = (int)Modo.EntradaConsultada;
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewEntradas.Rows[Convert.ToInt32(e.CommandArgument)];
                    String codigo = "";
                    codigo = Convert.ToString(idArrayEntrada[Convert.ToInt32(e.CommandArgument) + (this.gridViewEntradas.PageIndex)]);
                    consultarEntrada(codigo);
                    break;
            }
            cambiarModo();

          //  Response.Redirect("FormEntradas.aspx");
        }

        protected void consultarEntrada(String codigo)
        {
            seConsulto = true;
            try
            {
                entradaConsultada = controladoraEntradas.consultarEntrada(codigo);
                //facturaConsultada = controladoraEntradas.consultarFactura(entradaConsultada.IdFactura);
                CompletarDatosEntrada(entradaConsultada);
                //llenarGridDetalleFactura();
            }
            catch
            {
                modo = (int)Modo.Inicial;
            }
        }

        protected void gridViewEntradas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void botonAgregarEntradas_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
            modo = (int)Modo.BusquedaFactura;
            cambiarModo();
        }

        protected void botonMostrarFacturas_Click(object sender, EventArgs e)
        {
            facturaBuscada = "Todas";
            modo = (int)Modo.SeleccionFactura;
            cambiarModo();
            llenarGridFacturas();
        }

        protected void botonBuscarFactura_Click(object sender, EventArgs e)
        {
            facturaBuscada = this.barraDeBusquedaFactura.Value.ToString();
            modo = (int)Modo.SeleccionFactura;
            cambiarModo();
            llenarGridFacturas();
        }


        protected void gridViewFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            modo = (int)Modo.SeleccionProductos;
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewFacturas.Rows[Convert.ToInt32(e.CommandArgument)];
                    String codigo = "";
                    codigo = Convert.ToString(idArrayFactura[Convert.ToInt32(e.CommandArgument) + (this.gridViewFacturas.PageIndex)]);
                    consultarFactura(codigo);
                    break;
            }
            cambiarModo();
        }

        protected void gridViewFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void gridViewFacturas_Seleccion(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void consultarFactura(String codigo)
        {
            seConsulto = true;
            try
            {
                facturaConsultada = controladoraEntradas.consultarFactura(codigo);
                CompletarDatosFactura(facturaConsultada);
                llenarGridDetalleFactura();
            }
            catch
            {
                //actividadConsultada = null;
                modo = (int)Modo.Inicial;
            }
        }

        protected void CompletarDatosFactura(EntidadFactura facturaConsultada)
        {
            outputFactura.InnerText = Convert.ToString(facturaConsultada.IdFactura);
            outputFechaPago.InnerText = Convert.ToString(facturaConsultada.FechaPago);
            outputDescuento.InnerText = Convert.ToString(facturaConsultada.Descuento);
            outputTipoPago.InnerText = Convert.ToString(facturaConsultada.TipoDePago);
            outputPlazoPago.InnerText = Convert.ToString(facturaConsultada.PlazoDePago);
            outputSubtotal.InnerText = Convert.ToString(facturaConsultada.SubTotal);
            outputTotal.InnerText = Convert.ToString(facturaConsultada.Total);
            outputImpuestos.InnerText = Convert.ToString(facturaConsultada.RetencionImpuestos);
            outputMoneda.InnerText = Convert.ToString(facturaConsultada.Moneda);
            outputTipoCambio.InnerText = Convert.ToString(facturaConsultada.TipoCambio);
        }
        protected void CompletarDatosEntrada(EntidadEntrada entradaConsultada)
        {
            outputEntrada.InnerText = Convert.ToString(entradaConsultada.IdEntrada);
            outputFacturaAsociada.InnerText = Convert.ToString(entradaConsultada.IdFactura);
           //outputUsuario.InnerText = Convert.ToString(controladoraSeguridad.consultarNombreDeUsuario(entradaConsultada.IdEncargado));
            outputUsuario.InnerText = Convert.ToString(entradaConsultada.IdEncargado);
            outputBodega.InnerText = Convert.ToString(entradaConsultada.Bodega);
            outputFecha.InnerText = Convert.ToString(entradaConsultada.FechEntrada);
        }
        protected void gridDetalleFactura_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void llenarGridDetalleFactura()
        {
            DataTable tabla = tablaFacturaDetallada();
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[3];
                DataTable facturas = controladoraEntradas.consultarDetalleFactura(facturaConsultada.IdOrdenDeCompra);

                if (facturas.Rows.Count > 0)
                {
                    //idArray = new Object[facturas.Rows.Count];
                    foreach (DataRow fila in facturas.Rows)
                    {
                        //idArray[i] = fila[0];
                        datos[0] = fila[2].ToString();
                        datos[1] = fila[3].ToString();
                        datos[2] = fila[10].ToString();


                        tabla.Rows.Add(datos);
                        //if (entradaConsultada != null && (fila[0].Equals(entradaConsultada.Codigo)))
                        //{
                        //    indiceNuevaActividad = i;
                        //}
                        i++;
                    }
                }
                // No hay entradas almacenadas.
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridDetalleFactura.DataSource = tabla;
                this.gridDetalleFactura.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Facturas.");
            }
        }
        protected void botonBuscar_Click(object sender, EventArgs e)
        {
            //FieldsetResultadosBusqueda.Visible = true;
        }

        protected void gridViewFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void botonAgregarProductoFactura_Click(object sender, EventArgs e)
        {

        }

        protected void gridViewProductoBuscado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //FieldsetResultadosBusqueda.Visible = false;
        }

        protected void gridViewProductoBuscado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gridFacturaNueva_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void botonAceptarEntrada_ServerClick(object sender, EventArgs e)
        {

        }

        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        protected void gridProductosDeEntrada_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

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
                    this.FieldsetGridEntradas.Visible = false;
                    this.FielsetBuscarFactura.Visible = false;
                    this.FieldsetGridFacturas.Visible = false;
                    this.FieldsetEncabezadoFactura.Visible = false;
                    this.FieldsetCrearFactura.Visible = false;
                    this.FieldsetResultadosBusqueda.Visible = false;
                    this.botonAgregarEntradas.Disabled = false;
                    this.botonAceptarEntrada.Visible = false;
                    this.botonCancelarEntrada.Visible = false;
                    this.FieldsetGridProductosDeEntrada.Visible = false;
                    this.FieldsetGridProductosDeEntrada.Visible = false;
                    tituloAccionEntradas.InnerText = "Seleccione una opción";
                    break;


                case (int)Modo.BusquedaFactura: // Modo que permite buscar una factura por identificador o 
                                                // seleccionando del listado general.
                    this.botonAgregarEntradas.Disabled = true;
                    this.FielsetBuscarFactura.Visible = true;
                    this.botonAceptarEntrada.Visible = true;
                    this.botonAceptarEntrada.Disabled = true;
                    this.botonCancelarEntrada.Visible = true;
                    this.FieldsetGridEntradas.Visible = false;
                    tituloAccionEntradas.InnerText = "";
                    break;

                case (int)Modo.SeleccionFactura: // Se presenta la lista con los resultados de la búsqueda para	
                                                // elegir la que se desea trabajar.

                    this.FieldsetGridFacturas.Visible = true;
                    break;
                case (int)Modo.SeleccionProductos: // Visualiza la información de la factura seleccionada y permite 
                                                  // detallar los productos recibidos(Crear la entrada).
                    this.FieldsetEncabezadoFactura.Visible = true;
                    this.FieldsetCrearFactura.Visible = true;
                    this.botonAceptarEntrada.Disabled = false;
                    limpiarCampos();
                    break;

                case (int)Modo.EntradaConsultada:
                    tituloAccionEntradas.InnerText = "";
                    this.FieldsetGridEntradas.Visible = true;
                    this.FieldsetGridProductosDeEntrada.Visible = true;
                    break;

                case (int)Modo.SeleccionEntrada:
                    tituloAccionEntradas.InnerText = "";
                    this.FieldsetGridEntradas.Visible = true;
                    this.FieldsetEncabezadoFactura.Visible = false;
                    break;

                default:
                break;
            }
        }

        private void limpiarCampos()
        {
            this.inputCantidad.Value = "";
            this.inputCosto.Value = "";
        }






    }
}