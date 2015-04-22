using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;

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
            try
            {
                resultado = adaptadorEstaciones.GetData().CopyToDataTable();
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}