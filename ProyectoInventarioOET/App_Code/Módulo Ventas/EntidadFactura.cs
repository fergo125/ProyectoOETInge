using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Ventas
{
    /*
     * Entidad Factura, clase encargada de encapsulador la información acerca de las facturas asociadas a transacciones de venta.
     */
    public class EntidadFactura
    {
        //Atributos

        private String fecha; 
        private String estacion;
        private String compañia;
        private String actividad;
        private String vendedor;
        private String cliente;
        private String tipoMoneda;
        private int impuesto;
        private String metodoPago;
        private DataTable productos;


        /*
         * Constructor de la clase, recibe datos iniciales para crear una instancia de factura.
         */
        public EntidadFactura(Object[] datos)
        {
            this.fecha = DateTime.Now.ToString("dd:MMM:yy");
            this.estacion = datos[0].ToString();
            this.compañia = datos[1].ToString();
            this.actividad = datos[2].ToString();
            this.vendedor = datos[3].ToString();
            this.cliente = datos[4].ToString();
            this.tipoMoneda = datos[5].ToString();
            this.impuesto = Convert.ToInt32(datos[6].ToString());
            this.metodoPago = datos[7].ToString();
            this.productos = (DataTable) datos[8];

        }


        public String Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public String Estacion
        {
            get { return estacion; }
            set { estacion = value; }
        }

        public String Compañia
        {
            get { return compañia; }
            set { compañia = value; }
        }

        public String Actividad
        {
            get { return actividad; }
            set { actividad = value; }
        }

        public String Vendedor
        {
            get { return vendedor; }
            set { vendedor = value; }
        }

        public String Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public String TipoMoneda
        {
            get { return tipoMoneda; }
            set { tipoMoneda = value; }
        }

        public int Impuesto
        {
            get { return impuesto; }
            set { impuesto = value; }
        }

        public String MetodoPago
        {
            get { return metodoPago; }
            set { metodoPago = value; }
        }

        public DataTable Productos
        {
            get { return productos; }
            set { productos = value; }
        }

        /*
         * Métodos de acceso a datos (setters y getters).
         * Permiten obtener o manipular los atributos, cada método se refiere al atributo del mismo nombre.
         */
    }
}