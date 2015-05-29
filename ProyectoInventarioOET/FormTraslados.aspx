﻿<%@ Page Title="Traslados" Language="C#" EnableEventValidation="true" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="FormTraslados.aspx.cs" Inherits="ProyectoInventarioOET.FormTraslados" %>

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
        <h2 id="TituloTraslados" runat="server">Traslados de inventario</h2>
        <hr />
    </div>

    <!-- Botones -->
    <button runat="server" onserverclick="botonRealizarTraslado_ServerClick" id="botonRealizarTraslado" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true"><i class="fa fa-pencil"></i> Nuevo Traslado</button>
    <button runat="server" onserverclick="botonModificarTraslado_ServerClick" id="botonModificarTraslado" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true"><i class="fa fa-wrench"></i> Modificar Traslado</button>
    <button runat="server" onserverclick="botonConsultarTraslado_ServerClick"  id="botonConsultarTraslado" class=" btn btn-info-fozkr" type="button" style="float: left" visible="true"><i class="fa fa-bars"></i> Consultar Traslados</button>
    <br />
    <br />

    <!-- Titulo dinamico de la pagina -->
    <h3 id="tituloAccionTraslados" runat="server">Seleccione una opción</h3>
    <br />

    <!-- Fieldset para Traslados -->
    <fieldset id= "FieldsetTraslados" runat="server" class="fieldset">
        <div class="row">
            <div class="col-lg-5">
                <label for="outputBodegaSalida" class= "control-label"> Origen: </label>      
                <input type="text" id="outputBodegaSalida" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
            </div>
            <div class="col-lg-2">
                <h3 style="text-align:center"><i class="fa fa-long-arrow-right fa-2x"></i></h3>
            </div>
            <div class="col-lg-5">
                <label for="dropDownBodegaEntrada" class= "control-label"> Destino: </label>      
                <input type="text" id="dropDownBodegaEntrada" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-5">
                <label for="outputUsuario" class= "control-label"> Usuario responsable: </label>      
                <input type="text" id="outputUsuario" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
                <a id="botonAgregar" runat="server" href="#modalAgregarProducto" class="btn btn-success-fozkr" data-toggle="modal" role="button"><i class="fa fa-plus"></i> Agregar Producto</a>
            </div>
            <div class="col-lg-2"></div>
            <div class="col-lg-5">
                <label for="outputFecha" class= "control-label"> Fecha de creacion: </label>      
                <input type="text" id="outputFecha" class="form-control" required runat="server" style="max-width:100%" disabled="disabled"><br>
            </div>
        </div>
        <div class="col-lg-12">
            <asp:TextBox ID="inputNotas" runat="server" Rows="3" Width="100%" TextMode="MultiLine" style="resize:none"></asp:TextBox>
        </div>
    </fieldset>
    <!-- Fin del fieldset-->

    <br />


    <!-- Grid de productos a transferir -->
    <div id="bloqueGridProductos" class="col-lg-12">
        <fieldset id="Fieldset2" runat="server" class="fieldset">
            <!-- Gridview -->
            <div class="col-lg-12">
                <strong><div ID="tituloGridProductos" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Productos a Transferir</div></strong>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridViewProductos" CssClass="table" OnRowCommand="gridViewProductos_Seleccion" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                            <Columns>
                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Quitar">
                                    <ControlStyle CssClass="btn btn-default"></ControlStyle>
                                </asp:ButtonField>
                                <asp:TemplateField HeaderText="Cantidad" >
                                    <ItemTemplate>
                                        <asp:TextBox ID="textTraslados" runat="server" ReadOnly="false"></asp:TextBox>
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
                        <asp:AsyncPostBackTrigger ControlID="gridViewProductos" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>         
    </div>
    <!-- Fin Grid de productos a ajustar -->

    <br />


    <!-- Botones de aceptar y cancelar-->
    <div class="col-lg-12" id="bloqueBotones">
        <div class ="row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarTraslado_ServerClick" id="botonAceptarTraslado" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i> Guardar</button>
                <a id="botonCancelarTraslado" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i> Cancelar</a>                
            </div>
        </div>
    </div>
    <!-- Fin del bloque de botones-->

    <br />
    <br />


    <!-- Grid de consultas -->
    <div id="bloqueGridTraslados" class="col-lg-12">
        <fieldset id="FieldsetGridTraslados" runat="server" class="fieldset">
            <!-- Gridview de consultar -->
            <div class="col-lg-12">
                <strong><div ID="tituloGridConsulta" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Traslados en Bodega</div></strong>
                <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridViewTraslados" CssClass="table" OnRowCommand="gridViewTraslados_Seleccion" OnPageIndexChanging="gridViewTraslados_CambioPagina" runat="server" AllowPaging="True" PageSize="15" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
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
                        <asp:AsyncPostBackTrigger ControlID="gridViewTraslados" EventName="RowCommand" />
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
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitle2"><i class="fa fa-plus fa-lg"></i> Agregar un producto</h4>
                </div>
                <div class="modal-body">
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
            </div>
        </div>
    </div>
    <!-- Fin Modal Agregar Producto -->

</asp:Content>