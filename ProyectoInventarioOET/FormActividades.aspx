<%@ Page Title="Actividades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormActividades.aspx.cs" Inherits="ProyectoInventarioOET.FormActividades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" runat="server" visible =" false" style="margin-left: 50%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server"></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloActividades" runat="server">Gestión de actividades</h2>
        <hr />
    </div>

    <!-- Botones -->
    <button runat="server" onserverclick="botonConsultaActividades_ServerClick" causesvalidation="false"  id="botonConsultaActividades" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Consultar Actividades</button>
    <button runat="server" onserverclick="botonAgregarActividades_ServerClick" id="botonAgregarActividades" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Crear Actividad</button>
    <button runat="server" onserverclick="botonModificacionActividades_ServerClick" causesvalidation="false"  id="botonModificacionActividades" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false">Modificar Actividad</button>
    <br />
    <br />

    <h3 id="tituloAccionActividades" runat="server">Seleccione una opción</h3>
    <div class= "row" id="bloqueFormulario">
         <!-- Fieldset para Actividades -->
        <fieldset id= "FieldsetActividad" runat="server" class="fieldset">

            <div class="col-lg-5">
                <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                    ControlToValidate=inputDescripcionActividad
                    ErrorMessage=""> 
                </asp:RequiredFieldValidator>
                    <br />
                    <div class="form-group">
                        <label for="inputDescripcionActividad" class= "control-label"> Nombre:* </label>      
                        <input type="text" id= "inputDescripcionActividad" runat="server" class="form-control" style="max-width:100%">
                    </div>
            </div>

            <div class="col-lg-5">
            <asp:RequiredFieldValidator CssClass="label label-danger" runat=server 
                ControlToValidate=comboBoxEstadosActividades
                ErrorMessage="">
            </asp:RequiredFieldValidator>
                <br />
                <div class="form-group">
                    <label for="inputEstadoActividad" class= "control-label"> Estado:* </label>      
                    <asp:DropDownList id="comboBoxEstadosActividades" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

            <br />

        </fieldset>

        <label id="labelTextoObligatorioActividad" runat="server" for="textoObligatorioActividad" style="margin-left:1.30%" class="text-danger text-center">Los campos con (*) son obligatorios</label>

            
        <fieldset id= "Fieldset1" runat="server" class="fieldset">
            <asp:ValidationSummary Font-Size="Small" CssClass="label label-danger" runat=server 
            HeaderText="Uno de los campos está vacío o con información inválida" />


          
            <br />
            <div class="col-lg-5">
            </div>

        </fieldset>


    </div>
    <div class="col-lg-12" id="bloqueBotones">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarActividad_ServerClick" id="botonAceptarActividad" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Guardar</button>
                <a id="botonCancelarActividad" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>                
            </div>
        </div>
    </div>
    <br />
    <br />

    <div id="bloqueGrid" class="col-lg-12">
        <fieldset id="FieldsetGridActividades" runat="server" class="fieldset">
          <!-- Gridview de consultas -->
         <div class="col-lg-12"><strong><div ID="tituloGrid" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Catálogo de actividades</div></strong>
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewActividades" CssClass="table" OnRowCommand="gridViewActividades_Seleccion" OnPageIndexChanging="gridViewActividades_CambioPagina" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
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
                <asp:AsyncPostBackTrigger ControlID="gridViewActividades" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
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
                    <button type="button" id="botonAceptarModalCancelar" causesvalidation="false" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick" >Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" causesvalidation="false" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

    <!--Modal Desactivar-->
    <div class="modal fade" id="modalDesactivar" runat="server" tabindex="-1" role="dialog" aria-labelledby="modalDesactivarLabel" aria-hidden="true">
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

    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormBodegas").className = "active";
        }
    </script>
</asp:Content>
