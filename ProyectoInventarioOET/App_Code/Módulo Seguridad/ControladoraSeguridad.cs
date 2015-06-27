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
    public class ControladoraSeguridad
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
        }

        /*
         * Busca un usuario con un nombre y una password específicos.
         */
        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            //TODO: Encriptar password aqui***
            return controladoraBDSeguridad.consultarUsuario(nombre, password);
        }

        /*
         * Modifica la contraseña de un usuario en especifico
         */
        public String[] modificarContrasena(String codigoInternoUsuario, String password)
        {
            //TODO: Encriptar password aqui**
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
        public String encriptarTexto(String s)
        {
            String resultado = "";
            for( int i = 0; i < s.Length; ++i )
            {
                int letra = llaveEncriptacion[i % llaveEncriptacion.Length];
                letra = (char)((int)s[i] ^ (letra + 2));
                resultado += letra;
            }
            resultado.Replace("'", "$#6@$");
            return resultado;
        }

        /*
         * Desencripta un string en el formato de almacenamiento de la base de datos.
         * No funcional
         */
        public String desencriptarText(String s)
        {
            String resultado = "";
            s.Replace("$#6@$", "'");
            for (int i = 0; i < s.Length; ++i)
            {
                int letra = llaveEncriptacion[i % llaveEncriptacion.Length];
                letra = (char)( ((int)s[i] ^ letra) + 2);
                resultado += letra;
            }
            return resultado;
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

    }
}