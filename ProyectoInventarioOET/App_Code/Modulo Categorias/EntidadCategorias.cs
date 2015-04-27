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
        private String nombre;                  //???
        private String descripcion;             //???
        private int estado;                     //???

        /*
         * Constructor.
         */
        public EntidadCategoria(Object[] datos)
        {
            this.nombre = datos[0].ToString();
            this.descripcion = datos[1].ToString();
            this.estado = Convert.ToInt32(datos[2].ToString());
        }

        /*
         * Constructor.
         */
        public EntidadCategoria(String descripcion)
        {
            this.descripcion = descripcion;
            this.nombre = autogenerarCodigo();
            this.estado = 1;
        }

        /*
         * ???
         */
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        /*
         * ???
         */
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        //TODO: eliminar este método.
        public string autogenerarCodigo()
        {

            return DateTime.Now.ToString("h:mm:ss");
        }
    }
}