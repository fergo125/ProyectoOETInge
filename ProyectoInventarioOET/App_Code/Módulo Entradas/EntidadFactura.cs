using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    enum TipoDePago{Credito,Contado}
    public class EntidadFactura
    {
        private String idFactura;
        private String idOrdenDeCompra;
        private DateTime fechaPago;
        private int plazoDePago;
        private String tipoDePago;
        private String moneda;
        private double tipoCambio;
        private String idProvedor;
        private int subtotal;
        private int total;
        private int descuento;
        private int retencionImpuestos;
        private int detallado;

        public EntidadFactura(Object[] datos)
        {
            idFactura = datos[0].ToString();
            idOrdenDeCompra = datos[1].ToString();
            fechaPago = Convert.ToDateTime( datos[2]).Date;
            plazoDePago = Convert.ToInt32(datos[3]);
            tipoDePago = Convert.ToString( datos[4]);
            moneda = datos[5].ToString();
            tipoCambio = Convert.ToDouble( datos[6].ToString());
            idProvedor = datos[7].ToString();
            subtotal = Convert.ToInt32( datos[8]);
            total = Convert.ToInt32( datos[9]);
            descuento = Convert.ToInt32( datos[10]);
            retencionImpuestos = Convert.ToInt32( datos[11]);
            detallado = Convert.ToInt32(datos[12]);
        }

        public String IdFactura
        {
            get { return idFactura; }
            set { idFactura = value; }
        }

        public String IdOrdenDeCompra
        {
            get { return idOrdenDeCompra; }
            set { idOrdenDeCompra = value; }
        }

        public DateTime FechaPago
        {
            get { return fechaPago; }
            set { fechaPago = value; }
        }

        public int PlazoDePago
        {
            get { return plazoDePago; }
            set { plazoDePago = value; }
        }
        public String TipoDePago
        {
            get { return tipoDePago; }
            set { tipoDePago = value; }
        }
        public String Moneda
        {
            get { return moneda; }
            set { moneda= value; }
        }
        public double TipoCambio
        {
            get { return tipoCambio; }
            set { tipoCambio = value; }
        }
        public int SubTotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }

        public int Total
        {
            get { return total; }
            set { total = value; }
        }
        public int Descuento
        {
            get { return descuento; }
            set { descuento = value; }
        }
        public int RetencionImpuestos
        {
            get { return retencionImpuestos; }
            set { retencionImpuestos = value; }
        }
        public int Detallado
        {
            get { return detallado; }
            set { detallado = value; }
        }

    }
}