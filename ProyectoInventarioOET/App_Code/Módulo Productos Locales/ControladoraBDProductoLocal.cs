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
         * ???
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
         * ???
         */
        public DataTable consultarProductoDeBodega(String idBodega, String idProducto)
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                // nombre, codigo interno, codigo de barras, categoria, intencion, unidades metricas, estado local, existencia, impuesto, precio col, precio dol
                // costo col, costo dol, min, max, creador, creado, modifica, modificado, costo ult col, costo ult dol, idproveedor ult
                command.CommandText = "SELECT INV_PRODUCTOS.NOMBRE,INV_PRODUCTOS.CODIGO,INV_PRODUCTOS.CODIGO_BARRAS,INV_PRODUCTOS.CAT_CATEGORIAS,INV_PRODUCTOS.INTENCION,INV_PRODUCTOS.CAT_UNIDADES,INV_BODEGA_PRODUCTOS.ESTADO,INV_BODEGA_PRODUCTOS.SALDO,INV_PRODUCTOS.IMPUESTO,INV_PRODUCTOS.PRECIO_C,INV_PRODUCTOS.PRECIO_D,INV_BODEGA_PRODUCTOS.COSTO_COLONES,INV_BODEGA_PRODUCTOS.COSTO_DOLARES,INV_BODEGA_PRODUCTOS.MINIMO,INV_BODEGA_PRODUCTOS.MAXIMO,INV_BODEGA_PRODUCTOS.CREA,INV_BODEGA_PRODUCTOS.CREADO,INV_BODEGA_PRODUCTOS.MODIFICA,INV_BODEGA_PRODUCTOS.MODIFICADO,INV_BODEGA_PRODUCTOS.COSTO_ULTIMA_COMPRA_C,INV_BODEGA_PRODUCTOS.COSTO_ULTIMA_COMPRA_D,INV_BODEGA_PRODUCTOS.IDPROVEEDOR_UC FROM INV_BODEGA_PRODUCTOS,INV_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS.INV_PRODUCTOS = INV_PRODUCTOS.INV_PRODUCTOS AND INV_BODEGA_PRODUCTOS.CAT_BODEGA = '" + idBodega + "' AND INV_PRODUCTOS.CODIGO = '" + idProducto + "'";
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