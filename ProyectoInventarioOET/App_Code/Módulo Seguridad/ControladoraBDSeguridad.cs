﻿using System;
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
                Object[] datosConsultados = new Object[10];
                for(int i=0; i<10; ++i)
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
         * Consultar un perfil con base en su nombre.
         */
        public EntidadPerfil consultarPerfil(String nombre)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            EntidadPerfil perfil = null;
            String comandoSQL = "SELECT * FROM " + esquema + "SEG_PERFIL WHERE NOMBRE = '" + nombre + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
            {
                Object[] datosConsultados = new Object[3];
                datosConsultados[0] = resultado.Rows[0][1];
                datosConsultados[1] = resultado.Rows[0][3];

                String idPerfil = resultado.Rows[0][0].ToString();
                comandoSQL = "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Catalogo general de productos' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Categorias de productos' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Catalogos de productos en bodegas' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Gestion de bodegas' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Gestion de actividades' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Entradas de inventario' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Ajustes de inventario' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Traslados de inventario' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Facturacion' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Reportes' UNION ALL "
                    + "SELECT PERMISOS FROM SEG_PERMISOS WHERE SEG_PERFIL = '" + idPerfil + "' AND INTERFAZ = 'Seguridad'";
                resultado = ejecutarComandoSQL(comandoSQL, true);

                String[] permisos = new String[11];
                for (int i = 0; i < 11; ++i )
                {
                    permisos[i] = resultado.Rows[i][0].ToString();
                }
                datosConsultados[2] = permisos;
                perfil = new EntidadPerfil(datosConsultados);
            }
            return perfil;
        }

        /*
         * Asigna un perfil al usuario recien creado.
         */
        public String[] asociarPerfilNuevoUsuario(String llaveUsuario, String llavePerfil)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[3];
            String id = generarID();
            String comandoSQL = "INSERT INTO " + esquema + "SEG_PERFIL_USUARIO VALUES ('"
                + id + "', '"
                + llaveUsuario + "','"
                + llavePerfil + "')";
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                    resultado[0] = "success";
                    resultado[1] = "Éxito";
                    resultado[2] = "Perfil creado con éxito";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error";
                resultado[2] = "Error al intentar crear el nuevo perfil.";
            }
            return resultado;
        }



        /*
         * Insertar un perfil.
         */
        public String[] insertarPerfil(String nombre, int nivel, String[] permisos)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[3];
            String id = generarID();
            String comandoSQL = "INSERT INTO " + esquema + "SEG_PERFIL VALUES ('"
                + id + "', '"
                + nombre + "', "
                + "1, '"
                + nivel + "')";
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                if (insertarPermisosPerfil(permisos, id))
                {
                    resultado[0] = "success";
                    resultado[1] = "Éxito";
                    resultado[2] = "Perfil creado con éxito";
                }
                else
                {
                    resultado[0] = "danger";
                    resultado[1] = "Error";
                    resultado[2] = "Error al intentar crear el nuevo perfil.";
                }
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error";
                resultado[2] = "Error al intentar crear el nuevo perfil.";
            }
            return resultado;
        }

        /*
         * Insertar los permisos del perfil.
         */
        public bool insertarPermisosPerfil(String[] permisos, String llavePerfil)
        {
            String esquema = "Inventarios.";
            String tuplas = ""; //se agrega cada string de permisos en una inserción por aparte para luego unirlas en un sólo query
            for (short i = 1; i <= 11; ++i)
            {
                tuplas += " INTO " + esquema + "SEG_PERMISOS VALUES('"
                    + generarID() + "', '"
                    + llavePerfil + "', '";
                switch(i)
                {
                    case 1:  tuplas += "Catalogo general de productos";     break;
                    case 2:  tuplas += "Categorias de productos";           break;
                    case 3:  tuplas += "Catalogos de productos en bodegas"; break;
                    case 4:  tuplas += "Gestion de bodegas";                break;
                    case 5:  tuplas += "Gestion de actividades";            break;
                    case 6:  tuplas += "Entradas de inventario";            break;
                    case 7:  tuplas += "Traslados de inventario";           break;
                    case 8:  tuplas += "Ajustes de inventario";             break;
                    case 9:  tuplas += "Facturacion";                       break;
                    case 10: tuplas += "Reportes";                          break;
                    case 11: tuplas += "Seguridad";                         break;
                    default: break;
                }
                tuplas += "', '" + permisos[i-1].ToString() + "')";
            }

            String comandoSQL = "INSERT ALL" + tuplas + " SELECT * FROM DUAL"; //query unificado
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                return true;
            return false;
        }

        /*
         * Modifica un perfil en la base de datos, y los permisos asociados a este
         */
        public String [] modificarPerfil( String nombreViejo, EntidadPerfil nuevo )
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[3];
            String id = generarID();
            String comandoSQL = "UPDATE " + esquema + "SEG_PERFIL "
                + "SET NOMBRE = '" + nuevo.Nombre
                + "', CODIGO = " + nuevo.Nivel
                + " WHERE NOMBRE = '" + nombreViejo + "'";
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                comandoSQL = "SELECT SEG_PERFIL FROM " + esquema + "SEG_PERFIL WHERE NOMBRE = '" + nuevo.Nombre + "'";
                DataTable temp = ejecutarComandoSQL(comandoSQL, true);
                String codigo = temp.Rows[0][0].ToString();
                bool continuar = true;

                for (short i = 1; i <= 11 && continuar; ++i)
                {
                    comandoSQL = "UPDATE SEG_PERMISOS SET PERMISOS = '" + nuevo.Permisos[i-1] + "' "
                        + "WHERE SEG_PERFIL = '" + codigo + "' "
                        + "AND INTERFAZ = '";
                    switch (i)
                    {
                        case 1: comandoSQL += "Catalogo general de productos"; break;
                        case 2: comandoSQL += "Categorias de productos"; break;
                        case 3: comandoSQL += "Catalogos de productos en bodegas"; break;
                        case 4: comandoSQL += "Gestion de bodegas"; break;
                        case 5: comandoSQL += "Gestion de actividades"; break;
                        case 6: comandoSQL += "Entradas de inventario"; break;
                        case 7: comandoSQL += "Traslados de inventario"; break;
                        case 8: comandoSQL += "Ajustes de inventario"; break;
                        case 9: comandoSQL += "Facturacion"; break;
                        case 10: comandoSQL += "Reportes"; break;
                        case 11: comandoSQL += "Seguridad"; break;
                        default: break;
                    }
                    comandoSQL += "'";
                    continuar = (ejecutarComandoSQL(comandoSQL, false) != null);

                }

                if (continuar)
                {
                    resultado[0] = "success";
                    resultado[1] = "Éxito";
                    resultado[2] = "Perfil modificado con éxito";
                }
                else
                {
                    resultado[0] = "danger";
                    resultado[1] = "Error";
                    resultado[2] = "Error al intentar modificar el perfil.";
                }
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error";
                resultado[2] = "Error al intentar modificar el perfil.";
            }
            return resultado;
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

        /*
         * Método que retorna todas las cuentas en el sistema con informacion preliminar
        */ 
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

        /*
         * Método que retorna toda la información disponible sobre la cuenta de un usuario específico. 
        */ 
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

        /*
         * Método auxiliar que permite encontrar las bodegas en las que un usuario esta asociado 
         * mediante su id
         */ 

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
            String date = DateTime.Now.ToString("dd-MMM-yyyy");
            usuario.FechaCreacion = Convert.ToDateTime(date);
            String comandoSQL = "INSERT INTO " + esquema + "SEG_USUARIO (SEG_USUARIO, USUARIO, CLAVE,FECHACREACION,DESCRIPCION,IDESTACION,ANFITRIONA,NOMBRE, ESTADO, DESCUENTO_MAXIMO) VALUES ('"
                + usuario.Codigo + "','" + usuario.Usuario + "','" + usuario.Clave + "','" + date + "','" + usuario.Descripcion + "','" 
                + usuario.IdEstacion +"','" + usuario.Anfitriona +"','" + usuario.Nombre +"'," + usuario.Estado +",15)";
                if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                {
                    resultado[0] = "success";
                    resultado[1] = "Éxito:";
                    resultado[2] = "Cuenta agregada al sistema.";
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
        public String[] modificarUsuario(EntidadUsuario usuario, List<String> listadoBodegas, String perfil)
        {
            //String[] resultado = new String[3];
            String idPerfil = consultarPerfilPorNombre(perfil);
            String esquema = "Inventarios.";
            String[] resultado = new String[4];
            Boolean exito = true;
            String comandoSQL = "UPDATE " + esquema + "SEG_USUARIO SET USUARIO = '" + usuario.Usuario + "', DESCRIPCION = '" + usuario.Descripcion + "', IDESTACION = '"
                + usuario.IdEstacion + "', ANFITRIONA = '" + usuario.Anfitriona + "', NOMBRE = '" + usuario.Nombre + "', ESTADO = " + usuario.Estado + ", DESCUENTO_MAXIMO = "
                + usuario.DescuentoMaximo + " WHERE SEG_USUARIO = '" + usuario.Codigo + "'";
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                comandoSQL = "UPDATE " + esquema + "SEG_PERFIL_USUARIO SET SEG_PERFIL = '" + idPerfil + "' WHERE SEG_USUARIO = '" + usuario.Codigo + "'";
                if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                {
                    comandoSQL = "DELETE FROM " + esquema + "SEG_USUARIO_BODEGA WHERE SEG_USUARIO = '" + usuario.Codigo + "'";
                    if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                    {
                        foreach (String bodega in listadoBodegas)
                        {
                            comandoSQL = "INSERT INTO " + esquema + "SEG_USUARIO_BODEGA (SEG_USUARIO_BODEGA, SEG_USUARIO, CAT_BODEGA, ESTACION) VALUES ('"
                            + generarID() + "','" + usuario.Codigo + "','" + bodega + "','" + usuario.IdEstacion + "')";
                            if (ejecutarComandoSQL(comandoSQL, false) == null) //si no sale bien
                                exito = false;
                        }
                    }
                }
                else
                {
                    exito = false;
                }            
            
            }




            if (exito)
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
            String esquema = "Inventarios.";
            String[] resultado = new String[3];
            String comandoSQL = "INSERT INTO " + esquema + "SEG_USUARIO_BODEGA (SEG_USUARIO_BODEGA, SEG_USUARIO, CAT_BODEGA, ESTACION) VALUES ('"
                +generarID() + "','" + codigo + "','" + llaveBodega + "','" + idEstacion + "')";
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                resultado[0] = "success";
                resultado[1] = "Éxito:";
                resultado[2] = "Cuenta agregada al sistema y asociado a las bodegas seleccionadas.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error:";
                resultado[2] = "Cuenta no agregada, intente nuevamente.";
            }
            return resultado;
        }


        public DataTable consultarPerfiles()
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = " SELECT *"
            + " FROM " + esquema + "seg_perfil U WHERE U.ESTADO = 1 ";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Consulta el identificador de un perfil a partir de su nombre 
         * 
         */
        public String consultarPerfilPorNombre(String nombrePerfil)
        {
            String esquema = "Inventarios.";
            String idPerfil = "";
            DataTable consultado = new DataTable();
            String comandoSQL = " SELECT SEG_PERFIL"
            + " FROM " + esquema + "seg_perfil WHERE NOMBRE = '" + nombrePerfil + "'";

            consultado = ejecutarComandoSQL(comandoSQL, true);
            if (consultado.Rows.Count > 0)
            {
                foreach (DataRow fila in consultado.Rows)
                {
                    idPerfil = fila[0].ToString();
                }
            }

            return idPerfil;
        }

        public Boolean nombreUsuarioRepetido(String nombreUsuario)
        {
            Boolean repetido = false;
            String esquema = "Inventarios.";
            String comandoSQL = " SELECT SEG_USUARIO"
            + " FROM " + esquema + "SEG_USUARIO WHERE USUARIO = '" + nombreUsuario + "'";
            DataTable consultado = new DataTable();
            consultado = ejecutarComandoSQL(comandoSQL, true);
            
            if (consultado.Rows.Count > 0)
            {
                repetido = true;
            }

            return repetido;
        }

    }
}