using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Modulo_Ajustes
{

    // Entidad de producto que sirve para encapsular detalles de un traslado o ajuste
    public class EntidadDetalles
    {
        
        private String nombreProducto;  // Nombre de un propducto
        private String codigo;  // Codigo de un producto
        private double cambio;  // Cambio en el inventario de un producto
        private String idProductoBodega;    // Id de la bodega en que se hizo el ajuste
        private double final;               // Cantidad final en el invetario local
        private String unidades;            // Descripcion de las unidades del producto
        private String idProductoBodegaDestino; // id de la bodega destino (caso traslado)
        private String idProductoBodegaOrigen; // id de la bodega destino (caso traslado)


        /*
         * Constructor encargado de encapsular los datos de un ajuste
         */
            public EntidadDetalles (Object[] datos)
            {
                this.nombreProducto = datos[0].ToString();
                this.codigo = datos[1].ToString();
                this.cambio = Double.Parse(datos[2].ToString());
                this.idProductoBodega = datos[3].ToString();
                if( datos.Count() > 4 )
                    this.final = Double.Parse(datos[4].ToString());
            }

            /*
             * Constructor encargado de encapsular los datos de un traslado
             */
            public EntidadDetalles(Object[] datos, bool traslado)
            {
                this.nombreProducto = datos[0].ToString();
                this.codigo = datos[1].ToString();
                this.cambio = Double.Parse(datos[2].ToString());
                this.unidades = datos[3].ToString();
                this.idProductoBodegaOrigen = datos[4].ToString();
                this.idProductoBodegaDestino = datos[5].ToString();
            }
            /*
             * Método para obtener y establecer el nombre del producto.
             */
            public String NombreProducto
            {
                get { return nombreProducto; }
                set { nombreProducto = value; }
            }
            /*
             * Método para obtener y establecer el codigo del producto.
             */
            public String Codigo
            {
                get { return codigo; }
                set { codigo = value; }
            }
            /*
             * Método para obtener y establecer el cambio en el inventario (de una bodega especifica).
             */
            public double Cambio
            {
                get { return cambio; }
                set { cambio = value; }
            }

            /*
             * Método para obtener y establecer el id del producto en la bodega (Caso ajuste).
             */
            public String IdProductoBodega
            {
                get { return idProductoBodega; }
                set { idProductoBodega = value; }
            }

            /*
             * Método para obtener y establecer el id del producto en la bodega origen (Caso traslado).
             */   
            public String IdProductoBodegaOrigen
            {
                get { return idProductoBodegaOrigen; }
                set { idProductoBodegaOrigen = value; }
            }

            /*
             * Método para obtener y establecer el id del producto en la bodega destino (Caso traslado).
             */  
            public String IdProductoBodegaDestino
            {
                get { return idProductoBodegaDestino; }
                set { idProductoBodegaDestino = value; }
            }

            /*
             * Método para obtener y establecer las unidades del producto.
             */
            public String Unidades
            {
                get { return unidades; }
                set { unidades = value; }
            }



    }
}