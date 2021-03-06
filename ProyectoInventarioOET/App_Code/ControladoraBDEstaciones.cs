﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET
{
    /*
     * Controladora del conjunto de datos de las estaciones del sistema.
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDEstaciones : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDEstaciones()
        {
        }

        /*
         * Método que retorna una tabla con la información de las estaciones de la OET.
         */
        public DataTable consultarEstaciones()
        {
            String esquema = "Reservas.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "ESTACION";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Método que retorna una tabla con la información de una estación dada una de sus bodegas.
         */
        public DataTable consultarEstacionDeBodega(String idBodega) //TODO: implementar esta consulta
        {
            String esquema1 = "Reservas.";
            String esquema2 = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT NOMBRE,ID FROM " + esquema1 + "ESTACION WHERE ID=(SELECT " + esquema2 + "CAT_BODEGA.ESTACION FROM " + esquema2 + "CAT_BODEGA WHERE CAT_BODEGA='" + idBodega + "')";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }
    }
}