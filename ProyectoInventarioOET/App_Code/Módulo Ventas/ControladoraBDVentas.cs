﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET.Modulo_Ventas
{
    /*
     * Clase utilizada dentro del módulo de ventas para la comunicación con la base de datos.
     */
    public class ControladoraBDVentas : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDVentas()
        {
        }

        /*
         * ???
         */
        public String[] insertarFactura(EntidadFacturaVenta factura)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];
            bool exito = false;

            int idSiguienteFactura = getUltimoConsecutivo() + 1;
            String comandoSQL = "INSERT INTO " + esquema + "REGISTRO_FACTURAS_VENTA (CONSECUTIVO, FECHAHORA, BODEGA, ESTACION, COMPAÑIA, ACTIVIDAD, VENDEDOR, CLIENTE, TIPOMONEDA, METODOPAGO, MONTOTOTALCOLONES, MONTOTOTALDOLARES, ESTADO) VALUES ("
            + idSiguienteFactura + ",'"
            + factura.FechaHora + "','"
            + factura.Bodega + "','"
            + factura.Estacion + "','"
            + factura.Compania + "','"
            + factura.Actividad + "','"
            + factura.Vendedor + "','"
            + factura.Cliente + "','"
            + factura.TipoMoneda + "','"
            + factura.MetodoPago + "',"
            + factura.MontoTotalColones + ","
            + factura.MontoTotalDolares + ","
            + factura.Estado + ")";
            if (ejecutarComandoSQL(comandoSQL, false) != null) //si logra insertar la factura
            {
                quitarProductosHistorial(factura.Bodega, factura.Productos);
                if (insertarProductosFactura(idSiguienteFactura, factura.Productos)) //si logra insertar los productos
                {
                    if (factura.MetodoPago == "VARIOS")
                    {
                        if (insertarVariosMetodosPago(idSiguienteFactura, factura.PagosVariosMetodosPago)) //si logra insertar los métodos de pago
                            exito = true;
                    }
                    else
                            exito = true;
                }
            }
            if(exito)
            {
                    res[0] = "success";
                    res[1] = "Éxito:";
                    res[2] = "Factura agregada al sistema con éxito.";
            }
            else //hubo un error al intentar insertar los productos, pero la factura se insertó bien
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Error al intentar insertar la factura en la base de datos.";
            }
            return res;
        }

        /*
         * Antes de insertar los detalles de la factura, se actualiza la tabla de historial
         * Se elimina cada producto, en su cantidad
         */
        private void quitarProductosHistorial( String idBodega, DataTable productos )
        {
            foreach (DataRow producto in productos.Rows)
            {
                String comandoSQL = "SELECT INV_BODEGA_PRODUCTOS FROM INV_BODEGA_PRODUCTOS WHERE INV_PRODUCTOS = '" + producto[1].ToString() + "' AND CAT_BODEGA = '" + idBodega + "' ";
                DataTable temp = ejecutarComandoSQL(comandoSQL, true);
                String llaveProductoBodega = temp.Rows[0][0].ToString();
                comandoSQL = "call quitar_historial( '" + llaveProductoBodega + "', " + producto[4].ToString() + " )";
                ejecutarComandoSQL(comandoSQL, false);
            }
        }

        /*
         * Una vez que se inserta una factura general, se procede a insertar los detalles de la misma en otra tabla,
         * se insertan los productos asociados a la misma usando la llave que se insertó con éxito previamente.
         * Retorna true si logra insertar los productos con éxito, de lo contrario, retorna false.
         */
        private bool insertarProductosFactura(int idFactura, DataTable productos)
        {
            String esquema = "Inventarios.";
            String tuplas = ""; //se agrega cada producto en una inserción por aparte para luego unirlas en un sólo query
            foreach (DataRow producto in productos.Rows)
            {
                tuplas += " INTO " + esquema + "REGISTRO_DETALLES_FACTURAS VALUES( "
                    + idFactura + ", '"
                    + producto[1].ToString() + "', "
                    + producto[4].ToString() + ", "
                    + producto[2].ToString() + ", "
                    + producto[3].ToString() + ", "
                    + producto[6].ToString() + ", "
                    + producto[5].ToString() + ")";
            }

            String comandoSQL = "INSERT ALL" + tuplas + " SELECT * FROM DUAL"; //query unificado
            if (ejecutarComandoSQL(comandoSQL, false) != null)
                return true;
            return false;
        }

        /*
         * Una vez que se inserta una factura general y los productos asociados a la misma, en caso de que se hayan especificado
         * varios métodos de pago, se procede a insertar los detalles de éstos: la identificación de cada método usado y el pago
         * que se realizó con cada uno.
         * Retorna true si logra insertar todo con éxito, de lo contrario, retorna false.
         */
        private bool insertarVariosMetodosPago(int idFactura, List<String> pagosVariosMetodosPago)
        {
            String esquema = "Inventarios.";
            String tuplas = ""; //se agrega cada método en una inserción por aparte para luego unirlas en un sólo query
            for (short i=0; i<pagosVariosMetodosPago.Count;)
            {
                tuplas += " INTO " + esquema + "REGISTRO_FACTURAS_METODOS VALUES( "
                    + idFactura + ", '"
                    + pagosVariosMetodosPago.ElementAt<String>(i++) + "', "
                    + pagosVariosMetodosPago.ElementAt<String>(i++) + ")";
            }

            String comandoSQL = "INSERT ALL" + tuplas + " SELECT * FROM DUAL"; //query unificado
            if (ejecutarComandoSQL(comandoSQL, false) != null)
                return true;
            return false;
        }

        /*
         * Consulta las facturas usando los posibles filtros: vendedor, bodega, estación, método de pago, fecha inicial y fecha final.
         */
        public DataTable consultarFacturas(String idVendedor, String idBodega, String idEstacion, String idMetodoPago, String idCliente, String fechaInicio, String fechaFinal)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "REGISTRO_FACTURAS_VENTA";
            if (idEstacion == "All" && idBodega == "All" && idVendedor == "") idVendedor = "All";
            if (idVendedor != "All" || idBodega != "All" || idEstacion != "All" || idMetodoPago != "All" || idCliente != "All" || fechaInicio!="" || fechaFinal!="") //Se debe parametrizar con alguno de los 3
            {
                comandoSQL += " WHERE ";
                if (idVendedor != "All")
                    comandoSQL += "VENDEDOR = '" + idVendedor + "' AND ";
                if (idBodega != "All")
                    comandoSQL += "BODEGA = '" + idBodega + "' AND ";
                if (idEstacion != "All")
                    comandoSQL += "ESTACION = '" + idEstacion + "' AND ";
                if (idMetodoPago != "All")
                    comandoSQL += "METODOPAGO = '" + idMetodoPago + "' AND ";
                if (idCliente != "All")
                    comandoSQL += "CLIENTE = '" + idCliente + "' AND ";
                if (fechaInicio != "")
                    comandoSQL += "FECHAHORA >= TO_DATE('" + fechaInicio + "','DD/MM/YYYY') AND ";
                if (fechaFinal != "")
                    comandoSQL += "FECHAHORA <= TO_DATE('" + fechaFinal + "','DD/MM/YYYY') AND ";
                comandoSQL = comandoSQL.Substring(0, comandoSQL.Length - 4); //se le quita el último pedazo de "AND " que haya quedado
            }
            comandoSQL += " ORDER BY CONSECUTIVO DESC";
            resultado =  ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Consulta una factura de venta específica.
         */
        public EntidadFacturaVenta consultarFactura(String llaveFactura)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            EntidadFacturaVenta facturaConsultada = null;
            Object[] datosConsultados = new Object[15];
            String comandoSQL = "SELECT * FROM " + esquema + "REGISTRO_FACTURAS_VENTA WHERE CONSECUTIVO= '" + llaveFactura + "'";
            resultado =  ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
            {
                for (int i = 0; i < 13; i++)
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                datosConsultados[13] = consultarProductosDeFactura(llaveFactura); //productos asociados a factura
                datosConsultados[14] = null; //varios métodos de pago
                facturaConsultada = new EntidadFacturaVenta(datosConsultados);
            }
            return facturaConsultada;
        }

        /*
         * Obtiene la lista de los productos asociados a una factura, dada una factura específica consultada.
         */
        private DataTable consultarProductosDeFactura(String llaveFactura)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "REGISTRO_DETALLES_FACTURAS WHERE IDFACTURA= '" + llaveFactura + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * ???
         */
        public DataTable asociadosABodega(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT UNIQUE SEG_USUARIO FROM " + esquema + "SEG_USUARIO_BODEGA WHERE CAT_BODEGA = '" + idBodega + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Dada una estación, consulta todos los usuarios asociados a bodegas que pertenezcan a esa estación.
         */
        public DataTable asociadosAEstacion(String idEstacion)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "";
            OracleCommand command = conexionBD.CreateCommand();
            if(idEstacion == "All")
                comandoSQL = "SELECT UNIQUE SEG_USUARIO FROM " + esquema + "SEG_USUARIO_BODEGA"; //si se consultó de todas las estaciones, mostrar todos los vendedores
            else
                comandoSQL = "SELECT UNIQUE SEG_USUARIO FROM " + esquema + "SEG_USUARIO_BODEGA WHERE ESTACION = '" + idEstacion + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Consulta el último consecutivo de factura para generar el siguiente. Si no hay factura, retorna un 0.
         */
        public int getUltimoConsecutivo()
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT MAX(CONSECUTIVO) FROM " + esquema + "REGISTRO_FACTURAS_VENTA";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if(resultado.Rows.Count == 1)
                return (resultado.Rows[0][0].ToString() == "" ? 0 : Convert.ToInt32(resultado.Rows[0][0].ToString())); //operador ternario por si no hay ninguna factura existente y retorna null
            return -1;
        }

        /*
         * Invocada para revisar si un producto existe en el catálogo global, y está en estado activo. Parte del proceso de agregar un producto a una factura de venta.
         */
        public String verificarExistenciaProductoGlobal(String nombreProducto, String codigoProducto)
        {
            String esquema = "Inventarios.";
            String llaveProducto = null;
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT INV_PRODUCTOS FROM " + esquema + "INV_PRODUCTOS WHERE CODIGO = '" + codigoProducto + "' AND NOMBRE = (REPLACE('" + nombreProducto + "', '\\', '')) AND ESTADO = 1"; //se usa el replace de oracle porque si no entonces no quita el backslash
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                llaveProducto = resultado.Rows[0][0].ToString();
            return llaveProducto;
        }


        public String getLlaveProductoBodega(String idProducto, String bodega) 
        {
            String esquema = "Inventarios.";
            String llaveProducto = null;
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT INV_BODEGA_PRODUCTOS FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_PRODUCTOS = '" + idProducto + "' AND CAT_BODEGA = '" + bodega + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                llaveProducto = resultado.Rows[0][0].ToString();
            return llaveProducto;
        }

        /*
         * Invocada para revisar si un producto existente, se encuentra asociado o no a una bodega punto de venta, y si se encuentra en estado
         * activo. Parte del proceso de agregar un producto a una factura de venta.
         */
        public bool verificarExistenciaProductoLocal(String llaveProducto, String llaveBodega)
        {
            String esquema = "Inventarios.";
            bool valido = false; //se considera valido si existe y si está en estado activo
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT ESTADO FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_PRODUCTOS = '" + llaveProducto + "' AND CAT_BODEGA = '" + llaveBodega + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                valido = (resultado.Rows[0][0].ToString() == "1" ? true : false);
            return valido;
        }

        /*
         * Obtiene la venta del tipo de cambio más reciente desde la tabla de la base de datos.
         */
        public double consultarTipoCambio()
        {
            String esquema = "Reservas.";
            DataTable resultado = new DataTable();
            double ventaDolar = -1;
            String comandoSQL = "SELECT VENTA FROM " + esquema + "TIPOCAMBIO WHERE DEL = (SELECT MAX(DEL) FROM RESERVAS.TIPOCAMBIO)";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                ventaDolar = Convert.ToDouble(resultado.Rows[0][0]);
            return ventaDolar;
        }

        /*
         * Obtiene el precio unitario de un producto, según el parámetro de si es en colones o en dólares.
         */
        public double consultarPrecioUnitario(String llaveProducto, String tipoMoneda)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            double precioUnitario = -1;
            String comandoSQL = "";
            if(tipoMoneda == "Colones")
                comandoSQL = "SELECT PRECIO_C FROM " + esquema + "INV_PRODUCTOS WHERE INV_PRODUCTOS = '" + llaveProducto + "'";
            else //Dolares
                comandoSQL = "SELECT PRECIO_D FROM " + esquema + "INV_PRODUCTOS WHERE INV_PRODUCTOS = '" + llaveProducto + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                precioUnitario = Convert.ToDouble(resultado.Rows[0][0]);
            return precioUnitario;
        }

        /*
         * Obtiene el máximo de descuento aplicable a la venta de un producto específico por parte de un empleado específico.
         */
        public int maximoDescuentoAplicable(String idProducto, String idVendedor)
        {
            int maximo = 0;
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT MIN(DESCUENTO_MAXIMO) FROM ((SELECT DESCUENTO_MAXIMO FROM INV_PRODUCTOS WHERE INV_PRODUCTOS = '" + idProducto + "') UNION ALL (SELECT DESCUENTO_MAXIMO FROM SEG_USUARIO WHERE SEG_USUARIO = '" + idVendedor + "'))";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1) 
                maximo = Convert.ToInt32(resultado.Rows[0][0].ToString());
            return maximo;
        }

        /*
         * ???
         */
        public String[] anularFactura(EntidadFacturaVenta factura)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[4];
            resultado[3] = factura.Consecutivo;
            String comandoSQL = "UPDATE " + esquema + "REGISTRO_FACTURAS_VENTA SET ESTADO = 'Anulada' WHERE CONSECUTIVO='" + factura.Consecutivo + "'";
            if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                resultado[0] = "success";
                resultado[1] = "Éxito:";
                resultado[2] = "Factura anulada en el sistema.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error:";
                resultado[2] = "Factura no anulada, intente nuevamente.";
            }
            return resultado;
        }

        /*
         * Obtiene las diferentes opciones de formas de pago para escoger una durante la creación de una factura.
         */
        public DataTable consultarMetodosPago()
        {
            String esquema = "Reservas.";
            DataTable metodosPago = new DataTable(); ;
            String comandoSQL = "SELECT NOMBRE,ID FROM " + esquema + "FORMAPAGO ";
            metodosPago = ejecutarComandoSQL(comandoSQL, true);
            return metodosPago;
        }

        /*
         * Obtiene el nombre de un método de pago dado su ID.
         */
        public String consultarMetodoDePago(String llaveMetodo)
        {
            String esquema = "Reservas.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT NOMBRE FROM " + esquema + "FORMAPAGO WHERE ID='" + llaveMetodo + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                return resultado.Rows[0][0].ToString();
            return null;
        }

        /*
         * Obtiene la lista de empleados de la OET para asociar uno como cliente durante la creación de una factura.
         */
        public DataTable consultarClientes()
        {
            String esquema = "Inventarios.";
            DataTable posiblesClientes = new DataTable(); ;
            String comandoSQL = "SELECT NOMBRE,SEG_USUARIO FROM " + esquema + "SEG_USUARIO ";
            posiblesClientes = ejecutarComandoSQL(comandoSQL, true);
            return posiblesClientes;
        }

        public String getExistenciaActual(string idProductoBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT SALDO FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS ='" + idProductoBodega + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
                return resultado.Rows[0][0].ToString();
            return null;
        }
    }
}