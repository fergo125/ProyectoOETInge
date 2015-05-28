﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_Ventas;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.App_Code;

namespace ProyectoInventarioOET
{
    /*
     * Clase interfaz que se encarga de todo lo relacionado con ventas, desde un punto de vista de facturas.
     * Permite consultar facturas, crear facturas, y anular fcturas existentes. Dependiendo de los permisos que tenga el perfil del usuario conectado.
     */
    public partial class FormVentas : System.Web.UI.Page
    {
        enum Modo { Inicial, Consulta, Insercion, Modificacion, Consultado };
        //Atributos
        private static Modo modo = Modo.Inicial;                               //Indica en qué modo se encuentra la interfaz en un momento cualquiera, de éste depende cuáles elementos son visibles
        private String permisos = "111111";                             //Permisos utilizados para el control de seguridad //TODO: poner en 000000, está en 111111 sólo para pruebas
        private String codigoPerfilUsuario = "1";                       //Indica el perfil del usuario, usado para acciones de seguridad para las cuales la string de permisos no basta //TODO: poner en ""
        private DataTable facturasConsultadas;                          //Usada para llenar el grid y para mostrar los detalles de cada factura específica
        private static ControladoraVentas controladoraVentas;                  //Para accesar las tablas del módulo y realizar las operaciones de consulta, inserción, modificación y anulación
        private static ControladoraDatosGenerales controladoraDatosGenerales;  //Para accesar datos generales de la base de datos
        private static ControladoraBodegas controladoraBodegas;  //Para accesar datos generales de la base de datos
        private static ControladoraSeguridad controladoraSeguridad;     //???
        
        //Importante:
        //Para el codigoPerfilUsuario (que se usa un poco hard-coded), los números son:
        //1. Administrador global
        //2. Administrador local
        //3. Supervisor
        //4. Vendedor

        /*
         * Función invocada cada vez que se carga la página, encargada de invocar a las funciones que actualizan los elementos visuales de la página.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si es la primera vez que se carga la página
            if (!IsPostBack)
            {
                //Elementos visuales
                ScriptManager.RegisterStartupScript(this, GetType(), "setCurrentTab", "setCurrentTab()", true); //para que quede marcada la página seleccionada en el sitemaster
                //Controladoras
                controladoraDatosGenerales = ControladoraDatosGenerales.Instanciar;
                controladoraSeguridad = new ControladoraSeguridad();
                controladoraVentas = new ControladoraVentas();
                controladoraBodegas = new ControladoraBodegas();
                //Seguridad
                //permisos = (this.Master as SiteMaster).obtenerPermisosUsuarioLogueado("Facturacion"); //TODO: descomentar esto, está comentado sólo para pruebas
                if (permisos == "000000")
                    Response.Redirect("~/ErrorPages/404.html");
                //perfilUsuario = (this.Master as SiteMaster).Usuario.Perfil;
                mostrarElementosSegunPermisos();
                
            }
            //Si la página ya estaba cargada pero está siendo cargada de nuevo (porque se está realizando alguna acción que la refrezca/actualiza)

            cambiarModo();
            //código para probar algo
            DataTable testTable = new DataTable();
            DataRow testRow;
            DataColumn testColumn;

            testColumn = new DataColumn();
            testColumn.DataType = Type.GetType("System.String");
            testColumn.ColumnName = "Nombre";
            testTable.Columns.Add(testColumn);
            testColumn = new DataColumn();
            testColumn.DataType = Type.GetType("System.String");
            testColumn.ColumnName = "Código interno";
            testTable.Columns.Add(testColumn);
            testColumn = new DataColumn();
            testColumn.DataType = Type.GetType("System.Int32");
            testColumn.ColumnName = "Precio unitario";
            testTable.Columns.Add(testColumn);
            testColumn = new DataColumn();
            testColumn.DataType = Type.GetType("System.String");
            testColumn.ColumnName = "Impuesto";
            testTable.Columns.Add(testColumn);
            testColumn = new DataColumn();
            testColumn.DataType = Type.GetType("System.Int32");
            testColumn.ColumnName = "Descuento (%)";
            testTable.Columns.Add(testColumn);

            testRow = testTable.NewRow();
            testRow["Nombre"] = "Nombre de prueba";
            testRow["Código interno"] = "CRO001";
            testRow["Precio unitario"] = "500";
            testRow["Impuesto"] = "Sí";
            testRow["Descuento (%)"] = "0";
            testTable.Rows.Add(testRow);
            testRow = testTable.NewRow();
            testRow["Nombre"] = "Nombre de prueba larguísimo de esos que ponen las unidades y la vara";
            testRow["Código interno"] = "CRO002";
            testRow["Precio unitario"] = "50000";
            testRow["Impuesto"] = "Sí";
            testRow["Descuento (%)"] = "0";
            testTable.Rows.Add(testRow);

            gridViewCrearFacturaProductos.DataSource = testTable;
            gridViewCrearFacturaProductos.DataBind();

            //DataControlField[] backupColumn = new DataControlField[100];
            //gridViewCrearFacturaProductos.Columns.CopyTo(backupColumn, 0);
            //gridViewCrearFacturaProductos.Columns.Insert(0, backupColumn[0]);
            //DataControlField backup = gridViewCrearFacturaProductos.Columns[1];
            //gridViewCrearFacturaProductos.Columns.RemoveAt(1);
            //gridViewCrearFacturaProductos.Columns.Add(backup);
            //gridViewCrearFacturaProductos.Columns.Add(backup);
            //gridViewCrearFacturaProductos.Columns["Seleccionar"].DisplayIndex = 0;
        }

        /*
         * Dependiendo del perfil, algunas acciones están permitidas y otras no, ésto se controla escondiendo y mostrando los botones y otros elementos
         * que se usan para realizar esas acciones. Tratar de contener aquí todo lo relacionado con seguridad.
         */
        protected void mostrarElementosSegunPermisos()
        {
            //Botones principales
            botonConsultar.Visible = (permisos[5] == '1');
            botonCrear.Visible = (permisos[4] == '1');
            botonModificar.Visible = (permisos[3] == '1');
            //Dropdownlists
            dropDownListConsultaEstacion.Enabled = (Convert.ToInt32(codigoPerfilUsuario) <= 1);    //Sólo si es administrador global, puede escoger una estación
            dropDownListConsultaBodega.Enabled = (Convert.ToInt32(codigoPerfilUsuario) <= 2);      //Sólo si es administrador global, o administrador local, puede escoger una bodega
            dropDownListConsultaVendedor.Enabled = (Convert.ToInt32(codigoPerfilUsuario) <= 3);    //Sólo si es administrador global, o administrador local, o supervisor, puede escoger un vendedor
            //dropdownEstado.Enabled = (permisos[2] == '1');
        }

        /*
         * Función invocada cada vez que se cambia de modo en la interfaz, se encarga de mostrar u ocultar, habilitar o deshabilitar,
         * elementos visuales, dependiendo del modo al que se está entrando.
         */
        protected void cambiarModo()
        {
            //Código común (que debe ejecutarse en la mayoría de modos, en la minoría luego es arreglado en el switch)
            //Reduce un poco la eficiencia, pero simplifica el código bastante
            PanelConsultarFacturas.Visible = false;
            PanelConsultarFacturaEspecifica.Visible = false;
            PanelGridConsultas.Visible = false;
            PanelCrearFactura.Visible = false;
            botonCambioSesion.Visible = false;      //Estos dos botones sólo deben ser visibles
            botonAjusteEntrada.Visible = false;     //durante la creación de facturas

            //Código específico para cada modo
            switch (modo)
            {
                case Modo.Inicial:
                    tituloAccionFacturas.InnerText = "Seleccione una opción";
                    break;
                case Modo.Consulta:
                    tituloAccionFacturas.InnerText = "Seleccione datos para consultar";
                    PanelConsultarFacturas.Visible = true;
                    break;
                case Modo.Insercion:
                    tituloAccionFacturas.InnerText = "Ingrese los datos de la nueva factura";
                    PanelCrearFactura.Visible = true;
                    botonCambioSesion.Visible = true;  //Estos dos botones sólo deben ser visibles
                    botonAjusteEntrada.Visible = true; //durante la creación de facturas

                    break;
                case Modo.Modificacion:
                    tituloAccionFacturas.InnerText = "Ingrese los nuevos datos para la factura";
                    break;
                case Modo.Consultado:
                    tituloAccionFacturas.InnerText = "Detalles de la factura";
                    PanelConsultarFacturaEspecifica.Visible = true;
                    PanelGridConsultas.Visible = true;
                    break;
                default:  //Algo salió mal
                    mostrarMensaje("warning", "Alerta: ", "Error de interfaz, el 'modo' de la interfaz no se ha reconocido: " + modo);
                    break;
            }
        }

        /*
         * Invocada después de realizar operaciones de base de datos, para que muestre el mensaje de resultado (éxito o error).
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {
            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        /*
         * Invocada antes de iniciar operaciones que requieran que los campos no contengan información de ninguna operación previa.
         */
        protected void limpiarCampos()
        {
            //Campos de consulta
            dropDownListConsultaEstacion.SelectedValue = null;
            dropDownListConsultaBodega.SelectedValue = null;
            dropDownListConsultaVendedor.SelectedValue = null;
            //Campos de consulta individual

            //Campos de creación

        }

        /*
         * Invocada al entrar en modo de consulta normal, se debe cargar la lista de opciones en cada dropdownlist pero por separado (una vez que
         * se escoge una opción en una, se pueden cargar las opciones en la siguiente).
         */
        protected void cargarDropdownListsConsulta()
        {
            //Limpiar la lista de opciones para que no se acumulen
            dropDownListConsultaEstacion.Items.Clear();
            dropDownListConsultaBodega.Items.Clear();
            dropDownListConsultaVendedor.Items.Clear();

            //Dependiendo del perfil del usuario, puede que en la instancia de usuarioLogueado ya estén guardados los datos por default
            switch (Convert.ToInt32(codigoPerfilUsuario))
            {
                case 4: //Vendedor
                    //dropDownListConsultaVendedor.Items.Add(new ListItem());
                    //dropDownListConsultaVendedor.SelectedItem = 
                    goto case 3; //por alguna razón C# no permite fall through
                case 3: //Supervisor
                    //dropDownListConsultaBodega.Items.Add(new ListItem());
                    //dropDownListConsultaBodega.SelectedItem =
                    goto case 2;  //por alguna razón C# no permite fall through
                case 2: //Administrador local
                    //dropDownListConsultaEstacion.Items.Add(new ListItem());
                    //dropDownListConsultaEstacion.SelectedItem = 
                    break;
                default:
                    //Administrador global y cualquier otro, este switch es extendible a más perfiles
                    break;
            }
            //TODO: básicamente se obtienen los datos del perfil según cual sea para colocarlos en los dropdownlists de una vez y ahorrarse
            //viajes a la base de datos trayendo opciones.
            //TODO: también, falta agregar que el usuarioLogueado, su clase entidad, guarde la llave de la bodega a la que está asignado,
            //esto probablemente requiera agregar el campo a la base de datos.

            //Si una dropdownlist no queda con un valor seleccionado (porque el perfil es elevado), entonces sí se cargan opciones
            if(dropDownListConsultaEstacion.SelectedItem == null)
            {
                dropDownListConsultaEstacion.Items.Add(new ListItem("Todas")); //Agregar la opción de "Todas"/"Todos" al principio de la lista
                //TODO: descomentar esto, está comentado sólo para pruebas
                //DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();
                //foreach (DataRow fila in estaciones.Rows) //Agregar las opciones para cada caso
                //    dropDownListConsultaEstacion.Items.Add(new ListItem(fila[2].ToString(), fila[0].ToString())); //Nombre, llave
            }
            if (dropDownListConsultaBodega.SelectedItem == null)
            {
                dropDownListConsultaBodega.Items.Add(new ListItem("Todas")); //Agregar la opción de "Todas"/"Todos" al principio de la lista
                //DataTable bodegas = 
                //foreach (DataRow fila in bodegas.Rows) //Agregar las opciones para cada caso
                //    dropDownListConsultaEstacion.Items.Add(new ListItem(); //Nombre, llave
            }
            if (dropDownListConsultaVendedor.SelectedItem == null)
            {
                dropDownListConsultaVendedor.Items.Add(new ListItem("Todos")); //Agregar la opción de "Todas"/"Todos" al principio de la lista
                //DataTable vendedores = 
                //foreach (DataRow fila in vendedores.Rows) //Agregar las opciones para cada caso
                //    dropDownListConsultaEstacion.Items.Add(new ListItem(); //Nombre, llave
            }
            //TODO: agregar bien estas consultas para que cargue las listas de opciones
        }

        /*
         * Invocada al consultar facturas, dependiendo de los parámetros de consulta se muestran facturas asociadas a:
         * -Una estación o todas
         * -Una bodega de esa estación, o todas las bodegas de esa estación
         * -Un vendedor de esa bodega, o todos los vendedores de esa bodega
         */
        protected void llenarGrid()
        {
            //Importante: estos dropdownlists pueden contener una entidad específica o la palabra "Todas"/"Todos", en el segundo caso se envía "null", la controladora debe entenderlo
            String codigoEstacion = (dropDownListConsultaEstacion.SelectedValue != "Todas" ? dropDownListConsultaEstacion.SelectedValue : null);
            String codigoBodega = (dropDownListConsultaBodega.SelectedValue != "Todas" ? dropDownListConsultaBodega.SelectedValue : null);
            String codigoVendedor = (dropDownListConsultaVendedor.SelectedValue != "Todos" ? dropDownListConsultaVendedor.SelectedValue : null);
            //TODO: revisar que de los dropdownlists se obtengan las llaves, no los nombres, algo como dropDownList.SelectedItem[1] creo
            
            //Consultar a la controladora (implementar funciones en las capas inferiores)
            //facturasConsultadas = controladoraVentas.consultarFacturas(codigoEstacion, codigoBodega, codigoVendedor);
        }

        /*
         * Invocada al escoger una factura en el grid, se muestran todos los detalles de la misma en campos colocados arriba del grid.
         */
        protected void cargarDatosFactura(int indiceFilaSeleccionada)
        {
            //facturasConsultadas.Rows[indiceFilaSeleccionada]; //usar el grid así
        }


        protected Object[] obtenerDatos()
        {
            Object[] datos = new Object[8];
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;

            datos[0] = dropDownListCrearFacturaEstacion.SelectedValue;
            datos[1] = "02";
            datos[2] = "";
            datos[3] = usuarioActual.Codigo;
            datos[4] = dropDownListCrearFacturaCliente.SelectedValue;
            datos[5] = textBoxCrearFacturaTipoCambio.Text;
            datos[6] = dropDownListCrearFacturaMetodoPago.SelectedValue;
            datos[7] = null;
            return datos;
        
        }

        protected void cargarEstaciones() 
        {
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            DataTable estaciones = controladoraDatosGenerales.consultarEstaciones();

            if (estaciones.Rows.Count > 0)
            {
                this.dropDownListCrearFacturaEstacion.Items.Clear();
                foreach (DataRow fila in estaciones.Rows)
                {
                    if ((usuarioActual.Perfil.Equals("Administrador global"))||(usuarioActual.IdEstacion.Equals(fila[0])))
                    {
                        this.dropDownListCrearFacturaEstacion.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                    }
                }
            }
            if (usuarioActual.Perfil.Equals("Administrador global"))
            {
                dropDownListCrearFacturaEstacion.Enabled = true;
            }
            else
            {
                dropDownListCrearFacturaEstacion.Enabled = false;
            }
        }

        protected void cargarBodegas()
        {
            EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
            DataTable bodegas = controladoraBodegas.consultarBodegasDeEstacion(dropDownListCrearFacturaEstacion.SelectedValue);
            int i = 0;
            if (bodegas.Rows.Count > 0)
            {
                foreach (DataRow fila in bodegas.Rows)
                {
                    if ((usuarioActual.Perfil.Equals("Administrador global")) || (usuarioActual.Perfil.Equals("Administrador local")) || fila[1].ToString().Equals((this.Master as SiteMaster).NombreBodegaSesion))
                    {
                        this.dropDownListCrearFacturaBodega.Items.Add(new ListItem(fila[1].ToString(), fila[0].ToString()));
                    }
                }
            }
        }





        /*
         *****************************************************************************************************************************************************************************
         * Funciones invocadas por eventos en la interfaz (botones, grids, dropdownlists, etc.)
         *****************************************************************************************************************************************************************************
         */

        /*
         * Invocada cuando se da click al botón de "Consultar Facturas", muestra el panel de escoger datos para consultar, NO muestra el grid.
         * Para mostrar el panel, pone la interfaz en modo de consulta y luego se invoca la función de cambiarModo.
         */
        protected void clickBotonConsultarFacturas(object sender, EventArgs e)
        {
            modo = Modo.Consulta;
            cargarDropdownListsConsulta();
            cambiarModo();
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonEjecutarConsulta(object sender, EventArgs e)
        {
            llenarGrid();
            PanelGridConsultas.Visible = true;
            tituloAccionFacturas.InnerText = "Seleccione una factura para ver su información detallada";
            //Aquí NO se debe inovcar cambiarModo, ya que el modo no cambia
        }

        /*
         * Invocada cuando se da click al botón de "Consultar", muestra el grid con los resultados de la consulta.
         * La interfz se mantiene en modo de consulta.
         */
        protected void clickBotonCrearFactura(object sender, EventArgs e)
        {
            cargarEstaciones();
            cargarBodegas();
            modo = Modo.Insercion;
            cambiarModo();
        }

        /*
         * Invocada cuando se da click al botón de "Agregar Producto" a la factura, se revisa que exista primero
         * (el usuario puede escribir lo que quiera, es un textbox), si existe se agrega al grid para luego editar
         * su cantidad y poder aplicarle descuentos (o quitarlo de la factura).
         */
        protected void clickBotonAgregarProductoFactura(object sender, EventArgs e)
        {
            String productoEscogido = textBoxAutocompleteCrearFacturaBusquedaProducto.Text;
            productoEscogido = controladoraVentas.verificarExistenciaProductoLocal(dropDownListCrearFacturaBodega.SelectedItem.Value, productoEscogido); //TODO: obtener llave de la bodega, no nombre
        }

        /*
         * Invocada cuando se escoge una factura del grid de consultas para desplegar su información específica
         * (en el panel de consulta específica, el cual ahora reemplazará visualmente al panel de escoger datos para consultar).
         */
        protected void gridViewFacturas_FilaSeleccionada(object sender, EventArgs e)
        {
            modo = Modo.Consultado;
            cargarDatosFactura(gridViewFacturas.SelectedIndex);
            cambiarModo();
        }

        /*
         * ???
         */
        protected void botonAceptarCambioUsuario_ServerClick(object sender, EventArgs e)
        {
            // Consulta al usuario
            /*EntidadUsuario usuario = controladoraSeguridad.consultarUsuario(inputUsername.Value, inputPassword.Value);

            if (usuario != null)
            {
                // Si me retorna un usuario valido

                // Hacer el usuario logueado visible a todas los modulos
                (this.Master as SiteMaster).Usuario = usuario;
                // Redirigir a pagina principal
            }
            else
            {
                // Si no me retorna un usuario valido, advertir
                //mostrarMensaje();
            }* */

            String [] resultado = controladoraVentas.insertarFactura(obtenerDatos());

        }

        protected void dropDownListCrearFacturaEstacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarBodegas();
        }
    }
}