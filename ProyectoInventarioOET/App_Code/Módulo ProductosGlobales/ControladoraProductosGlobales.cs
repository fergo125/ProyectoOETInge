using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales
{
    public class ControladoraProductosGlobales
    {
        private ControladoraBDProductosGlobales controladoraBD;

        public ControladoraProductosGlobales()
        {
            controladoraBD = new ControladoraBDProductosGlobales();
        }


        public EntidadProductoGlobal consultarProductoGlobal(int id)
        {
            /*consulta la información de una bodega particular*/
            return ControladoraBDProductosGlobales.consultarProductoGlobal(id);
        }

        public String[] insertarDatos(Object[] datosBodega)
        {
            /*crea una nueva bodega dado un vector con los datos de la misma*/
            EntidadProductoGlobal productoGlobal = new EntidadProductoGlobal(datosBodega);
            return ControladoraBDProductosGlobales.insertarProductoGlobal(productoGlobal);
        }

        public String[] modificarDatos(EntidadProductoGlobal bodegaVieja, Object[] datosBodegaNueva)
        {
            /*modifica los datos de una bodega particular*/
            EntidadProductoGlobal bodegaNueva = new EntidadProductoGlobal(datosBodegaNueva);
            return ControladoraBDProductosGlobales.modificarProductoGlobal(bodegaVieja, bodegaNueva);
        }

        public String[] desactivarProductoGlobal(EntidadProductoGlobal productoGlobal)
        {
            /*desactiva una bodega de la base de datos*/
            return ControladoraBDProductosGlobales.desactivarProductoGlobal(productoGlobal);
        }

        /*
         * Método para cargar el grid
         */
        public DataTable consultarProductosGlobales()
        {
            /*consulta la información de todas los productos globales*/
            return ControladoraBDProductosGlobales.consultarProductosGlobales();
        }
    }
}