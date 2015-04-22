using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;

/*
 * Controladora del conjunto de datos de los estados del sistema.
 * Comunicación con la Base de Datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraBDEstados
    {
        CAT_ESTADOSTableAdapter adaptadorEstados;

        public ControladoraBDEstados()
        {
            adaptadorEstados = new CAT_ESTADOSTableAdapter();
        }

        public DataTable consultarEstados()
        {
            DataTable resultado = new DataTable();
            try
            {
                resultado = adaptadorEstados.GetData().CopyToDataTable();
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}