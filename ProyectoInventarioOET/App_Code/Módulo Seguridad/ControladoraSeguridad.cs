using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.App_Code.Módulo_Seguridad
{
    public class ControladoraSeguridad
    {
        private ControladoraBDSeguridad controladoraBDSeguridad;

        public ControladoraSeguridad()
        {
            controladoraBDSeguridad = new ControladoraBDSeguridad();
        }

        public EntidadUsuario consultarUsuario(String nombre, String password)
        {
            return controladoraBDSeguridad.consultarUsuario(nombre, password);
        }
    }
}