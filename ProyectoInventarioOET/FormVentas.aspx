<%@ Page Title="Ventas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormVentas.aspx.cs" Inherits="ProyectoInventarioOET.FormVentas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="" runat="server" Visible="false" style="margin-left:70%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text=""></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloVentas" runat="server">Factura</h2>
        <hr />
    </div>

    <!-- Botones principales -->
    <button runat="server" onserverclick="clickBotonConsultarFacturas" id="botonConsultar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Consultar Facturas</button>
    <button runat="server" onserverclick="Page_Load" id="botonCrear" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Crear Factura</button>
    <button runat="server" onserverclick="Page_Load" id="botonModificar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Modificar Factura</button>
    <button runat="server" onserverclick="Page_Load" id="botonCambioSesion" class="btn btn-info-fozkr" type="button" style="float: right" visible="false"><i></i>Cambio rápido sesión</button>
    <button runat="server" onserverclick="Page_Load" id="botonAjusteEntrada" class="btn btn-info-fozkr" type="button" style="float: right" visible="false"><i></i>Ajuste rápido inventario</button>
    <br />
    <br />

    <!-- Título de la acción que se está realizando -->
    <h3 id="tituloAccionFacturas" runat="server"></h3>
    <br />

    <!-- Panel consultar facturas -->
    <asp:Panel ID="PanelConsultarFacturas" runat="server" Visible="false">
        <div class="row" id="bloqueFormulario" runat="server">
            <div class="col-lg-12">
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Estacion:</label>    
                    <asp:DropDownList ID="DropDownListConsultaEstacion" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Bodega:</label>    
                    <asp:DropDownList ID="DropDownListConsultaBodega" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Vendedor:</label>    
                    <asp:DropDownList ID="DropDownListConsultaVendedor" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <button runat="server" onserverclick="clickBotonEjecutarConsulta" id="botonEjecutarConsulta" class="btn btn-info-fozkr" type="button" style="float:left; margin-top:9%;">
                        <i></i>Consultar
                    </button>
                </div>
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />

    <!-- Panel con el grid de consultar facturas (se mantiene aparte para poder esconder los campos de consulta grupal y mostrar los de consulta individual, sin tocar el grid) -->
    <asp:Panel ID="PanelGridConsultas" runat="server" Visible="true">
        <strong><div ID="tituloGrid" runat="server" tabindex="" class="control-label" style="text-align:center; font-size:larger; background-color: #C0C0C0;">Facturas en el sistema</div></strong>
            <asp:UpdatePanel ID="UpdatePanelFacturas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewFacturas" CssClass="table" OnRowCommand="gridViewFacturas_FilaSeleccionada" runat="server" AllowPaging="True" PageSize="5" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
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
                <asp:AsyncPostBackTrigger ControlID="gridViewFacturas" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
    </asp:Panel>

    <!-- Panel crear factura -->
    <asp:Panel ID="PanelCrearFactura" runat="server" Visible="false">
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
                <td><button type="button" id="botonAgregarProducto" class="btn btn-success-fozkr" onserverclick="Page_Load" runat="server">Agregar</button></td>
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
                    <button type="button" id="Button2" class="btn btn-danger-fozkr" href="#modalCancelarFactura" data-toggle="modal" style="float: left" runat="server">Quitar producto</button>
                    <button type="button" id="Button1" class="btn btn-warning-fozkr" onserverclick="Page_Load" style="float: left" runat="server">Aplicar descuento</button>
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
                <td>Método de pago:</td>
                <td colspan="4"><asp:DropDownList ID="dropDownListMetodoPago" class="input input-fozkr-dropdownlist" runat="server" Width="450px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="5">
                    <button type="button" id="botonCancelarFactura" class="btn btn-danger-fozkr" href="#modalCancelarFactura" data-toggle="modal" style="float: right" runat="server">Cancelar</button>
                    <button type="button" id="botonGuardarFactura" class="btn btn-success-fozkr" onserverclick="Page_Load" style="float: right" runat="server">Guardar</button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab()
        {
            document.getElementById("linkFormVentas").className = "active";
        }
    </script>
</asp:Content>
