<%@ Page Title="Productos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormProductos.aspx.cs" Inherits="ProyectoInventarioOET.FormProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function showStuff(id) {
            var estado = document.getElementById(id).style.visibility;
            if (estado === 'hidden') {
                document.getElementById(id).style.visibility = 'visible';
            } else {
                document.getElementById(id).style.visibility = 'hidden';
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
    
    <fieldset id= "FieldsetProductos" style="visibility: hidden" runat="server" class="fieldset">
        <legend>Ingresar datos de nuevo producto: </legend>
        
        <label for="inputNombre"> Nombre: </label>      
        <input type="text" id= "inputNombre" ><br>
        
        Descripción:  
        <input type="text"><br>
        Date of birth:
        <input type="text">
    </fieldset>


     <!-- Grid de Consulta de productos -->




</asp:Content>
