using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.Modulo_Productos_Locales;
using ProyectoInventarioOET.App_Code.Modulo_Ajustes;

namespace ProyectoInventarioOET.App_Code.Modulo_Traslados
{

    /*
     * Comunicación entre la Controladora de Base de Datos y la que maneja las operaciones de la interfaz.
     */
    public class ControladoraTraslado
    {

        private ControladoraBDTraslado controladoraBD;


        /* Constructor
         *  Se instancia la controladora de Base de datos
         */
        public ControladoraTraslado() {
            controladoraBD = new ControladoraBDTraslado();
        }


        /*
         * Método encargado de consultar las bodegas que el usuario y su rol pueden acceder
         * se utiliza la controladora de bodegas para que no ocurra un translape de capas de diferentes modulos 
         */

        public DataTable consultarBodegas(String idUsuario, String rol)
        {
            ControladoraBodegas controladoraBodega = new ControladoraBodegas();
            return controladoraBodega.consultarBodegas(idUsuario, rol);
        }


        /*
         * Método encargado de la consulta de los productos transferibles (productos presentes y activos en ambas bodegas)
         * Los datos devueltos pueden ser desplegables en la interfaz porque poseen datos humanamente significativos.
         * Ademas de lo más importante a) INV_BODEGA_PRODUCTOS DE ORIGEN, b) INV_BODEGA_PRODUCTOS DESTINO necesarios para la inserción
         */
        public DataTable consultarProductosTrasferibles(String idBodegaOrigen, String idBodegaDestino)
        {
            ControladoraProductoLocal controladoraProductoLocal = new ControladoraProductoLocal();
            return controladoraProductoLocal.consultarProductosDeBodega(idBodegaOrigen, idBodegaDestino);
        }


        /*
         * Método encargado de de consultar datos previos de los traslados de una bodega especifica, los traslados devueltos pueden ser de un 
         * solo tipo: de entrada o de salida y para elegir cual se utiliza el booleano entrada (TRUE= entrada, FALSE= salida)
         */
        public DataTable consultarTraslados(String idBodega, bool entrada)
        {
            DataTable traslados = controladoraBD.consultaTraslados(idBodega, entrada);
            traslados.Columns.Add("DescripcionEstado", typeof(string));     // NUEVA COLUMNA PARA DESCRIPCION DEL ESTADO
            if (traslados.Rows.Count > 0)
            {
                foreach (DataRow fila in traslados.Rows)
                {
                    fila[8] = getNombreEstado(fila[7].ToString()); // Se hace llena la nueva columna con la descripcion del ESTADO
                }
            }
            return traslados;
        }

        /*
         * Método encargado de hacer una consulta total de un traslado especifico, dicha consulta incluye todos los productos 
         * trasladados con su respectiva cantidad. Tambien se encapsula para un mejor manejo en la interfaz de los datos.
         */
        
        public EntidadTraslado consultarTraslado (String idTraslado)
        {
            Object[] datos = new Object[10];
            DataTable[] respuesta = controladoraBD.consultarTraslado(idTraslado);
            foreach (DataRow fila in respuesta[0].Rows)
            {  
                // Leo ponele cuidado a esta forma de encapsular! mas fácil
                datos[0] = idTraslado;          // Id
                datos[1] = fila[1];             //Fecha
                datos[2] = fila[2].ToString();  // usuario
                datos[3] = "";                  //idUsuario
                datos[4] = fila[0].ToString();  //notas 
                datos[5] = "";                  //idBodeOrigen
                datos[6] = "";                  //idBodeDestino
                datos[7] = fila[3].ToString();  //BodeOrigen
                datos[8] = fila[4].ToString();  //BodeDestino
                datos[9] = getNombreEstado(fila[5].ToString()); // Estado
            }

            EntidadTraslado consultada = new EntidadTraslado(datos);

            // Parte de los detalles
            Object[] datosProductos = new Object[6];
            foreach (DataRow fila in respuesta[1].Rows) 
            {
                datosProductos[0] = fila[0].ToString(); // Nombre
                datosProductos[1] = fila[1].ToString(); // Codigo
                datosProductos[2] = fila[2].ToString(); // Traslado, el parseo se hace en la entidad
                datosProductos[3] = fila[3].ToString(); // Unidades
                datosProductos[4] = fila[4].ToString(); // idProdOrigen
                datosProductos[5] = fila[5].ToString(); // idProdDestino
                consultada.agregarDetalle(datosProductos);
            }
            return consultada;
        }


        /*
         * Método encargado de insertar un translado proveniente de los datos ingresados por el usuario, dichos datos vienen
         * encapsuldos en la entidad para facilitar su uso en la sentencia SQL
         */
        public String[] insertarTraslado(EntidadTraslado nuevo)
        {
            return controladoraBD.insertarTraslado(nuevo);
        }

        /* Método encargado de aceptar un traslado especifico que tendra como efecto 
         * descongelar los productos de la bodega de origen y sumarlos a la bodega de destino
        *   y cambiar el estado de dicho traslado
        */ 
        public String[] acertarTraslado(EntidadTraslado aceptado) {
            return controladoraBD.modificarTraslado(aceptado, 1);
        }

        /* Método encargado de rechazar un traslado especifico que tendra como efecto 
         * descongelar y retornar la existencia de los productos de la bodega origen y cambiar el estado de dicho traslado
        */
        public String[] rechazarTraslado(EntidadTraslado aceptado)
        {
            return controladoraBD.modificarTraslado(aceptado, -1);
        }
        
        /* 
         * Método encargado de hacer la traducción del identificador del estado almacenado en la base de datos
         * a una descripcion que sea significativa para su despliegue en la interfaz
         */ 
        private object getNombreEstado(string estado)
        {
            String descripcion = "";
            switch (estado)
            {
                case "-1":
                    descripcion = "Rechazado";
                    break;
                case "1":
                    descripcion = "Aceptado";
                    break;
                default:
                    descripcion = "En Proceso";
                    break;
            }
            return descripcion;
        }



    }
}