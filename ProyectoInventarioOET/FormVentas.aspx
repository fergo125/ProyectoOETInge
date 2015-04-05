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
            <td colspan="5">Factura #<asp:Label ID="labelNumeroFactura" runat="server" Text="000001"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="5">Fecha y hora:  <%: DateTime.Now.Date %></td></tr>
        <tr>
            <td>Estación:</td>
            <td><asp:DropDownList ID="dropDownListEstaciones" class="input input-fozkr-dropdownlist" runat="server">
                <asp:ListItem>La Selva</asp:ListItem>
                <asp:ListItem>Las Cruces</asp:ListItem>
                <asp:ListItem>Palo Verde</asp:ListItem>
                <asp:ListItem>Oficinas centrales</asp:ListItem>
                </asp:DropDownList></td>
            <td></td>
            <td>Bodega:</td>
            <td><asp:DropDownList ID="dropDownListBodegas" class="input input-fozkr-dropdownlist" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Vendedor:</td>
            <td colspan="4"><asp:DropDownList ID="dropDownListVendedores" class="input input-fozkr-dropdownlist" runat="server" Width="450px"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Cliente:</td>
            <td colspan="4"><asp:DropDownList ID="dropDownList2" class="input input-fozkr-dropdownlist" runat="server" Width="450px"></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="4">Productos:</td>
            <td>Tipo de cambio: <asp:Label ID="labelTipoDeCambio" runat="server" Text="550"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="4"><asp:DropDownList ID="dropDownList1" class="input input-fozkr-dropdownlist" runat="server" Width="450px"></asp:DropDownList></td>
            <td><button type="button" id="botonAgregarProducto" class="btn btn-success" onserverclick="Page_Load" runat="server"><i>Agregar</i></button></td>
        </tr>
        <tr>
            <td colspan="2">Nombre</td>
            <td>Cantidad</td>
            <td>Precio <asp:Button ID="botonSwitchPrecios" class="btn" runat="server" Text="₡/$" /></td>
            <td>Descuento</td>
        </tr>
        <tr>
            <td colspan="2"><input id="Checkbox1" class="input input-fozkr-check" type="checkbox" />Ejemplo de nombre</td>
            <td><input id="cantidad1" type="text" class="input input-fozkr-quantity"/></td>
            <td>900</td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"><input id="Checkbox2" class="input input-fozkr-check" type="checkbox" />Ejemplo de nombre</td>
            <td><input id="cantidad2" type="text" class="input input-fozkr-quantity"/></td>
            <td>1,000</td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"><input id="Checkbox3" class="input input-fozkr-check" type="checkbox" />Ejemplo de nombre</td>
            <td><input id="cantidad3" type="text" class="input input-fozkr-quantity"/></td>
            <td>10,000</td>
            <td></td>
        </tr>
        <tr>
            <td colspan="5">
                <button type="button" id="Button2" class="btn btn-danger" href="#modalCancelarFactura" data-toggle="modal" style="float: left" runat="server"><i>Quitar producto</i></button>
                <button type="button" id="Button1" class="btn btn-info" onserverclick="Page_Load" style="float: left" runat="server"><i>Aplicar descuento</i></button>
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>Total:</td>
            <td>11,900</td>
            <td></td>
        </tr>
        <tr>
            <td colspan="5">
                <button type="button" id="botonGuardarFactura" class="btn btn-success" onserverclick="Page_Load" style="float: right" runat="server"><i>Guardar factura</i></button>
                <button type="button" id="botonCancelarFactura" class="btn btn-danger" href="#modalCancelarFactura" data-toggle="modal" style="float: right" runat="server"><i>Cancelar factura</i></button>
            </td>
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
