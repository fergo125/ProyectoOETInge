using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.Modulo_Productos_Locales;
using ProyectoInventarioOET.App_Code.Modulo_Traslados;


namespace ProyectoInventarioOET.App_Code.Modulo_Ajustes
{
    /*
     * Comunicación entre la Controladora de Base de Datos y la que maneja las operaciones de la interfaz.
     */
    public class ControladoraAjustes
    {

        private ControladoraBDAjustes controladoraBD;


        // Constructor
        public ControladoraAjustes() {
            controladoraBD = new ControladoraBDAjustes();
        }
        
        /* Metodo que trae las Bodegas de una estacion
        * Es llamado en el evento del dropdown list de estacion
         */
        public DataTable consultarBodegasDeEstacion(String codigo)
        {
            ControladoraBodegas controladoraBodegas = new ControladoraBodegas();
            return controladoraBodegas.consultarBodegasDeEstacion(codigo);
        }


        /* Método que consulta los datos del catalogo local de la bodega (esta consulta se hace a la hora de 
        * clickear el boton agregar producto
        * Lo malo es que vienen todos los datos....
        */
        public DataTable consultarProductosDeBodega(String idBodega)
        {
            ControladoraProductoLocal controladoraProductoLocal = new ControladoraProductoLocal();
            return controladoraProductoLocal.consultarProductosDeBodegaAjustes(idBodega);
        }

        /*
         * Metodo encargado de comunicar con la controladoraBD y obtener el tipo de ajuste
         */
        public DataTable tiposAjuste()
        {
            return controladoraBD.tiposAjuste();
        }

        /*
         * Metodo encargado de comunicar con la controladoraBD y obtener datos preliminares
         * de todos los ajustes de una bodega
         */
        public DataTable consultarAjustes(String idBodega) {
            return controladoraBD.consultarAjustes(idBodega);
        }

        /*
        * Metodo encargado de comunicar con la controladoraBD y insertar los datos ingresados por el usuario
        */
        public String[] insertarAjuste(EntidadAjustes nueva) {
            return controladoraBD.insertarAjuste(nueva);
        }

        /*
         * Metodo encargado de comunicar con la controladoraBD y obtener datos totales de un ajuste especifico
         * tambien se encarga de encapsular los datos para un mejor manejo en la interfaz
         */
        public EntidadAjustes consultarAjuste(String idAjuste)
        {
            
            Object[] datos = new Object[5];
            DataTable[] respuesta = controladoraBD.consultarAjuste(idAjuste);
            foreach (DataRow fila in respuesta[0].Rows) {  //Solo seria una fila
                datos[0] = fila[0].ToString();
                datos[1] = fila[1].ToString();
                datos[2] = fila[2];  // Es la fecha
                datos[3] = fila[3].ToString(); // Es la bodega
                datos[4] = fila[4].ToString();
            }

            EntidadAjustes consultada = new EntidadAjustes(datos);

            Object[] datosProductos = new Object[4];
            foreach (DataRow fila in respuesta[1].Rows) // Varias filas que corresponden a los productos
            {  
                datosProductos[0] = fila[0].ToString();
                datosProductos[1] = fila[1].ToString();
                datosProductos[2] = fila[2];  // Es la fecha
                datosProductos[3] = fila[3].ToString();
                consultada.agregarDetalle(datosProductos);
            }
            return consultada;
        }

    }
}