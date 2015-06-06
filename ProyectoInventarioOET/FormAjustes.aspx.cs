using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.App_Code;
using ProyectoInventarioOET.App_Code.Modulo_Ajustes;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_Productos_Locales;


namespace ProyectoInventarioOET
{
    public partial class FormAjustes : System.Web.UI.Page
    {

        enum Modo { Inicial, Consulta, Insercion, Consultado };

        // Atributos
        private static Boolean seConsulto = false;                              // True si se consulto y se debe visitar la base de datos
        private static Object[] idArrayAjustes;                                 // Array de llaves que no se muestran en el grid de consultas
        private static Object[] idArrayProductos;                               // Array de llaves que no se muestran en el grid de productos
        private static Object[] idArrayAgregarProductos;                        // Array de llaves que no se muestran en el grid de agregar productos
        private static DataTable tablaAgregarProductos;                         // Tabla en memoria de los productos agregables
        private static DataTable tablaProductos;                                // Tabla en memoria de los productos agregados
        private static int modo = (int)Modo.Inicial;                            // Modo actual de interfaz
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Controladora de datos generales
        private static ControladoraAjustes controladoraAjustes;                 // Controladora del modulo ajustes
        private static ControladoraProductoLocal controladoraProductosLocales;  // Controladora de catálogos locales
        private static EntidadAjustes ajusteConsultado;                         // El ajuste mostrado en pantalla
        private static bool[] signos;

        // DataTable bodegas = controladoraBodegas.consultarBodegasDeEstacion(idEstacion);

        /*
         * Método llamado cada vez que se carga la página.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            //Elementos visuales
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
            mensajeAlerta.Visible = false;

            if (!IsPostBack)
            {
                labelAlerta.Text = "";

                controladoraAjustes = new ControladoraAjustes();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraProductosLocales = new ControladoraProductoLocal();

                /*
                //Seguridad
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Gestion de bodegas");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                mostrarBotonesSegunPermisos();
                */

                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (ajusteConsultado == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar el ajuste.");
                    }
                    else
                    {
                        cargarTipos();
                        setDatosConsultados();

                        seConsulto = false;
                    }
                }
            }
            cambiarModo();

        }

        /*
         * Maneja la activación y desactivación de objetos dependiendo de la operación a realizar por el usuario
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial: //modo inicial
                    limpiarCampos();
                    botonAgregar.Visible = false;
                    FieldsetAjustes.Visible = false;
                    botonAceptarAjustes.Visible = false;
                    botonCancelarAjustes.Visible = false;
                    tituloAccionAjustes.InnerText = "Seleccione una opción";
                    botonRealizarAjuste.Disabled = false;
                    botonConsultarAjustes.Disabled = false;
                    tituloGridProductos.Visible = false;
                    tituloGridConsulta.Visible = false;
                    gridViewAjustes.Visible = false;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = false;
                    habilitarCampos(false);
                    break;

                case (int)Modo.Insercion: //insertar
                    botonAgregar.Visible = true;
                    FieldsetAjustes.Visible = true;
                    botonAceptarAjustes.Visible = true;
                    botonCancelarAjustes.Visible = true;
                    tituloAccionAjustes.InnerText = "Ingrese datos";
                    botonRealizarAjuste.Disabled = true;
                    botonConsultarAjustes.Disabled = false;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = false;
                    gridViewAjustes.Visible = false;
                    gridViewProductos.Enabled = true;
                    gridViewProductos.Visible = true;
                    habilitarCampos(true);
                    gridViewProductos.Columns[1].Visible = true;
                    foreach( DataControlField col in gridViewProductos.Columns )
                        col.Visible = true;
                    break;

                case (int)Modo.Consulta://consultar
                    botonAgregar.Visible = false;
                    FieldsetAjustes.Visible = false;
                    botonAceptarAjustes.Visible = false;
                    botonCancelarAjustes.Visible = false;
                    tituloAccionAjustes.InnerText = "Seleccione un ajuste";
                    botonRealizarAjuste.Disabled = false;
                    botonConsultarAjustes.Disabled = true;
                    tituloGridProductos.Visible = false;
                    tituloGridConsulta.Visible = true;
                    gridViewAjustes.Visible = true;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = false;
                    habilitarCampos(false);
                    break;

                case (int)Modo.Consultado://consultado, pero con los espacios bloqueados
                    botonAgregar.Visible = false;
                    FieldsetAjustes.Visible = true;
                    botonAceptarAjustes.Visible = false;
                    botonCancelarAjustes.Visible = false;
                    tituloAccionAjustes.InnerText = "Ajuste seleccionado";
                    botonRealizarAjuste.Disabled = false;
                    botonConsultarAjustes.Disabled = false;
                    tituloGridProductos.Visible = true;
                    tituloGridConsulta.Visible = false;
                    gridViewAjustes.Visible = false;
                    gridViewProductos.Enabled = false;
                    gridViewProductos.Visible = true;
                    habilitarCampos(false);
                    foreach (DataControlField col in gridViewProductos.Columns)
                        col.Visible = false;
                    llenarGrid();
                    break;

                default:
                    // Algo salio mal
                    break;
            }
        }

        /*
         * Toma la entidad consultada y carga su información en la interfaz
         */
        protected void setDatosConsultados()
        {
            this.dropdownTipo.SelectedValue = ajusteConsultado.IdTipoAjuste;
            this.outputUsuario.Value = ajusteConsultado.Usuario;
            this.outputFecha.Value = ajusteConsultado.Fecha.ToString();
            this.inputNotas.Text = ajusteConsultado.Notas;
            this.outputBodega.Value = ((this.Master as SiteMaster).NombreBodegaSesion);

            // Manejo grid
            DataTable tabla = tablaProductoConsulta();
            Object[] datos = new Object[3];
            if (ajusteConsultado.Detalles.Count > 0)
            {
                foreach (EntidadDetalles elemento in ajusteConsultado.Detalles)
                {
                    datos[0] = elemento.NombreProducto;
                    datos[1] = elemento.Codigo;
                    datos[2] = elemento.Cambio;
                    tabla.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                datos[2] = 0;
                tabla.Rows.Add(datos);
            }
            

            gridViewProductos.DataSource = tabla;
            gridViewProductos.DataBind();
        }


        /*
         * Limpia los campos editables
         */
        protected void limpiarCampos()
        {
            if (dropdownTipo.Items.Count > 0)
                dropdownTipo.SelectedIndex = 0;
            vaciarGridProductos();
            inputNotas.Text = "";
        }

        /*
         * Habilita o desabilita los campos editables
         */
        protected void habilitarCampos(bool habilitar)
        {
            this.dropdownTipo.Enabled = habilitar;
            this.inputNotas.Enabled = habilitar;
            gridViewProductos.Enabled = habilitar;
            // Habilitar/Desabilitar botones de grid
        }

        /*
         * Actualiza el contenido del mensaje y lo hace visible
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {

            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        /*
         * Viaja a la base de datos y obtiene los datos de consulta
         */
        protected void llenarGrid()
        {
            DataTable tabla = tablaAjustes();
            int indiceNuevoAjuste = -1;
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[3];

                DataTable ajustes = controladoraAjustes.consultarAjustes((this.Master as SiteMaster).LlaveBodegaSesion);
                //DataTable ajustes = controladoraAjustes.consultarAjustes("PITAN129012015101713605001");

                if (ajustes.Rows.Count > 0)
                {
                    idArrayAjustes = new Object[ajustes.Rows.Count];
                    foreach (DataRow fila in ajustes.Rows)
                    {
                        idArrayAjustes[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[2].ToString();
                        datos[2] = fila[3].ToString();
                        tabla.Rows.Add(datos);
                        /*
                        if (ajusteConsultado != null && (fila[0].Equals(ajusteConsultado.Codigo)))
                        {
                            indiceNuevoAjuste = i;
                        }
                         */
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = Convert.ToDateTime("01/01/1997");
                    datos[2] = "-";
                    tabla.Rows.Add(datos);
                    mostrarMensaje("warning", "Atención: ", "No existen ajustes en la bodega actual.");
                }

                this.gridViewAjustes.DataSource = tabla;
                this.gridViewAjustes.DataBind();
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * Carga datos del grid de productos agregables
         */
        protected void llenarGridAgregarProductos()
        {
            
            DataTable tabla = tablaAgregarProducto();
            int i = 0;

            try
            {
                // Cargar bodegas
                Object[] datos = new Object[6];


                DataTable productos = controladoraAjustes.consultarProductosDeBodega((this.Master as SiteMaster).LlaveBodegaSesion);

                if (productos.Rows.Count > 0)
                {
                    idArrayAgregarProductos = new Object[productos.Rows.Count];
                    foreach (DataRow fila in productos.Rows)
                    {
                        idArrayAgregarProductos[i] = fila[0];
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[2].ToString();
                        datos[2] = Convert.ToDouble(fila[3].ToString());
                        datos[3] = fila[6].ToString();
                        datos[4] = Convert.ToDouble(fila[4].ToString());
                        datos[5] = Convert.ToDouble(fila[5].ToString());
                        tabla.Rows.Add(datos);
                        i++;
                    }
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "0";
                    datos[3] = "-";
                    datos[4] = "0";
                    datos[5] = "0";
                    tabla.Rows.Add(datos);
                    mostrarMensaje("warning", "Atención: ", "No existen productos en la bodega actual.");
                }

                this.gridViewAgregarProductos.DataSource = tabla;
                this.gridViewAgregarProductos.DataBind();
                tablaAgregarProductos = tabla;
            }
            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
            
        }

        /*
         * Limpia el grid de productos
         */
        protected void vaciarGridProductos()
        {
            DataTable tablaLimpia = tablaProducto();

            Object[] datos = new Object[4];
            datos[0] = "-";
            datos[1] = "-";
            datos[2] = "0";
            datos[3] = "-";
            tablaLimpia.Rows.Add(datos);

            gridViewProductos.DataSource = tablaLimpia;
            gridViewProductos.DataBind();

            idArrayProductos = new Object[0];
            tablaProductos = tablaProducto();
        }

        /*
         * Crea una datatable en el formato del grid de consultas
         */
        protected DataTable tablaAjustes()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Tipo de Ajuste";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.DateTime");
            columna.ColumnName = "Fecha";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Encargado";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Crea una datatable en el formato del grid de productos en ajustes
         */
        protected DataTable tablaProducto()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Código";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Cantidad Actual";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Unidad Métrica";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Crea una datatable en el formato del grid de productos en ajustes, cuando son consultados
         */
        protected DataTable tablaProductoConsulta()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Código";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Ajuste de cambio";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Crea una datatable en el formato del grid de agregar productos
         */
        protected DataTable tablaAgregarProducto()
        {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Código";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Cantidad Actual";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Unidad Métrica";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Minimo";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.Double");
            columna.ColumnName = "Maximo";
            tabla.Columns.Add(columna);

            return tabla;
        }


        /*
         * Este metodo carga los tipos de movimiento en
         */
        protected void cargarTipos()
        {
            dropdownTipo.Items.Clear();
            DataTable tipos = controladoraAjustes.tiposAjuste();
            signos = new bool [tipos.Rows.Count];
            int i = 0;
            foreach (DataRow fila in tipos.Rows)
            {
                dropdownTipo.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                signos[i] = fila[2].ToString().Equals("1") ? true : false;
                i++;
            }
            dropdownTipo.SelectedIndex = 0;
        }


        /*
         * Esto pasa la interfaz al modo de crear ajustes.
         */
        protected void botonRealizarAjuste_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            llenarGridAgregarProductos();
            vaciarGridProductos();

            cargarTipos();
            if( (this.Master as SiteMaster).Usuario != null )
                outputUsuario.Value = (this.Master as SiteMaster).Usuario.Nombre;
            outputBodega.Value = (this.Master as SiteMaster).NombreBodegaSesion;
            outputFecha.Value = DateTime.Now.ToString();
        }

        /*
         * Esto pasa la interfaz al modo de consulta.
         */
        protected void botonConsultarAjustes_ServerClick(object sender, EventArgs e)
        {
            llenarGrid();
            modo = (int)Modo.Consulta;
            cambiarModo();
        }

        /*
         * Limpia el grid de consulta
         */
        protected void vaciarGridAjustes()
        {
            DataTable tablaLimpia = null;
            gridViewAjustes.DataSource = tablaLimpia;
            gridViewAjustes.DataBind();
        }

        /*
         * Método auxiliar que viaja a la base de datos y maneja la consulta de ajustes
         */
        protected void consultarAjuste(String id)
        {
            seConsulto = true;
            try
            {
                ajusteConsultado = controladoraAjustes.consultarAjuste(id);
                modo = (int)Modo.Consulta;
            }
            catch
            {
                ajusteConsultado = null;
                modo = (int)Modo.Inicial;
            }
            cambiarModo();
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de consultar.
         */
        protected void gridViewAjustes_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            if (idArrayAjustes != null && idArrayAjustes.Count() > 0)
            {
                switch (e.CommandName)
                {
                    case "Select":
                        GridViewRow filaSeleccionada = this.gridViewAjustes.Rows[Convert.ToInt32(e.CommandArgument)];
                        //String codigo = filaSeleccionada.Cells[0].Text.ToString();
                        String codigo = Convert.ToString(idArrayAjustes[Convert.ToInt32(e.CommandArgument) + (this.gridViewAjustes.PageIndex * this.gridViewAjustes.PageSize)]);
                        consultarAjuste(codigo);
                        modo = (int)Modo.Consultado;
                        Response.Redirect("FormAjustes.aspx");
                        break;
                }
            }
        }

        /*
         * Método que maneja el cambio de páginas en el grid de consultar
         */
        protected void gridViewAjustes_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGrid();
            this.gridViewAjustes.PageIndex = e.NewPageIndex;
            this.gridViewAjustes.DataBind();
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de productos.
         */
        protected void gridViewProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            if (idArrayProductos != null && idArrayProductos.Count() > 0)
            {
                switch (e.CommandName)
                {
                    case "Select":
                        int indice = Convert.ToInt32(e.CommandArgument);

                        // Eliminar vieja tupla de grid
                        tablaProductos.Rows[indice].Delete();
                        gridViewProductos.DataSource = tablaProductos;
                        gridViewProductos.DataBind();

                        // Actualizar listas de Ids
                        List<Object> temp = new List<Object>(idArrayProductos);
                        temp.RemoveAt(indice);
                        idArrayProductos = temp.ToArray();

                        if (idArrayProductos.Count() < 1)
                            vaciarGridProductos();

                        break;
                }
            }
        }

        /*
         * Método que maneja la selección de un ajuste en el grid de agregar productos.
         */
        protected void gridViewAgregarProductos_Seleccion(object sender, GridViewCommandEventArgs e)
        {
            if (idArrayAgregarProductos != null && idArrayAgregarProductos.Count() > 0)
            {
                switch (e.CommandName)
                {
                    case "Select":
                        int indice = Convert.ToInt32(e.CommandArgument) + (this.gridViewAgregarProductos.PageIndex * this.gridViewAgregarProductos.PageSize);
                        DataRow seleccionada = tablaAgregarProductos.Rows[indice];

                        // Sacamos datos pertinentes del producto
                        Object[] datos = new Object[4];
                        datos[0] = seleccionada["Nombre"];
                        datos[1] = seleccionada["Código"];
                        datos[2] = seleccionada["Cantidad Actual"];
                        datos[3] = seleccionada["Unidad Métrica"];

                        // Agregar nueva tupla a tabla
                        tablaProductos.Rows.Add(datos);
                        gridViewProductos.DataSource = tablaProductos;
                        gridViewProductos.DataBind();

                        // Eliminar vieja tupla de grid
                        tablaAgregarProductos.Rows[Convert.ToInt32(e.CommandArgument) + (this.gridViewAgregarProductos.PageIndex * this.gridViewAgregarProductos.PageSize)].Delete();
                        gridViewAgregarProductos.DataSource = tablaAgregarProductos;
                        gridViewAgregarProductos.DataBind();

                        // Actualizar listas de Ids
                        List<Object> temp = new List<Object>(idArrayProductos);
                        temp.Add(idArrayAgregarProductos[indice]);
                        idArrayProductos = temp.ToArray();

                        temp = new List<Object>(idArrayAgregarProductos);
                        temp.RemoveAt(indice);
                        idArrayAgregarProductos = temp.ToArray();

                        //Response.Redirect("FormAjustes.aspx");
                        break;
                }
            }
        }

        /*
         * Retorna la información del ajuste como un array de objetos
         */
        protected Object[] obtenerDatosAjuste()
        {
            Object[] datos = new Object[6];
            bool fun = signos[this.dropdownTipo.SelectedIndex];
            datos[0] = this.dropdownTipo.SelectedValue;
            datos[1] = this.outputFecha.Value;
            datos[2] = "";
            datos[3] = (this.Master as SiteMaster).Usuario.Codigo;
            datos[4] = this.inputNotas.Text;
            datos[5] = (this.Master as SiteMaster).LlaveBodegaSesion;
            return datos;
        }


        /*
         * Esto maneja la inserción de datos
         */
        protected String insertar()
        {
            String codigo = "";
            Object[] ajuste = obtenerDatosAjuste();
            EntidadAjustes nueva = new EntidadAjustes(ajuste);
            DataTable productoDeBodega;
            bool signo = signos[ dropdownTipo.SelectedIndex ];
            bool alerta = false;

            // Agregar detalles a entidad

            int i = 0;
            try
            {
                if (tablaProductos.Rows.Count ==0) { // Caso en que no se agrego nada
                    throw new NoNullAllowedException();
                }
                foreach (DataRow row in tablaProductos.Rows)
                {
                    if (((TextBox)gridViewProductos.Rows[i].FindControl("textAjustes")).Text=="") // Caso en que no se especifico un ajuste
                        throw new NullReferenceException();

                    Double cantAjuste = Double.Parse(((TextBox)gridViewProductos.Rows[i].FindControl("textAjustes")).Text);
                    if( cantAjuste < 0 )
                        throw new InvalidConstraintException();


                    ajuste = new Object[5];
                    ajuste[0] = ajuste[1] = "";
                    ajuste[2] = cantAjuste;
                    ajuste[3] = idArrayProductos[i];
                    ajuste[4] = cantAjuste - Double.Parse(row["Cantidad Actual"].ToString());

                    if (!(signo ^ (Convert.ToDouble(ajuste[4]) < 0.0)))
                        throw new AggregateException();

                    nueva.agregarDetalle(ajuste);
                    productoDeBodega = controladoraProductosLocales.consultarMinimoMaximoProductoEnBodega(idArrayProductos[i].ToString());
                    alerta |= cantAjuste <= Convert.ToDouble(productoDeBodega.Rows[0][0].ToString()) || cantAjuste >= Convert.ToDouble(productoDeBodega.Rows[0][1].ToString());
                    ++i;
                }

                String[] error = controladoraAjustes.insertarAjuste(nueva);
                codigo = Convert.ToString(error[3]);
                if (error[0].Contains("success"))
                {
                    llenarGrid();
                    if (alerta)
                    {
                        error[0] = "warning";
                        error[2] += "\nUno o más productos han salido de sus límites permitidos (nivel máximo o mínimo), revise el catálogo local.";
                    }
                }
                else
                {
                    codigo = "";
                    modo = (int)Modo.Insercion;
                }

                mostrarMensaje(error[0], error[1], error[2]);
            }
            catch (NoNullAllowedException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "No puede realizarse un ajuste sin productos");
            }
            catch (AggregateException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "La nueva cantidad de uno o más productos no concuerda con el tipo de ajuste");
            }
            catch (InvalidConstraintException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "Está tratando de ajustar 0 instancias de un producto");
            }
            catch (NullReferenceException e)
            {
                codigo = "";
                modo = (int)Modo.Insercion;
                mostrarMensaje("danger", "Error: ", "Introducir cantidad a ajustar");
            }

            return codigo;
        }


        /*
         * Este método confirma inserción de ajustes.
         */
        protected void botonAceptarAjustes_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";

            if (modo == (int)Modo.Insercion)
            {
                codigoInsertado = insertar();

                if (codigoInsertado != "")
                {
                    operacionCorrecta = true;
                    ajusteConsultado = controladoraAjustes.consultarAjuste(codigoInsertado);
                    modo = (int)Modo.Inicial;
                    habilitarCampos(false);
                    cambiarModo();
                }
                else
                    operacionCorrecta = false;
            }
        }

        /*
         * Método que maneja la aceptar la cancelación.
         * Elimina datos y reinicia la interfaz.
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            vaciarGridAjustes();
            modo = (int)Modo.Inicial;
            cambiarModo();
            limpiarCampos();
            ajusteConsultado = null;
        }

        protected void gridViewAgregarProductos_CambioPagina(Object sender, GridViewPageEventArgs e)
        {
            llenarGridAgregarProductos();
            this.gridViewAgregarProductos.PageIndex = e.NewPageIndex;
            this.gridViewAgregarProductos.DataBind();
        }
    }
}