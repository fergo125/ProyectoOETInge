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
        public DataTable consultaTraslados(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();

            try
            {
                // Interfaz ocupa 3 cosas TipoMovimiento(Descripcion), Fecha, Usuario(Encargado)
                // Yo agrego el ID de ajustes para la consulta individual
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT T.ID_TRASLADO, T.NOTAS, T.FECHA, U.NOMBRE, T.IDBODEGAORIGEN "
                   + " FROM " + esquema + "TRASLADOS T, " + esquema + "SEG_USUARIO U "
                   + " WHERE T.USUARIO_BODEGA = U.SEG_USUARIO "
                   + " AND ( T.IDBODEGAORIGEN = '" + idBodega + "' OR T.IDBODEGADESTINO = '" + idBodega + "') ORDER BY T.FECHA DESC";
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
        public DataTable[] consultarTraslado(String idTraslados)
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
                   + " AND AJ.ID_AJUSTES = '" + idTraslados + "' ";
                OracleDataReader reader = command.ExecuteReader();
                resultado[0] = new DataTable();
                resultado[0].Load(reader);
                //resultado[1] = consultarDetalles(idAjuste);
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

    }
}