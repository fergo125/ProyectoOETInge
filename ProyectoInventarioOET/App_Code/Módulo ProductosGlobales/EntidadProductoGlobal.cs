using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales
{
    public class EntidadProductoGlobal
    {

        private String nombre;
        private String codigo;
        private String codigoDeBarras;
        private String categoria; // es string
        private String intencion;
        private String unidades;
        private int estado;
        private double existencia;
        private int impuesto;
        private double precioColones;
        private double precioDolares; //Calculo se hace en interfaz
        private double costoColones;
        private double costoDolares; //Calculo se hace en interfaz
        private String inv_productos; //Clave primaria es autogenerada en la BD

        public EntidadProductoGlobal (Object[] datos)
        {
            this.nombre = datos[0].ToString();
            this.codigo = datos[1].ToString();
            this.codigoDeBarras = datos[2].ToString();
            this.categoria = datos[3].ToString(); // es string
            this.intencion = datos[4].ToString();
            this.unidades = datos[5].ToString();
            this.estado = Convert.ToInt32(datos[6].ToString());
            this.existencia = Convert.ToDouble(datos[7].ToString());
            this.impuesto = Convert.ToInt32(datos[8].ToString());
            this.precioColones = Convert.ToDouble(datos[9].ToString());
            this.precioDolares = Convert.ToDouble(datos[10].ToString());
            this.costoColones = Convert.ToDouble(datos[11].ToString());
            this.costoDolares = Convert.ToDouble(datos[12].ToString());
            this.inv_productos = datos[13].ToString(); //Clave primaria es autogenerada en la BD
        }



        

        public String Nombre 
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public String CodigoDeBarras
        {
            get { return codigoDeBarras; }
            set { codigoDeBarras = value; }
        }

        public String Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public String Intencion
        {
            get { return intencion; }
            set { intencion = value; }
        }

        public String Unidades
        {
            get { return unidades; }
            set { unidades = value; }
        }


        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public double Existencia // saldo
        {
            get { return existencia; }
            set { existencia = value; }
        }

        public int Impuesto
        {
            get { return impuesto; }
            set { impuesto = value; }
        }

        public double PrecioColones
        {
            get { return precioColones; }
            set { precioColones = value; }
        }

        public double PrecioDolares
        {
            get { return precioDolares; }
            set { precioDolares = value; }
        }

        public double CostoColones
        {
            get { return costoColones; }
            set { costoColones = value; }
        }

        public double CostoDolares
        {
            get { return costoDolares; }
            set { costoDolares = value; }
        }

        public String Inv_Productos
        {
            get { return inv_productos; }
            set { inv_productos = value; }
        }

    }
}