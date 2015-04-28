using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.Módulo_Seguridad
{
    /*
     * Controladora de Seguridad, encargada de las funciones de seguridad del sistema, también encripta/desencripta contraseñas.
     */
    public class ControladoraSeguridad
    {
        //Atributos
        private ControladoraBDSeguridad controladoraBDSeguridad;    //???
        private String llaveEncriptacion = "SISTEOTS";              //???

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
            // Encriptar password aqui***
            return controladoraBDSeguridad.consultarUsuario(nombre, password);
        }

        public String consultarPermisosUsuario(String codigoPerfil, String interfaz)
        {
            return controladoraBDSeguridad.consultarPermisosUsuario(codigoPerfil, interfaz);
        }

        /*
         * Encripta un string en el formato de almacenamiento de la base de datos.
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
    }
}