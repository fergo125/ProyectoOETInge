using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.Módulo_Actividades
{
    /*
     * ???
     * Comunicación con la Base de Datos.
     */
    class ControladoraBDActividades : ControladoraBD
    {
        /*
         * ???
         */
        public EntidadActividad consultarActividad(String codigo)
        {
            DataTable resultado = new DataTable();
            EntidadActividad actividadConsultada = null;
            Object[] datosConsultados = new Object[3];

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_ACTIVIDAD WHERE  CAT_ACTIVIDAD = '" + codigo + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

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
            catch (Exception e)
            {
            }
            return actividadConsultada;
        }

        /*
         * ???
         */
        public string[] insertarActividad(EntidadActividad actividad)
        {
            String[] res = new String[4];
            actividad.Codigo = generarID();
            res[3] = actividad.Codigo.ToString();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO CAT_ACTIVIDAD (CAT_ACTIVIDAD,DESCRIPCION,ESTADO) VALUES ('"
                + actividad.Codigo + "','" + actividad.Descripcion + "','"
                + (short)actividad.Estado + "')";
                OracleDataReader reader = command.ExecuteReader();


                res[0] = "success";
                res[1] = "Éxito";
                res[2] = "Actividad Agregada";
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

        /*
         * ???
         */
        public string[] modificarActividad(EntidadActividad actividad, EntidadActividad nuevaActividad)
        {
            String[] res = new String[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE CAT_ACTIVIDAD SET DESCRIPCION = '" + nuevaActividad.Descripcion 
                    + "',ESTADO = '" + (short)nuevaActividad.Estado + "' WHERE DESCRIPCION = '" + actividad.Descripcion + "' AND ESTADO = " + actividad.Estado;
                OracleDataReader reader = command.ExecuteReader();
                res[0] = "success";
                res[1] = "Éxito";
                res[2] = "Actividad modificada";
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

        /*
         * ???
         */
        public string[] desactivarActividad(EntidadActividad actividad)
        {
            String[] res = new String[3];
            try
            {

                //adaptadorBodega.Update();

                res[0] = "success";
                res[1] = "Éxito";
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

        /*
         * ???
         */
        public DataTable consultarActividades()
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_ACTIVIDAD";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}
