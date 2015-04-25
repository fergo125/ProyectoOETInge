using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Módulo_Actividades
{
    public class EntidadActividad
    {

        private String codigo;
        private String descripcion;
        private int estado;


        public EntidadActividad(Object[] datos)
        {
            this.codigo = datos[0].ToString();
            this.descripcion = datos[1].ToString();
            this.estado = Convert.ToInt32(datos[2].ToString());
        }

        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

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

        private String autogenerarCodigo()
        {
            //here's where the magic happens
            //in the meanwhile...
            return DateTime.Now.ToString("h:mm:ss");
        }

    }
}