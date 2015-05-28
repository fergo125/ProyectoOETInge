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
                int idSiguienteFactura = getCantidadFacturas();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO " + esquema + "REGISTRO_FACTURAS_VENTA (CONSECUTIVO, FECHA, ESTACION, COMPAÑIA, ACTIVIDAD, VENDEDOR, CLIENTE, TIPOMONEDA, METODOPAGO) VALUES ("
                + (idSiguienteFactura + 1) + ",'"
                + factura.Fecha + "','"
                + factura.Estacion + "','"
                + factura.Compañia + "','"
                + factura.Actividad + "','"
                + factura.Vendedor + "','"
                + factura.Cliente + "','"
                + factura.TipoMoneda + "','"
                + factura.MetodoPago + "')";
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


        public int getCantidadFacturas()
        {
            DataTable resultado = new DataTable();
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM REGISTRO_FACTURAS_VENTA";
            OracleDataReader reader = command.ExecuteReader();
            resultado.Load(reader);
            return Convert.ToInt32(resultado.Rows[0][0].ToString());
        }

    }
}