using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.App_Code.Modulo_Ajustes
{

    public class EntidadAjustes
    {

        private String idTipoAjuste; 
        private DateTime fecha;
        private String usuario; 
        private String idUsuario;
        private String notas;
        private String idBodega; 
        private List<EntidadDetalles> detalles = new List<EntidadDetalles>();

        //public EntidadAjustes(Object[] datos, DataTable datosProductos)
        public EntidadAjustes(Object[] datos)
        {
            detalles = new List<EntidadDetalles>();
            this.idTipoAjuste = datos[0].ToString(); 
            this.fecha = Convert.ToDateTime(datos[1]);
            this.usuario = datos[2].ToString(); 
            this.idUsuario = datos[3].ToString();
            this.notas = datos[4].ToString();
            if (datos.Count() >= 6)
            {
                this.idBodega = datos[5].ToString();
                //this.idUsuario = datos[6].ToString();
            }
        }

        // Decirle a leo que los nuevos productos vienen en un datatable
        public void agregarDetalle(Object[] datosProductos) {
            EntidadDetalles nuevo = new EntidadDetalles(datosProductos);
            detalles.Add(nuevo);
        }




        public String IdTipoAjuste
        {
            get { return idTipoAjuste; }
            set { idTipoAjuste = value; }
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

        public String Notas
        {
            get { return notas; }
            set { notas = value; }
        }

        public List<EntidadDetalles> Detalles
        {
            get { return detalles; }
        }

    }
}