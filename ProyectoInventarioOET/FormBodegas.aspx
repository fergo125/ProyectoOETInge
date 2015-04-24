<%@ Page Title="Bodegas" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormBodegas.aspx.cs" Inherits="ProyectoInventarioOET.FormBodegas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="alert alert-fozkr-error fade in" runat="server" Visible="false" style="margin-left:70%;">
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
    <button runat="server" onserverclick="botonAgregarBodega_ServerClick" id="botonAgregarBodega" class=" btn btn-info" type="button" style="float: left"><i></i>Nueva Bodega</button>
    <button runat="server" onserverclick="botonModificarBodega_ServerClick"  id="botonModificarBodega" class=" btn btn-info" type="button" style="float: left"><i></i>Modificar Bodega</button>
    <button runat="server" onserverclick="botonConsultarBodega_consultarBodegas"  id="botonConsultarBodega" class=" btn btn-info" type="button" style="float: left"><i></i>Consultar Bodegas</button>
    <br />
    <br />
    
    <h3 id="tituloAccionBodegas"></h3>
    <br />

    <div class= "row" id="bloqueFormulario" style="display:block">
 
        <!-- Fieldset que muestra el form para agregar una nueva bodega -->
        <fieldset id= "FieldsetBodegas" runat="server" class="fieldset">

            <div class= "col-lg-3">
                <div class="form-group col-lg-12">
                    <label for="inputEmpresa" class="control-label">Empresa: </label>
                    <asp:DropDownList ID="comboBoxEmpresa" AutoPostBack="True" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group col-lg-12">
                    <label for="inputEstacion" class="control-label">Estación: </label>
                    <asp:DropDownList ID="comboBoxEstacion" AutoPostBack="True" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-lg-6">
                <div class= "form-group col-lg-12">
                    <label for="inputNombre" class= "control-label"> Nombre: </label>      
                    <input type="text" ID="inputNombre" runat="server" class= "form-control"><br>
                </div>
            </div>
        

            <div class="col-lg-3">
                <div class="form-group col-lg-12">
                        <label for="inputEstado" class="control-label">Estado: </label>
                    <asp:DropDownList ID="dropdownEstado" AutoPostBack="True" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>

            </div>
            <div class="col-lg-9">
            </div>
                <label for="textoObligatorioBodega" class="text-danger text-center">Los campos con (*) son obligatorios</label>
        


        </fieldset>

    </div>

    <br />




    <div id="bloqueGrids" class="col-lg-12" style="display:block">
        <fieldset id="FieldsetGridBodegas" center="left" runat="server" class="fieldset">
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
    </div>
    
        <div class="col-lg-12" id="bloqueBotones" style="display:block">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarBodega_ServerClick" id="botonAceptarBodega" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Aceptar</button>
                <a id="botonCancelarBodega" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>
                
            </div>
        </div>

    </div>



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

    <script type="text/javascript">
        function showStuff(elementoATogglear, mensaje) {
            var estado = document.getElementById(elementoATogglear).style.display;
            document.getElementById('tituloAccionBodegas').innerHTML = mensaje;
            if (elementoATogglear === 'bloqueFormulario') {
                if (estado === 'none') {
                    document.getElementById('bloqueGrids').style.display = 'none';
                    document.getElementById(elementoATogglear).style.display = 'block';
                    document.getElementById('bloqueBotones').style.display = 'block';
                } else {
                    document.getElementById(elementoATogglear).style.display = 'none';
                    document.getElementById('bloqueBotones').style.display = 'none';
                }
            } else {
                document.getElementById('bloqueBotones').style.display = 'none';
                if (estado === 'none') {
                    document.getElementById(elementoATogglear).style.display = 'block';
                    document.getElementById('bloqueFormulario').style.display = 'none';
                } else {
                    document.getElementById(elementoATogglear).style.display = 'none';
                }
            }
        } // Final de funcion 
    </script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery-form-validator/2.1.47/jquery.form-validator.min.js"> </script>
    <script> $.validate(); </script>

    <br /><br /><br /><br />
    </asp:Content>