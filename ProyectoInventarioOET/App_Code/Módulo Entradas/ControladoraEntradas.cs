using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.Modulo_ProductosGlobales;
using ProyectoInventarioOET.Modulo_ProductosLocales;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    public class ControladoraEntradas : Controladora
    {

        //Atributos
        private ControladoraBDEntradas controladoraBDEntradas;                  // Instancia de la controladora de base de datos para realizar operaciones allí.
        private ControladoraProductoLocal controladoraProductoLocal;
        private ControladoraProductosGlobales controladoraProductosGlobales;
        private ControladoraDatosGenerales controladoraDatosGenerales;

        /*
         * Constructor.
         */
        public ControladoraEntradas()
        {
            controladoraBDEntradas = new ControladoraBDEntradas();
            controladoraProductoLocal = new ControladoraProductoLocal();
            controladoraProductosGlobales = new ControladoraProductosGlobales();
            controladoraBDEntradas.NombreUsuarioLogueado = (this.NombreUsuarioLogueado);
            controladoraProductoLocal.NombreUsuarioLogueado = (this.NombreUsuarioLogueado);
            controladoraProductosGlobales.NombreUsuarioLogueado = (this.NombreUsuarioLogueado);
            controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
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
            Object[] datosConsultados = new Object[8];

            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = resultado.Rows[0][0].ToString();
                for (int i = 1; i < 8; i++)
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

        /*
         * Consulta el nombre de la moneda asociada a una factura para mostrarla en el encabezado de esta.
         */
        public String consultarNombreMoneda(String idMoneda)
        {
            return controladoraBDEntradas.consultarNombreMoneda(idMoneda);
        }

        /*
         * Consulta el impuesto de ventas vigente en el momento.
         */
        public int consultarImpuestoDeVentas()
        {
            return controladoraDatosGenerales.impuestoVentas();
        }
        public string anularEntrada(string idEntrada)
        {
            return controladoraBDEntradas.anularEntrada(idEntrada);
        }
        public String consultarCodigoInventario(String codigoPro, String codigoBodega)
        {
            return controladoraBDEntradas.consultarCodigoInventario(codigoPro, codigoBodega);
        }
    }
}