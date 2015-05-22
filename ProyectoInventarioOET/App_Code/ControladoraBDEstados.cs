using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.App_Code
{
    /*
     * Controladora del conjunto de datos de los estados del sistema.
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDEstados : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDEstados()
        {
        }

        /*
         * Método que retorna una tabla con la información de los posibles estados para las entidades dentro del sistema.
         */
        public DataTable consultarEstados()
        {
            String esquema = "Tesoreria.";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM " + esquema + "CAT_ESTADOS";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }

        /*
         * Método que retorna una tabla con la información de un subgrupo de los posibles estados para las entidades dentro del sistema,
         * por ahora sólo "Activo" e "Inactivo".
         */
        public DataTable consultarEstadosActividad()
        {
            String esquema = "Tesoreria.";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM " + esquema + "CAT_ESTADOS WHERE VALOR < 2";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }
    }
}