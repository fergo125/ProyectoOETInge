﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/*
 * Controladora de Seguridad
 * Encargada de las funciones de seguridad del sistema
 * También encripta/desencripta contraseñas
 */
namespace ProyectoInventarioOET.Módulo_Seguridad
{
    public class ControladoraSeguridad
    {
        private ControladoraBDSeguridad controladoraBDSeguridad;
        private String llaveEncriptacion = "SISTEOTS";

        public ControladoraSeguridad()
        {
            controladoraBDSeguridad = new ControladoraBDSeguridad();
        }

        // Busca un usuario con un nombre y una password específicos
        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            // Probablemente encripta aqui
            return controladoraBDSeguridad.consultarUsuario(nombre, password);
        }

        // Encripta un string en el formato de almacenamiento de la base de datos
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

        // Desencripta un string en el formato de almacenamiento de la base de datos
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
    }
}