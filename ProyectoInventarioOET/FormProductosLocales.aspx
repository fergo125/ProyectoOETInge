<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormProductosLocales.aspx.cs" Inherits="ProyectoInventarioOET.FormProductosLocales" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function showStuff(id) {
            var estado = document.getElementById(id).style.display;
            if (estado === 'none') {
                document.getElementById(id).style.display = 'block'; //Color 7BC134
            } else {
                document.getElementById(id).style.display = 'none';
            }
        }
    </script>

    <br />
    <!-- Título del Form -->
    <div>
        <h2 id="TituloProductosLocales" runat="server">Catálogo Local</h2>
        <hr />
    </div>
    <br />
    <div class="row">
        <div class="col-lg-4">
            <label for="inputBodega" class="control-label">Seleccione bodega: </label>
            <asp:DropDownList ID="DropDownListBodega" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
     </div>
    <br /><br />

    <!-- Fieldset que muestra los productos de la bodega local elegida -->
    <fieldset id= "FieldsetCatalogoLocal" style="display:block" center="left" runat="server" class="fieldset" aria-hidden="true">
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
    <fieldset>
          <div class="form-group col-lg-4">
                <label for="inputIDProducto" class= "control-label"> Costo (dolares): </label>      
                <input type="text" id= "inputIDProducto" class="form-control" ><br>
            </div>
          <div class="form-group col-lg-5">
                <label for="inputNombreProducto" class= "control-label"> Nombre:</label>      
                <input type="text" id= "inputNombreProducto" class="form-control" ><br>
            </div>
            <div class= "form-group col-lg-6">
                <label for="inputCostoColones" class= "control-label"> Costo Colones: </label>      
                <input type="text" id= "inputCostoColones" class= "form-control" required runat="server"><br>
            </div>
            
            <div class="form-group col-lg-7">
                <label for="inputCostoDolares" class= "control-label"> Costo Dolares: </label>      
                <input type="text" id= "inputCostoDolares" name= "inputCodigoBarras" class="form-control" required runat="server"><br>
            </div>

            <div class="form-group col-lg-8">
                <label for="inputSaldo" class= "control-label" >Saldo: </label>
                <input type="text" id= "inputSaldo" name= "inputCodigoBarras" class="form-control" required runat="server"><br>
             
            </div>
            <div class="form-group col-lg-9">
                <label for="inputCantidadMinima" class= "control-label" >Cantidad Minima: </label>

                <asp:DropDownList ID="inputCantidadMinima" runat="server" class="form-control"></asp:DropDownList>
            </div>

            <div class="form-group col-lg-10">
                <label for="inputCantidadMaxima" class= "control-label"> Cantidad Maxima</label>      
                <input type="text" id= "inputCantidadMaxima" class="form-control" ><br>
            </div>

          
        </fieldset>
    <!-- costo colones,dolares,saldo, cantidad min. cantidad max--> 

</asp:Content>
