using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Seguridad;
//using ProyectoInventarioOET.App_Code.Módulo_Ventas; //TODO: arreglar esta vara -.-
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
        private Modo modo = Modo.Inicial;                               //Indica en qué modo se encuentra la interfaz en un momento cualquiera, de éste depende cuáles elementos son visibles
        private String permisos = "111111";                             //Permisos utilizados para el control de seguridad //TODO: poner en 000000, está en 111111 sólo para pruebas
        private String codigoPerfilUsuario = "";                        //Indica el perfil del usuario, usado para acciones de seguridad para las cuales la string de permisos no basta
        private DataTable facturasConsultadas;                          //Usada para llenar el grid y para mostrar los detalles de cada factura específica
        //private ControladoraVentas controladoraVentas;                  //Para accesar las tablas del módulo y realizar las operaciones de consulta, inserción, modificación y anulación
        private ControladoraDatosGenerales controladoraDatosGenerales;  //Para accesar datos generales de la base de datos
        private static ControladoraSeguridad controladoraSeguridad;
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
            //Si es la primera vez que se carga la página
            if (!IsPostBack)
            {
                //Elementos visuales
                ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
                //Controladoras
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraSeguridad = new ControladoraSeguridad();
                //Seguridad
                //permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Facturacion"); //TODO: descomentar esto, está comentado sólo para pruebas
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                //perfilUsuario = (this.Master as SiteMaster).Usuario.Perfil;
                mostrarElementosSegunPermisos();
            }
            //Si la página ya estaba cargada pero está siendo cargada de nuevo (porque se está realizando alguna acción que la refrezca/actualiza)
            else
            {
            }
            //cambiarModo();
            //código para probar algo
            TableRow row = new TableRow();
            TableCell cell1 = new TableCell();
            cell1.Text = "blah blah blah";
            row.Cells.Add(cell1);
            test.Rows.Add(row);
            //HtmlTableRow row = new HtmlTableRow();
            //row.Cells.Add(new HtmlTableCell());
            //estructuraFactura.Rows.Add(row);
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
            dropDownListConsultaEstacion.Enabled = (codigoPerfilUsuario == "1");    //Sólo si es administrador global puede escoger una estación
            dropDownListConsultaBodega.Enabled = (codigoPerfilUsuario == "2");      //Sólo si es administrador local puede escoger una bodega
            dropDownListConsultaVendedor.Enabled = (codigoPerfilUsuario == "3");    //Sólo si es supervisor puede escoger un vendedor
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
            botonCambioSesion.Visible = false;      //Estos dos botones sólo deben ser visibles
            botonAjusteEntrada.Visible = false;     //durante la creación de facturas

            //Código específico para cada modo
            switch (modo)
            {
                case Modo.Inicial:
                    tituloAccionFacturas.InnerText = "Seleccione una opción";
                    break;
                case Modo.Consulta:
                    tituloAccionFacturas.InnerText = "Seleccione filtros para consultar";
                    PanelConsultarFacturas.Visible = true;
                    break;
                case Modo.Insercion:
                    tituloAccionFacturas.InnerText = "Ingrese los datos de la nueva factura";
                    botonCambioSesion.Visible = true;  //Estos dos botones sólo deben ser visibles
                    botonAjusteEntrada.Visible = true; //durante la creación de facturas
                    break;
                case Modo.Modificacion:
                    tituloAccionFacturas.InnerText = "Ingrese los nuevos datos para la factura";
                    break;
                case Modo.Consultado:
                    tituloAccionFacturas.InnerText = "Detalles de la factura";
                    PanelConsultarFacturas.Visible = false;
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

            //Dependiendo del perfil del usuario, puede que en la instancia de usuarioLogueado ya estén guardados los datos por default
            switch (Convert.ToInt32(codigoPerfilUsuario))
            {
                case 4: //Vendedor
                    dropDownListConsultaVendedor.Enabled = false;
                    //dropDownListConsultaVendedor.Items.Add(new ListItem());
                    //dropDownListConsultaVendedor.SelectedItem = 
                    goto case 3; //por alguna razón C# no permite fall through
                case 3: //Supervisor
                    dropDownListConsultaBodega.Enabled = false;
                    //dropDownListConsultaBodega.Items.Add(new ListItem());
                    //dropDownListConsultaBodega.SelectedItem =
                    goto case 2;  //por alguna razón C# no permite fall through
                case 2: //Administrador local
                    dropDownListConsultaEstacion.Enabled = false;
                    //dropDownListConsultaEstacion.Items.Add(new ListItem());
                    //dropDownListConsultaEstacion.SelectedItem = 
                    break;
                default:
                    //Administrador global y cualquier otro, este switch es extendible a más perfiles
                    break;
            }
            //TODO: básicamente se obtienen los datos del perfil según cual sea para colocarlos en los dropdownlists de una vez y ahorrarse
            //viajes a la base de datos trayendo opciones.
            //TODO: también, falta agregar que el usuarioLogueado, su clase entidad, guarde la llave de la bodega a la que está asignado,
            //esto probablemente requiera agregar el campo a la base de datos.

            //Si una dropdownlist no queda con un valor seleccionado (porque el perfil es elevado), entonces sí se cargan opciones
            if(dropDownListConsultaEstacion.SelectedItem == null)
            {
                dropDownListConsultaEstacion.Items.Add(new ListItem("Todas")); //Agregar la opción de "Todas"/"Todos" al principio de la lista
                DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
                foreach (DataRow fila in estaciones.Rows) //Agregar las opciones para cada caso
                    dropDownListConsultaEstacion.Items.Add(new ListItem(fila[2].ToString(), fila[0].ToString())); //Nombre, llave
            }
            if (dropDownListConsultaBodega.SelectedItem == null)
            {
                dropDownListConsultaBodega.Items.Add(new ListItem("Todas")); //Agregar la opción de "Todas"/"Todos" al principio de la lista
                //DataTable bodegas = 
                //foreach (DataRow fila in bodegas.Rows) //Agregar las opciones para cada caso
                //    dropDownListConsultaEstacion.Items.Add(new ListItem(); //Nombre, llave
            }
            if (dropDownListConsultaVendedor.SelectedItem == null)
            {
                dropDownListConsultaVendedor.Items.Add(new ListItem("Todos")); //Agregar la opción de "Todas"/"Todos" al principio de la lista
                //DataTable vendedores = 
                //foreach (DataRow fila in vendedores.Rows) //Agregar las opciones para cada caso
                //    dropDownListConsultaEstacion.Items.Add(new ListItem(); //Nombre, llave
            }
            //TODO: agregar bien estas consultas para que cargue las listas de opciones
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
            String codigoEstacion = (dropDownListConsultaEstacion.SelectedValue != "Todas" ? dropDownListConsultaEstacion.SelectedValue : null);
            String codigoBodega = (dropDownListConsultaBodega.SelectedValue != "Todas" ? dropDownListConsultaBodega.SelectedValue : null);
            String codigoVendedor = (dropDownListConsultaVendedor.SelectedValue != "Todos" ? dropDownListConsultaVendedor.SelectedValue : null);
            //TODO: revisar que de los dropdownlists se obtengan las llaves, no los nombres, algo como dropDownList.SelectedItem[1] creo
            
            //Consultar a la controladora (implementar funciones en las capas inferiores)
            //facturasConsultadas = controladoraVentas.consultarFacturas(codigoEstacion, codigoBodega, codigoVendedor);
        }

        protected void cargarDatosFactura(int indiceFilaSeleccionada)
        {
            //facturasConsultadas.Rows[indiceFilaSeleccionada]; //usar el grid así
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
            cambiarModo();
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonEjecutarConsulta(object sender, EventArgs e)
        {
            llenarGrid();
            PanelGridConsultas.Visible = true;
            tituloAccionFacturas.InnerText = "Seleccione una factura para ver su información detallada";
        }

        /*
         * Invocada cuando se escoge una factura del grid de consultas para desplegar su información específica
         * (en el panel de consulta específica, el cual ahora reemplazará visualmente al panel de escoger datos para consultar).
         */
        protected void gridViewFacturas_FilaSeleccionada(object sender, EventArgs e)
        {
            modo = Modo.Consultado;
            cargarDatosFactura(gridViewFacturas.SelectedIndex);
            cambiarModo();
        }

        protected void botonAceptarCambioUsuario_ServerClick(object sender, EventArgs e)
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
        }

    }
}