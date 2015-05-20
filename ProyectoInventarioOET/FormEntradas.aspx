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
    <button runat="server" causesvalidation="false" id="botonAgregarEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Nueva Entrada</button>
    <button runat="server" causesvalidation="false"  id="botonEntradaExtraordinaria" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Nueva Entrada Extraordinaria</button>
    <button runat="server" causesvalidation="false"  id="botonConsultaEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Consultar Entradas</button>
    <br />
    <br />

    <h3 id="tituloAccionEntradas" runat="server">Seleccione una opción</h3>

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

    <div id="bloqueGridFacturas" class="col-lg-12">
        <fieldset id="FieldsetGridFacturas" runat="server" class="fieldset">
         <div class="col-lg-12"><strong><div ID="tituloGrid" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Listado de Facturas</div></strong>
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewFacturas" CssClass="table" OnRowCommand="gridViewFacturas_RowCommand" OnPageIndexChanging="gridViewFacturas_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
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
                <asp:AsyncPostBackTrigger ControlID="gridViewFacturas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
        </fieldset>        
    </div>

    <fieldset id="fieldsetEncabezadoFactura" runat="server" class="fieldset" visible="true">
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

    <div id="bloqueGridDetalleFactura" class="col-lg-12">
        <fieldset id="fieldsetGridDetalleFactura" runat="server" class="fieldset">
         <div class="col-lg-12"><strong><div ID="Div1" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Listado de Facturas</div></strong>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridDetalleFactura" CssClass="table" OnRowCommand="gridDetalleFactura_RowCommand" OnPageIndexChanging="gridDetalleFactura_PageIndexChanging" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
<%--                    <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>--%>
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
        </fieldset>        
    </div>

         <div class="row">
            <label class= "col-lg-12">Buscar producto:</label>
        </div>
            <div class="row">
                <div class="col-lg-9">
                    <input id="barraDeBusqueda" class="form-control" type="search" placeholder="Ingresa una palabra o código" runat="server" >
                </div>
<%--                <span class="glyphicon glyphicon-search" runat="server"></span>--%>
                <div class="col-lg-2">
                    <asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="btn btn-warning-fozkr" OnClick="botonBuscar_ServerClick"/>
                </div>
                <%--<Button ID="botonBuscar" runat="server" Text="Buscar" onserverclick="botonBuscar_ServerClick" value="sds" title="ewrwer" name="ppp" />--%>

            </div>

    

</asp:Content>
