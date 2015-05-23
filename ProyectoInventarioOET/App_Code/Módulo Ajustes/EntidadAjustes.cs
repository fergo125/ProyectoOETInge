using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{

    public class EntidadAjustes
    {

        private String idTipoAjuste; 
        private DateTime fecha;
        private String usuario; 
        private String idUsuario;
        private String idBodega;  //
        private List<EntidadDetalles> detalles = new List<EntidadDetalles>();

        //public EntidadAjustes(Object[] datos, DataTable datosProductos)
        public EntidadAjustes(Object[] datos)
        {
            detalles = new List<EntidadDetalles>();
            this.idTipoAjuste = datos[0].ToString(); 
            this.fecha = Convert.ToDateTime(datos[1]);
            this.usuario = datos[2].ToString(); 
            this.idUsuario = datos[3].ToString();
            this.idBodega =  datos[4].ToString();  //
            if (datos.Count() > 3)
            {
                this.usuario = datos[3].ToString();
                this.fecha = Convert.ToDateTime(datos[4].ToString());
            }
            //agregarDetalle(datosProductos);

        }

        // Decirle a leo que los nuevos productos vienen en un datatable
        public void agregarDetalle(DataTable datosProductos) {
            EntidadDetalles nuevo;
            foreach (DataRow row in datosProductos.Rows) {
                nuevo = new EntidadDetalles(row[0].ToString(), row[1].ToString(), Double.Parse(row[2].ToString()));
                detalles.Add(nuevo);
            }
        }




        public String IdTipoAjuste
        {
            get { return IdTipoAjuste; }
            set { IdTipoAjuste = value; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public String IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public String IdBodega
        {
            get { return idBodega; }
            set { idBodega = value; }
        }



        public List<EntidadDetalles> Detalles
        {
            get { return detalles; }
        }

    }
}