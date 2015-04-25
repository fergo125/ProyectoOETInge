using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.Módulo_Productos_Locales
{
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

        public DataTable consultarProductosDeBodega(String idBodega)
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT INV_PRODUCTOS.NOMBRE,INV_BODEGA_PRODUCTOS.COSTO_COLONES,INV_BODEGA_PRODUCTOS.SALDO,INV_BODEGA_PRODUCTOS.MINIMO,INV_BODEGA_PRODUCTOS.MAXIMO FROM INV_BODEGA_PRODUCTOS,INV_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS.INV_PRODUCTOS = INV_PRODUCTOS.INV_PRODUCTOS AND INV_BODEGA_PRODUCTOS.CAT_BODEGA = '" + idBodega + "'";
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