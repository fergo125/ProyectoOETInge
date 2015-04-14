<%@ Page Title="Bodegas" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormBodegas.aspx.cs" Inherits="ProyectoInventarioOET.FormBodegas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
    <button runat="server" id="botonConsultarBodega" class=" btn btn-info" type="button" style="float: right"><i></i>Consultar Bodega</button>
    <button runat="server" id="botonModificarBodega" class=" btn btn-info" type="button" style="float: right"><i></i>Modificar Bodega</button>
    <button runat="server" id="botonAgregarBodega" class=" btn btn-info" type="button" style="float: right"><i></i>Nueva Bodega</button>
    <br />


    <br /><br /><br /><br />
    </asp:Content>