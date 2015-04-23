using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Módulo_Actividades
{
    class ControladoraBDActividades
    {
        public EntidadActividad consultarActividad(String codigo)
        {
            DataTable resultado = new DataTable();
            EntidadActividad actividadConsultada = null;
            Object[] datosConsultados = new Object[3];

            try
            {
                //resultado = adaptadorBodega.consultarFilaBodega(codigo);

                if (resultado.Rows.Count == 1)
                {
                    datosConsultados[0] = codigo;
                    for (int i = 1; i < 3; i++)
                    {
                        datosConsultados[i] = resultado.Rows[0][i].ToString();
                    }

                    actividadConsultada = new EntidadActividad(datosConsultados);
                }
            }
            catch (Exception e) { }

            return actividadConsultada;
        }

        public string[] insertarActividad(EntidadActividad actividad)
        {
            String[] res = new String[4];
            res[3] = actividad.Codigo.ToString();
            try
            {
                //  adaptadorBodega.Insert();
                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega Agregada";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Fallo en la operacion";
                res[2] = "Intente nuevamente";
            }
            return res;
        }

        public string[] modificarActividad(EntidadActividad actividadVieja, EntidadActividad actividadNueva)
        {
            String[] res = new String[3];
            try
            {
                //adaptadorBodega.Update();
                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega modificado";
            }
            catch (SqlException e)
            {
                if (e.Number == 2627)
                {
                    res[0] = "danger";
                    res[1] = "Fallo";
                    res[2] = "Error al modificar";
                }
            }
            return res;
        }

        public string[] desactivarActividad(EntidadActividad actividad)
        {
            String[] res = new String[3];
            try
            {

                //adaptadorBodega.Update();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega eliminado";
            }
            catch (SqlException e)
            {
                res[1] = "danger";
                res[2] = "Fallo";
                res[3] = "Error al eliminar";
            }
            return res;
        }

        public DataTable consultarActividades()
        {
            DataTable resultado = new DataTable();

            try
            {
                //resultado = adaptadorBodega.GetData().CopyToDataTable();
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}
