<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormProductosLocales.aspx.cs" Inherits="ProyectoInventarioOET.FormProductosLocales" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Título del Form -->
    <div>
        <h2 id="TituloProductosLocales" runat="server">Catálogo Local</h2>
        <hr />
    </div>
    <br />
    <div class="row">
        <div class="col-lg-4">
            <label for="inputEstacion" class="control-label">Seleccione estación: </label>
            <asp:DropDownList ID="DropDownListEstacion" runat="server" CssClass="form-control" OnSelectedIndexChanged="DropDownListEstacion_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="col-lg-4">
            <label for="inputBodega" class="control-label">Seleccione bodega: </label>
            <asp:DropDownList ID="DropDownListBodega" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="col-lg-4">
            <button runat="server" onserverclick="botonConsultarBodega_ServerClick" id="botonConsultarBodega" class=" btn btn-info" type="button" style="float: left"><i></i>Consultar Bodega</button>
        </div>
     </div>
    <br /><br /><br />

    <!-- Fieldset que muestra los productos de la bodega local elegida -->
    <fieldset id= "FieldsetCatalogoLocal" center="left" runat="server" class="fieldset" visible="false">
        <!-- Gridview de productos -->
         <div class="col-lg-12">
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewCatalogoLocal" CssClass="table table-responsive table-condensed" OnRowCommand="gridViewCatalogoLocal_Seleccion" OnPageIndexChanging="gridViewCatalogoLocal_CambioPagina" runat="server" AllowPaging="True" PageSize="16" BorderColor="Transparent">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn-default disabled"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                       <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#EBEBEB" />
                       <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" />
                  </asp:GridView>
             </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridViewCatalogoLocal" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
        <br />
        <br />
        <br />
    </fieldset> 

</asp:Content>
