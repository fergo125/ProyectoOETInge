using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.App_Code.Módulo_Ajustes;


namespace ProyectoInventarioOET
{
    public partial class FromAjustes : System.Web.UI.Page
    {

        // Hace el llamado para cargar las estaciones
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Controladora de datos generales
        private static ControladoraAjustes controladoraAjustes;   // Controladora de datos generales


        // DataTable bodegas = controladoraBodegas.consultarBodegasDeEstacion(idEstacion);

        /*
         * Método llamado cada vez que se carga la página.
         */
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*
         * Esto pasa la interfaz al modo de crear ajustes.
         */
        protected void botonRealizarAjuste_ServerClick(object sender, EventArgs e)
        {
            
        }

        /*
         * Esto pasa la interfaz al modo de consulta.
         */
        protected void botonConsultarAjustes_ServerClick(object sender, EventArgs e)
        {

        }

        /*
         * Este método confirma inserción de ajustes.
         */
        protected void botonAceptarAjustes_ServerClick(object sender, EventArgs e)
        {

        }

        /*
         * Método que maneja la aceptar la cancelación.
         * Elimina datos y reinicia la interfaz.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {

        }
    }
}