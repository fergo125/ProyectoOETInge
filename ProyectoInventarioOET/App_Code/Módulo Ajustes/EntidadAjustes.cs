using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.App_Code.Modulo_Ajustes
{

    public class EntidadAjustes
    {

        private String idTipoAjuste; // Código del ajuste
        private DateTime fecha;     // Fecha de creación del ajuste
        private String usuario;     //Nombre del usuario responsable del ajuste
        private String idUsuario;   //iD del usuario responsable del ajuste
        private String notas;       // Notas realizadas sobre el ajuste 
        private String idBodega;    // Id de la bodega.
        private List<EntidadDetalles> detalles = new List<EntidadDetalles>(); // Lista de los productos trasladados

        /*
         * Constructor encargado de encapsular los datos de un ajuste
         */
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

        /*
         * Método encargado de encapsular y añadir un nuevo producto al ajuste
         */
        public void agregarDetalle(Object[] datosProductos) {
            EntidadDetalles nuevo = new EntidadDetalles(datosProductos);
            detalles.Add(nuevo);
        }



        /*
         * Método para obtener y establecer el id del tipo de ajuste.
         */
        public String IdTipoAjuste
        {
            get { return idTipoAjuste; }
            set { idTipoAjuste = value; }
        }
        /*
         * Método para obtener y establecer la fecha del ajuste.
         */
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        /*
         * Método para obtener y establecer el nombre del responsable del ajuste.
         */
        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        /*
         * Método para obtener y establecer el id del responsable del ajuste.
         */
        public String IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }
        /*
         * Método para obtener y establecer el id de la bodega.
         */
        public String IdBodega
        {
            get { return idBodega; }
            set { idBodega = value; }
        }

        /*
         * Método para obtener y establecer las notas sobre el ajuste.
         */
        public String Notas
        {
            get { return notas; }
            set { notas = value; }
        }

        /*
         * Método para obtener los detalles de los productos ajustados.
         */
        public List<EntidadDetalles> Detalles
        {
            get { return detalles; }
        }

    }
}