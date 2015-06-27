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
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearPerfil">Crear perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarPerfil">Consultar perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonModificarPerfil" disabled="disabled">Modificar perfil</button>
    </fieldset>
    <!-- Fieldset de administracion de usuarios -->
    <fieldset id= "FieldsetBotonesUsuarios" class="fieldset" runat="server" visible="false">
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearUsuario">Crear usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarUsuario">Consultar usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonModificarUsuario" disabled="disabled">Modificar usuario</button>
    </fieldset>

</asp:Content>
