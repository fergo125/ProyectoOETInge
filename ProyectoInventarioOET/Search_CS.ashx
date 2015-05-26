<%@ WebHandler Language="C#" Class="Search_CS" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

/*
 * Clase encargada de manejar el evento en el que se escribe en la barra de búsqueda de autocomplete.
 */
public class Search_CS : IHttpHandler
{
    /*
     * Función invocada al escribir en el textbox, busca en la base de datos nombres de productos similares a lo escrito y consulta
     * también los códigos internos de esos productos para luego agregarlos a la factura apropiadamente.
     */
    public void ProcessRequest (HttpContext context)
    {
        string prefixText = context.Request.QueryString["q"];
        StringBuilder sb = new StringBuilder(); 
        
        //Conseguir datos desde la base de datos aquí
        
        for(int i=0; i<5; ++i)
            sb.Append("Opcion " + i).Append(Environment.NewLine);
        
        context.Response.Write(sb.ToString()); 
    }
 
    public bool IsReusable
    {
        get { return false; }
    }
}