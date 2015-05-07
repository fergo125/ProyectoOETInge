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
    <button runat="server" id="botonAgregarEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Nueva Entrada</button>
    <button runat="server" causesvalidation="false"  id="botonEntradaExtraordinaria" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Nueva Entrada Extraordinaria</button>
    <button runat="server" causesvalidation="false"  id="botonConsultaEntradas" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Consultar Entradas</button>
    <br />
    <br />

    <h3 id="tituloAccionEntradas" runat="server">Seleccione una opción</h3>

    <div id="bloqueGridFacturas" class="col-lg-12">
        <fieldset id="FieldsetGridFacturas" runat="server" class="fieldset">
          <!-- Gridview de consultas -->
         <div class="col-lg-12"><strong><div ID="tituloGrid" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Catálogo de Facturas</div></strong>
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

    <div id="bloqueGridProductos" class="col-lg-12">
        <!-- Fieldset que muestra los productos que se pueden asociar a la bodega local elegida -->
        <fieldset id="FieldsetSeleccionarProductos" center="left" runat="server" class="fieldset" visible="false">
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
