<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormCategorias.aspx.cs" Inherits="ProyectoInventarioOET.FormCategorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="alert alert-fozkr-error fade in" runat="server" style="margin-left: 70%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloCategorias" runat="server"> Categorías </h2>
        <hr />
    </div>


    <!-- Cuerpo del Form -->
    <button runat="server" onserverclick="botonAgregarCategoria_ServerClick"  id="botonAgregarCategoria" class=" btn btn-info" type="button" style="float: left"><i></i> Nueva Categoria</button>
    <button runat="server" onserverclick="botonModificacionCategoria_ServerClick" id="botonModificacionCategoria" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Categoria </button>
    <button runat="server" onserverclick="botonConsultaCategoria_ServerClick" id="botonConsultaCategoria"  class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Categorias </button>


    <br />
    <br />

    <h3 id="tituloAccion"> Consulta de Categorias </h3>
     <!-- Grid de Consulta de categorias -->
      <!-- Gridview de consultar -->
    <br/>

     <div class="col-lg-12" id="bloqueGrid">

<%--        <label for="UpdatePanelPruebas" class= "control-label" > Catálogo global de categorias </label> OnRowCommand="gridViewCategorias_Seleccion" OnPageIndexChanging="gridViewCategorias_CambioPagina"--%>
        <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gridViewCategorias" CssClass="table able-responsive table-condensed" runat="server" OnRowCommand="gridViewCategorias_Seleccion" OnPageIndexChanging="gridViewCategorias_CambioPagina" AllowPaging="True" PageSize="16" BorderColor="Transparent" Visible ="false">
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
            <asp:AsyncPostBackTrigger ControlID="gridViewCategorias" EventName="RowCommand" />
         </Triggers>
      </asp:UpdatePanel>
   </div>

    <!-- Fieldset que muestra el form para agregar un nuevo categoria -->
    <div class= "row" id="bloqueFormulario" >
    <fieldset id= "FieldsetCategorias" class="fieldset">
        <br />

        <div class= "col-lg-5">

            <div class="form-group" id="camposCategoria" visible="false" runat="server">
                <label for="inputNombre" class= "control-label"> Descripcion*: </label>      
                <input type="text" id= "inputNombre" class="form-control" required runat="server" ><br>
            </div>

          

            <label class="text-danger text-center">Los campos con (*) son obligatorios</label>
        </div>

    </fieldset>
    </div>

<%--    Botones de aceptar y cancelar acción--%> 
    <div class= "row" id="bloqueBotones" visible="true" runat="server">
        <div class="text-center">
            <button id="botonAceptar" class="btn btn-success-fozkr" type="button" runat="server" onserverclick="botonAgregarCategoria_ServerClick"> Aceptar </button>
            <a id="botonCancelar" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>
        </div>
    </div>

    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormCategorias").className = "active";
        }
    </script>


    <div class="modal fade" id="modalCancelar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" runat="server"> Aceptar</button>
                    <!--onserverclick="botonCancelarModalCancelar_ServerClick" --> 
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr" onserverclick="botonCancelarModalCancelar_ServerClick" data-dismiss="modal" runat="server" >Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
</asp:Content>
