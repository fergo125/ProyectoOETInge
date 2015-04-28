using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales
{
    public class ControladoraBDProductosGlobales : ControladoraBD
    {
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

        public string[] insertarProductoGlobal(EntidadProductoGlobal productoGlobal)
        {
            String[] res = new String[4];
            res[3] = generarID();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                String aux =  "INSERT INTO INV_PRODUCTOS ( NOMBRE, CODIGO, CODIGO_BARRAS, CAT_CATEGORIAS, INTENCION, CAT_UNIDADES, ESTADO,  "
                + "SALDO, IMPUESTO, PRECIO_C, PRECIO_D, COSTO_COLONES , COSTO_DOLARES , INV_PRODUCTOS ) VALUES ( '"
                + productoGlobal.Nombre + "' , '" + productoGlobal.Codigo + "' , '"
                + productoGlobal.CodigoDeBarras + "' , '" + productoGlobal.Categoria + "' , '"
                + productoGlobal.Intencion + "' , '" + productoGlobal.Unidades + "' , "
                + (short) productoGlobal.Estado + " , " + productoGlobal.Existencia + " , "
                + productoGlobal.Impuesto + " , " + productoGlobal.PrecioColones + " , "
                + productoGlobal.PrecioDolares + " , " + productoGlobal.CostoColones + " , "
                + productoGlobal.CostoDolares + " , '" + res[3] + "' ) ";


                command.CommandText = aux; 
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Producto Agregado";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Fallo en la operacion";
                res[2] = "Intente nuevamente";
            }
            return res;
        }

        public string[] modificarProductoGlobal(EntidadProductoGlobal productoGlobalViejo, EntidadProductoGlobal productoGlobalNuevo)
        {
            String[] res = new String[4];
            res[3] = productoGlobalViejo.Inv_Productos.ToString();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                String aux =        "UPDATE INV_PRODUCTOS "
                                    + "SET CAT_CATEGORIA = '" + productoGlobalNuevo.Categoria.ToString() + "' , "
                                    + "SET CODIGO = '" + productoGlobalNuevo.Codigo.ToString() + "' , "
                                    + "SET CODIGO_BARRAS = '" + productoGlobalNuevo.CodigoDeBarras.ToString() + "' , "
                                    + "SET COSTO_COLONES = " + productoGlobalNuevo.CostoColones.ToString() + " , "
                                    + "SET COSTO_DOLARES = " + productoGlobalNuevo.CostoDolares.ToString() +  " , "
                                    + "SET ESTADO = " + productoGlobalNuevo.Estado.ToString() + " , "
                                    + "SET SALDO = " + productoGlobalNuevo.Existencia.ToString() + " , "
                                    + "SET IMPUESTO = " + productoGlobalNuevo.Impuesto.ToString() + " , "
                                    + "SET INTENCION = '" + productoGlobalNuevo.Intencion.ToString() + "' , "
                                    + "SET NOMBRE = '" + productoGlobalNuevo.Nombre.ToString() + "' , "
                                    + "SET PRECIO_COLONES = " + productoGlobalNuevo.PrecioColones.ToString() + " , "
                                    + "SET PRECIO_DOLARES = " + productoGlobalNuevo.PrecioDolares.ToString() + " , "
                                    + "SET CAT_UNIDADES = '" + productoGlobalNuevo.Unidades.ToString() + "' "
                                    + "WHERE INV_PRODUCTOS = " + productoGlobalViejo.Inv_Productos;


                command.CommandText = aux;
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Producto modificado";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Fallo en la operacion";
                res[2] = "Intente nuevamente";
            }
            return res;
        }

        public string[] desactivarProductoGlobal(EntidadProductoGlobal productoGlobal)
        {
            String[] res = new String[4];
            res[3] = productoGlobal.Codigo.ToString();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText =   "UPDATE INV_PRODUCTOS "
                                      + "SET ESTADO = " + productoGlobal.Estado.ToString()
                                      + "WHERE INV_PRODUCTOS = " + productoGlobal.Inv_Productos;
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Producto Desactivado";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Fallo en la operacion";
                res[2] = "Intente nuevamente";
            }
            return res;
        }

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
         * Método encargado de retornar los productos globales que concuerdan con la busqueda del usuario
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