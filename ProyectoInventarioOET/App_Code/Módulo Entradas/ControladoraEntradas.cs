using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.Modulo_ProductosGlobales;
using ProyectoInventarioOET.Modulo_Productos_Locales;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    public class ControladoraEntradas
    {

        //Atributos
        private ControladoraBDEntradas controladoraBDEntradas;    // Instancia de la controladora de base
        // de datos para realizar operaciones allí.
        private ControladoraProductosGlobales controladoraProductosGlobales;
        private ControladoraProductoLocal controladoraProductoLocal;

        /*
         * Constructor.
         */
        public ControladoraEntradas()
        {
            controladoraBDEntradas = new ControladoraBDEntradas();
            controladoraProductosGlobales = new ControladoraProductosGlobales();
            controladoraProductoLocal = new ControladoraProductoLocal();
        }

        /*
         *Consulta las entradas de una bodega especifica 
         */
        public DataTable consultarEntradas(String bodega) 
        {
            return controladoraBDEntradas.consultarEntradasDeBodega(bodega);
        }
        /*
         * Busca las facturas para las cuales se va a registrar una entrada
         */
        public DataTable buscarFacturas(String id)
        {
            return controladoraBDEntradas.buscarFacturas(id);
        }
        /*
         * Busca un producto especifico en el catalogo
         */
        public DataTable buscarProducto(String buscado) 
        {
            return controladoraProductosGlobales.consultarProductosGlobales(buscado);
        }
        /*
         * Consulta una factura determinada
         */
        public EntidadFactura consultarFactura(String id) {
            EntidadFactura factura = null;
            
            DataTable resultado = controladoraBDEntradas.consultarFactura(id);
            Object[] datosConsultados = new Object[16];

            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = resultado.Rows[0][0].ToString();
                for (int i = 1; i < 16; i++)
                {
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                }

                factura = new EntidadFactura(datosConsultados);
            }
            return factura;

        }
        /*
         * Consulta una entrada especifica a partir de un id
         */
        public EntidadEntrada consultarEntrada(String id)
        {
            EntidadEntrada entrada = null;

            DataTable resultado = controladoraBDEntradas.consultarEntrada(id);
            Object[] datosConsultados = new Object[5];

            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = resultado.Rows[0][0].ToString();
                for (int i = 1; i < 5; i++)
                {
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                }

                entrada = new EntidadEntrada(datosConsultados);
            }
            return entrada;

        }
        /*
         * Recupera el detalle de una factura a partir de una orden de compra
         */
        public DataTable consultarDetalleFactura(string idOrdenDeCompra)
        {
            return controladoraBDEntradas.consultarDetalleFactura(idOrdenDeCompra);
        }

        public DataTable consultarProductosEntrada(string id)
        {
            return controladoraBDEntradas.consultarProductosEntrada(id);
        }
        public String[] insertarEntrada(Object[] entrada, DataTable productosAsociados)
        {
            return controladoraBDEntradas.insertarEntrada(new EntidadEntrada(entrada), productosAsociados);
        }

        /*
         * Consulta la existencia de un producto específico perteneciente a una bodega.
         */
        public DataTable consultarProductoDeBodega(String idBodega, String idProducto)
        {
            return controladoraProductoLocal.consultarProductoDeBodega(idBodega, idProducto);
        }

        /*
         * Consulta el nombre del proveedor asociado a una factura para mostrarlo en el encabezado de esta.
         */
        public String consultarNombreProveedor(String idProveedor)
        {
            return controladoraBDEntradas.consultarNombreProveedor(idProveedor);
        }
    }
}