using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Módulo_ProductosLocales
{
    public class ControladoraBDProductosLocales : ControladoraBD
    {
        public EntidadProductoLocal consultarProductoLocal(String id)
        {
            DataTable resultado = new DataTable();
            EntidadProductoLocal producConsultado = null;
            Object[] datosConsultados = new Object[3];

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM INV_ACTIVIDAD WHERE INV_PRODUCTOS = '" + id + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

                if (resultado.Rows.Count == 1)
                {
                    datosConsultados[0] = id;
                    for (int i = 1; i < 3; i++)
                    {
                        datosConsultados[i] = resultado.Rows[0][i].ToString();
                    }

                    producConsultado = new EntidadProductoLocal(datosConsultados);
                }
            }
            catch (Exception e) { }

            return producConsultado;
        }

        public string[] insertarProductoLocal(EntidadProductoLocal productoLocal)
        {
            String[] res = new String[4];
            res[3] = productoLocal.Codigo.ToString();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO INV_PRODUCTOS (NOMBRE, CAT_CATEGORIA, CAT_UNIDADES, CODIGO, CODIGO_BARRAS, " +
                " ESTACION, ESTADO, COSTO_COLONES, COSTO_DOLARES ) VALUES ('"
                + productoLocal.Nombre + "','" + productoLocal.Categoria + "','"
                + productoLocal.Unidades + "','" + productoLocal.Codigo + "','"
                + productoLocal.CodigoDeBarras + "','" + productoLocal.Estado + "','"
                + productoLocal.CostoColones + "','" + productoLocal.CostoDolares + "');";
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Producto Agregada";
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

        public string[] modificarProductoLocal(EntidadProductoLocal bodegaVieja, EntidadProductoLocal bodegaNueva)
        {
            throw new NotImplementedException();
        }

        public string[] desactivarProductoLocal(EntidadProductoLocal productoLocal)
        {
            String[] res = new String[4];
            res[3] = productoLocal.Codigo.ToString();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE INV_PRODUCTOS "
                                      + "SET ESTADO = " + productoLocal.Estado.ToString()
                                      + "WHERE INV_PRODUCTOS = " + productoLocal.Inv_Productos;
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

        internal static System.Data.DataTable consultarProductosLocales()
        {
            throw new NotImplementedException();
        }
    }
}