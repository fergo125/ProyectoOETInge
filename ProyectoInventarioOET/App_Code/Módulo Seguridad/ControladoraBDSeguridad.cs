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


        public DataTable consultarUsuarios()
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = " SELECT u.seg_usuario, u.nombre, p.nombre as perfil, e.descripcion as Estado "
            + " FROM " + esquema + "seg_usuario U, " + esquema + "seg_perfil P, " + esquema + "seg_perfil_usuario PU, " + esquema + "cat_estados E"
            + " WHERE u.seg_usuario = PU.seg_usuario"
            + " AND PU.seg_perfil = p.seg_perfil"
            + " AND e.valor = u.estado";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        public DataTable[] consultarCuenta(String idUsuario)
        {
            String esquemaI = "Inventarios.";
            String esquemaR = "Reservas.";
            DataTable[] resultado = new DataTable[2];
            String comandoSQL = "SELECT u.seg_usuario, u.nombre, u.usuario, u.clave, e.descripcion, r.nombre, af.siglas, u.fechacreacion,  u.descripcion, u.descuento_maximo, r.id "
            + " FROM " + esquemaI + "seg_usuario U, " + esquemaI + "cat_estados E, " + esquemaR + "estacion R, " +  esquemaR + "anfitriona AF "
            + " WHERE u.seg_usuario = '" + idUsuario + "' "
            + " AND e.valor = u.estado"
            + " AND af.id = u.anfitriona"
            + " AND R.ID = u.idestacion";
            resultado[0] = ejecutarComandoSQL(comandoSQL, true);
            resultado[1] = consultarBodegasUsuario(idUsuario);
            return resultado;
        }


        private DataTable consultarBodegasUsuario(String idUsuario)
        {

            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT distinct b.cat_bodega, b.descripcion "
                                + " FROM seg_usuario_bodega UB, seg_usuario u, cat_bodega b "
                                + " where u.seg_usuario = ub.seg_usuario "
                                + " and ub.cat_bodega = b.cat_bodega "
                                + " and u.seg_usuario = '" + idUsuario + "' ";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado; 
        }

        public String[] insertarUsuario(EntidadUsuario usuario)
        {
            //String[] resultado = new String[4];
            String esquema = "Inventarios.";
            String[] resultado = new String[4];
            usuario.Codigo = generarID();
            String comandoSQL = "INSERT INTO " + esquema + "SEG_USUARIO (SEG_USUARIO, USUARIO, CLAVE,FECHACREACION,DESCRIPCION,IDESTACION,ANFITRIONA,NOMBRE, ESTADO, DESCUENTO_MAXIMO) VALUES ('"
                + usuario.Codigo + "','" + usuario.Usuario + "','" + usuario.Clave + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + usuario.Descripcion + "','" 
                + usuario.IdEstacion +"','" + usuario.Anfitriona +"','" + usuario.Nombre +"'," + usuario.Estado +",15)";
                if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                {
                    resultado[0] = "success";
                    resultado[1] = "Éxito:";
                    resultado[2] = "Cuenta agregada agregada al sistema.";
                    resultado[3] = usuario.Codigo;
                }
                else
                {
                    resultado[0] = "danger";
                    resultado[1] = "Error:";
                    resultado[2] = "Cuenta no agregada, intente nuevamente.";
                }
            return resultado;
        }

        /*
        * Modifica una cuenta dado un vector con los datos de la misma.
        */
        public String[] modificarUsuario(EntidadUsuario usuario)
        {
            //String[] resultado = new String[3];
            String esquema = "Inventarios.";
            String[] resultado = new String[4];
            usuario.Codigo = generarID();
            String comandoSQL = "UPDATE " + esquema + "SEG_USUARIO SET USUARIO = '" + usuario.Usuario + "', DESCRIPCION = '" + usuario.Descripcion + "', IDESTACION = '"
                + usuario.IdEstacion + "', ANFITRIONA = '" + usuario.Anfitriona + "', NOMBRE = '" + usuario.Nombre + "', ESTADO = " + usuario.Estado + ", DESCUENTO_MAXIMO ="
                + usuario.DescuentoMaximo + " WHERE SEG_USUARIO = '" + usuario.Codigo + "'";

            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                resultado[0] = "success";
                resultado[1] = "Éxito:";
                resultado[2] = "Cuenta modificada exitosamente.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error:";
                resultado[2] = "Cuenta no modificada, intente nuevamente.";
            }
            return resultado;
        }


        public String[] asociarABodega(String codigo, String llaveBodega, String idEstacion)
        {
            String[] resultado = new String[4];
            return resultado;
        }


        public DataTable consultarPerfiles()
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = " SELECT *"
            + " FROM " + esquema + "seg_perfil U ";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }


    }
}