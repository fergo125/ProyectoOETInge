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
        private String consecutivo;
        private String fecha;
        private String bodega;
        private String estacion;
        private String compañia;
        private String actividad;
        private String vendedor;
        private String cliente;
        private String tipoMoneda;
        private int impuesto;
        private String metodoPago;
        private DataTable productos;
        private double montoTotal;
        private String estado;


        /*
         * Constructor de la clase, recibe datos iniciales para crear una instancia de factura.
         */
        public EntidadFactura(Object[] datos)
        {
            this.consecutivo = datos[0].ToString();
            this.fecha = datos[1].ToString();
            this.bodega = datos[2].ToString();
            this.estacion = datos[3].ToString();
            this.compañia = datos[4].ToString();
            this.actividad = datos[5].ToString();
            this.vendedor = datos[6].ToString();
            this.cliente = datos[7].ToString();
            this.tipoMoneda = datos[8].ToString();
            this.metodoPago = datos[9].ToString();
            this.montoTotal = Convert.ToDouble(datos[10].ToString());
            this.estado = datos[11].ToString().ToString();
            this.productos = null; //(DataTable)datos[7];
        }

        public String Consecutivo
        {
            get { return consecutivo; }
            set { consecutivo = value; }
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

        public String Bodega
        {
            get { return bodega; }
            set { bodega = value; }
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

        public double MontoTotal
        {
            get { return montoTotal; }
            set { montoTotal = value; }
        }

        public String Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        /*
         * Métodos de acceso a datos (setters y getters).
         * Permiten obtener o manipular los atributos, cada método se refiere al atributo del mismo nombre.
         */
    }
}