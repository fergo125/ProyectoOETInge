<%@ Page Title="Ajustes" EnableEventValidation="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FromAjustes.aspx.cs" Inherits="ProyectoInventarioOET.FromAjustes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
        <h2 id="TituloAjustes" runat="server">Ajustes de inventario</h2>
        <hr />
    </div>

    <!-- Botones -->
    <button runat="server" onserverclick="botonRealizarAjuste_ServerClick" id="botonRealizarAjuste" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Realizar Ajuste</button>
    <button runat="server" onserverclick="botonConsultarAjustes_ServerClick"  id="botonConsultarAjustes" class=" btn btn-info-fozkr" type="button" style="float: left" visible="false"><i></i>Consultar Ajustes</button>
    <br />
    <br />

    <h3 id="tituloAccionAjustes" runat="server">Seleccione una opción</h3>
     
    
    <!-- Fieldset para Bodegas -->
    <fieldset id= "FieldsetBodegas" runat="server" class="fieldset">
    
    </fieldset>
    <!-- Fin del fieldset-->


    <!-- Botones de aceptar y cancelar-->
    <div class="col-lg-12" id="bloqueBotones">
        <div class =" row">
            <div class="text-center">
                <button runat="server" onserverclick="botonAceptarAjustes_ServerClick" id="botonAceptarAjustes" class="btn btn-success-fozkr" type="button"><i class="fa fa-pencil-square-o"></i>Enviar</button>
                <a id="botonCancelarAjustes" href="#modalCancelar" class="btn btn-danger-fozkr" role="button" data-toggle="modal" runat ="server"><i class="fa fa-trash-o fa-lg"></i>Cancelar</a>                
            </div>
        </div>
    </div>
    <!-- Fin del bloque de botones-->

    <br />
    <br />


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

</asp:Content>