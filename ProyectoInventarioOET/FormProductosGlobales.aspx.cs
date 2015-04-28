using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales;
using ProyectoInventarioOET.App_Code;


namespace ProyectoInventarioOET
{
    /*
     * ???
     */
    public partial class FormProductosGlobales : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static int modo = (int) Modo.Inicial;                           //???
        private static Boolean seConsulto = false;                              //???
        private static int resultadosPorPagina;                                 //??? Creo que hay que modificarlo por 16
        private static Object[] idArray;                                        //???
        private static EntidadProductoGlobal productoConsultado;                //???
        private static ControladoraDatosGenerales controladoraDatosGenerales;   //???
        private static ControladoraProductosGlobales controladora;              //???

        /*
         * ???
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                controladora = new ControladoraProductosGlobales();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
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
                        presentarDatos();
                        seConsulto = false;
                    }
                }
            }
            cambiarModo(); // Al cargar la página sin importar se cambia de modo
        }

        /*
         * ???
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Attributes.Remove("hidden");
        }

        /*
         * Manejo de la lógica de la interfaz.
         * ???
         */
        protected void cambiarModo()
        {
            switch (modo)
            {///Probar si aun se pueden mostrar los campos con el JS********************
                case (int)Modo.Inicial:
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = false;
                    this.bloqueBotones.Visible = false;
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    break;
                case (int)Modo.Insercion: //insertar
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = true;
                    this.bloqueBotones.Visible = true;
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    break;
                case (int)Modo.Modificacion: //modificar
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = true;
                    this.bloqueBotones.Visible = true;
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    break;
                case (int)Modo.Consulta:
                    this.bloqueGrid.Visible = true;
                    this.gridViewProductosGlobales.Visible = true;
                    this.bloqueFormulario.Visible = false;
                    this.bloqueBotones.Visible = false;
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = true;
                    this.botonConsultaProductos.Disabled = false;
                    break;
                case (int)Modo.Consultado:
                    this.bloqueGrid.Visible = false;
                    this.gridViewProductosGlobales.Visible = false;
                    this.bloqueFormulario.Visible = true;
                    this.bloqueBotones.Visible = true;
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = false;
                    this.botonConsultaProductos.Disabled = false;
                    break;
                default:
                    break;
            }
        }

        //*****************METODOS DE LLENADO DE DROPDOWNLIST Y GENERACION DE DICCIONARIOS PARA LLENADO DE GRID ********************
        /*
         * ???
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
         * ???
         */
        protected Dictionary<String, String> traduccionCategorias()
        {
            Dictionary<String, String> mapCategorias = new Dictionary<String, String>(); ;
            DataTable categorias = controladora.consultarCategorias(); // Hacer un llamado al metodo de Fernando
            foreach (DataRow fila in categorias.Rows)
            {
                mapCategorias.Add(fila[0].ToString(), fila[1].ToString());
            }
            return mapCategorias;
        }

        /*
         * ???
         */
        protected void cargarVendible()
        {
            inputVendible.Items.Clear();
            inputVendible.Items.Add(new ListItem("Consumo interno", null));
            inputVendible.Items.Add(new ListItem("Para venta", null));   
        }

        /*
         * ???
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
         * ???
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
         * ???
         */
        private Dictionary<string, string> traduccionEstado()
        {
            Dictionary<String, String> mapEstado = new Dictionary<String, String>(); ;
            DataTable estados = controladoraDatosGenerales.consultarEstadosActividad();
            foreach (DataRow fila in estados.Rows)
            {
                mapEstado.Add(fila[0].ToString(), fila[1].ToString());
            }
            return mapEstado;
        }

        //***********************************************************************************

        /*
         * Método para obtener los datos del usuario.
         */
        protected Object[] obtenerDatosProductosGlobales()
        {
            Object[] datos = new Object[14];
            datos[0] = this.inputCodigo.Value;
            datos[1] = this.inputCodigoBarras.Value;
            datos[2] = this.inputNombre.Value;
            datos[3] = this.inputCostoColones.Value;
            datos[4] = this.inpuCategoria.SelectedValue;
            datos[5] = this.inputUnidades.SelectedValue;
            datos[6] = this.inputSaldo.Value;
            datos[7] = this.inputEstado.SelectedValue;
            datos[8] = this.inputCostoDolares.Value;
            datos[9] = this.inputImpuesto.Value;
            datos[10] = this.inputVendible.SelectedValue;
            datos[11] = this.inputPrecioColones.Value;
            datos[12] = this.inputPrecioDolares.Value;
            datos[13] = "0"; // Id que identifica se genera despues en la controladora de BD
            return datos;
        }

        /*
         * ???
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
         * ???
         */
        protected void vaciarGridProductosGlobales()
        {
            DataTable tablaLimpia = null;
            gridViewProductosGlobales.DataSource = tablaLimpia;
            gridViewProductosGlobales.DataBind();
        }

        /*
         * ???
         */
        protected void llenarGrid(String query)
        {
            DataTable tabla = tablaProductosGlobales();  // Secrea el esquema de la tabla
            int indiceNuevoProductoGlobal = -1;
            int id = 0; // Es la posicion en donde se guardan los iD'  
            try
            {
                DataTable productosGlobales;
                Object[] datos = new Object[4];

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
                 if (productoConsultado != null)
                 {
                     GridViewRow filaSeleccionada = this.gridViewProductosGlobales.Rows[indiceNuevoProductoGlobal];
                 }
            }

            catch (Exception e)
            {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /*
         * ???
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
                this.inputImpuesto.Value = productoConsultado.Impuesto.ToString();
                this.inputPrecioColones.Value = productoConsultado.PrecioColones.ToString();
                this.inputPrecioDolares.Value = productoConsultado.PrecioDolares.ToString();
                this.inputCostoColones.Value = productoConsultado.CostoColones.ToString();
                this.inputCostoDolares.Value = productoConsultado.CostoDolares.ToString();
            }
    
        }

        //************************************* METODOS DE COMUNICACION CON LA CONTROLADORA******************************* 
        /*
         * ???
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
         * ???
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
         * ???
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


        // ************************************METODOS DE INTERFAZ RUTINARIOS
        /*
         * ???
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
            this.inputImpuesto.Value = "";
            this.inputSaldo.Value = "";
            this.inputUnidades.SelectedValue = null;
            this.inpuCategoria.SelectedValue = null;
            this.inputEstado.SelectedValue = null;
            this.inputVendible.SelectedValue= null;
        }

        /*
         * ???
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
            this.inputImpuesto.Disabled = !resp;
            this.inputSaldo.Disabled = !resp;
            this.inputUnidades.Enabled = resp;
            this.inpuCategoria.Enabled = resp;
            this.inputEstado.Enabled = resp;
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
        }

        /* 
         * Consulta de UN PRODUCTO EN PARTICULAR y muestra la información completa en el formulario.
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
         * Adición de un nuevo producto al inventario global de productos.
         */
        protected void botonAgregarProductos_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            cargarEstados();
            cargarUnidades();
            cargarVendible();
            cargarCategorias();
            limpiarCampos();
            habilitarCampos(true);
        }

        /*
         * ???
         */
        protected void botonModificacionProductos_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Modificacion;
            cambiarModo();
            habilitarCampos(true);  // Aqui va la logica en caso de no poder modificar un producto si este esta INACTIVO
        }

        //------Lógica de los botones Enviar y Cancelar

        /* 
         * Lógica del boton aceptar. Dependiendo del modo se tomarán distintas acciones acciones.
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
            }
            if (operacionCorrecta)
            {
                cambiarModo();
            }
        }

        /*
         * ???
         */
        protected void gridViewProductosGlobales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            llenarGrid(null);
            this.gridViewProductosGlobales.PageIndex = e.NewPageIndex;
            this.gridViewProductosGlobales.DataBind();
        }

        /*
         * ???
         */
        protected void botonAceptarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
            habilitarCampos(false);
            modo = (int)Modo.Inicial;
            cambiarModo();
        }

        /*
         * ???
         */
        protected void botonBuscar_ServerClick(object sender, EventArgs e)
        {
            llenarGrid(this.barraDeBusqueda.Value.ToString());
            modo = (int)Modo.Consulta;
            cambiarModo();
        }

        /*
         * ???
         */
        protected void botonAceptarModalDesactivar_ServerClick(object sender, EventArgs e)
        {
            // Hacer algo
        }
    }
}