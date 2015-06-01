using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Productos_Locales
{
    /*
     * ???
     */
    public class EntidadProductoLocal
    {
        //Atributos
        private String nombre;              //???
        private String categoria;           //String ???
        private String unidades;            //???
        private String codigo;              //???
        private String codigoDeBarras;      //???
        private String estacion;            //???
        private int estado;                 //???
        private double costoColones;        //???
        private double costoDolares;        //???, cuyo cálculo se hace en interfaz
        private String inv_productos;       //Llave primaria autogenerada en la BD

        /*
         * Constructor.
         */
        public EntidadProductoLocal(Object[] datos)
        {
            this.nombre = datos[0].ToString();
            this.categoria = datos[1].ToString();
            this.unidades = datos[2].ToString();
            this.codigo = datos[3].ToString(); // PREGUNTAR SI VA A SER AUTOGENERADO
            this.codigoDeBarras = datos[4].ToString(); ;
            this.estacion = datos[5].ToString();
            this.estado = Convert.ToInt32(datos[6].ToString());
            this.costoColones = Convert.ToDouble(datos[7].ToString());
            this.costoDolares = Convert.ToDouble(datos[8].ToString());
        }

        /*
         * ???
         */
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        /*
         * ???
         */
        public String Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        /*
         * ???
         */
        public String Unidades
        {
            get { return unidades; }
            set { unidades = value; }
        }

        /*
         * ???
         */
        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        /*
         * ???
         */
        public String CodigoDeBarras
        {
            get { return codigoDeBarras; }
            set { codigoDeBarras = value; }
        }

        /*
         * ???
         */
        public String Estacion
        {
            get { return estacion; }
            set { estacion = value; }
        }

        /*
         * ???
         */
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        /*
         * ???
         */
        public double CostoColones
        {
            get { return costoColones; }
            set { costoColones = value; }
        }

        /*
         * ???
         */
        public double CostoDolares
        {
            get { return costoDolares; }
            set { costoDolares = value; }
        }

        /*
         * ???
         */
        public String Inv_Productos
        {
            get { return inv_productos; }
            set { inv_productos = value; }
        }

        //TODO: eliminar este método.
        private String autogenerarCodigo()
        {
            //here's where the magic happens
            //in the meanwhile...
            return "";
        }
    }
}