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
    public partial class FormProductosGlobales : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        private static int modo = (int) Modo.Inicial;
        private static int idProducto = 0; //Sirve para estar en modo consulta
        private static int resultadosPorPagina;
        private static Object[] idArray;
        private EntidadProductoGlobal productoConsultado;
        private static ControladoraDatosGenerales controladoraDatosGenerales;
        private static ControladoraProductosGlobales controladora; 
        
        
        //ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack) 
            {
                modo = (int)Modo.Inicial;
                controladora = new ControladoraProductosGlobales();
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                cambiarModo();
            }
   
        }

        //*****************METODOS DE LLENADO DE DROPDOWNLIST*************************************
        protected void cargarCategorias()
        {
            inpuCategoria.Items.Clear();
            DataTable categorias = controladora.consultarCategorias(); // Hacer un llamado al metodo de Fernando
            foreach (DataRow fila in categorias.Rows)
            {
                inpuCategoria.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        protected void cargarVendible()
        {
            inputVendible.Items.Clear();
            inputVendible.Items.Add(new ListItem("Consumo interno", null));
            inputVendible.Items.Add(new ListItem("En venta", null));
            
        }

        protected void cargarUnidades()
        {
            inputUnidades.Items.Clear();
            DataTable unidades = controladoraDatosGenerales.consultarUnidades();
            foreach (DataRow fila in unidades.Rows)
            {
                inputUnidades.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
            }
        }

        protected void cargarEstados()
        {
            inputEstado.Items.Clear();
            DataTable estados = controladoraDatosGenerales.consultarEstados();
            foreach (DataRow fila in estados.Rows)
            {
                inputEstado.Items.Add(new ListItem(fila[1].ToString(), fila[2].ToString()));
            }
        }
        //***********************************************************************************


        protected Object[] obtenerDatosProductosGlobales()
        {
            Object[] datos = new Object[9];
            datos[0] = this.inputNombre.Value;
            datos[1] = this.inpuCategoria.SelectedValue;
            datos[2] = this.inputUnidades.SelectedValue;
            datos[3] = this.inputCodigo.Value;
            datos[4] = this.inputCodigoBarras.Value;
            datos[5] = this.inputEstado.SelectedValue;
            datos[6] = this.inputCostoColones.Value;
            datos[7] = this.inputCostoDolares.Value;
            datos[8] = 0; // Codigo
            return datos;
        }

        

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

        protected void llenarGrid()
        {
            DataTable tabla = tablaProductosGlobales();
            int indiceNuevoProductoGlobal = -1;
            int id = 0; // Es la posicion en donde se guardan los iD'  
            try
            {
                Object[] datos = new Object[4];
                DataTable productosGlobales = new DataTable(); //quitar una vez que ya está la controladora
                productosGlobales = controladora.consultarProductosGlobales();

                if (productosGlobales.Rows.Count > 0)
                {
                    idArray = new Object[productosGlobales.Rows.Count];
                    foreach (DataRow fila in productosGlobales.Rows)
                    {
                        idArray[id] = fila[0];   // Se guarda el id para hacer las consultas individuales
                        datos[0] = fila[1].ToString();
                        datos[1] = fila[2].ToString();
                        datos[2] = fila[3].ToString();
                        datos[3] = fila[4].ToString();
                        tabla.Rows.Add(datos);
                        if (productoConsultado != null && (fila[0].Equals(productoConsultado.Inv_Productos)))
                         {
                             indiceNuevoProductoGlobal = id; // Para marcar el producto consultado en el grid
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

                this.gridViewProductosGlobales.DataSource = tabla;
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


        protected String insertar()
        {
            String codigo = "";
            Object[] nuevoProductoGlobal = obtenerDatosProductosGlobales();

            String[] error = controladora.insertar(nuevoProductoGlobal); 

            codigo = Convert.ToString(error[3]);
            mostrarMensaje(error[0], error[1], error[2]);
            if (error[0].Contains("success"))
            {
                llenarGrid();
            }
            else
            {
                codigo = "";
                modo = 1;
            }

            return codigo;
        }

        protected Boolean modificar()
        {
            Boolean res = true;

            Object[] productoGlobalModificado = obtenerDatosProductosGlobales();
            String id = productoConsultado.Inv_Productos;
            productoGlobalModificado[9] = id;
            String[] error = controladora.modificarDatos(productoConsultado, productoGlobalModificado);
            mostrarMensaje(error[0], error[1], error[2]);

            if (error[0].Contains("success"))// si fue exitoso
            {
                llenarGrid();
                productoConsultado = controladora.consultar(productoConsultado.Inv_Productos);
                modo = (int)Modo.Consultado;
            }
            else
            {
                res = false;
                modo = (int)Modo.Consulta;
            }
            return res;
        }


        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Attributes.Remove("hidden");
        }


        protected void cambiarModo()
        {
            switch (modo)
            {///Probar si aun se pueden mostrar los campos con el JS********************
                case (int)Modo.Inicial:
                    limpiarCampos();
                    this.botonAgregarProductos.Disabled = false;
                    this.botonModificacionProductos.Disabled = true;
                    //this.gridViewProductosGlobales.Visible = false;///********************
                    this.botonAceptarProducto.Visible = false;///******************
                    this.botonCancelarProducto.Visible = false;///******************                                       ///
                    //cargarCategorias();
                    cargarEstados();
                    cargarUnidades();
                    cargarVendible();
                    habilitarCampos(false);
                    break;
                case (int)Modo.Insercion: //insertar
                    habilitarCampos(true);
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    this.gridViewProductosGlobales.Visible = false;///********************
                    break;
                case (int)Modo.Modificacion: //modificar
                    habilitarCampos(true);
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = true;
                    this.gridViewProductosGlobales.Visible = false;///********************
                    break;
                case (int)Modo.Consulta://consultar
                    limpiarCampos();
                    habilitarCampos(false);
                    this.botonAceptarProducto.Visible = false;///******************
                    this.botonCancelarProducto.Visible = false;///******************
                    this.botonModificacionProductos.Disabled = true;//**********************
                    this.gridViewProductosGlobales.Visible = true;///********************
                    break;
                case (int)Modo.Consultado://consultada una actividad
                    habilitarCampos(false);
                    this.botonAgregarProductos.Disabled = true;
                    this.botonModificacionProductos.Disabled = false;
                    this.botonAceptarProducto.Visible = false;///******************
                    this.botonCancelarProducto.Visible = false;///****************** 
                    this.gridViewProductosGlobales.Visible = false;///********************///
                    break;
                default:
                    break;
            }
        }


        // ************************************METODOS DE INTERFAZ RUTINARIOS
        protected void limpiarCampos()
        {
            this.inputNombre.Value = " ";
            this.inputCodigo.Value = " ";
            this.inputCodigoBarras.Value = " ";
            this.inputCostoColones.Value = " ";
            this.inputCostoDolares.Value = " ";
            this.inputEstado.SelectedValue = null;
            this.inputUnidades.SelectedValue = null;
            this.inpuCategoria.SelectedValue = null;
        }

        protected void habilitarCampos(bool resp)
        {
            this.inputNombre.Disabled = !resp;
            this.inputCodigo.Disabled = !resp;
            this.inputCodigoBarras.Disabled = !resp;
            this.inputCostoColones.Disabled = !resp;
            this.inputCostoDolares.Disabled = !resp;
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

        // *******************EVENTOS ***********************************
        protected void botonCancelarModalCancelar_ServerClick(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        protected void botonAgregarProductos_ServerClick(object sender, EventArgs e)
        {
            modo = (int)Modo.Insercion;
            cambiarModo();
            limpiarCampos();
            //cargarEstados();
        }

        protected void botonModificacionProductos_ServerClick(object sender, EventArgs e)
        {

        }

        protected void botonConsultaProductos_ServerClick(object sender, EventArgs e)
        {
            llenarGrid();
            modo = (int)Modo.Consulta;
            cambiarModo();
            
        }

        protected void gridViewProductosGlobales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewProductosGlobales.Rows[Convert.ToInt32(e.CommandArgument)];
                    String codigo = Convert.ToString(idArray[Convert.ToInt32(e.CommandArgument) + (this.gridViewProductosGlobales.PageIndex * resultadosPorPagina)]);
                    //consultarActividad(codigo);
                    modo = (int)Modo.Consultado;  
                    Response.Redirect("FormProductosGlobales.aspx"); //Se hace un PostBack
                    break;
            }
        }

        protected void gridViewProductosGlobales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gridViewProductosGlobales.PageIndex = e.NewPageIndex;
            this.gridViewProductosGlobales.DataBind();
        }



    }
}