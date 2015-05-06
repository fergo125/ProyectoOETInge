using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.App_Code
{
    /*
     * Controladora del conjunto de datos de las estaciones del sistema.
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDEstaciones : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDEstaciones()
        {
        }

        /*
         * Método que retorna una tabla con la información de las estaciones de la OET.
         */
        public DataTable consultarEstaciones()
        {
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM ESTACION";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }
    }
}