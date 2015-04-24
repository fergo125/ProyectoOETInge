using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace ProyectoInventarioOET.Módulo_Bodegas
{
    public class ControladoraBodegas
    {
        
        private ControladoraBDBodegas controladoraBDBodegas;

        public ControladoraBodegas()
        {
            controladoraBDBodegas = new ControladoraBDBodegas();
        }


        public EntidadBodega consultarBodega(String id)
        {
            /*consulta la información de una bodega particular*/
            return controladoraBDBodegas.consultarBodega(id);
        }

        public String[] insertarDatos(Object[] datosBodega)
        {
            /*crea una nueva bodega dado un vector con los datos de la misma*/
            EntidadBodega bodega = new EntidadBodega(datosBodega);
            return controladoraBDBodegas.insertarBodega(bodega);
        }

        public String[] modificarDatos(EntidadBodega bodegaVieja, Object[] datosBodegaNueva)
        {
            /*modifica los datos de una bodega particular*/
            EntidadBodega bodegaNueva = new EntidadBodega(datosBodegaNueva);
            return controladoraBDBodegas.modificarBodega(bodegaVieja, bodegaNueva);
        }

        public String[] desactivarBodega(EntidadBodega bodega)
        {
            /*desactiva una bodega de la base de datos*/
            return controladoraBDBodegas.desactivarBodega(bodega);
        }

        public DataTable consultarBodegas()
        {
            /*consulta la información de todas las bodegas*/
            return controladoraBDBodegas.consultarBodegas();
        }

        public DataTable consultarBodegasDeEstacion(String codigo)
        {
            /*consulta la información de las bodegas pertenecientes a la estación*/
            return controladoraBDBodegas.consultarBodegasDeEstacion(codigo);
        }
    }
}