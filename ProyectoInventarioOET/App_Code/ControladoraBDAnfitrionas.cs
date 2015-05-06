﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.App_Code
{
    /*
     * Controladora del conjunto de datos de las compañías anfitrionas del sistema.
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDAnfitrionas : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDAnfitrionas()
        {
        }

        /*
         * Método que retorna una tabla con la información de las compañías anfitrionas en el sistema.
         * Por ahora son sólo la OET o la ESINTRO.
         */
        public DataTable consultarAnfitriones()
        {
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM ANFITRIONA";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return resultado;
        }
    }
}