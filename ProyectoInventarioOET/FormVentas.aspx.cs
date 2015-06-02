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
        private static Modo modo = Modo.Inicial;                                //Indica en qué modo se encuentra la interfaz en un momento cualquiera, de éste depende cuáles elementos son visibles
        private static String permisos = "000000";                              //Permisos utilizados para el control de seguridad
        private static String codigoPerfilUsuario = "";                         //Indica el perfil del usuario, usado para acciones de seguridad para las cuales la string de permisos no basta
        private static String tipoMonedaCrearFactura = "Colones";               //Indica el tipo de moneda que se está usando para crear facturas
        private static DataTable productosAgregados;                            //Usada para llenar el grid de productos al crear una factura
        private static DataTable facturasConsultadas;                           //Usada para llenar el grid y para mostrar los detalles de cada factura específica
        private static EntidadFactura facturaConsultada;                        //???
        //private static Boolean seConsulto = false;                              //???
        private static Object[] idArray;                                        //Usada para llevar el control de las facturas consultadas
        private static ControladoraVentas controladoraVentas;                   //Para accesar las tablas del módulo y realizar las operaciones de consulta, inserción, modificación y anulación
        private static ControladoraAjustes controladoraAjustes;                 //???
        private static ControladoraBodegas controladoraBodegas;                 //???
        private static ControladoraSeguridad controladoraSeguridad;             //???
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //Para accesar datos generales de la base de datos
        
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
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Facturacion"); //TODO: descomentar esto, está comentado sólo para pruebas
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                codigoPerfilUsuario = (this.Master as SiteMaster).Usuario.CodigoPerfil;
                mostrarElementosSegunPermisos();
                //if (!seConsulto)
                //{
                //    modo = Modo.Inicial;
                //}
                //else
                //{
                //    if (facturaConsultada == null)
                //    {
                //        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar la bodega.");
                //    }
                //    else
                //    {
                //        setDatosConsultados();
                //        seConsulto = false;
                //    }
                //}
            }
            //Si la página ya estaba cargada pero está siendo cargada de nuevo (porque se está realizando alguna acción que la refrezca/actualiza)

            cambiarModo();
            ////código para probar algo
            
            //testRow = testTable.NewRow();
            //testRow["Nombre"] = "Nombre de prueba";
            //testRow["Código interno"] = "CRO001";
            //testRow["Precio unitario"] = "500";
            //testRow["Impuesto"] = "Sí";
            //testRow["Descuento (%)"] = "0";
            //testTable.Rows.Add(testRow);
            //testRow = testTable.NewRow();
            //testRow["Nombre"] = "Nombre de prueba larguísimo de esos que ponen las unidades y la vara";
            //testRow["Código interno"] = "CRO002";
            //testRow["Precio unitario"] = "50000";
            //testRow["Impuesto"] = "Sí";
            //testRow["Descuento (%)"] = "0";
            //testTable.Rows.Add(testRow);

            //gridViewCrearFacturaProductos.DataSource = testTable;
            //gridViewCrearFacturaProductos.DataBind();

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
            //Código común (que debe ejecutarse en la mayoría de modos, en la minoría luego es arreglado en el switch) Reduce un poco la eficiencia, pero simplifica el código bastante
            PanelConsultarFacturas.Visible = false;             //Consulta general
            PanelConsultarFacturaEspecifica.Visible = false;    //Consulta específica
            tituloGrid.Visible = false;                         //Grid de consulta
            gridViewFacturas.Visible = false;                   //Grid de consulta
            PanelCrearFactura.Visible = false;                  //Crear factura
            botonCambioSesion.Visible = false;                  //Cambio sesión rápido
            botonAjusteEntrada.Visible = false;                 //Ajuste inventario rápido
            //PanelGridConsultas.Visible = false; //hay que hacerlo directo con el panel, porque si no la paginacion no sirve

            //Código específico para cada modo
            switch (modo)
            {
                case Modo.Inicial:
                    tituloAccionFacturas.InnerText = "Seleccione una opción";
                    botonModificar.Disabled = true;
                    break;
                case Modo.Consulta:
                    tituloAccionFacturas.InnerText = "Seleccione datos para consultar";
                    PanelConsultarFacturas.Visible = true;
                    botonModificar.Disabled = true;
                    //this.gridViewFacturas.Visible = false;
                    //this.tituloGrid.Visible = false;
                    //llenarGrid();
                    break;
                case Modo.Insercion:
                    tituloAccionFacturas.InnerText = "Ingrese los datos de la nueva factura";
                    PanelCrearFactura.Visible = true;
                    botonCambioSesion.Visible = true;  //Estos dos botones sólo deben ser visibles
                    botonAjusteEntrada.Visible = true; //durante la creación de facturas
                    botonModificar.Disabled = true;
                    break;
                case Modo.Modificacion:
                    tituloAccionFacturas.InnerText = "Ingrese los nuevos datos para la factura"; //TODO: revisar este mensaje, ya que no se pueden modificar, sólo anular
                    PanelConsultarFacturaEspecifica.Visible = true;
                    botonAceptarModificacionFacturaEspecifica.Visible = true;
                    botonCancelarModificacionFacturaEspecifica.Visible = true;
                    botonModificar.Disabled = true;
                    habilitarCampos(true);
                    break;
                case Modo.Consultado:
                    tituloAccionFacturas.InnerText = "Detalles de la factura";
                    PanelConsultarFacturaEspecifica.Visible = true;
                    gridViewFacturas.Visible = true;
                    tituloGrid.Visible = true;
                    botonModificar.Disabled = false;
                    //cargarDropdownListsConsulta();
                    //llenarGrid();
                    //habilitarCampos(false);
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
            //dropDownListConsultaEstacion.Items.Clear();
            //dropDownListConsultaBodega.Items.Clear();
            //dropDownListConsultaVendedor.Items.Clear();

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

            //switch original
            //switch (Convert.ToInt32((this.Master as SiteMaster).Usuario.CodigoPerfil)) //TODO, revisar esto
            //{
            //    case 4: //Vendedor
            //        //Se pone al vendedor automáticamente
            //        dropDownListConsultaVendedor.Items.Add(new ListItem((this.Master as SiteMaster).Usuario.Nombre, (this.Master as SiteMaster).Usuario.Codigo));
            //        dropDownListConsultaVendedor.SelectedIndex = 0;
            //        dropDownListConsultaVendedor.Enabled = false;
            //        //Se pone la bodega automáticamente
            //        dropDownListConsultaBodega.Items.Add(new ListItem( (this.Master as SiteMaster).NombreBodegaSesion, (this.Master as SiteMaster).LlaveBodegaSesion));
            //        dropDownListConsultaBodega.SelectedIndex = 0;
            //        dropDownListConsultaBodega.Enabled = false;
            //        cargarEstacionesConsulta();
            //        dropDownListConsultaEstacion.Enabled = false;
            //        break;
            //    case 3: //Supervisor
            //        cargarAsociadosABodega((this.Master as SiteMaster).LlaveBodegaSesion);
            //        dropDownListConsultaBodega.Items.Add(new ListItem( (this.Master as SiteMaster).NombreBodegaSesion, (this.Master as SiteMaster).LlaveBodegaSesion));
            //        dropDownListConsultaBodega.SelectedIndex = 0;
            //        dropDownListConsultaBodega.Enabled = false;
            //        cargarEstacionesConsulta();
            //        dropDownListConsultaEstacion.Enabled = false;
            //        break;  
            //    case 2: //Administrador local
            //        cargarAsociadosABodega((this.Master as SiteMaster).LlaveBodegaSesion);
            //        cargarEstacionesConsulta();
            //        dropDownListConsultaEstacion.Enabled = false;
            //        cargarBodegas(this.dropDownListConsultaBodega);
            //        dropDownListConsultaBodega.SelectedIndex = 1;
            //        break;
            //    default:
            //        cargarAsociadosABodega((this.Master as SiteMaster).LlaveBodegaSesion);
            //        cargarEstacionesConsulta();
            //        dropDownListConsultaEstacion.SelectedIndex = 0;
            //        cargarBodegas(this.dropDownListConsultaBodega);
            //        dropDownListConsultaBodega.SelectedIndex = 1;
            //        //Administrador global y cualquier otro, este switch es extendible a más perfiles
            //        break;
            //}
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

            DataTable tabla = crearTablaFacturas();
            //int indiceNuevaFactura = -1;
            try
            {
                Object[] datos = new Object[5];
                facturasConsultadas = controladoraVentas.consultarFacturas((this.Master as SiteMaster).Usuario.Perfil, codigoVendedor, codigoBodega, codigoEstacion);
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
                        datos[4] = fila[9].ToString();  //Método de pago
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                else
                {
                    mostrarMensaje("warning", "Alerta", "No hay facturas asociadas a ese vendedor.");
                    //datos[0] = "-";
                    //datos[1] = "-";
                    //datos[2] = "-";
                    //datos[3] = "-";
                    //datos[4] = "-";
                    //tabla.Rows.Add(datos);
                }
                gridViewFacturas.DataSource = tabla;
                gridViewFacturas.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * Invocada al escoger una factura en el grid, se muestran todos los detalles de la misma en campos colocados arriba del grid.
         */
        //protected void cargarDatosFactura(String consecutivoSeleccionado)
        //{
        //    facturaConsultada = controladoraVentas.consultarFactura(consecutivoSeleccionado);
        //    setDatosConsultados();
        //    PanelConsultarFacturaEspecifica.Visible = true;
        //}

        /*
         * ???
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
                if(fila[0].ToString() == (this.Master as SiteMaster).Usuario.Codigo)
                    dropDownListConsultaVendedor.SelectedIndex = i;
                ++i;
            }
        }

        /*
         * ???
         */
        protected void consultarFactura(String idFactura)
        {
            //seConsulto = true;
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
            textBoxFacturaConsultadaFechaHora.Value = facturaConsultada.Fecha;
            textBoxFacturaConsultadaVendedor.Value = controladoraSeguridad.consultarNombreDeUsuario(facturaConsultada.Vendedor);
            textBoxFacturaConsultadaCliente.Value = facturaConsultada.Cliente;
            textBoxFacturaConsultadaTipoMoneda.Value = facturaConsultada.TipoMoneda;
            textBoxFacturaConsultadaMontoTotal.Value = facturaConsultada.MontoTotal.ToString();
            textBoxFacturaConsultadaMetodoPago.Value = facturaConsultada.MetodoPago;
            textBoxFacturaConsultadActividad.Value = facturaConsultada.Actividad;

            textBoxFacturaConsultadaEstado.Items.Clear();
            textBoxFacturaConsultadaEstado.Items.Add(new ListItem("Activa", "Activa"));
            textBoxFacturaConsultadaEstado.Items.Add(new ListItem("Anulada", "Anulada"));

            if (facturaConsultada.Estado.Equals("Activa"))
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
                if (textBoxFacturaConsultadaEstado.SelectedValue.Equals("Anulada"))
                    textBoxFacturaConsultadaEstado.Enabled = false;
            
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
            datos[3] = textBoxCrearFacturaEstacion.Text;
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
        protected DataTable crearTablaFacturas() 
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
            column.ColumnName = "Método de pago";
            tabla.Columns.Add(column);

            return tabla;
        }

        /*
         * ???
         */
        protected DataTable crearTablaProdcutosFactura()
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
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "Precio unitario";
            productosAgregados.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Impuesto";
            productosAgregados.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "Descuento (%)";
            productosAgregados.Columns.Add(column);

            return productosAgregados;
        }

        /*
         * ???
         */
        //protected void cargarEstaciones() 
        //{
        //    EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
        //    DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();

        //    if (estaciones.Rows.Count > 0)
        //    {
        //        this.dropDownListCrearFacturaEstacion.Items.Clear();
        //        foreach (DataRow fila in estaciones.Rows)
        //        {
        //            if ((usuarioActual.CodigoPerfil.Equals("1"))||(usuarioActual.IdEstacion.Equals(fila[0])))
        //            {
        //                this.dropDownListCrearFacturaEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
        //            }
        //        }
        //    }
        //    if ((usuarioActual.CodigoPerfil.Equals("1")))
        //    {
        //        dropDownListCrearFacturaEstacion.Enabled = true;
        //    }
        //    else
        //    {
        //        dropDownListCrearFacturaEstacion.Enabled = false;
        //    }
        //}

        /*
         * ???
         */
        protected void cargarEstacionesConsulta()
        {
            dropDownListConsultaEstacion.Items.Clear();
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            //int i=0;
            if (estaciones.Rows.Count > 0)
            {
                dropDownListConsultaEstacion.Items.Add(new ListItem("Todas", "All"));
                //i++;
                foreach (DataRow fila in estaciones.Rows)
                {
                    if ((usuarioActual.CodigoPerfil.Equals("1")) || (usuarioActual.IdEstacion.Equals(fila[0])))
                    {
                        this.dropDownListConsultaEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                        //if ((usuarioActual.IdEstacion.Equals(fila[0])) && (!usuarioActual.CodigoPerfil.Equals("1")))
                        //{
                        //    this.dropDownListConsultaEstacion.SelectedIndex = i;
                        //}
                        //i++;
                    }
                }
            }
        }

        /*
         * ???
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
            //else //esto no debería darse
            //{
            //    bodegas = controladoraBodegas.consultarBodegasDeEstacion(dropDownListCrearFacturaEstacion.SelectedValue);
            //}
            //int i = 0;
            if (bodegas.Rows.Count > 0)
            {
                foreach (DataRow fila in bodegas.Rows)
                {
                    //if ((usuarioActual.Perfil.Equals("Administrador global")) || (usuarioActual.Perfil.Equals("Administrador local")) || fila[1].ToString().Equals((this.Master as SiteMaster).NombreBodegaSesion))
                    //si se invoca esta función es porque ya se revisó la parte de seguridad
                    dropdown.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                }
            }
            //if ((usuarioActual.Perfil.Equals("Administrador global")) || (usuarioActual.Perfil.Equals("Administrador local")))
            //{
            //    dropdown.Enabled = true;
            //}
            //else
            //{
            //    dropdown.Enabled = false;
            //}
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
            gridViewFacturas.Visible = true;
            tituloGrid.Visible = true;
            tituloAccionFacturas.InnerText = "Seleccione una factura para ver su información detallada";
            //Aquí NO se debe inovcar cambiarModo, ya que el modo no cambia
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonCrearFactura(object sender, EventArgs e)
        {
            textBoxCrearFacturaEstacion.Text = controladoraDatosGenerales.consultarEstacionDeBodega((this.Master as SiteMaster).LlaveBodegaSesion)[0]; //nombre de la estación a la que pertenece la bodega de trabajo
            textBoxCrearFacturaBodega.Text = (this.Master as SiteMaster).NombreBodegaSesion; //nombre de la bodega de trabajo (punto de venta)
            textBoxCrearFacturaVendedor.Text = (this.Master as SiteMaster).Usuario.Nombre;
            textBoxCrearFacturaTipoCambio.Text = controladoraVentas.consultarTipoCambio().ToString();
            modo = Modo.Insercion;
            cambiarModo();
            productosAgregados = crearTablaProdcutosFactura(); //se crea una nueva tabla cada vez
        }

        /*
         * Separa la string de un producto escogido en una barra de autocomplete en sus dos partes, nombre y código ([0] y [1]).
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
         * Invocada cuando se da click al botón de "Agregar Producto" a la factura, se revisa que exista primero
         * (el usuario puede escribir lo que quiera, es un textbox), si existe se agrega al grid para luego editar
         * su cantidad y poder aplicarle descuentos (o quitarlo de la factura).
         */
        protected void clickBotonAgregarProductoFactura(object sender, EventArgs e)
        {
            //Primero, obtener la llave desde el catálogo global usando esos dos valores (es necesario revisar ambos
            //valores para asegurarse de que el usuario no cambio ninguno antes de dar click al botón).
            String productoEscogido = textBoxAutocompleteCrearFacturaBusquedaProducto.Text;
            String[] nombreCodigoProductoEscogido = separarNombreCodigoProductoEscogido(productoEscogido);
            String llaveProductoEscogido = controladoraVentas.verificarExistenciaProductoLocal((this.Master as SiteMaster).LlaveBodegaSesion, nombreCodigoProductoEscogido[0], nombreCodigoProductoEscogido[1]);

            if (llaveProductoEscogido != null)
            {
                DataRow testRow;
                testRow = productosAgregados.NewRow();
                testRow["Nombre"] = nombreCodigoProductoEscogido[0];            //nombre
                testRow["Código interno"] = nombreCodigoProductoEscogido[1];    //código interno
                testRow["Precio unitario"] = "500";                             //precio unitario (buscar en la BD)
                testRow["Impuesto"] = "Sí";                                     //impuesto (booleano)
                testRow["Descuento (%)"] = "0";                                 //descuento (siempre empieza con 0)
                productosAgregados.Rows.Add(testRow);
                gridViewCrearFacturaProductos.DataSource = productosAgregados;
                gridViewCrearFacturaProductos.DataBind();
            }
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
            llenarGrid();
            gridViewFacturas.PageIndex = e.NewPageIndex;
            gridViewFacturas.DataBind();
            tituloGrid.Visible = true;
            gridViewFacturas.Visible = true;
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

        protected void dropDownListCrearFacturaEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarBodegas(this.dropDownListCrearFacturaBodega);
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
         * ???
         */
        protected void botonAceptarAjusteRapido_ServerClick(object sender, EventArgs e)
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
         * ???
         */
        protected void botonModificar_ServerClick(object sender, EventArgs e)
        {
            modo = Modo.Modificacion;
            cambiarModo();
        }

        protected void botonAceptarModificacionFacturaEspecifica_ServerClick(object sender, EventArgs e)
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

        protected void botonCancelarModificacionFacturaEspecifica_ServerClick(object sender, EventArgs e)
        {

        }
    }
}