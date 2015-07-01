using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Seguridad
{
    /*
     * Entidad Perfil, clase encargada de encapsulador la información acerca de perfiles de permisos.
     */
    public class EntidadPerfil
    {
        //Atributos
        private String nombre;              // Nombre, único en la base de datos
        private int nivel;                  // Código secundario, usado en la base de datos para definir nivel de autoridad
        private String[] permisos;          // Permisos que tiene el perfil sobre las distintas interfaces

        /*
         * Constructor de la clase
         * Este método toma datos y los encapsula en la Entidad Perfil
         */
        public EntidadPerfil(Object[] datos)
        {
            this.nombre = datos[1].ToString();
            this.nivel = Convert.ToInt32(datos[2].ToString());
            this.permisos = (String[])datos[2];
        }

        /*
         * Métodos de acceso a datos
         * Permiten obtener o manipular los atributos encapsulados en Entidad Perfil
         * Cada método se refiere al atributo del mismo nombre
         */
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public int Nivel
        {
            get { return nivel; }
            set { nivel = value; }
        }

        public String[] Permisos
        {
            get { return permisos; }
            set { permisos = value; }
        }
    }
}