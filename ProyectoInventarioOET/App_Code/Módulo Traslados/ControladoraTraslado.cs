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
    public class ControladoraTraslado
    {

        private ControladoraBDTraslado controladoraBD;
        

        public ControladoraTraslado() {
            controladoraBD = new ControladoraBDTraslado();
        }


        /*
         * CONSULTA DE LAS BODEGAS
         */

        public DataTable consultarBodegas(String idUsuario, String rol)
        {
            ControladoraBodegas controladoraBodega = new ControladoraBodegas();
            return controladoraBodega.consultarBodegas(idUsuario, rol);
        }


        /*
         * CONSULTA DE LOS PRODUCTOS TRANSFERIBLES (PRODUCTOS QUE ESTEN EN AMBAS ESTACIONES)
         * CONTIENE: EL ID DEL PRODUCTO GLOBAL, NOMBRE DEL PRODUCTO, CODIGO DEL PRODUCTO, SALDO EN LA BODEGA ORIGEN, 
         * MINIMO EN LA BODEGA ORIGEN, MAXIMO EN LA BODEGA ORIGEN, 
         * Y LOS MAS IMPORTANTES!!!! INV_BODEGA_PRODUCTOS DE ORIGEN, INV_BODEGA_PRODUCTOS DESTINO NECESARIOS PARA LA INSERCION
         */
        public DataTable consultarProductosTrasferibles(String idBodegaOrigen, String idBodegaDestino)
        {
            ControladoraProductoLocal controladoraProductoLocal = new ControladoraProductoLocal();
            return controladoraProductoLocal.consultarProductosDeBodega(idBodegaOrigen, idBodegaDestino);
        }


        // CONSULTA DE TRASLADOS (NO INCLUYEN LOS DETALES DEL TRASLADO)
        // RECORDAR     -1: Traslado Rechazado  0: Traslado Anulado    1: Traslado Aceptado
        // RECORDAR     entrada=true  Traslados de entrada       entrada=false  Traslados de salida 
        // La consulta esta parametrizada para devolver los traslados de entrada o de salida pero no ambos
        // 
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
        // CONSULTA DE TRASLADO MAS SUS RESPECTIVOS DETALLES (Los productos trasladados) 
        // LA ENTIDAD DEVUELTA CONTIENE: NOTAS, FECHA, NOMBRE DEL RESPONSABLE, BODEGA ORIGEN, BODEGA DESTINO , ESTADO (SU DESCRIPCION)
        // LOS DETALLES (Productos trasladados) tienen NOMBRE, CODIGO, CANTIDAD TRASLADADA (para despliegue) 
        //                                      Y ID_PRODUCTODO_ORIGEN, ID_PRODUCTODO_ORIGEN (IMPORTANTE PARA ACEPTAR Y RECHAZAR UN TRASLADO!!!)
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


        // Este metodo hace 3 cosas 1) INSERTA EL NUEVO TRASLADO 2) INSERTA LOS DETALLES (Productos trasferibles) 3) RESTA LOS PRODUCTOS EN LA BODEGA ORIGEN Y LOS PONE EN LA COLUMNA CONGELADOS 
        // La Entidad Traslado debe tener obligatoriamente:  ID DEL USUARIO, ID DE BODEGA ORIGEN, ID BODEGA DESTINO Y NOTAS
        // lo demás se agrega automaticamente en la ControladoraBDTraslado
        // La EntidadDetalle debe tener obligatoriamente: El Cambio (Traslado), Y LO MAS IMPORTANTE!!!!
        // El Id BODEGA_PRODUCTO ORIGEN Y EL Id BODEGA_PRODUCTO DESTINO
        public String[] insertarAjuste(EntidadTraslado nuevo)
        {
            return controladoraBD.insertarAjuste(nuevo);
        }

        // DESCONGELA LOS PRODUCTOS DE LA BODEGA ORIGEN Y LOS SUMA A LA BODEGA DESTINO
        // LEO este recibe la EntidadTraslado porque para aceptarlo debe ser previamente Consultado!
        public String[] acertarTraslado(EntidadTraslado aceptado) {
            return controladoraBD.modificarTraslado(aceptado, 1);
        }

        // DESCONGELA LOS PRODUCTOS DE LA BODEGA ORIGEN Y LOS SUMA A LA BODEGA DESTINO
        // LEO este recibe la EntidadTraslado porque para aceptarlo debe ser previamente Consultado!
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