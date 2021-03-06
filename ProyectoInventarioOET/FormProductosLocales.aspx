﻿<%@ Page Title="Productos locales" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormProductosLocales.aspx.cs" Inherits="ProyectoInventarioOET.FormProductosLocales" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css">
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
        <h2 id="TituloProductosLocales" runat="server">Catálogos de productos en bodegas</h2>
        <hr />
    </div>

    <!-- Botones de acción -->

    <button runat="server" onserverclick="botonConsultarBodega_ServerClick" id="botonConsultarBodega" class=" btn btn-info-fozkr" type="button" style="float: left" disabled="disabled" visible="false"><i class="fa fa-bars"></i> Consultar Catálogo</button>
    <button runat="server" onserverclick="botonAsociarBodega_ServerClick" id="botonAsociarBodega" class=" btn btn-info-fozkr" type="button" style="float: left" disabled="disabled" visible="false"><i class="fa fa-plus"></i> Asociar a Catálogo</button>
    <button runat="server" onserverclick="botonModificarProductoLocal_ServerClick" id="botonModificarProductoLocal" class=" btn btn-info-fozkr" type="button" style="float: left" disabled="disabled" visible="false"><i class="fa fa-wrench"></i> Modificar Producto</button>
    <br />
    <br />
    <br />
    <!-- DropDown Lists que cargan las estaciones disponibles y las bodegas pertenecientes a la estación seleccionada -->
    <div class="row">
        <div class="col-lg-4">
            <label for="inputEstacion" class="control-label">Seleccione estación:</label>
            <asp:DropDownList ID="DropDownListEstacion" runat="server" CssClass="form-control" OnSelectedIndexChanged="DropDownListEstacion_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="col-lg-4">
            <label for="inputBodega" class="control-label">Seleccione bodega:</label>
            <asp:DropDownList ID="DropDownListBodega" runat="server" CssClass="form-control" OnSelectedIndexChanged="DropDownListBodega_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
     </div>
    <br />

    <!-- Fieldset para consultar y modificar productos, aquí se cargan los datos consultados -->
    <fieldset id= "FieldsetProductos" class="fieldset" runat="server">
            <div class= "col-lg-7">
                <div class="form-group col-lg-12" >
                    <label for="inputNombre" class= "control-label">Nombre:</label>      
                    <input type="text" id= "inputNombre" class="form-control" style="max-width:100%"  runat="server" disabled="disabled">
                </div>
                <div class= "form-group col-lg-6">
                    <label for="inputCodigo" class= "control-label">Código interno:</label>      
                    <input type="text" id= "inputCodigo" class= "form-control" style= "max-width: 100%" required runat="server" disabled="disabled">
                </div>
                <div class="form-group col-lg-6">
                    <label for="inputCodigoBarras" class= "control-label">Código de Barras:</label>      
                    <input type="text" id= "inputCodigoBarras" name= "inputCodigoBarras" style= "max-width: 100%" class="form-control" required runat="server" disabled="disabled">
                </div>
                <div class="form-group col-lg-6">
                    <label for="inputCategoria" class= "control-label">Categoría:</label>     
                    <input type="text" id= "inputCategoria" runat="server" class="form-control" style="max-width:100%" AutoPostBack="true" disabled="disabled"> </input> 
                </div>
                  <div class="form-group col-lg-6">
                    <label for="inputVendible" class= "control-label">Intención de uso:</label>     
                    <input type="text" id= "inputVendible" runat="server" class="form-control" style="max-width:100%" AutoPostBack="true" disabled="disabled"> </input> 
                </div>
                <div class="form-group col-lg-6">
                    <label for="inputUnidades" class= "control-label">Unidades métricas:</label>
                    <input type="text" ID="inputUnidades" runat="server" class="form-control" style="max-width:100%" AutoPostBack="true" disabled="disabled"></input>
                </div>
                <div class="form-group col-lg-6">
                    <label for="inputEstado" class= "control-label" >Estado:</label>
                    <asp:DropDownList ID="inputEstado" runat="server" Cssclass="form-control" enabled="false"></asp:DropDownList>
                </div>
                <div class= "form-group col-lg-6">
                    <label for="inputCreador" class= "control-label">Asociador:</label>      
                    <input type="text" id= "inputCreador" class= "form-control" style= "max-width: 100%" required runat="server" disabled="disabled">
                </div>
                <div class="form-group col-lg-6">
                    <label for="inputCreado" class= "control-label">Fecha asociado:</label>      
                    <input type="text" id= "inputCreado" style= "max-width: 100%" class="form-control" required runat="server" disabled="disabled">
                </div>
                <div class= "form-group col-lg-6">
                    <label for="inputModifica" class= "control-label">Modificador:</label>      
                    <input type="text" id= "inputModifica" class= "form-control" style= "max-width: 100%" required runat="server" disabled="disabled">
                </div>
                <div class="form-group col-lg-6">
                    <label for="inputModificado" class= "control-label">Fecha modificado:</label>      
                    <input type="text" id= "inputModificado" style= "max-width: 100%" class="form-control" required runat="server" disabled="disabled">
                </div>
            </div>
            <%-- COLUMNA IZQUIERDA --%>
            <div class="col-lg-5">
                <div class="row">
                    <div class="form-group col-lg-6">
                        <label for="inputSaldo" class= "control-label">Existencia local:</label>
                        <input type="text" id="inputSaldo" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>
                     <div class="form-group col-lg-6">
                        <label for="inputImpuesto" class= "control-label">Impuesto:</label>
                         <input type="text" id="inputImpuesto" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-6">
                        <label for="inputPrecioColones" class= "control-label">Precio (colones):</label>
                        <input type="text" id= "inputPrecioColones" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>
                    <div class="form-group col-lg-6">
                        <label for="inputPrecioDolares" class= "control-label">Precio (dólares):</label>
                        <input type="text" id= "inputPrecioDolares" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled" >
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-6">
                        <label for="inputCostoColones" class= "control-label">Costo promedio (colones):</label>
                        <input type="text" id= "inputCostoColones" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>

                    <div class="form-group col-lg-6">
                        <label for="inputCostoDolares" class= "control-label">Costo promedio (dólares):</label>
                        <input type="text" id= "inputCostoDolares" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-6">
                        <label for="inputMinimo" class= "control-label">Nivel mínimo de existencia:</label>      
                        <input type="text" id= "inputMinimo" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>

                    <div class="form-group col-lg-6">
                        <label for="inputMaximo" class= "control-label">Nivel máximo de existencia:</label>      
                        <input type="text" id= "inputMaximo" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-6">
                        <label for="inputCostoUltCol" class= "control-label">Último costo compra (col):</label>      
                        <input type="text" id= "inputCostoUltCol" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>

                    <div class="form-group col-lg-6">
                        <label for="inputCostoUltDol" class= "control-label">Último costo compra (dól):</label>      
                        <input type="text" id= "inputCostoUltDol" class="form-control" runat="server" style= "max-width: 100%" disabled="disabled">
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-12">
                        <label for="inputProveedorUlt" class="control-label">Último proveedor:</label>
                        <input type="text" id="inputProveedorUlt" class="form-control" runat="server" style="max-width: 100%" disabled="disabled" />
                    </div>
                </div>
            </div>
        <br />
        </fieldset>
            <br />

    <!-- Fieldset que muestra los productos de la bodega local elegida -->
    <fieldset id= "FieldsetCatalogoLocal" center="left" runat="server" class="fieldset" visible="false">
        <!-- Gridview de productos de bodega elegida -->
         <div class="col-lg-12">
              <div class="row">
            <label class= "col-lg-12">Buscar producto:</label>
        </div>
            <div class="row">
                <div class="col-lg-11">
                    <input id="barraDeBusqueda" class="form-control" type="search" placeholder="Ingrese un nombre o código" runat="server" >
                </div>
<%--                <span class="glyphicon glyphicon-search" runat="server"></span>--%>
                <div class="col-lg-1">
                    <button ID="ButtonBuscar" runat="server" class="btn btn-info-fozkr" onserverclick="botonDeBusqueda_Click"><i class="fa fa-search"></i> Buscar</button>
                </div>

            </div>
<%--        </div>--%>
        <br/> <br/>

            <strong><div ID="TituloGrid" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Catálogo de productos en bodega</div></strong>
            <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewCatalogoLocal" CssClass="table" OnRowCommand="gridViewCatalogoLocal_Seleccion" OnPageIndexChanging="gridViewCatalogoLocal_CambioPagina" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None" AllowSorting="true" OnSorting="gridViewCatalogoLocal_Ordenado">
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
             </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="gridViewCatalogoLocal"/>
             </Triggers>
          </asp:UpdatePanel>
       </div>
        <br />
        <br />
        <br />
    </fieldset> 

    <!-- Fieldset que muestra los productos que se pueden asociar a la bodega local elegida -->
    <fieldset id="FieldsetAsociarCatalogoLocal" center="left" runat="server" class="fieldset" visible="false">
        <!-- Gridview de productos -->
         <div class="col-lg-12">
            <asp:UpdatePanel ID="UpdatePanelAsociar" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewAsociarCatalogoLocal" CssClass="table" OnPageIndexChanging="gridViewAsociarCatalogoLocal_CambioPagina" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
						<Columns>
							<asp:TemplateField HeaderText="Seleccionar">
								<ItemTemplate>
									<asp:CheckBox ID="checkBoxProductos" runat="server"/>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
					<PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#F8F8F8" />
					<SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                  </asp:GridView>
             </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridViewAsociarCatalogoLocal" EventName="RowCommand" />
             </Triggers>
          </asp:UpdatePanel>
       </div>
    </fieldset>

    <!-- Fieldset de botones que se muestran debajo del grid cuando se asocian productos -->
    <fieldset id="FieldsetBloqueBotones" center="left" runat="server" class="fieldset" visible="false">
         <div class="col-lg-12" id="bloqueBotones" runat="server">
            <div class =" row">
                <div class="text-center">
                    <a runat="server" href="#modalAsociar" id="botonAsociarProducto" class="btn btn-success-fozkr" role="button" data-toggle="modal"><i class="fa fa-check"></i> Guardar</a>
                    <a runat="server" href="#modalDesactivar" id="botonDesactivarProducto" class="btn btn-success-fozkr" role="button" data-toggle="modal"><i class="fa fa-check"></i> Guardar</a>
                    <a runat="server" href="#modalCancelar" id="botonCancelarProductoGlobal" class="btn btn-danger-fozkr" role="button" data-toggle="modal" ><i class="fa fa-trash"></i> Cancelar</a>
                </div>
            </div>
        </div>
        <br />
        <br />
        <br />
    </fieldset>

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
    <!--Modal Aceptar Desactivacion-->
    <div class="modal fade" id="modalDesactivar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleDesactivar"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar desactivación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea modificar el producto? La modificación será guardada permanentemente.
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonAceptarModalDesactivar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalDesactivar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalDesactivar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
    <!--Modal Asociar-->
    <div class="modal fade" id="modalAsociar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleAsociar"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar asociación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea asociar el/los producto/s? La asociación será guardada permanentemente.
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonAceptarModalAsociar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalAsociar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalAsociar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormBodegas").className = "active";
        }
    </script>
</asp:Content>
