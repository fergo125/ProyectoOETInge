using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    enum TipoDePago{Credito,Contado}
    public class EntidadFactura
    {
        String idFactura;
        String idOrdenDeCompra;
        DateTime fechaPago;
        DateTime plazoDePago;
        int tipoDePago;
        String moneda;
        int tipoCambio;
        String idProvedor;
        int subtotal;
        int total;
        int descuento;
        int retencionImpuestos;
        bool detallado;

        public EntidadFactura(Object[] datos)
        {
            idFactura = datos[0].ToString();
            idOrdenDeCompra = datos[1].ToString();
            fechaPago = Convert.ToDateTime( datos[2]);
            plazoDePago = Convert.ToDateTime(datos[3]);
            tipoDePago = Convert.ToInt32( datos[4]);
            moneda = datos[5].ToString();
            tipoCambio = Convert.ToInt32( datos[6]);
            idProvedor = datos[7].ToString();
            subtotal = Convert.ToInt32( datos[8]);
            total = Convert.ToInt32( datos[9]);
            descuento = Convert.ToInt32( datos[10]);
            retencionImpuestos = Convert.ToInt32( datos[11]);
            detallado = Convert.ToBoolean(datos[12]);
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
        public DateTime PlazoDePago
        {
            get { return plazoDePago; }
            set { plazoDePago = value; }
        }
        public int TipoDePago
        {
            get { return tipoDePago; }
            set { tipoDePago = value; }
        }
        public String Moneda
        {
            get { return moneda; }
            set { moneda= value; }
        }
        public int TipoCambio
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
        public bool Detallado
        {
            get { return detallado; }
            set { detallado = value; }
        }

    }
}