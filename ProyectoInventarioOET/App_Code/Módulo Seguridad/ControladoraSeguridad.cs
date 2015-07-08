using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.Modulo_Seguridad
{
    /*
     * Controladora de Seguridad, encargada de las funciones de seguridad del sistema, también encripta/desencripta contraseñas.
     */
    public class ControladoraSeguridad : Controladora
    {
        //Atributos
        private ControladoraBDSeguridad controladoraBDSeguridad;    // Entidad de controladora de base de datos, usada para acceder a base de datos
        private String llaveEncriptacion = "SISTEOTS";              // Contraseña con la que se encripta un String

        /*
         * Constructor.
         */
        public ControladoraSeguridad()
        {
            controladoraBDSeguridad = new ControladoraBDSeguridad();
            controladoraBDSeguridad.NombreUsuarioLogueado = (this.NombreUsuarioLogueado);
        }

        /*
         * Busca un usuario con un nombre y una password específicos.
         */
        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            password = encriptarTexto(password);
            return controladoraBDSeguridad.consultarUsuario(nombre, password);
        }

        /*
         * Modifica la contraseña de un usuario en especifico
         */
        public String[] modificarContrasena(String codigoInternoUsuario, String password)
        {
            password = encriptarTexto(password);
            return controladoraBDSeguridad.modificarContrasena(codigoInternoUsuario,password);
        }

        public String consultarPermisosUsuario(String codigoPerfil, String interfaz)
        {
            return controladoraBDSeguridad.consultarPermisosUsuario(codigoPerfil, interfaz);
        }

        /*
         * Encripta un string en el formato de almacenamiento de la base de datos.
         * No funcional
         */
        public String encriptarTexto(String texto)
        {
            String codigo = "";
            String nrletrapass = "";
            int valorAscii = 0;
            for (int i = 1; i <= texto.Length; ++i)
            {
                nrletrapass = (Convert.ToInt32((llaveEncriptacion.Substring(((i % (llaveEncriptacion.Length))), 1).ToCharArray()[0]))).ToString();
                valorAscii = (Convert.ToInt32((texto.Substring(i - 1, 1).ToCharArray()[0])));
                valorAscii = Convert.ToInt32(valorAscii ^ (Convert.ToInt32(nrletrapass) + 2));
                codigo += ((char)valorAscii);
            }
            codigo = codigo.Replace("'", "$#6@$");
            return codigo;
        }

        /*
         * Obtiene el nombre de un usuario (texto, no llave) de un id de usuario en específico.
         */
        public String consultarNombreDeUsuario(String idUsuario)
        {
            return controladoraBDSeguridad.consultarNombreDeUsuario(idUsuario);
        }

        public String consultarNombreDeBodega(String id)
        {
            return controladoraBDSeguridad.consultarNombreDeBodega(id);
        }

        public String consultarNombreDeEstacion(String id)
        {
            return controladoraBDSeguridad.consultarNombreDeEstacion(id);
        }

        /*
         * Consulta un perfil con base en su nombre, si existe lo retorna, de lo contrario retorna null (útil al desear verificar la existencia del perfil).
         */
        public EntidadPerfil consultarPerfil(String nombre)
        {
            return controladoraBDSeguridad.consultarPerfil(nombre);
        }

        /*
         * Consulta un perfil con base en su nombre, si existe lo retorna, de lo contrario retorna null (útil al desear verificar la existencia del perfil).
         */
        public String[] insertarPerfil(String nombre, int nivel, String[] permisos)
        {
            return controladoraBDSeguridad.insertarPerfil(nombre, nivel, permisos);
        }

        /*
         * Modifica un perfil con base en su nombre
         */
        public String[] modificarPerfil( String nombreViejo, EntidadPerfil nuevo )
        {
            return controladoraBDSeguridad.modificarPerfil(nombreViejo, nuevo);
        }


        // Retorna si un String es una contraseña valida
        public bool contrasenaEsValida(String pass)
        {
            bool result = pass.Length>=8,mayuscula=false, minuscula=false, numero=false;
            if (result)
            {
                CharEnumerator recorrido = pass.GetEnumerator();
                char a = 'a', z = 'z', am = 'A', zm = 'Z', zero = '0', nueve = '9';
                while (recorrido.MoveNext())
                {
                    minuscula |= (recorrido.Current >= a && recorrido.Current <= z);
                    mayuscula |= (recorrido.Current >= am && recorrido.Current <= zm);
                    numero |= (recorrido.Current >= zero && recorrido.Current <= nueve);
                }
                result = minuscula && mayuscula && numero;
            }
            return result;
        }

        /*
        * Método encargado de consultar una cuenta específica y además 
         * encapsula los datos para una mejor manipulación en la interfaz
        */
        public EntidadUsuario consultarCuenta(String idUsuario)
        {
            DataTable[] cuenta = controladoraBDSeguridad.consultarCuenta(idUsuario); // en cuenta[1] van las bodegas
            String [] perfil = controladoraBDSeguridad.consultarPerfilUsuario(idUsuario); 
            EntidadUsuario consultada = new EntidadUsuario(cuenta[0], cuenta[1]);
            consultada.Perfil = perfil[0];
            return consultada;
        }

        /*
        * Crea una nueva cuenta dado un vector con los datos de la misma.
        */
        public String[] insertarUsuario(Object[] datosUsuario)
        {
            EntidadUsuario usuario = new EntidadUsuario(datosUsuario);
            return controladoraBDSeguridad.insertarUsuario(usuario);
        }

        /*
        * Modifica una cuenta dado un vector con los datos de la misma.
        */
        public String[] modificarUsuario(Object[] datosUsuario, List<String> listadoBodegas, String perfil)
        {
            EntidadUsuario usuario = new EntidadUsuario(datosUsuario);
            return controladoraBDSeguridad.modificarUsuario(usuario, listadoBodegas, perfil);
        }

        /*
        * Método encargado de consultar las cuentas de los usuarios del sistema.
        */
        public DataTable consultarUsuarios()
        {
            return controladoraBDSeguridad.consultarUsuarios();
        }

        public DataTable consultarPerfiles() {
            return controladoraBDSeguridad.consultarPerfiles();
        }

        public String[] asociarABodega(String codigo, String llaveBodega, String idEstacion)
        {
            return controladoraBDSeguridad.asociarABodega(codigo, llaveBodega, idEstacion);
        }

        public String[] asociarPerfilNuevoUsuario(String llaveUsuario, String llavePerfil) 
        {
            return controladoraBDSeguridad.asociarPerfilNuevoUsuario(llaveUsuario, llavePerfil);
        }

        public Boolean nombreUsuarioRepetido(String nombreUsuario)
        {
            return controladoraBDSeguridad.nombreUsuarioRepetido(nombreUsuario);        
        }
    
    }
}