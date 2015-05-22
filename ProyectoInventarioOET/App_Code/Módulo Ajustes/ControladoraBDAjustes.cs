using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{
    public class ControladoraBDAjustes : ControladoraBD
    {

        public string[] insertarAjuste(EntidadAjuste ajuste)
        {
            String[] res = new String[4]; // Vector que contiene la información sobre el resultado de la transacción en la base de datos
            res[3] = generarID();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                String aux = "INSERT INTO INV_PRODUCTOS ( NOMBRE, CODIGO, CODIGO_BARRAS, CAT_CATEGORIAS, INTENCION, CAT_UNIDADES, ESTADO,  "
                + "SALDO, IMPUESTO, PRECIO_C, PRECIO_D, COSTO_COLONES , COSTO_DOLARES , INV_PRODUCTOS, CREA, CREADO ) VALUES ( '"
                + productoGlobal.Nombre + "' , '" + productoGlobal.Codigo + "' , '"
                + productoGlobal.CodigoDeBarras + "' , '" + productoGlobal.Categoria + "' , '"
                + productoGlobal.Intencion + "' , '" + productoGlobal.Unidades + "' , "
                + (short)productoGlobal.Estado + " , " + productoGlobal.Existencia + " , "
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





    }
}