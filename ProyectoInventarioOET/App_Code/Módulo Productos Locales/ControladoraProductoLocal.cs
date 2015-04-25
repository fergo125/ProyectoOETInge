using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.Módulo_Productos_Locales
{
    public class ControladoraProductoLocal
    {
        private ControladoraBDProductosLocales controladoraBD;
        private EntidadProductoLocal productoActual; 

        public ControladoraProductoLocal()
        {
            controladoraBD = new ControladoraBDProductosLocales();
        }

        public DataTable consultarProductosDeBodega(String idBodega)
        {
            return controladoraBD.consultarProductosDeBodega(idBodega);
        }


        /*public EntidadProductoLocal consultarProductoLocal(String id)
        {
            productoActual = controladoraBD.consultarProductoLocal(id);
            return productoActual;
        }

        public String[] insertarDatos(String nombre, String categoria, String unidades, String codigo, String codigoDeBarras, String estacion,
            int estado, double costoColones, double costoDolares)
        {
            Object[] datosProductoLocal = new Object[9];
            datosProductoLocal[0] = nombre;
            datosProductoLocal[1] = categoria;
            datosProductoLocal[2] = unidades;
            datosProductoLocal[3] = codigo;
            datosProductoLocal[4] = codigoDeBarras;
            datosProductoLocal[5] = estacion;
            datosProductoLocal[6] = estado;
            datosProductoLocal[7] = costoColones;
            datosProductoLocal[8] = costoColones;
            EntidadProductoLocal productoLocal = new EntidadProductoLocal(datosProductoLocal);
            return controladoraBD.insertarProductoLocal(productoLocal);
        }

        public String[] modificarDatos(EntidadProductoLocal productoLocalViejo, String nombreModificado, String categoriaModificado, String unidadesModificado, String codigoModificado,
            String codigoDeBarrasModificado, String estacionModificado, int estadoModificado, double costoColonesModificado, double costoDolaresModificado)
        {
            Object[] datosProductoLocalModificado = new Object[9];
            datosProductoLocalModificado[0] = nombreModificado;
            datosProductoLocalModificado[1] = categoriaModificado;
            datosProductoLocalModificado[2] = unidadesModificado;
            datosProductoLocalModificado[3] = codigoModificado;
            datosProductoLocalModificado[4] = codigoDeBarrasModificado;
            datosProductoLocalModificado[5] = estacionModificado;
            datosProductoLocalModificado[6] = estadoModificado;
            datosProductoLocalModificado[7] = costoColonesModificado;
            datosProductoLocalModificado[8] = costoColonesModificado;
            EntidadProductoLocal productoLocalModificado = new EntidadProductoLocal(datosProductoLocalModificado);
            return controladoraBD.modificarProductoLocal(productoLocalViejo, productoLocalModificado);
        }*/
        /*
        public String[] desactivarProductoLocal(EntidadProductoLocal productoLocal)
        {
            /*desactiva una bodega de la base de datos
            return controladoraBD.desactivarProductoGlobal(productoLocal);
        }

        /*
         * Método para cargar el grid
         */
        /*public DataTable consultarProductosLocales()
        {
            /*consulta la información de todas los productos globales
            return ControladoraBDProductosLocales.consultarProductosLocales();
        }*/
    }
}