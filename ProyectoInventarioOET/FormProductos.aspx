﻿<%@ Page Title="Productos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormProductos.aspx.cs" Inherits="ProyectoInventarioOET.FormProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function showStuff(elementoAMostrar, elementoAEsconder) {
            var estado = document.getElementById(elementoAMostrar).style.display;
            if (estado === 'none') {
                document.getElementById(elementoAMostrar).style.display = 'block'; //Color 7BC134
            } else {
                document.getElementById(elementoAMostrar).style.display = 'none';
            }
            document.getElementById(elementoAEsconder).style.display = 'none'; //Color 7BC134

            if (elementoAMostrar === 'bloqueFormulario') {
                document.getElementById('bloqueBotones').style.display = 'block';
            }
            if (elementoAEsconder === 'bloqueFormulario') {
                document.getElementById('bloqueBotones').style.display = 'none';
            }
        }

    </script>

    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="alert alert-fozkr-error fade in" runat="server" style="margin-left: 70%;">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 id="TituloProductos" runat="server"> Productos </h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onclick="showStuff('bloqueFormulario', 'bloqueGrid' );" id="botonAgregarProductos" class=" btn btn-info" type="button" style="float: left"><i></i> Nuevo Producto</button>
    <button runat="server" onclick="showStuff('bloqueFormulario', 'bloqueGrid');" id="botonModificacionProductos" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Producto </button>
    <button runat="server" onclick="showStuff('bloqueGrid', 'bloqueFormulario');" id="botonConsultaProductos" class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Productos </button>
    <button runat="server" onserverclick="botonRedireccionCategorias_ServerClick" id="botonRedireccionCategorias" class=" btn btn-primary" type="button" style="float:right" ><i></i> Categorías</button>


    <br />
    <br />


    <!-- Fieldset que muestra el form para agregar un nuevo producto -->
    <div class= "row" id="bloqueFormulario" style="display:none">
    <fieldset id= "FieldsetProductos" runat="server" class="fieldset">
<%--        <legend >Ingresar datos de nuevo producto: </legend>--%>
    
        <br />
        <br />
        <br />

        <div class= "col-lg-5">

            <div class="form-group">
                <label for="inputNombre" class= "control-label "> Nombre: </label>      
                <input type="text" id= "inputNombre" class="form-control" ><br>
            </div>

            <div class="form-group">
                <label for="inputDescripcion" class= "control-label"> Descripción: </label>      
                <textarea rows="4" cols="50" id="inputDescripcion" class="form-control"> </textarea>
            </div>
          
            <div class="form-group">
                <label for="inputFamilia" class= "control-label"> Categoría: </label>     
                <asp:DropDownList id= "inpuFamilia" runat="server" class="form-control"> </asp:DropDownList> 
            </div>

<%--            <div class="form-group">
                <label for="inputCantidad" class= "control-label"> Cantidad (total): </label>      
                <input type="text" id= "inputCantidad"  class="form-control" >
            </div>--%>

<%--            <div class="form-group">
                <label for="inputCostoColones" class= "control-label"> Costo (colones): </label>      
                <input type="text" id= "inputCostoColones" class="form-control" ><br>
            </div>--%>

<%--            <div class="form-group">
                <label for="inputCostoDolares" class= "control-label"> Costo (dolares): </label>      
                <input type="text" id= "inputCostoDolares" class="form-control" ><br>
            </div>--%>


        </div>

        <div class="col-lg-7">

            <div class= "form-group col-lg-6">
                <label for="inputCodigo" class= "control-label"> Código: </label>      
                <input type="text" id= "inputCodigo" class= "form-control"><br>
            </div>
            
            <div class="form-group col-lg-6">
                <label for="inputCodigoBarras" class= "control-label"> Código de Barras: </label>      
                <input type="text" id= "inputCodigoBarras" class="form-control"><br>
            </div>


            <div class="form-group col-lg-4">
                <label for="inputUnidades" class= "control-label">Unidades: </label>
                <input type="text" id="inputUnidades" class="form-control"><br>
            </div>

            <div class="form-group col-lg-4">
                <label for="inputEstado" class= "control-label" >Estado: </label>
                <asp:DropDownList ID="inputEstado" runat="server" class="form-control"></asp:DropDownList>
            </div>

<%--            <div class="form-group">
                <label for="inputMinimo" class= "control-label"> Cantidad mínima: </label>
                <input type="text" id="inputMinimo" class="form-control"><br>
            </div>

            <div class="form-group">
                <label for="inputMaximo" class= "control-label"> Cantidad máxima: </label>
                <input type="text" id="inputMaximo" class="form-control"><br>
            </div>--%>

<%--            <div class="form-group">
                <label for="inputSaldo" class= "control-label" >Saldo: </label>
                <input type="text" id="inputSaldo" class="form-control"><br>
            </div>--%>

<%--            <div class="form-group">
                <label for="inputImpuesto" class= "control-label" >Impuesto: </label>
                <asp:DropDownList ID="inputImpuesto" runat="server" class="form-control"></asp:DropDownList>
            </div>--%>

<%--            <div class="form-group">
                <label for="inputProveedor" class= "control-label" >Proveedor: </label>
                <asp:DropDownList ID="inputProveedor" runat="server" class="form-control"></asp:DropDownList>
            </div>--%>

<%--            <div class="form-group">
                <label for="inputEstacion" class= "control-label" >Estación: </label>
                <asp:DropDownList ID="inputEstacion" runat="server" class="form-control"></asp:DropDownList>
            </div>--%>
        </div>

    </fieldset>
    </div>


     <!-- Grid de Consulta de productos -->
      <!-- Gridview de consultar -->

     <div class="col-lg-12" id="bloqueGrid">
        <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gridViewBodegas" CssClass="table able-responsive table-condensed" OnRowCommand="gridViewBodegas_Seleccion" OnPageIndexChanging="gridViewBodegas_CambioPagina" runat="server" AllowPaging="True" PageSize="16" BorderColor="Transparent">
                    <Columns>
                        <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn-default" CommandName="Select" Text="Consultar">
                            <ControlStyle CssClass="btn-default disabled"></ControlStyle>
                        </asp:ButtonField>
                   </Columns>
                   <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                   <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                   <AlternatingRowStyle BackColor="#EBEBEB" />
                   <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                   <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" />
              </asp:GridView>
         </ContentTemplate>
         <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gridViewBodegas" EventName="RowCommand" />
         </Triggers>
      </asp:UpdatePanel>
   </div>

    <div class= "row" id="bloqueBotones" style="display:none">
        <button id= "botonCancelar" class="btn btn-danger" style="float: right" runat="server"> Cancelar </button>
        <button id="botonAceptar" class="btn btn-info"  style="float: right" runat="server"> Aceptar </button>
    </div>


</asp:Content>
