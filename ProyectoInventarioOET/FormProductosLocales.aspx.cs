using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProyectoInventarioOET.Módulo_Bodegas;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.Módulo_Productos_Locales;
using ProyectoInventarioOET.Modulo_Categorias;
using ProyectoInventarioOET.Módulo_Seguridad;

namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormProductosLocales : System.Web.UI.Page
    {
        //Atributos
        private static ControladoraBodegas controladoraBodegas;                 //???
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //???
        private static ControladoraProductoLocal controladoraProductoLocal;     //???
        private static ControladoraSeguridad controladoraSeguridad;             //???
        private static ControladoraCategorias controladoraCategorias;           //???
        private static int resultadosPorPagina; //wtf?
        private static int modo = 0;                                            //???
        private static Object[] idArray;                                        //???
        private static int estacionSeleccionada, bodegaSeleccionada, pagina;    //???
        private static Object[] idArray2;                                       //???
        private static DataTable catalogoLocal, consultaProducto;               //???

        /*
         * ???
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                controladoraBodegas = new ControladoraBodegas();
                controladoraProductoLocal = new ControladoraProductoLocal();
                controladoraCategorias = new ControladoraCategorias();
                controladoraSeguridad = new ControladoraSeguridad();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                DropDownListEstacion_CargaEstaciones();
            }
            cambiarModo();
        }
        /*
         * ???
         */

        protected void cambiarModo()
        {
            switch (modo)
            {
                case 0:
                    FieldsetCatalogoLocal.Visible = false;
                    FieldsetProductos.Visible = false;
                    break;
                case 1: // consultar catálogo
                    FieldsetCatalogoLocal.Visible = true;
                    FieldsetProductos.Visible = false;
                    DropDownListEstacion.SelectedIndex = estacionSeleccionada;
                    DropDownListEstacion_SelectedIndexChanged(DropDownListEstacion,null);
                    DropDownListBodega.SelectedIndex = bodegaSeleccionada;
                    cargarCatalogoLocal();
                    break;
                case 2: //consultar con los espacios bloqueados
                    FieldsetCatalogoLocal.Visible = true;
                    FieldsetProductos.Visible = true;
                    DropDownListEstacion.SelectedIndex = estacionSeleccionada;
                    DropDownListEstacion_SelectedIndexChanged(DropDownListEstacion,null);
                    DropDownListBodega.SelectedIndex = bodegaSeleccionada;
                    cargarCatalogoLocal();
                    cargarDatosProducto();
                    break;
                default:
                    // Algo salio mal
                    break;
            }
        }

        /*
         * ???
         */
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
            columna.ColumnName = "Código Interno";
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

        /*
         * ???
         */
        protected void cargarCatalogoLocal()
        {
            this.gridViewCatalogoLocal.DataSource = catalogoLocal;
            this.gridViewCatalogoLocal.PageIndex = pagina;
            this.gridViewCatalogoLocal.DataBind();
        }

        /*
         * ???
         */
        protected void cargarDatosProducto()
        {
            // nombre, codigo interno, codigo de barras, categoria, intencion, unidades metricas, estado local, existencia, impuesto, precio col, precio dol
            // costo col, costo dol, min, max, creador, creado, modifica, modificado, costo ult col, costo ult dol, idproveedor ult
            if (consultaProducto.Rows.Count > 0)
            {
                DataRow productos = consultaProducto.Rows[0];
                String[] producto = new String[consultaProducto.Columns.Count];

                for (int i = 0; i < consultaProducto.Columns.Count; i++)
                {
                    producto[i] = productos[i].ToString();
                    if (producto[i].Length==0)
                    {
                        producto[i] = "No disponible";
                    }
                }

                DataTable unidades = controladoraDatosGenerales.consultarUnidades();
                foreach (DataRow unidad in unidades.Rows)
                {
                    if (unidad[0].ToString() == producto[5])
                    {
                        producto[5] = unidad[1].ToString();
                    }
                }
                DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
                foreach (DataRow estado in estados.Rows)
                {
                    inputEstado.Items.Add(estado[1].ToString());
                    if (estado[0].ToString() == producto[6])
                    {
                        inputEstado.SelectedValue.Equals(estado[1].ToString());
                    }
                }

                inputNombre.Value = producto[0].ToString();
                inputCodigo.Value = producto[1].ToString();
                inputCodigoBarras.Value = producto[2].ToString();
                inputCategoria.Value = controladoraCategorias.consultarCategoria(producto[3]).Descripcion;
                inputVendible.Value = producto[4].ToString();
                inputUnidades.Value = producto[5].ToString();
                inputSaldo.Value = producto[7].ToString();
                inputImpuesto.Value = producto[8].ToString();
                inputPrecioColones.Value = producto[9].ToString();
                inputPrecioDolares.Value = producto[10].ToString();
                inputCostoColones.Value = producto[11].ToString();
                inputCostoDolares.Value = producto[12].ToString();
                inputMinimo.Value = producto[13].ToString();
                inputMaximo.Value = producto[14].ToString();
                inputCreador.Value = controladoraSeguridad.consultarNombreDeUsuario(producto[15]);
                inputCreado.Value = producto[16].ToString();
                inputModifica.Value = producto[17].ToString();
                inputModificado.Value = producto[18].ToString();
                inputCostoUltCol.Value = producto[19].ToString();
                inputCostoUltDol.Value = producto[20].ToString();
                inputProveedorUlt.Value = producto[21].ToString();

            }

        }

        /*
         * ???
         */
        protected void gridViewCatalogoLocal_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewCatalogoLocal.Rows[Convert.ToInt32(e.CommandArgument)];
                    //int id = Convert.ToInt32(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewCatalogoLocal.PageIndex * resultadosPorPagina)]);
                    String codigo = filaSeleccionada.Cells[2].Text.ToString();
                    String idBodega = idArray2[bodegaSeleccionada].ToString();
                    consultaProducto = controladoraProductoLocal.consultarProductoDeBodega(idBodega, codigo);
                    modo=2;
                    Response.Redirect("FormProductosLocales.aspx");
                    break;
            }
        }

        /*
         * ???
         */
        protected void gridViewCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            pagina = e.NewPageIndex;
            modo = 1;
            //cargarCatalogoLocal();
            Response.Redirect("FormProductosLocales.aspx");
        }

        /*
         * Carga estaciones, ???
         */
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

        /*
         * Seleccion de estación, se cargan las bodegas disponibles.
         */
        protected void DropDownListEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DropDownListBodega.Items.Clear();
            estacionSeleccionada = this.DropDownListEstacion.SelectedIndex;
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
                    i++;
                }
                botonConsultarBodega.Disabled = false;
                botonAsociarBodega.Disabled = false;
            }
            else
            {
                botonConsultarBodega.Disabled = true;
                botonAsociarBodega.Disabled = true;
            }
            modo = 0;
        }


        /*
         * Consulta de bodega, aquí se carga la tabla.
         */
        protected void botonConsultarBodega_ServerClick(object sender, EventArgs e)
        {
            if (this.DropDownListBodega.SelectedItem != null)
            {
                bodegaSeleccionada = this.DropDownListBodega.SelectedIndex;
                pagina = 0;
                FieldsetCatalogoLocal.Visible = true;
                String idBodega = idArray2[bodegaSeleccionada].ToString();
                catalogoLocal = tablaCatalogoLocal();
                DataTable productos = controladoraProductoLocal.consultarProductosDeBodega(idBodega);
                if (productos.Rows.Count > 0)
                {
                    Object[] datos = new Object[5];
                    int i;
                    foreach (DataRow producto in productos.Rows)
                    {
                        for (i = 0; i < 5; i++)
                        {
                            datos[i] = producto[i];
                        }
                            catalogoLocal.Rows.Add(datos);
                    }
                }
                this.gridViewCatalogoLocal.DataSource = catalogoLocal;
                this.gridViewCatalogoLocal.PageIndex = pagina;
                this.gridViewCatalogoLocal.DataBind();
            }
        }

        /*// Envío de modificaciones de producto de catálogo local
        protected void botonEnviarProducto_ServerClick(object sender, EventArgs e)
        {

        }*/

        /*
         * Aceptar cancelación del modal de cancelar.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = 1;
            Response.Redirect("FormProductosLocales.aspx");
        }

        /*
         * Desactivación confirmada.
         */
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {

        }

        protected void DropDownListBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListBodega.SelectedItem != null)
            {
                botonConsultarBodega.Disabled = false;
                botonAsociarBodega.Disabled = false;
            }
        }

        //NO BORRAR    
        //La idea seria q haya un boton de asignacion de productos a la par del de consulta 
        //y que eso haga que se muestre el grid de productos disponibles, se marquen
        //los checkboxes y al momento de darle aceptar con el codigo de abajo se pueden tomar
        //los que estan seleccionados.

        //Con esto que sigue se llena el grid de productos a asignar
        //if (this.DropDownListBodega.SelectedItem != null)
        //{
        //    bodegaSeleccionada = this.DropDownListBodega.SelectedIndex;
        //    pagina = 0;
        //    FieldsetCatalogoLocal.Visible = true;
        //    String idBodega = idArray2[bodegaSeleccionada].ToString();
        //    catalogoLocal = tablaCatalogoLocal();
        //    DataTable productos = controladoraProductoLocal.consultarProductosDeBodega(idBodega);
        //    if (productos.Rows.Count > 0)
        //    {
        //        Object[] datos = new Object[5];
        //        int i;
        //        foreach (DataRow producto in productos.Rows)
        //        {
        //            for (i = 0; i < 5; i++)
        //            {
        //                datos[i] = producto[i];
        //            }
        //                catalogoLocal.Rows.Add(datos);
        //        }
        //    }
        //    this.gridViewProductosDisponibles.DataSource = catalogoLocal;
        //    this.gridViewProductosDisponibles.PageIndex = pagina;
        //    this.gridViewProductosDisponibles.DataBind();
        //}

        //Con esto se recolectan los productos asociados para luego asociarlos en la BD
        //int contador = 0; //Sirve para saber cuantos productos se asociaron y no recorrer todo el arreglo que puede tener muchos campos vacios
        //String[] productosSeleccionados = new String[gridViewProductosDisponibles.Rows.Count];//podria ser mas grande, como del total de productos disponibles, no se si esto toma en cuenta si hay multipagina
        //for (int i = 0; i < gridViewProductosDisponibles.Rows.Count; i++)
        //{				
        //    GridViewRow fila = gridViewProductosDisponibles.Rows[i];
        //    bool estaSeleccionadoProducto = ((CheckBox)fila.FindControl("checkBoxProductos")).Checked;
        //    if (estaSeleccionadoProducto)
        //    {
        //        String nuevoProducto = gridViewProductosDisponibles.Rows[i].Cells[2].Text.ToString() ;
        //        productosSeleccionados[contador] = nuevoProducto; 
        //        contador++;
        //    }
        //}
    }
}