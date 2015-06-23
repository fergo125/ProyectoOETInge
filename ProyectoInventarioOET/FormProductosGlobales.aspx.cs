using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_ProductosGlobales;
using ProyectoInventarioOET.App_Code;


namespace ProyectoInventarioOET
{
    /*
     * Controla las operaciones en la interfaz y comunica con la controladora.
     */
    public partial class FormProductosGlobales : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado }; // Sirve para controlar los modos de la interfaz
        //Atributos
        private static int modo = (int)Modo.Inicial;                            // Almacena el modo actual de la interfaz
        private static Boolean seConsulto = false;                              // Bandera para saber si hubo consulta de una actividad.
        private static Object[] idArray;                                        // Contiene los ids de los productos del grid para ser consultados especificamente
        private static EntidadProductoGlobal productoConsultado;                // Almacena una entidad/producto consultado
        private static ControladoraDatosGenerales controladoraDatosGenerales;   // Controladora para obtener datos generales
        private static ControladoraProductosGlobales controladora;              // Controladora para obtener datos y manejar lógica de negocio.
        private static String permisos = "000000";                              // Permisos utilizados para el control de seguridad.
        private static DataTable consulta;
        private static String argumentoSorteo = "";
        private static bool boolSorteo = false;

        /*
         * Maneja las acciones que se ejecutan cuando se carga la página, establecer el modo de operación, 
         * cargar elementos de la interfaz, gestión de seguridad.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            mensajeAlerta.Visible = false;
            //Elementos visuales
            ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster

            if (!IsPostBack)
            {
                controladora = new ControladoraProductosGlobales();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Catalogo general de productos");
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");

                // Esconder botones
                mostrarBotonesSegunPermisos();

                if (!seConsulto)
                {
                    modo = (int)Modo.Inicial;
                }
                else
                {
                    if (productoConsultado == null)
                    {
                        mostrarMensaje("warning", "Alerta: ", "No se pudo consultar el producto.");
                    }
                    else
                    { // Caso en que se hizo la consulta de un producto
                        cargarEstados();
                        cargarUnidades();
                        cargarVendible();
                        cargarCategorias();
                        cargarImpuesto();
                        presentarDatos();
                        seConsulto = false;
                    }
                }
            }
            cambiarModo(); // Al cargar la página sin importar se cambia de modo
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

        /*
         * Cambia el modo de la pantalla activando/desactivando o mostrando/ocultando elementos de acuerdo con los
         * permisos del usuario
         */
        protected void mostrarBotonesSegunPermisos()
        {
            botonConsultaProductos.Visible = (permisos[5] == '1');
            botonAgregarProductos.Visible = (permisos[4] == '1');
            botonModificacionProductos.Visible = (permisos[3] == '1');
            inputEstado.Enabled = (permisos[2] == '1');
        }

        /*
         * Cambia el modo de la pantalla activando/desactivando o mostrando/ocultando elementos de acuerdo con la 
         * acción que se va a realizar.
         */
        protected void cambiarModo()
        {
            switch (modo)
            {
                case (int)Modo.Inicial:
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = false;
                    this.bloqueBotones.Visible = false;
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    habilitarCampos(false);
                    tituloAccion.InnerText = "Seleccione una opción";
                    limpiarCampos();
                    break;
                case (int)Modo.Insercion: //insertar
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = true;
                    this.bloqueBotones.Visible = true;
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    tituloAccion.InnerText = "Ingrese datos";
                    break;
                case (int)Modo.Modificacion: //modificar
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = true;
                    this.bloqueBotones.Visible = true;
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    tituloAccion.InnerText = "Cambie los datos";
                    break;
                case (int)Modo.Consulta:
                    this.bloqueGrid.Visible = true;
                    this.gridViewProductosGlobales.Visible = true;
                    this.bloqueFormulario.Visible = false;
                    this.bloqueBotones.Visible = false;
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    tituloAccion.InnerText = "Seleccione un producto";
                    break;
                case (int)Modo.Consultado:
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = true;
                    this.bloqueBotones.Visible = false;
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = false;
                    this.botonConsultaProductos.Disabled = false;
                    habilitarCampos(false);
                    tituloAccion.InnerText = "Producto seleccionado";
                    break;
                default:
                    break;
            }
        }

        //*****************METODOS DE LLENADO DE DROPDOWNLIST Y GENERACION DE DICCIONARIOS PARA LLENADO DE GRID ********************


        /*
        * Carga las posibles categorías para un producto en el 'comboBox' establecido para esto.
        */
        protected void cargarCategorias()
        {
            inpuCategoria.Items.Clear();
            DataTable categorias = controladora.consultarCategorias(); // Hacer un llamado al metodo de Fernando
            foreach (DataRow fila in categorias.Rows)
            {
                inpuCategoria.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        /*
        * Guarda en memoria tanto el nombre y el id de la categoría para mostrar el nombre de la categoría en el grid  
        * en lugar de su id 
        */
        protected Dictionary<String, String> traduccionCategorias()
        {
            Dictionary<String, String> mapCategorias = new Dictionary<String, String>(); ;
            DataTable categorias = controladora.consultarCategorias(); 
            foreach (DataRow fila in categorias.Rows)
            {
                mapCategorias.Add(fila[0].ToString(), fila[1].ToString());
            }
            return mapCategorias;
        }

        /*
        * Carga las posibles intenciones de uso para un producto en el 'comboBox' establecido para esto.
        */
        protected void cargarVendible()
        {
            inputVendible.Items.Clear();
            inputVendible.Items.Add(new ListItem("Consumo interno", null));
            inputVendible.Items.Add(new ListItem("Para venta", null));   
        }

        protected void cargarImpuesto()
        {
            inputImpuesto.Items.Clear();
            DataTable impuesto = controladoraDatosGenerales.consultarBooleanos();
            foreach (DataRow fila in impuesto.Rows)
            {
                inputImpuesto.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }

        
        }

        /*
        * Carga las posibles unidades para un producto en el 'comboBox' establecido para esto.
        */
        protected void cargarUnidades()
        {
            inputUnidades.Items.Clear();
            DataTable unidades = controladoraDatosGenerales.consultarUnidades();
            foreach (DataRow fila in unidades.Rows)
            {
                inputUnidades.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        /*
        * Carga las posibles estados para un producto en el 'comboBox' establecido para esto.
        */
        protected void cargarEstados()
        {
            inputEstado.Items.Clear();
            DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
            foreach (DataRow fila in estados.Rows)
            {
                inputEstado.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }

        /*
        * Guarda en memoria tanto el nombre y el id del estado para mostrar el nombre de la categoría en el grid  
        * en lugar de su id 
        */
        private Dictionary<string, string> traduccionEstado()
        {
            Dictionary<String, String> mapEstado = new Dictionary<String, String>(); ;
            DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
            foreach (DataRow fila in estados.Rows)
            {
                mapEstado.Add(fila[2].ToString(), fila[1].ToString());
            }
            return mapEstado;
        }

        //*****************FIN DE METODOS DE LLENADO DE DROPDOWNLIST Y GENERACION DE DICCIONARIOS PARA LLENADO DE GRID ********************


        /*
         * Método para obtener los datos del usuario.
         */
        protected Object[] obtenerDatosProductosGlobales()
        {
            Object[] datos = new Object[16];
            datos[0] = this.inputCodigo.Value;
            datos[1] = this.inputCodigoBarras.Value;
            datos[2] = this.inputNombre.Value;
            datos[3] = this.inputCostoColones.Value;
            datos[4] = this.inpuCategoria.SelectedValue;
            datos[5] = this.inputUnidades.SelectedValue;
            datos[6] = this.inputSaldo.Value;
            datos[7] = this.inputEstado.SelectedValue;
            datos[8] = this.inputCostoDolares.Value;
            datos[9] = this.inputImpuesto.SelectedValue;
            datos[10] = this.inputVendible.SelectedValue;
            datos[11] = this.inputPrecioColones.Value;
            datos[12] = this.inputPrecioDolares.Value;
            datos[13] = "0"; // Id que identifica se genera despues en la controladora de BD
            datos[14] = (this.Master as SiteMaster).Usuario.Codigo;
            datos[15] = DateTime.Now;
            return datos;
        }


        /*
        * Construye la tabla que se va a utilizar para mostrar la información de los productos globales.
        */
        protected DataTable tablaProductosGlobales()
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
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Categoria";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Estado";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /*
         * Método que se encarga de limpiar el grid
         */
        protected void vaciarGridProductosGlobales()
        {
            DataTable tablaLimpia = null;
            gridViewProductosGlobales.DataSource = tablaLimpia;
            gridViewProductosGlobales.DataBind();
        }

        /*
        * Llena la tabla/grid con los productos globales almacenadas en la base de datos.
        */
        protected void llenarGrid(String query)
        {
            DataTable tabla = tablaProductosGlobales();  // Secrea el esquema de la tabla
            int indiceNuevoProductoGlobal = -1;
            int id = 0; // Es la posicion en donde se guardan los iD'  
            try
            {
               
                Object[] datos = new Object[4];
                DataTable productosGlobales;
                if (query == null)
                {
                    productosGlobales = controladora.consultarProductosGlobales(); //Se trae el resultado de todos los productos
                }
                else
                {
                    productosGlobales = controladora.consultarProductosGlobales(query); //Se trae el resultado de todos los productos
                }
                
                Dictionary<String, String>  mapCategorias= traduccionCategorias(); // Para cargar y llenar el map con los codigos de las categorias
                Dictionary<String, String> mapEstado = traduccionEstado(); // Para cargar y llenar el map con los codigos de las categorias
                if (productosGlobales.Rows.Count > 0)
                {
                    idArray = new Object[productosGlobales.Rows.Count]; 
                    foreach (DataRow fila in productosGlobales.Rows)
                    {
                        idArray[id] = fila[0];   // Se guarda el id para hacer las consultas individuales
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[2].ToString();
                        String aux;
                        datos[2] = mapCategorias.TryGetValue(fila[3].ToString(), out aux)?aux:" ";
                        datos[3] = mapEstado.TryGetValue(fila[4].ToString(), out aux) ? aux : " ";  //Traducir el estado a representación humana
                        
                        
                        tabla.Rows.Add(datos);
                        if (productoConsultado != null && (fila[0].Equals(productoConsultado.Inv_Productos)))
                         {
                             indiceNuevoProductoGlobal = id; // Para marcar el producto consultado en el grid, puede ser util en el futuro
                         }
                        id++;
                    }
                    DataView sort = new DataView(tabla);
                    sort.Sort = "Código";
                    tabla = sort.ToTable();
                }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    tabla.Rows.Add(datos);
                }

                this.gridViewProductosGlobales.DataSource = tabla;  // Se llena el grid con los datos de la BD
                this.gridViewProductosGlobales.DataBind();
                consulta = tabla;
                 if (productoConsultado != null)
                 {
                     //GridViewRow filaSeleccionada = this.gridViewProductosGlobales.Rows[indiceNuevoProductoGlobal];
                 }
            }

            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * Presenta la información del producto consultado en los campos correspondientes.
         */
        protected void presentarDatos(){
            if (productoConsultado != null)
            {
                this.inputNombre.Value = productoConsultado.Nombre;
                this.inputCodigo.Value = productoConsultado.Codigo;
                this.inputCodigoBarras.Value = productoConsultado.CodigoDeBarras;
                this.inpuCategoria.SelectedValue = productoConsultado.Categoria;
                this.inputUnidades.SelectedValue = productoConsultado.Unidades.ToString();
                this.inputSaldo.Value = productoConsultado.Existencia.ToString();
                this.inputImpuesto.SelectedValue = productoConsultado.Impuesto.ToString();
                this.inputPrecioColones.Value = productoConsultado.PrecioColones.ToString();
                this.inputPrecioDolares.Value = productoConsultado.PrecioDolares.ToString();
                this.inputCostoColones.Value = productoConsultado.CostoColones.ToString();
                this.inputCostoDolares.Value = productoConsultado.CostoDolares.ToString();
                //this.inputEstado.SelectedValue = productoConsultado.Estado.ToString();
            }
    
        }

        //************************************* METODOS DE COMUNICACION CON LA CONTROLADORA******************************* 
        
        /*
         * Pide a la controladora consultar la información de un producto específico a partir de su código.
         */
        protected void consultar(String id)
        {
            seConsulto = true;
            try
            {
                productoConsultado = controladora.consultarProductoGlobal(id);
                modo = (int)Modo.Consultado;
                presentarDatos();
            }
            catch
            {
                productoConsultado = null;
                modo = (int)Modo.Consultado;
            }
            cambiarModo();
        }

        /*
        *  Pide a la controladora insertar un nuevo producto con la información ingresada por el usuario.
        */
        protected String insertar()
        {
            String identificadorProducto = "";
            Object[] nuevoProductoGlobal = obtenerDatosProductosGlobales(); // Se recolectan los datos
            String[] error = controladora.insertar(nuevoProductoGlobal); 
            identificadorProducto = Convert.ToString(error[3]);  // Contiene el código generado por la controladora de BD que identifica el producto en la BD
            mostrarMensaje(error[0], error[1], error[2]);
            if (error[0].Contains("success"))
            {
                llenarGrid(null);
            }
            else
            {
                identificadorProducto = "";
                modo = 1; // REVISAR ESTO
            }

            return identificadorProducto;
        }



        /*
        *  Pide a la controladora modificar del producto consultado con la nueva información ingresada por el usuario.
        */
        protected Boolean modificar()
        {
            Boolean res = true;
            Object[] productoGlobalModificado = obtenerDatosProductosGlobales();
            String id = productoConsultado.Inv_Productos;
            productoGlobalModificado[13] = id;
            String[] error = controladora.modificarDatos(productoConsultado, productoGlobalModificado);
            mostrarMensaje(error[0], error[1], error[2]);

            if (error[0].Contains("success"))// si fue exitoso
            {
                consultar(productoConsultado.Inv_Productos);
            }
            else
            {
                res = false;
            }
            return res;
        }

        //************************************* FIN DE METODOS DE COMUNICACION CON LA CONTROLADORA******************************* 


        // ************************************METODOS DE INTERFAZ RUTINARIOS *******************************
        
        /*
        * Remueve la información de los campos de la interfaz.
        */
        protected void limpiarCampos()
        {
            this.inputNombre.Value = "";
            this.inputCodigo.Value = "";
            this.inputCodigoBarras.Value = "";
            this.inputCostoColones.Value = "";
            this.inputCostoDolares.Value = "";
            this.inputPrecioColones.Value = "";
            this.inputPrecioDolares.Value = "";
            this.inputImpuesto.SelectedValue = null;
            this.inputSaldo.Value = "";
            this.inputUnidades.SelectedValue = null;
            this.inpuCategoria.SelectedValue = null;
            this.inputEstado.SelectedValue = null;
            this.inputVendible.SelectedValue= null;
        }

        /*
         * Habilita los campos de la interfaz para una inserción/modificación.
         */
        protected void habilitarCampos(bool resp)
        {
            this.inputNombre.Disabled = !resp;
            this.inputCodigo.Disabled = !resp;
            this.inputCodigoBarras.Disabled = !resp;
            this.inputCostoColones.Disabled = true;
            this.inputCostoDolares.Disabled = true;
            this.inputPrecioColones.Disabled = !resp;
            this.inputPrecioDolares.Disabled = !resp;
            this.inputImpuesto.Enabled = resp;
            this.inputSaldo.Disabled = true;
            this.inputUnidades.Enabled = resp;
            this.inpuCategoria.Enabled = resp;
            bool resp2 = false;
            if ((int)Modo.Modificacion == modo) {
                resp2 = productoConsultado.Existencia==0;
            }
            this.inputEstado.Enabled = resp2 && (permisos[2] == '1');
            this.inputVendible.Enabled = resp;
        }
        //*******************************************************************************

        // *******************EVENTOS***********************************


        /* 
         * Consulta de todos los productos y mostrar información parcial en el grid.
         */
        protected void botonConsultaProductos_ServerClick(object sender, EventArgs e)
        {
            llenarGrid(null);
            modo = (int)Modo.Consulta;
            cambiarModo();
            habilitarCampos(false);
            argumentoSorteo = "";
        }

        /* 
         * Consulta de un producto en particular y muestra la información completa en el formulario.
         */
        protected void gridViewProductosGlobales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewProductosGlobales.Rows[Convert.ToInt32(e.CommandArgument)];
                    String identificador = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewProductosGlobales.PageIndex * this.gridViewProductosGlobales.PageSize)]);
                    consultar(identificador);
                    modo = (int)Modo.Consultado;
                    Response.Redirect("FormProductosGlobales.aspx"); //Se hace un PostBack
                    break;
            }
            
        }

        /* 
         * Permite preparar la interfaz para la adición de un nuevo producto al inventario global de productos.
         */
        protected void botonAgregarProductos_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            cargarEstados();
            cargarUnidades();
            cargarVendible();
            cargarCategorias();
            cargarImpuesto();
            limpiarCampos();
            habilitarCampos(true);
        }

        /* 
         * Permite preparar la interfaz para la modificación de producto consultado al inventario global de productos.
         */
        protected void botonModificacionProductos_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            cambiarModo();
            habilitarCampos(true);  // Aqui va la logica en caso de no poder modificar un producto si este esta INACTIVO
        }

        //------Lógica de los botones Enviar y Cancelar

        /* 
         * Lógica del boton aceptar. Dependiendo del modo se tomarán distintas acciones.
         */
        protected void botonAceptarProductoGlobal_ServerClick(object sender, EventArgs e)
        {
            Boolean operacionCorrecta = true;
            String codigoInsertado = "";
            if (modo == (int)Modo.Insercion)  // Caso inserción
            {
                codigoInsertado = insertar();
                if (codigoInsertado != "") // Fue éxitoso
                {
                    operacionCorrecta = true;
                    consultar(codigoInsertado);
                    modo = (int)Modo.Consultado; //De modo inserción se pasa a modo consultado para mostrar/consultar los datos del nuevo  producto 
                    habilitarCampos(false);
                }
                else
                    operacionCorrecta = false;
            }
            else if (modo == (int)Modo.Modificacion) // Caso modificación
            {
                operacionCorrecta = modificar();
                modo = (int)Modo.Consultado;
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }

        /*
         * Permite mostrar la página seleccionada en el grid con los datos parciales de los productos globales
         */
        protected void gridViewProductosGlobales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewProductosGlobales.DataSource = consulta;
            gridViewProductosGlobales.DataBind();
            this.gridViewProductosGlobales.PageIndex = e.NewPageIndex;
            this.gridViewProductosGlobales.DataBind();
        }

        
        /*
         * Descarta los cambios hechos por el usuario y deja la interfaz en modo inicial para la siguiente acción
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
            habilitarCampos(false);
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        /*
         * Permite hacer una busqueda de productos con código u nombre similar al criterio ingresado por el usuario
         */
        protected void botonBuscar_ServerClick(object sender, EventArgs e)
        {
            llenarGrid(this.barraDeBusqueda.Value.ToString());
            modo = (int)Modo.Consulta;
            cambiarModo();
        }

        /*
         * Permite controlar la desactivactivación de un producto
         * Probablemente se eliminará en las siguientes iteraciones
         */
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        /*
         * Sorteo del grid
         */
        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == argumentoSorteo)
            {
                if (boolSorteo == true)
                    boolSorteo = false;
                else
                    boolSorteo = true;
            }
            else //New Column clicked so the default sort direction will be incorporated
                boolSorteo = false;

            argumentoSorteo = e.SortExpression; //Update the sort column
            BindGrid(argumentoSorteo, boolSorteo);
        }

        /*
         * Auxiliar para ordenar grid
         */
        public void BindGrid(string sortBy, bool inAsc)
        {
            agregarID();
            consulta.DefaultView.Sort = sortBy + " " + (inAsc ? "DESC" : "ASC"); //Ordena
            actualizarIDs();
            gridViewProductosGlobales.DataSource = consulta;
            gridViewProductosGlobales.DataBind();
        }

        public void agregarID() {
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "id";
            consulta.Columns.Add(columna);
            int i = 0;
            foreach (DataRow fila in consulta.Rows) {
                fila[4] = idArray[i];
                i++;
            }
        }

        public void actualizarIDs()
        {
            int i = 0;
            foreach (DataRow fila in consulta.Rows)
            {
                idArray[i] = fila[4];
                i++;
            }
            consulta.Columns.Remove("id");
        }

    }
}