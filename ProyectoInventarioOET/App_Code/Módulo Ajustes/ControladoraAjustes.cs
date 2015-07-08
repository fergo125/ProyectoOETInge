using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.Modulo_ProductosLocales;
using ProyectoInventarioOET.Modulo_Traslados;
using ProyectoInventarioOET.Modulo_Seguridad;


namespace ProyectoInventarioOET.Modulo_Ajustes
{
    /*
     * Comunicación entre la Controladora de Base de Datos y la que maneja las operaciones de la interfaz.
     */
    public class ControladoraAjustes : Controladora
    {

        private ControladoraBDAjustes controladoraBD;


        /* Constructor
         *  Se instancia la controladora de Base de datos
         */
        public ControladoraAjustes()
        {
            controladoraBD = new ControladoraBDAjustes();
            controladoraBD.NombreUsuarioLogueado = (this.NombreUsuarioLogueado);
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
            /*ControladoraSeguridad segu = new ControladoraSeguridad(); //PRUEBA PARA CONSULTA DE CUENTAS
            DataTable test = segu.consultarCuentas();
            EntidadUsuario test2 = segu.consultarCuenta("3");*/ 
            Object[] datos = new Object[8];
            DataTable[] respuesta = controladoraBD.consultarAjuste(idAjuste);
            foreach (DataRow fila in respuesta[0].Rows) {  //Solo seria una fila
                datos[0] = fila[0].ToString(); //Tipo de ajuste (codigo)
                datos[1] = fila[1].ToString(); // Fecha
                datos[2] = fila[2];  // Es el usuario
                datos[3] = fila[3].ToString(); // Es el id del usuario
                datos[4] = fila[4].ToString(); // SOn las notas del ajuste
                datos[5] = fila[5].ToString(); //Es el Id de la bodega
                datos[6] = fila[7].ToString(); //Es el estado de la bodega    
                datos[7] = fila[8].ToString(); //Es el anulable de la bodega     
            }

            EntidadAjustes consultada = new EntidadAjustes(datos);

            Object[] datosProductos = new Object[7];
            foreach (DataRow fila in respuesta[1].Rows) // Varias filas que corresponden a los productos
            {  
                datosProductos[0] = fila[0].ToString(); //Nombre del producto
                datosProductos[1] = fila[1].ToString(); //Codigo del producto (CR0...)
                datosProductos[2] = Double.Parse(fila[2].ToString()) - Double.Parse(fila[3].ToString());  // Cambio actual - previo
                datosProductos[3] = fila[4].ToString(); // Id del producto en la bodega
                datosProductos[4] = fila[2].ToString(); // Cantidad Previa al ajuste
                datosProductos[5] = fila[3].ToString(); //Cantidad nueva
                consultada.agregarDetalle(datosProductos);
            }
            return consultada;
        }

        /*
         * Metodo encargado de comunicar con la controladoraBD y anular el ajuste especifico
         * dejando las existencias del producto previas al ajuste
         */
        public String[] anularAjuste(EntidadAjustes ajuste, String idAjuste) {
            return controladoraBD.anularAjuste(ajuste, idAjuste); 
        }

    }
}