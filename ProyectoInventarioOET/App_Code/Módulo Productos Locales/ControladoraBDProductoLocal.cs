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

        internal static System.Data.DataTable consultarProductosLocales()
        {
            throw new NotImplementedException();
        }
    }
}