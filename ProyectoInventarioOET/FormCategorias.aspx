<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormCategorias.aspx.cs" Inherits="ProyectoInventarioOET.FormCategorias" %>
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
        <h2 id="TituloCategorias" runat="server"> Categorías </h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onclick="showStuff('MainContent_FieldsetCategoria');" id="botonAgregarCategorias" class=" btn btn-info" type="button" style="float: left" > Nueva Categoría</button>
    <button runat="server" onclick="showStuff('MainContent_FieldsetCategoria');" id="botonModificacionCategorias" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Categoría </button>
    <button runat="server" onserverclick="Page_Load" id="botonConsultaCategorias" class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Categorías </button>
    <button runat="server" onserverclick="botonRedireccionProductos_ServerClick" id="botonRedireccionProductos" class=" btn btn-primary" type="button" style="float:right" ><i></i> Productos</button>


    <br />
    <br />


         <!-- Fieldset para Categorías -->
    <fieldset id= "FieldsetCategoria" style="display:none" runat="server" class="fieldset">
        <legend> Ingresar datos de nueva Categoría de productos: </legend>
    
        <br />
        <br />
        <br />

        <div class= "col-md-6">

            <div class= "form-group">
                <label for="inputNombreCategoria" class= "control-label"> Nombre: </label>      
                <input type="text" id= "inputNombreCategoria" class= "form-control"><br>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label for="inputDescripcionCategoria" class= "control-label"> Descripción: </label>      
                <input type="text" id= "inputDescripcionCategoria" class="form-control"><br>
            </div>

        </div>
    </fieldset>
</asp:Content>
