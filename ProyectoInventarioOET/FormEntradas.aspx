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

    <!-- Botones -->
<<<<<<< HEAD
    <button runat="server" onserverclick="botonAgregarEntradas_ServerClick" causesvalidation="false" id="botonAgregarEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Nueva Entrada</button>
    <button runat="server" onserverclick="botonConsultaEntradas_ServerClick" causesvalidation="false"  id="botonConsultaEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Consultar Entradas</button>
=======
    <button runat="server" causesvalidation="false" id="botonAgregarEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Nueva Entrada</button>
    <button runat="server" causesvalidation="false"  id="botonEntradaExtraordinaria" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Nueva Entrada Extraordinaria</button>
    <button runat="server" causesvalidation="false"  id="botonConsultaEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Consultar Entradas</button>
>>>>>>> origin/master
    <br />
    <br />

    <h3 id="tituloAccionEntradas" runat="server">Seleccione una opción</h3>

<<<<<<< HEAD
    <div id="bloqueGridEntradas" class="col-lg-12">
        <fieldset id="FieldsetGridEntradas" runat="server" class="fieldset" visible="false">
         <div class="col-lg-12"><strong><div ID="Div2" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Listado de Entradas</div></strong>
            <asp:UpdatePanel ID="UpdatePanelEntradas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewEntradas" CssClass="table" OnRowCommand="gridViewEntradas_RowCommand" OnPageIndexChanging="gridViewEntradas_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                       <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#F8F8F8" />
                       <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                  </asp:GridView>
             </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridViewEntradas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
        </fieldset>        
    </div>
=======
<%--        <div class="col-lg-4">
            <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                ControlToValidate=comboBoxEmpresa
                ErrorMessage="">
            </asp:RequiredFieldValidator>
                <br />
                <div class="form-group">
                    <label for="comboBoxEmpresa" class= "control-label"> Empresa*: </label>      
                    <asp:DropDownList id="comboBoxEmpresa" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                    </asp:DropDownList>
                </div>
        </div>

            
        <div class="col-lg-4">
            <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                ControlToValidate=comboBoxEstacion
                ErrorMessage="">
            </asp:RequiredFieldValidator>
                <br />
                <div class="form-group">
                    <label for="comboBoxEstacion" class= "control-label"> Estación*: </label>      
                    <asp:DropDownList id="comboBoxEstacion" runat="server" style="max-width=75%" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                    </asp:DropDownList>
                </div>
        </div>--%>
>>>>>>> origin/master

    <fieldset id="FielsetBuscarFactura" runat="server" visible="false">
        <div class="row">
            <label class= "col-lg-12">Buscar factura:</label>
        </div>
        <div class="row">
            <div class="col-lg-7">
                <input id="barraDeBusquedaFactura" class="form-control" type="search" placeholder="Ingresa el código de la Factura" runat="server" >
            </div>
            <div class="col-lg-1">
                <asp:Button ID="botonBuscarFactura" runat="server" Text="Buscar" CssClass="btn btn-warning-fozkr" OnClick="botonBuscarFactura_Click"/>
            </div>
            <div class="col-lg-2">
                <asp:Button ID="botonMostrarFacturas" runat="server" Text="Mostrar Todas" CssClass="btn btn-warning-fozkr" OnClick="botonMostrarFacturas_Click"/>
            </div>
        </div>
    </fieldset>

    <br />
    <br />

    <div id="bloqueGridFacturas" class="col-lg-12">
<<<<<<< HEAD
        <fieldset id="FieldsetGridFacturas" runat="server" class="fieldset" visible="false">
         <div class="col-lg-12"><strong><div ID="tituloGrid" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Listado de Facturas</div></strong>
<%--            <asp:UpdatePanel ID="UpdatePanelFacturas" runat="server">
                <ContentTemplate>--%>
        <div id="popup" style="max-height:600px;overflow-y:scroll;">
                    <asp:GridView ID="gridViewFacturas" CssClass="table" OnRowCommand="gridViewFacturas_RowCommand" OnPageIndexChanging="gridViewFacturas_PageIndexChanging" runat="server" AllowPaging="false" PageSize="2" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
=======
        <fieldset id="FieldsetGridFacturas" runat="server" class="fieldset">
          <!-- Gridview de consultas -->
         <div class="col-lg-12"><strong><div ID="tituloGrid" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Listado de Facturas</div></strong>
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewFacturas" CssClass="table" OnRowCommand="gridViewFacturas_RowCommand" OnPageIndexChanging="gridViewFacturas_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
>>>>>>> origin/master
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Seleccionar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                       <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#F8F8F8" />
                       <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="Green" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                  </asp:GridView>

        </div>

<%--             </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridViewFacturas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>--%>
       </div>
        </fieldset>        
    </div>

<<<<<<< HEAD
    <br />
    <br />

    <fieldset id="FieldsetEncabezadoFactura" runat="server" class="fieldset" visible="false">
        <div class="well well-lg" id="camposEncabezadoFactura" runat="server">
            <div class="row col-lg-12">
                <div class= "form-group col-lg-3">
                    <label for="outputFactura" class= "control-label">Factura:</label>      
                    <p id="outputFactura" runat="server" class="form-control-static"></p>
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
                    <label for="outputSubtotal" class= "control-label">SubTotal:</label>      
                    <p id="outputSubtotal" runat="server" class="form-control-static"></p>
                </div>

                <div class="form-group col-lg-3">
                    <label for="outputDescuento" class= "control-label">Descuento:</label>      
                    <p id="outputDescuento" runat="server" class="form-control-static"></p>
                </div>

                <div class="form-group col-lg-3">
                    <label for="outputTotal" class= "control-label">Total:</label>      
                    <p id="outputTotal" runat="server" class="form-control-static"></p>
                </div>

                <div class="form-group col-lg-3">
                    <label for="outputImpuestos" class="control-label">Retención de Impuestos:</label>      
                    <p id="outputImpuestos" runat="server" class="form-control-static"></p>
                </div>

            </div>       
        </div>

    </fieldset>

    <br />
    <br />

    <fieldset id="FieldsetCrearFactura" runat="server" visible="false">
        <div class="row">
            <div id="bloqueGridDetalleFactura" class="col-lg-5">
                 <div class="col-lg-12"><strong><div ID="Div1" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Detalle de la Factura Entrante</div></strong>
                    <asp:UpdatePanel ID="UpdatePanelDetalleFactura" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridDetalleFactura" CssClass="table" OnPageIndexChanging="gridDetalleFactura_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
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
            </div>

            <div id="bloqueGridFacturaNueva" class="col-lg-7">
<%--                <fieldset id="fieldsetGridFacturaNueva" runat="server" class="fieldset">--%>
                 <div class="col-lg-12"><strong><div ID="Div3" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Detalle de la Factura Consignada</div></strong>
                    <asp:UpdatePanel ID="UpdatePanelFacturaNueva" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridFacturaNueva" CssClass="table" OnPageIndexChanging="gridFacturaNueva_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                               <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                               <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                               <AlternatingRowStyle BackColor="#F8F8F8" />
                               <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                               <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                          </asp:GridView>
                     </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridFacturaNueva" EventName="RowCommand" />
                     </Triggers>
                  </asp:UpdatePanel>
               </div>
<%--                </fieldset>   --%>     
            </div>
        </div>

        <br />

        <div class="row">
            <label class= "col-lg-12">Buscar producto:</label>
        </div>
        <div class="row">
            <div class="col-lg-7">
                <input id="barraDeBusquedaProducto" class="form-control" type="search" placeholder="Ingresa una palabra o código" runat="server" >
            </div>
            <div class="col-lg-2">
                <asp:Button ID="botonBuscarProducto" runat="server" Text="Buscar" CssClass="btn btn-warning-fozkr" OnClick="botonBuscar_Click"/>
            </div>
        </div>

=======
    <div class= "form-group col-lg-6">
        <label for="outputFactura" class= "control-label">Factura:</label>      
        <p id="outputFactura" runat="server" class="navbar-text">Hola</p>
    </div>
            
    <div class="form-group col-lg-6">
        <label for="outputProveeduria" class="control-label">Proveeduria:</label>      
        <p id="outputProveeduria" runat="server" class="navbar-text">Hola</p>
    </div>
>>>>>>> origin/master

    <div class="form-group col-lg-6">
        <label for="outputFechaPago" class= "control-label">Fecha de Pago:</label>      
        <p id="outputFechaPago" runat="server" class="navbar-text">Hola</p>
    </div>

<<<<<<< HEAD
    <br />

    <fieldset id="FieldsetResultadosBusqueda" runat="server" visible="false" >
        <strong><div ID="UpdatePanelResultados" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Resultados de Búsqueda</div></strong>
<%--        <asp:UpdatePanel ID="UpdatePanelResultadosBusqueda" runat="server">
            <ContentTemplate>--%>

                <asp:GridView ID="gridViewProductoBuscado" CssClass="table able-responsive table-condensed" OnRowCommand="gridViewProductoBuscado_RowCommand" OnPageIndexChanging="gridViewProductoBuscado_PageIndexChanging" runat="server" AllowPaging="true" PageSize="16" BorderColor="Transparent">
                    <Columns>
                        <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Seleccionar">
                            <ControlStyle CssClass="btn btn-default"></ControlStyle>
                        </asp:ButtonField>
                    </Columns>
                    <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                    <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                    <AlternatingRowStyle BackColor="#EBEBEB" />
                    <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                    <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" />
                </asp:GridView>

<%--            </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gridViewProductoBuscado" EventName="RowCommand" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </fieldset>

    
=======
    <div class="form-group col-lg-6">
        <label for="outputTipoPago" class= "control-label">Tipo de Pago:</label>      
        <p id="outputTipoPago" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <label for="outputPlazoPago" class= "control-label">Plazo de Pago:</label>      
        <p id="outputPlazoPago" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <label for="outputSubtotal" class= "control-label">SubTotal:</label>      
        <p id="outputSubtotal" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <label for="outputDescuento" class= "control-label">Descuento:</label>      
        <p id="outputDescuento" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <label for="outputTotal" class= "control-label">Total:</label>      
        <p id="outputTotal" runat="server" class="navbar-text">Hola</p>
    </div>

>>>>>>> origin/master

</asp:Content>
