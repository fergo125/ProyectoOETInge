using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

/*
 * Controladora de base de datos de seguridad
 * Encargada de obtener información acerca de la seguridad del sistema desde la base de datos
 * Hereda de la controladora de base de datos
 */
namespace ProyectoInventarioOET.App_Code.Módulo_Seguridad
{
    public class ControladoraBDSeguridad : ControladoraBD
    {
        public ControladoraBDSeguridad() { }

        // Busca un usuario con un nombre y una password específicos
        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            DataTable resultado = new DataTable();
            EntidadUsuario usuario = null;

            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM SEG_USUARIO WHERE USUARIO = '" + nombre + "' AND CLAVE = '" + password + "'";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            if(resultado.Rows.Count == 1)
            {
                Object[] datosConsultados = new Object[9];
                for(int i=0; i<9; ++i)
                {
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                }
                usuario = new EntidadUsuario(datosConsultados);
            }

            return usuario;
        }
    }
}