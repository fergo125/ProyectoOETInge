using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.Modulo_ProductosLocales
{
    /*
     * Clase de controladora de productos
     */
    public class ControladoraProductoLocal : Controladora
    {
        //Atributos
        private ControladoraBDProductosLocales controladoraBD;  //???
        private EntidadProductoLocal productoActual; //wtf?

        /*
         * Constructor.
         */
        public ControladoraProductoLocal()
        {
            controladoraBD = new ControladoraBDProductosLocales();
            controladoraBD.NombreUsuarioLogueado = (this.NombreUsuarioLogueado);
        }

        /*
         * Consulta los productos que pertenecen a un idBodega específico
         */
        public DataTable consultarProductosDeBodega(String idBodega)
        {
            return controladoraBD.consultarProductosDeBodega(idBodega);
        }

        /*
         * Consulta los productos que pertenecen a un idBodega específico, especialziado para modulo ajustes.
         */
        public DataTable consultarProductosDeBodegaAjustes(String idBodega)
        {
            return controladoraBD.consultarProductosDeBodegaAjustes(idBodega);
        }

        /*
         * Consulta los datos de un producto específico perteneciente a una bodega.
         */
        public DataTable consultarProductoDeBodega(String idBodega, String idProducto)
        {
            return controladoraBD.consultarProductoDeBodega(idBodega, idProducto);
        }

        /*
         *  Realiza la asociación de un producto a una bodega en específico. 
         */
        public string[] asociarProductos(String idBodega, String idProducto, String idUsuario)
        {
            return controladoraBD.asociarProductos(idBodega,idProducto,idUsuario);
        }

        /*
         * Realiza la modificación de un producto en una bodega específica, modificando su estado. 
         */
        public string[] modificarProductoLocal(String idBodegaProductos, String est,String min, String max)
        {
            int estado;
            if(est.Equals("Activo"))
            {
                estado=1;
            }
            else
            {
                estado=0;
            }
            return controladoraBD.modificarProductoLocal(idBodegaProductos, estado,min,max);
        }

        //B1.INV_PRODUCTOS, P.NOMBRE, P.CODIGO, B1.SALDO, B1.MINIMO, B1.MAXIMO, B1.INV_BODEGA_PRODUCTOS, B2.INV_BODEGA_PRODUCTOS
        public DataTable consultarProductosDeBodega(string idBodegaOrigen, string idBodegaDestino, String query)
        {
            return controladoraBD.consultarProductosDeBodega(idBodegaOrigen, idBodegaDestino, query);
        }

        /*
         * Consulta el maximo y el minimo para un producto en una bodega especifica
         */
        public DataTable consultarMinimoMaximoProductoEnBodega(String idProductoEnBodega)
        {
            return controladoraBD.consultarMinimoMaximoProductoEnBodega(idProductoEnBodega);
        }

    }
}