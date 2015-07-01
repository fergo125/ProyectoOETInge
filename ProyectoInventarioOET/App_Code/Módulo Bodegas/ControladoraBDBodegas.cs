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
                    if(fila[1].Equals(bodega.Nombre))
                        existenteEnBD = true;
            }
            String[] resultado = new String[4];
            resultado[3] = bodega.Codigo;
            if (existenteEnBD)
            {
                resultado[0] = "danger";
                resultado[1] = "Error:";
                resultado[2] = "Ya existe una bodega con ese nombre en la base de datos.";
            }
            else
            {
                String comandoSQL = "INSERT INTO " + esquema + "CAT_BODEGA (CAT_BODEGA,DESCRIPCION,ANFITRIONA,ESTACION,ESTADO,CAT_INTENCIONUSO) VALUES ('"
                    + bodega.Codigo + "','" + bodega.Nombre + "','" + bodega.Anfitriona + "','"
                    + bodega.Estacion + "'," + (short)bodega.Estado + "," + bodega.IntencionUso + ")";
                if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                {
                    resultado[0] = "success";
                    resultado[1] = "Éxito:";
                    resultado[2] = "Bodega agregada al sistema.";
                }
                else
                {
                    resultado[0] = "danger";
                    resultado[1] = "Error:";
                    resultado[2] = "Bodega no agregada, intente nuevamente.";
                }
            }
            return resultado;
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

            String[] resultado = new String[4];
            resultado[3] = bodega.Codigo;

            if ((existenteEnBD)&&(nuevaBodega.Nombre!=bodega.Nombre))
            {
                resultado[0] = "danger";
                resultado[1] = "Error: ";
                resultado[2] = "Ya existe una bodega con ese nombre en la base de datos.";
            }
            else
            {
                String comandoSQL = "UPDATE " + esquema + "CAT_BODEGA SET DESCRIPCION = '" + nuevaBodega.Nombre + "',ANFITRIONA = '"
                    + nuevaBodega.Anfitriona + "',ESTACION = '" + nuevaBodega.Estacion + "',ESTADO = " + (short)nuevaBodega.Estado + ", CAT_INTENCIONUSO = "
                    + (short)nuevaBodega.IntencionUso + " WHERE DESCRIPCION = '"
                    + bodega.Nombre + "' AND ANFITRIONA = '" + bodega.Anfitriona + "' AND ESTACION = '" 
                    + bodega.Estacion + "' AND ESTADO = " + bodega.Estado + " AND CAT_INTENCIONUSO = " + bodega.IntencionUso;
                if(ejecutarComandoSQL(comandoSQL, false) != null) //si sale bien
                {
                    resultado[0] = "success";
                    resultado[1] = "Éxito:";
                    resultado[2] = "Bodega modificada en el sistema.";
                }
                else
                {
                    resultado[0] = "danger";
                    resultado[1] = "Error:";
                    resultado[2] = "Bodega no modificada, intente nuevamente.";
                }
            }
                
            return resultado;
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
            String comandoSQL = "";
            if (rol.Equals("Administrador global"))
            {
                comandoSQL = "SELECT C.CAT_BODEGA,C.DESCRIPCION,C.ANFITRIONA,D.NOMBRE,E.DESCRIPCION, F.NOMBRE FROM " + esquema1 + "CAT_BODEGA C, " + esquema2
                    + "ESTACION D, " + esquema3 + "CAT_ESTADOS E, " + esquema1 + "CAT_INTENCIONUSO F WHERE C.ESTACION = D.ID AND E.VALOR = C.ESTADO AND C.CAT_INTENCIONUSO = F.CAT_INTENCIONUSO";
            }
            else
            {
                comandoSQL = "SELECT C.CAT_BODEGA,C.DESCRIPCION,C.ANFITRIONA,D.NOMBRE,E.DESCRIPCION,"
                    + "F.NOMBRE FROM " + esquema1 + "CAT_BODEGA C, " + esquema2 + "ESTACION D, " + esquema3 + "CAT_ESTADOS E, " + esquema1 + "CAT_INTENCIONUSO F WHERE C.ESTACION = "
                    + "D.ID AND E.VALOR = C.ESTADO AND C.CAT_INTENCIONUSO = F.CAT_INTENCIONUSO AND C.CAT_BODEGA IN "
                    + "(SELECT CAT_BODEGA FROM " + esquema1 + "SEG_USUARIO_BODEGA WHERE SEG_USUARIO = '" + idUsuario + "')";
            }
            resultado = ejecutarComandoSQL(comandoSQL, true);
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
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_BODEGA WHERE CAT_BODEGA = '" + codigo + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            if (resultado.Rows.Count == 1)
            {
                datosConsultados[0] = codigo;
                for (int i = 1; i < 6; i++)
                    datosConsultados[i] = resultado.Rows[0][i].ToString();
                bodegaConsultada = new EntidadBodega(datosConsultados);
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
            String comandoSQL = "";
            if(codigo == null)
                comandoSQL = "SELECT * FROM " + esquema + "CAT_BODEGA";
            else
                comandoSQL = "SELECT * FROM " + esquema + "CAT_BODEGA WHERE ESTACION = '" + codigo + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
        * Consulta y devuelve las bodegas que pertenecen a una estacion en especifico.
        */
        public String consultarLlaveBodega(String nombre, String idEstacion)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT CAT_BODEGA FROM " + esquema + "CAT_BODEGA WHERE DESCRIPCION = '" + nombre + "' AND ESTACION = '" + idEstacion + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado.Rows[0][0].ToString();
        }
        
        public DataTable consultarIntenciones()
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT * FROM " + esquema + "CAT_INTENCIONUSO";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }
        /*
         * Obtiene la informacion de los productos que no pertenecen a la bodega especificada
         */
        public DataTable consultarProductosAsociables(String idBodega)
        {
            String esquema = "Inventarios."; //TODO: revisar si todos esos +esquema+ sirven
            DataTable resultado=new DataTable();
            String comandoSQL = "SELECT I.NOMBRE, I.CODIGO, I.CAT_CATEGORIAS, I.INTENCION, I.INV_PRODUCTOS FROM " + esquema + "INV_PRODUCTOS I WHERE I.INV_PRODUCTOS NOT IN "
                + "(SELECT B.INV_PRODUCTOS FROM " + esquema + "INV_BODEGA_PRODUCTOS B WHERE B.CAT_BODEGA = '" + idBodega + "')";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }

        /*
         * Consulta y devuelvela estación a la cual pertenece una bodega en específico.
         */
        public DataTable consultarEstacionDeBodega(String idBodega)
        {
            String esquema = "Inventarios.";
            DataTable resultado = new DataTable();
            String comandoSQL = "SELECT ESTACION FROM " + esquema + "CAT_BODEGA WHERE CAT_BODEGA = '" + idBodega + "'";
            resultado = ejecutarComandoSQL(comandoSQL, true);
            return resultado;
        }
      
    }
}