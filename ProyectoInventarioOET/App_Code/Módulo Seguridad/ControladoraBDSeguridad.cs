using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.Modulo_Seguridad
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
         * Utilizado para inicio de sesión.
         */
        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            EntidadUsuario usuario = null;

            // Comandos de SQL para acceder a la base de datos
            String comandoSQL = "SELECT * FROM " + esquema + "SEG_USUARIO WHERE USUARIO = '" + nombre + "' AND CLAVE = '" + password + "'";
            // Importante, modificar para que solo use cuentas activas
            resultado = ejecutarComandoSQL(comandoSQL, true);
            // Si encuentro una única cuenta
            if(resultado.Rows.Count == 1)
            {
                Object[] datosConsultados = new Object[9];
                for(int i=0; i<9; ++i)
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                usuario = new EntidadUsuario(datosConsultados);
                String[] perfil = consultarPerfilUsuario(usuario.Codigo);
                usuario.Perfil = perfil[0];
                usuario.LlavePerfil = perfil[1];
                usuario.CodigoPerfil = perfil[2];
            }
            return usuario;
        }
        /*
         * Modifica la contraseña de un usuario en especifico basado en su codigo interno de la base de datos
         */
        public String[] modificarContrasena(String codigoInternoUsuario, String password)
        {
            String esquema = "Inventarios.";
            String[] mensaje = new String[3];

            String comandoSQL = "UPDATE "+esquema+"SEG_USUARIO SET CLAVE = '"+password+"' WHERE SEG_USUARIO = '"+codigoInternoUsuario+"'";
            ejecutarComandoSQL(comandoSQL,false);

            mensaje[0] = "success";
            mensaje[1] = "Éxito";
            mensaje[2] = "Modificación de contraseña realizada con éxito";

            return mensaje;
        }

        /*
         * Dado un código de perfil y el nombre de una interfaz, cargo los permisos de ese usuario en esa interfaz
         */
        public String consultarPermisosUsuario(String codigoPerfil, String interfaz)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT PERMISOS FROM " + esquema + "SEG_PERMISOS WHERE SEG_PERFIL = '" + codigoPerfil + "' AND INTERFAZ = '" + interfaz + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                return resultado.Rows[0][0].ToString();
            return null;
        }


        /*
         * Obtiene el perfil (en texto, no llave) que tiene asignado un usuario usando su código interno.
         */
        public String[] consultarPerfilUsuario(String codigoUsuario)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT NOMBRE, SEG_PERFIL, CODIGO FROM " + esquema + "SEG_PERFIL WHERE (SEG_PERFIL = (SELECT SEG_PERFIL FROM SEG_PERFIL_USUARIO WHERE SEG_USUARIO = '" + codigoUsuario + "'))";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if(resultado.Rows.Count == 1)
            {
                String[] res = new String[3];
                res[0] = resultado.Rows[0][0].ToString();
                res[1] = resultado.Rows[0][1].ToString();
                res[2] = resultado.Rows[0][2].ToString();
                return res;
            }
            return null;
        }

        /*
         * Obtiene el nombre de un usuario (texto, no llave) de un id de usuario en específico.
         */
        public String consultarNombreDeUsuario(String idUsuario)
        {
            String esquema = "Inventarios.";
            String nombre = "";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT NOMBRE FROM " + esquema + "SEG_USUARIO WHERE SEG_USUARIO = '" + idUsuario + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                nombre = resultado.Rows[0][0].ToString();
            return nombre;
        }

        /*
         * ???
         */
        public String consultarNombreDeBodega(String id)
        {
            String esquema = "Inventarios.";
            String nombre = "";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT DESCRIPCION FROM " + esquema + "CAT_BODEGA WHERE CAT_BODEGA = '" + id + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                nombre = resultado.Rows[0][0].ToString();
            return nombre;
        }

        /*
         * ???
         */
        public String consultarNombreDeEstacion(String id)
        {
            String esquema = "Reservas.";
            String nombre = "";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT NOMBRE FROM " + esquema + "ESTACION WHERE ID ='" + id + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                nombre = resultado.Rows[0][0].ToString();
            return nombre;
        }
    }
}