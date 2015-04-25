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
        private EntidadProductoGlobal productoActual; // 

        public ControladoraProductosGlobales()
        {
            controladoraBD = new ControladoraBDProductosGlobales();
        }


        public EntidadProductoGlobal consultar(String id)
        {
            productoActual = controladoraBD.consultarProductoGlobal(id);
            return productoActual;
        }

        public String[] insertar(Object[] datosProductoGlobal)
        {
            //Logica de generacion de identificador
            //datosProductoGlobal[9] = codigoAutogenerado();
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
            /*consulta la información de todas los productos globales*/
            return ControladoraBDProductosGlobales.consultarProductosGlobales();
        }

        public DataTable consultarCategorias()
        {
            DataTable resp = new DataTable();
            return resp;
            //Hacer llamado a la controladora de fer ;
        }
    }
}