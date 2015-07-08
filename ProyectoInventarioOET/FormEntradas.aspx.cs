using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ProyectoInventarioOET.Modulo_Entradas;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.Modulo_Ajustes;
using ProyectoInventarioOET.Modulo_ProductosLocales;

namespace ProyectoInventarioOET
{
    /*
     * Clase interfaz que se encarga de todo lo relacionado con entradas, desde el punto de vista de las facturas que vienen del sistema
     * de compras.
     * Permite consultar entradas, crear entradas. Dependiendo de los permisos que tenga el perfil del usuario conectado.
     */
    public partial class FormEntradas : System.Web.UI.Page
    {
        enum Modo { Inicial, BusquedaFactura, SeleccionFactura, SeleccionProductos,  EntradaConsultada, SeleccionEntrada,ModificarEntrada }; // Sirve para controlar los modos de la interfaz
        //Atributos

        private static int modo = (int)Modo.Inicial; 
                           // Almacena el modo actual de la interfaz
        private static ControladoraEntradas controladoraEntradas;               // Comunica con la base de datos.
        private static Object[] idArrayFactura;                                 // Almacena identificadores de las facturas
        private static Object[] idArrayEntrada;                                 // Almacena identificadores de entradas
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Obtiene datos generales (estados)
        private static EntidadEntrada entradaConsultada;                        // Almacena la entrada que se consultó (o acaba de agregar)
        private static Boolean seConsulto = false;                              // Bandera para saber si hubo consulta de una actividad.
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static String bodegaDeTrabajo;                                  // Almacena el identificador de la bodega en la que se está logueado.
        private static String facturaBuscada;                                   // Almacena el identificador de la factura que se eligió para trabajar.
        private static EntidadFactura facturaConsultada;                        // Almacena la factura que se consultó.
        private static ControladoraSeguridad controladoraSeguridad;             // Ayuda con funciones de seguridad
        private static DataTable tablaProductosNuevos;                          // Tabla persistente con los productos que están siendo ingresados.
        private static double totalFactura;
        private static double totalFacturaConsultada;
        private static ControladoraBodegas controladoraBodegas;
        private static ControladoraProductoLocal controladoraProductoLocal;
        private static ControladoraAjustes controladoraAjustes;
        /*
         * Maneja las acciones que se ejecutan cuando se carga la página, establecer el modo de operación, 
         * cargar elementos de la interfaz, gestión de seguridad.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            //Elementos visuales
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
            controladoraEntradas = new ControladoraEntradas();
            bodegaDeTrabajo = (this.Master as SiteMaster).LlaveBodegaSesion;            
            mensajeAlerta.Visible = false;
            controladoraSeguridad = new ControladoraSeguridad();
            controladoraBodegas = new ControladoraBodegas();
            controladoraProductoLocal = new ControladoraProductoLocal();
            controladoraAjustes = new ControladoraAjustes();

            controladoraSeguridad.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
            controladoraBodegas.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
            if (!IsPostBack)
            {
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraEntradas = new ControladoraEntradas();
                controladoraEntradas.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Entradas de inventario");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");

                // Esconder botones
                mostrarBotonesSegunPermisos();
                modo = (int)Modo.Inicial;

                tablaProductosNuevos = new DataTable();
                tablaProductosNuevos = tablaFacturaDetallada();
                entradaConsultada = null;
                facturaConsultada = null;
                totalFactura = 0;
                //totalFacturaConsultada = 0;
                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (entradaConsultada == null)
                    {
                        //mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la entrada.");
                    }
                    else
                    {
                        seConsulto = false;
                        CompletarDatosFactura(facturaConsultada);
                    }
                }

            }
            cambiarModo();
            if (tablaProductosNuevos.Rows.Count != 0)
            {
                this.botonEliminarProducto.Enabled = true;
                this.botonModificarProducto.Enabled = true;

            }
        }

        //Construcción y llenado de las distintas tablas que se muestran en la interfaz.

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
            columna.ColumnName = "Proveedor";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Fecha de Pago";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Tipo de Pago";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Moneda";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información del detalle de las Facturas entrantes.
         */
        protected DataTable tablaFacturaEntranteDetallada()
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
            columna.ColumnName = "Costo Total";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Costo Unitario";
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
            columna.ColumnName = "Costo Total";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Costo Unitario";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Impuesto";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Descuento";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Llena la tabla con las entradas almacenadas en la base de datos.
         */
        protected void llenarGridEntradas()
        {
            DataTable tabla = tablaEntradas();
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[4];
                DataTable entradas = controladoraEntradas.consultarEntradas(bodegaDeTrabajo);

                if (entradas.Rows.Count > 0)
                {
                    idArrayEntrada = new Object[entradas.Rows.Count];
                    foreach (DataRow fila in entradas.Rows)
                    {
                        idArrayEntrada[i] = fila[0];
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = controladoraSeguridad.consultarNombreDeUsuario(fila[2].ToString());
                        datos[3] = Convert.ToDateTime(fila[4]).Date.ToString("dd/MM/yyyy"); 


                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                // No hay entradas almacenadas.
                else
                {
                    mostrarMensaje("warning", "Alerta", "No hay entradas almacenadas.");
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
         * Llena la tabla con las facturas almacenadas en la base de datos.
         */
        protected void llenarGridFacturas()
        {
            DataTable tabla = tablaFacturas();
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[5];
                DataTable facturas = controladoraEntradas.buscarFacturas(facturaBuscada);

                if (facturas.Rows.Count > 0)
                {
                    idArrayFactura = new Object[facturas.Rows.Count];
                    foreach (DataRow fila in facturas.Rows)
                    {
                        idArrayFactura[i] = fila[0];
                        datos[0] = fila[0].ToString();
                        datos[1] = controladoraEntradas.consultarNombreProveedor(fila[6].ToString());
                        datos[2] = Convert.ToDateTime(fila[1]).Date.ToString("dd/MM/yyyy");
                        datos[3] = fila[3].ToString();
                        datos[4] = controladoraEntradas.consultarNombreMoneda(fila[4].ToString());


                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                // No hay entradas almacenadas.
                else
                {
                    mostrarMensaje("warning", "Alerta", "No hay Facturas disponibles en este momento.");
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
         * Llena la tabla con el detalle de la factura consultada.
         */
        protected void llenarGridDetalleFactura()
        {
            DataTable tabla = tablaFacturaEntranteDetallada();
            int i = 0;

            try
            {
                // Cargar facturas
                Object[] datos = new Object[4];
                DataTable productos = controladoraEntradas.consultarDetalleFactura(facturaConsultada.IdFactura);

                if (productos.Rows.Count > 0)
                {
                    foreach (DataRow fila in productos.Rows)
                    {
                        datos[0] = fila[2].ToString();
                        datos[1] = fila[3].ToString();
                        datos[2] = fila[5].ToString();
                        datos[3] = Math.Truncate( Convert.ToDouble(fila[5].ToString()) / Convert.ToDouble(fila[3].ToString())*100)/100;
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }

                else
                {
                    datos[0] = "-";
                    datos[1] = "No existe detalle para esta factura.";
                    datos[2] = "-";
                    datos[3] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridDetalleFactura.DataSource = tabla;
                this.gridDetalleFactura.DataBind();
                this.outputTotalFacturaEntrante.InnerText = facturaConsultada.Total.ToString();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla con el detalle.");
            }
        }


        /*
         * Llena la tabla con el detalle de la entrada consultada.
         */
        protected void llenarGridProductos()
        {
            DataTable tabla = tablaFacturaDetallada();
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[6];
                DataTable facturas = controladoraEntradas.consultarProductosEntrada(entradaConsultada.IdEntrada);

                if (facturas.Rows.Count > 0)
                {
                    idArrayFactura = new Object[facturas.Rows.Count];
                    foreach (DataRow fila in facturas.Rows)
                    {
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = fila[2].ToString();
                        datos[3] = fila[3].ToString();
                        datos[4] = fila[4].ToString();
                        datos[5] = fila[5].ToString();
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                // No hay productos asociados.
                else
                {
                    datos[0] = "-";
                    datos[1] = "No hay productos asociados.";
                    datos[2] = "-";
                    datos[3] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridProductosDeEntrada.DataSource = tabla;
                this.gridProductosDeEntrada.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla con el detalle.");
            }
        }

        // Control de eventos que son desencadenados desde la interfaz.

        /*
         * Maneja el evento al presionar el botón de consultas, mostrando la tabla de Entradas.
         */
        protected void botonConsultaEntradas_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();            
            modo = (int)Modo.SeleccionEntrada;
            cambiarModo();
            llenarGridEntradas();
        
        }
        protected void botonModificarEntrada_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.ModificarEntrada;
            CompletarDatosEntrada(entradaConsultada);
            cambiarModo();

        }
        /*
         * Se dispara cuando se selecciona una entrada para consultar su información.
         */
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



        /*
         * Maneja el evento de presionar el botón para agregar una entrada, cambiando el modo de la pantalla.
         */
        protected void botonAgregarEntradas_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
            tablaProductosNuevos = new DataTable();
            tablaProductosNuevos = tablaFacturaDetallada();
            this.gridFacturaNueva.DataSource = tablaProductosNuevos;
            this.gridFacturaNueva.DataBind();
            totalFactura = 0;
            outputTotalFacturaNueva.InnerText = totalFactura.ToString();
            modo = (int)Modo.BusquedaFactura;
            facturaBuscada = "Todas";
            llenarGridFacturas();
            cambiarModo();
        }

        /*
         * Maneja el evento de presionar el botón para mostrar todas las facturas, cambiando
         * el modo y mostrando los elementos pertinentes en la pantalla.
         */
        protected void botonMostrarFacturas_Click(object sender, EventArgs e)
        {
            facturaBuscada = "Todas";
            modo = (int)Modo.Inicial;
            cambiarModo();
            modo = (int)Modo.BusquedaFactura;
            cambiarModo();
            //this.gridViewFacturas.Visible = true;
            llenarGridFacturas();
        }

        /*
         * Realiza la búsqueda de una factura usando lo especificado en la barra de búsqueda.
         */
        protected void botonBuscarFactura_Click(object sender, EventArgs e)
        {
            facturaBuscada = this.barraDeBusquedaFactura.Value.ToString();
            //modo = (int)Modo.SeleccionFactura;
            //cambiarModo();
            llenarGridFacturas();
        }

        /*
         * Se dispara al seleccionar una factura, mostrando la información de esta en la 
         * interfaz y cambiando al modo pertinente.
         */
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

        //protected void gridViewFacturas_Seleccion(object sender, GridViewCommandEventArgs e)
        //{

        //}
        
        //protected void gridViewFacturas_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}






        /*
         * Regresa la interfaz a modo inicial en caso de darse una cancelación de una acción (inserción).
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        /*
         * Toma la información del producto elegido y lo agrega a la entrada que se está 
         * trabajando.
         */
        protected void botonAgregarProductoFactura_Click(object sender, EventArgs e)
        {
            //String producto = this.textBoxAutocompleteCrearFacturaBusquedaProducto.Text;
            String productoEscogido = this.textBoxAutocompleteCrearFacturaBusquedaProducto.Text;
            String cantidad = this.inputCantidadProducto.Value.ToString();
            String costo = this.inputCostoProducto.Value.ToString();
            Double costoUnitario = 0.0;
            String descuento = this.inputDescuentoProducto.Value.ToString();
            String descuentoReal = "";
            String impuesto = this.dropdownlistProductoImpuesto.SelectedValue.ToString();
            String[] provisional = new String[2];
            provisional = obtenerCodigoDeProducto(productoEscogido);
            int impuestoDeVentas = controladoraEntradas.consultarImpuestoDeVentas();
            bool repetido = false;

            DataTable producto = controladoraEntradas.consultarProductoDeBodega(bodegaDeTrabajo, provisional[1]);

            //foreach (DataRow fila in tablaProductosNuevos.Rows)
            //{
            //    if (productoEscogido == fila[0].ToString())
            //    {
            //        repetido = true;
            //    }            
            //}

            if (producto.Rows.Count == 0)
            {
                mostrarMensaje("warning", "Error", "El producto especificado no existe en la bodega, consulte al administrador del sistema.");
            } //ojo que esta llave es extra

            //} else if (repetido) SI PUEDE ESTAR REPETIDO EN CASO DE QUE SE COBRE 'X' CANTIDAD Y DEN DE REGALIA 'Y' CANTIDAD
            //{
            //    mostrarMensaje("warning", "Error", "El producto especificado ya fue ingresado, seleccione otro o actualice las cantidades.");            
            //}

            else
            {
                descuentoReal = verificarExtraerDescuento(descuento, costo).ToString();

                if (descuentoReal != "Inválido")
                {
                    if ("Sí".Equals(impuesto))
                    {
                        Double costoTotalSinImpuesto = Convert.ToDouble(costo) - ((impuestoDeVentas * Convert.ToDouble(costo)) / 100.0);
                        costoUnitario = costoTotalSinImpuesto / Convert.ToDouble(cantidad);
                    }
                    else
                    { 
                        costoUnitario = Convert.ToDouble(costo) / Convert.ToDouble(cantidad);                    
                    }

                    Object[] datos = new Object[6];
                    datos[0] = productoEscogido;
                    datos[1] = cantidad;
                    datos[2] = costo;
                    datos[3] = Math.Truncate((costoUnitario) * 100) / 100;
                    datos[4] = dropdownlistProductoImpuesto.SelectedValue;
                    datos[5] = Math.Truncate(Convert.ToDouble(descuentoReal) * 100) / 100;
                    tablaProductosNuevos.Rows.Add(datos);
                    actualizarTotalFactura(Convert.ToDouble(costo));
                    outputTotalFacturaNueva.InnerText = totalFactura.ToString();

                    this.botonEliminarProducto.Enabled = true;
                    this.botonModificarProducto.Enabled = true;
                    this.botonAceptarEntrada.Disabled = false;
                    this.gridFacturaNueva.DataSource = tablaProductosNuevos;
                    this.gridFacturaNueva.DataBind();

                    limpiarCampos();
                    mostrarMensaje("success", "Éxito:", "El producto fue agregado exitosamente.");

                }
                else 
                {
                    mostrarMensaje("warning", "Error", "El descuento especificado no es válido.");
                }


            }
            this.botonAgregarProductoFactura.Text = "Agregar a Factura";

        }

        private String verificarExtraerDescuento(string descuento, string costo)
        {
            int tamano = descuento.Length;
            String descuentoReal = "";

            if (descuento.Contains("%"))
            {
                descuentoReal = descuento.Remove(descuento.LastIndexOf('%'));
                if (Convert.ToDouble(descuentoReal) > 100)
                {
                    descuentoReal = "Inválido";
                }
            }
            else
            {
                if (Convert.ToDouble(descuento) > Convert.ToDouble(costo))
                {
                    descuentoReal = "Inválido";
                }
                else
                {
                    descuentoReal = descuento;
                }
            }

            return descuentoReal;

        }

        /*
         * Permite modificar el costo, cantidad o hasta el producto que se eligió en la creación
         * de la entrada de productos.
         */
        protected void botonModificarProducto_Click(object sender, EventArgs e)
        {
            String producto;
            String costo = "";
            String cantidad;
            String descuento;
            String impuesto;
            bool estaSeleccionadoProducto = false;

            for (int i = 0; i < gridFacturaNueva.Rows.Count; i++)
            {
                GridViewRow row = gridFacturaNueva.Rows[i];
                estaSeleccionadoProducto = ((CheckBox)row.FindControl("checkBoxProductos")).Checked;


                if (estaSeleccionadoProducto)
                {

                    producto = HttpUtility.HtmlDecode(gridFacturaNueva.Rows[i].Cells[1].Text.ToString());
                    cantidad = gridFacturaNueva.Rows[i].Cells[2].Text.ToString();
                    costo = gridFacturaNueva.Rows[i].Cells[3].Text.ToString();
                    impuesto = gridFacturaNueva.Rows[i].Cells[5].Text.ToString();
                    descuento = gridFacturaNueva.Rows[i].Cells[6].Text.ToString();
                    
                    tablaProductosNuevos.Rows.RemoveAt(i);
                    this.dropdownlistProductoImpuesto.SelectedValue = impuesto;
                    this.inputDescuentoProducto.Value = descuento;
                    this.inputCantidadProducto.Value = cantidad;
                    this.inputCostoProducto.Value = costo;
                    this.textBoxAutocompleteCrearFacturaBusquedaProducto.Text = producto;

                    double restarCosto = Convert.ToDouble(costo) * -1;
                    actualizarTotalFactura(restarCosto);
                    outputTotalFacturaNueva.InnerText = totalFactura.ToString();
                }
            }

            if (!estaSeleccionadoProducto)
            {
                mostrarMensaje("warning", "Aviso", "Seleccione un producto para modificar.");            
            
            }


            if (tablaProductosNuevos.Rows.Count > 0)
            {
                this.botonEliminarProducto.Enabled = true;
                this.botonModificarProducto.Enabled = true;
            }

            this.botonAgregarProductoFactura.Text = "Aceptar Modificación";
        }

        /*
         * Evento que se dispara cuando se selecciona un producto y se decide a eliminarlo
         * de la tabla de productos elegidos.
         */
        protected void botonEliminarProducto_Click(object sender, EventArgs e)
        {
            String costo = "";
            bool estaSeleccionadoProducto = false;
            for (int i = 0; i < gridFacturaNueva.Rows.Count; i++)
            {
                GridViewRow row = gridFacturaNueva.Rows[i];
                estaSeleccionadoProducto = ((CheckBox)row.FindControl("checkBoxProductos")).Checked;


                if (estaSeleccionadoProducto)
                {
                    costo = gridFacturaNueva.Rows[i].Cells[3].Text.ToString();
                    tablaProductosNuevos.Rows.RemoveAt(i);
                    this.gridFacturaNueva.DataSource = tablaProductosNuevos;
                    this.gridFacturaNueva.DataBind();

                    double restarCosto = Convert.ToDouble(costo) * -1;
                    actualizarTotalFactura(restarCosto);
                    outputTotalFacturaNueva.InnerText = totalFactura.ToString();

                }
            }


            if (!estaSeleccionadoProducto)
            {
                mostrarMensaje("warning", "Aviso", "Seleccione un producto para eliminar.");

            }

            if (tablaProductosNuevos.Rows.Count > 0)
            {
                this.botonEliminarProducto.Enabled = true;
                this.botonModificarProducto.Enabled = true;
            }
        }

        /*
         * Evento que se dispara cuando se cambia la selección en la tabla de la factura
         *  que se está creando, de manera que solo 1 se pueda elegir a la vez.
         */
        protected void checkBoxProductos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gv = (GridViewRow)chk.NamingContainer;
            int rownumber = gv.RowIndex;

            if (chk.Checked)
            {
                int i;
                for (i = 0; i <= gridFacturaNueva.Rows.Count - 1; i++)
                {
                    if (i != rownumber)
                    {
                        CheckBox chkcheckbox = ((CheckBox)(gridFacturaNueva.Rows[i].FindControl("checkBoxProductos")));
                        chkcheckbox.Checked = false;
                    }
                }
            }
        }

        /*
         * Evento que maneja el cambio de página en la tabla de facturas.
         */
        protected void gridViewFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            llenarGridFacturas();
            this.gridViewFacturas.PageIndex = e.NewPageIndex;
            this.gridViewFacturas.DataBind();
        }

        /*
         * Evento que maneja el cambio de página en la tabla de los productos que forman el detalle
         * de una factura.
         */
        protected void gridDetalleFactura_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            llenarGridDetalleFactura();
            this.gridDetalleFactura.PageIndex = e.NewPageIndex;
            this.gridDetalleFactura.DataBind();
        }

        /*
         * Evento que maneja el cambio de página en la tabla de entradas.
         */
        protected void gridViewEntradas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            llenarGridEntradas();
            this.gridViewEntradas.PageIndex = e.NewPageIndex;
            this.gridViewEntradas.DataBind();
        }

        /*
         * Evento que maneja el cambio de página en la tabla que contiene los productos de la factura
         * detallada que se está creando.
         */
        protected void gridFacturaNueva_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gridFacturaNueva.DataSource = tablaProductosNuevos;
            this.gridProductosDeEntrada.PageIndex = e.NewPageIndex;
            this.gridFacturaNueva.DataBind();
        }

        /*
         * Evento que maneja el cambio de página en la tabla de entradas.
         */
        protected void gridProductosDeEntrada_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            llenarGridProductos();
            this.gridProductosDeEntrada.PageIndex = e.NewPageIndex;
            this.gridProductosDeEntrada.DataBind();
        }


        // Métodos de apoyo para realizar procesos según las acciones realizadas.

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
                    this.botonAgregarEntradas.Disabled = false;
                    this.botonAceptarEntrada.Visible = false;
                    this.botonCancelarEntrada.Visible = false;
                    this.FieldsetGridProductosDeEntrada.Visible = false;
                    this.FieldsetGridProductosDeEntrada.Visible = false;
                    this.botonModificarEntrada.Disabled = true;
                    //this.botonAnularEntradas.Visible = false;
                    tituloAccionEntradas.InnerText = "Seleccione una opción";
                    break;


                case (int)Modo.BusquedaFactura: // Modo que permite buscar una factura por identificador o 
                    // seleccionando del listado general.
                    this.botonAgregarEntradas.Disabled = true;
                    this.FielsetBuscarFactura.Visible = true;
                    this.FieldsetGridEntradas.Visible = false;
                    this.FieldsetGridFacturas.Visible = true;
                    tituloAccionEntradas.InnerText = "Seleccione o busque una factura";
                    this.botonModificarEntrada.Disabled = true;    
                    //this.botonAnularEntradas.Visible = false;
                    break;

                case (int)Modo.SeleccionProductos: // Visualiza la información de la factura seleccionada y permite 
                    // detallar los productos recibidos(Crear la entrada).
                    this.FieldsetEncabezadoFactura.Visible = true;
                    this.FieldsetCrearFactura.Visible = true;
                    //this.botonAceptarEntrada.Disabled = false;
                    this.botonAceptarEntrada.Visible = true;
                    this.botonAceptarEntrada.Disabled = true;
                    this.botonCancelarEntrada.Visible = true;
                    this.botonModificarProducto.Enabled = false;
                    this.botonEliminarProducto.Enabled = false;
                    this.FieldsetGridFacturas.Visible = false;
                    tituloAccionEntradas.InnerText = "Seleccione los productos entrantes";
                    //this.botonAnularEntradas.Visible = false;
                    this.botonModificarEntrada.Disabled = true;
                    break;

                case (int)Modo.EntradaConsultada:
                    tituloAccionEntradas.InnerText = "";
                    this.FieldsetGridEntradas.Visible = false;
                    this.FieldsetGridProductosDeEntrada.Visible = true;
                  
                    tituloAccionEntradas.InnerText = "";
                    //this.botonAnularEntradas.Visible = true;
                    this.botonModificarEntrada.Disabled = !("1".Equals(entradaConsultada.Estado));
                    this.estadoEntrada.Enabled = false;
                    this.fieldSetProductosEntrada.Visible = true;
                    
                    break;

                case (int)Modo.SeleccionEntrada:
                    tituloAccionEntradas.InnerText = "";
                    this.FieldsetGridEntradas.Visible = true;
                    this.FieldsetEncabezadoFactura.Visible = false;
                    this.gridViewEntradas.Visible = true;
                    tituloAccionEntradas.InnerText = "Seleccione una entrada a consultar";
                    //this.botonAnularEntradas.Visible = false;
                    this.botonModificarEntrada.Disabled = true;
                  
                    
                    break;
                case (int)Modo.ModificarEntrada:
                    tituloAccionEntradas.InnerText = "";
                    this.FieldsetGridProductosDeEntrada.Visible = true;
                    this.FieldsetEncabezadoFactura.Visible = false;
                    this.gridViewEntradas.Visible = false;
                    this.FieldsetGridEntradas.Visible = false;
                    this.fieldSetProductosEntrada.Visible = true;
                    tituloAccionEntradas.InnerText = "Modificacion de una entrada";
                    this.estadoEntrada.Enabled = true;
                    this.botonModificarEntrada.Disabled = true;
                    botonAceptarEntrada.Visible = true;
                    botonCancelarEntrada.Visible = true;
                    
                    break;
                default:
                    break;
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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        }

        /*
         * Procedimiento que llena la información del encabezado de una factura en los
         * campos correspondientes.
         */
        protected void CompletarDatosFactura(EntidadFactura facturaConsultada)
        {
            outputFactura.InnerText = Convert.ToString(facturaConsultada.IdFactura);
            outputProveedor.InnerText = Convert.ToString(controladoraEntradas.consultarNombreProveedor(facturaConsultada.IdProveedor));
            outputFechaPago.InnerText = facturaConsultada.FechaPago.Date.ToString("dd/MM/yyyy");
            outputDescuento.InnerText = Convert.ToString(facturaConsultada.Descuento) + "%";
            outputTipoPago.InnerText = Convert.ToString(facturaConsultada.TipoDePago);
            outputPlazoPago.InnerText = Convert.ToString(facturaConsultada.PlazoDePago);
            outputSubtotal.InnerText = Convert.ToString(facturaConsultada.SubTotal);
            outputTotal.InnerText = Convert.ToString(facturaConsultada.Total);
            outputImpuestos.InnerText = Convert.ToString(facturaConsultada.RetencionImpuestos) + "%";
            outputMoneda.InnerText = controladoraEntradas.consultarNombreMoneda(facturaConsultada.Moneda.ToString()); 
            outputTipoCambio.InnerText = Convert.ToString(facturaConsultada.TipoCambio);

        }

        /*
         * Método que llena la información de una entrada en los campos correspondientes.
         */
        protected void CompletarDatosEntrada(EntidadEntrada entradaConsultada)
        {
            //outputEntrada.InnerText = Convert.ToString(entradaConsultada.IdEntrada);
            outputFacturaAsociada.InnerText = Convert.ToString(entradaConsultada.IdFactura);
            outputUsuario.InnerText = controladoraSeguridad.consultarNombreDeUsuario(Convert.ToString(entradaConsultada.IdEncargado));
            outputBodega.InnerText = controladoraBodegas.consultarBodega(Convert.ToString(entradaConsultada.Bodega)).Nombre;
            outputFecha.InnerText = Convert.ToDateTime(entradaConsultada.FechEntrada).Date.ToString("dd/MM/yyyy"); 
            outputTipoMonedaEntrada.InnerText = entradaConsultada.TipoMoneda;
            outputMetodoPagoEntrada.InnerText = entradaConsultada.MetodoPago;
            estadoEntrada.SelectedIndex = (String.Equals(entradaConsultada.Estado,"1"))?0:1;
            //estadoEntrada.Enabled = (String.Equals(entradaConsultada.Estado, "1"));
        }

        /*
         * Remueve la información de los campos de la interfaz.
         */
        private void limpiarCampos()
        {
            this.inputCantidadProducto.Value = "";
            this.inputCostoProducto.Value = "";
            this.textBoxAutocompleteCrearFacturaBusquedaProducto.Text = "";
            this.inputDescuentoProducto.Value = "";
            this.dropdownlistProductoImpuesto.SelectedValue = "Sí";
        }

        private String[] obtenerCodigoDeProducto(String producto) 
        {
            String[] resultado = new String[2];
            String codigoProductoEscogido = producto.Substring(producto.LastIndexOf('(') + 1);  //el código sin el primer paréntesis
            codigoProductoEscogido = codigoProductoEscogido.TrimEnd(')');                                       //el código
            producto = producto.Remove(producto.LastIndexOf('(') - 1);                  //nombre del producto (-1 al final por el espacio)
            resultado[0] = producto;
            resultado[1] = codigoProductoEscogido;
            return resultado;        
        
        }

        /*
         * Método que consulta la información de una factura para mostrar su información.
         */
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

        /*
         * Método que consulta la información de una entrada para mostrar su información.
         */
        protected void consultarEntrada(String codigo)
        {
            seConsulto = true;
            try
            {
                entradaConsultada = controladoraEntradas.consultarEntrada(codigo);
                CompletarDatosEntrada(entradaConsultada);
                llenarGridProductos();
            }
            catch
            {
                modo = (int)Modo.Inicial;
            }
        }

        /*
         * Cambia el modo de la pantalla activando/desactivando o mostrando/ocultando elementos de acuerdo con los
         * permisos del usuario
         */
        protected void mostrarBotonesSegunPermisos()
        {
            botonConsultaEntradas.Visible = (permisos[5] == '1');
            botonAgregarEntradas.Visible = (permisos[4] == '1');
            //botonModificacionActividades.Visible = (permisos[3] == '1');
            //comboBoxEstadosActividades.Enabled = (permisos[2] == '1');
        }

        private void actualizarTotalFactura(double cambio) {
            totalFactura += cambio;        
        }

        protected void dropdownlist1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string anularDatosEntrada()
        {
                Object[] datos = new Object[6];
                Object[] datosAjustes = new Object[8];
                datosAjustes[0] = "CYCLO106062012145300769001";
                datosAjustes[1] = DateTime.Now.ToString("h:mm:ss");
                datosAjustes[2] = (this.Master as SiteMaster).Usuario.Nombre;
                datosAjustes[3] = (this.Master as SiteMaster).Usuario.Codigo;
                datosAjustes[4] = "Anulacion de entrada de inventario";
                datosAjustes[5] = (this.Master as SiteMaster).LlaveBodegaSesion;
                datosAjustes[6] = 2;
                datosAjustes[7] = 0;
                string res = "";
                DataTable facturas = controladoraEntradas.consultarProductosEntrada(entradaConsultada.IdEntrada);
                List<EntidadDetalles> detallesEntrada = new List<EntidadDetalles>();
                EntidadAjustes ajuste = new EntidadAjustes(datosAjustes);
                try
                {
                    if (facturas.Rows.Count > 0)
                    {
                        idArrayFactura = new Object[facturas.Rows.Count];
                        foreach (DataRow fila in facturas.Rows)
                        {
                             DataRow rowProducto = controladoraProductoLocal.consultarProductoDeBodega((this.Master as SiteMaster).LlaveBodegaSesion, fila[6].ToString()).Rows[0];
                            datos[0] = fila[0].ToString();
                            datos[1] = "";
                            datos[2] = fila[2].ToString();
                            datos[3] = controladoraEntradas.consultarCodigoInventario(rowProducto[1].ToString(),(this.Master as SiteMaster).LlaveBodegaSesion);
                            datos[4] = rowProducto[7];
                            datos[5] = Convert.ToInt32(datos[4]) - Convert.ToInt32(fila[1].ToString());
                            ajuste.agregarDetalle(datos);

                        }
                    }
                    controladoraAjustes.insertarAjuste(ajuste);
                    controladoraEntradas.anularEntrada(entradaConsultada.IdEntrada);
                    res ="exito";
                    }
                catch (Exception e)
                {
                    res = "fallo";
                }
                return res;
        }
        /*
         * Se dispara para completar la entrada con los productos seleccionados.
         */			
        protected void botonAceptarEntrada_ServerClick(object sender, EventArgs e)
        {
            switch (modo)
            {
                case (int)Modo.SeleccionProductos:
                    if (totalFactura > (facturaConsultada.Total + 1) || totalFactura < (facturaConsultada.Total - 1))
                    {
                        mostrarMensaje("warning", "Alerta", "La diferencia entre el total de la factura nueva y el de la factura consultada no puede ser superior a 1 colón.");
                        if (tablaProductosNuevos.Rows.Count > 0)
                        {
                            this.botonEliminarProducto.Enabled = true;
                            this.botonModificarProducto.Enabled = true;
                        }
                    }
                    else
                    {
                        Boolean operacionCorrecta = false;
                        String codigoInsertado = "";
                        String usuario = (this.Master as SiteMaster).Usuario.Codigo;
                        String idFactura = facturaConsultada.IdFactura;
                        String fecha = DateTime.Now.ToString("h:mm:ss");
                        String[] resultado = new String[3];
                        String[] provisional = new String[2];
                        Object[] objetoEntrada = new Object[8];
                        Object[] datos = new Object[6];
                        DataTable tablaProductosConID = new DataTable();
                        tablaProductosConID = tablaFacturaDetallada();

                        if (modo == (int)Modo.SeleccionProductos)
                        {
                            try
                            {
                                objetoEntrada[1] = idFactura;
                                objetoEntrada[0] = "";
                                objetoEntrada[4] = fecha;
                                objetoEntrada[3] = bodegaDeTrabajo;
                                objetoEntrada[2] = usuario;
                                objetoEntrada[5] = this.dropdownlistTipoMoneda.SelectedValue;
                                objetoEntrada[6] = this.dropdownlistMetodoPago.SelectedValue;
                                objetoEntrada[7] = 1;
                                foreach (DataRow fila in tablaProductosNuevos.Rows)
                                {
                                    provisional = obtenerCodigoDeProducto(fila[0].ToString());
                                    datos[0] = provisional[1];
                                    datos[1] = fila[1].ToString();
                                    datos[2] = fila[2].ToString();
                                    datos[3] = fila[3].ToString();
                                    datos[4] = fila[4].ToString();
                                    datos[5] = fila[5].ToString();


                                    tablaProductosConID.Rows.Add(datos);

                                }
                                resultado = controladoraEntradas.insertarEntrada(objetoEntrada, tablaProductosConID);
                                if (resultado[1] == "Éxito:")
                                {
                                    modo = (int)Modo.Inicial;
                                    operacionCorrecta = true;
                                }
                            }
                            catch (Exception t)
                            {
                                mostrarMensaje("warning", "Alerta", "No se pudo insertar la entrada.");
                                operacionCorrecta = false;
                            }

                            if (operacionCorrecta)
                            {
                                mostrarMensaje(resultado[0], resultado[1], resultado[2]);
                                cambiarModo();
                            }
                        }
                    }
                    break;
                case (int)Modo.ModificarEntrada:

                    if (this.estadoEntrada.SelectedIndex == 1)
                    {
                        string res = anularDatosEntrada();
                        if ("exito".Equals(res))
                        {
                            modo = (int)Modo.Inicial;
                            mostrarMensaje("success", "Exito:", "La entrada ha sido modificada satisfactoriamente.");
                            cambiarModo();
                        }
                        else
                        {
                            mostrarMensaje("warning", "Alerta", "No se pudo modificar la entrada.");
                        }
                    }
                    else {
                        mostrarMensaje("warning", "Alerta", "No seleciono modificar la entrada.");
                    }

                    break;
            }
        }
        
    }
}
