using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Modulo_Bodegas
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
        public String[] insertarBodega(EntidadBodega bodega, String idUsuario, String rol)
        {
            String esquema = "Inventarios.";
            bool existenteEnBD = false;
            DataTable bodegas = consultarBodegas(idUsuario, rol);
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
                    command.CommandText = "INSERT INTO " + esquema + "CAT_BODEGA (CAT_BODEGA,DESCRIPCION,ANFITRIONA,ESTACION,ESTADO,CAT_INTENCIONUSO) VALUES ('"
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
        public String[] modificarBodega(EntidadBodega bodega, EntidadBodega nuevaBodega, String idUsuario, String rol)
        {
            String esquema = "Inventarios.";
            bool existenteEnBD = false;
            DataTable bodegas = consultarBodegas(idUsuario,rol);
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
                    command.CommandText = "UPDATE " + esquema + "CAT_BODEGA SET DESCRIPCION = '" + nuevaBodega.Nombre + "',ANFITRIONA = '"
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
        public DataTable consultarBodegas(String idUsuario, String rol)
        {
            String esquema1 = "Inventarios.";
            String esquema2 = "Reservas.";
            String esquema3 = "Tesoreria.";
            DataTable resultado = new DataTable();
            
            /*Modificar para recibir como parametros*/
            try 
            {
                OracleCommand command = conexionBD.CreateCommand();
                if (rol.Equals("Administrador global"))
                {
                    command.CommandText = "SELECT C.CAT_BODEGA,C.DESCRIPCION,C.ANFITRIONA,D.NOMBRE,E.DESCRIPCION, F.NOMBRE FROM " + esquema1 + "CAT_BODEGA C, " + esquema2
                        + "ESTACION D, " + esquema3 + "CAT_ESTADOS E, " + esquema1 + "CAT_INTENCIONUSO F WHERE C.ESTACION = D.ID AND E.VALOR = C.ESTADO AND C.CAT_INTENCIONUSO = F.CAT_INTENCIONUSO";
                }
                else
                { 
                    command.CommandText = "SELECT C.CAT_BODEGA,C.DESCRIPCION,C.ANFITRIONA,D.NOMBRE,E.DESCRIPCION,"
                        + "F.NOMBRE FROM " + esquema1 + "CAT_BODEGA C, " + esquema2 + "ESTACION D, " + esquema3 + "CAT_ESTADOS E, " + esquema1 + "CAT_INTENCIONUSO F WHERE C.ESTACION = "
                        + "D.ID AND E.VALOR = C.ESTADO AND C.CAT_INTENCIONUSO = F.CAT_INTENCIONUSO AND C.CAT_BODEGA IN "
                        + "(SELECT CAT_BODEGA FROM " + esquema1 + "SEG_USUARIO_BODEGA WHERE SEG_USUARIO = '" + idUsuario + "')";
                }
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
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            EntidadBodega bodegaConsultada = null; 
            Object[] datosConsultados = new Object[6]; 
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM " + esquema + "CAT_BODEGA WHERE CAT_BODEGA = '" + codigo + "'";
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
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                //command.CommandText = "SELECT * FROM " + esquema + "CAT_BODEGA WHERE CAT_BODEGA.ESTACION = '" + codigo + "'";
                command.CommandText = "SELECT * FROM " + esquema + "CAT_BODEGA WHERE ESTACION = '" + codigo + "'";
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
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM " + esquema + "CAT_INTENCIONUSO";
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
            String esquema = "Inventarios."; //TODO: revisar si todos esos +esquema+ sirven
            DataTable resultado=new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT I.NOMBRE, I.CODIGO, I.CAT_CATEGORIAS, I.INTENCION, I.INV_PRODUCTOS FROM " + esquema + "INV_PRODUCTOS I WHERE I.INV_PRODUCTOS NOT IN "
                    + "(SELECT B.INV_PRODUCTOS FROM " + esquema + "INV_BODEGA_PRODUCTOS B WHERE B.CAT_BODEGA = '" + idBodega + "')";
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