using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
        private Modo modo = Modo.Inicial;                                //Indica en qué modo se encuentra la interfaz en un momento cualquiera, de éste depende cuáles elementos son visibles
        private String permisos = "111111";                              //Permisos utilizados para el control de seguridad //TODO: poner en 000000, está en 111111 sólo para pruebas
        private String codigoPerfilUsuario = "";                         //Indica el perfil del usuario, usado para acciones de seguridad para las cuales la string de permisos no basta
        private ControladoraDatosGenerales controladoraDatosGenerales;   //Para accesar datos generales de la base de datos

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
            //cambiarModo(); //por ahora siempre se invoca la función de cambiar modo
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
         * ???
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case Modo.Inicial:
                    tituloAccionFacturas.InnerText = "Seleccione una opción";
                    botonCambioSesion.Visible = false;
                    botonAjusteEntrada.Visible = false;
                    break;
                case Modo.Consulta:
                    tituloAccionFacturas.InnerText = "Seleccione filtros para consultar";
                    PanelConsultarFacturas.Visible = true;
                    botonCambioSesion.Visible = false;
                    botonAjusteEntrada.Visible = false;
                    break;
                case Modo.Insercion:
                    tituloAccionFacturas.InnerText = "Ingrese los datos de la nueva factura";
                    botonCambioSesion.Visible = true;      //Estos dos botones sólo deben ser visibles
                    botonAjusteEntrada.Visible = true;     //durante la creación de facturas
                    break;
                case Modo.Modificacion:
                    tituloAccionFacturas.InnerText = "Ingrese los nuevos datos para la factura";
                    botonCambioSesion.Visible = false;
                    botonAjusteEntrada.Visible = false;
                    break;
                case Modo.Consultado:
                    tituloAccionFacturas.InnerText = "Detalles de la factura";
                    botonCambioSesion.Visible = false;
                    botonAjusteEntrada.Visible = false;
                    break;
                default:
                    //Algo salió mal
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
         * 
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
         * 
         */
        protected void cargarDropdownListsConsulta()
        {
            //Limpiar
            dropDownListConsultaEstacion.Items.Clear();
            dropDownListConsultaBodega.Items.Clear();
            dropDownListConsultaVendedor.Items.Clear();
            //Agregar la opción de "Todas"/"Todos"
            dropDownListConsultaEstacion.Items.Add(new ListItem("Todas"));
            dropDownListConsultaBodega.Items.Add(new ListItem("Todas"));
            dropDownListConsultaVendedor.Items.Add(new ListItem("Todos"));
            //Agregar las opciones
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            DataTable bodegas = controladoraDatosGenerales.consultarEstaciones();
            DataTable vendedores = controladoraDatosGenerales.consultarEstaciones();
            foreach (DataRow fila in estaciones.Rows)
                dropDownListConsultaEstacion.Items.Add(new ListItem(fila[2].ToString(), fila[0].ToString()));
        }

        /*
         * Invocada al consultar facturas, dependiendo de los parámetros de consulta se muestran facturas asociadas a:
         * -Una estación o todas
         * -Una bodega de esa estación, o todas las bodegas de esa estación
         * -Un vendedor de esa bodega, o todos los vendedores de esa bodega
         */
        protected void llenarGrid()
        {
            //Importante: estos dropdownlists pueden contener una entidad específica o la palabra "Todas"/"Todos", en el segundo caso se envía "null" para que el sistema sepa lo que significa
            String estacion = (dropDownListConsultaEstacion.SelectedValue != "Todas" ? dropDownListConsultaEstacion.SelectedValue : null);
            String bodega = (dropDownListConsultaBodega.SelectedValue != "Todas" ? dropDownListConsultaBodega.SelectedValue : null);
            String vendedor = (dropDownListConsultaVendedor.SelectedValue != "Todos" ? dropDownListConsultaVendedor.SelectedValue : null);
            
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
            tituloAccionFacturas.InnerText = "Seleccione una factura para ver su información detallada";
            llenarGrid();
            PanelGridConsultas.Visible = true;
        }

        /*
         * Invocada cuando se escoge una factura del grid de consultas para desplegar su información específica
         * (en el panel de consulta específica, el cual ahora reemplazará visualmente al panel de escoger datos para consultar).
         */
        protected void gridViewFacturas_FilaSeleccionada(object sender, EventArgs e)
        {

        }
    }
}