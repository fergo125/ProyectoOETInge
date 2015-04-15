<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormActividades.aspx.cs" Inherits="ProyectoInventarioOET.FormActividades" %>
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
        <h2 id="Titulos" runat="server"> Actividades </h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <button runat="server" onclick="showStuff('MainContent_FieldsetActividad');" id="botonAgregaractividades" class=" btn btn-info" type="button" style="float: left" > Nueva Actividad</button>
    <button runat="server" onclick="showStuff('MainContent_FieldsetActividad');" id="botonModificacionactividades" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Actividad </button>
    <button runat="server" onserverclick="Page_Load" id="botonConsultaactividades" class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Actividades </button>
    <%--<button runat="server" onserverclick="botonRedireccionProductos_ServerClick" id="botonRedireccionProductos" class=" btn btn-primary" type="button" style="float:right" ><i></i> Productos</button>--%>


    <br />
    <br />

         <!-- Fieldset para Actividades -->
    <fieldset id= "FieldsetActividad" style="display:none" runat="server" class="fieldset">
        <legend> Ingresar datos de nueva Actividad: </legend>
    
        <br />
        <br />
        <br />

        <div class= "col-md-4">

            <div class= "form-group">
                <label for="inputNombreActividad" class= "control-label"> Nombre: </label>      
                <input type="text" id= "inputNombreActividad" class= "form-control"><br>
            </div>
        </div>

        <div class="col-md-4">
            <div class="form-group">
                <label for="inputDescripcionActividad" class= "control-label"> Descripción: </label>      
                <input type="text" id= "inputDescripcionActividad" class="form-control"><br>
            </div>

        </div>

        <div class="col-md-4">
            <div class="form-group">
                <label for="inputEstadoActividad" class= "control-label"> Estado: </label>      
                <asp:DropDownList id="comboBoxEstadosActividades" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                    <asp:ListItem Selected="True">Ninguno</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>

        
    </fieldset>


</asp:Content>
