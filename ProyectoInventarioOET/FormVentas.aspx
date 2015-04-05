<%@ Page Title="Ventas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormVentas.aspx.cs" Inherits="ProyectoInventarioOET.FormVentas" %>
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
        <h2 id="TituloVentas" runat="server">Facturación</h2>
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onserverclick="Page_Load" id="botonCambioRapidoSesion" class=" btn btn-info" type="button" style="float: right"><i></i>Cambio rápido sesión</button>
    <button runat="server" onserverclick="Page_Load" id="botonAjusteRapidoEntrada" class=" btn btn-info" type="button" style="float: right"><i></i>Ajuste rápido inventario</button>
    <br />
    <br />
    <!-- Tabla factura -->
    <table class="table table-fozkr">
        <tr>
            <td></td>
            <td></td>
            <td>Factura #000000</td>
            <td></td>
            <td>Fecha: 00/00/0000</td>
        </tr>
        <tr>
            <td>Estación:</td>
            <td></td>
            <td></td>
            <td>Bodega:</td>
            <td></td>
        </tr>
        <tr>
            <td>Vendedor:</td>
            <td colspan="4"></td>
        </tr>
        <tr>
            <td>Productos:</td>
            <td></td>
            <td></td>
            <td></td>
            <td>Tipo de cambio:</td>
        </tr>
        <tr>
            <td colspan="2">Nombre</td>
            <td>Cantidad</td>
            <td>Precio</td>
            <td></td>
        </tr>
    </table>
    
    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab()
        {
            document.getElementById("linkFormVentas").className = "active";
        }
    </script>
</asp:Content>
