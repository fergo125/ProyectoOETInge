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
        private String consecutivo;         //Número de la factura, llave
        private String fechaYhora;          //Fecha y hora de la creación de la factura
        private String estacion;            //Estación donde fue creada la factura
        private String bodega;              //Bodega donde fue creada la factura
        private String compania;            //Compañía a la que está asociada la factura
        private String vendedor;            //Vendedor que creó la factura
        private String tipoMoneda;          //Tipo de moneda usada en la creación de la factura
        private String metodoPago;          //Método de pago usado, puede ser "VARIOS"
        private String cliente;             //Cliente (empleado de la OET) de la venta
        private String actividad;           //Actividad a la que está asociada la factura
        private String estado;              //Estado de la factura (puede anularse)
        private double montoTotalColones;   //Monto total pagado en colones
        private double montoTotalDolares;   //Monto total pagado en dólares
        private DataTable productos;        //Lista de productos que fueron vendidos
        private List<Tuple<String, double>> pagosVariosMetodosPago;    //Opcional, cuando se paga con "VARIOS" métodos de pago, se guarda cuáles y lo pagado con cada uno

        /*
         * Constructor de la clase, recibe datos iniciales para crear una instancia de factura.
         */
        public EntidadFacturaVenta(Object[] datos)
        {
            this.consecutivo = (datos[0] == null ? "" : datos[0].ToString()); //durante la creación se recibe null, durante la consulta no
            this.fechaYhora = datos[1].ToString();
            this.bodega = datos[2].ToString();
            this.estacion = datos[3].ToString();
            this.compania = datos[4].ToString();
            this.actividad = datos[5].ToString();
            this.vendedor = datos[6].ToString();
            this.cliente = datos[7].ToString();
            this.tipoMoneda = datos[8].ToString();
            this.metodoPago = datos[9].ToString();
            this.montoTotalColones = Convert.ToDouble(datos[10].ToString());
            this.montoTotalDolares = Convert.ToDouble(datos[11].ToString());
            this.estado = datos[12].ToString().ToString();
            this.productos = (datos[13] == null ? null : (DataTable)datos[13]);
            this.pagosVariosMetodosPago = (datos[14] == null ? null : (List<Tuple<String, double>>)datos[14]);
        }



        /*
         * Métodos de acceso a datos (setters y getters).
         * Permiten obtener o manipular los atributos, cada método se refiere al atributo del mismo nombre.
         */

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

        public List<Tuple<String, double>> PagosVariosMetodosPago
        {
            get { return pagosVariosMetodosPago; }
            set { pagosVariosMetodosPago = value; }
        }
    }
}