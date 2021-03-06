﻿<%@ Page Title="Ajustes" EnableEventValidation="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormAjustes.aspx.cs" Inherits="ProyectoInventarioOET.FormAjustes" %>

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
        <h2 id="TituloAjustes" runat="server">Ajustes de inventario</h2>
        <hr />
    </div>

    <!-- Botones -->
    <button runat="server" onserverclick="botonConsultarAjustes_ServerClick"  id="botonConsultarAjustes" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true"><i class="fa fa-bars"></i> Consultar Ajustes</button>
    <button runat="server" onserverclick="botonRealizarAjuste_ServerClick" id="botonRealizarAjuste" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true"><i class="fa fa-plus"></i> Crear Ajuste</button> 
    <button runat="server" onserverclick="botonModificarAjuste_ServerClick" id="botonModificarAjuste" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true"><i class="fa fa-wrench"></i> Modificar Ajuste</button> <br> <br>
    <!-- Titulo dinamico de la pagina -->
    <h3 id="tituloAccionAjustes" runat="server">Seleccione una opción</h3>
    <br />
    
    <!-- Fieldset para Ajustes -->
    <fieldset id= "FieldsetAjustes" runat="server" class="fieldset">
        <div class="row">
            <div class="col-lg-6">
                <label for="outputBodega" class= "control-label" onclick="mifuncion()"> Bodega Actual: </label>      
                <input type="text" id="outputBodega" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
                <label for="outputUsuario" class= "control-label"> Usuario Responsable: </label>      
                <input type="text" id="outputUsuario" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
                
            </div>
            <div class="col-lg-6">
                <label for="dropdownTipo" class= "control-label"> Tipo de Ajuste: </label>      
                <asp:DropDownList id="dropdownTipo" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control"></asp:DropDownList><br>
                <label for="outputFecha" class= "control-label"> Fecha y hora de Creación: </label>      
                <input type="text" id="outputFecha" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-9">
                <label for="inputNotas" class= "control-label"> Notas: </label>      
                <asp:TextBox ID="inputNotas" runat="server" Rows="3" Width="100%" TextMode="MultiLine" style="resize:none" MaxLength="140"></asp:TextBox>
            </div>
            <div class = "col-lg-3 ">
                 <label for="dropdownAnular" class= "control-label"> Estado: </label>      
                <asp:DropDownList id="DropDownEstado" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                </asp:DropDownList><br>
            </div>
        </div>
            <div class="row" >
                <br><br>
                <div class="col-lg-12">
                <a id="botonAgregar" runat="server" data-target="#modalAgregarProducto" class="btn btn-success-fozkr" data-toggle="modal" role="button"><i class="fa fa-plus"></i> Agregar Producto</a> 
                </div>
            </div>
    </fieldset>
    <!-- Fin del fieldset-->

   
    <!-- Grid de productos a ajustar -->
    <div id="bloqueGridProductos" class="col-lg-12">
        <fieldset id="Fieldset2" runat="server" class="fieldset">
            <!-- Gridview -->
            <div class="col-lg-12">
                <br>
                <br>
                <strong><div ID="tituloGridProductos" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Productos a Cambiar</div></strong>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridViewProductos" CssClass="table" OnRowCreated="gridViewProductos_RowCreated" OnRowCommand="gridViewProductos_Seleccion" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                            <Columns>
                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Eliminar">
                                    <ControlStyle CssClass="btn btn-default"></ControlStyle>
                                </asp:ButtonField>
                                <asp:TemplateField HeaderText="Cantidad Nueva" >
                                    <ItemTemplate>
                                        <asp:TextBox ID="textAjustes" runat="server" ReadOnly="false"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="textAjustes" ClientValidationFunction="changeColor" Display="Dynamic"
                                                 ForeColor="Red" BorderStyle="Dotted" runat="server" ErrorMessage="Sólo se permiten números enteros o decimales"  Font-Bold="true" ValidationExpression="^\d*(\.\d+)?$"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                            <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="#F8F8F8" />
                            <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridViewProductos" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>         
    </div>
    <!-- Fin Grid de productos a ajustar -->

    <!-- Botones de aceptar y cancelar-->
    <div class="col-lg-12" id="bloqueBotones">
        <div class ="row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarAjustes_ServerClick" id="botonAceptarAjustes" class="btn btn-success-fozkr" type="button"><i class="fa fa-check"></i> Guardar</button>
                <a id="botonCancelarAjustes" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash"></i> Cancelar</a>                
            </div>
        </div>
    </div>
    <!-- Fin del bloque de botones-->


    <!-- Grid de consultas -->
    <div id="bloqueGridAjustes" class="col-lg-12">
        <fieldset id="FieldsetGridAjustes" runat="server" class="fieldset">
            <!-- Gridview de consultar -->
            <div class="row">
         <div class="col-lg-6">
                <label for="estacion" class= "control-label"> Estación: </label>      
                <input type="text" id="estacion" class="form-control" runat="server" style="max-width:100%" disabled="disabled"><br>                
            </div>
            <div class="col-lg-6">
                <label for="bodega" class= "control-label"> Bodega: </label>      
                <input type="text" id="bodega" class="form-control" runat="server" style="max-width:100%" disabled="disabled"><br>
            </div>
        </div>
            <br>
            <br>
            <div class="col-lg-12">
                <strong><div ID="tituloGridConsulta" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Ajustes en Bodega</div></strong>
                <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridViewAjustes" CssClass="table" OnRowCommand="gridViewAjustes_Seleccion" OnPageIndexChanging="gridViewAjustes_CambioPagina" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None" AllowSorting="true" OnSorting="gridViewAjustes_Sorting">
                            <Columns>
                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                    <ControlStyle CssClass="btn btn-default"></ControlStyle>
                                </asp:ButtonField>
                            </Columns>
                            <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                            <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="#F8F8F8" />
                            <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridViewAjustes" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>         
    </div>
    
    <!-- Fin Grid de consultas -->

    <!-- Modal Cancelar -->
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
                    <button type="button" id="botonAceptarModalCancelar" causesvalidation="false" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" causesvalidation="false" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
    <!-- Fin Modal Cancelar -->


    <!-- Modal Agregar Producto -->
    <div class="modal fade" id="modalAgregarProducto" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width:1000px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitle2"><i class="fa fa-plus fa-lg"></i> Agregar un producto</h4>
                </div>
                <div class="modal-body">
                    <!-- Bara de búsqueda -->
                    <div class="row">
                        <div class="col-lg-10">
                            <input id="barraDeBusqueda" style="margin-left:5%" class="form-control" type="search" placeholder="Ingresa una palabra o código" runat="server" >
                        </div>
                        <div class="col-lg-2">
                            <asp:Button ID="botonBuscar" style="margin-left:20%" runat="server" Text="Buscar" CssClass="btn btn-info-fozkr" OnClick="botonBuscar_Click"/>
                        </div>
                    </div>
                    <br />
                    <br />
                    <!-- Grid de consultas -->
                    <div id="bloqueGridAgregarProductos" class="col-lg-12">
                        <fieldset id="Fieldset3" runat="server" class="fieldset">
                            <!-- Gridview de consultar -->
                            <div class="col-lg-12">
                                <strong><div ID="Div1" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Productos en Bodega</div></strong>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gridViewAgregarProductos" CssClass="table" OnRowCommand="gridViewAgregarProductos_Seleccion" OnPageIndexChanging="gridViewAgregarProductos_CambioPagina" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Agregar">
                                                    <ControlStyle CssClass="btn btn-default"></ControlStyle>
                                                </asp:ButtonField>
                                            </Columns>
                                            <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                                            <AlternatingRowStyle BackColor="#F8F8F8" />
                                            <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gridViewAgregarProductos" EventName="RowCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </fieldset>         
                    </div>
                </div>
                 <div class="modal-footer">
                    <button type="button" id="botonCerrarModal" class="btn btn-danger-fozkr"  data-dismiss="modal">Cerrar</button>                   
                </div>
            </div>
        </div>
    </div>
    <!-- Fin Modal Agregar Producto -->

    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormInventario").className = "active";
        }
    </script>


</asp:Content>