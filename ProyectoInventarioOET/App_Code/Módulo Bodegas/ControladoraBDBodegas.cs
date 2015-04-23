using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.DataSetGeneralTableAdapters;
using System.Data.SqlClient;

namespace ProyectoInventarioOET.Módulo_Bodegas

{
    
    public class ControladoraBDBodegas
    {
        
        CAT_BODEGATableAdapter adaptadorBodega;

        public ControladoraBDBodegas()
        {
            adaptadorBodega = new CAT_BODEGATableAdapter();
        }

        public String[] insertarBodega(EntidadBodega bodega)
        {
            String[] res = new String[4];
            res[3] = bodega.Codigo.ToString();
            try
            {
              //  adaptadorBodega.Insert();
                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega Agregada";
            }
            catch (SqlException e)
            {
                // Como la llave es generada se puede volver a intentar
                res[0] = "danger";
                res[1] = "Fallo en la operacion";
                res[2] = "Intente nuevamente";
            }
            return res;
        }


        public String[] modificarBodega(EntidadBodega bodega, EntidadBodega nuevoBodega)
        {
            String[] res = new String[3];
            try
            {
                //adaptadorBodega.Update();
                res[0] = "success";
                res[1] = "Exito";
                res[2] = "Bodega modificado";
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
                resultado = adaptadorBodega.GetData().CopyToDataTable();
            }
            catch (Exception e)
            {
                resultado = null;
            }
            return resultado;
        }

       

        public EntidadBodega consultarBodega(int codigo)
        {
            DataTable resultado = new DataTable();
            EntidadBodega bodegaConsultada = null; 
            Object[] datosConsultados = new Object[5]; 

            try
            {
                //resultado = adaptadorBodega.consultarFilaBodega(id);

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
     
    }
}