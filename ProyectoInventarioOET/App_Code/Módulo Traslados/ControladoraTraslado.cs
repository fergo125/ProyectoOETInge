using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.Modulo_Productos_Locales;

namespace ProyectoInventarioOET.App_Code.Modulo_Traslados
{
    public class ControladoraTraslado
    {

        private ControladoraBDTraslado controladoraBD;
        

        public ControladoraTraslado() {
            controladoraBD = new ControladoraBDTraslado();
        }


        public DataTable consultarBodegas(String idUsuario, String rol)
        {
            ControladoraBodegas controladoraBodega = new ControladoraBodegas();
            return controladoraBodega.consultarBodegas(idUsuario, rol);
        }


        // Lista de productos para poder trasladar
        public DataTable consultarProductosDeBodega(String idBodegaOrigen, String idBodegaDestino)
        {
            ControladoraProductoLocal controladoraProductoLocal = new ControladoraProductoLocal();
            return controladoraProductoLocal.consultarProductosDeBodega(idBodegaOrigen, idBodegaDestino);
        }


        // Consulta de los traslados tanto entrantes como salientes de la bodega actual (con la que esta loggeado)
        // RECORDAR     -1: Traslado Rechazado
        //               0: Traslado Anulado
        //               1: Traslado Aceptado
        public DataTable consultarTraslados(String idBodega, bool entrada)
        {
            DataTable traslados = controladoraBD.consultaTraslados(idBodega, entrada);
            traslados.Columns.Add("DescripcionEstado", typeof(string));
            //DataTable[] prueba = controladoraBD.consultarTraslado("1111"); //PRUEBA CARLOS
            if (traslados.Rows.Count > 0)
            {
                foreach (DataRow fila in traslados.Rows)
                {
                    fila[8] = getNombreEstado(fila[7].ToString()); 
                }
            }
            return traslados;
        }

        public EntidadTraslado consultarTraslado (String idTraslado)
        {

            Object[] datos = new Object[5];
            DataTable[] respuesta = controladoraBD.consultarTraslado(idTraslado);
            foreach (DataRow fila in respuesta[0].Rows)
            {  //Solo seria una fila
                datos[0] = idTraslado;
                datos[1] = fila[1].ToString(); //Fecha
                datos[2] = fila[2];  //
                datos[3] = fila[3].ToString(); // Es la bodega
                datos[4] = fila[4].ToString();
            }

            EntidadTraslado consultada = new EntidadTraslado(datos);

            Object[] datosProductos = new Object[4];
            foreach (DataRow fila in respuesta[1].Rows) // Varias filas que corresponden a los productos
            {
                datosProductos[0] = fila[0].ToString(); // Id
                datosProductos[1] = fila[1];            //Fecha
                datosProductos[2] = fila[2].ToString();  // usuario
                datosProductos[3] = fila[4].ToString(); //idUsuario
                datosProductos[4] = fila[5].ToString(); //notas 
                datosProductos[5] = fila[3].ToString(); //idBodeOrigen
                datosProductos[6] = fila[3].ToString(); //idBodeDestino
                datosProductos[7] = fila[3].ToString(); //BodeOrigen
                datosProductos[8] = fila[3].ToString(); //BodeDestino
                datosProductos[9] = getNombreEstado (fila[9].ToString()); // Estado
                consultada.agregarDetalle(datosProductos);
            }

            //consultada.IdBodega = "PITAN129012015101713605001";
            //consultada.IdUsuario = "3";
            //consultada.Notas = "PRUEBADEINSERCIONALOMACHO";

            //controladoraBD.insertarAjuste(consultada);
            return consultada;
        }

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