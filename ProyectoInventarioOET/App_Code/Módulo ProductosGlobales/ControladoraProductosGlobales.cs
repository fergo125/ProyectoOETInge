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

        public String[] modificarDatos(EntidadProductoGlobal productoGlobalViejo, String nombreModificado, String categoriaModificado, String unidadesModificado, String codigoModificado,
            String codigoDeBarrasModificado, String estacionModificado, int estadoModificado, double costoColonesModificado, double costoDolaresModificado)
        {
            Object[] datosProductoGlobalModificado = new Object[9];
            datosProductoGlobalModificado[0] = nombreModificado;
            datosProductoGlobalModificado[1] = categoriaModificado;
            datosProductoGlobalModificado[2] = unidadesModificado;
            datosProductoGlobalModificado[3] = codigoModificado;
            datosProductoGlobalModificado[4] = codigoDeBarrasModificado;
            datosProductoGlobalModificado[5] = estacionModificado;
            datosProductoGlobalModificado[6] = estadoModificado;
            datosProductoGlobalModificado[7] = costoColonesModificado;
            datosProductoGlobalModificado[8] = costoColonesModificado;
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
    }
}