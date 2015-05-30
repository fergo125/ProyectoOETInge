using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_Ventas;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.App_Code.Modulo_Ajustes;
using ProyectoInventarioOET.App_Code;

namespace ProyectoInventarioOET
{
    /*
     * Clase interfaz que se encarga de todo lo relacionado con ventas, desde un punto de vista de facturas.
     * Permite consultar facturas, crear facturas, y anular fcturas existentes. Dependiendo de los permisos que tenga el perfil del usuario conectado.
     */
    public partial class FormVentas : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static Modo modo = Modo.Inicial;                               //Indica en qué modo se encuentra la interfaz en un momento cualquiera, de éste depende cuáles elementos son visibles
        private static String permisos = "111111";                             //Permisos utilizados para el control de seguridad //TODO: poner en 000000, está en 111111 sólo para pruebas
        private static String codigoPerfilUsuario = "1";                       //Indica el perfil del usuario, usado para acciones de seguridad para las cuales la string de permisos no basta //TODO: poner en ""
        private static DataTable facturasConsultadas;                          //Usada para llenar el grid y para mostrar los detalles de cada factura específica
        private static EntidadFactura facturaConsultada;
        private static Boolean seConsulto = false;
        private static Object[] idArray;                                //Usada para llevar el control de las facturas consultadas
        private static ControladoraVentas controladoraVentas;                  //Para accesar las tablas del módulo y realizar las operaciones de consulta, inserción, modificación y anulación
        private static ControladoraDatosGenerales controladoraDatosGenerales;  //Para accesar datos generales de la base de datos
        private static ControladoraBodegas controladoraBodegas;  //Para accesar datos generales de la base de datos
        private static ControladoraSeguridad controladoraSeguridad;     //???
        private static ControladoraAjustes controladoraAjustes;     //???
        
        //Importante:
        //Para el codigoPerfilUsuario (que se usa un poco hard-coded), los números son:
        //1. Administrador global
        //2. Administrador local
        //3. Supervisor
        //4. Vendedor

        /*
         * Función invocada cada vez que se carga la página, encargada de invocar a las funciones que actualizan los elementos visuales de la página.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            mensajeAlerta.Visible = false;
            //Si es la primera vez que se carga la página
            if (!IsPostBack)
            {
                //Elementos visuales
                ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
                //Controladoras
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraSeguridad = new ControladoraSeguridad();
                controladoraVentas = new ControladoraVentas();
                controladoraBodegas = new ControladoraBodegas();
                controladoraAjustes = new ControladoraAjustes();
                //Seguridad
                //permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Facturacion"); //TODO: descomentar esto, está comentado sólo para pruebas
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                //perfilUsuario = (this.Master as SiteMaster).Usuario.Perfil;
                mostrarElementosSegunPermisos();


                if (!seConsulto)
                {
                    modo = Modo.Inicial;
                }
                else
                {
                    if (facturaConsultada == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la bodega.");
                    }
                    else
                    {
                        setDatosConsultados();
                        seConsulto = false;
                    }
                }

                
            }
            //Si la página ya estaba cargada pero está siendo cargada de nuevo (porque se está realizando alguna acción que la refrezca/actualiza)

            cambiarModo();
            //código para probar algo
            DataTable testTable = new DataTable();
            DataRow testRow;
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nombre";
            testTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Código interno";
            testTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "Precio unitario";
            testTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Impuesto";
            testTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "Descuento (%)";
            testTable.Columns.Add(column);

            testRow = testTable.NewRow();
            testRow["Nombre"] = "Nombre de prueba";
            testRow["Código interno"] = "CRO001";
            testRow["Precio unitario"] = "500";
            testRow["Impuesto"] = "Sí";
            testRow["Descuento (%)"] = "0";
            testTable.Rows.Add(testRow);
            testRow = testTable.NewRow();
            testRow["Nombre"] = "Nombre de prueba larguísimo de esos que ponen las unidades y la vara";
            testRow["Código interno"] = "CRO002";
            testRow["Precio unitario"] = "50000";
            testRow["Impuesto"] = "Sí";
            testRow["Descuento (%)"] = "0";
            testTable.Rows.Add(testRow);

            gridViewCrearFacturaProductos.DataSource = testTable;
            gridViewCrearFacturaProductos.DataBind();

            //DataControlField[] backupColumn = new DataControlField[100];
            //gridViewCrearFacturaProductos.Columns.CopyTo(backupColumn, 0);
            //gridViewCrearFacturaProductos.Columns.Insert(0, backupColumn[0]);
            //DataControlField backup = gridViewCrearFacturaProductos.Columns[1];
            //gridViewCrearFacturaProductos.Columns.RemoveAt(1);
            //gridViewCrearFacturaProductos.Columns.Add(backup);
            //gridViewCrearFacturaProductos.Columns.Add(backup);
            //gridViewCrearFacturaProductos.Columns["Seleccionar"].DisplayIndex = 0;
        }

        /*
         * Dependiendo del perfil, algunas acciones están permitidas y otras no, ésto se controla escondiendo y mostrando los botones y otros elementos
         * que se usan para realizar esas acciones. Tratar de contener aquí todo lo relacionado con seguridad.
         */
        protected void mostrarElementosSegunPermisos()
        {
            //Botones principales
            botonConsultar.Visible = (permisos[5] == '1');
            botonCrear.Visible = (permisos[4] == '1');
            botonModificar.Visible = (permisos[3] == '1');
            //Dropdownlists
            dropDownListConsultaEstacion.Enabled = (Convert.ToInt32(codigoPerfilUsuario) <= 1);    //Sólo si es administrador global, puede escoger una estación
            dropDownListConsultaBodega.Enabled = (Convert.ToInt32(codigoPerfilUsuario) <= 2);      //Sólo si es administrador global, o administrador local, puede escoger una bodega
            dropDownListConsultaVendedor.Enabled = (Convert.ToInt32(codigoPerfilUsuario) <= 3);    //Sólo si es administrador global, o administrador local, o supervisor, puede escoger un vendedor
            //dropdownEstado.Enabled = (permisos[2] == '1');
        }

        /*
         * Función invocada cada vez que se cambia de modo en la interfaz, se encarga de mostrar u ocultar, habilitar o deshabilitar,
         * elementos visuales, dependiendo del modo al que se está entrando.
         */
        protected void cambiarModo()
        {
            //Código común (que debe ejecutarse en la mayoría de modos, en la minoría luego es arreglado en el switch)
            //Reduce un poco la eficiencia, pero simplifica el código bastante
            PanelConsultarFacturas.Visible = false;
            PanelConsultarFacturaEspecifica.Visible = false;
            //PanelGridConsultas.Visible = false;
            //hay que hacerlo directo con el panel, porque si no la paginacion no sirve
            this.tituloGrid.Visible = false;
            PanelCrearFactura.Visible = false;
            botonCambioSesion.Visible = false;      //Estos dos botones sólo deben ser visibles
            botonAjusteEntrada.Visible = false;     //durante la creación de facturas

            //Código específico para cada modo
            switch (modo)
            {
                case Modo.Inicial:
                    tituloAccionFacturas.InnerText = "Seleccione una opción";
                    break;
                case Modo.Consulta:
                    tituloAccionFacturas.InnerText = "Seleccione datos para consultar";
                    PanelConsultarFacturas.Visible = true;
                    //this.gridViewFacturas.Visible = false;
                    //this.tituloGrid.Visible = false;
                    llenarGrid();
                    break;
                case Modo.Insercion:
                    tituloAccionFacturas.InnerText = "Ingrese los datos de la nueva factura";
                    PanelCrearFactura.Visible = true;
                    botonCambioSesion.Visible = true;  //Estos dos botones sólo deben ser visibles
                    botonAjusteEntrada.Visible = true; //durante la creación de facturas

                    break;
                case Modo.Modificacion:
                    tituloAccionFacturas.InnerText = "Ingrese los nuevos datos para la factura";
                    PanelConsultarFacturaEspecifica.Visible = true;
                    habilitarCampos(true);
                    break;
                case Modo.Consultado:
                    tituloAccionFacturas.InnerText = "Detalles de la factura";
                    PanelConsultarFacturaEspecifica.Visible = true;
                    //PanelGridConsultas.Visible = true;
                    //hay que hacerlo directo con el panel, porque si no la paginacion no sirve
                    this.gridViewFacturas.Visible = true;
                    this.tituloGrid.Visible = true;
                    cargarDropdownListsConsulta();
                    llenarGrid();
                    habilitarCampos(false);
                    break;
                default:  //Algo salió mal
                    mostrarMensaje("warning", "Alerta: ", "Error de interfaz, el 'modo' de la interfaz no se ha reconocido: " + modo);
                    break;
            }
        }

        /*
         * Invocada después de realizar operaciones de base de datos, para que muestre el mensaje de resultado (éxito o error).
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        /*
         * Invocada antes de iniciar operaciones que requieran que los campos no contengan información de ninguna operación previa.
         */
        protected void limpiarCampos()
        {
            //Campos de consulta
            dropDownListConsultaEstacion.SelectedValue = null;
            dropDownListConsultaBodega.SelectedValue = null;
            dropDownListConsultaVendedor.SelectedValue = null;
            //Campos de consulta individual

            //Campos de creación

        }

        /*
         * Invocada al entrar en modo de consulta normal, se debe cargar la lista de opciones en cada dropdownlist pero por separado (una vez que
         * se escoge una opción en una, se pueden cargar las opciones en la siguiente).
         */
        protected void cargarDropdownListsConsulta()
        {
            //Limpiar la lista de opciones para que no se acumulen
            dropDownListConsultaEstacion.Items.Clear();
            dropDownListConsultaBodega.Items.Clear();
            dropDownListConsultaVendedor.Items.Clear();
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            //Dependiendo del perfil del usuario, puede que en la instancia de usuarioLogueado ya estén guardados los datos por default
            switch (Convert.ToInt32(usuarioActual.CodigoPerfil))
            {
                case 4: //Vendedor
                    dropDownListConsultaVendedor.Items.Add(new ListItem(usuarioActual.Nombre,usuarioActual.Codigo));
                    dropDownListConsultaVendedor.SelectedIndex = 0;
                    dropDownListConsultaVendedor.Enabled = false;


                    dropDownListConsultaBodega.Items.Add(new ListItem( (this.Master as SiteMaster).NombreBodegaSesion, (this.Master as SiteMaster).LlaveBodegaSesion));
                    dropDownListConsultaBodega.SelectedIndex = 0;
                    dropDownListConsultaBodega.Enabled = false;

                    cargarEstacionesConsulta();
                    dropDownListConsultaEstacion.Enabled = false;

                    break;
                case 3: //Supervisor

                    cargarAsociadosABodega((this.Master as SiteMaster).LlaveBodegaSesion);

                    dropDownListConsultaBodega.Items.Add(new ListItem( (this.Master as SiteMaster).NombreBodegaSesion, (this.Master as SiteMaster).LlaveBodegaSesion));
                    dropDownListConsultaBodega.SelectedIndex = 0;
                    dropDownListConsultaBodega.Enabled = false;

                    
                    cargarEstacionesConsulta();
                    dropDownListConsultaEstacion.Enabled = false;

                    break;  
                case 2: //Administrador local
                    cargarAsociadosABodega((this.Master as SiteMaster).LlaveBodegaSesion);
                    
                    cargarEstacionesConsulta();
                    dropDownListConsultaEstacion.Enabled = false;

                    cargarBodegas(this.dropDownListConsultaBodega);
                    dropDownListConsultaBodega.SelectedIndex = 1;
                    break;
                default:

                    cargarAsociadosABodega((this.Master as SiteMaster).LlaveBodegaSesion);

                    cargarEstacionesConsulta();
                    dropDownListConsultaEstacion.SelectedIndex = 0;

                    cargarBodegas(this.dropDownListConsultaBodega);
                    dropDownListConsultaBodega.SelectedIndex = 1;
                    //Administrador global y cualquier otro, este switch es extendible a más perfiles
                    break;
            }
        }

        /*
         * Invocada al consultar facturas, dependiendo de los parámetros de consulta se muestran facturas asociadas a:
         * -Una estación o todas
         * -Una bodega de esa estación, o todas las bodegas de esa estación
         * -Un vendedor de esa bodega, o todos los vendedores de esa bodega
         */
        protected void llenarGrid()
        {
            //Importante: estos dropdownlists pueden contener una entidad específica o la palabra "Todas"/"Todos", en el segundo caso se envía "null", la controladora debe entenderlo
            String codigoEstacion = dropDownListConsultaEstacion.SelectedValue;
            String codigoBodega = dropDownListConsultaBodega.SelectedValue;
            String codigoVendedor = dropDownListConsultaVendedor.SelectedValue;
            //TODO: revisar que de los dropdownlists se obtengan las llaves, no los nombres, algo como dropDownList.SelectedItem[1] creo



            DataTable tabla = tablaFacturas();
            int indiceNuevaFactura = -1;
            int i = 0;


            try
            {
                // Cargar facturas
                Object[] datos = new Object[5];

                DataTable facturas = controladoraVentas.consultarFacturas((this.Master as SiteMaster).Usuario.Perfil, codigoVendedor,codigoBodega, codigoEstacion);
                facturasConsultadas = facturas;
                if (facturas.Rows.Count > 0)
                {
                    idArray = new Object[facturas.Rows.Count];
                    foreach (DataRow fila in facturas.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = controladoraSeguridad.consultarNombreDeUsuario(fila[6].ToString());
                        datos[3] = fila[9].ToString();
                        datos[4] = fila[8].ToString();
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = "-";
                    datos[4] = "-";
                    tabla.Rows.Add(datos);
                }
                this.gridViewFacturas.DataSource = tabla;
                this.gridViewFacturas.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * Invocada al escoger una factura en el grid, se muestran todos los detalles de la misma en campos colocados arriba del grid.
         */
        protected void cargarDatosFactura(String consecutivoSeleccionado)
        {
            facturaConsultada = controladoraVentas.consultarFactura(consecutivoSeleccionado);
            setDatosConsultados();
            PanelConsultarFacturaEspecifica.Visible = true;

        }

        /*
         * ???
         */
        protected void cargarAsociadosABodega(String idBodega)
        {
            dropDownListConsultaVendedor.Items.Clear();
            dropDownListConsultaVendedor.Items.Add(new ListItem("", null));
            dropDownListConsultaVendedor.Items.Add(new ListItem("Todos", "Todos"));
            DataTable vendedores = controladoraVentas.asociadosABodega(idBodega);
            foreach (DataRow fila in vendedores.Rows)
            {
                dropDownListConsultaVendedor.Items.Add(new ListItem(controladoraSeguridad.consultarNombreDeUsuario(fila[0].ToString()), fila[0].ToString()));
            }
            dropDownListConsultaVendedor.SelectedIndex = 1;
        }

        /*
         * ???
         */
        protected void consultarFactura(String id)
        {
            seConsulto = true;
            try
            {
                facturaConsultada = controladoraVentas.consultarFactura(id);
                modo = Modo.Consulta;
            }
            catch
            {
                facturaConsultada = null;
                modo = Modo.Inicial;
            }
            cambiarModo();
        }

        /*
         * ???
         */
        protected void setDatosConsultados()
        {
            textBoxFacturaConsultadaConsecutivo.Text=facturaConsultada.Consecutivo;
            textBoxFacturaConsultadaEstacion.Text=controladoraSeguridad.consultarNombreDeEstacion(facturaConsultada.Estacion);
            textBoxFacturaConsultadaBodega.Text = controladoraSeguridad.consultarNombreDeBodega(facturaConsultada.Bodega);
            textBoxFacturaConsultadaFechaHora.Text=facturaConsultada.Fecha;
            textBoxFacturaConsultadaVendedor.Text = controladoraSeguridad.consultarNombreDeUsuario(facturaConsultada.Vendedor);
            textBoxFacturaConsultadaCliente.Text=facturaConsultada.Cliente;
            textBoxFacturaConsultadaTipoMoneda.Text=facturaConsultada.TipoMoneda;
            textBoxFacturaConsultadaMontoTotal.Text=facturaConsultada.MontoTotal.ToString();
            textBoxFacturaConsultadaMetodoPago.Text=facturaConsultada.MetodoPago;
            textBoxFacturaConsultadActividad.Text=facturaConsultada.Actividad;
            textBoxFacturaConsultadaEstado.Text = facturaConsultada.Estado; 

        }

        /*
         * ???
         */
        protected void habilitarCampos(bool habilitar)
        {
            /*
             * 
             * Esto sirve, peeeeero, cuando se bloquean, el estilo se les cambia.
             * Entonces le toca a quien hizo el .aspx, hacer bien la definicion de
             * los comboboxes. A cada elemento TextBox le hacen falta muchos atributos
             * de estilo, y como no los tiene, cuando se hace Enabled = false, el estilo
             * se le despicha feo. En fin, le toca arreglarlo a quien hizo el .aspx.
             * */


            textBoxFacturaConsultadaConsecutivo.Enabled = habilitar;
            textBoxFacturaConsultadaEstacion.Enabled = habilitar;
            textBoxFacturaConsultadaBodega.Enabled = habilitar; ;
            textBoxFacturaConsultadaFechaHora.Enabled = habilitar;
            textBoxFacturaConsultadaVendedor.Enabled = habilitar;
            textBoxFacturaConsultadaCliente.Enabled = habilitar;
            textBoxFacturaConsultadaTipoMoneda.Enabled = habilitar;
            textBoxFacturaConsultadaMontoTotal.Enabled = habilitar;
            textBoxFacturaConsultadaMetodoPago.Enabled = habilitar;
            textBoxFacturaConsultadActividad.Enabled = habilitar;
            textBoxFacturaConsultadaEstado.Enabled = habilitar;
        }

        /*
         * ???
         */
        protected Object[] obtenerDatos()
        {
            Object[] datos = new Object[13];
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            datos[0] = "";
            datos[1] = DateTime.Now.ToString("dd:MMM:yy");
            datos[2] = (this.Master as SiteMaster).LlaveBodegaSesion;
            datos[3] = dropDownListCrearFacturaEstacion.SelectedValue;
            datos[4] = "02";
            datos[5] = "";
            datos[6] = usuarioActual.Codigo;
            datos[7] = dropDownListCrearFacturaCliente.SelectedValue;
            datos[8] = textBoxCrearFacturaTipoCambio.Text;
            datos[9] = dropDownListCrearFacturaMetodoPago.SelectedValue;
            datos[10] = 456.6;
            datos[11] = "Activo";  
            datos[12] = null;
            return datos;
        
        }

        /*
         * ???
         */
        protected DataTable tablaFacturas() 
        {
            DataTable tabla = new DataTable();
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Consecutivo";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Fecha";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Vendedor";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Monto total";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Método de pago";
            tabla.Columns.Add(column);

            return tabla;

        }

        /*
         * ???
         */
        protected void cargarEstaciones() 
        {
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();

            if (estaciones.Rows.Count > 0)
            {
                this.dropDownListCrearFacturaEstacion.Items.Clear();
                foreach (DataRow fila in estaciones.Rows)
                {
                    if ((usuarioActual.Perfil.Equals("Administrador global"))||(usuarioActual.IdEstacion.Equals(fila[0])))
                    {
                        this.dropDownListCrearFacturaEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                    }
                }
            }
            if (usuarioActual.Perfil.Equals("Administrador global"))
            {
                dropDownListCrearFacturaEstacion.Enabled = true;
            }
            else
            {
                dropDownListCrearFacturaEstacion.Enabled = false;
            }
        }

        /*
         * ???
         */
        protected void cargarEstacionesConsulta()
        {
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            int i=0;
            if (estaciones.Rows.Count > 0)
            {
                this.dropDownListConsultaEstacion.Items.Clear();
                this.dropDownListConsultaEstacion.Items.Add(new ListItem("Todas", "Todas"));
                i++;
                foreach (DataRow fila in estaciones.Rows)
                {
                    if ((usuarioActual.Perfil.Equals("Administrador global")) || (usuarioActual.IdEstacion.Equals(fila[0])))
                    {
                        this.dropDownListConsultaEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                        if ((usuarioActual.IdEstacion.Equals(fila[0])) && (!usuarioActual.Perfil.Equals("Administrador global")))
                        {
                            this.dropDownListConsultaEstacion.SelectedIndex = i;
                        }
                        i++;
                    }
                }
            }
        }

        /*
         * ???
         */
        protected void cargarBodegas(DropDownList dropdown)
        {
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            dropdown.Items.Clear();
            DataTable bodegas;
            if (dropdown == this.dropDownListConsultaBodega)
            {
                dropdown.Items.Add(new ListItem("", null));
                dropdown.Items.Add(new ListItem("Todas", "Todas"));
                 bodegas = controladoraBodegas.consultarBodegasDeEstacion(dropDownListConsultaEstacion.SelectedValue);
            }
            else
            {
                 bodegas = controladoraBodegas.consultarBodegasDeEstacion(dropDownListCrearFacturaEstacion.SelectedValue);
            }
            int i = 0;
            if (bodegas.Rows.Count > 0)
            {
                foreach (DataRow fila in bodegas.Rows)
                {
                    if ((usuarioActual.Perfil.Equals("Administrador global")) || (usuarioActual.Perfil.Equals("Administrador local")) || fila[1].ToString().Equals((this.Master as SiteMaster).NombreBodegaSesion))
                    {
                        dropdown.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                    }
                }
            }
            if ((usuarioActual.Perfil.Equals("Administrador global")) || (usuarioActual.Perfil.Equals("Administrador local")))
            {
                dropdown.Enabled = true;
            }
            else
            {
                dropdown.Enabled = false;
            }
        }







        /*
         *****************************************************************************************************************************************************************************
         * Funciones invocadas por eventos en la interfaz (botones, grids, dropdownlists, etc.)
         *****************************************************************************************************************************************************************************
         */

        /*
         * Invocada cuando se da click al botón de "Consultar Facturas", muestra el panel de escoger datos para consultar, NO muestra el grid.
         * Para mostrar el panel, pone la interfaz en modo de consulta y luego se invoca la función de cambiarModo.
         */
        protected void clickBotonConsultarFacturas(object sender, EventArgs e)
        {
            modo = Modo.Consulta;
            cargarDropdownListsConsulta();
            this.gridViewFacturas.Visible = false;
            this.tituloGrid.Visible = false;
                    
            cambiarModo();
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonEjecutarConsulta(object sender, EventArgs e)
        {
            llenarGrid();
            //PanelGridConsultas.Visible = true;
            //hay que hacerlo directo con el panel, porque si no la paginacion no sirve
            this.gridViewFacturas.Visible = true;
            this.tituloGrid.Visible = true;
            tituloAccionFacturas.InnerText = "Seleccione una factura para ver su información detallada";
            //Aquí NO se debe inovcar cambiarModo, ya que el modo no cambia
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonCrearFactura(object sender, EventArgs e)
        {
            cargarEstaciones();
            cargarBodegas(this.dropDownListCrearFacturaBodega);
            textBoxCrearFacturaVendedor.Text = (this.Master as SiteMaster).Usuario.Nombre;
            textBoxCrearFacturaVendedor.Enabled = false;
            modo = Modo.Insercion;
            cambiarModo();
        }

        /*
         * Invocada cuando se da click al botón de "Agregar Producto" a la factura, se revisa que exista primero
         * (el usuario puede escribir lo que quiera, es un textbox), si existe se agrega al grid para luego editar
         * su cantidad y poder aplicarle descuentos (o quitarlo de la factura).
         */
        protected void clickBotonAgregarProductoFactura(object sender, EventArgs e)
        {
            String productoEscogido = textBoxAutocompleteCrearFacturaBusquedaProducto.Text;
            productoEscogido = controladoraVentas.verificarExistenciaProductoLocal((this.Master as SiteMaster).LlaveBodegaSesion, productoEscogido);
        }

        /*
         * Invocada cuando se escoge una factura del grid de consultas para desplegar su información específica
         * (en el panel de consulta específica, el cual ahora reemplazará visualmente al panel de escoger datos para consultar).
         */
        protected void gridViewFacturas_FilaSeleccionada(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewFacturas.PageIndex * this.gridViewFacturas.PageSize)]);
                    consultarFactura(codigo);
                    modo = Modo.Consultado;
                    Response.Redirect("FormVentas.aspx");
                    break;
            }
        }
        
        /*
         * ???
         */
        protected void gridViewFacturas_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid();
            this.gridViewFacturas.PageIndex = e.NewPageIndex;
            this.gridViewFacturas.DataBind();
        }

        /*
         * ???
         */
        protected void botonAceptarCambioUsuario_ServerClick(object sender, EventArgs e)
        {
            // Consulta al usuario
            /*EntidadUsuario usuario = controladoraSeguridad.consultarUsuario(inputUsername.Value, inputPassword.Value);

            if (usuario != null)
            {
                // Si me retorna un usuario valido

                // Hacer el usuario logueado visible a todas los modulos
                (this.Master as SiteMaster).Usuario = usuario;
                // Redirigir a pagina principal
            }
            else
            {
                // Si no me retorna un usuario valido, advertir
                //mostrarMensaje();
            }* */

            String [] resultado = controladoraVentas.insertarFactura(obtenerDatos());

        }

        /*
         * ???
         */
        protected void dropDownListCrearFacturaEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarBodegas(this.dropDownListCrearFacturaBodega);
        }

        /*
         * ???
         */
        protected void dropDownListConsultaBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarAsociadosABodega(dropDownListConsultaBodega.SelectedValue);
        }

        /*
         * ???
         */
        protected void dropDownListConsultaEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarBodegas(this.dropDownListConsultaBodega);
        }

        protected void botonAceptarAjusteRapido_ServerClick(object sender, EventArgs e)
        {
            String productoEscogido = textBoxAutocompleteAjusteRapidoBusquedaProducto.Text;
            productoEscogido = controladoraVentas.verificarExistenciaProductoLocal( (this.Master as SiteMaster).LlaveBodegaSesion, productoEscogido); //TODO: obtener llave de la bodega, no nombre

            Object[] datos = new Object[6];
            datos[0] = "CYCLO106062012145550408008";
            datos[1] = DateTime.Now.ToString("dd-MMM-yy");
            datos[2] = (this.Master as SiteMaster).Usuario.Nombre;
            datos[3] = (this.Master as SiteMaster).Usuario.Codigo;
            datos[4] = "Ajuste realizado para permitir una venta";
            datos[5] = (this.Master as SiteMaster).LlaveBodegaSesion;

            EntidadAjustes nuevoAjusteRapido = new EntidadAjustes(datos);
            
            datos = new Object[5];
            datos[0] = datos[1] = "";
            datos[2] = Convert.ToInt32(nuevaCantidadParaAjusteRapido.Text);
            datos[3] = productoEscogido;
            datos[4] = Convert.ToInt32(nuevaCantidadParaAjusteRapido.Text);


            nuevoAjusteRapido.agregarDetalle(datos);
            String [] resultado = controladoraAjustes.insertarAjuste(nuevoAjusteRapido);
            mostrarMensaje(resultado[0],resultado[1],resultado[2]);
        
        
        
        
        
        }

        protected void botonModificar_ServerClick(object sender, EventArgs e)
        {
            modo = Modo.Modificacion;
            cambiarModo();
        }
    }
}