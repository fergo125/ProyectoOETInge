using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
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


        public DataTable consultarAjustes()
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
                   + "AND AJ.CAT_TIPO_MOVIMIENTO = M.CAT_TIPO_MOVIMIENTO";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }

        public String[] insertarAjuste(EntidadAjustes ajuste)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];
            res[3] = generarID();
                try
                {
                    OracleCommand command = conexionBD.CreateCommand();
                    command.CommandText = "INSERT INTO " + esquema + 
                                           "AJUSTE (idAjuste, tipoAjuste, fecha, usuario) VALUES ('"
                                            + res[3] + "','" + ajuste.IdTipoAjuste + "','" + ajuste.Fecha + "','"
                                            + ajuste.Usuario + "' )";
                    OracleDataReader reader = command.ExecuteReader();
                    
                    foreach(EntidadDetalles detallesProducto in  ajuste.Detalles  ){ // Por cada producto meterlo en el detalles ajustes
                        insertarDetalle(res[3], detallesProducto);
                        //Hacer el cambio
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

        private void insertarDetalle(string idAjuste, EntidadDetalles detallesProducto)
        {
            String esquema = "Inventarios.";
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "INSERT INTO " + esquema +
                                   "DETALLESAJUSTES (ID_AJUSTES, INV_BODEGA_PRODUCTOS, CAMBIO) VALUES ('"
                                    + idAjuste + "','" + detallesProducto.IdProductoBodega + "', " + detallesProducto.Cambio + " )";
            OracleDataReader reader = command.ExecuteReader();
        }


        }
    }
