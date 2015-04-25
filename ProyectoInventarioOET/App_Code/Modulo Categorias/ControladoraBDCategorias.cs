using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;
using System.Data;

namespace ProyectoInventarioOET.Modulo_Categorias
{
    public class ControladoraBDCategorias : ControladoraBD
    {

        public ControladoraBDCategorias()
        {
        }
        public String[] insertarCategoria(EntidadCategoria categoria)
        {
            String[] res = new String[4];
            res[4] = categoria.Nombre;
            try
            {

                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO CAT_CATEGORIAS (CAT_CATEGORIAS,DESCRIPCION) VALUES ('"
                + categoria.Nombre + "','" + categoria.Descripcion + ")";
                OracleDataReader reader = command.ExecuteReader();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Categoria Agregada";
            }
            catch (SqlException e)
            {
                res[0] = "danger";
                res[1] = "Fallo en la operacion";
                res[2] = "Intente nuevamente";
            }
            return res;
        }


        public String[] modificarCategoria(EntidadCategoria categoria, EntidadCategoria nuevaCategoria)
        {
            String[] res = new String[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE CAT_CATEGORIAS SET CAT_CATEGORIAS = '" + nuevaCategoria.Nombre+ "', DESCRIPCION = '" + nuevaCategoria.Descripcion;
                OracleDataReader reader = command.ExecuteReader();


                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Categoria modificada";
            }
            catch (SqlException e)
            {
                if (e.Number == 2627)
                {
                    res[0] = "danger";
                    res[1] = "Fallo";
                    res[2] = "Error al modificar";
                }
            }
            return res;
        }


        public String[] desactivarCategoria(EntidadCategoria categoria)
        {
            String[] res = new String[3];
            try
            {

                //adaptadorCategoria.Update();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Categoria eliminado";
            }
            catch (SqlException e)
            {
                res[1] = "danger";
                res[2] = "Fallo";
                res[3] = "Error al eliminar";
            }
            return res;
        }

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

        public EntidadCategoria consultarCategoria(String nombre)
        {
            DataTable resultado = new DataTable();
            EntidadCategoria categorìaConsultada = null;
            Object[] datosConsultados = new Object[2];

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

                    categorìaConsultada = new EntidadCategoria(datosConsultados);

                }
            }
            catch (Exception e) { }

            return categorìaConsultada;
        }

    }
}