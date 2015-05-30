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
        public String[] insertarFactura(EntidadFactura factura)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];

            try
            {
                int idSiguienteFactura = getCantidadFacturas();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO " + esquema + "REGISTRO_FACTURAS_VENTA (CONSECUTIVO, FECHA, BODEGA, ESTACION, COMPAÑIA, ACTIVIDAD, VENDEDOR, CLIENTE, TIPOMONEDA, METODOPAGO, MONTOTOTAL, ESTADO) VALUES ("
                + (idSiguienteFactura + 1) + ",'"
                + factura.Fecha + "','"
                + factura.Bodega + "','"
                + factura.Estacion + "','"
                + factura.Compañia + "','"
                + factura.Actividad + "','"
                + factura.Vendedor + "','"
                + factura.Cliente + "','"
                + factura.TipoMoneda + "','"
                + factura.MetodoPago + "',"
                + factura.MontoTotal + ",'"
                + factura.Estado + "')";
                OracleDataReader reader = command.ExecuteReader();


                String tuplasAMeter = "";
                /* foreach (DataRow producto in factura.Productos.Rows)
                 {
                     tuplasAMeter += " INTO REGISTRO_DETALLES_FACTURAS VALUES( "
                         + idSiguienteFactura +",'"
                         + producto[0].ToString() + "',"
                         + producto[1].ToString() + ","
                         + producto[2].ToString() + ","
                         + producto[3].ToString() + ","
                         + producto[4].ToString() + ","
                         + producto[5].ToString() + ","
                         + ") "; 
                 }*/

                String insercion = "INSERT ALL " + tuplasAMeter + " SELECT * FROM dual";

                command = conexionBD.CreateCommand();
                command.CommandText = insercion;
                reader = command.ExecuteReader();


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
         * ???
         */
        public DataTable consultarFacturas(String perfil, String idUsuario, String idBodega, String idEstacion)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                if (perfil.Equals("Vendedor"))
                    command.CommandText = "select * from " + esquema + "registro_facturas_venta where " + esquema + "registro_facturas_venta.vendedor = '" + idUsuario + "'";
                else
                {
                    if (perfil.Equals("Supervisor"))
                    {
                        if (idUsuario.Equals("Todos"))
                            command.CommandText = "select * from " + esquema + "registro_facturas_venta where " + esquema + "registro_facturas_venta.bodega = '" + idBodega + "'";
                        else
                            command.CommandText = "select * from " + esquema + "registro_facturas_venta where " + esquema + "registro_facturas_venta.bodega = '" + idBodega + "' and registro_facturas_venta.vendedor='" + idUsuario + "'";
                    }
                    else
                    {
                        if (perfil.Equals("Administrador local"))
                        {
                            if (idBodega.Equals("Todas"))
                                command.CommandText = "select * from registro_facturas_venta where estacion = '" + idEstacion + "'";
                            else
                            {
                                if (idUsuario.Equals("Todos"))
                                    command.CommandText = "select * from " + esquema + "registro_facturas_venta where " + esquema + "registro_facturas_venta.estacion = '" + idEstacion + "' and registro_facturas_venta.bodega = '" + idBodega + "'";
                                else
                                    command.CommandText = "select * from " + esquema + "registro_facturas_venta where " + esquema + "registro_facturas_venta.estacion = '" + idEstacion + "' and registro_facturas_venta.bodega = '" + idBodega + "' and registro_facturas_venta.vendedor='" + idUsuario + "'";
                            }
                        }
                        else
                        {
                            if (idEstacion.Equals("Todas"))
                                command.CommandText = "select * from registro_facturas_venta";
                            else
                            {
                                if (idBodega.Equals("Todas"))
                                    command.CommandText = "select * from registro_facturas_venta where estacion = '" + idEstacion + "'";
                                else
                                {
                                    if (idUsuario.Equals("Todos"))
                                        command.CommandText = "select * from " + esquema + "registro_facturas_venta where " + esquema + "registro_facturas_venta.estacion = '" + idEstacion + "' and registro_facturas_venta.bodega = '" + idBodega + "'";
                                    else
                                        command.CommandText = "select * from " + esquema + "registro_facturas_venta where " + esquema + "registro_facturas_venta.estacion = '" + idEstacion + "' and registro_facturas_venta.bodega = '" + idBodega + "' and registro_facturas_venta.vendedor='" + idUsuario + "'";
                                }
                            }
                        }
                    }
                }
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
        public EntidadFactura consultarFactura(String codigo)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            EntidadFactura facturaConsultada = null;
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

                    facturaConsultada = new EntidadFactura(datosConsultados);
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
                command.CommandText = "select seg_usuario from "+ esquema +"seg_usuario_bodega where cat_bodega = '"+idBodega+"'";
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
        public int getCantidadFacturas()
        {
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM REGISTRO_FACTURAS_VENTA";
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
    }
}