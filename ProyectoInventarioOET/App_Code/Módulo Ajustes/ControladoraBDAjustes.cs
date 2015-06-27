using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Modulo_Ajustes
{
    /*
     * Clase encargada de la comunicación entre el modulo de ajustes y la Base de Datos.
     */
    public class ControladoraBDAjustes : ControladoraBD
    {

        /*
         * Método encargado de obtener los tipos de ajustes que se pueden realizar en el 
         * inventario
         */
        public DataTable tiposAjuste()
        {
            String esquema = "Inventarios.";
            DataTable resultado = null;
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_TIPO_MOVIMIENTO WHERE CAT_TIPO_MOVIMIENTO <> 'CYCLO106062012145550408008'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }


        /*
         * Método encargado de consultar todos los ajustes de una bodega específica
         * con los datos listos para ser desplegados por la interfaz
         */
        public DataTable consultarAjustes(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = null;
            // Interfaz ocupa 3 cosas TipoMovimiento(Descripcion), Fecha, Usuario(Encargado)
            // Se trae el ID de ajustes para la consulta individual
            String comandoSQL = "SELECT AJ.ID_AJUSTES, M.DESCRIPCION, AJ.FECHA, U.NOMBRE"
                + " FROM " + esquema + "AJUSTES AJ, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_TIPO_MOVIMIENTO M"
                + " WHERE AJ.USUARIO_BODEGA = U.SEG_USUARIO "
                + " AND AJ.CAT_TIPO_MOVIMIENTO = M.CAT_TIPO_MOVIMIENTO"
                + " AND AJ.IDBODEGA = '" + idBodega + "' ORDER BY AJ.FECHA DESC";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }


        /*
         * Método encargado de consultar un ajuste específico basado en su ID
         * con los datos listos para ser desplegados por la interfaz
         */
        public DataTable[] consultarAjuste(String idAjuste)
        {
            String esquema = "Inventarios.";
            DataTable[] resultado = new DataTable[2];
            String comandoSQL = "SELECT M.CAT_TIPO_MOVIMIENTO, AJ.FECHA, U.NOMBRE, U.SEG_USUARIO, AJ.NOTAS ,AJ.IDBODEGA, M.DESCRIPCION  "
                + " FROM " + esquema + "AJUSTES AJ, " + esquema + "SEG_USUARIO U, " + esquema + "CAT_TIPO_MOVIMIENTO M"
                + " WHERE AJ.USUARIO_BODEGA = U.SEG_USUARIO "
                + " AND AJ.CAT_TIPO_MOVIMIENTO = M.CAT_TIPO_MOVIMIENTO"
                + " AND AJ.ID_AJUSTES = '" + idAjuste + "' ";
            resultado[0] = ejecutarComandoSQL(comandoSQL, true);
            resultado[1] = consultarDetalles(idAjuste);
            return resultado;
        }

        /*
         * Método auxiliar de consultar los detalles de un ajuste específico
         * con los datos listos para ser desplegados por la interfaz. Con detalles nos referimos a los productos 
         * a los cuales se les ajusto su existencia. Este método es llamado en el consultarAjuste ya que dicho método devuelve 
         * toda la informacion de un ajuste.
         */
        private DataTable consultarDetalles(String idAjuste)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT P.NOMBRE, P.CODIGO, D.CANTIDADPREVIA, D.CANTIDADNUEVA, B.INV_BODEGA_PRODUCTOS, B.SALDO, U.DESCRIPCION "
            //String comandoSQL = "SELECT P.NOMBRE, P.CODIGO, D.CANTIDADNUEVA - D.CANTIDADPREVIA, B.INV_BODEGA_PRODUCTOS, B.SALDO, U.DESCRIPCION "
                + " FROM " + esquema + "DETALLES_AJUSTES D, " + esquema + "INV_BODEGA_PRODUCTOS B, " + esquema + "INV_PRODUCTOS P, " + esquema + "CAT_UNIDADES U "
                + " WHERE D.ID_AJUSTES = '" + idAjuste + "' "
                + " AND D.INV_BODEGA_PRODUCTOS = B.INV_BODEGA_PRODUCTOS "
                + " AND B.INV_PRODUCTOS = P.INV_PRODUCTOS "
                + " AND P.CAT_UNIDADES = U.CAT_UNIDADES ";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }


        /*
         * Método encargado de insertar un ajustes con los datos provenientes de la Entidad encapsulada en la interfaz,
         * este método genera el ID e introduce la fecha actual.
         */
        public String[] insertarAjuste(EntidadAjustes ajuste)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[4];
            resultado[3] = generarID();
            String comandoSQL = "INSERT INTO " + esquema + 
                "AJUSTES (ID_AJUSTES, CAT_TIPO_MOVIMIENTO, FECHA, USUARIO_BODEGA, IDBODEGA, NOTAS, ANULABLE) VALUES ('"
                + resultado[3] + "','" + ajuste.IdTipoAjuste + "', TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',  'dd/mm/yyyy hh24:mi:ss') , '"
                + ajuste.IdUsuario  + "','" + ajuste.IdBodega + "' , '" + ajuste.Notas + "', " + 1 + ")";
            if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                foreach(EntidadDetalles detallesProducto in  ajuste.Detalles) // Por cada producto meterlo en el detalles ajustes
                {
                    actualizarProducto(detallesProducto.IdProductoBodega, detallesProducto.CantidadNueva);
                    insertarDetalle(resultado[3], detallesProducto);
                    //actualizarProducto(detallesProducto.IdProductoBodega, detallesProducto.CantidadNueva);
                }
                resultado[0] = "success";
                resultado[1] = "Éxito:";
                resultado[2] = "Ajuste agregado al sistema.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error:";
                resultado[2] = "Ajuste no agregado, intente nuevamente.";
            }
            return resultado;
        }

        /*
         * Método auxiliar que se encarga de insertar en la tabla detalles de ajuste 
         * (que son los ajustes individuales correspondientes a cada producto) esta tabla es producto de la relacion
         * NM entre productos locales y ajustes
         */
        private void insertarDetalle(String idAjuste, EntidadDetalles detallesProducto)
        {
            String esquema = "Inventarios.";
            String comandoSQL = "INSERT INTO " + esquema + "DETALLES_AJUSTES (ID_AJUSTES, INV_BODEGA_PRODUCTOS, CANTIDADPREVIA, CANTIDADNUEVA) VALUES ('" + idAjuste
                + "','" + detallesProducto.IdProductoBodega + "', " + detallesProducto.CantidadPrevia + ", " + detallesProducto.CantidadNueva + " )";

           // String comandoSQL = "INSERT INTO " + esquema + "DETALLES_AJUSTES (ID_AJUSTES, INV_BODEGA_PRODUCTOS, CANTIDAD_PREVIA, CANTIDAD_NUEVA) VALUES ('" + idAjuste
            // + "','" + detallesProducto.IdProductoBodega + "', " + detallesProducto.CantidadPrevia + "', " + detallesProducto.CantidadNueva + " )";

            ejecutarComandoSQL(comandoSQL, false);
        }

        /*
         * Método auxiliar que se encarga de actualizar la existencia del productos en el catalogo local
         */ 
        private void actualizarProducto(String idBodegaProducto, double nuevaCantidad)
        {
            String esquema = "Inventarios.";
            String comandoSQL = "UPDATE " + esquema + "INV_BODEGA_PRODUCTOS "
                                   + " SET SALDO = " + nuevaCantidad + " , "
                                   + " MODIFICADO =  TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',  'dd/mm/yyyy hh24:mi:ss') "
                                   + " WHERE INV_BODEGA_PRODUCTOS = '" + idBodegaProducto + "'";
            ejecutarComandoSQL(comandoSQL, false);
        }

        public void anularAjuste (EntidadAjustes ajuste, String idAjuste)
        {
            String esquema = "Inventarios.";
            String comandoSQL = "UPDATE " + esquema + "AJUSTE "
                                   + " SET ANULABLE = " + 0  
                                   + " WHERE ID_AJUSTE = '" + idAjuste + "'";
            ejecutarComandoSQL(comandoSQL, false);
        }
    }
}
