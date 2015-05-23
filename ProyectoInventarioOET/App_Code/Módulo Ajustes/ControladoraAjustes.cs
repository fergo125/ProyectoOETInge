using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.Módulo_Bodegas;
using ProyectoInventarioOET.Módulo_Productos_Locales;


namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{
    public class ControladoraAjustes
    {

        // Las estaciones se cargan desde la 

        private ControladoraBDAjustes controladoraBD;

        public ControladoraAjustes() {
            controladoraBD = new ControladoraBDAjustes();
        }
        
        //Metodo que trae las Bodegas de una estacion
        // Es llamado en el evento del dropdown list de estacion
        public DataTable consultarBodegasDeEstacion(String codigo)
        {
            ControladoraBodegas controladoraBodegas = new ControladoraBodegas();
            return controladoraBodegas.consultarBodegasDeEstacion(codigo);
        }


        //Metodo que consulta los datos del catalogo local de la bodega (esta consulta se hace a la hora de 
        // clickear el boton agregar producto
        // Lo malo es que vienen todos los datos....

        public DataTable consultarProductosDeBodega(String idBodega)
        {
            ControladoraProductoLocal controladoraProductoLocal = new ControladoraProductoLocal();
            return controladoraProductoLocal.consultarProductosDeBodega(idBodega);
        }

        public DataTable tiposAjuste()
        {
            return controladoraBD.tiposAjuste();
        }

        public DataTable consultarAjustes(String idBodega) {
            return controladoraBD.consultarAjustes(idBodega);
        }

        public DataTable consultarAjuste(String idAjuste)
        {
            return controladoraBD.consultarAjuste(idAjuste);
        }

    }
}