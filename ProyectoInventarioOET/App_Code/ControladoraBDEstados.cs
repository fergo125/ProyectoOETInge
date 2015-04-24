using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

/*
 * Controladora del conjunto de datos de los estados del sistema.
 * Comunicación con la Base de Datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraBDEstados : ControladoraBD
    {
        CAT_ESTADOSTableAdapter adaptadorEstados;

        public ControladoraBDEstados()
        {
            adaptadorEstados = new CAT_ESTADOSTableAdapter();
        }

        public DataTable consultarEstados()
        {
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM CAT_ESTADOS";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }
    }
}