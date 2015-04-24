using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;

/*
 * Controladora del conjunto de datos de las organizaciones anfitrionas del sistema.
 * Comunicación con la Base de Datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraBDAnfitrionas
    {
        ANFITRIONATableAdapter adaptadorAnfitriones;

        public ControladoraBDAnfitrionas()
        {
            adaptadorAnfitriones = new ANFITRIONATableAdapter();
        }

        public DataTable consultarAnfitriones()
        {
            DataTable resultado = new DataTable();
            try
            {
                resultado = adaptadorAnfitriones.GetData().CopyToDataTable();
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}