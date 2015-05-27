using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Modulo_Ajustes
{
    public class ControladoraBDAjustes : ControladoraBD
    {

        public DataTable tiposAjuste()
        {
            String esquema1 = "Inventarios.";
            String esquema2 = "Reservas.";
            String esquema3 = "Tesoreria.";
            DataTable resultado = new DataTable();
            
            /*Modificar para recibir como parametros*/
            try 
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_TIPO_MOVIMIENTO";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }


        public DataTable consultarAjustes(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();

            try
            {
                // Interfaz ocupa 3 cosas TipoMovimiento(Descripcion), Fecha, Usuario(Encargado)
                // Yo agrego el ID de ajustes para la consulta individual
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT AJ.ID_AJUSTES, M.DESCRIPCION, AJ.FECHA, U.NOMBRE"
                   + " FROM " + esquema + "AJUSTES AJ, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_TIPO_MOVIMIENTO M"
                   + " WHERE AJ.USUARIO_BODEGA = U.SEG_USUARIO "
                   + " AND AJ.CAT_TIPO_MOVIMIENTO = M.CAT_TIPO_MOVIMIENTO"
                   + " AND AJ.IDBODEGA = '" + idBodega +"' ORDER BY AJ.FECHA DESC";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }


        public DataTable[] consultarAjuste(String idAjuste)
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
                   + " AND AJ.ID_AJUSTES = '" + idAjuste + "' ";
                OracleDataReader reader = command.ExecuteReader();
                resultado[0] = new DataTable();
                resultado[0].Load(reader);
                resultado[1] = consultarDetalles(idAjuste);
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
                command.CommandText = "SELECT P.NOMBRE, P.CODIGO, D.CAMBIO, B.INV_BODEGA_PRODUCTOS, B.SALDO, U.DESCRIPCION "
                   + " FROM " + esquema + "DETALLES_AJUSTES D, " + esquema + "INV_BODEGA_PRODUCTOS B, " + esquema + "INV_PRODUCTOS P, " + esquema + "CAT_UNIDADES U "
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


        public String[] insertarAjuste(EntidadAjustes ajuste, bool signo)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];
            res[3] = generarID();
                try
                {
                    OracleCommand command = conexionBD.CreateCommand();
                    command.CommandText = "INSERT INTO " + esquema + 
                                           "AJUSTES (ID_AJUSTES, CAT_TIPO_MOVIMIENTO, FECHA, USUARIO_BODEGA, IDBODEGA, NOTAS) VALUES ('"
                                            + res[3] + "','" + ajuste.IdTipoAjuste + "', TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',  'dd/mm/yyyy hh24:mi:ss') , '"
                                            + ajuste.IdUsuario  + "','" + ajuste.IdBodega + "' , '" + ajuste.Notas + "' )";
                    OracleDataReader reader = command.ExecuteReader();
                    
                    foreach(EntidadDetalles detallesProducto in  ajuste.Detalles  ){ // Por cada producto meterlo en el detalles ajustes
                        insertarDetalle(res[3], detallesProducto);
                        actualizarProducto(detallesProducto.IdProductoBodega, detallesProducto.Cambio, signo);
                    }

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

        private void insertarDetalle(String idAjuste, EntidadDetalles detallesProducto)
        {
            String esquema = "Inventarios.";
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "INSERT INTO " + esquema +
                                   "DETALLESAJUSTES (ID_AJUSTES, INV_BODEGA_PRODUCTOS, CAMBIO) VALUES ('"
                                    + idAjuste + "','" + detallesProducto.IdProductoBodega + "', " + detallesProducto.Cambio + " )";
            OracleDataReader reader = command.ExecuteReader();
        }

        // Hay que hablar con LEO para ver si queremos el cambio o mejor el valor que va a ser actual. FUNCIONA CORRECTAMENTE
        private void actualizarProducto(String idBodegaProducto, double cambio, bool signo)
        {
            String esquema = "Inventarios.";
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "UPDATE " + esquema + "INV_BODEGA_PRODUCTOS "
                                   + " SET SALDO = " + cambio
                                   + " WHERE INV_BODEGA_PRODUCTOS = '"+ idBodegaProducto+"'";
            OracleDataReader reader = command.ExecuteReader();
        }




        }
    }
