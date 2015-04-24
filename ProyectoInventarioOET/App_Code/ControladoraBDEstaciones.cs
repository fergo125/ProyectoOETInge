using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;
using Oracle.DataAccess.Client;

/*
 * Controladora del conjunto de datos de las estaciones del sistema.
 * Comunicación con la Base de Datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraBDEstaciones
    {
        ESTACIONTableAdapter adaptadorEstaciones;

        public ControladoraBDEstaciones()
        {
            adaptadorEstaciones = new ESTACIONTableAdapter();
        }

        public DataTable consultarEstaciones()
        {
            DataTable resultado = new DataTable();
            //try
            //{
            //    resultado = adaptadorEstaciones.GetData().CopyToDataTable();
            //}
            //catch (Exception e)
            //{
            //    resultado = null;
            //}
            //return resultado;

            //Crea la conexión, con la string de conexión, y la abre
            OracleConnection myConnection = new OracleConnection();
            myConnection.ConnectionString = "Data Source=10.1.4.93;User ID=grupo02;Password=blacjof7";
            myConnection.Open();
            //ejecutar operaciones
            OracleCommand command = myConnection.CreateCommand();
            string operacion = "SELECT ID,NOMBRE,SIGLAS,RESERVABLE FROM ESTACION";
            command.CommandText = operacion;
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            //cerrar conexión
            myConnection.Close();

            return resultado;
        }
    }
}