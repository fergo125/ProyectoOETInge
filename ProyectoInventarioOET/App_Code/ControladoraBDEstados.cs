using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.App_Code
{
    /*
     * Controladora del conjunto de datos de los estados del sistema.
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDEstados : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDEstados()
        {
        }

        /*
         * Método que retorna una tabla con la información de los posibles estados para las entidades dentro del sistema.
         */
        public DataTable consultarEstados()
        {
            String esquema = "Tesoreria.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_ESTADOS";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }
        /*
         * Método que retorna una tabla con la información de los posibles estados para las entidades dentro del sistema.
         */
        public DataTable consultarEstadosAnular()
        {
            String esquema = "Tesoreria.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_ESTADOS WHERE CAT_ESTADO > 1 AND CAT_ESTADO < 4";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Método que retorna una tabla con la información de un subgrupo de los posibles estados para las entidades dentro del sistema,
         * por ahora sólo "Activo" e "Inactivo".
         */
        public DataTable consultarEstadosActividad()
        {
            String esquema = "Tesoreria.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_ESTADOS WHERE VALOR < 2";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }
    }
}