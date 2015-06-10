using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ProyectoInventarioOET.App_Code.Modulo_Ajustes;

namespace ProyectoInventarioOET.App_Code.Modulo_Traslados
{

    /*
     * Clase encargada de la comunicación entre el modulo de traslado y la Base de Datos.
     */
    public class ControladoraBDTraslado : ControladoraBD
    {


        /*
         * Método encargado de consultar todos los traslados de una bodega específica
         * con los datos listos para ser desplegados por la interfaz, notar que segun el tipo de traslado (entrada i salida)
         * el SQL cambia
         */
        public DataTable consultaTraslados(String idBodega, bool entrada)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String tipoConsulta = entrada ? "T.IDBODEGADESTINO = '" + idBodega + "'" : "T.IDBODEGAORIGEN = '" + idBodega + "'";

            try
            {
                // Interfaz ocupa 3 cosas TipoMovimiento(Descripcion), Fecha, Usuario(Encargado)
                // Se agrega el ID de los traslados para la consulta individual
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT T.ID_TRASLADO, T.FECHA, U.NOMBRE, T.IDBODEGAORIGEN, T.IDBODEGADESTINO, B1.DESCRIPCION, B2.DESCRIPCION, T.ESTADO  "
                   + " FROM " + esquema + "TRASLADOS T, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_BODEGA B1, " + esquema + "CAT_BODEGA B2 "
                   + " WHERE T.USUARIO_BODEGA = U.SEG_USUARIO "
                   + " AND " + tipoConsulta
                   + " AND  T.IDBODEGAORIGEN = B1.CAT_BODEGA" 
                   + " AND T.IDBODEGADESTINO = B2.CAT_BODEGA  ORDER BY T.FECHA DESC"; 
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
         * Método encargado de consultar un traslado especifico de una bodega específica
         * los datos incluyen los datos totales del traslado incluyendo los datos desplegables de los productos trasladados 
         */       
        public DataTable[] consultarTraslado(String idTraslado)
        {
            String esquema = "Inventarios.";
            DataTable[] resultado = new DataTable[2];
            resultado[0] = new DataTable();
            resultado[1] = new DataTable();

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT T.NOTAS, T.FECHA, U.NOMBRE, B1.DESCRIPCION, B2.DESCRIPCION, T.ESTADO  "
                   + " FROM " + esquema + "TRASLADOS T, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_BODEGA B1, " + esquema + "CAT_BODEGA B2 "
                   + " WHERE T.USUARIO_BODEGA = U.SEG_USUARIO "
                   + " AND  T.IDBODEGAORIGEN = B1.CAT_BODEGA"
                   + " AND T.IDBODEGADESTINO = B2.CAT_BODEGA"
                   + " AND T.ID_TRASLADO = '" + idTraslado + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado[0].Load(reader);
                resultado[1] = consultarDetalles(idTraslado);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }

        /*
         * Método auxiliar encargado de consultar los datos desplegables (por ejemplo el nombre del producto en lugar de su ID) 
         * de los productos trasladados de un ajuste específico
         */
        private DataTable consultarDetalles(String idTraslado)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT P.NOMBRE, P.CODIGO, D.TRASLADO, U.DESCRIPCION, D.INV_BODEGA_PRODUCTOSORIGEN, D.INV_BODEGA_PRODUCTOSDESTINO "
                   + " FROM " + esquema + "DETALLES_TRASLADO D, " + esquema + "INV_BODEGA_PRODUCTOS B, " + esquema + "INV_PRODUCTOS P, " + esquema + "CAT_UNIDADES U "
                   + " WHERE D.ID_TRASLADO = '" + idTraslado + "' "
                   + " AND D.INV_BODEGA_PRODUCTOSORIGEN = B.INV_BODEGA_PRODUCTOS "
                   + " AND B.INV_PRODUCTOS = P.INV_PRODUCTOS "
                   + " AND P.CAT_UNIDADES = U.CAT_UNIDADES ";
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
         * Método encargado de insertar los datos ingresados por el usuario, notar que los datos entrantes vienen encapsulados para mayor facilidad para la 
         * construcción del String SQL
         */
        public String[] insertarTraslado(EntidadTraslado nuevo)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];
            res[3] = generarID();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO " + esquema + "TRASLADOS (ID_TRASLADO, FECHA, USUARIO_BODEGA, IDBODEGAORIGEN, IDBODEGADESTINO, ESTADO, NOTAS) "
               + "VALUES ('" + res[3] + "', TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',  'dd/mm/yyyy hh24:mi:ss') , '"
               + nuevo.IdUsuario + "','" + nuevo.IdBodegaOrigen + "' , '" + nuevo.IdBodegaDestino + "' , " + 0 + ", '" + nuevo.Notas+ "' )";
                OracleDataReader reader = command.ExecuteReader();

                foreach (EntidadDetalles detallesProducto in nuevo.Detalles)
                { // Por cada producto ingresarlo en el detalles traslado
                    insertarDetalle(res[3], detallesProducto);
                    congelarProducto(detallesProducto.IdProductoBodegaOrigen, detallesProducto.Cambio);
                }

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Traslado en trámite.";
            }
            catch (OracleException e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Traslado no efectuado, revise e intente nuevamente.";
            }

            return res;
        }


        /* Método encargado de "congelar" los productos del inventario local añadiendolos en la columna congelados para que su existencia 
        * no este disponible. 
        */
        private void congelarProducto(String idProductoBodega, double traslado)
        {
            String esquema = "Inventarios.";
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = " UPDATE " + esquema + "INV_BODEGA_PRODUCTOS "
                                     + " SET SALDOCONGELADO = SALDOCONGELADO + " + traslado + " , "
                                     + " SALDO = SALDO - " + traslado
                                     + " WHERE INV_BODEGA_PRODUCTOS = '" + idProductoBodega+ "'";  
                OracleDataReader reader = command.ExecuteReader();
            }
            catch (OracleException e) {

            }
        }


        /* Método encargado de "descongelar" los productos del inventario local moviendolos de la columna congelados a la columna de existencia para que su existencia 
        * este disponible. Dicha disponibilidad dependera de si se trata de aceptar un traslado (los productos se agregan a la bodega destino) 
        * o rechazarlo (los productos se retornan a la bodega origen)  
        */
        private void desCongelarProducto(String idProductoBodegaOrigen, String idProductoBodegaDestino, double traslado, int aceptarTraslado)
        {
            String esquema = "Inventarios.";  
            try
            {
                if (aceptarTraslado>0)
                {
                    OracleCommand command = conexionBD.CreateCommand();
                    // Descongelar el Producto en la Bodega ORIGEN
                    command.CommandText = " UPDATE " + esquema + "INV_BODEGA_PRODUCTOS "
                                         + " SET SALDOCONGELADO = SALDOCONGELADO - " + traslado
                                         + " WHERE INV_BODEGA_PRODUCTOS = '" + idProductoBodegaOrigen + "'";
                    OracleDataReader reader = command.ExecuteReader();
                    // SUMAR los productos a la Bogega DESTINO pues el traslado se completo!!
                    command.CommandText = " UPDATE " + esquema + "INV_BODEGA_PRODUCTOS "
                                        + " SET SALDO = SALDO + " + traslado
                                        + " WHERE INV_BODEGA_PRODUCTOS = '" + idProductoBodegaDestino + "'";
                    reader = command.ExecuteReader();
                }
                else {  // CASO DE RECHAZAR TRASLADO
                    // Descongelar el Producto en la Bodega ORIGEN
                    OracleCommand command = conexionBD.CreateCommand();
                    command.CommandText = " UPDATE " + esquema + "INV_BODEGA_PRODUCTOS "
                                         + " SET SALDOCONGELADO = SALDOCONGELADO - " + traslado
                                         + " WHERE INV_BODEGA_PRODUCTOS = '" + idProductoBodegaOrigen + "'";
                    OracleDataReader reader = command.ExecuteReader();
                    // Devolver los productos a la Bogega Origen pues no se lograron trasladar
                    command.CommandText = " UPDATE " + esquema + "INV_BODEGA_PRODUCTOS "
                                        + " SET SALDO = SALDO + " + traslado
                                        + " WHERE INV_BODEGA_PRODUCTOS = '" + idProductoBodegaOrigen + "'";
                    reader = command.ExecuteReader();
                }
            }
            catch (OracleException e)  {             }
        }

        /*
         * Método auxiliar encargado de insertar los productos traslados y su respectiva cantidad en la 
         *  tabla detalles de traslado producto de la relación N:M con la tabla de traslados 
         */
        private void insertarDetalle(String idTraslado, EntidadDetalles detallesProducto)
        {
            String esquema = "Inventarios.";
            String[] res = new String[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = " INSERT INTO " + esquema + "DETALLES_TRASLADO "
                                     + " VALUES ('" + idTraslado +"', '" + detallesProducto.IdProductoBodegaDestino + "', '"+ detallesProducto.IdProductoBodegaOrigen +"' , " +detallesProducto.Cambio+ ")";
                OracleDataReader reader = command.ExecuteReader();
                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Ajuste agregado al sistema.";
            }
            catch (OracleException e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Error en traslado, intente nuevamente.";
            }
        }


        /*
         * Método encargado de hacer o delegar todos los cambios necesarios cuando se acepta o rechaza un traslado 
         */
        public String[] modificarTraslado(EntidadTraslado traslado, int estado) {
            String esquema = "Inventarios.";
            String[] res = new String[3];
            DataTable productoLocal = new DataTable();
            bool alerta = false;
            double cantAjuste;
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = " UPDATE " + esquema + "TRASLADOS "
                                     + " SET ESTADO = " + estado 
                                     + " WHERE ID_TRASLADO = '" + traslado.IdTraslado + "'" ;
                OracleDataReader reader = command.ExecuteReader();
                foreach (EntidadDetalles detalle in traslado.Detalles){
                    desCongelarProducto(detalle.IdProductoBodegaOrigen, detalle.IdProductoBodegaDestino, detalle.Cambio, estado);
                    if (estado == 1)
                    { // acepta
                        command.CommandText = "SELECT MINIMO,MAXIMO,SALDO FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS = '" + detalle.IdProductoBodegaDestino + "'";
                    }
                    else
                    { //rechaza
                        command.CommandText = "SELECT MINIMO,MAXIMO,SALDO FROM " + esquema + "INV_BODEGA_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS = '" + detalle.IdProductoBodegaOrigen + "'";
                    }
                    reader = command.ExecuteReader();
                    productoLocal.Load(reader);
                    cantAjuste = Convert.ToDouble(productoLocal.Rows[0][0].ToString());
                    if (estado == 1) { cantAjuste += detalle.Cambio; } else { cantAjuste -= detalle.Cambio; }
                    alerta |= cantAjuste <= Convert.ToDouble(productoLocal.Rows[0][0].ToString()) || cantAjuste >= Convert.ToDouble(productoLocal.Rows[0][1].ToString());
                }
                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Transacción efectuada.";
                if (alerta)
                {
                    res[0] = "warning";
                    res[2] += "\nUno o más productos han salido de sus límites permitidos (nivel máximo o mínimo), revise el catálogo local.";
                }
            }
            catch (OracleException e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Traslado no modificado, intente nuevamente.";
            }
            return res;
        }


    }
}