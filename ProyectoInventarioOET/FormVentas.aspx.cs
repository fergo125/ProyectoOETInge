using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Modo modo = Modo.Inicial;                            //Indica en qué modo se encuentra la interfaz en un momento cualquiera, de éste depende cuáles elementos son visibles
        private static String permisos = "111111";                              //Permisos utilizados para el control de seguridad //TODO: poner en 000000, está en 111111 sólo para pruebas
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //Para accesar datos generales de la base de datos
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
                mostrarBotonesSegunPermisos();
            }
            //Si la página ya estaba cargada pero está siendo cargada de nuevo (porque se está realizando alguna acción que la refrezca/actualiza)
            else
            {
            }
            //cambiarModo(); //por ahora siempre se invoca la función de cambiar modo
        }

        /*
         * Dependiendo del perfil, algunas acciones están permitidas y otras no, ésto se controla escondiendo y mostrando los botones que se usan para realizar esas acciones.
         */
        protected void mostrarBotonesSegunPermisos()
        {
            botonConsultar.Visible = (permisos[5] == '1');
            botonCrear.Visible = (permisos[4] == '1');
            botonModificar.Visible = (permisos[3] == '1');
            botonCambioSesion.Visible = false;              //Estos dos botones sólo deben ser visibles
            botonAjusteEntrada.Visible = false;             //durante la creación de facturas
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
                    break;
                case Modo.Consulta:
                    tituloAccionFacturas.InnerText = "Seleccione filtros para consultar";
                    PanelConsultarFacturas.Visible = true;
                    break;
                case Modo.Insercion:
                    tituloAccionFacturas.InnerText = "Ingrese los datos de la nueva factura";
                    break;
                case Modo.Modificacion:
                    tituloAccionFacturas.InnerText = "Ingrese los nuevos datos para la factura";
                    break;
                case Modo.Consultado:
                    tituloAccionFacturas.InnerText = "Detalles de la factura";
                    break;
                default:
                    //Algo salió mal
                    break;
            }
        }

        /*
         * 
         */
        protected void limpiarCampos()
        {
            //Campos de consulta
            DropDownListConsultaEstacion.SelectedValue = null;
            DropDownListConsultaBodega.SelectedValue = null;
            DropDownListConsultaVendedor.SelectedValue = null;
            //Campos de consulta individual
            //Campos de creación
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
            cambiarModo();
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonEjecutarConsulta(object sender, EventArgs e)
        {
            tituloAccionFacturas.InnerText = "Seleccione una factura para ver su información detallada";
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