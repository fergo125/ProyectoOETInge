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
    public class EntidadFacturaVenta
    {
        //Atributos
        private String consecutivo;
        private String fechaYhora;
        private String estacion;
        private String bodega;
        private String compania;
        private String vendedor;
        private String tipoMoneda;
        private String metodoPago;
        private String cliente;
        private String actividad;
        private String estado;
        private double montoTotalColones;
        private double montoTotalDolares;
        private DataTable productos;


        /*
         * Constructor de la clase, recibe datos iniciales para crear una instancia de factura.
         */
        public EntidadFacturaVenta(Object[] datos)
        {
            this.consecutivo = datos[0].ToString();
            this.fechaYhora = datos[1].ToString();
            this.estacion = datos[2].ToString();
            this.bodega = datos[3].ToString();
            this.compania = datos[4].ToString();
            this.vendedor = datos[5].ToString();
            this.tipoMoneda = datos[6].ToString();
            this.metodoPago = datos[7].ToString();
            this.cliente = datos[8].ToString();
            this.actividad = datos[9].ToString();
            this.estado = datos[10].ToString().ToString();
            this.montoTotalColones = Convert.ToDouble(datos[11].ToString());
            this.montoTotalDolares = Convert.ToDouble(datos[12].ToString());
            this.productos = null; //(DataTable)datos[13];
        }

        public String Consecutivo
        {
            get { return consecutivo; }
            set { consecutivo = value; }
        }


        public String FechaHora
        {
            get { return fechaYhora; }
            set { fechaYhora = value; }
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

        public String Compania
        {
            get { return compania; }
            set { compania = value; }
        }

        public String Vendedor
        {
            get { return vendedor; }
            set { vendedor = value; }
        }

        public String TipoMoneda
        {
            get { return tipoMoneda; }
            set { tipoMoneda = value; }
        }

        public String MetodoPago
        {
            get { return metodoPago; }
            set { metodoPago = value; }
        }

        public String Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public String Actividad
        {
            get { return actividad; }
            set { actividad = value; }
        }

        public String Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public double MontoTotalColones
        {
            get { return montoTotalColones; }
            set { montoTotalColones = value; }
        }

        public double MontoTotalDolares
        {
            get { return montoTotalDolares; }
            set { montoTotalDolares = value; }
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