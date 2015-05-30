﻿using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Modulo_Traslados
{
    public class ControladoraBDTraslado : ControladoraBD
    {


        // Funciona
        public DataTable consultaTraslados(String idBodega, bool entrada)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String tipoConsulta = entrada ? "T.IDBODEGADESTINO = '" + idBodega + "'" : "T.IDBODEGAORIGEN = '" + idBodega + "'";

            try
            {
                // Interfaz ocupa 3 cosas TipoMovimiento(Descripcion), Fecha, Usuario(Encargado)
                // Yo agrego el ID de ajustes para la consulta individual
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT T.ID_TRASLADO, T.FECHA, U.NOMBRE, T.IDBODEGAORIGEN, T.IDBODEGADESTINO, B1.DESCRIPCION, B2.DESCRIPCION, T.ESTADO  "
                   + " FROM " + esquema + "TRASLADOS T, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_BODEGA B1, " + esquema + "CAT_BODEGA B2 "
                   + " WHERE T.USUARIO_BODEGA = U.SEG_USUARIO "
                   + " AND " + tipoConsulta
                   + " AND  T.IDBODEGAORIGEN = B1.CAT_BODEGA" 
                   + " AND T.IDBODEGADESTINO = B2.CAT_BODEGA  ORDER BY T.FECHA DESC"; 
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }

        // LISTO
        public DataTable[] consultarTraslado(String idTraslado)
        {
            String esquema = "Inventarios.";
            DataTable[] resultado = new DataTable[2];
            resultado[0] = new DataTable();
            resultado[1] = new DataTable();

            try
            {
                // Interfaz ocupa 3 cosas TipoMovimiento(Descripcion), Fecha, Usuario(Encargado)
                // Yo agrego el ID de ajustes para la consulta individual
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT T.NOTAS, T.FECHA, U.NOMBRE, B1.DESCRIPCION, B2.DESCRIPCION, T.ESTADO  "
                   + " FROM " + esquema + "TRASLADOS T, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_BODEGA B1, " + esquema + "CAT_BODEGA B2 "
                   + " WHERE T.USUARIO_BODEGA = U.SEG_USUARIO "
                   + " AND  T.IDBODEGAORIGEN = B1.CAT_BODEGA"
                   + " AND T.IDBODEGADESTINO = B2.CAT_BODEGA  ORDER BY T.FECHA DESC";
                OracleDataReader reader = command.ExecuteReader();
                resultado[0].Load(reader);
                resultado[1] = consultarDetalles(idTraslado);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }

        //listo
        private DataTable consultarDetalles(String idTraslado)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT P.NOMBRE, P.CODIGO, D.TRASLADO, U.DESCRIPCION "
                   + " FROM " + esquema + "DETALLES_TRASLADO D, " + esquema + "INV_BODEGA_PRODUCTOS B, " + esquema + "INV_PRODUCTOS P, " + esquema + "CAT_UNIDADES U "
                   + " WHERE D.ID_TRASLADO = '" + idTraslado + "' "
                   + " AND D.INV_BODEGA_PRODUCTOSORIGEN = B.INV_BODEGA_PRODUCTOS "
                   + " AND B.INV_PRODUCTOS = P.INV_PRODUCTOS "
                   + " AND P.CAT_UNIDADES = U.CAT_UNIDADES ";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }



        public String[] insertarAjuste(EntidadTraslado nuevo)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];
            res[3] = generarID();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO " + esquema + "TRASLADOS (ID_TRASLADO, FECHA, USUARIO_BODEGA, IDBODEGAORIGEN, IDBODEGADESTINO, ESTADO, NOTAS) "
               + "VALUES ('" + res[3] + "', TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',  'dd/mm/yyyy hh24:mi:ss') , '"
               + nuevo.IdUsuario + "','" + nuevo.IdBodegaOrigen + "' , '" + nuevo.IdBodegaDestino + "' , " + 0 + ", '" + nuevo.Notas+ "' )";
                OracleDataReader reader = command.ExecuteReader();

                /*foreach (EntidadDetalles detallesProducto in ajuste.Detalles)
                { // Por cada producto meterlo en el detalles ajustes
                    insertarDetalle(res[3], detallesProducto);
                    actualizarProducto(detallesProducto.IdProductoBodega, detallesProducto.Cambio);
                }*/

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Ajuste agregado al sistema.";
            }
            catch (OracleException e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Ajuste no agregado, intente nuevamente.";
            }

            return res;
        }
    }
}