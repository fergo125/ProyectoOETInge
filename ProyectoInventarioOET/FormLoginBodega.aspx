<%@ Page Title="" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="FormLoginBodega.aspx.cs" Inherits="ProyectoInventarioOET.FormBodegaLocal" %>
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
        <h2 id="TituloBodegaLocal" runat="server">Cambiar sesión</h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-offset-8 col-md-2">
                        <a runat="server" href="#modalCambiarContrasena" id="botonCambiarContrasena" class="btn btn-info-fozkr" role="button" data-toggle="modal">Cambiar contraseña</a>
                    </div>
                    <div class="col-md-2">
                        <asp:Button runat="server" ID="CerrarSesion" Text="Cerrar sesión" CssClass="btn btn-danger-fozkr" OnClick="CerrarSesion_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class =" form-horizontal">
            <h3>Seleccione una bodega local de trabajo</h3> <br />
        </div>
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" CssClass="col-md-4 control-label">Selecciona una bodega:</asp:Label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownListBodega" runat="server" CssClass="form-control" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="form-group">
                    <div class="col-md-offset-5 col-md-10">
                        <asp:Button runat="server" ID="EnviarBodegaLocal" Text="Aceptar" CssClass="btn btn-success-fozkr" OnClick="EnviarBodegaLocal_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <br />
    <br />
    <br />
    <!--Modal Cambiar Contraseña-->
    <div class="modal fade" id="modalCambiarContrasena" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleCambio"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Cambio de Contraseña</h4>
                </div>
                <div class="modal-body">
                    Introduzca la información solicitada:<br /><br />
                    <div class= "form-group">
                        <label for="inputActual" class= "control-label">Contraseña actual:</label>
                        <asp:TextBox runat="server" ID="inputActual" TextMode="Password" CssClass="form-control"/>
                    </div>
                    <div class= "form-group">
                        <label for="inputNueva" class= "control-label">Contraseña nueva:</label>
                        <asp:TextBox runat="server" ID="inputNueva" TextMode="Password" CssClass="form-control"/>
                    </div>
                    <div class= "form-group">
                        <label for="inputNuevaConfirmacion" class= "control-label">Confirmar contraseña nueva:</label>      
                        <asp:TextBox runat="server" ID="inputNuevaConfirmacion" TextMode="Password" CssClass="form-control"/>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonAceptarModalCambiar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCambiar_Click">Aceptar</button>
                    <button type="button" id="botonCancelarModalCambiar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
</asp:Content>