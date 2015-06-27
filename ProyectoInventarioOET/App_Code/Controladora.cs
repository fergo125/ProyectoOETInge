using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET
{
    /*
     * Clase no instanciable usada sólo para heredar el código que todas las controladoras tienen en común.
     */
    public abstract class Controladora
    {
        //Atributos
        protected static String nombreUsuarioLogueado;  // Usado para insertar en la tabla de réplica

        /*
         * Constructor, crea y abre la conexión sólo la primera vez que es necesario.
         */
        public Controladora()
        {
        }

        /*
         * Setter y getter de nombreUsuarioLogueado.
         */
        public String NombreUsuarioLogueado
        {
            get { return nombreUsuarioLogueado; }
            set { nombreUsuarioLogueado = value; }
        }
    }
}