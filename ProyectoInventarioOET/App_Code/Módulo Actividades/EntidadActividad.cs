using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Módulo_Actividades
{
    /*
     * ???
     */
    public class EntidadActividad
    {
        //Atributos
        private String codigo;          // Código interno identificador de cada Actividad
        private String descripcion;     // Descripción de la actividad
        private int estado;             // Estado de la actividad: activo o inactivo.

        /*
         * Constructor.
         */
        public EntidadActividad(Object[] datos)
        {
            this.codigo = datos[0].ToString();
            this.descripcion = datos[1].ToString();
            this.estado = Convert.ToInt32(datos[2].ToString());
        }

        /*
         * Método para obtener y establecer el código de la entidad.
         */
        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        /*
         * Método para obtener y establecer la descripción de la entidad.
         */
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        /*
         * Método para obtener y establecer el estado de la entidad.
         */
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }
}