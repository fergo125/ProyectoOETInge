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
        private static Object[] idArray;                                        //Usada para llevar el control de las facturas consultadas
        private static List<bool> checksProductos;                              //Usada para guardar cuáles checks han sido marcados en el grid de productos durante el proceso de creación
        private static List<int> cantidadesProductos;                           //Usada para guardar las cantidades en los textboxes del grid de productos durante el proceso de creación //TODO: usar esto
        private static DataTable productosAgregados;                            //Usada para llenar el grid de productos al crear una factura
        private static DataTable facturasConsultadas;                           //Usada para llenar el grid y para mostrar los detalles de cada factura específica
        private static EntidadFacturaVenta facturaConsultada;                   //???
        private static ControladoraVentas controladoraVentas;                   //Para accesar las tablas del módulo y realizar las operaciones de consulta, inserción, modificación y anulación
        private static ControladoraAjustes controladoraAjustes;                 //???
        private static ControladoraBodegas controladoraBodegas;                 //???
        private static ControladoraSeguridad controladoraSeguridad;             //???
        private static ControladoraActividades controladoraActividades;         //Para consultar las actividades a las que se puede asociar una nueva factura
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //Para accesar datos generales de la base de datos
        private static ControladoraProductoLocal controladoraProductoLocal ;    //Para revisar existencias de productos
        
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
            mensajeAlerta.Visible = false;
            //Elementos visuales
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
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;

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
                //if ((checksProductos.Count > 0) || (cantidadesProductos.Count > 0))
                //    restaurarCheckBoxesYCantidades();
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

            DataTable tablaFacturas = crearTablaFacturasConsultadas(); //tabla que se usará para el grid
            try
            {
                Object[] datos = new Object[6];
                facturasConsultadas = null; //tabla atributo que se usará para almacenar toda la consulta
                facturasConsultadas = controladoraVentas.consultarFacturas(codigoVendedor, codigoBodega, codigoEstacion);
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
         * ???
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
         * ???
         */
        protected void setDatosConsultados()
        {
            textBoxFacturaConsultadaConsecutivo.Value = facturaConsultada.Consecutivo;
            textBoxFacturaConsultadaEstacion.Value = controladoraSeguridad.consultarNombreDeEstacion(facturaConsultada.Estacion);
            textBoxFacturaConsultadaBodega.Value = controladoraSeguridad.consultarNombreDeBodega(facturaConsultada.Bodega);
            textBoxFacturaConsultadaFechaHora.Value = facturaConsultada.FechaHora;
            textBoxFacturaConsultadaVendedor.Value = controladoraSeguridad.consultarNombreDeUsuario(facturaConsultada.Vendedor);
            textBoxFacturaConsultadaCliente.Value = facturaConsultada.Cliente;
            textBoxFacturaConsultadaTipoMoneda.Value = facturaConsultada.TipoMoneda;
            textBoxFacturaConsultadaMontoTotal.Value = facturaConsultada.MontoTotalColones.ToString();
            textBoxFacturaConsultadaMetodoPago.Value = facturaConsultada.MetodoPago;
            textBoxFacturaConsultadActividad.Value = facturaConsultada.Actividad;

            textBoxFacturaConsultadaEstado.Items.Clear();
            textBoxFacturaConsultadaEstado.Items.Add(new ListItem("Activa", "1"));//en la BD el valor de "activo" es 1
            textBoxFacturaConsultadaEstado.Items.Add(new ListItem("Anulada", "5")); //en la BD el valor de "anulado" es 5

            if (facturaConsultada.Estado.Equals("1"))
                textBoxFacturaConsultadaEstado.SelectedIndex = 0;
            else
                textBoxFacturaConsultadaEstado.SelectedIndex = 1;
        }

        /*
         * ???
         */
        protected void habilitarCampos(bool habilitar)
        {
            textBoxFacturaConsultadaEstado.Enabled = habilitar;
            if (textBoxFacturaConsultadaEstado.SelectedValue.Equals("5"))  //en la BD el valor de "anulado" es 5
                textBoxFacturaConsultadaEstado.Enabled = false;
        }

        /*
         * ???
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
        protected DataTable crearTablaProductosFactura(bool paraInsertar)
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
                column = new DataColumn();
                column.DataType = Type.GetType("System.Double");
                column.ColumnName = "Precio unitario dólares";
                productosAgregados.Columns.Add(column);

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

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Total";
            productosAgregados.Columns.Add(column);

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
        protected void cargarMetodosPago()
        {
            DataTable metodosPago = controladoraVentas.consultarMetodosPago();
            if(metodosPago != null)
            {
                dropDownListCrearFacturaMetodoPago.Items.Clear();
                foreach (DataRow fila in metodosPago.Rows)
                    dropDownListCrearFacturaMetodoPago.Items.Add(new ListItem(fila[0].ToString(), fila[1].ToString()));
            }
            else
                mostrarMensaje("warning", "Alerta: ", "Error al intentar cargar los métodos de pago.");
        }

        /*
         * Consulta en la BD las diferentes formas de pago para cargarlas en el dropdownlist.
         */
        protected void cargarPosiblesClientes()
        {
            DataTable clientes = controladoraVentas.consultarPosiblesClientes();
            if (clientes != null)
            {
                dropDownListCrearFacturaCliente.Items.Clear();
                dropDownListCrearFacturaCliente.Items.Add(new ListItem("", ""));
                foreach (DataRow fila in clientes.Rows)
                    dropDownListCrearFacturaCliente.Items.Add(new ListItem(fila[0].ToString(), fila[1].ToString()));
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
            actualizarPreciosTotales();
            //buscarlo en el grid para poner automáticamente cantidad 1
            //foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
            //    if (fila.Cells[3].Text == nombreCodigoProductoEscogido[1]) //si el código interno se repite
            //        ((TextBox)fila.FindControl("gridCrearFacturaCantidadProducto")).Text = "1";
            cantidadesProductos.Add(1); //agregar la existencia inicial (1)
            //Con el databind se borran las cantidades, entonces se restauran, y se pone la nueva default
            if ((checksProductos.Count > 0) || (cantidadesProductos.Count > 0))
                restaurarCheckBoxesYCantidades();
        }

        /*
         * Invocada cuando se agregan productos o se cambia el tipo de moneda durante la creación de una factura.
         */
        protected void actualizarPreciosTotales()
        {
            double precioTotal = 0;
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows) //actualiza el "total" de cada fila del grid y va sumando para ponerlo al final
                precioTotal += Convert.ToDouble((fila.Cells[7].Text = fila.Cells[4].Text.ToString())); //total = unitario (falta multiplicar por cantidad)
            labelCrearFacturaPrecioTotal.Text = ((labelCrearFacturaTipoMoneda.Text == "Colones" ? "₡ " : "$ ") + precioTotal.ToString());
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
         * ???
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
            DataTable productos = crearTablaProductosFactura(true); //con la columna de cantidad
            DataRow detallesProducto;
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
            {
                detallesProducto = productos.NewRow();
                detallesProducto[0] = HttpUtility.HtmlDecode(fila.Cells[2].Text); //nombre
                detallesProducto[1] = HttpUtility.HtmlDecode(fila.Cells[3].Text); //codigo interno
                detallesProducto[6] = HttpUtility.HtmlDecode(fila.Cells[6].Text); //impuesto
                detallesProducto[7] = HttpUtility.HtmlDecode(fila.Cells[7].Text); //total
                detallesProducto[5] = (HttpUtility.HtmlDecode(fila.Cells[5].Text) == "No" ? 0 : 1); //descuento
                detallesProducto[4] = Convert.ToInt32(((TextBox)fila.FindControl("gridCrearFacturaCantidadProducto")).Text); //cantidad
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
                    ((CheckBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturCheckBoxSeleccionarProducto")).Checked = checksProductos[i];
                if (i < cantidadesProductos.Count) //para evitar que intente accesar posiciones inexistentes
                    if (((TextBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturaCantidadProducto")).Text == "") //para evitar que borre lo nuevo
                        ((TextBox)gridViewCrearFacturaProductos.Rows[i].FindControl("gridCrearFacturaCantidadProducto")).Text = cantidadesProductos[i].ToString();
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
            cargarMetodosPago();
            //cargarPosiblesClientes();
            cargarActividades();

            modo = Modo.Insercion;
            cambiarModo();
            productosAgregados = crearTablaProductosFactura(false); //se crea una nueva tabla cada vez, sin la columna de cantidad
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
         * Invocada cuando termina de hacer la factura y se envia a la base de datos.
         */
        protected void clickBotonCrearGuardar(object sender, EventArgs e)
        {
            Object[] datosFactura = obtenerDatos();
            String[] resultado = controladoraVentas.insertarFactura(datosFactura);
            mostrarMensaje(resultado[0], resultado[1], resultado[2]);
        }

        /*
         * Invocada al dar click al botón de Cancelar durante el proceso de creación. Limpia los campos y vuelve al modo de creación.
         */
        protected void clickBotonCrearCancelarModal(object sender, EventArgs e)
        {
            clickBotonCrear(sender, e); //se hace prácticamente lo mismo en ambas funciones
        }

        /*
         * ???
         */
        protected void clickBotonModificar(object sender, EventArgs e)
        {
            modo = Modo.Modificacion;
            cambiarModo();
        }

        /*
         * ???
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
         * ???
         */
        protected void clickBotonCancelarModificar(object sender, EventArgs e)
        {

        }

        /*
         * ???
         */
        protected void clickBotonAceptarCambioUsuario(object sender, EventArgs e)
        {
            // Consulta al usuario
            EntidadUsuario usuario = controladoraSeguridad.consultarUsuario(inputUsername.Value, inputPassword.Value);

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
            }
            //String[] resultado = controladoraVentas.insertarFactura(obtenerDatos());
        }

        /*
         * ???
         */
        protected void clickBotonAceptarAjusteRapido(object sender, EventArgs e)
        {
            String productoEscogido = textBoxAutocompleteAjusteRapidoBusquedaProducto.Text;
            String[] nombreCodigoProductoEscogido = separarNombreCodigoProductoEscogido(productoEscogido);
            String llaveProductoEscogido = controladoraVentas.verificarExistenciaProductoLocal((this.Master as SiteMaster).LlaveBodegaSesion, nombreCodigoProductoEscogido[0], nombreCodigoProductoEscogido[1]);

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
                    //Response.Redirect("FormVentas.aspx");
                    break;
            }
        }

        /*
         * ???
         */
        protected void gridViewFacturas_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid(); //súper ineficiente, TODO: buscar cómo evitar esto
            gridViewFacturas.PageIndex = e.NewPageIndex;
            gridViewFacturas.DataBind();
            tituloGrid.Visible = true;
            gridViewFacturas.Visible = true;
        }

        /*
         * ???
         */
        protected void dropDownListConsultaBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarAsociadosABodegas(dropDownListConsultaBodega.SelectedValue);
        }

        /*
         * ???
         */
        protected void dropDownListConsultaEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarBodegas(this.dropDownListConsultaBodega);
            dropDownListConsultaVendedor.Items.Clear(); //para evitar que escoja vendedores cargados de bodegas anteriores
        }

        /*
         * Cuando se escoja el método de pago "deducción de planilla", se debe cargar los empleados de la OET como posibles clientes.
         */
        protected void dropDownListCrearFacturaMetodoPago_ValorCambiado(object sender, EventArgs e)
        {
            //Deberia hacerse solo cuando escoja "deduccion de planilla"
            cargarPosiblesClientes();
        }

        /*
         * Maneja el evento en el que se marca o desmarca un checkbox de seleccionar un producto en el grid durante la creación de una factura.
         * Si se marca deben desmarcarse los demás checkboxes. También guarda el estado de todos para poder usar los botones de eliminar y editar.
         */
        protected void checkBoxCrearFacturaProductos_CheckCambiado(object sender, EventArgs e)
        {
            botonCrearFacturaQuitarProducto.Disabled = !((CheckBox)sender).Checked;
            botonCrearFacturaEditarProducto.Disabled = !((CheckBox)sender).Checked;
            checksProductos.Clear();
            foreach (GridViewRow fila in gridViewCrearFacturaProductos.Rows)
            {
                //Primero, desmarcar todos los otros checkboxes al marcar uno nuevo (también se hace al desmarcar pero parece ser bastante rápido)
                if (((CheckBox)fila.FindControl("gridCrearFacturCheckBoxSeleccionarProducto")).Checked && ((CheckBox)fila.FindControl("gridCrearFacturCheckBoxSeleccionarProducto") != (CheckBox)sender))
                    ((CheckBox)fila.FindControl("gridCrearFacturCheckBoxSeleccionarProducto")).Checked = false;
                //Segundo, guardar el estado de cada checkbox
                checksProductos.Add(((CheckBox)fila.FindControl("gridCrearFacturCheckBoxSeleccionarProducto")).Checked);
            }
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
                if(((TextBox)fila.FindControl("gridCrearFacturaCantidadProducto")) == (TextBox)sender) //es la fila donde está el textbox que fue cambiado
                {
                    cantidadesProductos[i] = Convert.ToInt32(((TextBox)sender).Text); //se actualiza la cantidad
                    //Revisar que la nueva cantidad no supere la existencia real local (usando funciones de controladoras)
                    double existenciaReal = Convert.ToDouble(controladoraProductoLocal.consultarProductoDeBodega((this.Master as SiteMaster).LlaveBodegaSesion, fila.Cells[3].Text).Rows[0][7]);
                    if (cantidadesProductos[i] > existenciaReal) //si se pretende vender más de lo que hay disponible
                    {
                        mostrarMensaje("danger", "Alerta: ", "La cantidad del producto '" + fila.Cells[2].Text + "' que intenta venderse es mayor a la existencia real en la bodega " + (this.Master as SiteMaster).NombreBodegaSesion + ". Esta factura no puede guardarse sin arreglar la cantidad.");
                        botonCrearFacturaGuardar.Disabled = true;
                    }
                    else
                        botonCrearFacturaGuardar.Disabled = false;
                }
                ++i;
            }

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
    }
}