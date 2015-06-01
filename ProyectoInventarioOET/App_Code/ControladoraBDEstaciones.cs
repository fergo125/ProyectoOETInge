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
            String esquema = "Reservas.";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM " + esquema + "ESTACION";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }

        /*
         * Método que retorna una tabla con la información de una estación dada una de sus bodegas.
         */
        public DataTable consultarEstacionDeBodega(String idBodega) //TODO: implementar esta consulta
        {
            String esquema1 = "Reservas.";
            String esquema2 = "Inventarios.";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            //command.CommandText = "SELECT * FROM " + esquema1 + "ESTACION";
            //seleccionar el nombre y la llave de la estación (esquema1) cuya llave coincide con la de la estación a la que pertenece la bodega (esquema 2)
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }
    }
}