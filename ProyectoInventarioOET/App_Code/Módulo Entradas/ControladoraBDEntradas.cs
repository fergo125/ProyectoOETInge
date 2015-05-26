using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    public class ControladoraBDEntradas : ControladoraBD
    {
                /*
         * Constructor.
         */
        public ControladoraBDEntradas()
        {
        }
        
        public DataTable consultarEntradasDeBodega(string bodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM " + esquema + "CAT_ENTRADAS WHERE CAT_BODEGA = '" + bodega + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        public DataTable consultarFacturas()
        {
            String esquema = "Compras.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM " + esquema + "FACTURAS";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        public DataTable buscarFacturas(string id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Compras.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                if ("Todas".Equals(id))
                {                    
                    command.CommandText = "SELECT * FROM " + esquema + "FACTURAS";
                    OracleDataReader reader = command.ExecuteReader();
                    resultado.Load(reader);

                }
                else
                {
                    command.CommandText = "SELECT *  "
                    + " FROM " + esquema + "FACTURAS "
                    + " WHERE IDFACTURA LIKE " + " '" + id + "%'";
                    OracleDataReader reader = command.ExecuteReader();
                    resultado.Load(reader);                                
                }

            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}