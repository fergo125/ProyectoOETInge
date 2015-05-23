using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Entradas;
using ProyectoInventarioOET.App_Code;


namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormEntradas : System.Web.UI.Page
    {
        enum Modo { Inicial, ConsultaEntradas, Insercion, Consultado }; // Sirve para controlar los modos de la interfaz
        //Atributos
        private static int modo = (int)Modo.Inicial;                            // Almacena el modo actual de la interfaz
        private static ControladoraEntradas controladoraEntradas;               // Comunica con la base de datos.
        private static Object[] idArray;                                        // Almacena identificadores de entradas
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Obtiene datos generales (estados)
        private static EntidadEntrada entradaConsultada;                    // Almacena la entrada que se consultó (o acaba de agregar)
        private static Boolean seConsulto = false;                              // Bandera para saber si hubo consulta de una actividad.
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static String bodegaDeTrabajo;
        private static String facturaBuscada;


        protected void Page_Load(object sender, EventArgs e)
        {
            controladoraEntradas = new ControladoraEntradas();
            //bodegaDeTrabajo = (this.Master as SiteMaster).LlaveBodegaSesion;
            
            //llenarGrid();
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información de las actividades.
         */
<<<<<<< HEAD
        protected DataTable tablaEntradas()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Entrada";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Factura";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Responsable";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Fecha de Realización";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
 * Construye la tabla que se va a utilizar para mostrar la información de las Facturas.
 */
=======
>>>>>>> origin/master
        protected DataTable tablaFacturas()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Factura";
            tabla.Columns.Add(columna);

<<<<<<< HEAD
            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Orden de Compra";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Fecha de Pago";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Tipo de Pago";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información del detalle de las Facturas entrantes.
         */
        protected DataTable tablaFacturaDetallada()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Producto";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Cantidad";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Costo Unitario";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Construye la tabla que se va a utilizar para mostrar la información de como se va construyendo la factura según los artículos recibidos.
         */
        protected DataTable tablaResultadosBusqueda()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);
=======
            //columna = new DataColumn();
            //columna.DataType = System.Type.GetType("System.String");
            //columna.ColumnName = "Código Interno";
            //tabla.Columns.Add(columna);
>>>>>>> origin/master

            return tabla;
        }

        /*
         * Llena la tabla con las actividades almacenadas en la base de datos.
         */
        protected void llenarGridEntradas()
        {
            DataTable tabla = tablaEntradas();
            int indiceNuevaActividad = -1;
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[4];
                bodegaDeTrabajo = "CRO44452";
                DataTable entradas = controladoraEntradas.consultarEntradas(bodegaDeTrabajo);

                if (entradas.Rows.Count > 0)
                {
                    idArray = new Object[entradas.Rows.Count];
                    foreach (DataRow fila in entradas.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = fila[2].ToString();
                        datos[3] = fila[4].ToString();


                        tabla.Rows.Add(datos);
                        //if (entradaConsultada != null && (fila[0].Equals(entradaConsultada.Codigo)))
                        //{
                        //    indiceNuevaActividad = i;
                        //}
                        i++;
                    }
                }
                // No hay entradas almacenadas.
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridViewEntradas.DataSource = tabla;
                this.gridViewEntradas.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Entradas.");
            }
        }

        /*
         * Llena la tabla con las actividades almacenadas en la base de datos.
         */
        protected void llenarGridFacturas()
        {
            DataTable tabla = tablaFacturas();
            int indiceNuevaActividad = -1;
            int i = 0;

            try
            {
                // Cargar entradas
                Object[] datos = new Object[4];
                DataTable facturas = controladoraEntradas.buscarFacturas(facturaBuscada);

                if (facturas.Rows.Count > 0)
                {
                    idArray = new Object[facturas.Rows.Count];
                    foreach (DataRow fila in facturas.Rows)
                    {
                        idArray[i] = fila[0];
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[1].ToString();
                        datos[2] = fila[2].ToString();
                        datos[3] = fila[4].ToString();


                        tabla.Rows.Add(datos);
                        //if (entradaConsultada != null && (fila[0].Equals(entradaConsultada.Codigo)))
                        //{
                        //    indiceNuevaActividad = i;
                        //}
                        i++;
                    }
                }
                // No hay entradas almacenadas.
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridViewFacturas.DataSource = tabla;
                this.gridViewFacturas.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Facturas.");
            }
        }

        /*
         * Llena la tabla con las actividades almacenadas en la base de datos.
         */
        protected void llenarGrid()
        {
            DataTable tabla = tablaFacturas();
            int indiceNuevaActividad = -1;
            //int i = 0;

            try
            {
                // Cargar actividades
                //Object[] datos = new Object[3];
                Object[] datos = new Object[4];
                //DataTable actividades = controladoraActividades.consultarActividades();
                //idArray = new Object[actividades.Rows.Count];
                for (int i = 0; i < 3; i++ )
                {
                    //idArray[i] = fila[0];
                    datos[0] = 55;
                    datos[1] = 55;
                    datos[2] = 55;
                    datos[3] = 55;
                    //datos[1] = fila[0].ToString();
                    //if (fila[2].ToString().Equals("0"))
                    //{
                    //    datos[2] = "Inactivo";
                    //}
                    //else if (fila[2].ToString().Equals("1"))
                    //{
                    //    datos[2] = "Activo";
                    //}
                    //else
                    //{
                    //    datos[2] = fila[2].ToString();
                    //}
                    //if (fila[2].ToString().Equals("0"))
                    //{
                    //    datos[1] = "Inactivo";
                    //}
                    //else if (fila[2].ToString().Equals("1"))
                    //{
                    //    datos[1] = "Activo";
                    //}
                    //else
                    //{
                    //    datos[1] = fila[2].ToString();
                    //}

                    tabla.Rows.Add(datos);
                    //if (actividadConsultada != null && (fila[0].Equals(actividadConsultada.Codigo)))
                    //{
                    //    indiceNuevaActividad = i;
                    //}
                    //i++;
                }
                

                this.gridViewFacturas.DataSource = tabla;
                this.gridViewFacturas.DataBind();
            }
            catch (Exception e)
            {
                //mostrarMensaje("warning", "Alerta", "Error al llenar la tabla de Facturas.");
            }
        }

        /*
         * Muestra el mensaje que da el resultado de las transacciones que se efectúan.
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        protected void botonConsultaEntradas_ServerClick(object sender, EventArgs e)
        {
            llenarGridEntradas();
            FieldsetGridEntradas.Visible = true;
        }

        protected void gridViewEntradas_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gridViewEntradas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

<<<<<<< HEAD
        protected void botonAgregarEntradas_ServerClick(object sender, EventArgs e)
        {
            FielsetBuscarFactura.Visible = true;
        }

        protected void botonMostrarFacturas_Click(object sender, EventArgs e)
        {
            facturaBuscada = "Todas";
            llenarGridFacturas();
            FieldsetGridFacturas.Visible = true;
        }

        protected void botonBuscarFactura_Click(object sender, EventArgs e)
        {
            facturaBuscada = this.barraDeBusquedaFactura.Value.ToString();
            llenarGridFacturas();
            FieldsetGridFacturas.Visible = true;
        }


        protected void gridViewFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            FieldsetEncabezadoFactura.Visible = true;
            FieldsetCrearFactura.Visible = true;
        }

        protected void gridViewFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gridDetalleFactura_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void botonBuscar_Click(object sender, EventArgs e)
        {
            FieldsetResultadosBusqueda.Visible = true;
        }

        protected void gridViewProductoBuscado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            FieldsetResultadosBusqueda.Visible = false;
        }

        protected void gridViewProductoBuscado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gridFacturaNueva_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

=======
>>>>>>> origin/master

















    }
}