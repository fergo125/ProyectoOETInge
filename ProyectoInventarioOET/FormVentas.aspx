<%@ Page Title="Ventas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormVentas.aspx.cs" Inherits="ProyectoInventarioOET.FormVentas" %>
<asp:Content ID="ContentVentas" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Label para desplegar mensajes -->
    <br />
    <div>
        <div ID="mensajeAlerta" class="" runat="server" Visible="false" style="margin-left:60%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text=""></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 ID="TituloFormVentas" runat="server">Facturación</h2>
        <hr />
    </div>

    <!-- Botones principales -->
    <button runat="server" onserverclick="clickBotonConsultar" ID="botonConsultar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Consultar Facturas</button>
    <button runat="server" onserverclick="clickBotonCrear" ID="botonCrear" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Crear Factura</button>
    <button runat="server" onserverclick="clickBotonModificar" ID="botonModificar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Modificar Factura</button>
    <a ID="botonAjusteEntrada" accesskey="A" href="#modalAjusteRapido" class="btn btn-info-fozkr" role="button" style="float: right" visible="false" data-toggle="modal" runat ="server">Ajuste rápido de entrada</a> 
    <a ID="botonCambioSesion" accesskey="S" href="#modalCambioSesion" class="btn btn-info-fozkr" role="button" style="float: right" visible="false" data-toggle="modal" runat ="server">Cambio rápido de sesión</a>  
    <br />
    <br />

    <!-- Título de la acción que se está realizando -->
    <h3 ID="tituloAccionForm" runat="server"></h3>

    <!-- Panel para consultar facturas -->
    <asp:Panel ID="PanelConsultar" runat="server" Visible="false">
        <br />
        <div class="row" runat="server">
            <div class="col-lg-12">
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Estación:</label>
                    <asp:DropDownList ID="dropDownListConsultaEstacion" AutoPostBack="true" onselectedindexchanged="dropDownListConsultaEstacion_ValorCambiado" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="95%" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Bodega:</label>
                    <asp:DropDownList ID="dropDownListConsultaBodega" AutoPostBack="true" onselectedindexchanged="dropDownListConsultaBodega_ValorCambiado" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="95%" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaVendedor" class="control-label">Vendedor:</label>
                    <asp:DropDownList ID="dropDownListConsultaVendedor" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="95%" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-1" style="margin-top:2%;">
                    <button runat="server" onserverclick="clickBotonEjecutarConsulta" ID="botonEjecutarConsulta" class="btn btn-info-fozkr" type="button">
                        <i></i>Consultar
                    </button>
                </div>
                <div class="form-group col-lg-2" style="margin-top:2.5%;">
					<asp:CheckBox ID="checkboxConsultaDetalles" OnCheckedChanged="checkBoxConsultarDetalles_CheckCambiado" CssClass="input-fozkr-check" runat="server" AutoPostBack="true"/>
                    <label for="ConsultaDetalles" class="control-label">Más detalles</label>    
                </div>
            </div>
        </div>
        <div class="row" visible="false" ID="detallesConsulta" runat="server">
            <div class="col-lg-12">
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Método de pago:</label>
                    <asp:DropDownList ID="dropDownListConsultaMetodoPago" AutoPostBack="true" class="input input-fozkr-dropdownlist" runat="server" Enabled="true" Width="95%" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Cliente:</label>
                    <asp:DropDownList ID="dropDownListConsultaCliente" AutoPostBack="true" class="input input-fozkr-dropdownlist" runat="server" Enabled="true" Width="95%" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaVendedor" class="control-label">Fecha inicial:</label>
                    <table class="table-fozkr">
                        <tr>
                            <td><input id="textboxConsultaFechaInicio" disabled="disabled" class="form-control" type="text"/></td>
                            <td><button runat="server" ID="botonConsultaCalendarioInicio" class="btn btn-default" type="button">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaVendedor" class="control-label">Fecha final:</label>
                    <table class="table-fozkr">
                        <tr>
                            <td><input id="textboxConsultaFechaFinal" disabled="disabled" class="form-control" type="text"/></td>
                            <td><button runat="server" ID="botonConsultaCalendarioFinal" class="btn btn-default" type="button">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Panel para consultar la información específica de una factura después de seleccionarla en el grid -->
    <asp:Panel ID="PanelConsultarFacturaEspecifica" runat="server" Visible="false">
        <table class="table table-fozkr">
            <tr>
                <td style="width:20%"></td>
                <td style="width:20%"></td>
                <td style="width:20%"></td>
                <td rowspan="6"> <%--Grid con los productos de la factura, tiene su propia "fila" (abarcando todas las demás) para encontrarlo fácilmente aquí en el aspx--%>
                    <asp:Panel ID="PanelGridProductosFacturaEspecifica" runat="server" Width="100%">
                        <strong><div ID="divTituloGridProductosFactura" runat="server" tabindex="" class="control-label" style="text-align:center; font-size:larger; background-color: #C0C0C0;">Productos de la factura</div></strong>
                        <asp:GridView ID="gridFacturaEspecificaProductos" CssClass="table" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                            <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                            <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="#F8F8F8" />
                            <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaConsecutivo" class="control-label">Consecutivo:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaConsecutivo" runat="server" class="form-control">
                </td>
                <td>
                    <label for="FacturaConsultadaBodega" class="control-label">Bodega:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaBodega" runat="server" class="form-control">
                </td>
                <td>
                    <label for="FacturaConsultadaEstacion" class="control-label">Estación:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaEstacion" runat="server" class="form-control">
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaFechaHora" class="control-label">Fecha y hora:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaFechaHora" runat="server" class="form-control">
                </td>
                <td>
                    <label for="FacturaConsultadaVendedor" class="control-label">Vendedor:</label>
                    <input type="text"  disabled="disabled" ID="textBoxFacturaConsultadaVendedor" runat="server" class="form-control">
                </td>
                <td>
                    <label for="FacturaConsultadaCliente" class="control-label">Cliente:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaCliente" runat="server" class="form-control">
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaMontoTotal" class="control-label">Monto total:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaMontoTotal" runat="server" class="form-control">
                </td>
                <td>
                    <label for="FacturaConsultadaTipoMoneda" class="control-label">Tipo de moneda:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaTipoMoneda" runat="server" class="form-control">
                </td>
                <td>
                    <label for="FacturaConsultadaMetodoPago" class="control-label">Método de pago:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadaMetodoPago" runat="server" class="form-control">
                </td>
            </tr>
            <tr>
                <td>
                    <label for="FacturaConsultadaActividad" class="control-label">Actividad:</label>
                    <input type="text" disabled="disabled" ID="textBoxFacturaConsultadActividad" runat="server" class="form-control">
                </td>
                <td>
                    <label for="FacturaConsultadaEstado" class="control-label">Estado:</label>
                    <asp:DropDownList ID="textBoxFacturaConsultadaEstado" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" CssClass="form-control"></asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <button type="button" ID="botonAceptarModificacionFacturaEspecifica" class="btn btn-success-fozkr" visible="false" onserverclick="clickBotonAceptarModificar" data-toggle="modal" runat="server">Aceptar</button>
    <button type="button" ID="botonCancelarModificacionFacturaEspecifica" class="btn btn-danger-fozkr" visible="false" onserverclick="clickBotonCancelarModificar" runat="server">Cancelar</button>
                


    <!-- Panel con el grid de consultar facturas (se mantiene aparte para poder esconder los campos de consulta grupal y mostrar los de consulta individual, sin tocar el grid) -->
    <br />
    <br />
    <%--<asp:Panel ID="PanelGridConsultarFacturas" runat="server"></asp:Panel>--%>
    <strong><div ID="tituloGrid" runat="server" tabindex="" class="control-label" style="text-align:center; font-size:larger; background-color: #C0C0C0;" visible="false">Facturas en el sistema</div></strong>
        <asp:GridView ID="gridViewFacturas" Visible="false" CssClass="table able-responsive table-condensed" OnRowCommand="gridViewFacturas_FilaSeleccionada" OnPageIndexChanging="gridViewFacturas_CambioPagina" runat="server" AllowPaging="true" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
            <Columns>
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                    <ControlStyle CssClass="btn btn-default"></ControlStyle>
                </asp:ButtonField>
            </Columns>
            <RowStyle Font-Size="small" BackColor="White" ForeColor="Black"/>
            <PagerStyle CssClass="paging" HorizontalAlign="Center"/>
            <AlternatingRowStyle BackColor="#F8F8F8"/>
            <SelectedRowStyle CssClass="info" Font-Bold="false" ForeColor="Black"/>
            <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver"/>
        </asp:GridView>

    <!-- Panel crear factura -->
    <asp:Panel ID="PanelCrearFactura" runat="server" Visible="false">
        <table class="table table-bills-fozkr">
            <tr>
                <td colspan="4">Fecha y hora:  <%: DateTime.Now.Date.ToShortDateString() %>,  <%: DateTime.Now.TimeOfDay.ToString().Substring(0,5) %></td></tr>
            <tr>
                <td>Estación:</td>
                <td><asp:TextBox ID="textBoxCrearFacturaEstacion" CssClass="form-control" Width="90%" Enabled="false" runat="server"></asp:TextBox></td>
                <td>Bodega:</td>
                <td><asp:TextBox ID="textBoxCrearFacturaBodega" CssClass="form-control" Width="90%" Enabled="false" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Vendedor:</td>
                <td><asp:TextBox ID="textBoxCrearFacturaVendedor" CssClass="form-control" Width="90%" Enabled="false" runat="server"></asp:TextBox></td>
                <td>Tipo cambio:</td>
                <td><asp:TextBox ID="textBoxCrearFacturaTipoCambio" CssClass="form-control" Width="90%" Enabled="false" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="4">Lista de productos en la factura:</td>
            </tr>
            <tr>
                <td>Producto:</td>
                <td colspan="2"><asp:TextBox ID="textBoxAutocompleteCrearFacturaBusquedaProducto" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                </td>
                <td><button type="button" ID="botonCrearFacturaAgregarProducto" class="btn btn-success-fozkr" onserverclick="clickBotonAgregarProductoFacturaNueva" runat="server">Agregar Producto</button></td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Panel ID="PanelCrearFacturaProductos" runat="server" ScrollBars="Vertical" Height="300px">
                    <asp:GridView ID="gridViewCrearFacturaProductos" CssClass="table" runat="server" AllowPaging="False" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" GridLines="None" ShowHeaderWhenEmpty="True">
                        <Columns>
							<asp:TemplateField HeaderText="Seleccionar">
								<ItemTemplate>
									<asp:CheckBox ID="gridCrearFacturaCheckBoxSeleccionarProducto" OnCheckedChanged="checkBoxCrearFacturaProductos_CheckCambiado" runat="server" AutoPostBack="true"/>
								</ItemTemplate>
							</asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad">
                                <ItemTemplate>
                                    <asp:TextBox ID="gridCrearFacturaTextBoxCantidadProducto" OnTextChanged="textBoxCrearFacturaProductosCantidad_TextoCambiado" ReadOnly="false" Width="75%" CausesValidation="true" AutoPostBack="true" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="gridCrearFacturaTextBoxCantidadProducto" ClientValidationFunction="changeColor" Display="Dynamic"
                                            ForeColor="Red" BorderStyle="Dotted" runat="server" ErrorMessage="Ingrese solo números enteros" Font-Bold="true" ValidationExpression="\d+$"></asp:RegularExpressionValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle Font-Size="small" BackColor="White" ForeColor="Black"/>
                        <PagerStyle CssClass="paging" HorizontalAlign="Center"/>
                        <AlternatingRowStyle BackColor="#F8F8F8"/>
                        <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="Black"/>
                        <HeaderStyle CssClass="active" Font-Size="Small" Font-Bold="true" BackColor="Silver"/>
                    </asp:GridView>
                    </asp:Panel>
                </td>
            <tr>
                <td colspan="4">
                    <button type="button" ID="botonCrearFacturaEliminarProducto" class="btn btn-danger-fozkr" onserverclick="clickBotonCrearEliminarProducto" style="float: left" disabled="disabled" runat="server">Eliminar Producto</button>
                    <button type="button" ID="botonCrearFacturaModificarProducto" class="btn btn-warning-fozkr" href="#modalModificarProducto" data-toggle="modal" style="float: left" disabled="disabled" runat="server">Modificar Producto</button>
                </td>
            </tr>
            <tr>
                <td>Tipo moneda: <asp:Label ID="labelCrearFacturaTipoMoneda" runat="server"></asp:Label></td>
                <td><asp:Button ID="botonCrearFacturaSwitchPrecios" OnClick="clickBotonCambiarTipoMoneda" class="btn" runat="server" Text="Cambiar moneda ₡/$"/></td>
                <td style="text-align: right;">Precio total:</td>
                <td><asp:Label ID="labelCrearFacturaPrecioTotal" runat="server" Text="0"></asp:Label></td>
            </tr>
            <tr>
                <td>Método de pago:</td>
                <td colspan="3"><asp:DropDownList ID="dropDownListCrearFacturaMetodoPago" OnSelectedIndexChanged="dropDownListCrearFacturaMetodoPago_ValorCambiado" AutoPostBack="true" class="input input-fozkr-dropdownlist" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Cliente:</td>
                <td colspan="3"><asp:DropDownList ID="dropDownListCrearFacturaCliente" class="input input-fozkr-dropdownlist" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Actividad:</td>
                <td colspan="3"><asp:DropDownList ID="dropDownListCrearFacturaActividad" class="input input-fozkr-dropdownlist" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="4">
                    <button type="button" ID="botonCrearFacturaGuardar" class="btn btn-success-fozkr" onserverclick="clickBotonCrearGuardar" runat="server">Guardar</button>
                    <%--<button type="button" ID="botonCrearFacturaCancelar" class="btn btn-danger-fozkr" href="#modalCancelarFactura" data-toggle="modal" runat="server">Cancelar</button>--%>
                    <button type="button" ID="Button1" class="btn btn-danger-fozkr" href="#modalVariosMetodosPago" data-toggle="modal" runat="server">Cancelar</button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <!--Modal Cancelar-->
    <div class="modal fade" ID="modalCancelarFactura" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" ID="modalCancelarFacturaTitulo"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar cancelación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea cancelar la operación? Perdería todos los datos no guardados.
                </div>
                <div class="modal-footer">
                    <button type="button" ID="botonAceptarModalCancelar" causesvalidation="false" class="btn btn-success-fozkr" runat="server" onserverclick="clickBotonAceptarModalCancelar">Aceptar</button>
                    <button type="button" ID="botonCancelarModalCancelar" causesvalidation="false" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
    
    <!--Modal Métodos de pago-->
    <div class="modal fade" ID="modalVariosMetodosPago" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" ID="modalVariosMetodosPagoTitulo"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Escoger varios métodos de pago</h4>
                </div>
                <div class="modal-body">
                    Escoja los diferentes métodos de pago usados y la cantidad pagada con cada uno.
                    <br />
                    <br />
                    <div class="row" ID="Div1" runat="server">
                        <div class="col-lg-12">
                            <div class="form-group col-lg-8">
                                <asp:DropDownList ID="dropDownList1" OnSelectedIndexChanged="dropDownListCrearFacturaMetodoPago_ValorCambiado" AutoPostBack="true" class="input input-fozkr-dropdownlist" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-3">
                                <button type="button" ID="botonVariosMetodosPagoAgregarMetodo" class="btn btn-success-fozkr" onserverclick="clickBotonAgregarProductoFacturaNueva" runat="server">Agregar Método</button>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="PanelmodalVariosMetodosPago" runat="server" ScrollBars="Vertical" Height="300px">
                    <asp:GridView ID="gridViewModalVariosMetodosPago" CssClass="table" runat="server" AllowPaging="False" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" GridLines="None" ShowHeaderWhenEmpty="True">
                        <Columns>
							<asp:TemplateField HeaderText="Seleccionar">
								<ItemTemplate>
									<asp:CheckBox ID="gridViewModalVariosMetodosPagoSeleccionarMetodo" OnCheckedChanged="checkBoxCrearFacturaProductos_CheckCambiado" runat="server" AutoPostBack="true"/>
								</ItemTemplate>
							</asp:TemplateField>
                            <asp:TemplateField HeaderText="Pago">
                                <ItemTemplate>
                                    <asp:TextBox ID="gridViewModalVariosMetodosPagoTextBoxPago" OnTextChanged="textBoxCrearFacturaProductosCantidad_TextoCambiado" ReadOnly="false" Width="75%" CausesValidation="true" AutoPostBack="true" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="gridCrearFacturaTextBoxCantidadProducto" ClientValidationFunction="changeColor" Display="Dynamic"
                                            ForeColor="Red" BorderStyle="Dotted" runat="server" ErrorMessage="Ingrese solo números enteros" Font-Bold="true" ValidationExpression="\d+$"></asp:RegularExpressionValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle Font-Size="small" BackColor="White" ForeColor="Black"/>
                        <PagerStyle CssClass="paging" HorizontalAlign="Center"/>
                        <AlternatingRowStyle BackColor="#F8F8F8"/>
                        <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="Black"/>
                        <HeaderStyle CssClass="active" Font-Size="Small" Font-Bold="true" BackColor="Silver"/>
                    </asp:GridView>
                    </asp:Panel>
                    <button type="button" ID="Button3" class="btn btn-danger-fozkr" onserverclick="clickBotonCrearEliminarProducto" style="float: left" disabled="disabled" runat="server">Eliminar Método</button>
                    <br />
                    <br />
                </div>
                <div class="modal-footer">
                    <button type="button" ID="botonAceptarModalMetodosPago" causesvalidation="false" class="btn btn-success-fozkr" runat="server" onserverclick="clickBotonAceptarModalCancelar">Aceptar</button>
                    <button type="button" ID="botonCancelarModalMetodosPago" causesvalidation="false" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

    <!--Modal Modificar producto-->
    <div class="modal fade" ID="modalModificarProducto" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" ID="modalTitleModificarProducto"><i></i>Modificar producto</h4>
                </div>
                <div class="modal-body">
                    Seleccione un porcentaje de descuento para el producto o cambie su impuesto
                </div>
                <table class="table-fozkr" style="margin-left: 10%; width: 80%">
                    <tr>
                        <td>
                            <label class= "control-label">Descuento:</label>
                            <asp:DropDownList ID="dropdownlistModalModificarProductoDescuento" CssClass="form-control" Width="75%" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <label class= "control-label">Impuesto:</label>
                            <asp:DropDownList ID="dropdownlistModalModificarProductoImpuesto" CssClass="form-control" Width="75%" runat="server">
                                <asp:ListItem Value="Sí">Sí</asp:ListItem>
                                <asp:ListItem Value="No">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div class="modal-footer">
                    <button type="button" ID="botonAceptarModalModificarProducto" class="btn btn-success-fozkr" onserverclick="clickBotonAceptarModalModificarProducto" runat="server">Aceptar</button>
                    <button type="button" ID="botonCancelarModalModificarProducto" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

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
                    <button type="button" ID="botonAceptarCambioUsuario" class="btn btn-success-fozkr"  onserverclick="clickBotonAceptarCambioUsuario"   runat="server">Aceptar</button>
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
                    <br />
                    <br />
                    <label for="productoParaAjuste" class= "control-label"> Producto a ajustar: </label>     
                    <asp:TextBox ID="textBoxAutocompleteAjusteRapidoBusquedaProducto" runat="server" CssClass="form-control" style="max-width:100%"></asp:TextBox>
                    <br />
                    <label for="cantidadParaAjuste" class= "control-label"> Nueva cantidad: </label>     
                    <asp:TextBox ID="nuevaCantidadParaAjusteRapido" runat="server" CssClass="form-control" style="max-width:100%"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <button type="button" ID="botonAceptarAjusteRapido" class="btn btn-success-fozkr" onserverclick="clickBotonAceptarAjusteRapido" runat="server">Aceptar</button>
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
    <!-- Código necesario para el autocomplete de agregar productos a la factura -->
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.autocomplete.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=textBoxAutocompleteCrearFacturaBusquedaProducto.ClientID%>").autocomplete('Search_CS.ashx');
        });       
    </script>
    <!-- Código necesario para el autocomplete de escoger un producto para un ajuste -->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=textBoxAutocompleteAjusteRapidoBusquedaProducto.ClientID%>").autocomplete('Search_CS.ashx');
        });
    </script>

</asp:Content>
