using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoInventarioOET
{
    public partial class FormTraslados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*
         * Esto pasa la interfaz al modo de crear traslados.
         */
        protected void botonRealizarTraslado_ServerClick(object sender, EventArgs e)
        {
        }

        /*
         * Esto pasa la interfaz al modo de modificar traslados.
         */
        protected void botonModificarTraslado_ServerClick(object sender, EventArgs e)
        {
        }

        /*
         * Esto pasa la interfaz al modo de consulta.
         */
        protected void botonConsultarTraslado_ServerClick(object sender, EventArgs e)
        {
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de productos.
         */
        protected void gridViewProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            /*
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewAjustes.Rows[Convert.ToInt32(e.CommandArgument)];
                    //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewBodegas.PageIndex * this.gridViewBodegas.PageSize)]);
                    consultarBodega(codigo);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormAjustes.aspx");
                    break;
            }*/
        }

        /*
         * Este método confirma las transacciones de traslados.
         */
        protected void botonAceptarTraslado_ServerClick(object sender, EventArgs e)
        {
            
        }

        /*
         * Método que maneja la aceptar la cancelación.
         * Elimina datos y reinicia la interfaz.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            //vaciarGridAjustes();
            //modo = (int)Modo.Inicial;
            //cambiarModo();
            //limpiarCampos();
            //ajusteConsultado = null;
        }

    }
}