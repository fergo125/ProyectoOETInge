using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.Modulo_Categorias;

namespace ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales
{
    public class ControladoraProductosGlobales
    {
        private ControladoraBDProductosGlobales controladoraBD;
        private EntidadProductoGlobal productoActual; // 

        public ControladoraProductosGlobales()
        {
            controladoraBD = new ControladoraBDProductosGlobales();
        }


        public EntidadProductoGlobal consultarProductoGlobal(String id)
        {
            productoActual = controladoraBD.consultarProductoGlobal(id);
            return productoActual;
        }

        public String[] insertar(Object[] datosProductoGlobal)
        {
            EntidadProductoGlobal productoGlobal = new EntidadProductoGlobal(datosProductoGlobal);
            return controladoraBD.insertarProductoGlobal(productoGlobal);
        }

        public String[] modificarDatos(EntidadProductoGlobal productoGlobalViejo, Object[] datosProductoGlobalModificado)
        {
            EntidadProductoGlobal productoGlobalModificado = new EntidadProductoGlobal(datosProductoGlobalModificado);
            return controladoraBD.modificarProductoGlobal(productoGlobalViejo, productoGlobalModificado);
        }

        public String[] desactivarProductoGlobal(EntidadProductoGlobal productoGlobal)
        {
            /*desactiva una bodega de la base de datos*/
            return controladoraBD.desactivarProductoGlobal(productoGlobal);
        }

        /*
         * Método para cargar el grid
         */
        public DataTable consultarProductosGlobales()
        {
            return ControladoraBDProductosGlobales.consultarProductosGlobales();
        }

        public DataTable consultarCategorias()
        {

            ControladoraCategorias controladoraCategorias = new ControladoraCategorias();
            return controladoraCategorias.consultarCategorias();
            //Hacer llamado a la controladora de fer ;
        }

        public DataTable consultarProductosGlobales(string query)
        {
            return ControladoraBDProductosGlobales.consultarProductosGlobales(query); ;
        }
    }
}