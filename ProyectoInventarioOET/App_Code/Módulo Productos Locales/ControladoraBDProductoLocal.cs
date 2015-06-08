using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Modulo_Productos_Locales
{
    /*
     * ???
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDProductosLocales : ControladoraBD
    {
        /*
         * Consulta los productos que pertenecen a una bodega en específico.
         */
        public DataTable consultarProductosDeBodega(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();  
                command.CommandText = "SELECT A.INV_PRODUCTOS, B.NOMBRE, B.CODIGO, A.SALDO, A.MINIMO, A.MAXIMO FROM " + esquema + "INV_BODEGA_PRODUCTOS A, " + esquema + "INV_PRODUCTOS B WHERE A.INV_PRODUCTOS = B.INV_PRODUCTOS AND A.CAT_BODEGA = '" + idBodega + "'";
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
         * Consulta los productos que pertenecen a una bodega en específico, especializado para el modulo ajustes.
         */
        public DataTable consultarProductosDeBodegaAjustes(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();  //Cambio Carlos
                command.CommandText = "SELECT A.INV_BODEGA_PRODUCTOS, B.NOMBRE, B.CODIGO, A.SALDO, A.MINIMO, A.MAXIMO, C.DESCRIPCION FROM " + esquema + "INV_BODEGA_PRODUCTOS A, " + esquema + "INV_PRODUCTOS B, " + esquema + "CAT_UNIDADES C WHERE A.INV_PRODUCTOS = B.INV_PRODUCTOS AND B.CAT_UNIDADES = C.CAT_UNIDADES AND A.CAT_BODEGA = '" + idBodega + "' AND A.ESTADO = 1";
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
        public DataTable consultarProductoDeBodega(String idBodega, String codigoProducto)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                // nombre, codigo interno, codigo de barras, categoria, intencion, unidades metricas, estado local, existencia, impuesto, precio col, precio dol
                // costo col, costo dol, min, max, creador, creado, modifica, modificado, costo ult col, costo ult dol, idproveedor ult
                command.CommandText = "SELECT B.NOMBRE, B.CODIGO, B.CODIGO_BARRAS, B.CAT_CATEGORIAS, B.INTENCION, B.CAT_UNIDADES, A.ESTADO, A.SALDO, B.IMPUESTO, B.PRECIO_C, B.PRECIO_D, "
                    + "A.COSTO_COLONES, A.COSTO_DOLARES, A.MINIMO, A.MAXIMO, A.CREA, A.CREADO, A.MODIFICA, A.MODIFICADO, A.COSTO_ULTIMA_COMPRA_C, A.COSTO_ULTIMA_COMPRA_D, A.IDPROVEEDOR_UC, "
                    + "A.INV_BODEGA_PRODUCTOS FROM " + esquema + "INV_BODEGA_PRODUCTOS A, " + esquema + "INV_PRODUCTOS B WHERE A.INV_PRODUCTOS = B.INV_PRODUCTOS AND A.CAT_BODEGA = '" + idBodega + "' AND B.CODIGO = '" + codigoProducto + "'";
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
        public string[] modificarProductoLocal(String idBodegaProductos, int estado,String min,String max)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String[] res = new String[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE " + esquema + "INV_BODEGA_PRODUCTOS SET ESTADO = " + estado + ", MINIMO = " + min + ", MAXIMO = "+max+" WHERE INV_BODEGA_PRODUCTOS = '" + idBodegaProductos + "'";
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
            String esquema = "Inventarios.";
            String[] res = new String[4];
            res[3] = generarID();
            try
            {
                DataTable resultado = new DataTable();
                OracleCommand command = conexionBD.CreateCommand();
                String aux = "INSERT INTO " + esquema + "INV_BODEGA_PRODUCTOS ( INV_BODEGA_PRODUCTOS,CAT_BODEGA, INV_PRODUCTOS,CREA,CREADO,ESTADO ) VALUES ( ' "
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

        public DataTable consultarProductosDeBodega(string idBodegaOrigen, string idBodegaDestino)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();  //Cambio Carlos
                command.CommandText = " SELECT B1.INV_PRODUCTOS, P.NOMBRE, P.CODIGO, B1.SALDO, B1.MINIMO, B1.MAXIMO, B1.INV_BODEGA_PRODUCTOS, B2.INV_BODEGA_PRODUCTOS, C.DESCRIPCION "
                                     + " FROM " + esquema + "INV_BODEGA_PRODUCTOS B1, " + esquema + "INV_BODEGA_PRODUCTOS B2, " + esquema + "INV_PRODUCTOS P, " + esquema + "CAT_UNIDADES C "
                                     + " WHERE B1.CAT_BODEGA = '" + idBodegaOrigen + "' "
                                     + " AND B2.CAT_BODEGA = '" + idBodegaDestino + "' "
                                     + " AND B1.INV_PRODUCTOS = B2.INV_PRODUCTOS "
                                     + " AND B1.INV_PRODUCTOS = P.INV_PRODUCTOS "
                                     + " AND B2.INV_PRODUCTOS = P.INV_PRODUCTOS "
                                     + " AND P.CAT_UNIDADES = C.CAT_UNIDADES "
                                     + " AND B1.SALDO > 0 ";   
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
         * Consulta el maximo y el minimo para un producto en una bodega especifica
         */
        public DataTable consultarMinimoMaximoProductoEnBodega(String idProductoEnBodega)
        {
            DataTable resultado = new DataTable();
            String esquema = "Inventarios.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT MINIMO,MAXIMO,SALDO FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS = '" + idProductoEnBodega + "'";
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