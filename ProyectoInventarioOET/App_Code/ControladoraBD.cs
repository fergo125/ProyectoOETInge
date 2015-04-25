﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET
{
    /*
     * Clase no instanciable usada sólo para heredar el código necesario para conectarse a la base de datos.
     */
    public abstract class ControladoraBD
    {
        //Atributos
        protected static OracleConnection conexionBD; //atributo estático compartido por todas las ControladorasBD para conectarse
        protected static int consecutivo;

        //Constructor, crea y abre la conexión sólo la primera vez que es necesario.
        public ControladoraBD()
        {
            if (conexionBD == null)
            {
                conexionBD = new OracleConnection();
                conexionBD.ConnectionString = "Data Source=10.1.4.93;User ID=grupo02;Password=blacjof7"; //en el futuro se podría leer esta string desde un archivo
                conexionBD.Open();
            }
        }

        //Destructor, cierra la conexión cuando se termina la ejecución general de la sesión.
        ~ControladoraBD()
        {
            //conexionBD.Close();
        }

        protected String generarID()
        {
            consecutivo = (consecutivo + 1) % 999;
            return "" + DateTime.Now.Day.ToString("D2") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString("D3") + consecutivo.ToString("D3");
        }
    }
}