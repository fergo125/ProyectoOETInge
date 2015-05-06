using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales
{
   
    /* Controladora de bases de datos de productos globales, utilizada por la controladora de productos globales.
     * Se comunica directamente con la base de datos y provee los datos pedidos. 
     * Ofrece la comunicación entre la Controladora de productos globales y la base de datos.
     * Es una sub-clase de la controladoraBD que permite la conexión con la BD y la generación de códigos
     */
    public class ControladoraBDProductosGlobales : ControladoraBD
    {

        /*
        * Método que retorna la entidad de un producto global específico, mediante la 
         * ejecución de un comando SQL mediante el cliente de Oracle
        */
        public EntidadProductoGlobal consultarProductoGlobal(String id)
        {
            DataTable resultado = new DataTable();
            EntidadProductoGlobal productoConsultado = null;
            Object[] datosConsultados = new Object[14];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT P.CODIGO, P.CODIGO_BARRAS, P.NOMBRE, P.COSTO_COLONES, P.CAT_CATEGORIAS, P.CAT_UNIDADES, P.SALDO, "
                +                     "P.ESTADO, P.COSTO_DOLARES, P.IMPUESTO, P.INTENCION, P.PRECIO_C, P.PRECIO_D, P.INV_PRODUCTOS "
                +                     "FROM INV_PRODUCTOS P WHERE INV_PRODUCTOS = '" + id + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

                if (resultado.Rows.Count == 1)
                {
                    datosConsultados[0] = id;
                    for (int i = 0; i < 14; i++)
                    {  
                        datosConsultados[i] = resultado.Rows[0][i].ToString();
                    }
                    productoConsultado = new EntidadProductoGlobal(datosConsultados);
                }
            }
            catch (Exception e)
            {
                productoConsultado = null;
            }

            return productoConsultado;
        }

        /*
        * Método que inserta un producto global específico, previamente encapsulado en la entidad 
        * correspondiente, mediante la ejecución de un comando SQL mediante el cliente de Oracle
        */

        public string[] insertarProductoGlobal(EntidadProductoGlobal productoGlobal)
        {
            String[] res = new String[4]; // Vector que contiene la información sobre el resultado de la transacción en la base de datos
            res[3] = generarID();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                String aux =  "INSERT INTO INV_PRODUCTOS ( NOMBRE, CODIGO, CODIGO_BARRAS, CAT_CATEGORIAS, INTENCION, CAT_UNIDADES, ESTADO,  "
                + "SALDO, IMPUESTO, PRECIO_C, PRECIO_D, COSTO_COLONES , COSTO_DOLARES , INV_PRODUCTOS, CREA, CREADO ) VALUES ( '"
                + productoGlobal.Nombre + "' , '" + productoGlobal.Codigo + "' , '"
                + productoGlobal.CodigoDeBarras + "' , '" + productoGlobal.Categoria + "' , '"
                + productoGlobal.Intencion + "' , '" + productoGlobal.Unidades + "' , "
                + (short) productoGlobal.Estado + " , " + productoGlobal.Existencia + " , "
                + productoGlobal.Impuesto + " , " + productoGlobal.PrecioColones + " , "
                + productoGlobal.PrecioDolares + " , " + productoGlobal.CostoColones + " , "
                + productoGlobal.CostoDolares + " , '" + res[3] + "' , '" + productoGlobal.Usuario +
                "' , TO_DATE('" + productoGlobal.Fecha.ToString("MM/dd/yyyy HH:mm:ss") + "', 'mm/dd/yyyy hh24:mi:ss') ) ";


                command.CommandText = aux; 
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Producto agregado al catálogo general.";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Producto no agregado, intente nuevamente.";
            }
            return res;
        }

        /*
        * Método encargado de modificar los datos de un producto global específico(en forma de entidad) con los nuevos 
        * datos ingresados por el usuario mediante el envío de un comando SQL a Oracle 
        */
        public string[] modificarProductoGlobal(EntidadProductoGlobal productoGlobalViejo, EntidadProductoGlobal productoGlobalNuevo)
        {
            String[] res = new String[4]; // Vector que contiene la información sobre el resultado de la transacción en la base de datos
            res[3] = productoGlobalViejo.Inv_Productos.ToString();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                String aux =        "UPDATE INV_PRODUCTOS "
                                    + "SET CAT_CATEGORIAS = '" + productoGlobalNuevo.Categoria + "' , "
                                    + " CODIGO = '" + productoGlobalNuevo.Codigo + "' , "
                                    + " CODIGO_BARRAS = '" + productoGlobalNuevo.CodigoDeBarras + "' , "
                                    + " COSTO_COLONES = " + productoGlobalNuevo.CostoColones + " , "
                                    + " COSTO_DOLARES = " + productoGlobalNuevo.CostoDolares +  " , "
                                    + " ESTADO = " + productoGlobalNuevo.Estado + " , "
                                    + " SALDO = " + productoGlobalNuevo.Existencia + " , "
                                    + " IMPUESTO = " + productoGlobalNuevo.Impuesto + " , "
                                    + " INTENCION = '" + productoGlobalNuevo.Intencion + "' , "
                                    + " NOMBRE = '" + productoGlobalNuevo.Nombre + "' , "
                                    + " PRECIO_C = " + productoGlobalNuevo.PrecioColones + " , "
                                    + " PRECIO_D= " + productoGlobalNuevo.PrecioDolares + " , "
                                    + " CAT_UNIDADES = '" + productoGlobalNuevo.Unidades + "' , "
                                    + " MODIFICA = '" + productoGlobalNuevo.Usuario + "' , "
                                    + " MODIFICADO = TO_DATE('" + productoGlobalNuevo.Fecha.ToString("MM/dd/yyyy HH:mm:ss") + "', 'mm/dd/yyyy hh24:mi:ss') "
                                    + "WHERE INV_PRODUCTOS = '" + productoGlobalViejo.Inv_Productos + "' ";


                command.CommandText = aux;
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Producto modificado en el sistema.";
            }
            catch (OracleException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Producto no modificado, intente nuevamente.";
            }
            return res;
        }
        /*
         * Método encargado de desactivar un producto global (en forma de entidad) 
         * mediante el envío de un comando SQL a Oracle 
         */
        public string[] desactivarProductoGlobal(EntidadProductoGlobal productoGlobal)
        {
            String[] res = new String[4];
            res[3] = productoGlobal.Codigo.ToString(); // Vector que contiene la información sobre el resultado de la transacción en la base de datos
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText =   "UPDATE INV_PRODUCTOS "
                                      + "SET ESTADO = " + productoGlobal.Estado.ToString()
                                      + "WHERE INV_PRODUCTOS = " + productoGlobal.Inv_Productos;
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Producto desactivado en el sistema.";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Producto no desactivado, intente nuevamente.";
            }
            return res;
        }

        /*
         * Método encargado de retornar la información más relevante de todos productos globales 
         * mediante el envío de un comando SQL a Oracle 
         */

        public DataTable consultarProductosGlobales()
        {
            DataTable resultado = new DataTable();

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText =   "SELECT P.INV_PRODUCTOS, P.NOMBRE, P.CODIGO, P.CAT_CATEGORIAS, P.ESTADO  "  
                +                       "FROM INV_PRODUCTOS P";
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
         * Método encargado de retornar la información más relevante de los productos globales que concuerdan con la busqueda del
         * usuario mediante el envío de un comando SQL a Oracle 
         */
        public DataTable consultarProductosGlobales(String query)
        {
            DataTable resultado = new DataTable();

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT P.INV_PRODUCTOS, P.NOMBRE, P.CODIGO, P.CAT_CATEGORIAS, P.ESTADO  "
                + " FROM INV_PRODUCTOS P "
                + " WHERE P.NOMBRE LIKE " + " '" + query + "%'"
                + " OR P.CODIGO LIKE "     + " '" + query + "%'";
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