<%@ Page Title="Productos Globales" Language="C#" AutoEventWireup="True" MasterPageFile="~/Site.Master" CodeBehind="FormProductosGlobales.aspx.cs" Inherits="ProyectoInventarioOET.FormProductosGlobales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">   
    <br />
    <!-- Label para desplegar mensajes -->
    <div>
        <div id="mensajeAlerta" runat="server" style="margin-left: 70%; visibility:hidden">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <strong>
                <asp:Label ID="labelTipoAlerta" runat="server"></asp:Label>
            </strong>
            <asp:Label ID="labelAlerta" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Título del Form -->
    <div>
        <h2 runat="server"> Catálogo general de productos </h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onserverclick="botonAgregarProductos_ServerClick" id="botonAgregarProductos" class=" btn btn-info-fozkr" type="button" style="float: left">Nuevo Producto</button>
    <button runat="server" onserverclick="botonModificacionProductos_ServerClick" id="botonModificacionProductos" class=" btn btn-info-fozkr" type="button" style="float: left">Modificar Producto</button>
    <button runat="server" id="botonConsultaProductos" onserverclick="botonConsultaProductos_ServerClick" class=" btn btn-info-fozkr" type="button" style="float: left">Consultar Productos</button>

    <br />
    <br />

    <!-- Fieldset que muestra el form para agregar un nuevo producto -->
    <div class= "row" id="bloqueFormulario" runat="server">
<%--    <h3 id="tituloAccion"> Consulta de productos </h3>--%>
    <fieldset id= "FieldsetProductos" class="fieldset">
        <br />

        <div class= "col-lg-7">

            <div class="form-group col-lg-12" >
                <label for="inputNombre" class= "control-label">Nombre:</label>      
                <input type="text" id= "inputNombre" class="form-control" style="max-width:100%"  runat="server"><br>
            </div>
         
            <div class= "form-group col-lg-6">
                <label for="inputCodigo" class= "control-label">Código:</label>      
                <input type="text" id= "inputCodigo" class= "form-control" style= "max-width: 100%" required runat="server"><br>
            </div>
            
            <div class="form-group col-lg-6">
                <label for="inputCodigoBarras" class= "control-label">Código de Barras:</label>      
                <input type="text" id= "inputCodigoBarras" name= "inputCodigoBarras" style= "max-width: 100%" class="form-control" required runat="server"><br>
            </div>

            <div class="form-group col-lg-6">
                <label for="inputCategoria" class= "control-label">Categoría:</label>     
                <asp:DropDownList id= "inpuCategoria" runat="server" CssClass="form-control" ></asp:DropDownList> 
            </div>

              <div class="form-group col-lg-6">
                <label for="inputVendible" class= "control-label">Intención de uso:</label>     
                <asp:DropDownList id= "inputVendible" runat="server" CssClass="form-control"></asp:DropDownList> 
            </div>

            <div class="form-group col-lg-6">
                <label for="inputUnidades" class= "control-label">Unidades:</label>
                <asp:DropDownList ID="inputUnidades" runat="server" Cssclass="form-control" ></asp:DropDownList>
            </div>

            <div class="form-group col-lg-6">
                <label for="inputEstado" class= "control-label">Estado:</label>
                <asp:DropDownList ID="inputEstado" runat="server" Cssclass="form-control"></asp:DropDownList>
            </div>

            <label class="text-danger text-center">Los campos con (*) son obligatorios</label>
        </div>


<%--        COLUMNA IZQUIERDA--%>

        <div class="col-lg-5">
           
            <div class="row">
            <div class="form-group col-lg-6">
                <label for="inputSaldo" class= "control-label">Existencia global:</label>
                <input type="text" id="inputSaldo" class="form-control" runat="server" style= "max-width: 100%"><br>
            </div>

             <div class="form-group col-lg-6">
                <label for="inputImpuesto" class= "control-label">Impuesto:</label>
                 <input type="text" id= "inputImpuesto" class="form-control" style="max-width:100%" runat="server"><br>
            </div>
            </div>
            
            <div class="row">
            <div class="form-group col-lg-6">
                <label for="inputPrecioColones" class= "control-label">Precio (colones):</label>      
                <input type="text" id= "inputPrecioColones" class="form-control" runat="server" style= "max-width: 100%"><br>
            </div>

            <div class="form-group col-lg-6">
                <label for="inputPrecioDolares" class= "control-label">Precio (dólares):</label>      
                <input type="text" id= "inputPrecioDolares" class="form-control" runat="server" style= "max-width: 100%" ><br>
            </div>
            </div>

            <div class="row">
            <div class="form-group col-lg-6">
                <label for="inputCostoColones" class= "control-label">Costo (colones):</label>      
                <input type="text" id= "inputCostoColones" class="form-control" runat="server" style= "max-width: 100%"><br>
            </div>

            <div class="form-group col-lg-6">
                <label for="inputCostoDolares" class= "control-label">Costo (dólares):</label>      
                <input type="text" id= "inputCostoDolares" class="form-control" runat="server" style= "max-width: 100%"><br>
            </div>
            </div>
        </div>

    </fieldset>
    </div>


     <!-- Grid de Consulta de productos -->
      <!-- Gridview de consultar -->
    <br/>

     <div class="col-lg-12" id="bloqueGrid" runat="server">
       
         <div class="row">
            <label class= "col-lg-12">Buscar producto:</label>
        </div>
            <div class="row">
                <div class="col-lg-9">
                    <input id="barraDeBusqueda" class="form-control" type="search" placeholder="Ingresa una palabra o código" runat="server" >
                </div>
<%--                <span class="glyphicon glyphicon-search" runat="server"></span>--%>
                <div class="col-lg-2">
                    <asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="btn btn-danger-fozkr" OnClick="botonBuscar_ServerClick"/>
                </div>
                <%--<Button ID="botonBuscar" runat="server" Text="Buscar" onserverclick="botonBuscar_ServerClick" value="sds" title="ewrwer" name="ppp" />--%>

            </div>
<%--        </div>--%>
        <br/> <br/>

       <label for="UpdatePanelPruebas" class= "control-label" >Catálogo general de Productos</label>
       <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
           <ContentTemplate>
                <asp:GridView ID="gridViewProductosGlobales" CssClass="table able-responsive table-condensed" OnRowCommand="gridViewProductosGlobales_RowCommand" OnPageIndexChanging="gridViewProductosGlobales_PageIndexChanging" runat="server" AllowPaging="true" PageSize="16" BorderColor="Transparent">
                    <Columns>
                        <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                            <ControlStyle CssClass="btn btn-default"></ControlStyle>
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
            <asp:AsyncPostBackTrigger ControlID="gridViewProductosGlobales" EventName="RowCommand" />
         </Triggers>
      </asp:UpdatePanel>
   </div>



<%--    Botones de aceptar y cancelar acción--%> 
    <div class="col-lg-12" id="bloqueBotones" runat="server">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarProductoGlobal_ServerClick" id="botonAceptarProductoGlobal" class="btn btn-success-fozkr" type="button">Enviar</button>
                <a id="botonCancelarProductoGlobal" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server">Cancelar</a>
            </div>
        </div>

    </div>

   
    <script type="text/javascript">
onclick        function showStuff(elementoATogglear, mensaje) {
            //var estado = document.getElementById(elementoATogglear).style.display;
            //document.getElementById('tituloAccion').innerHTML = mensaje;
            //if (elementoATogglear === 'bloqueFormulario') {
            //    if (estado === 'none') {
            //        document.getElementById('bloqueGrid').style.display = 'none';
            //        document.getElementById(elementoATogglear).style.display = 'block';
            //        document.getElementById('bloqueBotones').style.display = 'block';
            //    } else {
            //        document.getElementById(elementoATogglear).style.display = 'none';
            //        document.getElementById('bloqueBotones').style.display = 'none';
            //    }
            //} else {
            //    document.getElementById('bloqueBotones').style.display = 'none';
            //    if (estado === 'none') {
            //        document.getElementById(elementoATogglear).style.display = 'block';
            //        document.getElementById('bloqueFormulario').style.display = 'none';
            //    } else {
            //        document.getElementById(elementoATogglear).style.display = 'none';
            //    }
            //}
        } // Final de funcion 
    </script>

    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormProductos").className = "active";
        }
    </script>

    <%--MODAL CANCELAR--%>
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
        <!--Modal Aceptar-->
    <div class="modal fade" id="modalDesactivar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="modalTitleDesactivar"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar desactivación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea desactivar la bodega? La modificación sería guardada permanentemente.
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonAceptarModalDesactivar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalDesactivar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalDesactivar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>



</asp:Content>
