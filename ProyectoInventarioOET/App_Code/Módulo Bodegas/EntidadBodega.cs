using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Módulo_Bodegas
{
    /*
     * ???
     */
    public class EntidadBodega : ControladoraBD
    {
        //Atributos
        private String codigo;          //???
        private String nombre;          //???
        private String anfitriona;      //???
        private String estacion;        //???
        private int estado;             //???
        private int intencionUso;       //???

        /*
         * Constructor.
         */
        public EntidadBodega(Object[] datos)
        {
            this.codigo = autogenerarCodigo();
            this.nombre = datos[1].ToString();
            this.anfitriona = datos[2].ToString();
            this.estacion = datos[3].ToString();
            this.estado = Convert.ToInt32(datos[4].ToString());
            this.intencionUso = Convert.ToInt32(datos[5].ToString());
        }

        /*
         * ???
         */
        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
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
        public String Anfitriona
        {
            get { return anfitriona; }
            set { anfitriona = value; }
        }

        /*
         * ???
         */
        public String Estacion
        {
            get { return estacion; }
            set { estacion = value; }
        }

        /*
         * ???
         */
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        //TODO: eliminar este método.
        private String autogenerarCodigo()
        {
            //here's where the magic happens
            //in the meanwhile...
            return generarID();
        }

        /*
         * ???
         */
        public int IntencionUso
        {
            get { return intencionUso; }
            set { intencionUso = value; } 
        }
    }
}