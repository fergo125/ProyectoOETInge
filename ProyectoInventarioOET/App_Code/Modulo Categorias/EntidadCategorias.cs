using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Categorias
{
    public class EntidadCategoria
    {

        private String nombre;
        private String descripcion;


        public EntidadCategoria(Object[] datos)
        {
            this.nombre = autogenerarCodigo();
            this.descripcion = datos[1].ToString();
        }

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        public string autogenerarCodigo()
        {

            return DateTime.Now.ToString("h:mm:ss");
        }
    }
}