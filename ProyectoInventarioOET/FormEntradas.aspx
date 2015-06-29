<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormEntradas.aspx.cs" Inherits="ProyectoInventarioOET.FormEntradas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" runat="server" visible =" false" style="margin-left: 50%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server"></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloEntradas" runat="server">Gestión de Entradas al Inventario</h2>
        <hr />
    </div>

    <!-- Botones de acciones de la interfaz -->
    
    <button runat="server" onserverclick="botonConsultaEntradas_ServerClick" causesvalidation="false"  id="botonConsultaEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Consultar Entradas</button>
    <button runat="server" onserverclick="botonAgregarEntradas_ServerClick" causesvalidation="false" id="botonAgregarEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Crear Entrada</button>
    <br />
    <br />

    <!-- Título de acción -->
    <h3 id="tituloAccionEntradas" runat="server"></h3>

    <!-- Bloque con la tabla que muestra las entradas -->
    <div id="bloqueGridEntradas" class="col-lg-12">
        <fieldset id="FieldsetGridEntradas" runat="server" class="fieldset" visible="false">
         <div class="col-lg-12"><strong><div ID="Div2" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Listado de Entradas</div></strong>
<%--            <asp:UpdatePanel ID="UpdatePanelEntradas" runat="server">
                <ContentTemplate>--%>
                    <asp:GridView ID="gridViewEntradas" CssClass="table" OnRowCommand="gridViewEntradas_RowCommand" OnPageIndexChanging="gridViewEntradas_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                       <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#F8F8F8" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                  </asp:GridView>
<%--             </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridViewEntradas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>--%>
       </div>
        </fieldset>        
    </div>

    <!-- Bloque que muestra la información de la entrada -->
    <div id="bloqueGridProductosDeEntrada" class="col-lg-12">
        <fieldset id="FieldsetGridProductosDeEntrada" runat="server" class="fieldset" visible="false">

            <div id="FieldsetEncabezadoEntrada" runat="server" class="col-lg-12">
                        <h4>Encabezado de la Entrada</h4>

                    <div class="well well-lg" id="Div5" runat="server">
                        <div class="row col-lg-12">
                            <div class= "form-group col-lg-3">
                                <label for="outputEntrada" class= "control-label">Código de Entrada:</label>      
                                <p id="outputEntrada" runat="server" class="form-control-static"></p>
                            </div>
            
                            <div class="form-group col-lg-3">
                                <label for="outputFacturaAsociada" class= "control-label">Factura Asociada:</label>      
                                <p id="outputFacturaAsociada" runat="server" class="form-control-static"></p>
                            </div>

                            <div class="form-group col-lg-3">
                                <label for="outputUsuario" class= "control-label">Usuario Responsable:</label>      
                                <p id="outputUsuario" runat="server" class="form-control-static"></p>
                            </div>

                            <div class="form-group col-lg-3">
                                <label for="outputBodega" class= "control-label">Bodega:</label>      
                                <p id="outputBodega" runat="server" class="form-control-static"></p>
                            </div>

                            <div class="form-group col-lg-3">
                                <label for="outputFecha" class="control-label">Fecha de Realización:</label>      
                                <p id="outputFecha" runat="server" class="form-control-static"></p>
                            </div>

                        </div>       
                    </div>
            </div>
            <div class="col-lg-12"><strong><div ID="Div4" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Productos relacionados con la Entrada</div></strong>
        <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                <asp:GridView ID="gridProductosDeEntrada" CssClass="table" OnPageIndexChanging="gridProductosDeEntrada_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                    <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                    <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                    <AlternatingRowStyle BackColor="#F8F8F8" />
                    <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="Green" />
                    <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                </asp:GridView>
        <%--    </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridProductosDeEntrada" EventName="RowCommand" />
                </Triggers>
            </asp:UpdatePanel>--%>
            </div> 
        </fieldset>
    </div>

    <!-- Sección con lo necesario para realizar una búsqueda de una factura -->
    <fieldset id="FielsetBuscarFactura" runat="server" visible="false">
        <div class="row">
            <label class= "col-lg-12">Buscar factura:</label>
        </div>
        <div class="row">
            <div class="col-lg-7">
                <input id="barraDeBusquedaFactura" class="form-control" type="search" placeholder="Ingresa el código de la Factura" runat="server" >
            </div>
            <div class="col-lg-1">
                <asp:Button ID="botonBuscarFactura" runat="server" Text="Buscar" CssClass="btn btn-info-fozkr" OnClick="botonBuscarFactura_Click"/>
            </div>
            <div class="col-lg-2">
                <asp:Button ID="botonMostrarFacturas" runat="server" Text="Mostrar Todas" CssClass="btn btn-info-fozkr" OnClick="botonMostrarFacturas_Click"/>
            </div>
        </div>
    </fieldset>

    <br />
    <br />

    <!-- Tabla con los resultados de búsqueda de facturas y sección para mostrar el encabezado -->
    <div id="bloqueGridFacturas" class="col-lg-12">
        <fieldset id="FieldsetGridFacturas" runat="server" class="fieldset" visible="false">
         <div class="col-lg-12"><strong><div ID="tituloGrid" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Listado de Facturas</div></strong>
<%--            <asp:UpdatePanel ID="UpdatePanelFacturas" runat="server">
                <ContentTemplate>--%>
                    <asp:GridView ID="gridViewFacturas" CssClass="table" OnRowCommand="gridViewFacturas_RowCommand" OnPageIndexChanging="gridViewFacturas_PageIndexChanging" runat="server" AllowPaging="true" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Seleccionar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                       <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#F8F8F8" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                  </asp:GridView>
<%--             </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridViewFacturas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>--%>
       </div>
        </fieldset>        
    </div>
    <br />
    <div id="FieldsetEncabezadoFactura" runat="server"  visible="false">
        <fieldset id="FieldsetEncabezadoFactura1" runat="server" class="fieldset" >

                <h4>Encabezado de la Factura</h4>

            <div class="well well-lg" id="camposEncabezadoFactura" runat="server">
                <div class="row col-lg-12">
                    <div class= "form-group col-lg-3">
                        <label for="outputFactura" class= "control-label">Factura:</label>      
                        <p id="outputFactura" runat="server" class="form-control-static"></p>
                    </div>

                    <div class= "form-group col-lg-3">
                        <label for="outputProveedor" class= "control-label">Proveedor:</label>      
                        <p id="outputProveedor" runat="server" class="form-control-static"></p>
                    </div>
            
                    <div class="form-group col-lg-3">
                        <label for="outputFechaPago" class= "control-label">Fecha de Pago:</label>      
                        <p id="outputFechaPago" runat="server" class="form-control-static"></p>
                    </div>

                    <div class="form-group col-lg-3">
                        <label for="outputTipoPago" class= "control-label">Tipo de Pago:</label>      
                        <p id="outputTipoPago" runat="server" class="form-control-static"></p>
                    </div>

                    <div class="form-group col-lg-3">
                        <label for="outputPlazoPago" class= "control-label">Plazo de Pago:</label>      
                        <p id="outputPlazoPago" runat="server" class="form-control-static"></p>
                    </div>

                    <div class="form-group col-lg-3">
                        <label for="outputImpuestos" class="control-label">Tipo de Moneda:</label>      
                        <p id="outputMoneda" runat="server" class="form-control-static"></p>
                    </div>

                    <div class="form-group col-lg-3">
                        <label for="outputImpuestos" class="control-label">Tipo de Cambio:</label>      
                        <p id="outputTipoCambio" runat="server" class="form-control-static"></p>
                    </div>

                    <div class="form-group col-lg-3">
                        <label for="outputDescuento" class= "control-label">Descuento:</label>      
                        <p id="outputDescuento" runat="server" class="form-control-static"></p>
                    </div>
                     <div class="form-group col-lg-3">
                        <label for="outputImpuestos" class="control-label">Retención de Impuestos:</label>      
                        <p id="outputImpuestos" runat="server" class="form-control-static"></p>
                    </div>

                    <div class="form-group col-lg-3">
                        <label for="outputSubtotal" class= "control-label">SubTotal:</label>      
                        <p id="outputSubtotal" runat="server" class="form-control-static"></p>
                    </div>
                    
                    <div class="form-group col-lg-3">
                        <label for="outputTotal" class= "control-label">Total:</label>      
                        <p id="outputTotal" runat="server" class="form-control-static"></p>
                    </div>

                   
                </div>       
            </div>

        </fieldset>
    </div>

    <br />
    <br />


    <!-- Contiene el detalle de la factura: la creada desde el sistema de compras, búsqueda
         de productos y la factura detallada que se está creando
     -->
    <fieldset id="FieldsetCrearFactura" runat="server" visible="false">
        <h4>Utilice la barra de búsqueda para seleccionar los productos entrantes</h4>
        <br />
        <div class="row">
            <div id="bloqueGridDetalleFactura" class="col-lg-4">
                 <div class="col-lg-12"><strong><div ID="Div1" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Detalle de la Factura Entrante</div></strong>
                    <asp:UpdatePanel ID="UpdatePanelDetalleFactura" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridDetalleFactura" CssClass="table" OnPageIndexChanging="gridDetalleFactura_PageIndexChanging" runat="server" AllowPaging="True" PageSize="7" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                               <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                               <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                               <AlternatingRowStyle BackColor="#F8F8F8" />
                               <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                               <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                          </asp:GridView>
                     </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridDetalleFactura" EventName="RowCommand" />
                     </Triggers>
                  </asp:UpdatePanel>
               </div>     
            <div class="row" style="margin-left:5%">
                    <label for="outputTotalFacturaEntrante" class="control-label">Total de la Factura entrante:</label>      
                    <p id="outputTotalFacturaEntrante" runat="server" class="form-control-static"></p>
            </div>
            </div>


            <div class="row col-lg-3">
                <label class= "control-label">Buscar producto:</label>
                <asp:TextBox ID="textBoxAutocompleteCrearFacturaBusquedaProducto" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                <br />
                <label for="inputCantidadProducto" class= "control-label">Cantidad:</label>      
                <input id="inputCantidadProducto" class="form-control" type="text" placeholder="Ingrese una cantidad" runat="server">
                
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="inputCantidadProducto" ClientValidationFunction="changeColor" Display="Dynamic"
                ForeColor="Red" BorderStyle="Dotted" runat="server" ErrorMessage="Sólo se permiten números enteros"  Font-Bold="true" ValidationExpression="[1-9]+[0-9]*$"></asp:RegularExpressionValidator>

                <br />
                <label for="inputCostoProducto" class= "control-label">Costo Total:</label>      
                <input id="inputCostoProducto" class="form-control" type="text" placeholder="Ingrese una cantidad" runat="server">
                                
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="inputCostoProducto" ClientValidationFunction="changeColor" Display="Dynamic"
                ForeColor="Red" BorderStyle="Dotted" runat="server" ErrorMessage="Sólo se permiten números enteros"  Font-Bold="true" ValidationExpression="(([1-9]+\d*(\.\d)*)|([0]\.[1-9]+)|([1-9]+(\.[0])*))?$"></asp:RegularExpressionValidator>
                
                <br />

                <label for="inputDescuentoProducto" class= "control-label">Descuento (por ej. 1500 o 15%):</label>      
                <input id="inputDescuentoProducto" class="form-control" type="text" placeholder="Ingrese una cantidad" runat="server">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="inputDescuentoProducto" ClientValidationFunction="changeColor" Display="Dynamic"
                ForeColor="Red" BorderStyle="Dotted" runat="server" ErrorMessage="Sólo se permiten cantidades o porcentajes"  Font-Bold="true" ValidationExpression="(([0]\.[0]*[1-9]+)|([1-9]+\d*(\.[0-9]+)?\%)|([0]\.[0]*[1-9]+\%)|([1-9]+\d*(\.[0-9]+)?)|([0]*))$"></asp:RegularExpressionValidator>
                
                <br />

                <label class= "control-label">Impuesto:</label>
                <asp:DropDownList ID="dropdownlistProductoImpuesto" CssClass="form-control" Width="75%" runat="server">
                    <asp:ListItem Value="Sí">Sí</asp:ListItem>
                    <asp:ListItem Value="No">No</asp:ListItem>
                </asp:DropDownList>

                <br />

                <asp:Button ID="botonAgregarProductoFactura" runat="server" Text="Agregar a Factura" CssClass="btn btn-success-fozkr" OnClick="botonAgregarProductoFactura_Click"/>
            </div>

            <div id="bloqueGridFacturaNueva" class="col-lg-5">
                 <div class="col-lg-12"><strong><div ID="Div3" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Detalle de la Factura Consignada</div></strong>
                    <asp:UpdatePanel ID="UpdatePanelFacturaNueva" runat="server">
                        <ContentTemplate>
                                                <asp:Panel ID="PanelCrearFacturaProductos" runat="server" ScrollBars="Vertical" Height="300px">
                            <asp:GridView ID="gridFacturaNueva" CssClass="table" OnPageIndexChanging="gridFacturaNueva_PageIndexChanging" runat="server" AllowPaging="false"  BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
						        <Columns>
							        <asp:TemplateField HeaderText="Seleccionar">
								        <ItemTemplate>
									        <asp:CheckBox ID="checkBoxProductos" OnCheckedChanged="checkBoxProductos_CheckedChanged" runat="server" AutoPostBack="true"/>
								        </ItemTemplate>
							        </asp:TemplateField>
						        </Columns>                                
                                <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                               <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                               <AlternatingRowStyle BackColor="#F8F8F8" />
                               <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                               <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                          </asp:GridView>
                                                    </asp:Panel>
                     </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridFacturaNueva" EventName="RowCommand" />
                     </Triggers>
                  </asp:UpdatePanel>
               </div>  
                <br />
                <div class="row" style="margin-left:5%">
                    <asp:Button ID="botonModificarProducto" runat="server" Text="Modificar" OnClick="botonModificarProducto_Click" CssClass="btn btn-warning-fozkr"/>                 
                    <asp:Button ID="botonEliminarProducto" runat="server" Text="Eliminar" OnClick="botonEliminarProducto_Click" CssClass="btn btn-danger-fozkr"/> 
                </div>
                <br />
                <div class="row" style="margin-left:5%">
                    <label for="outputTotalFacturaNueva" class="control-label">Total de la factura:</label>      
                    <p id="outputTotalFacturaNueva" runat="server" class="form-control-static"></p>
                </div>
                <br />
                <br />
                <div class="row" style="margin-left:5%">
                    <label class= "control-label">Tipo de moneda:</label>
                    <asp:DropDownList ID="dropdownlistTipoMoneda" CssClass="form-control" Width="60%" runat="server">
                        <asp:ListItem Value="Colones">Colones</asp:ListItem>
                        <asp:ListItem Value="Dólares">Dólares</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <br />
                <div class="row" style="margin-left:5%">
                    <label class= "control-label">Método de pago:</label>
                    <asp:DropDownList ID="dropdownlistMetodoPago" CssClass="form-control" Width="60%" runat="server">
                        <asp:ListItem Value="Contado">Contado</asp:ListItem>
                        <asp:ListItem Value="Crédito">Crédito</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>
        </div>

        <br />
    </fieldset>
    <br />

    <!-- Botones de Aceptar y Cancelar -->
    <div class="col-lg-12" id="bloqueBotones">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarEntrada_ServerClick" id="botonAceptarEntrada" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Guardar</button>
                <a id="botonCancelarEntrada" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>                
            </div>
        </div>
    </div>

        <!--Modal Cancelar-->
    <div class="modal fade" id="modalCancelar" tabindex="-1" role="dialog" aria-labelledby="modalCancelarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitle"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar cancelación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea cancelar los cambios? Perdería todos los datos no guardados.
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonAceptarModalCancelar" causesvalidation="false" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick" >Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" causesvalidation="false" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
    
    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormInventario").className = "active";
        }
    </script>
    <!-- Código necesario para el autocomplete -->
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.autocomplete.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=textBoxAutocompleteCrearFacturaBusquedaProducto.ClientID%>").autocomplete('Search_CS.ashx');
        });
    </script>
</asp:Content>