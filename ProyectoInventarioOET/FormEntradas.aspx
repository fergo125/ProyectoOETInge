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
    <button runat="server" id="botonAgregarEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Nueva Entrada</button>
    <button runat="server" causesvalidation="false"  id="botonEntradaExtraordinaria" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Nueva Entrada Extraordinaria</button>
    <button runat="server" causesvalidation="false"  id="botonConsultaEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true">Consultar Entradas</button>
    <br />
    <br />

    <h3 id="tituloAccionEntradas" runat="server">Seleccione una opción</h3>

        <div class="col-lg-4">
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
        </div>

    <div id="bloqueGridFacturas" class="col-lg-12">
        <fieldset id="FieldsetGridFacturas" runat="server" class="fieldset">
          <!-- Gridview de consultas -->
         <div class="col-lg-12"><strong><div ID="tituloGrid" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Catálogo de Facturas</div></strong>
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
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

    <div class= "form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputFactura" class= "control-label">Factura:</label>      
        <p id="outputFactura" runat="server" class="navbar-text">Hola</p>
    </div>
            
    <div class="form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputProveeduria" class="control-label">Proveeduria:</label>      
        <p id="outputProveeduria" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputFechaPago" class= "control-label">Fecha de Pago:</label>      
        <p id="outputFechaPago" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputTipoPago" class= "control-label">Tipo de Pago:</label>      
        <p id="outputTipoPago" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputPlazoPago" class= "control-label">Plazo de Pago:</label>      
        <p id="outputPlazoPago" runat="server" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputSubtotal" class= "control-label">SubTotal:</label>      
        <p id="outputSubtotal" runat="outputSubtotal" class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputDescuento" class= "control-label">Descuento:</label>      
        <p class="navbar-text">Hola</p>
    </div>

    <div class="form-group col-lg-6">
        <asp:RequiredFieldValidator CssClass="label label-danger" runat=server ControlToValidate=inputCodigo ErrorMessage=""> </asp:RequiredFieldValidator>
        <label for="outputTotal" class= "control-label">Total:</label>      
        <p class="navbar-text">Hola</p>
    </div>

    <div id="bloqueGridProductos" class="col-lg-12">
        <!-- Fieldset que muestra los productos que se pueden asociar a la bodega local elegida -->
        <fieldset id="FieldsetSeleccionarProductos" center="left" runat="server" class="fieldset" visible="true">
            <!-- Gridview de productos -->
             <div class="col-lg-12">
                <asp:UpdatePanel ID="UpdatePanelAsociar" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridViewSeleccionarProductos" CssClass="table" OnPageIndexChanging="gridViewSeleccionarProductos_PageIndexChanging" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
						    <Columns>
							    <asp:TemplateField HeaderText="Seleccionar">
								    <ItemTemplate>
									    <asp:CheckBox ID="checkBoxProductos" runat="server"/>
								    </ItemTemplate>
							    </asp:TemplateField>
						    </Columns>
                           <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
					    <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                           <AlternatingRowStyle BackColor="#F8F8F8" />
					    <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                           <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                      </asp:GridView>
                 </ContentTemplate>
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gridViewSeleccionarProductos" EventName="RowCommand" />
                 </Triggers>
              </asp:UpdatePanel>
           </div>
        </fieldset>


    </div>
</asp:Content>
