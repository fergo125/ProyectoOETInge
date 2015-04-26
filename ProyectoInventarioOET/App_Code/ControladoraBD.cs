using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET
{
    /*
     * Clase no instanciable usada sólo para heredar el código necesario para conectarse a la base de datos y para generar
     * el ID de las tuplas de la base de datos.
     */
    public abstract class ControladoraBD
    {
        //Atributos
        protected static OracleConnection conexionBD;   //atributo estático compartido por todas las ControladorasBD para conectarse
        protected static int consecutivo;               //???

        /*
         * Constructor, crea y abre la conexión sólo la primera vez que es necesario.
         */
        public ControladoraBD()
        {
            if (conexionBD == null)
            {
                conexionBD = new OracleConnection();
                conexionBD.ConnectionString = "Data Source=10.1.4.93;User ID=grupo02;Password=blacjof7"; //en el futuro se podría leer esta string desde un archivo
                conexionBD.Open();
            }
        }

        /*
         * Destructor, cierra la conexión cuando se termina la ejecución general de la sesión.
         */
        ~ControladoraBD()
        {
            //conexionBD.Close(); //por ahora no sirve de nada intentar cerrarla aquí porque los destructores no son invocados
        }

        /*
         * Método usado para generar el ID de las tuplas en la base de datos a partir de la fecha y hora del momento exacto,
         * siguiendo la convención de la OET con la única diferencia de omitir el nombre del servidor que crea este ID.
         */
        protected String generarID()
        {
            consecutivo = (consecutivo + 1) % 999;
            return ""
            + DateTime.Now.Day.ToString("D2")
            + DateTime.Now.Month.ToString("D2")
            + DateTime.Now.Year.ToString()
            + DateTime.Now.Hour.ToString("D2")
            + DateTime.Now.Minute.ToString("D2")
            + DateTime.Now.Second.ToString("D2")
            + DateTime.Now.Millisecond.ToString("D3")
            + consecutivo.ToString("D3");
        }
    }
}