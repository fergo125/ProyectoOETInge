using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.Modulo_Ajustes;

namespace ProyectoInventarioOET.Modulo_Traslados
{
    public class EntidadTraslado
    {
        private String idTraslado;  //Código del traslado 
        private DateTime fecha;     // Fecha de creación del traslado
        private String usuario;     // Nombre del usuario responsable
        private String idUsuario;   // Id del usuario responsable
        private String notas;       // Notas del traslado
        private String idBodegaOrigen;  // Id de la bodega origen 
        private String idBodegaDestino; // Id de la bodega destino
        private String bodegaOrigen;    //Nombre de la bodega origen
        private String bodegaDestino;   //Nombre de la bodega destino
        private String estado; //Es String para poder consultar mas facil

        private List<EntidadDetalles> detalles = new List<EntidadDetalles>(); // Lista de los productos trasladados

        /*
         * Constructor encargado de encapsular los datos de un traslado
         */
        public EntidadTraslado(Object[] datos)
        {
            this.idTraslado = datos[0].ToString(); 
            this.fecha = Convert.ToDateTime(datos[1]);
            this.usuario = datos[2].ToString(); 
            this.idUsuario = datos[3].ToString();
            this.notas = datos[4].ToString();
            this.idBodegaOrigen = datos[5].ToString();
            this.idBodegaDestino = datos[6].ToString();
            this.bodegaOrigen = datos[7].ToString();
            this.bodegaDestino = datos[8].ToString();
            this.estado = datos[9].ToString();
            detalles = new List<EntidadDetalles>();
        }

        /*
         * Método para agregar el detalle del traslado de un producto a la entidad.
         */
        public void agregarDetalle(Object[] datosProductos) {
            EntidadDetalles nuevo = new EntidadDetalles(datosProductos, true);
            detalles.Add(nuevo);
        }

        /*
         * Método para obtener y establecer el id del traslado.
         */
        public String IdTraslado
        {
            get { return idTraslado; }
            set { idTraslado = value; }
        }
        /*
         * Método para obtener y establecer la fecha de la entidad.
         */
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        /*
         * Método para obtener y establecer el nombre del usuario responsable del traslado de la entidad.
         */
        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        /*
         * Método para obtener y establecer el id del usuario responsable del traslado de la entidad.
         */
        public String IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }
        /*
         * Método para obtener y establecer las notas de la entidad.
         */
        public String Notas
        {
            get { return notas; }
            set { notas = value; }
        }
        /*
         * Método para obtener y establecer el id de la bodega origen en la entidad.
         */
        public String IdBodegaOrigen
        {
            get { return idBodegaOrigen; }
            set { idBodegaOrigen = value; }
        }
        /*
         * Método para obtener y establecer el id de la bodega destino de la entidad.
         */
        public String IdBodegaDestino
        {
            get { return idBodegaDestino; }
            set { idBodegaDestino = value; }
        }
        /*
         * Método para obtener y establecer el nombre de la bodega origen en la entidad.
         */
        public String BodegaOrigen
        {
            get { return bodegaOrigen; }
            set { bodegaOrigen = value; }
        }
        /*
         * Método para obtener y establecer el nombre de la bodega de destino en la entidad.
         */
        public String BodegaDestino
        {
            get { return bodegaDestino; }
            set { bodegaDestino = value; }
        }
        /*
         * Método para obtener y establecer el estado la entidad.
         */
        public String Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        /*
         * Método para obtener los detalles (productos trasladados) de la entidad.
         */
        public List<EntidadDetalles> Detalles
        {
            get { return detalles; }
        }



    }
}