<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormCategorias.aspx.cs" Inherits="ProyectoInventarioOET.FormCategorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="alert alert-fozkr-error fade in" runat="server" style="margin-left: 70%;" visible="false">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloCategorias" runat="server">Categorías de productos</h2>
        <hr />
    </div>


    <!-- Cuerpo del Form -->
    <button runat="server" onserverclick="botonAgregarCategoria_ServerClick"  id="botonAgregarCategoria"  class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Nueva Categoría</button>
    <button runat="server" onserverclick="botonModificacionCategoria_ServerClick" id="botonModificacionCategoria" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Modificar Categoría</button>
    <button runat="server" causesvalidation="false" onserverclick="botonConsultaCategoria_ServerClick" id="botonConsultaCategoria"  class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Consultar Categorías</button>


    <br />
    <br />

    <h3 id="tituloAccion" runat="server"> Consulta de Categorias </h3>
     <!-- Grid de Consulta de categorias -->
      <!-- Gridview de consultar -->



    <!-- Fieldset que muestra el form para agregar un nuevo categoria -->
    <div class= "row" id="bloqueFormulario" >
    <fieldset id= "FieldsetCategorias" class="fieldset">
        <br />

        

            <div class="form-group" id="camposCategoria" visible="false" runat="server">
                <div class= "col-lg-6">


                 <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                 ControlToValidate=inputNombre
                 ErrorMessage=""> 
                </asp:RequiredFieldValidator>


                <label for="inputNombre" class= "control-label"> Descripcion*: </label>      
                <input type="text" id= "inputNombre" class="form-control" required runat="server" style="max-width: 100%" ><br>
                </div>


                <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                 ControlToValidate=comboBoxEstadosActividades
                 ErrorMessage=""> 
                </asp:RequiredFieldValidator>
                <div class= "col-lg-6">
                     <label for="comboBoxEstadosActividades" class= "control-label"> Estado: </label>  
                <asp:DropDownList id="comboBoxEstadosActividades" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control" >
                    </asp:DropDownList>
                </div>
            </div>
    </fieldset>
        <label id="textoObligatorioBodega" runat="server" for="textoObligatorioBodega" style="margin-left:1.30%" class="text-danger text-center">Los campos con (*) son obligatorios</label>
          <fieldset id= "Fieldset1" runat="server" class="fieldset">
            <asp:ValidationSummary Font-Size="Small" CssClass="label label-danger" runat=server 
            HeaderText="Uno de los campos está vacío o con información inválida" />


          
            <br />
            <div class="col-lg-5">
            </div>

        </fieldset>
    </div>
         <div class="col-lg-12" id="bloqueGrid" style ="padding-top:50px">
             
<%--  <label for="UpdatePanelPruebas" class= "control-label" > Catálogo global de categorias </label> OnRowCommand="gridViewCategorias_Seleccion" OnPageIndexChanging="gridViewCategorias_CambioPagina"--%>
       <strong><div ID="tituloGrid" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Catálogo de categorías</div></strong>
        <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gridViewCategorias" CssClass="table able-responsive table-condensed" runat="server" OnRowCommand="gridViewCategorias_Seleccion" OnPageIndexChanging="gridViewCategorias_CambioPagina" AllowPaging="True" PageSize="16" BorderColor="Transparent" Visible ="false">
                    <Columns>
                        <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                            <ControlStyle CssClass="btn btn-default"></ControlStyle>
                        </asp:ButtonField>
                   </Columns>
                   <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                   <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                   <AlternatingRowStyle BackColor="#F8F8F8" />
                   <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                   <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" />
              </asp:GridView>
         </ContentTemplate>
         <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gridViewCategorias" EventName="RowCommand" />
         </Triggers>
      </asp:UpdatePanel>
   </div>
<%--    Botones de aceptar y cancelar acción--%> 
    <div class= "row" id="bloqueBotones" visible="true" runat="server">
        <div class="text-center">
            <button id="botonAceptar" class="btn btn-success-fozkr" type="button" runat="server" onserverclick="botonAceptarCategoria_ServerClick" > Enviar </button>
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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" onserverclick="botonCancelarModalCancelar_ServerClick" runat="server" > Aceptar</button>
                    <!--onserverclick="botonCancelarModalCancelar_ServerClick" --> 
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr"  data-dismiss="modal" runat="server" >Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
</asp:Content>
