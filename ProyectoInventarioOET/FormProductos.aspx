<%@ Page Title="Productos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormProductos.aspx.cs" Inherits="ProyectoInventarioOET.FormProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function showStuff(id) {
            var estado = document.getElementById(id).style.display;
            if (estado === 'none') {
                document.getElementById(id).style.display = 'block';
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
        <h2 id="TituloVentas" runat="server">Inventario</h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onclick="showStuff('MainContent_FieldsetProductos');" id="botonAgregarProductos" class=" btn btn-info" type="button" style="float: right"><i></i>Nuevo Producto</button>
    <button runat="server" onclick="showStuff('MainContent_FieldsetProductos');" id="botonModificacionProductos" class=" btn btn-info" type="button" style="float: right"><i></i>Modificar Producto </button>
    <button runat="server" onserverclick="Page_Load" id="botonConsultaProductos" class=" btn btn-info" type="button" style="float: right"><i></i>Consulta de Productos </button>


    <br />
    <br />


    <!-- Fieldset que muestra el form para agregar un nuevo producto -->
    
    <fieldset id= "FieldsetProductos" style="display:none" runat="server" class="fieldset row">
        <legend>Ingresar datos de nuevo producto: </legend>
        
        <div class= "form-inline  col-md-6">

            <label for="inputCodigo"> Código: </label>      
            <input type="text" id= "inputCodigo" class="form-control input-sm col-xs-3"><br>
            <br />
            <label for="inputCodigoBarras"> Código de Barras: </label>      
            <input type="text" id= "inputCodigoBarras" ><br>
            <br />
            <label for="inputNombre"> Nombre: </label>      
            <input type="text" id= "inputNombre" ><br>
        
            <label for="inputDescripcion"> Descripción: </label>      
            <input type="text" id= "inputDescripcion" ><br>

            <label for="inputCantidad"> Cantidad: </label>      
            <input type="text" id= "inputCantidad"  class=""><br>

            <label for="inputFamilia"> Familia: </label>     
            <asp:DropDownList id= "inpuFamilia" runat="server"></asp:DropDownList> 
            <br />
            <label for="inputCostoColones"> Costo (colones): </label>      
            <input type="text" id= "inputCostoColones" ><br>
            <br />
            <label for="inputCostoDolares"> Costo (dolares): </label>      
            <input type="text" id= "inputCostoDolares" ><br>
            <br />
        </div>
        
        <div class="form-group  col-md-6">
            <label for="inputCosto"> Unidades: </label>      
            <input type="text" id= "inputCosto" ><br>
            <br />
             <label for="inputEstado"> Estado: </label>      
            <input type="text" id= "inputEstado" ><br>
            <br />
            <label for="inputMinimo"> Cantidad mínima: </label>      
            <input type="text" id= "inputMinimo" ><br>
            <br />
             <label for="inputMaximo"> Cantidad máxima: </label>      
            <input type="text" id= "inputMaximo" ><br>
            <br />
             <label for="inputSaldo"> Saldo: </label>      
            <input type="text" id= "inputSaldo" ><br>
            <br />
            <label for="inputImpuesto"> Impuesto: </label>      
            <asp:DropDownList id= "inputImpuesto" runat="server"></asp:DropDownList> 
            <br />
            <label for="inputProveedor"> Proveedor: </label>     
            <asp:DropDownList id= "inputProveedor" runat="server"></asp:DropDownList> 
            <br />
            <label for="inputEstacion"> Estación: </label>     
            <asp:DropDownList id= "inputEstacion" runat="server"></asp:DropDownList> 
            <br />
        <div>

    </fieldset>


     <!-- Grid de Consulta de productos -->
    
    <asp:Button ID="Button1" runat="server" Text="Button" />

</asp:Content>
