using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Modulo_Entradas
{
    public class ControladoraBDEntradas : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDEntradas()
        {
        }
        /*
-        * Recibe un id de bodega y devuelve las entradas para esa bodega especifica
-        */
        public DataTable consultarEntradasDeBodega(string bodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_ENTRADAS WHERE CAT_BODEGA = '" + bodega + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
-        * Si en el ID se pasa por parametro todas, el sistema devuelve todas las facturas en la base,
-        * pero si recibe por paramentro un id o el inicio de un id, tratara de buscar todas las facturas que sean similares
-        */
        public DataTable buscarFacturas(string id)
        {
            DataTable resultado = new DataTable();
            String esquema1 = "Compras.";
            String esquema2 = "Inventarios.";
            String comandoSQL = "";
            if ("Todas".Equals(id))
                comandoSQL = "SELECT *"
                    + "FROM " + esquema1 + "facturas full outer join " + esquema2 + "cat_entradas on " + esquema1 + "facturas.idfactura = " + esquema2 + "cat_entradas.factura"
                    + " where " + esquema2 + "cat_entradas.cat_entradas is null";
            else
                comandoSQL = "SELECT *"
                    + "FROM " + esquema1 + "facturas full outer join " + esquema2 + "cat_entradas on " + esquema1 + "facturas.idfactura = " + esquema2 + "cat_entradas.factura"
                    + " where" + esquema2 + "cat_entradas.cat_entradas is null and " + esquema1 + "facturas.idfactura" + " like '" + id + "%'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

         /*
-         * Recibe el id de una factura y devuelve el detalle de la misma 
-         */
        public DataTable consultarDetalleFactura(String idFactura)
        {
            DataTable resultado = new DataTable();
            String ordenDeCompra = "";
            String esquema = "Compras.";
            String comandoSQL = "SELECT IDORDENDECOMPRA "
            + "FROM " + esquema + "FACTURAS_CON_OC"
            + " WHERE IDFACTURA = '" + idFactura + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);


            if (resultado.Rows.Count > 0)
            {
                foreach (DataRow fila in resultado.Rows)
                {
                    ordenDeCompra = fila[0].ToString();
                }

                resultado = new DataTable();
                String comandoSQL2 = "SELECT *  "
                    + " FROM " + esquema + "PRODUCTO_ORDENADOS "
                    + " WHERE IDORDENDECOMPRA= " + " '" + ordenDeCompra + "'";
                resultado = ejecutarComandoSQL(comandoSQL2, true);
            }

            return resultado;
        }

        /*
-        * Recibe el id de una factura y devuelve los datos de esa factura.
-        */
        public DataTable consultarFactura(String id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Compras.";
            String comandoSQL = "SELECT *  "
                + " FROM " + esquema + "FACTURAS "
                + " WHERE IDFACTURA =" + " '" + id + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
-        * Recibe el id de una entrada y devuelve sus datos.
-        */
        public DataTable consultarEntrada(String id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Inventarios.";
            String comandoSQL = "SELECT *  "
                + " FROM " + esquema + "CAT_ENTRADAS"
                + " WHERE CAT_ENTRADAS =" + " '" + id + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Recibe el id de una entrada de inventario y devuelve los productos asociados a esa 
         * entrada.
         */
        public DataTable consultarProductosEntrada(string id)
        {
            DataTable resultado = new DataTable();
            String esquema = "Inventarios.";
            String comandoSQL = "select A.NOMBRE, A.CANTIDAD, A.COSTO_TOTAL, A.COSTO_UNITARIO, A.DESCUENTO, A.GRAVADO" +
                "from (select * from " + esquema + "CAT_ENTRADAS_PRODUCTOS entradasProductos, " + esquema + "INV_PRODUCTOS productos where entradasProductos.cat_productos = productos.Codigo ) A" +
                " WHERE CAT_ENTRADAS" + "= '" + id + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;

//select A.NOMBRE, A.CANTIDAD, A.COSTO_TOTAL, 
//A.COSTO_UNITARIO, A.DESCUENTO, A.GRAVADO
//from (select * from Inventarios.CAT_ENTRADAS_PRODUCTOS entradasProductos, Inventarios.INV_PRODUCTOS productos where entradasProductos.cat_productos = productos.Codigo ) A;

        }

        /*
         * Recibe la informacion de una entrada y los productos asociados y los ingresa a la
         * base de datos.
         */
        public String[] insertarEntrada(EntidadEntrada entrada, DataTable productosAsociados)
        {
            String esquema = "Inventarios.";
            //bool existenteEnBD = false;
            int temp;
            String[] res = new String[4];
            entrada.IdEntrada= generarID();
            res[3] = entrada.IdEntrada;
            String comandoSQL = "insert into "+esquema+"cat_entradas values("+
                        "'"+ entrada.IdEntrada +"'"+
                        ",'" + entrada.IdFactura + "'"+
                        ",'" + entrada.IdEncargado+ "'" +
                        ",'" + entrada.Bodega+ "'" +
                        ",'" + entrada.FechEntrada.ToString("dd-MMM-yyy")+ "'" +
                        ",'" + entrada.TipoMoneda + "'" + 
                        ",'" + entrada.MetodoPago + "'"
                        +")";

            try {
                if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                {
                    if (productosAsociados.Rows.Count > 0)
                    {
                        foreach (DataRow fila in productosAsociados.Rows)
                        {
                            if (fila[4] == "No")
                                temp = 0;
                            else temp = 1;

                            comandoSQL = "insert into " + esquema + "cat_entradas_productos values(" +
                                "'" + generarID() + "'" +
                                ",'" + entrada.IdEntrada + "'" +
                                ",'" + fila[0] + "'" +
                                "," + fila[1] +
                                "," + fila[3] +
                                ",'" + fila[5] + "'" +
                                "," + fila[2] +
                                "," + temp
                                + ")";
                            if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                            {
                                comandoSQL = "update INV_BODEGA_PRODUCTOS set saldo = saldo + " + fila[1]
                                    + ", MODIFICADO =  TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',  'dd/mm/yyyy hh24:mi:ss') where inv_productos = '" + fila[0] + "' and cat_bodega = '" + entrada.Bodega + "' ";
                                if (ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                                {
                                    res[0] = "success";
                                    res[1] = "Éxito:";
                                    res[2] = "Entrada agregada al sistema.";
                                }
                                //comandoSQL = "call insertar_historial( llave_bodega_local, cantidad, precio_colones )";
                                // Aqui para agregar tabla de costo promedio
                            }
                        }
                    }
                }
                else
                {
                    res[0] = "danger";
                    res[1] = "Error:";
                    res[2] = "Entrada no agregada, intente nuevamente.";
                }            
            }
            catch (Exception x){
            
            }

            return res;
        }

        /*
         * Consulta el nombre de un proveedor a partir de su identificador.
         */
        public String consultarNombreProveedor(string idProveedor)
        {
            String esquema = "Compras.";
            DataTable consultado = new DataTable();
            String resultado = "";
            String comandoSQL = "select NOMBRE from "+ esquema + "V_PROVEEDOR where IDPROVEEDOR = '" + idProveedor + "'";

            consultado = ejecutarComandoSQL(comandoSQL, true);
            if (consultado.Rows.Count > 0) 
            {
                foreach (DataRow fila in consultado.Rows)
                {
                    resultado = fila[0].ToString();
                }            
            }

            return resultado;
        }
    }
}