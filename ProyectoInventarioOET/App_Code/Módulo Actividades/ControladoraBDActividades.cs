using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.Modulo_Actividades
{
    /*
     * 
     * Comunicación con la Base de Datos.
     */
    class ControladoraBDActividades : ControladoraBD
    {
        /*
         * Método para obtener la información de una actividad como una EntidadActividad 
         * a partir del código de esta.
         */
        public EntidadActividad consultarActividad(String codigo)
        {
            String esquema = "Inventarios.";
            DataTable resultado = null;
            EntidadActividad actividadConsultada = null;
            Object[] datosConsultados = new Object[3];
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_ACTIVIDAD WHERE CAT_ACTIVIDAD = '" + codigo + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);

            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = codigo;
                for (int i = 1; i < 3; i++)
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                actividadConsultada = new EntidadActividad(datosConsultados);
            }
            return actividadConsultada;
        }

        /*
         * Obtiene la información de una actividad como una EntidadActividad 
         * a partir del nombre de esta.
         */
        public EntidadActividad consultarActividadPorNombre(String nombre)
        {
            String esquema = "Inventarios.";
            DataTable resultado = null;
            EntidadActividad actividadConsultada = null;
            Object[] datosConsultados = new Object[3];
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_ACTIVIDAD WHERE DESCRIPCION = '" + nombre + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);

            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = resultado.Rows[0][0].ToString();
                for (int i = 1; i < 3; i++)
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                actividadConsultada = new EntidadActividad(datosConsultados);
            }
            return actividadConsultada;
        }

        /*
         * Método para insertar una actividad en la base de datos a partir 
         * de una EntidadActividad y retornando un vector de hileras con el resultado
         * de la transacción.
         */
        public string[] insertarActividad(EntidadActividad actividad)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[4];
            actividad.Codigo = generarID();
            resultado[3] = actividad.Codigo.ToString();
            String comandoSQL = "INSERT INTO " + esquema + "CAT_ACTIVIDAD (CAT_ACTIVIDAD,DESCRIPCION,ESTADO) VALUES ('" + actividad.Codigo + "','"
                + actividad.Descripcion + "','" + (short)actividad.Estado + "')";
            if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                resultado[0] = "success";
                resultado[1] = "Éxito";
                resultado[2] = "Actividad agregada al sistema.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error";
                resultado[2] = "Actividad no agregada, intente nuevamente."; // Como la llave es generada se puede volver a intentar
            }
            return resultado;
        }

        /*
         * Modifica una actividad recibiendo la información nueva en 
         * una EntidadActividad y retornando un vector de hileras con el resultado
         * de la transacción.
         */
        public string[] modificarActividad(EntidadActividad actividad, EntidadActividad nuevaActividad)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[3];
            String comandoSQL = "UPDATE " + esquema + "CAT_ACTIVIDAD SET DESCRIPCION = '" + nuevaActividad.Descripcion + "',ESTADO = '"
                + (short)nuevaActividad.Estado + "' WHERE DESCRIPCION = '" + actividad.Descripcion + "' AND ESTADO = " + actividad.Estado;
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                resultado[0] = "success";
                resultado[1] = "Éxito";
                resultado[2] = "Actividad modificada en el sistema.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error";
                resultado[2] = "Actividad no modificada.";
            }
            return resultado;
        }

        /*
         * Retorna una estructura DataTable con la información de las actividades
         * almacenadas en la base de datos.
         */
        public DataTable consultarActividades()
        {
            String esquema = "Inventarios.";
            DataTable resultado = null;
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_ACTIVIDAD";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }
    }
}
