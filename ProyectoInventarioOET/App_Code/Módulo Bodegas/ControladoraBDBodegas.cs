﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Módulo_Bodegas

{
    
    public class ControladoraBDBodegas : ControladoraBD
    {
        
        CAT_BODEGATableAdapter adaptadorBodega;

        public ControladoraBDBodegas()
        {
            adaptadorBodega = new CAT_BODEGATableAdapter();
        }
        public String[] insertarBodega(EntidadBodega bodega)
        {
            String[] res = new String[4];
            res[3] = bodega.Codigo;
            try
            {
               
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "INSERT INTO CAT_BODEGA (CAT_BODEGA,DESCRIPCION,ANFITRIONA,ESTACION,ESTADO) VALUES ('"
                + bodega.Codigo + "','" + bodega.Nombre + "','" + bodega.Anfitriona + "','"
                + bodega.Estacion + "'," + (short)bodega.Estado + ")";
                OracleDataReader reader = command.ExecuteReader();
                
                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega Agregada";
            }
            catch (SqlException e)
            {
                res[0] = "danger";
                res[1] = "Fallo en la operacion";
                res[2] = "Intente nuevamente";
            }
            return res;
        }


        public String[] modificarBodega(EntidadBodega bodega, EntidadBodega nuevaBodega)
        {
            String[] res = new String[3];
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "UPDATE CAT_BODEGA SET CAT_BODEGA = '" + nuevaBodega.Codigo + "', DESCRIPCION = '" + nuevaBodega.Nombre + "',ANFITRIONA = '"
                    + nuevaBodega.Anfitriona + "',ESTACION = '" + nuevaBodega.Estacion + "',ESTADO = " + (short)nuevaBodega.Estado + "WHERE CAT_BODEGA = '"
                    + bodega.Codigo + "' AND DESCRIPCION = '" + bodega.Nombre + "' AND ANFITRIONA = '" + bodega.Anfitriona + "' AND ESTACION = '"
                    + bodega.Estacion + "' AND ESTADO = " + bodega.Estado;
                OracleDataReader reader = command.ExecuteReader();
                
                
                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega modificada";
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


        public String[] desactivarBodega(EntidadBodega bodega)
        {
            String[] res = new String[3];
            try
            {

                //adaptadorBodega.Update();

                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega eliminado";
            }
            catch (SqlException e)
            {
                res[1] = "danger";
                res[2] = "Fallo";
                res[3] = "Error al eliminar";
            }
            return res;
        }

        public DataTable consultarBodegas()
        {
            DataTable resultado = new DataTable();

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_BODEGA";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }

       

        public EntidadBodega consultarBodega(String codigo)
        {
            DataTable resultado = new DataTable();
            EntidadBodega bodegaConsultada = null; 
            Object[] datosConsultados = new Object[5]; 

            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = "SELECT * FROM CAT_BODEGA WHERE  CAT_BODEGA = '" + codigo + "'";
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);

                if (resultado.Rows.Count == 1)
                {
                    datosConsultados[0] = codigo;
                    for (int i = 1; i < 5; i++)
                    {
                        datosConsultados[i] = resultado.Rows[0][i].ToString();
                    }

                    bodegaConsultada = new EntidadBodega(datosConsultados);
                }
            }
            catch (Exception e) { }

            return bodegaConsultada;
        }

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
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }
     
    }
}