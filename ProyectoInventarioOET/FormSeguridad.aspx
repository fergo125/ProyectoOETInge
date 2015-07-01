﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormSeguridad.aspx.cs" Inherits="ProyectoInventarioOET.FormSeguridad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Label para desplegar mensajes -->
    <br />
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
        <h2 id="TituloSeguridad" runat="server">Seguridad</h2>
        <hr />
    </div>

    <!-- Botones de acción -->
    <button runat="server" type="button" class=" btn btn-info-fozkr" id="botonPerfiles" onserverclick="botonPerfiles_ServerClick">Administrar perfiles</button>
    <button runat="server" type="button" class=" btn btn-info-fozkr" id="botonUsuarios" onserverclick="botonUsuarios_ServerClick">Administrar usuarios</button>

    <br /><br />
    <!-- Fieldset de administracion de perfiles -->
    <fieldset id= "FieldsetBotonesPerfiles" class="fieldset" runat="server" visible="false">
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarPerfil">Consultar perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearPerfil">Crear perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonModificarPerfil" disabled="disabled">Modificar perfil</button>
    </fieldset>
    <!-- Fieldset de administracion de usuarios -->
    <fieldset id= "FieldsetBotonesUsuarios" class="fieldset" runat="server" visible="false">
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarUsuario" onserverclick="botonConsultarUsuario_ServerClick">Consultar usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearUsuario" onserverclick="botonCrearUsuario_ServerClick">Crear usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonAsociarPerfil" onserverclick="botonAsociarPerfil_ServerClick">Asociar usuario a perfil</button>
    </fieldset>
    <br />

    <!-- Título de la acción que se está realizando -->
    <h3 ID="tituloAccionForm" runat="server"></h3>

    <!-- Fieldset de informacion de perfil (crear/consultar/modificar) -->
    <fieldset id="FieldsetPerfil" class="fieldset" runat="server" visible="true">
        <!-- Fieldset de consulta -->
        <fieldset id="FieldsetPerfilConsulta" class="fieldset" runat="server" visible="false">
        </fieldset>
        <!-- Fieldset de creación (Oscar) -->
        <br />
        <fieldset id="FieldsetPerfilCreacion" class="fieldset" runat="server" visible="true">
            <table>
                <tr>
                    <td style="width:80%; vertical-align:top;">
                        <label class="control-label">Nombre:</label>  
                        <input id="textBoxCrearPerfilNombre" type="text" class="form-control" style="width: 70%;"/>
                        <br />
                        <label class="control-label">Nivel:</label>
                        <asp:DropDownList ID="dropDownListCrearPerfilNivel" class="input input-fozkr-dropdownlist" CssClass="form-control" Width="70%" runat="server"></asp:DropDownList>
                    </td>
                    <td rowspan="100"> <!-- Árbol -->
                        <label class="control-label">Permisos:</label>  
                        <asp:Panel ID="PanelArbolPermisos" runat="server" ScrollBars="Vertical" Height="500px" Width="500px" BorderWidth="1px" style="">
                            <asp:TreeView ID="ArbolPermisos" ShowCheckBoxes="Leaf" runat="server" ImageSet="Simple" ShowLines="True">
                                <HoverNodeStyle Font-Underline="False" ForeColor="Black" />
                                <LeafNodeStyle Font-Size="Medium" />
                                <NodeStyle Font-Names="Tahoma" Font-Size="Medium" ForeColor="Black" HorizontalPadding="5px" ChildNodesPadding="5px" />
                                <ParentNodeStyle Font-Bold="False" />
                                <RootNodeStyle Font-Size="Medium" />
                                <SelectedNodeStyle Font-Underline="True" ForeColor="#336699" HorizontalPadding="5px" VerticalPadding="0px" />
                            </asp:TreeView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </fieldset>
        <!-- Fieldset de modificación -->
        <fieldset id="FieldsetPerfilModificacion" class="fieldset" runat="server" visible="false">
        </fieldset>
    </fieldset>

    <!-- Fieldset de informacion de usuario (crear/consultar/modificar) -->
    <fieldset id="FieldsetUsuario" class="fieldset" runat="server" visible="false">
        <!-- Fieldset de consulta -->
        <fieldset id="FieldsetUsuarioConsulta" class="fieldset" runat="server" visible="false">
        </fieldset>
        <!-- Fieldset de creación -->
        <fieldset id="FieldsetUsuarioCreacion" class="fieldset" runat="server" visible="false">
        </fieldset>
        <!-- Fieldset de modificación -->
        <fieldset id="FieldsetUsuarioModificacion" class="fieldset" runat="server" visible="false">
        </fieldset>
        <div class="col-lg-12">
            <div class="col-lg-4">
                <div class="form-group col-lg-12" >
                    <label for="inputUsuario" class= "control-label">Usuario:</label>      
                    <input type="text" id="inputUsuario" class="form-control" style="max-width:100%"  runat="server">
                </div>
            </div>
            <div class="col-lg-6">
                <div class="form-group col-lg-12" >
                    <label for="inputNombre" class= "control-label">Nombre de usuario:</label>      
                    <input type="text" id="inputNombre" class="form-control" style="max-width:100%"  runat="server">
                </div>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="col-lg-4">
                <div class= "form-group">
                    <label for="inputPassword" class= "control-label">Contraseña:</label>
                    <asp:TextBox runat="server" ID="inputPassword" TextMode="Password" CssClass="form-control"/>
                </div>
            </div>
            <div class="col-lg-4">
                <div class= "form-group">
                    <label for="inputPasswordConfirm" class= "control-label">Confirmar contraseña:</label>
                    <asp:TextBox runat="server" ID="inputPasswordConfirm" TextMode="Password" CssClass="form-control"/>
                </div>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="col-lg-4">
                <label for="inputEstacion" class="control-label">Seleccione estación:</label>
                <asp:DropDownList ID="DropDownListEstacion" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-lg-4">
                <label for="inputDescripcion" class="control-label">Seleccione su descripción:</label>
                <asp:DropDownList ID="DropDownListDescripcion" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
    </fieldset>

    <!-- Fieldset de asociacion de usuario a perfil -->
    <fieldset id="FieldsetAsociarUsuario" class="fieldset" runat="server" visible="false">
        <!-- DropDown Lists que cargan las estaciones disponibles y las bodegas pertenecientes a la estación seleccionada -->
        <div class="row">
            <div class="col-lg-4">
                <label for="inputUsuario" class="control-label">Seleccione al usuario:</label>
                <asp:DropDownList ID="DropDownListUsuario" runat="server" CssClass="form-control" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="col-lg-4">
                <label for="inputPerfil" class="control-label">Seleccione perfil a asignar:</label>
                <asp:DropDownList ID="DropDownListPerfil" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
         </div>
    </fieldset>

    <!-- Fieldset de grid -->
    <fieldset id="FieldsetGrid" class="fieldset" runat="server" visible="false">

            <!-- Grid de prueba para que blopa vea como vienen los permisos-->

            <asp:GridView ID="gridPermisos" runat="server"></asp:GridView>

             <!-- Grid de prueba para que blopa vea como vienen los permisos-->

             <asp:GridView ID="gridCuentas" runat="server"></asp:GridView>

            <asp:GridView ID="gridViewGeneral" CssClass="table" runat="server" AllowPaging="false" PageSize="16" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                       </Columns>
                       <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                       <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                       <AlternatingRowStyle BackColor="#F8F8F8" />
                       <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="Black" />
                       <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
           </asp:GridView>

    </fieldset>
    <br />


    <!-- Fieldset de botones de aceptar/cancelar -->
    <fieldset id="FieldsetBotones" class="fieldset" runat="server" visible="false">
        <div class="col-lg-12" id="bloqueBotones" runat="server">
            <div class =" row">
                <div class="text-center">
                    <button runat="server" type="button" class="btn btn-success-fozkr" id="botonAceptar">Aceptar</button>
                    <a runat="server" href="#modalCancelar" id="botonCancelarEverything" class="btn btn-danger-fozkr" role="button" data-toggle="modal">Cancelar</a>
        </div></div></div>
    </fieldset>

    <br />


    <!--Modal Cancelar-->
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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>

</asp:Content>
