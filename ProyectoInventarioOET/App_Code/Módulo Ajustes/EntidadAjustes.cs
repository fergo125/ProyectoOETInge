using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{

    public class EntidadAjustes
    {

        private class DetalleAjuste
        {
            private String idAjuste;
            private String idProductoBodega;
            private double cambio;

            public DetalleAjuste(String idAjuste, String idProductoBodega, double cambio)
            {
                this.idAjuste = idAjuste;
                this.idProductoBodega = idProductoBodega;
                this.cambio = cambio;
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

        public EntidadAjustes(Object[] datos, DataTable datosProductos)
        {
            detalles =  new List<DetalleAjuste>();
            this.idAjuste = datos[0].ToString();
            this.idTipoAjuste = datos[1].ToString();
            if (datos.Count() > 2)
            {
                this.usuario = datos[2].ToString();
                this.fecha = Convert.ToDateTime(datos[3].ToString());
            }
            agregarDetalle(datosProductos);

        }

        // Decirle a leo que los nuevos productos vienen en un datatable
        public void agregarDetalle(DataTable datosProductos) {
            DetalleAjuste nuevo;
            foreach (DataRow row in datosProductos.Rows) {
                nuevo = new DetalleAjuste(row[0].ToString(), row[1].ToString(), Double.Parse(row[2].ToString()));
                detalles.Add(nuevo);
            }
        }


        public String IdAjuste
        {
            get { return idAjuste; }
            set { idAjuste = value; }
        }


        public String IdTipoAjuste
        {
            get { return IdTipoAjuste; }
            set { IdTipoAjuste = value; }
        }

        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }



    }
}