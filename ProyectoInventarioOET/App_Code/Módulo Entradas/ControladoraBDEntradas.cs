using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    public class ControladoraBDEntradas : ControladoraBD
    {
        /*
 * Constructor.
 */
        public ControladoraBDEntradas()
        {
        }
        /*
-         * Recibe un id de bodega y devuelve las entradas para esa bodega especifica
-         */
        public DataTable consultarEntradasDeBodega(string bodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM " + esquema + "CAT_ENTRADAS WHERE CAT_BODEGA = '" + bodega + "'";
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
-         * Si en el ID se pasa por parametro todas, el sistema devuelve todas las facturas en la base,
-         * pero si recibe por paramentro un id o el inicio de un id, tratara de buscar todas las facturas que sean similares
-         */
        public DataTable buscarFacturas(string id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Compras.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                if ("Todas".Equals(id))
                {
                    command.CommandText = "SELECT * FROM " + esquema + "FACTURAS";
                    OracleDataReader reader = command.ExecuteReader();
                    resultado.Load(reader);

                }
                else
                {
                    command.CommandText = "SELECT *  "
                    + " FROM " + esquema + "FACTURAS "
                    + " WHERE IDFACTURA LIKE " + " '" + id + "%'";
                    OracleDataReader reader = command.ExecuteReader();
                    resultado.Load(reader);
                }

            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
         /*
-         *Recibe el id de una factura y devuelve el detalle de la misma 
-         */
        public DataTable consultarDetalleFactura(String id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Compras.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT *  "
                + " FROM " + esquema + "PRODUCTO_ORDENADOS "
                + " WHERE IDORDENDECOMPRA= " + " '" + id + "' ";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
        /*
-         * Recibe el id de una factura y devuelve los datos de esa factura.
-         */
        public DataTable consultarFactura(String id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Compras.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();

                command.CommandText = "SELECT *  "
                + " FROM " + esquema + "FACTURAS "
                + " WHERE IDFACTURA =" + " '" + id + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
        /*
-         * Recibe el id de una entrada y devuelve sus datos.
-         */
        public DataTable consultarEntrada(String id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Inventarios.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();

                command.CommandText = "SELECT *  "
                + " FROM " + esquema + "CAT_ENTRADAS"
                + " WHERE CAT_ENTRADAS =" + " '" + id + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
        /*
         *Recibe el id de una entrada de inventario y devuelve los productos asociados a esa 
         *entrada.
         */
        public DataTable consultarProductosEntrada(string id)
        {
            DataTable resultado = new DataTable();
            //String esquema = "Inventarios.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT NOMBRE,CANTIDAD,PRECIO_UNITARIO  FROM (SELECT * FROM CAT_ENTRADAS_PRODUCTOS JOIN INV_PRODUCTOS ON CAT_ENTRADAS_PRODUCTOS.CAT_PRODUCTOS = INV_PRODUCTOS.INV_PRODUCTOS) WHERE CAT_ENTRADAS" + "= '" + id + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
    }
}