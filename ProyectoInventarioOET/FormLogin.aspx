<%@ Page Title="Inicio de Sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormLogin.aspx.cs" Inherits="ProyectoInventarioOET.FormLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" runat="server" style="margin-left:70%;" visible="false">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloLogin" runat="server">Iniciar sesión</h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <div class="row">
        <div class="col-md-12">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h3>Utilice una cuenta de la OET para iniciar sesión.</h3>
                    <br />
                      <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <!-- Campos de ingreso de datos -->
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-4 control-label">Nombre de usuario:</asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control"/>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="El campo de nombre de usuario es obligatorio." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-4 control-label">Contraseña:</asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control"/>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="El campo de contraseña es obligatorio." />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-5 col-md-10">
                            <asp:Button runat="server" OnClick="iniciarSesion" Text="Iniciar sesión" CssClass="btn btn-success-fozkr" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

</asp:Content>
