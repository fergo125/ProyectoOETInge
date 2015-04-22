<%@ Page Title="Actividades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormActividades.aspx.cs" Inherits="ProyectoInventarioOET.FormActividades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
    <button runat="server" onclick="showStuff('MainContent_FieldsetActividad');" id="botonAgregarActividades" class=" btn btn-info" type="button" style="float: left" > Nueva Actividad</button>
    <button runat="server" onclick="showStuff('MainContent_FieldsetActividad');" id="botonModificacionActividades" class=" btn btn-info" type="button" style="float: left"><i></i> Modificar Actividad </button>
    <button runat="server" onserverclick="Page_Load" id="botonConsultaActividades" class=" btn btn-info" type="button" style="float: left"><i></i>Consulta de Actividades </button>
    <%--<button runat="server" onserverclick="botonRedireccionProductos_ServerClick" id="botonRedireccionProductos" class=" btn btn-primary" type="button" style="float:right" ><i></i> Productos</button>--%>


    <br />
    <br />
    <div class= "row" id="bloqueFormulario" style="display:none">
         <!-- Fieldset para Actividades -->
        <fieldset id= "FieldsetActividad" style="display:none" runat="server" class="fieldset">
            <legend> Ingresar datos de una nueva Actividad: </legend>
    
            <br />
            <br />
            <br />
            <div class="row">

                <div class="col-lg-4">
                    <div class="form-group">
                        <label for="inputDescripcionActividad" class= "control-label"> Descripción: </label>      
                        <input type="text" id= "inputDescripcionActividad" class="form-control"><br>
                    </div>

                </div>

                <div class="col-lg-4">
                    <div class="form-group">
                        <label for="inputEstadoActividad" class= "control-label"> Estado: </label>      
                        <asp:DropDownList id="comboBoxEstadosActividades" runat="server" DataSourceID="" DataTextField="" DataValueField="" CssClass="form-control">
                            <asp:ListItem Selected="True">Ninguno</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>


            </div>

        </fieldset>
        <label for="textoObligatorioActividad" class="text-danger text-center">Los campos con (*) son obligatorios</label>

    </div>
    <div class="col-lg-12" id="bloqueBotones" style="display:none">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarActividad_ServerClick" id="botonAceptarActividad" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Aceptar</button>
                <a id="botonCancelarActividad" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>
                
            </div>
        </div>

    </div>

        
    <!--Modal Cancelar-->
    <div class="modal fade" id="modalCancelar" tabindex="-1" role="dialog" aria-labelledby="modalCancelarLabel" aria-hidden="true">
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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick" >Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showStuff(elementoATogglear, mensaje) {
            var estado = document.getElementById(elementoATogglear).style.display;
            document.getElementById('tituloAccionBodegas').innerHTML = mensaje;
            if (elementoATogglear === 'bloqueFormulario') {
                if (estado === 'none') {
                    document.getElementById('bloqueGrids').style.display = 'none';
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


</asp:Content>
