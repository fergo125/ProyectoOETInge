using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.Modulo_Productos_Locales;
using ProyectoInventarioOET.Modulo_Categorias;
using ProyectoInventarioOET.Modulo_Seguridad;

namespace ProyectoInventarioOET
{
    /*
     * Catálogos locales de productos en bodega.
     */
    public partial class FormProductosLocales : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static ControladoraBodegas controladoraBodegas;                 // Controladora de bodegas
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Controladora de datos generales
        private static ControladoraProductoLocal controladoraProductoLocal;     // Controladora de productos locales
        private static ControladoraSeguridad controladoraSeguridad;             // Controladora de seguridad
        private static ControladoraCategorias controladoraCategorias;           // Controladora de categorías
        private static int modo = 0;                                            // Modo actual de la página
        private static Object[] idArray;                                        // Arreglo de id's de estaciones
        private static int estacionSeleccionada, bodegaSeleccionada, pagina;    // Variables que almacenan la estación seleccionada, la bodega seleccionada y la página actual del grid
        private static Object[] idArray2;                                       // Arreglo de id's de bodegas
        private static DataTable catalogoLocal, consultaProducto;               // Tablas de datos que almacenan los productos del catálogo local y los datos del producto consultado
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static bool[] asociados;                                        // Almacena cuales itemes estan siendo asociados actualmente.
        private static String[] idProductos;                                    // Almacena el id de los productos mostrados.
        private static Boolean mensaje = false;                                 // Almacena si se debe mostrar el mensaje.
        private String codigoSeleccionado, idBodegaSeleccionada;                // Almacena el codigo y la id de bodega del producto consultado

        /*
         * Cuando se accede la pagina inicializa los controladores si es la primera vez, sino solo realiza el cambio de modo.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            //Elementos visuales
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster

            if (!IsPostBack)
            {
                modo = (int)Modo.Inicial;
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraBodegas = new ControladoraBodegas();
                controladoraProductoLocal = new ControladoraProductoLocal();
                controladoraCategorias = new ControladoraCategorias();
                controladoraSeguridad = new ControladoraSeguridad();
                controladoraBodegas.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraProductoLocal.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraCategorias.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraSeguridad.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                DropDownListEstacion_CargaEstaciones();

                //Seguridad
                
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Catalogos de productos en bodegas");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                mostrarBotonesSegunPermisos();

                // Carga estacion y bodega predeterminados
                DropDownListEstacion_CargaEstaciones();
                String idEstacionSesion = controladoraBodegas.consultarEstacionDeBodega((this.Master as SiteMaster).LlaveBodegaSesion).Rows[0][0].ToString();
                for (int i = 0; i < idArray.Length; i++)
                {
                    if(idEstacionSesion.Equals(idArray[i])){
                        DropDownListEstacion.SelectedIndex=i;
                    }
                }
                DropDownListEstacion_SelectedIndexChanged(null, null);
                DropDownListBodega.SelectedValue = (this.Master as SiteMaster).NombreBodegaSesion;
                if (Convert.ToInt32((this.Master as SiteMaster).Usuario.CodigoPerfil) > 2){
                   DropDownListEstacion.Enabled = false;
                   DropDownListBodega.Enabled = false;
                }

                cambiarModo();
            }
        }

        /*
         * Realiza los cambios de modo que determinan que se puede ver y que no.
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial:
                    FieldsetProductos.Visible = false;
                    FieldsetCatalogoLocal.Visible = false;
                    FieldsetAsociarCatalogoLocal.Visible = false;
                    FieldsetBloqueBotones.Visible = false;
                    botonModificarProductoLocal.Disabled = true;
                    break;
                case (int)Modo.Insercion:
                    FieldsetProductos.Visible = false;
                    FieldsetCatalogoLocal.Visible = false;
                    FieldsetAsociarCatalogoLocal.Visible = true;
                    FieldsetBloqueBotones.Visible = true;
                    botonAsociarProducto.Visible = true;
                    botonDesactivarProducto.Visible = false;
                    botonModificarProductoLocal.Disabled = true;
                    break;
                case (int)Modo.Consulta:
                    FieldsetProductos.Visible = false;
                    FieldsetCatalogoLocal.Visible = true;
                    FieldsetAsociarCatalogoLocal.Visible = false;
                    FieldsetBloqueBotones.Visible = false;
                    botonModificarProductoLocal.Disabled = true;
                    break;
                case (int)Modo.Modificacion:
                    FieldsetProductos.Visible = true;
                    FieldsetCatalogoLocal.Visible = false;
                    FieldsetAsociarCatalogoLocal.Visible = false;
                    FieldsetBloqueBotones.Visible = true;
                    botonAsociarProducto.Visible = false;
                    botonDesactivarProducto.Visible = true;
                    cargarDatosProducto();
                    inputMinimo.Disabled = false;
                    inputMaximo.Disabled = false;
                    break;
                case (int)Modo.Consultado:
                    FieldsetProductos.Visible = true;
                    FieldsetCatalogoLocal.Visible = true;
                    FieldsetAsociarCatalogoLocal.Visible = false;
                    FieldsetBloqueBotones.Visible = false;
                    botonModificarProductoLocal.Disabled = false;
                    cargarDatosProducto();
                    inputEstado.Enabled = false;
                    inputMinimo.Disabled = true;
                    inputMaximo.Disabled = true;
                    break;
            }
            if (mensaje)
            {
                mensajeAlerta.Visible = true;
                mensaje = false;
            }
            else
            {
                mensajeAlerta.Visible = false;
            }
        }

        /*
         *  Decide que botones mostrar según los permisos del usuario.
         */
        protected void mostrarBotonesSegunPermisos()
        {
            if (permisos == null)
            {
                permisos = "000000";
            }
            botonConsultarBodega.Visible = (permisos[5] == '1');
            botonAsociarBodega.Visible = (permisos[4] == '1');
            botonModificarProductoLocal.Visible = (permisos[3] == '1');
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

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Advertencias";
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
                inputEstado.Items.Clear();
                foreach (DataRow estado in estados.Rows)
                {
                    inputEstado.Items.Add(estado[1].ToString());
                    if (estado[2].ToString() == producto[6])
                    {
                        inputEstado.SelectedValue = estado[1].ToString();
                    }
                }

                inputNombre.Value = producto[0].ToString();
                inputCodigo.Value = producto[1].ToString();
                inputCodigoBarras.Value = producto[2].ToString();
                inputCategoria.Value = controladoraCategorias.consultarCategoria(producto[3]).Descripcion;
                inputVendible.Value = producto[4].ToString();
                inputUnidades.Value = producto[5].ToString();
                inputSaldo.Value = producto[7].ToString();
                inputImpuesto.Value = "No";
                if (producto[8].Equals("1"))
                {
                    inputImpuesto.Value = "Sí";
                }
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
            if (e.CommandName == "Select")
            {
                GridViewRow filaSeleccionada = this.gridViewCatalogoLocal.Rows[Convert.ToInt32(e.CommandArgument)];
                codigoSeleccionado = filaSeleccionada.Cells[2].Text.ToString();
                idBodegaSeleccionada = idArray2[bodegaSeleccionada].ToString();
                consultaProducto = controladoraProductoLocal.consultarProductoDeBodega(idBodegaSeleccionada, codigoSeleccionado);
                modo = (int)Modo.Consultado;
                cambiarModo();
            }
        }

        /*
         * El botón entra a modo de modificación de productos locales, oculta el grid del catálogo local.
         */
        protected void botonModificarProductoLocal_ServerClick(object sender, EventArgs e)
        {
            if (consultaProducto.Rows.Count > 0)
            {
                DataRow productos = consultaProducto.Rows[0];
                if (productos[7].ToString().Equals("0"))
                {
                    inputEstado.Enabled = true;
                }
                modo = (int)Modo.Modificacion;
                cambiarModo();
            }
        }

        /*
         * Realiza el cambio de pagina adentro de los grids.
         */
        protected void gridViewCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            this.gridViewCatalogoLocal.DataSource = catalogoLocal;
            this.gridViewCatalogoLocal.PageIndex = e.NewPageIndex;
            this.gridViewCatalogoLocal.DataBind();
        }
        protected void gridViewAsociarCatalogoLocal_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            int pos = gridViewAsociarCatalogoLocal.PageSize * gridViewAsociarCatalogoLocal.PageIndex;
            for (int i = 0; i < gridViewAsociarCatalogoLocal.PageSize; i++)
            {
                try
                {
                    GridViewRow fila = gridViewAsociarCatalogoLocal.Rows[i];
                    bool estaSeleccionadoProducto = ((CheckBox)fila.FindControl("checkBoxProductos")).Checked;
                    if (estaSeleccionadoProducto)
                    {
                        asociados[pos] = true;
                    }
                    else
                    {
                        asociados[pos] = false;
                    }
                    pos++;
                }
                catch (ArgumentOutOfRangeException) { break; }

            }
            this.gridViewAsociarCatalogoLocal.DataSource = catalogoLocal;
            this.gridViewAsociarCatalogoLocal.PageIndex = e.NewPageIndex;
            this.gridViewAsociarCatalogoLocal.DataBind();
            pos = gridViewAsociarCatalogoLocal.PageSize * gridViewAsociarCatalogoLocal.PageIndex;
            for (int i = 0; i < gridViewAsociarCatalogoLocal.PageSize; i++)
            {
                try
                {
                    GridViewRow fila = gridViewAsociarCatalogoLocal.Rows[i];
                    ((CheckBox)fila.FindControl("checkBoxProductos")).Checked = asociados[pos];
                    pos++;
                }
                catch (ArgumentOutOfRangeException) { break;  }
            }

        }

        /*
         * Carga estaciones las estaciones disponibles en la base de datos.
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
         * Selección de estación, se cargan las bodegas disponibles.
         */
        protected void DropDownListEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
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
        }
        /*
         * Selección de bodega, se habilitan las opciones de consulta y asociacion. 
         */
        protected void DropDownListBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListBodega.SelectedItem != null)
            {
                botonConsultarBodega.Disabled = false;
                botonAsociarBodega.Disabled = false;
                modo = (int)Modo.Inicial;
                cambiarModo();
            }
        }

        /*
         * Consulta de bodega, aquí se carga la tabla.
         */
        protected void botonConsultarBodega_ServerClick(object sender, EventArgs e)
        {
            if (this.DropDownListBodega.SelectedItem != null)
            {
                modo = (int)Modo.Consulta;
                cambiarModo();
                bodegaSeleccionada = this.DropDownListBodega.SelectedIndex;
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
                    Object[] datos = new Object[6];  
                    int i;
                    foreach (DataRow producto in productos.Rows)
                    {
                        for (i = 0; i < 5; i++)  
                        {
                            datos[i] = producto[i+1];  //Cambio Carlos
                        }
                        datos[5] = "";
                        try
                        {
                            if (Convert.ToDouble(datos[2].ToString()) <= Convert.ToDouble(datos[3].ToString()))
                            {
                                datos[5] = "Debajo del mínimo";

                            }
                            else
                            {
                                if (Convert.ToDouble(datos[2].ToString()) >= Convert.ToDouble(datos[4].ToString()))
                                {
                                    datos[5] = "Encima del máximo";
                                }
                            }
                        }
                        catch (SystemException) { };
                        catalogoLocal.Rows.Add(datos);
                    }
                }
                this.gridViewCatalogoLocal.DataSource = catalogoLocal;
                this.gridViewCatalogoLocal.PageIndex = 0;
                this.gridViewCatalogoLocal.DataBind();
            }
        }
        /*
         * Consulta de productos no asociados a la bodega seleccionada, se carga la tabla.
         */
        protected void botonAsociarBodega_ServerClick(object sender, EventArgs e)
        {
            if (this.DropDownListBodega.SelectedItem != null)
            {
                modo = (int)Modo.Insercion;
                cambiarModo();
                bodegaSeleccionada = this.DropDownListBodega.SelectedIndex;
                String idBodega = idArray2[bodegaSeleccionada].ToString();
                catalogoLocal = tablaAsociacion();
                DataTable productos = controladoraBodegas.consultarProductosAsociables(idBodega);
                if (productos != null) { 
                    if (productos.Rows.Count > 0)
                    {
                        asociados = new bool[productos.Rows.Count];
                        idProductos = new String[productos.Rows.Count];
                        Object[] datos = new Object[4];
                        int j=0;
                        foreach (DataRow producto in productos.Rows)
                        {
                            idProductos[j] = producto[4].ToString();
                            for (int i = 0; i < 4; i++)
                            {
                                datos[i] = producto[i];
                            }
                            try
                            {
                                datos[2] = controladoraCategorias.consultarCategoria(producto[2].ToString()).Descripcion;
                            }
                            catch (NullReferenceException excepcion) { datos[2] = ""; }
                            catalogoLocal.Rows.Add(datos);
                            ++j;
                        }
                    }
                    this.gridViewAsociarCatalogoLocal.DataSource = catalogoLocal;
                    this.gridViewAsociarCatalogoLocal.PageIndex = 0;
                    this.gridViewAsociarCatalogoLocal.DataBind();
                }
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
            FormProductosLocales.mensaje = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        }

        /*
         * Aceptar cancelación del modal de cancelar.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        /*
         * Desactivación confirmada.
         */
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {
            String[] res = new String[3];
            res = controladoraProductoLocal.modificarProductoLocal(consultaProducto.Rows[0][22].ToString(), inputEstado.SelectedItem.ToString(),inputMinimo.Value,inputMaximo.Value);
            botonConsultarBodega_ServerClick(null, null);
            consultaProducto = controladoraProductoLocal.consultarProductoDeBodega(idBodegaSeleccionada, codigoSeleccionado);
            modo = (int)Modo.Consultado;
            mostrarMensaje(res[0], res[1], res[2]);
            cambiarModo();
        }

        /*
         * Asociación confirmada.
         */
        protected void botonAceptarModalAsociar_ServerClick(object sender, EventArgs e)
        {
            String[] res = new String[3];
            int pos = gridViewAsociarCatalogoLocal.PageSize * gridViewAsociarCatalogoLocal.PageIndex;
            for (int i = 0; i < gridViewAsociarCatalogoLocal.PageSize; i++)
            {
                GridViewRow fila = gridViewAsociarCatalogoLocal.Rows[i];
                bool estaSeleccionadoProducto = ((CheckBox)fila.FindControl("checkBoxProductos")).Checked;
                if (estaSeleccionadoProducto)
                {
                    asociados[pos] = true;
                }
                else
                {
                    asociados[pos] = false;
                }
                pos++;
            }
            String idBodega = idArray2[bodegaSeleccionada].ToString();
            res[0] = "go";
            for (int i = 0; i < catalogoLocal.Rows.Count && !res[0].Equals("danger"); i++)
            {
                if (asociados[i])
                {
                    res = controladoraProductoLocal.asociarProductos(idBodega, idProductos[i], (this.Master as SiteMaster).Usuario.Codigo);
                }
            }
            modo = (int)Modo.Inicial;
            mostrarMensaje(res[0], res[1], res[2]);
            cambiarModo();
        }



    }
}