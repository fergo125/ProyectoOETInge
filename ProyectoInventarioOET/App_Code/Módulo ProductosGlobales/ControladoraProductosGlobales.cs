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


        public EntidadProductoGlobal consultarProductoGlobal(String id)
        {
            productoActual = controladoraBD.consultarProductoGlobal(id);
            return productoActual;
        }

        public String[] insertarDatos(String nombre, String categoria, String unidades, String codigo, String codigoDeBarras, String estacion,
            int estado, double costoColones, double costoDolares )
        {
            Object[] datosProductoGlobal = new Object[9];
            datosProductoGlobal[0] = nombre;
            datosProductoGlobal[1] = categoria;
            datosProductoGlobal[2] = unidades;
            datosProductoGlobal[3] = codigo;
            datosProductoGlobal[4] = codigoDeBarras;
            datosProductoGlobal[5] = estacion;
            datosProductoGlobal[6] = estado;
            datosProductoGlobal[7] = costoColones;
            datosProductoGlobal[8] = costoColones;
            EntidadProductoGlobal productoGlobal = new EntidadProductoGlobal(datosProductoGlobal);
            return controladoraBD.insertarProductoGlobal(productoGlobal);
        }

        public String[] modificarDatos(EntidadProductoGlobal productoViejo, Object[] datosProductoNuevo)
        {
            /*modifica los datos de una bodega particular*/
            EntidadProductoGlobal bodegaNueva = new EntidadProductoGlobal(datosProductoNuevo);
            return controladoraBD.modificarProductoGlobal(bVieja, bodegaNueva);
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
    }
}