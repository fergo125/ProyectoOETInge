using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Ventas;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_Actividades;
using ProyectoInventarioOET.App_Code.Modulo_Ajustes;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.Modulo_Productos_Locales;
using ProyectoInventarioOET.Modulo_ProductosGlobales;

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
        private static Modo modo = Modo.Inicial;                                //Indica en qué modo se encuentra la interfaz en un momento cualquiera, de éste depende cuáles elementos son visibles
        private static String permisos = "000000";                              //Permisos utilizados para el control de seguridad
        private static String codigoPerfilUsuario = "";                         //Indica el perfil del usuario, usado para acciones de seguridad para las cuales la string de permisos no basta
        private static Object[] idArray;                                        //Para llevar el control de las facturas consultadas
        private static List<bool> checksProductos;                              //Para guardar cuáles checks han sido marcados en el grid de productos durante el proceso de creación
        private static List<int> cantidadesProductos;                           //Para guardar las cantidades en los textboxes del grid de productos durante el proceso de creación //TODO: usar esto
        private static DataTable productosAgregados;                            //Para llenar el grid de productos al crear una factura
        private static DataTable facturasConsultadas;                           //Para llenar el grid y para mostrar los detalles de cada factura específica
        private static EntidadFacturaVenta facturaConsultada;                   //Entidad de factura para almacenar la consulta de la base de datos
        private static ControladoraVentas controladoraVentas;                   //Para accesar las tablas del módulo y realizar las operaciones de consulta, inserción, modificación y anulación
        private static ControladoraAjustes controladoraAjustes;                 //Controladora de ajustes para trabajar con los ajustes de factura
        private static ControladoraBodegas controladoraBodegas;                 //Controlaodra de bodegas para tramitar las existencias de productos en bodega
        private static ControladoraSeguridad controladoraSeguridad;             //Controladora de seguridad para la comprobación de permisos de usuario y para el cambio de sesión
        private static ControladoraActividades controladoraActividades;         //Para consultar las actividades a las que se puede asociar una nueva factura
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //Para accesar datos generales de la base de datos
        private static ControladoraProductoLocal controladoraProductoLocal ;    //Para revisar existencias de productos
        private static ControladoraProductosGlobales controladoraProductoGlobal;//Para consultar nombre y otra información de los productos al desplegar facturas
        //Importante:
        //Para el codigoPerfilUsuario (que se usa un poco hard-coded), los números son:
        //1 = Administrador global
        //2 = Administrador local
        //3 = Supervisor
        //4 = Vendedor

        /*
         * Función invocada cada vez que se carga la página, encargada de invocar a las funciones que actualizan los elementos visuales de la página.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            //Elementos visuales
            mensajeAlerta.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster

            if (!IsPostBack) //Si es la primera vez que se carga la página
            {
                //Modo
                modo = Modo.Inicial;
                cambiarModo();

                //Controladoras
                controladoraVentas = new ControladoraVentas();
                controladoraBodegas = new ControladoraBodegas();
                controladoraAjustes = new ControladoraAjustes();
                controladoraSeguridad = new ControladoraSeguridad();
                controladoraActividades = new ControladoraActividades();
                controladoraProductoLocal = new ControladoraProductoLocal();
                controladoraProductoGlobal = new ControladoraProductosGlobales();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraVentas.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraBodegas.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraAjustes.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraSeguridad.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraActividades.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraProductoLocal.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraProductoGlobal.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;

                //Seguridad
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Facturacion");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                codigoPerfilUsuario = (this.Master as SiteMaster).Usuario.CodigoPerfil;
                mostrarElementosSegunPermisos();

                //Otros
                checksProductos = new List<bool>();
                cantidadesProductos = new List<int>();
            }
            else //si la página fue refrezcada por algún elemento
            {
            }
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
            //dropdownEstado.Enabled = (permisos[2] == '1'); //TODO: revisar esto
        }

        /*
         * Función invocada cada vez que se cambia de modo en la interfaz, se encarga de mostrar u ocultar, habilitar o deshabilitar,
         * elementos visuales, dependiendo del modo al que se está entrando.
         */
        protected void cambiarModo()
        {
            //Código común (que debe ejecutarse en la mayoría de modos, en la minoría luego es arreglado en el switch) Reduce un poco la eficiencia, pero simplifica el código bastante
            PanelConsultar.Visible = false;                     //Consulta general
            PanelConsultarFacturaEspecifica.Visible = false;    //Consulta específica
            tituloGrid.Visible = false;                         //Grid de consulta
            gridViewFacturas.Visible = false;                   //Grid de consulta
            PanelCrearFactura.Visible = false;                  //Crear factura
            botonModificar.Disabled = true;                     //Modificar factura
            botonCambioSesion.Visible = false;                  //Cambio sesión rápido
            botonAjusteEntrada.Visible = false;                 //Ajuste inventario rápido
            //PanelGridConsultas.Visible = false; //hay que hacerlo directo con el panel, porque si no la paginacion no sirve

            //Código específico para cada modo
            switch (modo)
            {
                case Modo.Inicial:
                    tituloAccionForm.InnerText = "Seleccione una opción";
                    break;
                case Modo.Consulta:
                    tituloAccionForm.InnerText = "Seleccione datos para consultar";
                    PanelConsultar.Visible = true;
                    break;
                case Modo.Insercion:
                    tituloAccionForm.InnerText = "Ingrese los datos de la nueva factura";
                    PanelCrearFactura.Visible = true;
                    botonCambioSesion.Visible = true;  //Estos dos botones sólo deben ser visibles
                    botonAjusteEntrada.Visible = true; //durante la creación de facturas
                    break;
                case Modo.Modificacion:
                    tituloAccionForm.InnerText = "Ingrese los nuevos datos para la factura"; //TODO: revisar este mensaje, ya que no se pueden modificar, sólo anular
                    PanelConsultarFacturaEspecifica.Visible = true;
                    botonAceptarModificacionFacturaEspecifica.Visible = true;
                    botonCancelarModificacionFacturaEspecifica.Visible = true;
                    habilitarCampos(true);
                    break;
                case Modo.Consultado:
                    tituloAccionForm.InnerText = "Detalles de la factura";
                    PanelConsultarFacturaEspecifica.Visible = true;
                    gridViewFacturas.Visible = true;
                    tituloGrid.Visible = true;
                    botonModificar.Disabled = false;
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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
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
            productosAgregados = null;
            gridViewCrearFacturaProductos.DataBind();
            labelCrearFacturaPrecioTotal.Text = "";
        }

        /*
         * Invocada al entrar en modo de consulta normal, se debe cargar la lista de opciones en cada dropdownlist pero por separado (una vez que
         * se escoge una opción en una, se pueden cargar las opciones en la siguiente).
         */
        protected void cargarDropdownListsConsulta()
        {
            //Switch para cargar los datos default y los datos escogibles
            switch (Convert.ToInt32((this.Master as SiteMaster).Usuario.CodigoPerfil))
            {
                case 4: //Vendedor
                    dropDownListConsultaVendedor.Items.Add(new ListItem((this.Master as SiteMaster).Usuario.Nombre, (this.Master as SiteMaster).Usuario.Codigo));
                    dropDownListConsultaVendedor.SelectedIndex = 0;                         //Se pone al vendedor automáticamente
                    dropDownListConsultaVendedor.Enabled = false;                           //NO se le permite escoger vendedor
                    cargarDropDownListsAutomaticamente(true, false, true, false);           //Se ponen la bodega y la estación de esa bodega automáticamente (bodega única, x, estación única, x)
                    break;

                case 3: //Supervisor
                    cargarAsociadosABodegas((this.Master as SiteMaster).LlaveBodegaSesion); //Se cargan los posibles vendedores dada la bodega de trabajo de la sesión
                    dropDownListConsultaVendedor.Enabled = true;                            //Sí se le permite escoger vendedor
                    cargarDropDownListsAutomaticamente(true, false, true, false);           //Se ponen la bodega y la estación automáticamente (bodega única, x, estación única, x)
                    break;

                case 2: //Administrador local
                    cargarAsociadosABodegas((this.Master as SiteMaster).LlaveBodegaSesion); //Se cargan los posibles vendedores dada la bodega de trabajo de la sesión
                    dropDownListConsultaVendedor.Enabled = true;                            //Sí se le permite escoger vendedor
                    cargarDropDownListsAutomaticamente(false, false, true, false);          //Se pone la estación de la bodega automáticamente* (x, x, estación única, x)
                    cargarBodegas(this.dropDownListConsultaBodega);                         //*en este caso es importante cargar la estación primero, para luego cargar sus bodegas
                    cargarDropDownListsAutomaticamente(false, true, false, false);          //Se cargan las posibles bodegas, una vez cargadas, se pone de una vez la de sesión, pero se dejan las opciones por si quiere cambiar (x, bodegas múltiples, x, x)
                    break;

                case 1: //Administrador global
                    cargarAsociadosABodegas((this.Master as SiteMaster).LlaveBodegaSesion); //Se cargan los posibles vendedores dada la bodega de trabajo de la sesión
                    dropDownListConsultaVendedor.Enabled = true;                            //Sí se le permite escoger vendedor
                    cargarDropDownListsAutomaticamente(false, false, true, false);          //Se pone la estación de la bodega automáticamente* (x, x, estación única, x)
                    cargarBodegas(this.dropDownListConsultaBodega);                         //*en este caso es importante cargar la estación primero, para luego cargar sus bodegas
                    cargarEstacionesConsulta();                                             //Se cargan las posibles estaciones
                    cargarDropDownListsAutomaticamente(false, true, false, true);           //Se ponen la bodega y la estación automáticamente (x, bodegas múltiples, x, estaciones múltiples)
                    break;

                default:
                    break;
            }
            //Todos los perfiles pueden escoger estos criterios extra, se limpian, se cargan igual que al crear, pero se ocupa hacer algunos arreglos para consulta
            dropDownListConsultaMetodoPago.Items.Clear();
            dropDownListConsultaCliente.Items.Clear();
            cargarMetodosPago(dropDownListConsultaMetodoPago);
            cargarPosiblesClientes(dropDownListConsultaCliente);
            dropDownListConsultaMetodoPago.Items.Insert(0, (new ListItem("Todos", "All"))); //En el caso de métodos de pago, se ocupa insertar la opción de "todos" al tope
            dropDownListConsultaCliente.Items[0].Text = "Todos";    //En el caso de los posibles clientes, se reemplaza la opción de dejar el campo vacío por
            dropDownListConsultaCliente.Items[0].Value = "All";     //escoger "todos" los clientes, lo mismo que decir cualquier cliente
        }

        /*
         * Invocada al cargar los dropdownlists de consultar facturas. Ya que al iniciar sesión siempre se escoge una bodega de trabajo, ésta ya debe estar
         * escogida para la consulta de facturas, a la vez, si ya se escogió una bodega, ya se escogió la estación donde ésta se encuentra, por lo que esos
         * dos dropdownlists deben cargarse con datos default siempre, sin importar el perfil, lo que importa es que algunos perfiles pueden elegir cambiar
         * esa escogencia que se da por defecto.
         */
        protected void cargarDropDownListsAutomaticamente(bool bodegaUnica, bool buscarBodega, bool estacionUnica, bool buscarEstacion)
        {
            if (bodegaUnica) //Se pone la bodega automáticamente, se usa la que se escogió al iniciar sesión
            {
                dropDownListConsultaBodega.Items.Add(new ListItem((this.Master as SiteMaster).NombreBodegaSesion, (this.Master as SiteMaster).LlaveBodegaSesion));
                dropDownListConsultaBodega.SelectedIndex = 0;
                dropDownListConsultaBodega.Enabled = false; //NO se le permite escoger la bodega
            }
            if (buscarBodega) //Se busca la bodega en la lista de opciones para escogerla y dejarla así desde el principio
            {
                for (short i = 0; i < dropDownListConsultaBodega.Items.Count; ++i)
                {
                    if (dropDownListConsultaBodega.Items[i].Value == (this.Master as SiteMaster).LlaveBodegaSesion) //si las llaves son iguales
                    {
                        dropDownListConsultaBodega.SelectedIndex = i;
                        break;
                    }
                }
                dropDownListConsultaBodega.Enabled = true; // sí se le permite escoger la bodega
            }
            if (estacionUnica || buscarEstacion)
            {
                String[] datosEstacion = controladoraDatosGenerales.consultarEstacionDeBodega(((this.Master as SiteMaster).LlaveBodegaSesion));
                if (datosEstacion != null)
                {
                    if (estacionUnica) //Se pone la estación automáticamente, la estación a la que pertenece la bodega escogida previamente (por consistencia)
                    {
                        dropDownListConsultaEstacion.Items.Add(new ListItem(datosEstacion[0], datosEstacion[1]));
                        dropDownListConsultaEstacion.SelectedIndex = 0;
                        dropDownListConsultaEstacion.Enabled = false; //NO se le permite escoger la estación
                    }
                    if (buscarEstacion) //Se busca la estación en la lista de opciones para escogerla y dejarla así desde el principio
                    {
                        for (short i = 0; i < dropDownListConsultaEstacion.Items.Count; ++i)
                        {
                            if (dropDownListConsultaEstacion.Items[i].Value == datosEstacion[1]) //si las llaves son iguales
                            {
                                dropDownListConsultaEstacion.SelectedIndex = i;
                                break;
                            }
                        }
                        dropDownListConsultaEstacion.Enabled = true; // sí se le permite escoger la estación
                    }
                }
                else
                    mostrarMensaje("warning", "Alerta", "Ocurrió un error al intentar obtener la estación a la que pertence la bodega actual de trabajo.");
            }
        }

        /*
         * Obtiene desde la base de datos la lista de las estaciones que existen,
         * para que el administrador global pueda escoger una y consultar sus facturas.
         */
        protected void cargarEstacionesConsulta()
        {
            dropDownListConsultaEstacion.Items.Clear();
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            if (estaciones.Rows.Count > 0)
            {
                dropDownListConsultaEstacion.Items.Add(new ListItem("Todas", "All"));
                foreach (DataRow fila in estaciones.Rows)
                    if ((usuarioActual.CodigoPerfil.Equals("1")) || (usuarioActual.IdEstacion.Equals(fila[0])))
                        this.dropDownListConsultaEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        /*
         * Obtiene desde la base de datos la lista de las bodegas asociadas a una estación (o a todas),
         * para que el administrador global, o el administrador local pueda escoger y consultar sus facturas.
         */
        protected void cargarBodegas(DropDownList dropdown)
        {
            dropdown.Items.Clear();
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            DataTable bodegas = null;
            if (dropdown == dropDownListConsultaBodega)
            {
                dropdown.Items.Add(new ListItem("Todas", "All"));
                bodegas = controladoraBodegas.consultarBodegasDeEstacion(dropDownListConsultaEstacion.SelectedValue);
            }
            if (bodegas.Rows.Count > 0)
            {
                foreach (DataRow fila in bodegas.Rows)
                    dropdown.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        /*
         * Obtiene desde la base de datos la lista de los vendedores asociados a una bodega (o a todas),
         * para que el administrador global, o el administrador local, o el supervisor pueda escoger y consultar sus facturas.
         */
        protected void cargarAsociadosABodegas(String idBodega)
        {
            dropDownListConsultaVendedor.Items.Clear();
            dropDownListConsultaVendedor.Items.Add(new ListItem("Todos", "All"));
            DataTable vendedores = null;
            if (idBodega == "All") //si se escogió "Todas las bodegas"
                if (dropDownListConsultaEstacion.SelectedValue == "All") //si se escogió "Todas las estaciones"
                    vendedores = controladoraVentas.asociadosABodegas("All", "All"); //escoge todas las estaciones, todas las bodegas
                else //si se escogió una estación específica
                    vendedores = controladoraVentas.asociadosABodegas("All", dropDownListConsultaEstacion.SelectedValue); //Para el caso en que se consultan todos los vendedores de todas las bodegas de una estación
            else //si se escogió una bodega específica
                vendedores = controladoraVentas.asociadosABodegas(idBodega, null);
            short i = 1; //dejar en 1
            foreach (DataRow fila in vendedores.Rows)
            {
                dropDownListConsultaVendedor.Items.Add(new ListItem(controladoraSeguridad.consultarNombreDeUsuario(fila[0].ToString()), fila[0].ToString()));
                if (fila[0].ToString() == (this.Master as SiteMaster).Usuario.Codigo)
                    dropDownListConsultaVendedor.SelectedIndex = i;
                ++i;
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
            //Importante: estos dropdownlists pueden contener una entidad específica o la palabra "Todas"/"Todos", en el segundo caso se envía "All", la controladora debe entenderlo
            String codigoEstacion = dropDownListConsultaEstacion.SelectedValue;
            String codigoBodega = dropDownListConsultaBodega.SelectedValue;
            String codigoVendedor = dropDownListConsultaVendedor.SelectedValue;
            String codigoMetodoPago = dropDownListConsultaMetodoPago.SelectedValue;
            String codigoCliente = dropDownListConsultaCliente.SelectedValue;
            String fechaInicio="", fechaFinal = "";
            if (checkboxConsultaDetalles.Checked)
            {
                //fechaInicio = CalendarFechaInicio.SelectedDate.ToString("dd/MM/yyyy").Substring(0, 10);
                //fechaFinal = CalendarFechaFinal.SelectedDate.ToString("dd/MM/yyyy").Substring(0, 10);
                fechaInicio = textboxConsultaFechaInicio.Value.ToString();
                fechaFinal = textboxConsultaFechaFinal.Value.ToString();
            }

            DataTable tablaFacturas = crearTablaFacturasConsultadas(); //tabla que se usará para el grid
            try
            {
                Object[] datos = new Object[6];
                facturasConsultadas = null; //tabla atributo que se usará para almacenar toda la consulta
                facturasConsultadas = controladoraVentas.consultarFacturas(codigoVendedor, codigoBodega, codigoEstacion, codigoMetodoPago, codigoCliente, fechaInicio, fechaFinal);
                if (facturasConsultadas.Rows.Count > 0)
                {
                    idArray = new Object[facturasConsultadas.Rows.Count];
                    int i = 0;
                    foreach (DataRow fila in facturasConsultadas.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[0].ToString();  //Consecutivo
                        datos[1] = fila[1].ToString();  //Fecha y hora
                        datos[2] = controladoraSeguridad.consultarNombreDeUsuario(fila[6].ToString());  //Vendedor
                        datos[3] = fila[10].ToString(); //Monto total
                        datos[4] = fila[8].ToString();  //Tipo moneda
                        datos[5] = fila[9].ToString();  //Método de pago
                        tablaFacturas.Rows.Add(datos);
                        i++;
                    }
                }
                else
                {
                    mostrarMensaje("warning", "Alerta", "No hay facturas asociadas a ese vendedor.");
                    //evitar esto ya que crea una fila con un botón de consultar, el cual provoca que el sistema se caiga ya que consulta una tupla vacía
                    //datos[0] = "-";
                    //datos[1] = "-";
                    //datos[2] = "-";
                    //datos[3] = "-";
                    //datos[4] = "-";
                    //tabla.Rows.Add(datos);
                }
                gridViewFacturas.DataSource = tablaFacturas;
                gridViewFacturas.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * Método de controladora que consulta la factura de la base de datos con la llave seleccionada
         */
        protected void consultarFactura(String idFactura)
        {
            try
            {
                facturaConsultada = controladoraVentas.consultarFactura(idFactura);
                setDatosConsultados();
                habilitarCampos(false);
                modo = Modo.Consultado;
            }
            catch
            {
                facturaConsultada = null;
                modo = Modo.Consulta;
                mostrarMensaje("danger", "Error: ", "Error al intentar cargar los datos de esa factura específica.");
            }
            cambiarModo();
        }

        /*
         * Método de interfaz que pone en los datos de la factura consultada en la interfaz
         */
        protected void setDatosConsultados()
        {
            textBoxFacturaConsultadaEstado.Items.Clear();
            textBoxFacturaConsultadaEstado.Items.Add(new ListItem("Activa", "1"));//en la BD el valor de "activo" es 1
            textBoxFacturaConsultadaEstado.Items.Add(new ListItem("Anulada", "5")); //en la BD el valor de "anulado" es 5
            //datos generales de la factura
            textBoxFacturaConsultadaConsecutivo.Value = facturaConsultada.Consecutivo;
            textBoxFacturaConsultadaEstacion.Value = controladoraSeguridad.consultarNombreDeEstacion(facturaConsultada.Estacion);
            textBoxFacturaConsultadaBodega.Value = controladoraSeguridad.consultarNombreDeBodega(facturaConsultada.Bodega);
            textBoxFacturaConsultadaFechaHora.Value = facturaConsultada.FechaHora;
            textBoxFacturaConsultadaVendedor.Value = controladoraSeguridad.consultarNombreDeUsuario(facturaConsultada.Vendedor);
            textBoxFacturaConsultadaCliente.Value = facturaConsultada.Cliente;
            textBoxFacturaConsultadaTipoMoneda.Value = facturaConsultada.TipoMoneda;
            textBoxFacturaConsultadaMontoTotal.Value = facturaConsultada.MontoTotalColones.ToString();
            textBoxFacturaConsultadaMetodoPago.Value = controladoraVentas.consultarMetodoDePago(facturaConsultada.MetodoPago);
            textBoxFacturaConsultadActividad.Value = facturaConsultada.Actividad;
            textBoxFacturaConsultadaEstado.SelectedIndex = (facturaConsultada.Estado.Equals("1") ? 0 : 1);
            //datos de los productos de la factura
            DataTable productosAsociados = crearTablaProductosFactura(true, true);
            EntidadProductoGlobal productoConsultado;
            DataRow filaNuevaProducto;
            foreach(DataRow fila in facturaConsultada.Productos.Rows)
            {
                productoConsultado = controladoraProductoGlobal.consultarProductoGlobal(fila[1].ToString()); //se consulta usando la llave
                filaNuevaProducto = productosAgregados.NewRow();
                filaNuevaProducto["Nombre"] = productoConsultado.Nombre;                                                //nombre
                filaNuevaProducto["Código interno"] = productoConsultado.Codigo;                                        //código interno
                filaNuevaProducto["Precio unitario"] = (facturaConsultada.TipoMoneda == "Colones" ? fila[3] : fila[4]); //precio unitario (buscar en la BD)
                filaNuevaProducto["Cantidad"] = fila[2];                                                                //cantidad
                filaNuevaProducto["Impuesto (13%)"] = (fila[6].ToString() == "1" ? "Sí" : "No");                        //impuesto (booleano)
                filaNuevaProducto["Descuento (%)"] = fila[5];                                                           //descuento
                productosAsociados.Rows.Add(filaNuevaProducto);
            }
            gridFacturaEspecificaProductos.DataSource = productosAsociados;
            gridFacturaEspecificaProductos.DataBind();
        }

        /*
         * Método de interfaz que habilita o deshabilita la casilla de estado; el resto de casillas siempre están bloqueadas
         */
        protected void habilitarCampos(bool habilitar)
        {
            textBoxFacturaConsultadaEstado.Enabled = habilitar;
            if (textBoxFacturaConsultadaEstado.SelectedValue.Equals("5"))  //en la BD el valor de "anulado" es 5
                textBoxFacturaConsultadaEstado.Enabled = false;
        }

        /*
         * Creación de la tabla para unir al grid donde irán los datos principales de la factura 
         */
        protected DataTable crearTablaFacturasConsultadas() 
        {
            DataTable tabla = new DataTable();
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Consecutivo";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Fecha y hora";
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
            column.ColumnName = "Tipo moneda";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Método de pago";
            tabla.Columns.Add(column);

            return tabla;
        }

        /*
         * Crea una DataTable para conservar los productos agregados a la factura, o para almacenar toda la información de los mismos que se encuentra
         * en el grid durante el proceso de inserción.
         */
        protected DataTable crearTablaProductosFactura(bool paraInsertar, bool paraMostrar)
        {
            productosAgregados = new DataTable();
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nombre";
            productosAgregados.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Código interno";
            productosAgregados.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Precio unitario";
            productosAgregados.Columns.Add(column);

            if (paraInsertar)
            {
                if (!paraMostrar)
                {
                    column = new DataColumn();
                    column.DataType = Type.GetType("System.Double");
                    column.ColumnName = "Precio unitario dólares";
                    productosAgregados.Columns.Add(column);
                }

                column = new DataColumn();
                column.DataType = Type.GetType("System.Int32");
                column.ColumnName = "Cantidad";
                productosAgregados.Columns.Add(column);
            }

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Impuesto (13%)";
            productosAgregados.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "Descuento (%)";
            productosAgregados.Columns.Add(column);

            if (!paraMostrar)
            {
                column = new DataColumn();
                column.DataType = Type.GetType("System.Double");
                column.ColumnName = "Total";
                productosAgregados.Columns.Add(column);
            }

            return productosAgregados;
        }

        /*
         * Separa la string de un producto escogido en una barra de autocomplete en sus dos partes, nombre y código ([0] y [1] respectivamente).
         */
        protected String[] separarNombreCodigoProductoEscogido(String productoEscogido)
        {
            String[] resultado = new String[2];
            String codigoProductoEscogido = productoEscogido.Substring(productoEscogido.LastIndexOf('(') + 1);  //el código sin el primer paréntesis
            codigoProductoEscogido = codigoProductoEscogido.TrimEnd(')');                                       //el código
            productoEscogido = productoEscogido.Remove(productoEscogido.LastIndexOf('(') - 1);                  //nombre del producto (-1 al final por el espacio)
            resultado[0] = productoEscogido;
            resultado[1] = codigoProductoEscogido;
            return resultado;
        }

        /*
         * Consulta en la BD las diferentes formas de pago para cargarlas en el dropdownlist.
         */
        protected void cargarMetodosPago(DropDownList dropdownlist)
        {
            DataTable metodosPago = controladoraVentas.consultarMetodosPago();
            if(metodosPago != null)
            {
                dropdownlist.Items.Clear();
                foreach (DataRow fila in metodosPago.Rows)
                    dropdownlist.Items.Add(new ListItem(fila[0].ToString(), fila[1].ToString()));
                dropdownlist.Items.Add(new ListItem("Varios (definidos al crear la factura)", "VARIOS"));
            }
            else
                mostrarMensaje("warning", "Alerta: ", "Error al intentar cargar los métodos de pago.");
        }

        /*
         * Consulta en la BD las diferentes formas de pago para cargarlas en el dropdownlist.
         */
        protected void cargarPosiblesClientes(DropDownList dropdownlist)
        {
            DataTable clientes = controladoraVentas.consultarPosiblesClientes();
            if (clientes != null)
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Add(new ListItem("", ""));
                foreach (DataRow fila in clientes.Rows)
                    dropdownlist.Items.Add(new ListItem(fila[0].ToString(), fila[1].ToString()));
            }
            else
                mostrarMensaje("warning", "Alerta: ", "Error al intentar cargar los posibles clientes.");
        }

        /*
         * Consulta en la BD las diferentes actividades a las que se puede asociar la factura.
         */
        protected void cargarActividades()
        {
            DataTable actividadesDisponibles = controladoraActividades.consultarActividades();
            if (actividadesDisponibles != null)
            {
                dropDownListCrearFacturaActividad.Items.Clear();
                dropDownListCrearFacturaActividad.Items.Add(new ListItem("", ""));
                foreach (DataRow fila in actividadesDisponibles.Rows)
                    dropDownListCrearFacturaActividad.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
            else
                mostrarMensaje("warning", "Alerta: ", "Error al intentar cargar las actividades en el sistema.");
        }

        /*
         * Se realizan todas las verificaciones necesarias antes de agregar un producto a una factura, las cuales son:
         * -Revisar que el producto exista en el catálogo global (el usuario puede modificar el texto después de escogerlo en el autocomplete, o escribir lo que sea en realidad)
         * -Revisar que el producto esté asociado al catálogo local de la bodega de trabajo (esto puede filtrarse en el autocomplete de una vez, TODO: hacer eso en el sprint 3)
         * -Revisar que el producto esté activo en ese catálogo (se hace al mismo tiempo que en la consulta anterior)
         * -Revisar que el producto tenga existencia mayor a 1
         * -Revisar que el producto no haya sido agregado ya a la factura
         * Si todo sale bien, retorna la llave del producto para continuar agregando.
         */
        protected String verificarProductoNuevo(String[] nombreCodigoProductoEscogido)
        {
            //Prmero, revisar que el producto no haya sido agregado ya a la factura
            gridViewCrearFacturaProductos.DataSource = productosAgregados; //se refrezca el grid ya que aparentemente en el pageload se vacía
            gridViewCrearFacturaProductos.DataBind();
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
            {
                if (fila.Cells[3].Text == nombreCodigoProductoEscogido[1]) //si el código interno se repite
                {
                    mostrarMensaje("warning", "Alerta: ", "Ese producto ya fue agregado a la factura. En lugar de agregarlo de nuevo puede modificar su cantidad.");
                    return null;
                }
            }
            
            //Segundo, obtener la llave desde el catálogo global usando el nombre y el código interno para revisar si existe global y localmente, y si está activo
            String llaveProductoEscogido = controladoraVentas.verificarExistenciaProductoLocal((this.Master as SiteMaster).LlaveBodegaSesion, nombreCodigoProductoEscogido[0], nombreCodigoProductoEscogido[1]); //trae la llave
            if(llaveProductoEscogido == null)
            {
                mostrarMensaje("warning", "Alerta: ", "Ese producto no está asociado a la bodega " + (this.Master as SiteMaster).NombreBodegaSesion + ", o no existe.");
                return null;
            }

            //Tercero, revisar que tenga existencia mayor a 0
            double existencia = Convert.ToDouble(controladoraProductoLocal.consultarProductoDeBodega((this.Master as SiteMaster).LlaveBodegaSesion, nombreCodigoProductoEscogido[1]).Rows[0][7]);
            if(existencia < 1) //la cantidad default es 1
            {
                mostrarMensaje("warning", "Alerta: ", "No hay suficiente existencia de ese producto disponible para venta en la bodega " + (this.Master as SiteMaster).NombreBodegaSesion + ".");
                return null;
            }

            //Si el producto no fue agregado antes, existe, está localmente, en estado activo, y con existencia, continuar
            return llaveProductoEscogido; //todo salió bien
        }

        /*
         * Agrega un producto a la tabla de productos en la factura y actualiza el grid también, una vez que ya se verificó que se puede.
         */
        protected void agregarProductoAFactura(String[] nombreCodigoProductoEscogido, String llaveProductoEscogido)
        {
            DataRow nuevoProducto;
            nuevoProducto = productosAgregados.NewRow();
            nuevoProducto["Nombre"] = nombreCodigoProductoEscogido[0];                                       //nombre
            nuevoProducto["Código interno"] = nombreCodigoProductoEscogido[1];                               //código interno
            nuevoProducto["Precio unitario"] = (controladoraVentas.consultarPrecioUnitario(llaveProductoEscogido, labelCrearFacturaTipoMoneda.Text)); //precio unitario (buscar en la BD)
            nuevoProducto["Impuesto (13%)"] = "Sí";                                                          //impuesto (booleano)
            nuevoProducto["Descuento (%)"] = "0";                                                            //descuento (siempre empieza con 0)
            nuevoProducto["Total"] = nuevoProducto["Precio unitario"];                                       //total (empieza con precio unitario * 1)
            productosAgregados.Rows.Add(nuevoProducto);
            gridViewCrearFacturaProductos.DataSource = productosAgregados;
            gridViewCrearFacturaProductos.DataBind();
            textBoxAutocompleteCrearFacturaBusquedaProducto.Text = "";
            cantidadesProductos.Add(1); //agregar la existencia inicial (1)
            //Con el databind se borran las cantidades, entonces se restauran, y se pone la nueva default
            if ((checksProductos.Count > 0) || (cantidadesProductos.Count > 0))
                restaurarCheckBoxesYCantidades();
            actualizarPreciosTotales();
        }

        /*
         * Invocada cuando se agregan productos o se cambia el tipo de moneda durante la creación de una factura.
         * Se calcula el descuento antes que el impuesto.
         */
        protected void actualizarPreciosTotales()
        {
            double precioTotal = 0;
            double precioProducto = 0;
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows) //actualiza el "total" de cada fila del grid y va sumando para ponerlo al final
            {
                String cantidad = ((TextBox)fila.FindControl("gridCrearFacturaTextBoxCantidadProducto")).Text;
                precioProducto = Convert.ToDouble(Convert.ToDouble(fila.Cells[4].Text) * Convert.ToInt32(cantidad)); //precio = precio unitario * cantidad
                if(fila.Cells[6].Text != "0") //si tiene descuento
                    precioProducto -= (precioProducto * (Convert.ToDouble(fila.Cells[6].Text) / 100)); //se le resta
                if (fila.Cells[5].Text != "No") //si tiene impuesto
                    precioProducto += (precioProducto * 0.13); //se le suma
                precioProducto = Math.Round(precioProducto, 2, MidpointRounding.AwayFromZero); //se redondea a dos decimales
                fila.Cells[7].Text = precioProducto.ToString(); //se actualiza el total en la línea de la factura
                precioTotal += precioProducto; //se actualiza el total de toda la factura
            }
            labelCrearFacturaPrecioTotal.Text = ((labelCrearFacturaTipoMoneda.Text == "Colones" ? "₡ " : "$ ") + precioTotal.ToString());
            actualizarTablaProductosSegunGrid();
        }

        /*
         * El manejo de precios y totales se realiza en el grid directamente, sin embargo al agregarle nuevos productos se usa
         * la tabla atributo "productosAgregados" (con DataBind), por lo que ésta debe actualizarse también al actualizar el grid.
         * Se utiliza "HttpUtility.HtmlDecode" porque html a veces corrompe caracteres especiales
         */
        protected void actualizarTablaProductosSegunGrid()
        {
            for(int i=0; i<gridViewCrearFacturaProductos.Rows.Count; ++i)
            {
                productosAgregados.Rows[i][0] = HttpUtility.HtmlDecode(gridViewCrearFacturaProductos.Rows[i].Cells[2].Text); //nombre
                productosAgregados.Rows[i][1] = HttpUtility.HtmlDecode(gridViewCrearFacturaProductos.Rows[i].Cells[3].Text); //codigo interno
                productosAgregados.Rows[i][2] = HttpUtility.HtmlDecode(gridViewCrearFacturaProductos.Rows[i].Cells[4].Text); //precio unitario
                productosAgregados.Rows[i][3] = HttpUtility.HtmlDecode(gridViewCrearFacturaProductos.Rows[i].Cells[5].Text); //impuesto
                productosAgregados.Rows[i][4] = HttpUtility.HtmlDecode(gridViewCrearFacturaProductos.Rows[i].Cells[6].Text); //descuento
                productosAgregados.Rows[i][5] = HttpUtility.HtmlDecode(gridViewCrearFacturaProductos.Rows[i].Cells[7].Text); //total
            }
        }

        /*
         * Método de interfaz que toma los datos de la interfaz y los almacena en un vector para trabajar de manera encapsulada
         */
        protected Object[] obtenerDatos()
        {
            String[] datosEstacion = controladoraDatosGenerales.consultarEstacionDeBodega(((this.Master as SiteMaster).LlaveBodegaSesion));
            Object[] datosFactura = new Object[14];
            datosFactura[0] = null;                                                 //consecutivo, se autogenera al insertar a nivel de BD
            datosFactura[1] = DateTime.Now.Date.ToString("dd/MMM/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss tt"); //fecha y hora
            datosFactura[2] = ((this.Master as SiteMaster).LlaveBodegaSesion);      //bodega (llave)
            datosFactura[3] = datosEstacion[1];                                     //estacion (llave)
            datosFactura[4] = "02";                                                 //compania, siempre esintro (llave)
            datosFactura[5] = dropDownListCrearFacturaActividad.SelectedValue;      //actividad (llave)
            datosFactura[6] = ((this.Master as SiteMaster).Usuario.Codigo);         //vendedor (llave)
            datosFactura[7] = dropDownListCrearFacturaCliente.SelectedValue;        //cliente usuario (llave)
            datosFactura[8] = labelCrearFacturaTipoMoneda.Text;                     //tipo moneda
            datosFactura[9] = dropDownListCrearFacturaMetodoPago.SelectedValue;     //metodo de pago (llave)
            datosFactura[12] = 1;                                                   //estado (1=activo por default)
            if (labelCrearFacturaTipoMoneda.Text == "Colones")
            {
                datosFactura[10] = Convert.ToDouble(labelCrearFacturaPrecioTotal.Text.Substring(2));  //montoTotalColones (substring para omitir el símbolo de la moneda)
                datosFactura[11] = Math.Round((Convert.ToDouble(datosFactura[11]) / Convert.ToInt32(textBoxCrearFacturaTipoCambio.Text)), 2, MidpointRounding.AwayFromZero);  //montoTotalDolares (colones / tipocambio)
            }
            else //"Dólares"
            {
                datosFactura[11] = Convert.ToDouble(labelCrearFacturaPrecioTotal.Text.Substring(2));  //montoTotalDolares (substring para omitir el símbolo de la moneda)
                datosFactura[10] = Math.Round((Convert.ToDouble(datosFactura[11]) * Convert.ToInt32(textBoxCrearFacturaTipoCambio.Text)), 2, MidpointRounding.AwayFromZero);  //montoTotalColones (dólares * tipocambio)
            }
            datosFactura[13] = obtenerProductosAgregados();                         //tabla de productos
            return datosFactura;
        }

        /*
         * Obtiene una tabla con base en el grid de productos en la factura que se está creando.
         */
        protected DataTable obtenerProductosAgregados()
        {
            DataTable productos = crearTablaProductosFactura(true, false); //con la columna de cantidad
            DataRow detallesProducto;
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
            {
                detallesProducto = productos.NewRow();
                detallesProducto[0] = HttpUtility.HtmlDecode(fila.Cells[2].Text); //nombre
                detallesProducto[1] = HttpUtility.HtmlDecode(fila.Cells[3].Text); //codigo interno
                detallesProducto[6] = HttpUtility.HtmlDecode(fila.Cells[6].Text); //impuesto
                detallesProducto[7] = HttpUtility.HtmlDecode(fila.Cells[7].Text); //total
                detallesProducto[5] = (HttpUtility.HtmlDecode(fila.Cells[5].Text) == "No" ? 0 : 1); //descuento
                detallesProducto[4] = Convert.ToInt32(((TextBox)fila.FindControl("gridCrearFacturaTextBoxCantidadProducto")).Text); //cantidad
                if (labelCrearFacturaTipoMoneda.Text == "Colones")
                {
                    detallesProducto[2] = HttpUtility.HtmlDecode(fila.Cells[4].Text); //precio unitario colones
                    detallesProducto[3] = Math.Round((Convert.ToDouble(detallesProducto[2]) / Convert.ToInt32(textBoxCrearFacturaTipoCambio.Text)), 2, MidpointRounding.AwayFromZero); //precio unitario dólares
                }
                else //dólares
                {
                    detallesProducto[3] = HttpUtility.HtmlDecode(fila.Cells[4].Text); //precio unitario dólares
                    detallesProducto[2] = Math.Round((Convert.ToDouble(detallesProducto[3]) * Convert.ToInt32(textBoxCrearFacturaTipoCambio.Text)), 2, MidpointRounding.AwayFromZero); //precio unitario colones
                }
                productos.Rows.Add(detallesProducto);
            }
            return productos;
        }

        /*
         * Al darse el PageLoad, se borran los checkboxes marcados, por lo que se usa la lista de booleanos para restaurarlos.
         */
        protected void restaurarCheckBoxesYCantidades()
        {
            for (int i = 0; i < gridViewCrearFacturaProductos.Rows.Count; ++i)
            {
                if (i < checksProductos.Count) //para evitar que intente accesar posiciones inexistentes
                    ((CheckBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturaCheckBoxSeleccionarProducto")).Checked = checksProductos[i];
                if (i < cantidadesProductos.Count) //para evitar que intente accesar posiciones inexistentes
                    if (((TextBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturaTextBoxCantidadProducto")).Text == "") //para evitar que borre lo nuevo
                        ((TextBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturaTextBoxCantidadProducto")).Text = cantidadesProductos[i].ToString();
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
        protected void clickBotonConsultar(object sender, EventArgs e)
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
            gridViewFacturas.Visible = true;
            tituloGrid.Visible = true;
            tituloAccionForm.InnerText = "Seleccione una factura para ver su información detallada";
            //Aquí NO se debe invocar cambiarModo, ya que el modo no cambia
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonCrear(object sender, EventArgs e)
        {
            limpiarCampos();
            //Cargar datos default y opcionales
            textBoxCrearFacturaEstacion.Text = controladoraDatosGenerales.consultarEstacionDeBodega((this.Master as SiteMaster).LlaveBodegaSesion)[0]; //nombre de la estación a la que pertenece la bodega de trabajo
            textBoxCrearFacturaBodega.Text = (this.Master as SiteMaster).NombreBodegaSesion; //nombre de la bodega de trabajo (punto de venta)
            textBoxCrearFacturaVendedor.Text = (this.Master as SiteMaster).Usuario.Nombre;
            textBoxCrearFacturaTipoCambio.Text = controladoraVentas.consultarTipoCambio().ToString();
            cargarMetodosPago(dropDownListCrearFacturaMetodoPago);
            cargarActividades();

            modo = Modo.Insercion;
            cambiarModo();
            productosAgregados = crearTablaProductosFactura(false, false); //se crea una nueva tabla cada vez, sin la columna de cantidad
            labelCrearFacturaTipoMoneda.Text = "Colones"; //por default

            //Se limpian para que no conserven datos de facturas anteriores
            checksProductos.Clear();
            cantidadesProductos.Clear();
        }

        /*
         * Invocada cuando se da click al botón de "Agregar Producto" a la factura, se revisa que exista primero
         * (el usuario puede escribir lo que quiera, es un textbox), si existe se agrega al grid para luego editar
         * su cantidad y poder aplicarle descuentos (o quitarlo de la factura).
         */
        protected void clickBotonAgregarProductoFacturaNueva(object sender, EventArgs e)
        {
            //Antes que nada, asegurarse de que el textbox tiene algo
            if (textBoxAutocompleteCrearFacturaBusquedaProducto.Text == "")
                return;
            //Primero, obtener nombre y código del producto ingresado
            String productoEscogido = textBoxAutocompleteCrearFacturaBusquedaProducto.Text;
            String[] nombreCodigoProductoEscogido = separarNombreCodigoProductoEscogido(productoEscogido);
            //Segundo, verificar si se puede agregar, si sí, se agrega
            if ((productoEscogido = verificarProductoNuevo(nombreCodigoProductoEscogido)) != null)
                agregarProductoAFactura(nombreCodigoProductoEscogido, productoEscogido);
        }

        /*
         * Invocada cuando se desea cambiar el tipo de moneda de la factura, afecta toda la lista de precios y el total.
         * Se usa "Math.Round" para redondear, a 2 decimales, con MidpointRounding.AwayFromZero para mayor exactitud.
         */
        protected void clickBotonCambiarTipoMoneda(object sender, EventArgs e)
        {
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
                if (labelCrearFacturaTipoMoneda.Text == "Colones") //pasar a dólares
                    fila.Cells[4].Text = (Math.Round((Convert.ToDouble(fila.Cells[4].Text) / Convert.ToDouble(textBoxCrearFacturaTipoCambio.Text)), 2, MidpointRounding.AwayFromZero)).ToString(); //fila[4] es el precio unitario
                else //pasar a colones
                    fila.Cells[4].Text = (Math.Round((Convert.ToDouble(fila.Cells[4].Text) * Convert.ToDouble(textBoxCrearFacturaTipoCambio.Text)), 2, MidpointRounding.AwayFromZero)).ToString(); //fila[4] es el precio unitario

            labelCrearFacturaTipoMoneda.Text = (labelCrearFacturaTipoMoneda.Text == "Colones" ? "Dólares" : "Colones");
            actualizarPreciosTotales();
            actualizarTablaProductosSegunGrid();
        }

        /*
         * Elimina el producto de la factura que esté marcado (seleccionado, con el checkbox).
         */
        protected void clickBotonCrearEliminarProducto(object sender, EventArgs e)
        {
            for(int i=0; i<gridViewCrearFacturaProductos.Rows.Count; ++i)
            {
                if(((CheckBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturaCheckBoxSeleccionarProducto")).Checked) //se encontró, eliminar esta fila de todo lado
                {
                    productosAgregados.Rows.RemoveAt(i); //se remueve de la tabla
                    checksProductos.RemoveAt(i); //se remueve de la lista de booleanos de checks
                    cantidadesProductos.RemoveAt(i); //se remueve de la lista de cantidades
                    gridViewCrearFacturaProductos.DataSource = productosAgregados;
                    gridViewCrearFacturaProductos.DataBind(); //se refrezca el grid
                    break;
                }
            }
            restaurarCheckBoxesYCantidades();
        }

        /*
         * Edita las propiedades de descuento e impuesto del producto seleccionado.
         */
        protected void clickBotonAceptarModalModificarProducto(object sender, EventArgs e)
        {
            for(int i=0; i<gridViewCrearFacturaProductos.Rows.Count; ++i)
            {
                if(((CheckBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturaCheckBoxSeleccionarProducto")).Checked) //se encontró la fila
                {
                    productosAgregados.Rows[i][4] = dropdownlistModalModificarProductoDescuento.SelectedValue;
                    productosAgregados.Rows[i][3] = dropdownlistModalModificarProductoImpuesto.SelectedValue;
                    gridViewCrearFacturaProductos.DataSource = productosAgregados;
                    gridViewCrearFacturaProductos.DataBind();
                    restaurarCheckBoxesYCantidades();
                    break;
                }
            }
            actualizarPreciosTotales();
        }

        /*
         * Invocada cuando termina de hacer la factura y se envia a la base de datos.
         */
        protected void clickBotonCrearGuardar(object sender, EventArgs e)
        {
            //Antes que nada, revisar que hayan productos, si no, no se puede guardar la factura
            if(productosAgregados.Rows.Count < 1)
            {
                mostrarMensaje("warning", "Alerta: ", "No puede guardarse una factura vacía, sin productos.");
                return;
            }
            Object[] datosFactura = obtenerDatos();
            String[] resultado = controladoraVentas.insertarFactura(datosFactura);
            mostrarMensaje(resultado[0], resultado[1], resultado[2]);
            if (resultado[0] == "success") //si todo salió bien, reiniciar la interfaz para insertar más facturas
                clickBotonCrear(sender, e);


            //blopa
            //DataTable productoConsultado;
            //bool alerta = false;
            //bool error = false;
            //for (int i = 0; i < productosAgregados.Rows.Count && !error; i++)
            //{
            //    productoConsultado = controladoraProductoLocal.consultarProductoDeBodega((this.Master as SiteMaster).LlaveBodegaSesion, productosAgregados.Rows[i][2].ToString());
            //    double cantidad = Convert.ToDouble(productosAgregados.Rows[i][0].ToString());
            //    double existencias = Convert.ToDouble(productoConsultado.Rows[0][7]);
            //    double minimo = Convert.ToDouble(productoConsultado.Rows[0][13]);
            //    error = existencias < cantidad;
            //    alerta |= (existencias - cantidad) >= minimo;
            //}
            ////Con que un producto no cumpla esto, hay que mostrar un mensaje de error e interrumpir todo
            //if (!error)
            //{

            //    if (alerta)
            //    {
            //        resultado[0] = "warning";
            //        resultado[2] += "\nUno o más productos han salido de sus límites permitidos (nivel máximo o mínimo), revise el catálogo local.";
            //    }
            //}
        }

        /*
         * Invocada al dar click al botón de Cancelar durante el proceso de creación. Limpia los campos y vuelve al modo de creación.
         */
        protected void clickBotonAceptarModalCancelar(object sender, EventArgs e)
        {
            clickBotonCrear(sender, e); //se hace prácticamente lo mismo en ambas funciones
        }

        /*
         * Evento de disparo que cambia el modo a modificacón tras haber presionado el botón Modificar para iniciar un cambio
         */
        protected void clickBotonModificar(object sender, EventArgs e)
        {
            modo = Modo.Modificacion;
            cambiarModo();
        }

        /*
         * Método que consolida el cambio a la factura
         */
        protected void clickBotonAceptarModificar(object sender, EventArgs e)
        {
            String[] error = controladoraVentas.anularFactura(facturaConsultada);
            mostrarMensaje(error[0], error[1], error[2]);

            if (error[0].Contains("success"))// si fue exitoso
            {
                llenarGrid();
                facturaConsultada = controladoraVentas.consultarFactura(facturaConsultada.Consecutivo);
                modo = Modo.Consulta;
            }
            else
            {
                modo = Modo.Modificacion;
            }
        }

        /*
         * Método de cancelar modificación
         */
        protected void clickBotonCancelarModificar(object sender, EventArgs e)
        {

        }

        /*
         * Evento que hace el cambio de usuario rápido
         */
        protected void clickBotonAceptarCambioUsuario(object sender, EventArgs e)
        {
            // Consulta al usuario
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            EntidadUsuario usuario = controladoraSeguridad.consultarUsuario(inputUsername.Value, inputPassword.Value);

            if (usuario != null)
            {
                // Si me retorna un usuario valido
                // Hacer el usuario logueado visible a todas los modulos
                (this.Master as SiteMaster).Usuario = usuario;
                (this.Master as SiteMaster).cambiarSesion(usuario.Nombre,usuario.Perfil,(this.Master as SiteMaster).NombreBodegaSesion);
                this.textBoxCrearFacturaVendedor.Text = (this.Master as SiteMaster).Usuario.Nombre;
                // Redirigir a pagina principal
            }
            else
            {
                mostrarMensaje("danger", "Error: ", "Credenciales inválidas ingresadas para cambio de usuario.");
            }
        }

        /*
         * Método que realiza el ajuste rápido 
         */
        protected void clickBotonAceptarAjusteRapido(object sender, EventArgs e)
        {
            String productoEscogido = textBoxAutocompleteAjusteRapidoBusquedaProducto.Text;
            String[] nombreCodigoProductoEscogido = separarNombreCodigoProductoEscogido(productoEscogido);
            String llaveProductoEscogido = controladoraVentas.verificarExistenciaProductoLocal((this.Master as SiteMaster).LlaveBodegaSesion, nombreCodigoProductoEscogido[0], nombreCodigoProductoEscogido[1]);

            Object[] datos = new Object[6];
            datos[0] = "CYCLO106062012145550408008"; //Todas las ventas están asociadas a ESYNTRO
            datos[1] = DateTime.Now.ToString("dd-MMM-yy");
            datos[2] = (this.Master as SiteMaster).Usuario.Nombre;
            datos[3] = (this.Master as SiteMaster).Usuario.Codigo;
            datos[4] = "Ajuste realizado para permitir una venta"; //Todos los ajustes rápidos son de esta categoría; no se deberia pedir al usuario que indique el tipo, siempre es fijo
            datos[5] = (this.Master as SiteMaster).LlaveBodegaSesion;

            EntidadAjustes nuevoAjusteRapido = new EntidadAjustes(datos);
            
            datos = new Object[5];
            datos[0] = nombreCodigoProductoEscogido[0];
            datos[1] = nombreCodigoProductoEscogido[1];
            datos[2] = Convert.ToInt32(nuevaCantidadParaAjusteRapido.Text);
            datos[3] = controladoraVentas.getLlaveProductoBodega(llaveProductoEscogido);
            datos[4] = Convert.ToInt32(nuevaCantidadParaAjusteRapido.Text);

            nuevoAjusteRapido.agregarDetalle(datos);
            String [] resultado = controladoraAjustes.insertarAjuste(nuevoAjusteRapido);
            mostrarMensaje(resultado[0],resultado[1],resultado[2]);
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
                    break;
            }
        }

        /*
         * Método que se encarga del cambio de página del grid
         */
        protected void gridViewFacturas_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid(); 
            gridViewFacturas.PageIndex = e.NewPageIndex;
            gridViewFacturas.DataBind();
            tituloGrid.Visible = true;
            gridViewFacturas.Visible = true;
        }

        /*
         * Evento que cada vez que se selecciona una estación, refresca las bodegas de esa estación
         */
        protected void dropDownListConsultaEstacion_ValorCambiado(object sender, EventArgs e)
        {
            cargarBodegas(this.dropDownListConsultaBodega);
            dropDownListConsultaVendedor.Items.Clear(); //para evitar que escoja vendedores cargados de bodegas anteriores
        }

        /*
         * Evento que cada vez que se selecciona una bodega, refresca los asociados a esa bodega
         */
        protected void dropDownListConsultaBodega_ValorCambiado(object sender, EventArgs e)
        {
            cargarAsociadosABodegas(dropDownListConsultaBodega.SelectedValue);
        }

        /*
         * Cuando se escoja el método de pago "deducción de planilla", se debe cargar los empleados de la OET como posibles clientes.
         */
        protected void dropDownListCrearFacturaMetodoPago_ValorCambiado(object sender, EventArgs e)
        {
            //Deberia hacerse solo cuando escoja "deduccion de planilla"
            cargarPosiblesClientes(dropDownListCrearFacturaCliente);
            if (dropDownListCrearFacturaMetodoPago.SelectedValue == "VARIOS")
            {
                botonCrearFacturaVariosMetodosPago.Visible = true;
                dropDownListCrearFacturaMetodoPago.Style["width"] = "100%";
            }
            else
            {
                botonCrearFacturaVariosMetodosPago.Visible = false;
                dropDownListCrearFacturaMetodoPago.Style["width"] = "160.5%%";
            }
        }

        /*
         * Maneja el evento en el que se marca o desmarca un checkbox de seleccionar un producto en el grid durante la creación de una factura.
         * Si se marca deben desmarcarse los demás checkboxes. También guarda el estado de todos para poder usar los botones de eliminar y editar.
         */
        protected void checkBoxCrearFacturaProductos_CheckCambiado(object sender, EventArgs e)
        {
            botonCrearFacturaEliminarProducto.Disabled = !((CheckBox)sender).Checked;
            botonCrearFacturaModificarProducto.Disabled = !((CheckBox)sender).Checked;
            checksProductos.Clear();
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
            {
                //Primero, desmarcar todos los otros checkboxes al marcar uno nuevo
                CheckBox check = (CheckBox)fila.FindControl("gridCrearFacturaCheckBoxSeleccionarProducto");
                if (((CheckBox)sender).Checked && (check.Checked) && (check != (CheckBox)sender))
                    check.Checked = false;
                //Segundo, guardar el estado de cada checkbox
                checksProductos.Add(check.Checked);
                //Tercero, cargar los posibles porcentajes de descuento del producto dentro del modal desde ya
                if(check == (CheckBox)sender) //se encontró la fila
                {
                    int maximoDescuento = controladoraVentas.maximoDescuentoAplicable(controladoraVentas.verificarExistenciaProductoLocal((this.Master as SiteMaster).LlaveBodegaSesion, fila.Cells[2].ToString(), fila.Cells[3].ToString()), (this.Master as SiteMaster).Usuario.Codigo);
                    dropdownlistModalModificarProductoDescuento.Items.Clear();
                    for (int i = 0; i <= maximoDescuento; ++i)
                        dropdownlistModalModificarProductoDescuento.Items.Add(new ListItem(i + "%",i.ToString()));
                }
            }
        }

        /*
         * Se usa para hacer visible o invisible la fila que contiene los campos extra para consultas de facturas más detalladas (método de pago, cliente, y fechas de inicio y final).
         */
        protected void checkBoxConsultarDetalles_CheckCambiado(object sender, EventArgs e)
        {
            detallesConsulta.Visible = (((CheckBox)sender).Checked);
        }

        /*
         * Maneja el evento en el que se modifica la cantidad de un producto que está en la nueva factura, guarda todas las cantidades
         * para poder restaurarlas al darse el pageload.
         */
        protected void textBoxCrearFacturaProductosCantidad_TextoCambiado(object sender, EventArgs e)
        {
            int i = 0;
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
            {
                if (((TextBox)fila.FindControl("gridCrearFacturaTextBoxCantidadProducto")) == (TextBox)sender) //es la fila donde está el textbox que fue cambiado
                {
                    try
                    {
                        cantidadesProductos[i] = Convert.ToInt32(((TextBox)sender).Text); //se actualiza la cantidad
                        //Revisar que la nueva cantidad no supere la existencia real local (usando funciones de controladoras)
                        double existenciaReal = Convert.ToDouble(controladoraProductoLocal.consultarProductoDeBodega((this.Master as SiteMaster).LlaveBodegaSesion, fila.Cells[3].Text).Rows[0][7]);
                        actualizarPreciosTotales(); //actualiza la línea final de la factura
                        if (cantidadesProductos[i] > existenciaReal) //si se pretende vender más de lo que hay disponible
                        {
                            mostrarMensaje("danger", "Alerta: ", "La cantidad del producto '" + fila.Cells[2].Text + "' que intenta venderse es mayor a la existencia real en la bodega " + (this.Master as SiteMaster).NombreBodegaSesion + ". Esta factura no puede guardarse sin arreglar la cantidad.");
                            ((TextBox)sender).ForeColor = System.Drawing.Color.Red; //para alertar al usuario
                            botonCrearFacturaGuardar.Disabled = true;
                        }
                        else
                        {
                            botonCrearFacturaGuardar.Disabled = false;
                            ((TextBox)sender).ForeColor = System.Drawing.Color.Black; //para volver a la normalidad
                        }
                    }
                    catch (Exception x)
                    {
                        return; //se maneja como excepción que la textbox contenga algo no numérico, como ya se maneja en tiempo real, aquí no se hace nada
                    }
                    break; //sólo se hace una vez
                }
                ++i;
            }
        }

        // Presionado boton de fecha de inicio/final de calendario
        protected void clickBotonConsultaCalendarioInicio_ServerClick(object sender, EventArgs e)
        {
            this.CalendarFechaInicio.Visible = true;
        }
        protected void clickBotonConsultaCalendarioFinal_ServerClick(object sender, EventArgs e)
        {
            this.CalendarFechaFinal.Visible = true;
        }

        // Seleccionada una nueva fecha en el calendario inicial/final
        protected void CalendarFechaInicio_SelectionChanged(object sender, EventArgs e)
        {
            this.textboxConsultaFechaInicio.Value = this.CalendarFechaInicio.SelectedDate.ToString("dd/MM/yyyy").Substring(0,10);
            this.CalendarFechaInicio.Visible = false;
        }
        protected void CalendarFechaFinal_SelectionChanged(object sender, EventArgs e)
        {
            this.textboxConsultaFechaFinal.Value = this.CalendarFechaFinal.SelectedDate.ToString("dd/MM/yyyy").Substring(0,10);
            this.CalendarFechaFinal.Visible = false;
        }

        /*
         * Intento fallido de poner el textbox de cantidad de los productos en algún punto medio de cada fila.
         * No hacer.
         */
        //protected void gridViewCrearFacturaProductos_FilaCreada()
        //{
        //    //GridViewRow row = gridViewCrearFacturaProductos.Rows[gridViewCrearFacturaProductos.Rows.Count - 1];
        //    foreach (GridViewRow row in gridViewCrearFacturaProductos.Rows)
        //    {
        //        List<TableCell> columns = new List<TableCell>();
        //        TableCell cantidad = row.Cells[1];
        //        int i = 0;
        //        foreach (DataControlFieldCell column in row.Cells)
        //        {
        //            TableCell cell = row.Cells[i];
        //            if (i == 4) columns.Add(cantidad);
        //            if (i != 1) columns.Add(cell);
        //            ++i;
        //        }
        //        for (int k = 0; k < columns.Count; ++k)
        //            row.Cells.Remove(columns[k]);
        //        row.Cells.AddRange(columns.ToArray());
        //    }
        //}
    }
}