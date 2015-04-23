using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.App_Code.Módulo_Seguridad
{
    public class ControladoraBDSeguridad : ControladoraBD
    {
        public ControladoraBDSeguridad() { }

        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            DataTable resultado = new DataTable();
            EntidadUsuario usuario = null;

            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT * FROM SEG_USUARIO WHERE USUARIO = '" + nombre + "' AND CLAVE = '" + password + "'";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            if( resultado.Rows.Count == 1 )
            {
                Object[] datosConsultados = new Object[9];
                for(int i = 0; i < 9; ++i)
                {
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                }
                usuario = new EntidadUsuario(datosConsultados);
            }

            return usuario;
        }
    }
}