using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{

    public class EntidadAjustes
    {


        private class DetalleAjuste
        {
            private String idAjuste;
            private String idProductoBodega;
            private double cambio;

            public DetalleAjuste(Object[] datos)
            {
                this.idAjuste = datos[0].ToString();
                this.idProductoBodega = datos[1].ToString();
                this.cambio = Double.Parse(datos[2].ToString());
            }

            public String IdAjuste
            {
                get { return idAjuste; }
                set { idAjuste = value; }
            }


            public String IdProductoBodega
            {
                get { return idProductoBodega; }
                set { idProductoBodega = value; }
            }

            public double Cambio
            {
                get { return cambio; }
                set { cambio = value; }
            }


        }   //Final de clase


        private String idAjuste;
        private String idTipoAjuste;
        private DateTime fecha; //Ver entidad producto global
        private String usuario;
        private List<DetalleAjuste> detalles =  new List<DetalleAjuste>(); 

        public EntidadAjustes (Object[] datos)
        {
            detalles =  new List<DetalleAjuste>();
            this.idAjuste = datos[0].ToString();
            this.idTipoAjuste = datos[1].ToString();
            if (datos.Count() > 2)
            {
                this.usuario = datos[2].ToString();
                this.fecha = Convert.ToDateTime(datos[3].ToString());
            }

        }


        public void agregarDetalle(Object[] datos) {
            DetalleAjuste nuevo = new DetalleAjuste(datos);
            detalles.Add(nuevo);
        }




    }
}