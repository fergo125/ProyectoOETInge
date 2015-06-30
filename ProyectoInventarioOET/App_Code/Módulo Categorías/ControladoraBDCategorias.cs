using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Modulo_Categorias 
{
    /*
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDCategorias : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDCategorias()
        {
        }

        /*
         * Metodo que recibe una entidad categoria y la inserta en la BD
         */
        public String[] insertarCategoria(EntidadCategoria categoria)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[4];
            resultado[3] = categoria.Nombre;
            String comandoSQL = "INSERT INTO " + esquema + "CAT_CATEGORIAS (CAT_CATEGORIAS,DESCRIPCION,Estado) VALUES ('"
                + generarID() + "','" + categoria.Descripcion + "','" + categoria.Estado+ "')";
            if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                resultado[0] = "success";
                resultado[1] = "Éxito:";
                resultado[2] = "Categoría agregada al sistema.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error:";
                resultado[2] = "Categoría no agregada, intente nuevamente.";
            }
            return resultado;
        }

        /*
         * Metodo que recibe una categoria original y la modificada para meterla en la BD
         */
        public String[] modificarCategoria(EntidadCategoria categoria, EntidadCategoria nuevaCategoria)
        {
            String esquema = "Inventarios.";
            String[] resultado = new String[3];
            String comandoSQL = "UPDATE " + esquema + "CAT_CATEGORIAS SET DESCRIPCION = '" + nuevaCategoria.Nombre + "', estado = "
                + (short)nuevaCategoria.Estado + " WHERE CAT_CATEGORIAS = '" + categoria.Nombre + "'";
            if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
            {
                resultado[0] = "success";
                resultado[1] = "Éxito:";
                resultado[2] = "Categoria modificada en el sistema.";
            }
            else
            {
                resultado[0] = "danger";
                resultado[1] = "Error:";
                resultado[2] = "Categoría no modificada, intente nuevamente.";
            }
            return resultado;
        }

        /*
         * Metodo que devuelve un datatable con los datos de todas las categorias existentes
         */
        public DataTable consultarCategorias()
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_CATEGORIAS";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Metodo que recibe el id de una categoria y devuelve el nombre
         */
        public String consultarDescripcionCategoria(String idCategoria)
        {
            String esquema = "Inventarios.";
            String res = "";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT C.DESCRIPCION FROM " + esquema + "CAT_BODEGA C WHERE C.NOMBRE = '" + idCategoria + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if(resultado.Rows.Count == 1)
                res = resultado.Rows[0][0].ToString();
            return res;
        }

        /*
         * Metodo que recibe el nombre de una categoria y devuelve sus datos
         */
        public EntidadCategoria consultarCategoria(String nombre)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            EntidadCategoria categoriaConsultada = null;
            Object[] datosConsultados = new Object[3];
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_CATEGORIAS WHERE CAT_CATEGORIAS = '" + nombre + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = nombre;
                datosConsultados[1] = resultado.Rows[0][1].ToString();
                datosConsultados[2] = Convert.ToInt32(resultado.Rows[0][2].ToString());
                categoriaConsultada = new EntidadCategoria(datosConsultados);
            }
            return categoriaConsultada;
        }
    }
}