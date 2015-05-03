<%@ Page Title="Actividades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormActividades.aspx.cs" Inherits="ProyectoInventarioOET.FormActividades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="alert alert-fozkr-error fade in" runat="server" visible =" false" style="margin-left: 70%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloActividades" runat="server">Actividades</h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onserverclick="botonAgregarActividades_ServerClick" id="botonAgregarActividades" class=" btn btn-info" type="button" style="float: left" > Nueva Actividad</button>
    <button runat="server" onserverclick="botonModificacionActividades_ServerClick" id="botonModificacionActividades" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Actividad </button>
    <button runat="server" onserverclick="botonConsultaActividades_ServerClick"  id="botonConsultaActividades" class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Actividades </button>
    <br />
    <br />

    <h3 id="tituloAccionActividades"></h3>
    <br />

    <div class= "row" id="bloqueFormulario">
         <!-- Fieldset para Actividades -->
        <fieldset id= "FieldsetActividad" runat="server" class="fieldset">
    <asp:ValidationSummary CssClass="label label-warning" runat=server 
    HeaderText="Uno de los campos está vacío o con información inválida" />
            <div class="col-lg-4">
            <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                ControlToValidate=inputDescripcionActividad
                ErrorMessage=""> 
            </asp:RequiredFieldValidator>
                <br />
                <div class="form-group">
                    <label for="inputDescripcionActividad" class= "control-label"> Nombre*: </label>      
                    <input type="text" id= "inputDescripcionActividad" runat="server" class="form-control required">

                </div>
            </div>

            <div class="col-lg-3">
            <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                ControlToValidate=comboBoxEstadosActividades
                ErrorMessage="">
            </asp:RequiredFieldValidator>
                <br />
                <div class="form-group">
                    <label for="inputEstadoActividad" class= "control-label"> Estado*: </label>      
                    <asp:DropDownList id="comboBoxEstadosActividades" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

            <br />
            <div class="col-lg-5">
                <div class="form-group">
                    <label for="inputCodigoActividad" id="labelCodigoInterno" runat="server" class= "control-label"> Código Interno: </label>      
                    <input type="text" id= "codigoInternoActividad" runat="server" class="form-control"><br>
                </div>
            </div>

        </fieldset>

        <label id="labelTextoObligatorioActividad" runat="server" for="textoObligatorioActividad" style="margin-left:1.30%" class="text-danger text-center">Los campos con (*) son obligatorios</label>

    </div>
    <div class="col-lg-12" id="bloqueBotones">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarActividad_ServerClick" id="botonAceptarActividad" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Aceptar</button>
                <a id="botonCancelarActividad" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>                
            </div>
        </div>
    </div>
     <%--style="display:none"--%>
    <div id="bloqueGrid" class="col-lg-12">
        <fieldset id="FieldsetGridActividades" runat="server" class="fieldset">
          <!-- Gridview de consultar -->
         <div class="col-lg-12">
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewActividades" CssClass="table table-responsive table-condensed" OnRowCommand="gridViewActividades_Seleccion" OnPageIndexChanging="gridViewActividades_CambioPagina" runat="server" AllowPaging="True" PageSize="2" BorderColor="Transparent">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-info" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-info"></ControlStyle>
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
                <asp:AsyncPostBackTrigger ControlID="gridViewActividades" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
        <br />
        <br />
        <br />
        </fieldset>        
    </div>
    
        
    <!--Modal Cancelar-->
    <div class="modal fade" id="modalCancelar" tabindex="-1" role="dialog" aria-labelledby="modalCancelarLabel" aria-hidden="true">
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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick" >Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

    <!--Modal Desactivar-->
    <div class="modal fade" id="modalDesactivar" tabindex="-1" role="dialog" aria-labelledby="modalDesactivarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleDesactivar"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar desactivación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea desactivar la actividad? La modificación sería guardada permanentemente.
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonAceptarModalDesactivar" class="btn btn-success-fozkr" runat="server"  onserverclick="botonAceptarModalDesactivar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalDesactivar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showStuff(elementoATogglear, mensaje) {
            var estado = document.getElementById(elementoATogglear).style.display;
            document.getElementById('tituloAccionActividades').innerHTML = mensaje;
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

</asp:Content>
