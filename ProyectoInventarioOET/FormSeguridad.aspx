<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormSeguridad.aspx.cs" Inherits="ProyectoInventarioOET.FormSeguridad" %>
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
        <h2 id="TituloSeguridad" runat="server">Seguridad</h2>
        <hr />
    </div>

    <!-- Botones de acción -->
    <button runat="server" type="button" class=" btn btn-info-fozkr" id="botonPerfiles" onserverclick="botonPerfiles_ServerClick">Administrar perfiles</button>
    <button runat="server" type="button" class=" btn btn-info-fozkr" id="botonUsuarios" onserverclick="botonUsuarios_ServerClick">Administrar usuarios</button>

    <br /><br />
    <!-- Fieldset de administracion de perfiles -->
    <fieldset id= "FieldsetBotonesPerfiles" class="fieldset" runat="server" visible="false">
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarPerfil" onserverclick="botonConsultarPerfil_ServerClick">Consultar perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearPerfil">Crear perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonModificarPerfil" disabled="disabled">Modificar perfil</button>
    </fieldset>
    <!-- Fieldset de administracion de usuarios -->
    <fieldset id= "FieldsetBotonesUsuarios" class="fieldset" runat="server" visible="false">
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarUsuario" onserverclick="botonConsultarUsuario_ServerClick">Consultar usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearUsuario" onserverclick="botonCrearUsuario_ServerClick">Crear usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonAsociarPerfil" onserverclick="botonAsociarPerfil_ServerClick">Asociar usuario a perfil</button>
    </fieldset>

    <br />

    <!-- Fieldset de informacion de perfil (crear/consultar/modificar) -->
    <fieldset id="FieldsetPerfil" class="fieldset" runat="server" visible="false">

    </fieldset>

    <!-- Fieldset de informacion de usuario (crear/consultar/modificar) -->
    <fieldset id="FieldsetUsuario" class="fieldset" runat="server" visible="false">
        <br />
        <div class="col-lg-12">
            <div class="col-lg-12 row">
                <div class="col-lg-4 form-group">
                    <label for="inputNombre" class= "control-label">Nombre completo:</label>
                    <input id="inputNombre" runat="server"  type="text"  class="form-control"/>
                </div>
                <div class="col-lg-4 form-group">
                    <label for="inputUsuario" class= "control-label">Nombre de Usuario:</label>      
                    <input id="inputUsuario" runat="server" type="text" class="form-control" />
                </div>
                <div class="col-lg-4">
                    <label for="inputEstacion" class="control-label">Estación:</label>
                    <asp:DropDownList ID="DropDownListEstacion" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-lg-12 row">
                <div class="col-lg-4">
                    <div class= "form-group">
                        <label for="inputPassword" class= "control-label">Contraseña:</label>
                        <input id="inputPassword" runat="server"  type="password" class="form-control" />
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class= "form-group">
                        <label for="inputPasswordConfirm" class= "control-label">Confirmar contraseña:</label>
                        <input id="inputPasswordConfirm" runat="server"  type="password"  class="form-control"/>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="form-group">
                        <label for="DropDownListAnfitriona" class="control-label">Anfitriona:</label>
                        <asp:DropDownList ID="DropDownListAnfitriona" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-lg-12 row">
                <div class="col-lg-4">
                    <label for="inputDescripcion" class="control-label">Descripción:</label>
                    <input id="inputDescripcion" runat="server"  type="text" class="form-control"/>
                </div>
                <div class="col-lg-4">
                    <label for="DropDownListEstado" class="control-label">Estado:</label>
                    <asp:DropDownList ID="DropDownListEstado" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-lg-4">
                    <label for="inputDescuentoMaximo" class="control-label">Descuento máximo:</label>
                    <input id="inputDescuentoMaximo" runat="server" type="text" class="form-control" />
                </div>
            </div>
        </div>
    </fieldset>

    <!-- Fieldset de asociacion de usuario a perfil -->
    <fieldset id="FieldsetAsociarUsuario" class="fieldset" runat="server" visible="false">
        <!-- DropDown Lists que cargan las estaciones disponibles y las bodegas pertenecientes a la estación seleccionada -->
        <div class="row">
            <div class="col-lg-4">
                <label for="DropDownListUsuario" class="control-label">Seleccione al usuario:</label>
                <asp:DropDownList ID="DropDownListUsuario" runat="server" CssClass="form-control" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="col-lg-4">
                <label for="DropDownListPerfil" class="control-label">Seleccione perfil a asignar:</label>
                <asp:DropDownList ID="DropDownListPerfil" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
         </div>
    </fieldset>

    <!-- Fieldset de grid -->
    <fieldset id="FieldsetGrid" class="fieldset" runat="server" visible="false">

            <!-- Grid de prueba para que blopa vea como vienen los permisos-->

            <asp:GridView ID="gridPermisos" runat="server"></asp:GridView>

             <!-- Grid de prueba para que blopa vea como vienen los permisos-->

             <asp:GridView ID="gridCuentas" runat="server"></asp:GridView>

            <asp:GridView ID="gridViewGeneral" CssClass="table" runat="server" AllowPaging="false" PageSize="16" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                       <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#F8F8F8" />
                       <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="Black" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
           </asp:GridView>

    </fieldset>
    <br />


    <!-- Fieldset de botones de aceptar/cancelar -->
    <fieldset id="FieldsetBotones" class="fieldset" runat="server" visible="false">
        <div class="col-lg-12" id="bloqueBotones" runat="server">
            <div class =" row">
                <div class="text-center">
                    <button runat="server" type="button" class="btn btn-success-fozkr" id="botonAceptar">Aceptar</button>
                    <a runat="server" href="#modalCancelar" id="botonCancelarEverything" class="btn btn-danger-fozkr" role="button" data-toggle="modal">Cancelar</a>
                </div>
            </div>
        </div>
    </fieldset>

    <br />


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

</asp:Content>
