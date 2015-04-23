using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

/*
 * Controladora del conjunto de datos de las estaciones del sistema.
 * Comunicación con la Base de Datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraBDEstaciones : ControladoraBD
    {
        ESTACIONTableAdapter adaptadorEstaciones;

        public ControladoraBDEstaciones()
        {
            adaptadorEstaciones = new ESTACIONTableAdapter();
        }

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