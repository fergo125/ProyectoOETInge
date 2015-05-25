<%@ Page Title="Ventas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormVentas.aspx.cs" Inherits="ProyectoInventarioOET.FormVentas" %>
<asp:Content ID="ContentVentas" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div ID="mensajeAlerta" class="" runat="server" Visible="false" style="margin-left:70%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text=""></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 ID="TituloVentas" runat="server">Facturación</h2>
        <hr />
    </div>

    <!-- Botones principales -->
    <button runat="server" onserverclick="clickBotonConsultarFacturas" id="botonConsultar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Consultar Facturas</button>
    <button runat="server" onserverclick="clickBotonCrearFactura" id="botonCrear" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Crear Factura</button>
    <button runat="server" onserverclick="Page_Load" id="botonModificar" class="btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Modificar Factura</button>
    <a id="botonAjusteEntrada" accesskey="A" href="#modalAjusteRapido" class="btn btn-info-fozkr" role="button" style="float: right" visible="true" data-toggle="modal" runat ="server">Ajuste rápido de entrada</a> 
    <a id="botonCambioSesion" accesskey="S" href="#modalCambioSesion" class="btn btn-info-fozkr" role="button" style="float: right" visible="true" data-toggle="modal" runat ="server">Cambio rápido de sesión</a>  
    <br />
    <br />

    <!-- Título de la acción que se está realizando -->
    <h3 ID="tituloAccionFacturas" runat="server"></h3>
    <br />

    <!-- Panel para consultar facturas -->
    <asp:Panel ID="PanelConsultarFacturas" runat="server" Visible="false">
        <div class="row" ID="bloqueFormulario" runat="server">
            <div class="col-lg-12">
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Estacion:</label>    
                    <asp:DropDownList ID="dropDownListConsultaEstacion" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Bodega:</label>    
                    <asp:DropDownList ID="dropDownListConsultaBodega" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <label for="ConsultaEstacion" class="control-label">Vendedor:</label>    
                    <asp:DropDownList ID="dropDownListConsultaVendedor" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-lg-3">
                    <button runat="server" onserverclick="clickBotonEjecutarConsulta" ID="botonEjecutarConsulta" class="btn btn-info-fozkr" type="button" style="float:left; margin-top:9%;">
                        <i></i>Consultar
                    </button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Panel para consultar la información específica de una factura después de seleccionarla en el grid -->
    <asp:Panel ID="PanelConsultarFacturaEspecifica" runat="server" Visible="false">
    </asp:Panel>

    <!-- Panel con el grid de consultar facturas (se mantiene aparte para poder esconder los campos de consulta grupal y mostrar los de consulta individual, sin tocar el grid) -->
    <br />
    <br />
    <asp:Panel ID="PanelGridConsultas" runat="server" Visible="false">
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
    <asp:Panel ID="PanelCrearFactura" runat="server" Visible="true">
        <table class="table table-fozkr">
            <tr>
                <td colspan="5">Fecha y hora:  <%: DateTime.Now.Date.ToShortDateString() %>,  <%: DateTime.Now.TimeOfDay.ToString().Substring(0,5) %></td></tr>
            <tr>
                <td>Estación:</td>
                <td><asp:DropDownList ID="dropDownListCrearFacturaEstacion" class="input input-fozkr-dropdownlist" runat="server" Enabled="false" Width="150px" CssClass="form-control"></asp:DropDownList></td>
                <td>Bodega:</td>
                <td colspan="2"><asp:DropDownList ID="dropDownListCrearFacturaBodega" class="input input-fozkr-dropdownlist" runat="server" Width="150px" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Vendedor:</td>
                <%--<td colspan="4"><asp:DropDownList ID="dropDownListCrearFacturaVendedor" class="input input-fozkr-dropdownlist" runat="server" Width="450px"></asp:DropDownList></td>--%>
                <td colspan="2"><asp:Label ID="labelCrearFacturaVendedor" runat="server"></asp:Label></td>
                <td>Tipo de cambio:</td>
                <td><asp:Label ID="labelCrearFacturaTipoCambio" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="5">Productos:</td>
            </tr>
            <tr>
                <%--<td colspan="4"><asp:DropDownList ID="dropDownListAgregarProductoFactura" class="input input-fozkr-dropdownlist" runat="server" Width="450px"></asp:DropDownList></td>--%>
                <td colspan="4"><asp:TextBox ID="labelCrearFacturaBusquedaProducto" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                </td>
                <td><button type="button" ID="botonAgregarProductoFactura" class="btn btn-success-fozkr" onserverclick="Page_Load" runat="server">Agregar</button></td>
            </tr>
            <tr>
                <td colspan="2">Nombre</td>
                <td>Cantidad</td>
                <td>Precio <asp:Button ID="botonSwitchPrecios" class="btn" runat="server" Text="₡/$" /></td>
                <td>Descuento</td>
            </tr>
            <%--TODO: considerar cambiar la lista de producto por un grid, mucho más fácil de manejar, mayor consistencia, un poco más feo--%>
                                            <!-- Para pruebas, quitar despues -->
                                            <%--<asp:Table ID="test" runat="server">
                                                <asp:TableRow>
                                                    <asp:TableCell>Prueba</asp:TableCell>
                                                    <asp:TableCell>de tabla</asp:TableCell>
                                                    <asp:TableCell>ASP</asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell ColumnSpan="3">para dinamismo</asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>--%>
            <%--<tr>
                <td colspan="2"><input ID="Checkbox1" class="input input-fozkr-check" type="checkbox" />Ejemplo de nombre</td>
                <td><input ID="cantidad1" type="text" class="input input-fozkr-quantity"/></td>
                <td>900</td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2"><input ID="Checkbox2" class="input input-fozkr-check" type="checkbox" />Ejemplo de nombre</td>
                <td><input ID="cantidad2" type="text" class="input input-fozkr-quantity"/></td>
                <td>1,000</td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2"><input ID="Checkbox3" class="input input-fozkr-check" type="checkbox" />Ejemplo de nombre</td>
                <td><input ID="cantidad3" type="text" class="input input-fozkr-quantity"/></td>
                <td>10,000</td>
                <td></td>
            </tr>--%>
            <tr>
                <td colspan="5">
                    <button type="button" ID="Button2" class="btn btn-danger-fozkr" href="#modalCancelarFactura" data-toggle="modal" style="float: left" runat="server">Quitar producto</button>
                    <button type="button" ID="Button1" class="btn btn-warning-fozkr" onserverclick="Page_Load" style="float: left" runat="server">Aplicar descuento</button>
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
                <td colspan="4"><asp:DropDownList ID="dropDownListMetodoPago" class="input input-fozkr-dropdownlist" runat="server" Width="450px" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Cliente:</td>
                <td colspan="4"><asp:DropDownList ID="dropDownList2" class="input input-fozkr-dropdownlist" runat="server" Width="450px" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="5">
                    <button type="button" ID="botonCancelarFactura" class="btn btn-danger-fozkr" href="#modalCancelarFactura" data-toggle="modal" style="float: right" runat="server">Cancelar</button>
                    <button type="button" ID="botonGuardarFactura" class="btn btn-success-fozkr" onserverclick="Page_Load" style="float: right" runat="server">Guardar</button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    
     <!--Modal Cambio de sesión-->
    <div class="modal fade" id="modalCambioSesion" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleCambioSesion"><i></i>Cambio rápido de sesión</h4>
                </div>

                <div class="modal-body">
                    Ingrese las nuevas credenciales de usuario con las que desea utilizar el sistema.
                </div>
                
                <div style="margin-left:30px">
                        <div style="margin-left:30px; margin-right:70px" class="form-group">
                            <label for="inputUsername" class= "control-label"> Nombre de usuario: </label>      
                            <input type="text" id= "inputUsername" runat="server" class="form-control" style="max-width:100%" >
                        </div>

                     <br />
                        <div style="margin-left:30px; margin-right:70px" class="form-group">
                            <label for="inputPassword" class= "control-label"> Contraseña: </label>      
                            <input type="password" id= "inputPassword" runat="server" class="form-control"  style="max-width:100%" >
                        </div>

                 </div>
 
                <div class="modal-footer">
                    <button type="button" id="botonAceptarCambioUsuario" class="btn btn-success-fozkr" onserverclick="botonAceptarCambioUsuario_ServerClick" runat="server">Aceptar</button>
                    <button type="button" id="botonCancelarCambioUsuario" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
                
            
            </div>
        </div>
    </div>

    
     <!--Modal Ajuste rápido de inventario-->
    <div class="modal fade" id="modalAjusteRapido" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleAjusteInventario"><i></i>Ajuste rápido de inventario</h4>
                </div>

                <div class="modal-body">
                    Ingrese los datos con los que desea hacer el ajuste rápido de inventario.

                <!-- Ingresar el producto y la cantidad
                    Para el producto, hay que poner un grid y una barra de busqueda
                    -->


                 </div>
 
                <div class="modal-footer">
                    <button type="button" id="botonAceptarAjusteRapido" class="btn btn-success-fozkr" onserverclick="botonAceptarCambioUsuario_ServerClick" runat="server">Aceptar</button>
                    <button type="button" id="botonCancelarAjusteRapido" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
                
            
            </div>
        </div>
    </div>

    



    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab()
        {
            document.getElementById("linkFormVentas").className = "active";
        }
    </script>

</asp:Content>
