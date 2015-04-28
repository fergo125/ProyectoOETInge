using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Módulo_Bodegas
{
    /*
     * ???
     * Comunicación con la Base de Datos.
     */
    public class ControladoraBDBodegas : ControladoraBD
    {
        /*
         * Constructor.
         */
        public ControladoraBDBodegas()
        {
        }

        /*
         * ???
         */
        public String[] insertarBodega(EntidadBodega bodega)
        {
            bool existenteEnBD = false;
            DataTable bodegas = consultarBodegas();
            if (bodegas.Rows.Count > 0)
            {
                existenteEnBD = false;
                foreach (DataRow fila in bodegas.Rows)
                {
                    if(fila[1].Equals(bodega.Nombre))
                    {
                        existenteEnBD = true;
                    }
                }
            }

            String[] res = new String[4];
            res[3] = bodega.Codigo;

            if (existenteEnBD)
            {
                res[0] = "danger";
                res[1] = "Error:";
                res[2] = "Ya existe una bodega con ese nombre en la base de datos.";
            }
            else
            {


                try
                {
                    OracleCommand command = conexionBD.CreateCommand();
                    command.CommandText = "INSERT INTO CAT_BODEGA (CAT_BODEGA,DESCRIPCION,ANFITRIONA,ESTACION,ESTADO,CAT_INTENCIONUSO) VALUES ('"
                    + bodega.Codigo + "','" + bodega.Nombre + "','" + bodega.Anfitriona + "','"
                    + bodega.Estacion + "'," + (short)bodega.Estado + "," + bodega.IntencionUso + ")";
                    OracleDataReader reader = command.ExecuteReader();

                    res[0] = "success";
                    res[1] = "Éxito:";
                    res[2] = "Bodega agregada al sistema.";
                }
                catch (OracleException e)
                {
                    res[0] = "danger";
                    res[1] = "Error:";
                    res[2] = "Bodega no agregada, intente nuevamente.";
                }
            }
            return res;
        }

        /*
         * ???
         */
        public String[] modificarBodega(EntidadBodega bodega, EntidadBodega nuevaBodega)
        {
            bool existenteEnBD = false;
            DataTable bodegas = consultarBodegas();
            if (bodegas.Rows.Count > 0)
            {
                existenteEnBD = false;
                foreach (DataRow fila in bodegas.Rows)
                {
                    if(fila[1].Equals(nuevaBodega.Nombre))
                    {
                        existenteEnBD = true;
                    }
                }
            }

            String[] res = new String[4];
            res[3] = bodega.Codigo;

            if ((existenteEnBD)&&(nuevaBodega.Nombre!=bodega.Nombre))
            {
                res[0] = "danger";
                res[1] = "Error: ";
                res[2] = "Ya existe una bodega con ese nombre en la base de datos.";
            }
            else
            {
               try
                {
                    OracleCommand command = conexionBD.CreateCommand();
                    command.CommandText = "UPDATE CAT_BODEGA SET DESCRIPCION = '" + nuevaBodega.Nombre + "',ANFITRIONA = '"
                        + nuevaBodega.Anfitriona + "',ESTACION = '" + nuevaBodega.Estacion + "',ESTADO = " + (short)nuevaBodega.Estado + ", CAT_INTENCIONUSO = "
                        + (short)nuevaBodega.IntencionUso + " WHERE DESCRIPCION = '"
                        + bodega.Nombre + "' AND ANFITRIONA = '" + bodega.Anfitriona + "' AND ESTACION = '" 
                        + bodega.Estacion + "' AND ESTADO = " + bodega.Estado + " AND CAT_INTENCIONUSO = " + bodega.IntencionUso;
                    OracleDataReader reader = command.ExecuteReader();
                
                    res[0] = "success";
                    res[1] = "Éxito:";
                    res[2] = "Bodega modificada en el sistema.";
                }
                catch (OracleException e)
                {
                    if (e.Number == 2627)
                    {
                        res[0] = "danger";
                        res[1] = "Error:";
                        res[2] = "Bodega no modificada, intente nuevamente.";
                    }
                }
            }
                
            return res;
        }

        /*
         * ???
         */
        public String[] desactivarBodega(EntidadBodega bodega)
        {
            String[] res = new String[3];
            try
            {
                //adaptadorBodega.Update();
                res[0] = "success";
                res[1] = "Éxito:";
                res[2] = "Bodega desactivada en el sistema.";
            }
            catch (OracleException e)
            {
                res[1] = "danger";
                res[2] = "Error:";
                res[3] = "Bodega no desactivada.";
            }
            return res;
        }

        /*
         * ???
         */
        public DataTable consultarBodegas()
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT C.CAT_BODEGA,C.DESCRIPCION,C.ANFITRIONA,D.NOMBRE,E.DESCRIPCION, F.NOMBRE FROM cat_bodega C, estacion D, cat_estados E, cat_intencionuso F WHERE C.ESTACION = D.ID AND E.CAT_ESTADO = C.ESTADO AND C.CAT_INTENCIONUSO = F.CAT_INTENCIONUSO";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        /*
         * ???
         */
        public EntidadBodega consultarBodega(String codigo)
        {
            DataTable resultado = new DataTable();
            EntidadBodega bodegaConsultada = null; 
            Object[] datosConsultados = new Object[6]; 
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_BODEGA WHERE  CAT_BODEGA = '" + codigo + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

                if (resultado.Rows.Count == 1)
                {
                    datosConsultados[0] = codigo;
                    for (int i = 1; i < 6; i++)
                    {
                        datosConsultados[i] = resultado.Rows[0][i].ToString();
                    }

                    bodegaConsultada = new EntidadBodega(datosConsultados);
                }
            }
            catch (OracleException e)
            {
            }
            return bodegaConsultada;
        }

        /*
         * Consulta y devuelve las bodegas que pertenecen a una estacion en especifico.
         */
        public DataTable consultarBodegasDeEstacion(String codigo)
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_BODEGA WHERE CAT_BODEGA.ESTACION = '"+codigo+"'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        /*
         * ???
         */
        public DataTable consultarIntenciones()
        {
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_INTENCIONUSO";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }
        /*
         * Obtiene la informacion de los productos que no pertenecen a la bodega especificada
         */
        public DataTable consultarProductosAsociables(String idBodega)
        {
            DataTable resultado=new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT INV_PRODUCTOS.NOMBRE,INV_PRODUCTOS.CODIGO,INV_PRODUCTOS.CAT_CATEGORIAS,INV_PRODUCTOS.INTENCION FROM INV_PRODUCTOS WHERE INV_PRODUCTOS.INV_PRODUCTOS NOT IN (SELECT INV_BODEGA_PRODUCTOS.INV_PRODUCTOS FROM INV_BODEGA_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS.CAT_BODEGA = '"+idBodega+"')";
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