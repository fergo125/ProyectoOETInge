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
        private double subtotal;
        private double total;
        private int descuento;
        private int retencionImpuestos;
        private int detallado;
        private String responsable;
        private String estacion;
        private String compania;
        private DateTime fechaDeIngreso;

        public EntidadFactura(Object[] datos)
        {
            idFactura = datos[0].ToString();
            fechaPago = Convert.ToDateTime( datos[1]).Date;
            plazoDePago = Convert.ToInt32(datos[2]);
            tipoDePago = Convert.ToString( datos[3]);
            moneda = datos[4].ToString();
            tipoCambio = Convert.ToDouble( datos[5].ToString());
            idProvedor = datos[6].ToString();
            subtotal = Convert.ToDouble(datos[7]);
            total = Convert.ToDouble(datos[8]);
            descuento = Convert.ToInt32( datos[9]);
            retencionImpuestos = Convert.ToInt32( datos[10]);
            detallado = Convert.ToInt32(datos[11]);
            responsable = Convert.ToString(datos[12]);
            estacion = Convert.ToString(datos[13]);
            compania = Convert.ToString(datos[14]);
            fechaDeIngreso = Convert.ToDateTime(datos[15]);

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
        public double SubTotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }

        public double Total
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
        public string Responsable
        {
            get { return responsable; }
            set { responsable = value; }
        }
        public string Estacion
        {
            get { return estacion; }
            set { estacion = value; }
        }
        public string Compania
        {
            get { return compania; }
            set { compania = value; }
        }
        public DateTime FechaDeIngreso
        {
            get { return fechaDeIngreso; }
            set { fechaDeIngreso = value; }
        }
    }
}