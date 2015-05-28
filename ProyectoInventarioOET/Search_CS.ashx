<%@ WebHandler Language="C#" Class="Search_CS" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

/*
 * Clase encargada de manejar el evento en el que se escribe en la barra de búsqueda de autocomplete.
 */
public class Search_CS : IHttpHandler
{
    //Atributos
    protected static OracleConnection conexionBD;   // Atributo estático compartido por todas las ControladorasBD para conectarse
    /*
     * Función invocada al escribir en el textbox, busca en la base de datos nombres de productos similares a lo escrito y consulta
     * también los códigos internos de esos productos para luego agregarlos a la factura apropiadamente.
     */
    public void ProcessRequest (HttpContext context)
    {
        string prefixText = context.Request.QueryString["q"];
        StringBuilder sb = new StringBuilder();

        //Conseguir datos desde la base de datos aquí
        if (conexionBD == null)
        {
            conexionBD = new OracleConnection();
            conexionBD.ConnectionString = "Data Source=10.1.4.93;User ID=inventarios;Password=inventarios"; //en el futuro se podría leer esta string desde un archivo
            conexionBD.Open();
        }
        if(conexionBD != null)
        {
            DataTable productosSimilares = consultarProductosAutocompletar(prefixText);
            if (productosSimilares != null)
                for (int i = 0; i < productosSimilares.Rows.Count; ++i)
                    sb.Append((productosSimilares.Rows[i][0].ToString()) + " (" + (productosSimilares.Rows[i][1].ToString()) + ")").Append(Environment.NewLine);
            else
            {
                for (int i = 0; i < 5; ++i)
                    sb.Append("Opcion " + i).Append(Environment.NewLine);
            }
        }
        
        context.Response.Write(sb.ToString()); 
    }
 
    public bool IsReusable
    {
        get { return false; }
    }
    
    /*
     * Invocada por la barra de autocomplete, busca sólo el nombre y los códigos internos de
     * los productos en el catálogo global, con base en un String escrito por el usuario
     * que se asocia a uno de esos dos (puede buscar productos por nombre o por código).
     * Procurar que sea eficiente, ya que se invoca por cada key stroke.
     */
    public DataTable consultarProductosAutocompletar(String query)
    {
        String esquema = "Inventarios.";
        DataTable resultado = new DataTable();
        try
        {
            OracleCommand command = conexionBD.CreateCommand();
            command.CommandText = "SELECT NOMBRE, CODIGO FROM " + esquema + "INV_PRODUCTOS WHERE (UPPER(NOMBRE) LIKE UPPER('" + query + "%') OR UPPER(CODIGO) LIKE UPPER('" + query + "%'))";
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