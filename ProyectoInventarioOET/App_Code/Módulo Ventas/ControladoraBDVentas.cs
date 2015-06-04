using System;
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

            try
            {
                int idSiguienteFactura = getUltimoConsecutivo() + 1;
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO " + esquema + "REGISTRO_FACTURAS_VENTA (CONSECUTIVO, FECHAHORA, BODEGA, ESTACION, COMPAÑIA, ACTIVIDAD, VENDEDOR, CLIENTE, TIPOMONEDA, METODOPAGO, MONTOTOTALCOLONES, MONTOTOTALDOLARES, ESTADO) VALUES ("
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
                + factura.MontoTotalDolares + ","
                + factura.MontoTotalColones + ","
                + factura.Estado + ")";
                OracleDataReader reader = command.ExecuteReader();

                if (insertarProductosFactura(idSiguienteFactura, factura.Productos))
                {
                    res[0] = "success";
                    res[1] = "Éxito:";
                    res[2] = "Factura agregada al sistema con éxito.";
                }
                else //hubo un error al intentar insertar los productos, pero la factura se insertó bien
                {
                    res[0] = "danger";
                    res[1] = "Error:";
                    res[2] = "Error al intentar insertar los productos de la factura en la base de datos. La factura fue insertada con éxito pero sus productos no.";
                }
            }
            catch (OracleException e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Error al intentar insertar la factura en la base de datos.";
            }

            return res;
        }

        /*
         * Una vez que se inserta una factura general, se procede a insertar los detalles de la misma en otra tabla,
         * se insertan los productos asociados a la misma usando la llave que se insertó con éxito previamente.
         * Retorna true si logra insertar los productos con éxito, de lo contrario, retorna false.
         */
        private bool insertarProductosFactura(int idFactura, DataTable productos)
        {
            String esquema = "Inventarios.";
            try
            {
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

                String queryInsercion = "INSERT ALL" + tuplas + " SELECT * FROM DUAL"; //query unificado

                OracleCommand command = conexionBD.CreateCommand();
                command = conexionBD.CreateCommand();
                command.CommandText = queryInsercion;
                OracleDataReader reader = command.ExecuteReader();

                return true;
            }
            catch (OracleException e)
            {
                return false;
            }
        }

        /*
         * ???
         */
        public DataTable consultarFacturas(String idVendedor, String idBodega, String idEstacion)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                String consulta = "SELECT * FROM " + esquema + "REGISTRO_FACTURAS_VENTA";
                if (idVendedor != "All" || idBodega != "All" || idEstacion != "All") //Se debe parametrizar con alguno de los 3
                {
                    consulta = consulta + " WHERE ";
                    if (idVendedor != "All")
                        consulta = consulta + "VENDEDOR = '" + idVendedor + "' AND ";
                    if (idBodega != "All")
                        consulta = consulta + "BODEGA = '" + idBodega + "' AND ";
                    if (idEstacion != "All")
                        consulta = consulta + "ESTACION = '" + idEstacion + "' AND ";
                    consulta = consulta.Substring(0, consulta.Length - 5); //se le quita el último pedazo de " AND "
                }
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = consulta;
                OracleDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                    resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        /*
         * ???
         */
        public EntidadFacturaVenta consultarFactura(String codigo)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            EntidadFacturaVenta facturaConsultada = null;
            Object[] datosConsultados = new Object[13];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM " + esquema + "REGISTRO_FACTURAS_VENTA WHERE REGISTRO_FACTURAS_VENTA.CONSECUTIVO= '" + codigo + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

                if (resultado.Rows.Count == 1)
                {
                    datosConsultados[0] = codigo;
                    for (int i = 1; i < 12; i++)
                    {
                        datosConsultados[i] = resultado.Rows[0][i].ToString();
                    }

                    facturaConsultada = new EntidadFacturaVenta(datosConsultados);
                }
            }
            catch (OracleException e)
            {
            }
            return facturaConsultada;
        }

        /*
         * ???
         */
        public DataTable asociadosABodega(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT UNIQUE SEG_USUARIO FROM " + esquema + "SEG_USUARIO_BODEGA WHERE CAT_BODEGA = '" + idBodega + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        /*
         * Dada una estación, consulta todos los usuarios asociados a bodegas que pertenezcan a esa estación.
         */
        public DataTable asociadosAEstacion(String idEstacion)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                if(idEstacion == "All")
                    command.CommandText = "SELECT UNIQUE SEG_USUARIO FROM " + esquema + "SEG_USUARIO_BODEGA"; //si se consultó de todas las estaciones, mostrar todos los vendedores
                else
                    command.CommandText = "SELECT UNIQUE SEG_USUARIO FROM " + esquema + "SEG_USUARIO_BODEGA WHERE ESTACION = '" + idEstacion + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        /*
         * ???
         */
        public int getUltimoConsecutivo()
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT MAX(CONSECUTIVO) FROM " + esquema + "REGISTRO_FACTURAS_VENTA";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return Convert.ToInt32(resultado.Rows[0][0].ToString());
        }

        /*
         * Invocada para revisar si un producto existe en el catálogo global, y está en estado activo. Parte del proceso de agregar un producto a una factura de venta.
         */
        public String verificarExistenciaProductoGlobal(String nombreProducto, String codigoProducto)
        {
            String esquema = "Inventarios.";
            String llaveProducto = null;
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT INV_PRODUCTOS FROM " + esquema + "INV_PRODUCTOS WHERE CODIGO = '" + codigoProducto + "' AND NOMBRE = (REPLACE('" + nombreProducto + "', '\\', '')) AND ESTADO = 1"; //se usa el replace de oracle porque si no entonces no quita el backslash
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
                if (resultado.Rows.Count == 1)
                {
                    llaveProducto = resultado.Rows[0][0].ToString();
                }
            }
            catch (Exception e)
            {
                llaveProducto = null;
            }
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
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT ESTADO FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_PRODUCTOS = '" + llaveProducto + "' AND CAT_BODEGA = '" + llaveBodega + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
                if (resultado.Rows.Count == 1)
                {
                    valido = (resultado.Rows[0][0].ToString() == "1" ? true : false);
                }
            }
            catch (Exception e)
            {
                valido = false;
            }
            return valido;
        }

        /*
         * Obtiene la venta del tipo de cambio más reciente desde la tabla de la base de datos.
         */
        public double consultarTipoCambio()
        {
            String esquema = "Reservas.";
            DataTable resultado = new DataTable();
            double ventaDolar = 0;
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT VENTA FROM " + esquema + "TIPOCAMBIO WHERE DEL = (SELECT MAX(DEL) FROM RESERVAS.TIPOCAMBIO)";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
                if (resultado.Rows.Count == 1)
                {
                    ventaDolar = Convert.ToDouble(resultado.Rows[0][0]);
                }
            }
            catch (Exception e)
            {
                ventaDolar = -1;
            }
            return ventaDolar;
        }

        /*
         * Obtiene el precio unitario de un producto, según el parámetro de si es en colones o en dólares.
         */
        public double consultarPrecioUnitario(String llaveProducto, String tipoMoneda)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            double precioUnitario = 0;
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                if(tipoMoneda == "Colones")
                    command.CommandText = "SELECT PRECIO_C FROM " + esquema + "INV_PRODUCTOS WHERE INV_PRODUCTOS = '" + llaveProducto + "'";
                else //Dolares
                    command.CommandText = "SELECT PRECIO_D FROM " + esquema + "INV_PRODUCTOS WHERE INV_PRODUCTOS = '" + llaveProducto + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
                if (resultado.Rows.Count == 1)
                {
                    precioUnitario = Convert.ToDouble(resultado.Rows[0][0]);
                }
            }
            catch (Exception e)
            {
                precioUnitario = -1;
            }
            return precioUnitario;
        }

        /*
         * Obtiene el máximo de descuento aplicable a la venta de un producto específico por parte de un empleado específico.
         */
        public int maximoDescuentoAplicable(String idProducto, String idVendedor)
        {
            int maximo=0;
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT MIN(DESCUENTO_MAXIMO) FROM ((SELECT DESCUENTO_MAXIMO FROM INV_PRODUCTOS WHERE INV_PRODUCTOS = '" + idProducto + "') UNION ALL (SELECT DESCUENTO_MAXIMO FROM SEG_USUARIO WHERE SEG_USUARIO = '" + idVendedor + "'))";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
                maximo = Convert.ToInt32(resultado.Rows[0][0].ToString());
            }
            catch (Exception){}
            return maximo;
        }


        public String[] anularFactura(EntidadFacturaVenta factura)
        {
            String esquema = "Inventarios.";

            String[] res = new String[4];
            res[3] = factura.Consecutivo;

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE " + esquema + "REGISTRO_FACTURAS_VENTA SET ESTADO = 'Anulada' WHERE CONSECUTIVO='" + factura.Consecutivo + "'";
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Factura anulada en el sistema.";
            }
            catch (OracleException e)
            {
                if (e.Number == 2627)
                {
                    res[0] = "danger";
                    res[1] = "Error:";
                    res[2] = "Factura no anulada, intente nuevamente.";
                }
            }
            return res;
        }

        /*
         * 
         */
        public DataTable consultarMetodosPago()
        {
            String esquema = "Reservas.";
            DataTable metodosPago = new DataTable(); ;
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT NOMBRE,ID FROM " + esquema + "FORMAPAGO ";
                OracleDataReader reader = command.ExecuteReader();
                metodosPago.Load(reader);
            }
            catch (OracleException e)
            {
                metodosPago = null;
            }
            return metodosPago;
        }

        /*
         * 
         */
        public DataTable consultarClientes()
        {
            String esquema = "Inventarios.";
            DataTable posiblesClientes = new DataTable(); ;
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT NOMBRE,SEG_USUARIO FROM " + esquema + "SEG_USUARIO ";
                OracleDataReader reader = command.ExecuteReader();
                posiblesClientes.Load(reader);
            }
            catch (OracleException e)
            {
                posiblesClientes = null;
            }
            return posiblesClientes;
        }
    }
}