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

        public String[] insertarFactura(EntidadFactura factura)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO " + esquema + "REGISTRO_FACTURAS_VENTA (CONSECUTIVO, FECHA, ESTACION, COMPAÑIA, ACTIVIDAD, VENDEDOR, CLIENTE, TIPOMONEDA, IMPUESTO, METODOPAGO) VALUES ("
                + "REGISTRO_FACTURAS_SEQ.nextval,'"
                + factura.Fecha + "','"
                + factura.Estacion + "','"
                + factura.Compañia + "','"
                + factura.Actividad + "','"
                + factura.Vendedor + "','"
                + factura.Cliente + "','"
                + factura.TipoMoneda + "',"
                + factura.Impuesto + ",'"
                + factura.MetodoPago + ")";
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Factura agregada al sistema.";
            }
            catch (OracleException e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Factura no agregada.";
            }


                
            return res;
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
                command.CommandText = "SELECT INV_PRODUCTOS FROM " + esquema + "INV_PRODUCTOS WHERE CODIGO = '" + codigoProducto + "' AND NOMBRE = '" + nombreProducto + "' AND ESTADO = 1";
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
                command.CommandText = "SELECT ESTADO FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_PRODUCTOS = '" + llaveProducto + "'";
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
    }
}