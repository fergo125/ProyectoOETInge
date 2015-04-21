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
                <asp:GridView ID="gridViewBodegas" CssClass="table table-responsive table-condensed" OnRowCommand="gridViewBodegas_Seleccion" OnPageIndexChanging="gridViewBodegas_CambioPagina" runat="server" AllowPaging="True" PageSize="16" BorderColor="Transparent">
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

        <div class="row">
            <div class= "col-lg-4">
                <div class= "form-group">
                    <label for="inputCodigo" class= "control-label"> Código: </label>      
                    <input type="text" id= "inputCodigo" class= "form-control"><br>
                </div>
            </div>
            <div class="col-lg-4">
                <div class= "form-group">
                    <label for="inputNombre" class= "control-label"> Nombre: </label>      
                    <input type="text" id= "inputNombre" class= "form-control"><br>
                </div>
            </div>
            <div class="col-lg-4">
                <div class= "form-group">
                    <label for="inputDescripcion" class= "control-label"> Descripción: </label>      
                    <input type="text" id= "inputDescripcion" class= "form-control"><br>
                </div>

            </div>

        </div>
        


        <div class="row">


            <div class="col-lg-4">
                <div class =" form-group">
                    <label for="inputAnfitrion" class="control-label">Anfitrión: </label>
                    <input type="text" id="inputAnfitrion" class="form-control" /><br />
                </div>

            </div>

            <div class="col-lg-3">
                <div class="form-group">
                    <label for="inputEstado" class="control-label">Estado?: </label>
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>

            </div>
            <div class="col-lg-5">

                       
            </div>

        </div>
        <label for="textoObligatorioBodega" class="text-danger text-center">Los campos con (*) son obligatorios</label>
    <div class="col-lg-12">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarBodega_ServerClick" id="botonAceptarBodega" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Aceptar</button>
                <a id="botonCancelarBodega" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>
                
            </div>
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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
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
                    <h4 class="modal-title" id="modalTitle"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar desactivación</h4>
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

    <br /><br /><br /><br />
    </asp:Content>