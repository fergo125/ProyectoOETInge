<%@ Page Title="Productos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormProductos.aspx.cs" Inherits="ProyectoInventarioOET.FormProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function showStuff(id) {
            var estado = document.getElementById(id).style.display;
            if (estado === 'none') {
                document.getElementById(id).style.display = 'block'; //Color 7BC134
            } else {
                document.getElementById(id).style.display = 'none';
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
    <button runat="server" onclick="showStuff('MainContent_FieldsetProductos');" id="botonAgregarProductos" class=" btn btn-info" type="button" style="float: left"><i></i> Nuevo Producto</button>
    <button runat="server" onclick="showStuff('MainContent_FieldsetProductos');" id="botonModificacionProductos" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Producto </button>
    <button runat="server" onserverclick="Page_Load" id="botonConsultaProductos" class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Productos </button>
    <button runat="server" onserverclick="botonRedireccionCategorias_ServerClick" id="botonRedireccionCategorias" class=" btn btn-primary" type="button" style="float:right" ><i></i> Categorías</button>


    <br />
    <br />


    <!-- Fieldset que muestra el form para agregar un nuevo producto -->
    <fieldset id= "FieldsetProductos" style="display:none" runat="server" class="fieldset">
        <legend >Ingresar datos de nuevo productow: </legend>
    
        <br />
        <br />
        <br />

        <div class= "col-md-6">

            <div class= "form-group">
                <label for="inputCodigo" class= "control-label"> Código: </label>      
                <input type="text" id= "inputCodigo" class= "form-control"><br>
            </div>

       
            <div class="form-group">
                <label for="inputNombre" class= "control-label "> Nombre: </label>      
                <input type="text" id= "inputNombre" class="form-control" ><br>
            </div>


            <div class="form-group">
                <label for="inputFamilia" class= "control-label"> Catgoría: </label>     
                <asp:DropDownList id= "inpuFamilia" runat="server" class="form-control"> </asp:DropDownList> 
            </div>

            <div class="form-group">
                <label for="inputCantidad" class= "control-label"> Cantidad (total): </label>      
                <input type="text" id= "inputCantidad"  class="form-control" >
            </div>




<%--            <div class="form-group">
                <label for="inputCostoColones" class= "control-label"> Costo (colones): </label>      
                <input type="text" id= "inputCostoColones" class="form-control" ><br>
            </div>--%>

<%--            <div class="form-group">
                <label for="inputCostoDolares" class= "control-label"> Costo (dolares): </label>      
                <input type="text" id= "inputCostoDolares" class="form-control" ><br>
            </div>--%>


        </div>

        <div class="col-md-6">

            <div class="form-group">
                <label for="inputCodigoBarras" class= "control-label"> Código de Barras: </label>      
                <input type="text" id= "inputCodigoBarras" class="form-control"><br>
            </div>
            
            <div class="form-group">
                <label for="inputDescripcion" class= "control-label"> Descripción: </label>      
                <input type="text" id= "inputDescripcion" class="form-control"> <br>
            </div>

            <div class="form-group">
                <label for="inputUnidades" class= "control-label">Unidades: </label>
                <input type="text" id="inputUnidades" class="form-control"><br>
            </div>

            <div class="form-group">
                <label for="inputEstado" class= "control-label" >Estado: </label>
                <input type="text" id="inputEstado" class="form-control"><br>
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

            <div class="form-group">
                <label for="inputProveedor" class= "control-label" >Proveedor: </label>
                <asp:DropDownList ID="inputProveedor" runat="server" class="form-control"></asp:DropDownList>
            </div>

<%--            <div class="form-group">
                <label for="inputEstacion" class= "control-label" >Estación: </label>
                <asp:DropDownList ID="inputEstacion" runat="server" class="form-control"></asp:DropDownList>
            </div>--%>
        </div>

    </fieldset>



     <!-- Grid de Consulta de productos -->
    <asp:GridView ID="listaDeProductos" runat="server">
    </asp:GridView>


</asp:Content>
