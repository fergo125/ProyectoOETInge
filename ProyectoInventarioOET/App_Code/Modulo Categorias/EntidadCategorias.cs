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
        private object[] datoscategoriaNueva;
        private string p;


        public EntidadCategoria(Object[] datos)
        {
            this.nombre = datos[0].ToString();
            this.descripcion = datos[1].ToString();
        }

        public EntidadCategoria(object[] datoscategoriaNueva, string p)
        {
            // TODO: Complete member initialization
            descripcion = datoscategoriaNueva[0].ToString();
            nombre = p;

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