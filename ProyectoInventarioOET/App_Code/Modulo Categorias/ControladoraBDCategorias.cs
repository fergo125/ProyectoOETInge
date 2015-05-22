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
            String[] res = new String[4];
            res[3] = categoria.Nombre;
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO CAT_CATEGORIAS (CAT_CATEGORIAS,DESCRIPCION,Estado) VALUES ('"
                + generarID() + "','" + categoria.Descripcion + "','" + categoria.Estado+ "')";
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Categoría agregada al sistema.";
            }
            catch (SqlException e)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Categoría no agregada, intente nuevamente.";
            }
            return res;
        }

        /*
         * Metodo que recibe una categoria original y la modificada para meterla en la BD
         */
        public String[] modificarCategoria(EntidadCategoria categoria, EntidadCategoria nuevaCategoria)
        {
            String[] res = new String[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE CAT_CATEGORIAS SET DESCRIPCION = '" + nuevaCategoria.Nombre+ "', estado = " + (short)nuevaCategoria.Estado  + " WHERE CAT_CATEGORIAs = '" + categoria.Nombre + "'";
                OracleDataReader reader = command.ExecuteReader();


                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Categoria modificada en el sistema.";
            }
            catch (SqlException e)
            {
                if (e.Number == 2627)
                {
                    res[0] = "danger";
                    res[1] = "Error:";
                    res[2] = "Categoría no modificada, intente nuevamente.";
                }
            }
            return res;
        }

        /*
         * Metodo que recibe una entidad categoria para desactivarla
         */
        public String[] desactivarCategoria(EntidadCategoria categoria)
        {
            String[] res = new String[3];
            try
            {
                //adaptadorCategoria.Update();
                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Categoría desactivada en el sistema.";
            }
            catch (SqlException e)
            {
                res[1] = "danger";
                res[1] = "Error:";
                res[3] = "Categoría no desactivada, intente nuevamente.";
            }
            return res;
        }

        /*
         * Metodo que devuelve un datatable con los datos de todas las categorias existentes
         */
        public DataTable consultarCategorias()
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM cat_categorias";
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
         * Metodo que recibe el id de una categoria y devuelve el nombre
         */
        public String consultarDescripcionCategoria(String idCategoria)
        {
            String res = "";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT C.DESCRIPCION FROM cat_bodega C WHERE C.NOMBRE = '" + idCategoria + "'";
                OracleDataReader reader = command.ExecuteReader();
                res = resultado.Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return res;
        }

        /*
         * Metodo que recibe el nombre de una categoria y devuelve sus datos
         */
        public EntidadCategoria consultarCategoria(String nombre)
        {
            DataTable resultado = new DataTable();
            EntidadCategoria categoriaConsultada = null;
            Object[] datosConsultados = new Object[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_CATEGORIAS WHERE  CAT_CATEGORIAS = '" + nombre + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

                if (resultado.Rows.Count == 1)
                {
                    datosConsultados[0] = nombre;
                    datosConsultados[1] = resultado.Rows[0][1].ToString();
                    datosConsultados[2] = Convert.ToInt32(resultado.Rows[0][2].ToString());
                    categoriaConsultada = new EntidadCategoria(datosConsultados);
                }
            }
            catch (Exception e)
            {
            }
            return categoriaConsultada;
        }
    }
}