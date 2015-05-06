using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.App_Code
{
    /*
     * Controladora del conjunto de datos de las unidades métricas del sistema.
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDUnidades : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDUnidades()
        {
        }

        /*
         * Método que retorna una tabla con la información de las unidades métricas existentes en el sistema, usadas para asociarlas
         * a los productos que son vendidos y/o consumidos.
         */
        public DataTable consultarUnidades()
        {
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM CAT_UNIDADES";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }
    }
}