<%@ Page Title="Bodegas" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormBodegas.aspx.cs" Inherits="ProyectoInventarioOET.FormBodegas" %>
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
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="alert alert-fozkr-error fade in" runat="server" style="margin-left:70%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloBodegas" runat="server">Bodegas</h2>
        <hr />
    </div>

      <!-- Cuerpo del Form -->
    <button runat="server" onclick="showStuff('MainContent_FieldsetBodegas');" id="botonAgregarBodega" class=" btn btn-info" type="button" style="float: left"><i></i>Nueva Bodega</button>
    <button runat="server" onclick="showStuff('MainContent_FieldsetBodegas');" id="botonModificarBodega" class=" btn btn-info" type="button" style="float: left"><i></i>Modificar Bodega</button>
    <button runat="server" onclick="showStuff('MainContent_FieldsetGridBodegas');showStuff('MainContent_FieldsetCatalogoLocal');" id="botonConsultarBodega" class=" btn btn-info" type="button" style="float: left"><i></i>Consultar Bodegas</button>
    <br /><br /><br />

    <fieldset id="FieldsetGridBodegas" style="display:none" center="left" runat="server" class="fieldset">
      <!-- Gridview de consultar -->
     <div class="col-lg-12">
        <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gridViewBodegas" CssClass="table able-responsive table-condensed" OnRowCommand="gridViewBodegas_Seleccion" OnPageIndexChanging="gridViewBodegas_CambioPagina" runat="server" AllowPaging="True" PageSize="16" BorderColor="Transparent">
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
            <asp:AsyncPostBackTrigger ControlID="gridViewBodegas" EventName="RowCommand" />
         </Triggers>
      </asp:UpdatePanel>
   </div>
    <br />
    <br />
    <br />
    </fieldset>

        <!-- Fieldset que muestra el form para agregar una nueva bodega -->
    <fieldset id= "FieldsetBodegas" style="display:none" center="left" runat="server" class="fieldset">
        <legend >Ingresar datos de nueva bodega: </legend>
    
        <br />
        <br />
        <br />

        <div class= "col-md-6">

            <div class= "form-group">
                <label for="inputCodigo" class= "control-label"> Código: </label>      
                <input type="text" id= "inputCodigo" class= "form-control"><br>
            </div>

            <div class= "form-group">
                <label for="inputNombre" class= "control-label"> Nombre: </label>      
                <input type="text" id= "inputNombre" class= "form-control"><br>
            </div>

            <div class= "form-group">
                <label for="inputDescripcion" class= "control-label"> Descripción: </label>      
                <input type="text" id= "inputDescripcion" class= "form-control"><br>
            </div>
            <div class =" form-group">
                <label for="inputAnfitrion" class="control-label">Anfitrión: </label>
                <input type="text" id="inputAnfitrion" class="form-control" /><br />
                <asp:DropDownList ID="DropDownList1" runat="server">
    </asp:DropDownList>
            </div>

       

        </div>

    </fieldset>

    <fieldset id="FieldsetCatalogoLocal" style="display:none" center="left" runat="server" class="fieldset">
      <!-- Gridview de consultar -->
     <div class="col-lg-12">
        <asp:UpdatePanel ID="UpdatePanelCatalogoLocal" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gridViewCatalogoLocal" CssClass="table able-responsive table-condensed" OnRowCommand="gridViewCatalogoLocal_Seleccion" OnPageIndexChanging="gridViewCatalogoLocal_CambioPagina" runat="server" AllowPaging="True" PageSize="16" BorderColor="Transparent">
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
            <asp:AsyncPostBackTrigger ControlID="gridViewBodegas" EventName="RowCommand" />
         </Triggers>
      </asp:UpdatePanel>
   </div>
    <br />
    <br />
    <br />
    </fieldset>


    <br /><br /><br /><br />
    </asp:Content>