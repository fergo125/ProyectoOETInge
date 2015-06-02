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
                    <div class="col-md-offset-10 col-md-2">
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

</asp:Content>