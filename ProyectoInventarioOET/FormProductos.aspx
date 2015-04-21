﻿<%@ Page Title="Productos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormProductos.aspx.cs" Inherits="ProyectoInventarioOET.FormProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">   
    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" class="alert alert-fozkr-error fade in" runat="server" style="margin-left: 70%; visibility:hidden">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 runat="server"> Catálogo global de productos </h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onclick="showStuff('bloqueFormulario', 'Nuevo producto');" id="botonAgregarProductos" class=" btn btn-info" type="button" style="float: left"><i></i> Nuevo Producto</button>
    <button runat="server" onclick="showStuff('bloqueFormulario', 'Modificación de producto');" id="botonModificacionProductos" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Producto </button>
    <button runat="server" onclick="showStuff('bloqueGrid', 'Consulta de productos');" id="botonConsultaProductos" class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Productos </button>


    <br />
    <br />

    <h3 id="tituloAccion"> Consulta de productos </h3>

    <!-- Fieldset que muestra el form para agregar un nuevo producto -->
    <div class= "row" id="bloqueFormulario" style="display:none">
    <fieldset id= "FieldsetProductos" runat="server" class="fieldset">
        <br />
        <br />
        <br />

        <div class= "col-lg-5">

            <div class="form-group">
                <label for="inputNombre" class= "control-label"> Nombre: </label>      
                <input type="text" id= "inputNombre" class="form-control" required" ><br>
            </div>

            <div class="form-group">
                <label for="inputDescripcion" class= "control-label"> Descripción: </label>      
                <textarea rows="4" cols="50" id="inputDescripcion" class="form-control" required> </textarea>

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
                <input type="text" id= "inputCodigo" class= "form-control" required><br>
            </div>
            
            <div class="form-group col-lg-6">
                <label for="inputCodigoBarras" class= "control-label"> Código de Barras: </label>      
                <input type="text" id= "inputCodigoBarras" class="form-control" required><br>
            </div>


            <div class="form-group col-lg-4">
                <label for="inputUnidades" class= "control-label">Unidades: </label>
                <input type="text" id="inputUnidades" class="form-control" required ><br>
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
     <label class="text-danger text-center">Los campos con (*) son obligatorios</label>
    </div>


     <!-- Grid de Consulta de productos -->
      <!-- Gridview de consultar -->
    <br/> <br/>

     <div class="col-lg-12" id="bloqueGrid">
       <div class="row">
            <div class="col-xs-9">
                <input class="form-control" type="search" placeholder="Busqueda">
                <span class="glyphicon glyphicon-search"></span>
            </div>
        </div>
        <br/> <br/>

<%--        <label for="UpdatePanelPruebas" class= "control-label" > Catálogo global de Productos </label>--%>
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



<%--    Botones de aceptar y cancelar acción--%> 
    <div class= "row" id="bloqueBotones" style="display:none;">
        <div class="text-center">
            <button id="botonAceptarProducto" class="btn btn-success-fozkr" type="button" runat="server"> Aceptar </button>
            <a id="botonCancelarProducto" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>
        </div>
    </div>

   
    <script type="text/javascript">
        function showStuff(elementoATogglear, mensaje) {
            var estado = document.getElementById(elementoATogglear).style.display;
            document.getElementById('tituloAccion').innerHTML = mensaje;
            if (elementoATogglear === 'bloqueFormulario') {
                if (estado === 'none') {
                    document.getElementById('bloqueGrid').style.display = 'none';
                    document.getElementById(elementoATogglear).style.display = 'block';
                    document.getElementById('bloqueBotones').style.display = 'block';
                } else {
                    document.getElementById(elementoATogglear).style.display = 'none';
                    document.getElementById('bloqueBotones').style.display = 'none';
                }
            } else {
                document.getElementById('bloqueBotones').style.display = 'none';
                if (estado === 'none') {
                    document.getElementById(elementoATogglear).style.display = 'block';
                    document.getElementById('bloqueFormulario').style.display = 'none';
                } else {
                    document.getElementById(elementoATogglear).style.display = 'none';
                }
            }
        } // Final de funcion 
    </script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery-form-validator/2.1.47/jquery.form-validator.min.js"> </script>
    <script> $.validate(); </script>

    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormProductos").className = "active";
        }
    </script>


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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" runat="server"> Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>


</asp:Content>
