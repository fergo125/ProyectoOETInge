<%@ Page Title="Bodegas" EnableEventValidation="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormBodegas.aspx.cs" Inherits="ProyectoInventarioOET.FormBodegas" %>
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
        <h2 id="TituloBodegas" runat="server">Gestión de bodegas</h2>
        <hr />
    </div>

    <!-- Botones -->
    <button runat="server" onserverclick="botonAgregarBodega_ServerClick" id="botonAgregarBodega" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Nueva Bodega</button>
    <button runat="server" onserverclick="botonModificarBodega_ServerClick"  id="botonModificarBodega" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Modificar Bodega</button>
    <button runat="server" onserverclick="botonConsultarBodega_consultarBodegas" causesvalidation="false" id="botonConsultarBodega" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Consultar Bodegas</button>
    <br />
    <br />
    
    
    <h3 id="tituloAccionBodegas" runat="server">Seleccione una opción</h3>
      <!-- Fieldset para Bodegas -->


        <fieldset id= "FieldsetBodegas" runat="server" class="fieldset">


                  <div class="col-lg-4">
                <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                    ControlToValidate=inputNombre
                    ErrorMessage=""> 
                </asp:RequiredFieldValidator>
                    <br />
                    <div class="form-group">
                        <label for="inputNombre" class= "control-label"> Nombre*: </label>      
                        <input type="text" id= "inputNombre" runat="server" class="form-control" style="max-width:100%">
                    </div>
            </div>



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




            <div class="col-lg-4">
            <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                ControlToValidate=comboBoxIntencion
                ErrorMessage="">
            </asp:RequiredFieldValidator>
                <br />
                <div class="form-group">
                    <label for="comboBoxIntencion" class= "control-label"> Intención de uso*: </label>      
                    <asp:DropDownList id="comboBoxIntencion" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>


   

            <div class="col-lg-4">
            <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                ControlToValidate=dropdownEstado
                ErrorMessage="">
            </asp:RequiredFieldValidator>
                <br />
                <div class="form-group">
                    <label for="dropdownEstado" class= "control-label"> Estado*: </label>      
                    <asp:DropDownList id="dropdownEstado" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

            <br />
            <div class="col-lg-4">
            </div>

        </fieldset>

        <label id="textoObligatorioBodega" runat="server" for="textoObligatorioBodega" style="margin-left:1.30%" class="text-danger text-center">Los campos con (*) son obligatorios</label>

    <br />
      
            
    
        <fieldset id= "Fieldset1" runat="server" class="fieldset">
            <asp:ValidationSummary Font-Size="Small" CssClass="label label-danger" runat=server 
            HeaderText="Uno de los campos está vacío o con información inválida" />


          
            <br />
            <div class="col-lg-5">
            </div>

        </fieldset>
    <!-- Fin del fieldset-->


    <!-- Botones de aceptar y cancelar-->

    <div class="col-lg-12" id="bloqueBotones">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarBodega_ServerClick" id="botonAceptarBodega" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Enviar</button>
                <a id="botonCancelarBodega" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>                
            </div>
        </div>
    </div>
    <br />
    <br />
    
    <!-- Fin del bloque de botones-->



    
    <!--Grid ddonde se despliega lo de consultar-->
      <div id="bloqueGrid" class="col-lg-12">
        <fieldset id="FieldsetGridBodegas" runat="server" class="fieldset">
          <!-- Gridview de consultar -->
         <div class="col-lg-12">
            <strong><div ID="tituloGrid" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Catálogo de bodegas</div></strong>
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewBodegas" CssClass="table" OnRowCommand="gridViewBodegas_Seleccion" OnPageIndexChanging="gridViewBodegas_CambioPagina" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
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
                <asp:AsyncPostBackTrigger ControlID="gridViewBodegas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
        </fieldset>         
    </div>
    
    <!--Final del grid donde se despliega lo de consultar-->


    <!--Modal Cancelar-->
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

    <!--Modal Aceptar-->
    <div class="modal fade" id="modalDesactivar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleDesactivar"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar desactivación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea desactivar la bodega? La modificación sería guardada permanentemente.
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonAceptarModalDesactivar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalDesactivar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalDesactivar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>


    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery-form-validator/2.1.47/jquery.form-validator.min.js"> </script>
    <script> $.validate(); </script>
</asp:Content>