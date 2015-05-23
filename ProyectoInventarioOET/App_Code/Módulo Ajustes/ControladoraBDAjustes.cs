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
                command.CommandText = "SELECT * FROM tIPO";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }
        }
    }
