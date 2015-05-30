﻿using System;
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
            if (traslados.Rows.Count > 0)
            {
                foreach (DataRow fila in traslados.Rows)
                {
                    switch (fila[7].ToString()) { 
                        case  "-1":
                            fila[8] = "Rechazado";
                            break;
                        case "1":
                            fila[8] = "Aceptado";
                            break;
                        default:
                            fila[8] = "En Proceso";
                            break;
                    }

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
                datos[0] = fila[0].ToString();
                datos[1] = fila[1].ToString();
                datos[2] = fila[2];  // Es la fecha
                datos[3] = fila[3].ToString(); // Es la bodega
                datos[4] = fila[4].ToString();
            }

            EntidadTraslado consultada = new EntidadTraslado(datos);

            Object[] datosProductos = new Object[4];
            foreach (DataRow fila in respuesta[1].Rows) // Varias filas que corresponden a los productos
            {
                datosProductos[0] = fila[0].ToString();
                datosProductos[1] = fila[1].ToString();
                datosProductos[2] = fila[2];  // Es la fecha
                datosProductos[3] = fila[3].ToString();
                consultada.agregarDetalle(datosProductos);
            }

            //consultada.IdBodega = "PITAN129012015101713605001";
            //consultada.IdUsuario = "3";
            //consultada.Notas = "PRUEBADEINSERCIONALOMACHO";

            //controladoraBD.insertarAjuste(consultada);
            return consultada;
        }



    }
}