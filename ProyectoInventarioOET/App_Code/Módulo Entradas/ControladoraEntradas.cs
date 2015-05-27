using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.Modulo_ProductosGlobales;

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

        public DataTable consultarFacturas()
        {
            return controladoraBDEntradas.consultarFacturas();
        }

        public DataTable buscarFacturas(String id)
        {
            return controladoraBDEntradas.buscarFacturas(id);
        }

        public DataTable buscarProducto(String buscado) 
        {
            return controladoraProductosGlobales.consultarProductosGlobales(buscado);
        }
        //public EntidadFactura b
    }
}