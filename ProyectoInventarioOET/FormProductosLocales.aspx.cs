﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProyectoInventarioOET.Módulo_Bodegas;
using ProyectoInventarioOET.App_Code;

namespace ProyectoInventarioOET
{
    public partial class FormProductosLocales : System.Web.UI.Page
    {
        private static ControladoraBodegas controladoraBodegas;
        private static ControladoraDatosGenerales controladoraDatosGenerales;
        private static int resultadosPorPagina;
        private static int modo=0;
        private static Object[] idArray;
        private static int estacionSeleccionada, bodegaSeleccionada;
        private static Object[] idArray2;

        protected void Page_Load(object sender, EventArgs e)
        {
            testGrid();
            if (!IsPostBack)
            {
                controladoraBodegas = new ControladoraBodegas();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                DropDownListEstacion_CargaEstaciones();
            }
            cambiarModo();
        }

        protected void cambiarModo()
        {
            switch (modo)
            {
                case 0:
                    FieldsetCatalogoLocal.Visible = false;
                    FieldsetProductos.Visible = false;
                    break;
                case 1://consultar con los espacios bloqueados
                    FieldsetCatalogoLocal.Visible = true;
                    FieldsetProductos.Visible = true;
                    DropDownListEstacion.SelectedIndex = estacionSeleccionada;
                    DropDownListEstacion_SelectedIndexChanged(DropDownListEstacion,null);
                    DropDownListBodega.SelectedIndex = bodegaSeleccionada;
                    break;
                default:
                    // Algo salio mal
                    break;
            }
        }

        // SOLO PARA PRUEBAS
        protected void testGrid()
        {
            DataTable tabla2 = tablaCatalogoLocal();
            for (int i = 1; i < 5; i++)
            {
                Object[] datos2 = new Object[5];
                datos2[0] = i * 2;
                datos2[1] = i * 3;
                datos2[2] = i * 4;
                datos2[3] = i * 5;
                datos2[4] = i * 6;
                tabla2.Rows.Add(datos2);
            }
            this.gridViewCatalogoLocal.DataSource = tabla2;
            this.gridViewCatalogoLocal.DataBind();

        }

        protected DataTable tablaCatalogoLocal()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Precio";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Cantidad";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Mínimo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Máximo";
            tabla.Columns.Add(columna);

            return tabla;
        }

        protected void gridViewCatalogoLocal_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    //GridViewRow filaSeleccionada = this.gridViewCatalogoLocal.Rows[Convert.ToInt32(e.CommandArgument)];
                    //int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCatalogoLocal.PageIndex * resultadosPorPagina)]);
                    modo=1;
                    Response.Redirect("FormProductosLocales.aspx");
                    break;
            }
        }

        protected void gridViewCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            this.gridViewCatalogoLocal.PageIndex = e.NewPageIndex;
            this.gridViewCatalogoLocal.DataBind();
        }
        // Carga estaciones
        protected void DropDownListEstacion_CargaEstaciones()
        {
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
            int i=0;
            if (estaciones.Rows.Count > 0)
            {
                this.DropDownListEstacion.Items.Clear();
                idArray = new Object[estaciones.Rows.Count];
                foreach (DataRow fila in estaciones.Rows)
                {
                    idArray[i] = fila[0];
                    this.DropDownListEstacion.Items.Add(new ListItem(fila[1].ToString()));
                    i++;
                }
            }
        }
        // Seleccion de estación, se cargan las bodegas disponibles
        protected void DropDownListEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DropDownListBodega.Items.Clear();
            estacionSeleccionada= this.DropDownListEstacion.SelectedIndex;
            String idEstacion = idArray[estacionSeleccionada].ToString();
            DataTable bodegas = controladoraBodegas.consultarBodegasDeEstacion(idEstacion);
            int i = 0;
            if (bodegas.Rows.Count > 0)
            {
                idArray2 = new Object[bodegas.Rows.Count];
                foreach (DataRow fila in bodegas.Rows)
                {
                    idArray2[i] = fila[0];
                    this.DropDownListBodega.Items.Add(new ListItem(fila[1].ToString()));
                }
            }

        }
        //Consulta de bodega, aquí se carga la tabla
        protected void botonConsultarBodega_ServerClick(object sender, EventArgs e)
        {
            if (this.DropDownListBodega.SelectedItem != null)
            {
                bodegaSeleccionada = this.DropDownListBodega.SelectedIndex;
                FieldsetCatalogoLocal.Visible = true;
            }
        }
        /*// Envío de modificaciones de producto de catálogo local
        protected void botonEnviarProducto_ServerClick(object sender, EventArgs e)
        {

        }*/
        // Aceptar cancelación del modal de cancelar
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("FormProductosLocales.aspx");
        }
        // Desactivación confirmada
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {

        }

    }
}