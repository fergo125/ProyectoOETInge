using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.Módulo_Seguridad
{
    /*
     * Controladora de base de datos de seguridad, encargada de obtener información acerca de la seguridad
     * del sistema desde la base de datos. Hereda de la controladora de base de datos
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDSeguridad : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDSeguridad()
        {
        }

        /*
         * Busca un usuario con un nombre y un password específicos.
         */
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
            usuario.Perfil = consultarPerfilUsuario(usuario.Codigo);

            return usuario;
        }

        /*
         * Obtiene el perfil (en texto, no llave) que tiene asignado un usuario usando su código interno.
         * TODO: obtener la llave también por si se llega a necesitar.
         */
        public String consultarPerfilUsuario(String codigoUsuario)
        {
            DataTable resultado = new DataTable();

            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT NOMBRE FROM SEG_PERFIL WHERE (SEG_PERFIL = (SELECT SEG_PERFIL FROM SEG_PERFIL_USUARIO WHERE SEG_USUARIO = '" + codigoUsuario + "'))";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            if(resultado.Rows.Count == 1)
            {
                return resultado.Rows[0][0].ToString();
            }
            return null;
        }

        /*
         * Obtiene el nombre de un usuario (texto, no llave) de un id de usuario en específico.
         */
        public String consultarNombreDeUsuario(String idUsuario)
        {
            String nombre = "";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT NOMBRE FROM SEG_USUARIO WHERE SEG_USUARIO = '"+idUsuario+"'";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            if (resultado.Rows.Count == 1)
            {
                nombre = resultado.Rows[0][0].ToString();
            }
            return nombre;
        }

    }
}