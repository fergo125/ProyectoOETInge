<%@ Page Title="Ventas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormVentas.aspx.cs" Inherits="ProyectoInventarioOET.FormVentas" %>
<asp:Content ID="ContentVentas" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div ID="mensajeAlerta" class="" runat="server" Visible="false" style="margin-left:70%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text=""></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 ID="TituloVentas" runat="server">Facturación</h2>
        <hr />
    </div>

    <!-- Botones principales -->
    <button runat="server" onserverclick="clickBotonConsultarFacturas" ID="botonConsultar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Consultar Facturas</button>
    <button runat="server" onserverclick="clickBotonCrearFactura" ID="botonCrear" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Crear Factura</button>
    <button runat="server" onserverclick="Page_Load" ID="botonModificar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Modificar Factura</button>
    <a ID="botonAjusteEntrada" accesskey="A" href="#modalAjusteRapido" class="btn btn-info-fozkr" role="button" style="float: right" visible="true" data-toggle="modal" runat ="server">Ajuste rápido de entrada</a> 
    <a ID="botonCambioSesion" accesskey="S" href="#modalCambioSesion" class="btn btn-info-fozkr" role="button" style="float: right" visible="true" data-toggle="modal" runat ="server">Cambio rápido de sesión</a>  
    <br />
    <br />

    <!-- Título de la acción que se está realizando -->
    <h3 ID="tituloAccionFacturas" runat="server"></h3>
    <br />

    <!-- Panel para consultar facturas -->
    <asp:Panel ID="PanelConsultarFacturas" runat="server" Visible="false">
        <div class="row" ID="bloqueFormulario" runat="server">
            <div class="col-lg-12">
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Estación:</label>    
                    <asp:DropDownList ID="dropDownListConsultaEstacion" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="90%" CssClass="form-control"></asp:DropDownList>

                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Bodega:</label>    
                    <asp:DropDownList ID="dropDownListConsultaBodega" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="90%" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaVendedor" class="control-label">Vendedor:</label>    
                    <asp:DropDownList ID="dropDownListConsultaVendedor" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="90%" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <button runat="server" onserverclick="clickBotonEjecutarConsulta" ID="botonEjecutarConsulta" class="btn btn-info-fozkr" type="button" style="float:left; margin-top:9%;">
                        <i></i>Consultar
                    </button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Panel para consultar la información específica de una factura después de seleccionarla en el grid -->
    <asp:Panel ID="PanelConsultarFacturaEspecifica" runat="server" Visible="false">
        <table class="table table-fozkr">
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td rowspan="6"> <%--Grid con los productos de la factura, tiene su propia "fila" (abarcando todas las demás) para encontrarlo fácilmente aquí en el aspx--%>
                    <strong><div ID="divTituloGridProductosFactura" runat="server" tabindex="" class="control-label" style="text-align:center; font-size:larger; background-color: #C0C0C0;">Productos de la factura</div></strong>
                    <asp:UpdatePanel ID="UpdatePanelFacturaConsultada" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridFacturaEspecificaProductos" CssClass="table" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                               <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                               <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                               <AlternatingRowStyle BackColor="#F8F8F8" />
                               <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaConsecutivo" class="control-label">Consecutivo:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaConsecutivo" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    <label for="FacturaConsultadaEstacion" class="control-label">Estación:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaEstacion" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    <label for="FacturaConsultadaBodega" class="control-label">Bodega:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaBodega" runat="server" class="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaFechaHora" class="control-label">Fecha y hora:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaFechaHora" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    <label for="FacturaConsultadaVendedor" class="control-label">Vendedor:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaVendedor" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    <label for="FacturaConsultadaCliente" class="control-label">Cliente:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaCliente" runat="server" class="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaTipoMoneda" class="control-label">Tipo de moneda:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaTipoMoneda" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    <label for="FacturaConsultadaImpuesto" class="control-label">Impuesto:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaImpuesto" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    <label for="FacturaConsultadaMetodoPago" class="control-label">Método de pago:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaMetodoPago" runat="server" class="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaActividad" class="control-label">Actividad:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadActividad" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    <label for="FacturaConsultadaEstado" class="control-label">Estado:</label>
                    <asp:TextBox ID="textBoxFacturaConsultadaEstado" runat="server" class="form-control"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <!-- Panel con el grid de consultar facturas (se mantiene aparte para poder esconder los campos de consulta grupal y mostrar los de consulta individual, sin tocar el grid) -->
    <br />
    <br />
    <asp:Panel ID="PanelGridConsultas" runat="server" Visible="false">
        <strong><div ID="tituloGrid" runat="server" tabindex="" class="control-label" style="text-align:center; font-size:larger; background-color: #C0C0C0;">Facturas en el sistema</div></strong>
            <asp:UpdatePanel ID="UpdatePanelFacturas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewFacturas" CssClass="table" OnRowCommand="gridViewFacturas_FilaSeleccionada" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black"/>
                       <PagerStyle CssClass="paging" HorizontalAlign="Center"/>
                       <AlternatingRowStyle BackColor="#F8F8F8"/>
                       <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White"/>
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver"/>
                  </asp:GridView>
             </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridViewFacturas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
    </asp:Panel>

    <!-- Panel crear factura -->
    <asp:Panel ID="PanelCrearFactura" runat="server" Visible="true">
        <table class="table table-bills-fozkr">
            <tr>
                <td colspan="4">Fecha y hora:  <%: DateTime.Now.Date.ToShortDateString() %>,  <%: DateTime.Now.TimeOfDay.ToString().Substring(0,5) %></td></tr>
            <tr>
                <td>Estación:</td>
                <td><asp:DropDownList ID="dropDownListCrearFacturaEstacion" onselectedindexchanged="dropDownListCrearFacturaEstacion_SelectedIndexChanged" AutoPostBack="true" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="90%" CssClass="form-control"></asp:DropDownList></td>
                <td>Bodega:</td>
                <td><asp:DropDownList ID="dropDownListCrearFacturaBodega" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="90%" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Vendedor:</td>
                <td><asp:TextBox ID="textBoxCrearFacturaVendedor" runat="server" CssClass="form-control" Width="90%"></asp:TextBox></td>
                <td>Tipo cambio:</td>
                <td><asp:TextBox ID="textBoxCrearFacturaTipoCambio" runat="server" CssClass="form-control" Width="90%"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="4">Lista de productos en la factura:</td>
            </tr>
            <tr>
                <td>Producto:</td>
                <td colspan="2"><asp:TextBox ID="textBoxAutocompleteCrearFacturaBusquedaProducto" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                </td>
                <td><button type="button" ID="botonCrearFacturaAgregarProducto" class="btn btn-success-fozkr" onserverclick="clickBotonAgregarProductoFactura" disabled="disabled" runat="server">Agregar Producto</button></td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <%--<asp:GridView ID="gridViewCrearFacturaProductos" CssClass="table" runat="server" AllowPaging="True" PageSize="5" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" GridLines="None" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Seleccionar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Nombre" />
                                    <asp:BoundField HeaderText="Código interno" />
                                    <asp:TemplateField HeaderText="Cantidad">
                                        <ItemTemplate>
                                            <asp:TextBox ID="gridCrearFacturaCantidadProducto" runat="server" ReadOnly="false" Width="50px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Precio unitario" />
                                    <asp:BoundField HeaderText="Impuesto" />
                                    <asp:BoundField HeaderText="Descuento" />
                                </Columns>
                                <RowStyle Font-Size="small" BackColor="White" ForeColor="Black"/>
                                <PagerStyle CssClass="paging" HorizontalAlign="Center"/>
                                <AlternatingRowStyle BackColor="#F8F8F8"/>
                                <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White"/>
                                <HeaderStyle CssClass="active" Font-Size="Small" Font-Bold="true" BackColor="Silver"/>
                            </asp:GridView>--%>
                            <asp:GridView ID="gridViewCrearFacturaProductos" CssClass="table" runat="server" AllowPaging="True" PageSize="5" BorderColor="#ffffff" BorderStyle="Solid" BorderWidth="1px" GridLines="None" ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="Seleccionar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad">
                                        <ItemTemplate>
                                            <asp:TextBox ID="gridCrearFacturaCantidadProducto" runat="server" ReadOnly="false" Width="50px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Font-Size="small" BackColor="White" ForeColor="Black"/>
                                <PagerStyle CssClass="paging" HorizontalAlign="Center"/>
                                <AlternatingRowStyle BackColor="#F8F8F8"/>
                                <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White"/>
                                <HeaderStyle CssClass="active" Font-Size="Small" Font-Bold="true" BackColor="Silver"/>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            <tr>
                <td colspan="4">
                    <button type="button" ID="botonCrearFacturaQuitarProducto" class="btn btn-danger-fozkr" href="#modalCancelarFactura" data-toggle="modal" style="float: left" disabled="disabled" runat="server">Quitar producto</button>
                    <button type="button" ID="botonCrearFacturaDescuentoProducto" class="btn btn-warning-fozkr" onserverclick="Page_Load" style="float: left" disabled="disabled" runat="server">Aplicar descuento</button>
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: right;">Precio total:</td>
                <td><asp:Label ID="labelCrearFacturaPrecioTotal" runat="server"></asp:Label></td>
                <td><asp:Button ID="botonCrearFacturaSwitchPrecios" class="btn" runat="server" Text="Cambiar moneda ₡/$"/></td>
            </tr>
            <tr>
                <td>Método de pago:</td>
                <td colspan="3"><asp:DropDownList ID="dropDownListCrearFacturaMetodoPago" class="input input-fozkr-dropdownlist" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Cliente:</td>
                <td colspan="3"><asp:DropDownList ID="dropDownListCrearFacturaCliente" class="input input-fozkr-dropdownlist" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="4">
                    <button type="button" ID="botonCrearFacturaCancelar" class="btn btn-danger-fozkr" href="#modalCancelarFactura" data-toggle="modal" style="float: right" runat="server">Cancelar</button>
                    <button type="button" ID="botonCrearFacturaGuardar" class="btn btn-success-fozkr" onserverclick="Page_Load" style="float: right" runat="server">Guardar</button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    
     <!--Modal Cambio de sesión-->
    <div class="modal fade" ID="modalCambioSesion" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" ID="modalTitleCambioSesion"><i></i>Cambio rápido de sesión</h4>
                </div>
                <div class="modal-body">
                    Ingrese las nuevas credenciales de usuario con las que desea utilizar el sistema.
                </div>
                <div style="margin-left:30px">
                    <div style="margin-left:30px; margin-right:70px" class="form-group">
                        <label for="inputUsername" class= "control-label"> Nombre de usuario: </label>      
                        <input type="text" ID= "inputUsername" runat="server" class="form-control" style="max-width:100%" >
                    </div>
                    <br />
                    <div style="margin-left:30px; margin-right:70px" class="form-group">
                        <label for="inputPassword" class= "control-label"> Contraseña: </label>      
                        <input type="password" ID= "inputPassword" runat="server" class="form-control"  style="max-width:100%" >
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" ID="botonAceptarCambioUsuario" class="btn btn-success-fozkr" onserverclick="botonAceptarCambioUsuario_ServerClick" runat="server">Aceptar</button>
                    <button type="button" ID="botonCancelarCambioUsuario" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

     <!--Modal Ajuste rápido de inventario-->
    <div class="modal fade" ID="modalAjusteRapido" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" ID="modalTitleAjusteInventario"><i></i>Ajuste rápido de inventario</h4>
                </div>
                <div class="modal-body">
                    Ingrese los datos con los que desea hacer el ajuste rápido de inventario.

                <!-- Ingresar el producto y la cantidad
                    Para el producto, hay que poner un grid y una barra de busqueda
                    -->

                </div>
                <div class="modal-footer">
                    <button type="button" ID="botonAceptarAjusteRapido" class="btn btn-success-fozkr" onserverclick="botonAceptarCambioUsuario_ServerClick" runat="server">Aceptar</button>
                    <button type="button" ID="botonCancelarAjusteRapido" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>



    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab()
        {
            document.getElementById("linkFormVentas").className = "active";
        }
    </script>
    <!-- Código necesario para el autocomplete -->
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.autocomplete.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=textBoxAutocompleteCrearFacturaBusquedaProducto.ClientID%>").autocomplete('Search_CS.ashx');
        });       
    </script>

</asp:Content>
