using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.App_Code
{
    /*
     * Controladora del conjunto de datos de las aspectos generales del sistema.
     * Tablas muy pequeñas como para implementar una controladora propia.
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDGeneral : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDGeneral()
        {
        }

        /*
         * Retorna el impuesto de venta
         */
        public int consultarImpuesto()
        {
            String esquema = "Compras.";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM " + esquema + "GEN_GENERAL WHERE ROWNUM = 1";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return Convert.ToInt32(resultado.Rows[0][0]);
        }

        /*
         * Lee de la base de datos el tipo de cambio actual, de compra y de venta, del dolar con respecto al colon
         */
        public DataTable consultarTipoCambio()
        {
            String esquema = "Reservas.";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM(SELECT COMPRA, VENTA FROM " + esquema + "TIPOCAMBIO ORDER BY ORDEN DESC) WHERE ROWNUM = 1";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }
    }
}