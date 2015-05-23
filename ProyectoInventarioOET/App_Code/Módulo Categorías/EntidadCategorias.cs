using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Categorias
{
    /*
     * ???
     */
    public class EntidadCategoria
    {
        //Atributos
        private String nombre;                  //Identificador de la categoria en la base de datos
        private String descripcion;             //El nombre propiamente de la categoria
        private int estado;                     //Indica si la categoria esta activa o no en la base de datos

        /*
         * Recibe todos los datos para crear una entidad de una categoria nueva
         */
        public EntidadCategoria(Object[] datos)
        {
            this.nombre = datos[0].ToString();
            this.descripcion = datos[1].ToString();
            this.estado = Convert.ToInt32(datos[2].ToString());
        }

        /*
         * Recibe unicamente una descripcion, se usa unicamente para cuando se va a agregar una categoria nueva
         */
        public EntidadCategoria(String descripcion)
        {
            this.descripcion = descripcion;
            this.nombre = autogenerarCodigo();
            this.estado = 1;
        }
        public EntidadCategoria(String descripcion, String estado)
        {
            this.descripcion = descripcion;
            this.nombre = autogenerarCodigo();
            this.estado = Convert.ToInt32(estado);
        }

        /*
         * Método para obtener y establecer el nombre de la entidad.
         */
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        /*
         * Método para obtener y establecer la descripcion de la entidad.
         */
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        /*
         * Método para obtener y establecer el estde la entidad.
         */
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        
        /*
         * Método para generar un nuevo codigo para una categoria nueva.
         */
        public string autogenerarCodigo()
        {

            return DateTime.Now.ToString("h:mm:ss");
        }
    }
}