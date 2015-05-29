using Oracle.DataAccess.Client;
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
                command.CommandText = "SELECT T.ID_TRASLADO, T.NOTAS, T.FECHA, U.NOMBRE, T.IDBODEGAORIGEN, T.IDBODEGADESTINO, B1.DESCRIPCION, B2.DESCRIPCION  "
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

        // Falata arreglar
        public DataTable[] consultarTraslado(String idTraslado)
        {
            String esquema = "Inventarios.";
            DataTable[] resultado = new DataTable[2];

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT M.CAT_TIPO_MOVIMIENTO, AJ.FECHA, U.NOMBRE, U.SEG_USUARIO, AJ.NOTAS ,AJ.IDBODEGA, M.DESCRIPCION  "
                   + " FROM " + esquema + "AJUSTES AJ, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_TIPO_MOVIMIENTO M"
                   + " WHERE AJ.USUARIO_BODEGA = U.SEG_USUARIO "
                   + " AND AJ.CAT_TIPO_MOVIMIENTO = M.CAT_TIPO_MOVIMIENTO"
                   + " AND AJ.ID_AJUSTES = '" + idTraslado + "' ";
                OracleDataReader reader = command.ExecuteReader();
                resultado[0] = new DataTable();
                resultado[0].Load(reader);
                resultado[1] = consultarDetalles(idTraslado);
                int x = 9;
                x = 8;
                //actualizarProducto("PITAN102022015142627451180", 10, true); // PRUEBAAAA QUE FUNCIONA
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }


        private DataTable consultarDetalles(String idAjuste)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT P.NOMBRE, P.CODIGO, T.TRASLADO, B.INV_BODEGA_PRODUCTOS, B.SALDO, U.DESCRIPCION "
                   + " FROM " + esquema + "DETALLES_TRASLADO T, " + esquema + "INV_BODEGA_PRODUCTOS B, " + esquema + "INV_PRODUCTOS P, " + esquema + "CAT_UNIDADES U "
                   + " WHERE D.ID_AJUSTES = '" + idAjuste + "' "
                   + " AND D.INV_BODEGA_PRODUCTOS = B.INV_BODEGA_PRODUCTOS "
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


    }
}