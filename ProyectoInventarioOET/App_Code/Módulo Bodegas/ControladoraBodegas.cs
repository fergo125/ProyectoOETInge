using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.Modulo_Bodegas
{
    /*
     * ???
     */
    public class ControladoraBodegas
    {
        //Atributos
        private ControladoraBDBodegas controladoraBDBodegas;    //???

        /*
         * Constructor.
         */
        public ControladoraBodegas()
        {
            controladoraBDBodegas = new ControladoraBDBodegas();
        }

        /*
         * Consulta la información de una bodega particular.
         */
        public EntidadBodega consultarBodega(String id)
        {
            return controladoraBDBodegas.consultarBodega(id);
        }

        /*
         * Crea una nueva bodega dado un vector con los datos de la misma.
         */
        public String[] insertarDatos(Object[] datosBodega, String idUsuario, String rol)
        {
            EntidadBodega bodega = new EntidadBodega(datosBodega);
            return controladoraBDBodegas.insertarBodega(bodega, idUsuario, rol);
        }

        /*
         * Modifica los datos de una bodega particular.
         */
        public String[] modificarDatos(EntidadBodega bodegaVieja, Object[] datosBodegaNueva, String idUsuario, String rol)
        {
            EntidadBodega bodegaNueva = new EntidadBodega(datosBodegaNueva);
            return controladoraBDBodegas.modificarBodega(bodegaVieja, bodegaNueva, idUsuario, rol);
        }

        /*
         * Consulta la información de todas las bodegas.
         */
        public DataTable consultarBodegas(String idUsuario, String rol)
        {
            return controladoraBDBodegas.consultarBodegas(idUsuario,rol);
        }

        /*
         * Consulta la información de las bodegas pertenecientes a la estación.
         */
        public DataTable consultarBodegasDeEstacion(String codigo)
        {
            return controladoraBDBodegas.consultarBodegasDeEstacion((codigo == "All" ? null : codigo));  //Si se pregunta por todas ("All"), se envía null para que entienda
        }
        /*
         * Obtiene la informacion de los productos que no pertenecen a la bodega especificada
         */
        public DataTable consultarProductosAsociables(String idBodega)
        {
            return controladoraBDBodegas.consultarProductosAsociables(idBodega);
        }
        /*
         * Consulta el ID de la estación a la cual pertenece una bodega
         */
        public DataTable consultarEstacionDeBodega(String idBodega)
        {
            return controladoraBDBodegas.consultarEstacionDeBodega(idBodega);
        }
    }
}