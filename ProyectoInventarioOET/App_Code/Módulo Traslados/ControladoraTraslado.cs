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

            Object[] datos = new Object[10];
            DataTable[] respuesta = controladoraBD.consultarTraslado(idTraslado);
            foreach (DataRow fila in respuesta[0].Rows)
            {  
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
            consultada.IdTraslado = "222";
            consultada.IdUsuario = "3";
            consultada.IdBodegaOrigen = "CYCLO128122012112950388004";
            consultada.IdBodegaDestino = "PITAN129012015101713605001";
            consultada.Notas = "PRIMER INSERTARRRRR";
      

            Object[] datosProductos = new Object[4];
            foreach (DataRow fila in respuesta[1].Rows) 
            {
                datosProductos[0] = fila[0].ToString(); // Nombre
                datosProductos[1] = fila[1].ToString(); // Codigo
                datosProductos[2] = fila[2].ToString(); // Traslado, el parse se hace en la entidad
                datosProductos[3] = fila[3].ToString(); // Unidades
                consultada.agregarDetalle(datosProductos);
            }
            return consultada;
        }

        public String[] insertarAjuste(EntidadTraslado nuevo)
        {
            return controladoraBD.insertarAjuste(nuevo);
        }


        public String[] acertarTraslador(EntidadTraslado aceptado) {
            return controladoraBD.modificarTraslado(aceptado, 1);
        }

        public String[] rechazarTraslador(EntidadTraslado aceptado)
        {
            return controladoraBD.modificarTraslado(aceptado, -1);
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