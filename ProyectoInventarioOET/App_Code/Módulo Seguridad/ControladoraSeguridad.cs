using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/*
 * Controladora de Seguridad
 * Encargada de las funciones de seguridad del sistema
 * También encripta/desencripta contraseñas
 */
namespace ProyectoInventarioOET.App_Code.Módulo_Seguridad
{
    public class ControladoraSeguridad
    {
        private ControladoraBDSeguridad controladoraBDSeguridad;

        public ControladoraSeguridad()
        {
            controladoraBDSeguridad = new ControladoraBDSeguridad();
        }

        // Busca un usuario con un nombre y una password específicos
        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            return controladoraBDSeguridad.consultarUsuario(nombre, password);
        }
    }
}