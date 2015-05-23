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

        public EntidadAjustes consultarAjuste(String idAjuste)
        {
            
            Object[] datos = new Object[4];
            DataTable[] respuesta = controladoraBD.consultarAjuste(idAjuste);
            foreach (DataRow fila in respuesta[0].Rows) {  //Solo seria una fila
                datos[0] = fila[0].ToString();
                datos[1] = fila[1].ToString();
                datos[2] = fila[2];  // Es la fecha
                datos[3] = fila[3].ToString(); // Es la bodega
            }

            EntidadAjustes consultada = new EntidadAjustes(datos);

            Object[] datosProductos = new Object[4];
            foreach (DataRow fila in respuesta[1].Rows) // Varias filas que corresponden a los productos
            {  //Solo seria una fila
                datosProductos[0] = fila[0].ToString();
                datosProductos[1] = fila[1].ToString();
                datosProductos[2] = fila[2];  // Es la fecha
                datosProductos[3] = fila[3].ToString();
            }            
            
            return consultada;
            //EntidadAjustes respuesta =    
        }

    }
}