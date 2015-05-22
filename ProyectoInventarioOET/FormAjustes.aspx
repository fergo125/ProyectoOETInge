<%@ Page Title="Ajustes" EnableEventValidation="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormAjustes.aspx.cs" Inherits="ProyectoInventarioOET.FormAjustes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />

    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" runat="server" Visible="false" style="margin-left:50%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server"></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloAjustes" runat="server">Ajustes de inventario</h2>
        <hr />
    </div>

    <!-- Botones -->
    <button runat="server" onserverclick="botonRealizarAjuste_ServerClick" id="botonRealizarAjuste" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Realizar Ajuste</button>
    <button runat="server" onserverclick="botonConsultarAjustes_ServerClick"  id="botonConsultarAjustes" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Consultar Ajustes</button>
    <br />
    <br />

    <!-- Titulo dinamico de la pagina -->
    <h3 id="tituloAccionAjustes" runat="server">Seleccione una opción</h3>
     
    
    <!-- Fieldset para Bodegas -->
    <fieldset id= "FieldsetAjustes" runat="server" class="fieldset">
        <div class="col-lg-4">
            <label for="outputBodega" class= "control-label"> Bodega actual: </label>      
            <input type="text" id="outputBodega" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
            <button runat="server" onserverclick="botonAceptarAjustes_ServerClick" id="botonAgregar" class="btn btn-success-fozkr" type="button"><i class="fa fa-plus"></i>Agregar Producto</button>
        </div>
        <div class="col-lg-4">
            <label for="dropdownTipo" class= "control-label"> Tipo de Ajuste*: </label>      
            <asp:DropDownList id="dropdownTipo" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control"></asp:DropDownList>
        </div>
    </fieldset>
    <!-- Fin del fieldset-->


    <!-- Botones de aceptar y cancelar-->
    <div class="col-lg-12" id="bloqueBotones">
        <div class ="row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarAjustes_ServerClick" id="botonAceptarAjustes" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Enviar</button>
                <a id="botonCancelarAjustes" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>                
            </div>
        </div>
    </div>
    <!-- Fin del bloque de botones-->

    <br />
    <br />


    <!-- Grid de consultas -->
      <div id="bloqueGrid" class="col-lg-12">
        <fieldset id="FieldsetGridAjustes" runat="server" class="fieldset">
          <!-- Gridview de consultar -->
         <div class="col-lg-12">
            <strong><div ID="tituloGrid" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Ajustes en Bodega</div></strong>
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewAjustes" CssClass="table" OnRowCommand="gridViewAjustes_Seleccion" OnPageIndexChanging="gridViewAjustes_CambioPagina" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
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
                <asp:AsyncPostBackTrigger ControlID="gridViewAjustes" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
        </fieldset>         
    </div>
    
    <!-- Fin Grid de consultas -->

    <!-- Modal Cancelar -->
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
                    <button type="button" id="botonAceptarModalCancelar" causesvalidation="false" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" causesvalidation="false" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
    <!-- Fin Modal Cancelar -->

</asp:Content>