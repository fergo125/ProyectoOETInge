using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;

/*
 * Controladora del conjunto de datos de las unidades métricas del sistema.
 * Comunicación con la Base de Datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraBDUnidades
    {
        CAT_UNIDADESTableAdapter adaptadorUnidades;

        public ControladoraBDUnidades()
        {
            adaptadorUnidades = new CAT_UNIDADESTableAdapter();
        }

        public DataTable consultarUnidades()
        {
            DataTable resultado = new DataTable();
            try
            {
                resultado = adaptadorUnidades.GetData().CopyToDataTable();
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}