using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.App_Code.Modulo_Ajustes;

namespace ProyectoInventarioOET.App_Code.Modulo_Traslados
{
    public class EntidadTraslado
    {
        private String idTraslado; 
        private DateTime fecha;
        private String usuario; 
        private String idUsuario;
        private String notas;
        private String idBodegaOrigen;
        private String idBodegaDestino;
        private String bodegaOrigen;
        private String bodegaDestino; 

        private List<EntidadDetalles> detalles = new List<EntidadDetalles>();

        public EntidadTraslado(Object[] datos)
        {
            this.idTraslado = datos[0].ToString(); 
            this.fecha = Convert.ToDateTime(datos[1]);
            this.usuario = datos[2].ToString(); 
            this.idUsuario = datos[3].ToString();
            this.notas = datos[4].ToString();
            this.idBodegaOrigen = datos[5].ToString();
            this.idBodegaDestino = datos[6].ToString();
            this.bodegaOrigen = datos[5].ToString();
            this.bodegaDestino = datos[6].ToString();
            detalles = new List<EntidadDetalles>();
        }


        public void agregarDetalle(Object[] datosProductos) {
            EntidadDetalles nuevo = new EntidadDetalles(datosProductos);
            detalles.Add(nuevo);
        }


        public String IdTraslado
        {
            get { return idTraslado; }
            set { idTraslado = value; }
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

        public String Notas
        {
            get { return notas; }
            set { notas = value; }
        }

        public String IdBodegaOrigen
        {
            get { return idBodegaOrigen; }
            set { idBodegaOrigen = value; }
        }

        public String IdBodegaDestino
        {
            get { return idBodegaDestino; }
            set { idBodegaDestino = value; }
        }

        public String BodegaOrigen
        {
            get { return bodegaOrigen; }
            set { bodegaOrigen = value; }
        }

        public String BodegaDestino
        {
            get { return bodegaDestino; }
            set { bodegaDestino = value; }
        }


        public List<EntidadDetalles> Detalles
        {
            get { return detalles; }
        }



    }
}