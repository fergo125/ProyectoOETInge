using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Módulo_Productos_Locales
{
    /*
     * ???
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDProductosLocales : ControladoraBD
    {
        /*public EntidadProductoLocal consultarProductoLocal(String id)
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

        internal static System.Data.DataTable consultarProductosLocales()
        {
            throw new NotImplementedException();
        }*/

        /*
         * Consulta los productos que pertenecen a una bodega en específico.
         */
        public DataTable consultarProductosDeBodega(String idBodega)
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT INV_PRODUCTOS.NOMBRE,INV_PRODUCTOS.CODIGO,INV_BODEGA_PRODUCTOS.SALDO,INV_BODEGA_PRODUCTOS.MINIMO,INV_BODEGA_PRODUCTOS.MAXIMO FROM INV_BODEGA_PRODUCTOS,INV_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS.INV_PRODUCTOS = INV_PRODUCTOS.INV_PRODUCTOS AND INV_BODEGA_PRODUCTOS.CAT_BODEGA = '" + idBodega + "'";
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
         * Consulta un producto en específico perteneciente a la bodega especificada. 
         */
        public DataTable consultarProductoDeBodega(String idBodega, String idProducto)
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                // nombre, codigo interno, codigo de barras, categoria, intencion, unidades metricas, estado local, existencia, impuesto, precio col, precio dol
                // costo col, costo dol, min, max, creador, creado, modifica, modificado, costo ult col, costo ult dol, idproveedor ult
                command.CommandText = "SELECT INV_PRODUCTOS.NOMBRE,INV_PRODUCTOS.CODIGO,INV_PRODUCTOS.CODIGO_BARRAS,INV_PRODUCTOS.CAT_CATEGORIAS,INV_PRODUCTOS.INTENCION,INV_PRODUCTOS.CAT_UNIDADES,INV_BODEGA_PRODUCTOS.ESTADO,INV_BODEGA_PRODUCTOS.SALDO,INV_PRODUCTOS.IMPUESTO,INV_PRODUCTOS.PRECIO_C,INV_PRODUCTOS.PRECIO_D,INV_BODEGA_PRODUCTOS.COSTO_COLONES,INV_BODEGA_PRODUCTOS.COSTO_DOLARES,INV_BODEGA_PRODUCTOS.MINIMO,INV_BODEGA_PRODUCTOS.MAXIMO,INV_BODEGA_PRODUCTOS.CREA,INV_BODEGA_PRODUCTOS.CREADO,INV_BODEGA_PRODUCTOS.MODIFICA,INV_BODEGA_PRODUCTOS.MODIFICADO,INV_BODEGA_PRODUCTOS.COSTO_ULTIMA_COMPRA_C,INV_BODEGA_PRODUCTOS.COSTO_ULTIMA_COMPRA_D,INV_BODEGA_PRODUCTOS.IDPROVEEDOR_UC,INV_BODEGA_PRODUCTOS.INV_BODEGA_PRODUCTOS FROM INV_BODEGA_PRODUCTOS,INV_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS.INV_PRODUCTOS = INV_PRODUCTOS.INV_PRODUCTOS AND INV_BODEGA_PRODUCTOS.CAT_BODEGA = '" + idBodega + "' AND INV_PRODUCTOS.CODIGO = '" + idProducto + "'";
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
         * Modifica el estado de un producto en específico.
         */
        public string[] modificarProductoLocal(String idBodegaProductos, int estado)
        {
            DataTable resultado = new DataTable();
            String[] res = new String[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE INV_BODEGA_PRODUCTOS SET ESTADO = " + estado + " WHERE INV_BODEGA_PRODUCTOS = '" + idBodegaProductos + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Producto modificado localmente.";
            }
            catch (Exception e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Producto no modificado, intente nuevamente.";
            }
            return res;

        }
        /*
         * Asocia el producto especificado con la bodega especificada. 
         */
        public string[] asociarProductos(String idBodega,String idProducto,String idUsuario)
        {
            String[] res = new String[4];
            res[3] = generarID();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                String aux = "INSERT INTO INV_BODEGA_PRODUCTOS ( INV_BODEGA_PRODUCTOS,CAT_BODEGA, INV_PRODUCTOS,CREA,CREADO,ESTADO ) VALUES ( ' "
                + generarID() + "' , '" + idBodega + "' , '"
                + idProducto + "' , '" + idUsuario + "' , '"
                + DateTime.Now.ToString("dd:MMM:yy") +"' , 1)";

                command.CommandText = aux;
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Producto/s asociado/s al catálogo de bodega.";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Producto/s no asociado, intente nuevamente.";
            }
            return res;
        }

    }
}