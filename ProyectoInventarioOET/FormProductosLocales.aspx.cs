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
        private static int modo = 0;                                            //???
        private static Object[] idArray;                                        //???
        private static int estacionSeleccionada, bodegaSeleccionada, pagina;    //???
        private static Object[] idArray2;                                       //???
        private static DataTable catalogoLocal, consultaProducto;               //???
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static Boolean gridCatalogoLocal = false;
        private static bool[] asociados;

        /*
         * Cuando se accede la pagina inicializa los controladores si es la primera vez, sino solo realiza el cambio de modo.
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

                //Seguridad
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Catalogos de productos en bodegas");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                mostrarBotonesSegunPermisos();
            }
            cambiarModo();
        }

        /*
         * 
         */
        protected void mostrarBotonesSegunPermisos()
        {
            botonConsultarBodega.Visible = (permisos[5] == '1');
            botonAsociarBodega.Visible = (permisos[4] == '1');
            botonModificarBodega.Visible = (permisos[3] == '1');
            inputEstado.Enabled = (permisos[2] == '1');
        }

        /*
         * Realiza los cambios de modo que determinan que se puede ver y que no.
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case 0:
                    FieldsetCatalogoLocal.Visible = false;
                    FieldsetAsociarCatalogoLocal.Visible = false;
                    FieldsetProductos.Visible = false;
                    break;
                case 1: // consultar catálogo
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
         * Cargas las columnas del catalogo local de la bodega.
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
         * Cargas las columnas del catalogo local de la bodega.
         */
        protected DataTable tablaAsociacion()
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
            columna.ColumnName = "Categoría";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Intención";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Recarga el catalogo local que se tiene actualmente en memoria en el grid.
         */
        protected void cargarCatalogoLocal()
        {
            if (gridCatalogoLocal)
            {
                FieldsetCatalogoLocal.Visible = true;
            this.gridViewCatalogoLocal.DataSource = catalogoLocal;
            this.gridViewCatalogoLocal.PageIndex = pagina;
            this.gridViewCatalogoLocal.DataBind();
            }else{
                FieldsetAsociarCatalogoLocal.Visible = true;
                this.gridViewAsociarCatalogoLocal.DataSource = catalogoLocal;
                this.gridViewAsociarCatalogoLocal.PageIndex = pagina;
                this.gridViewAsociarCatalogoLocal.DataBind();
            }
        }

        /*
         * Carga los datos del producto consultado en los textos.
         */
        protected void cargarDatosProducto()
        {
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
         * Realiza la consulta del producto seleccionado en la tabla de catalogo local.
         */
        protected void gridViewCatalogoLocal_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewCatalogoLocal.Rows[Convert.ToInt32(e.CommandArgument)];
                    String codigo = filaSeleccionada.Cells[2].Text.ToString();
                    String idBodega = idArray2[bodegaSeleccionada].ToString();
                    consultaProducto = controladoraProductoLocal.consultarProductoDeBodega(idBodega, codigo);
                    modo=2;
                    Response.Redirect("FormProductosLocales.aspx");
                    break;
            }
        }

        /*
         * Realiza el cambio de pagina adentro de los grids
         */
        protected void gridViewCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            pagina = e.NewPageIndex;
            modo = 1;
            Response.Redirect("FormProductosLocales.aspx");
        }
        protected void gridViewAsociarCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            pagina = e.NewPageIndex;
            modo = 1;
            Response.Redirect("FormProductosLocales.aspx");
        }

        /*
         * Carga estaciones las estaciones disponibles en la base de datos
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
         * Seleccion de bodega, se habilitan las opciones de consulta y asociacion. 
         */
        protected void DropDownListBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListBodega.SelectedItem != null)
            {
                botonConsultarBodega.Disabled = false;
                botonAsociarBodega.Disabled = false;
            }
        }


        /*
         * Consulta de bodega, aquí se carga la tabla.
         */
        protected void botonConsultarBodega_ServerClick(object sender, EventArgs e)
        {
            gridCatalogoLocal = true;
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
                    asociados = new bool[productos.Rows.Count];
                    for (int x = 0; x < productos.Rows.Count; x++)
                    {
                        asociados[x]=false;
                    }
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
        /*
         * Consulta de productos no asociados a la bodega seleccionada, se carga la tabla.
         */
        protected void botonAsociarBodega_ServerClick(object sender, EventArgs e)
        {
            gridCatalogoLocal = false;
            if (this.DropDownListBodega.SelectedItem != null)
            {
                bodegaSeleccionada = this.DropDownListBodega.SelectedIndex;
                pagina = 0;
                FieldsetAsociarCatalogoLocal.Visible = true;
                String idBodega = idArray2[bodegaSeleccionada].ToString();
                catalogoLocal = tablaAsociacion();
                DataTable productos = controladoraBodegas.consultarProductosAsociables(idBodega);
                if (productos != null) { 
                    if (productos.Rows.Count > 0)
                    {
                        Object[] datos = new Object[4];
                        int i;
                        foreach (DataRow producto in productos.Rows)
                        {
                            for (i = 0; i < 4; i++)
                            {
                                datos[i] = producto[i];
                            }
                            try
                            {
                                datos[2] = controladoraCategorias.consultarCategoria(producto[2].ToString()).Descripcion;
                            }
                            catch (NullReferenceException excepcion) { datos[2] = ""; }
                            catalogoLocal.Rows.Add(datos);
                        }
                    }
                    this.gridViewAsociarCatalogoLocal.DataSource = catalogoLocal;
                    this.gridViewAsociarCatalogoLocal.PageIndex = pagina;
                    this.gridViewAsociarCatalogoLocal.DataBind();
                }
            }
        }



        /*
         * Realiza la asociacion de los productos confirmados.
         */
        protected void botonAsociarProductos_ServerClick(object sender, EventArgs e)
        {

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
            modo = 0;
            Response.Redirect("FormProductosLocales.aspx");
        }

        /*
         * Desactivación confirmada.
         */
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {

        }

        protected void checkBoxProductos_CheckedChanged(object sender, EventArgs e)
        {

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