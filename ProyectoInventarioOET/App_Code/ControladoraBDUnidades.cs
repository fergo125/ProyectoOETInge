using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

/*
 * Controladora del conjunto de datos de las unidades métricas del sistema.
 * Comunicación con la Base de Datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraBDUnidades : ControladoraBD
    {
        CAT_UNIDADESTableAdapter adaptadorUnidades;

        public ControladoraBDUnidades()
        {
            adaptadorUnidades = new CAT_UNIDADESTableAdapter();
        }

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