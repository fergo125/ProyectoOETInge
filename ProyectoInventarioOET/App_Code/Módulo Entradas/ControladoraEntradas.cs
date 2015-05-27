using ProyectoInventarioOET.App_Code.Modulo_ProductosGlobales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    public class ControladoraEntradas
    {

        //Atributos
        private ControladoraBDEntradas controladoraBDEntradas;    // Instancia de la controladora de base
        // de datos para realizar operaciones allí.
        private ControladoraProductosGlobales controladoraProductosGlobales;

        /*
         * Constructor.
         */
        public ControladoraEntradas()
        {
            controladoraBDEntradas = new ControladoraBDEntradas();
            controladoraProductosGlobales = new ControladoraProductosGlobales();
        }

        public DataTable consultarEntradas(String bodega) 
        {
            return controladoraBDEntradas.consultarEntradasDeBodega(bodega);
        }
        public DataTable buscarFacturas(String id)
        {
            return controladoraBDEntradas.buscarFacturas(id);
        }

        public DataTable buscarProducto(String buscado) 
        {
            return controladoraProductosGlobales.consultarProductosGlobales(buscado);
        }
        public EntidadFactura consultarFactura(String id) {
            EntidadFactura factura = null;
            
            DataTable resultado = controladoraBDEntradas.consultarFactura(id);
            Object[] datosConsultados = new Object[13];

            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = resultado.Rows[0][0].ToString();
                for (int i = 1; i < 13; i++)
                {
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                }

                factura = new EntidadFactura(datosConsultados);
            }
            return factura;

        }
    }
}