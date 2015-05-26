using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Modulo_ProductosGlobales
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
        
        private String estacion; //Desde la cual fue creado el producto
        // Seguridad
        private String usuario;
        private DateTime fecha;


        /*
         * Constructor.
         * Encapsula los datos ingresados del usuario asociandolos a su respectivo atributo en la entidad
         */
        public EntidadProductoGlobal (Object[] datos)
        {
            this.codigo = datos[0].ToString();
            this.codigoDeBarras = datos[1].ToString();
            this.nombre = datos[2].ToString();
            this.costoColones = datos[3].ToString()!=""?Convert.ToDouble(datos[3].ToString()):0;
            this.categoria = datos[4].ToString(); // es string
            this.unidades = datos[5].ToString();
            this.existencia = datos[6].ToString()!=""?Convert.ToDouble(datos[6].ToString()):0;
            this.estado = datos[7].ToString()!=""?Convert.ToInt32(datos[7].ToString()):0;
            this.costoDolares = datos[8].ToString() != "" ? Convert.ToDouble(datos[8].ToString()) : 0;
            this.impuesto = datos[9].ToString()!=""?Convert.ToInt32(datos[9].ToString()):0;
            this.intencion = datos[10].ToString();
            this.precioColones = datos[11].ToString()!=""?Convert.ToDouble(datos[11].ToString()):0;
            this.precioDolares = datos[12].ToString() != "" ? Convert.ToDouble(datos[12].ToString()) : 0;
            this.inv_productos = datos[13].ToString(); //Clave primaria es autogenerada en la BD
            if( datos.Count() > 14 )
            {
                this.usuario = datos[14].ToString();
                this.fecha = Convert.ToDateTime(datos[15].ToString());
            }
        }

        /*
        * Método para obtener y establecer el nombre de la entidad.
        */
        public String Nombre 
        {
            get { return nombre; }
            set { nombre = value; }
        }

        /*
        * Método para obtener y establecer el código de la entidad.
        */
        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        /*
        * Método para obtener y establecer el código de barras de la entidad.
        */
        public String CodigoDeBarras
        {
            get { return codigoDeBarras; }
            set { codigoDeBarras = value; }
        }
        /*
        * Método para obtener y establecer el id de la categoría de la entidad.
        */
        public String Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }
        /*
        * Método para obtener y establecer la intención de la entidad.
        */
        public String Intencion
        {
            get { return intencion; }
            set { intencion = value; }
        }
        /*
        * Método para obtener y establecer el id de las unidades de la entidad.
        */
        public String Unidades
        {
            get { return unidades; }
            set { unidades = value; }
        }
        /*
        * Método para obtener y establecer el estado de la entidad.
        */
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        /*
        * Método para obtener y establecer la existencia/saldo de la entidad.
        */
        public double Existencia // saldo
        {
            get { return existencia; }
            set { existencia = value; }
        }
        /*
        * Método para obtener y establecer el impuesto de la entidad.
        */
        public int Impuesto
        {
            get { return impuesto; }
            set { impuesto = value; }
        }
        /*
        * Método para obtener y establecer el precio en colones de la entidad.
        */
        public double PrecioColones
        {
            get { return precioColones; }
            set { precioColones = value; }
        }
        /*
        * Método para obtener y establecer el precio en dólares de la entidad.
        */
        public double PrecioDolares
        {
            get { return precioDolares; }
            set { precioDolares = value; }
        }
        /*
        * Método para obtener y establecer el costo en colones de la entidad.
        */
        public double CostoColones
        {
            get { return costoColones; }
            set { costoColones = value; }
        }
        /*
        * Método para obtener y establecer el costo en dólares de la entidad.
        */
        public double CostoDolares
        {
            get { return costoDolares; }
            set { costoDolares = value; }
        }
        /*
        * Método para obtener y establecer el id del producto global de la entidad.
        */
        public String Inv_Productos
        {
            get { return inv_productos; }
            set { inv_productos = value; }
        }
        /*
        * Método para obtener y establecer el id de la estación de la entidad.
        */
        public String Estacion
        {
            get { return estacion; }
            set { estacion = value; }
        }
        /*
        * Método para obtener y establecer el id del usuario de la entidad.
        */
        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        /*
        * Método para obtener y establecer la fecha de la entidad.
        */
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }

        }
    }
}