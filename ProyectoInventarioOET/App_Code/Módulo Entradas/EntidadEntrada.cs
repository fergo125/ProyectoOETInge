using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    public class EntidadEntrada
    {
        String idFactura;
        String idEntrada;
        DateTime fechaEntrada;
        String bodega;
        String idEncargado;
        String tipoMoneda;
        String metodoPago;
        String estado;

        public EntidadEntrada(Object[] datos)
        {
            idFactura = datos[1].ToString();
            idEntrada = datos[0].ToString();
            fechaEntrada = Convert.ToDateTime(datos[4]).Date;
            Bodega = datos[3].ToString();
            idEncargado = datos[2].ToString();
            tipoMoneda = datos[5].ToString();
            metodoPago = datos[6].ToString();
            estado = datos[7].ToString();
        }
        public String IdFactura
        {
            get { return idFactura; }
            set { idFactura = value; }
        }
        public String IdEntrada
        {
            get { return idEntrada; }
            set { idEntrada = value; }
        }
        public DateTime FechEntrada
        {
            get { return fechaEntrada; }
            set { fechaEntrada = value; }
        }
        public String Bodega
        {
            get { return bodega; }
            set { bodega = value; }
        }
        public String IdEncargado
        {
            get { return idEncargado; }
            set { idEncargado = value; }
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
        public String Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }
}