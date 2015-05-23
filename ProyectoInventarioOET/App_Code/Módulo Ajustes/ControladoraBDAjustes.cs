using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{
    public class ControladoraBDAjustes : ControladoraBD
    {

        public DataTable tiposAjuste()
        {
            String esquema1 = "Inventarios.";
            String esquema2 = "Reservas.";
            String esquema3 = "Tesoreria.";
            DataTable resultado = new DataTable();
            
            /*Modificar para recibir como parametros*/
            try 
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_TIPO_MOVIMIENTO";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (OracleException e)
            {
                resultado = null;
            }
            return resultado;
        }

        public String[] insertarAjuste(EntidadAjustes ajuste)
        {
            String esquema = "Inventarios.";
            String[] res = new String[4];
            res[3] = generarID();
                try
                {
                    OracleCommand command = conexionBD.CreateCommand();
                    command.CommandText = "INSERT INTO " + esquema + 
                                           "AJUSTE (idAjuste, tipoAjuste, fecha, usuario) VALUES ('"
                                            + res[3] + "','" + ajuste.IdTipoAjuste + "','" + ajuste.Fecha + "','"
                                            + ajuste.Usuario + "' )";
                    OracleDataReader reader = command.ExecuteReader();
                    
                    //foreach(List <EntidadDetalles> ajuste.Detalles()  ){
                    
                    //}

                    res[0] = "success";
                    res[1] = "Éxito:";
                    res[2] = "Ajuste agregado al sistema.";
                }
                catch (OracleException e)
                {
                    res[0] = "danger";
                    res[1] = "Error:";
                    res[2] = "Ajuste no agregado, intente nuevamente.";
                }
            
            return res;
        }


        //private Boolean insertarDetalles() { 
        
        
        //}



        }
    }
