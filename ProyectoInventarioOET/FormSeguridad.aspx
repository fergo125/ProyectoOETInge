﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormSeguridad.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="ProyectoInventarioOET.FormSeguridad" %>
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
    <button runat="server" type="button" class=" btn btn-info-fozkr" id="botonUsuarios" causesvalidation="false" onserverclick="botonUsuarios_ServerClick">Administrar usuarios</button>
    <button runat="server" type="button" class=" btn btn-info-fozkr" id="botonPerfiles" causesvalidation="false"  onserverclick="botonPerfiles_ServerClick">Administrar perfiles</button>

    <br /><br />
    <!-- Fieldset de administracion de perfiles -->
    <fieldset id= "FieldsetBotonesPerfiles" class="fieldset" runat="server" visible="false">
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarPerfil" causesvalidation="false" onserverclick="botonConsultarPerfil_ServerClick">Consultar perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearPerfil" causesvalidation="false" onserverclick="botonCrearPerfil_ServerClick">Crear perfil</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonModificarPerfil" causesvalidation="false" disabled="disabled">Modificar perfil</button>
    </fieldset>
    <!-- Fieldset de administracion de usuarios -->
    <fieldset id= "FieldsetBotonesUsuarios" class="fieldset" runat="server" visible="false">
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonConsultarUsuario" causesvalidation="false" onserverclick="botonConsultarUsuario_ServerClick">Consultar usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonCrearUsuario" causesvalidation="false" onserverclick="botonCrearUsuario_ServerClick">Crear usuario</button>
        <button runat="server" type="button" class="btn btn-info-fozkr" id="botonModificarUsuario" causesvalidation="false" onserverclick="botonModificarUsuario_ServerClick" visible="false" >Modificar usuario</button>
    </fieldset>
    <br />

    <!-- Título de la acción que se está realizando -->
    <h3 ID="tituloAccionForm" runat="server"></h3>


    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="La contraseña es un espacio necesario." 
                            ControlToValidate="inputPassword"
                            Font-Bold="true"
                            ForeColor="Red"
                            Display="Dynamic"
                            CssClass="ValidationError">
                            </asp:RequiredFieldValidator>

                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                            ControlToValidate="inputPasswordConfirm"
                            CssClass="ValidationError"
                            ControlToCompare="inputPassword"
                            ClientValidationFunction="changeColor" 
                            ForeColor="Red"
                            Display="Dynamic"
                            Font-Bold="true"
                            ErrorMessage="La contraseña no coincide con lo ingresado en la confirmación."/>

    
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ErrorMessage="Su contraseña debe tener una mayúscula, una minúscula, y un número. Debe ser de al menos 8 caracteres." 
                            ControlToValidate="inputPassword"
                            CssClass="ValidationError"
                            ClientValidationFunction="changeColor" 
                            ForeColor="Red"
                            Font-Bold="true"
                            Display="Dynamic"
                            ValidationExpression="(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"></asp:RegularExpressionValidator>


    <!-- Fieldset de informacion de perfil (crear/consultar/modificar) -->
    <fieldset id="FieldsetPerfil" class="fieldset" runat="server" visible="false">
        <!-- Fieldset de creación (Oscar) -->
        <br />
        <fieldset id="FieldsetPerfilCreacion" class="fieldset" runat="server" visible="false">
            <table>
                <tr>
                    <td style="width:80%; vertical-align:top;">
                        <label class="control-label">Nombre:</label>  
                        <input id="textBoxCrearPerfilNombre" type="text" class="form-control" style="width: 70%;" runat="server"/>
                        <br />
                        <label class="control-label">Nivel:</label>
                        <asp:DropDownList ID="dropDownListCrearPerfilNivel" class="input input-fozkr-dropdownlist" CssClass="form-control" Width="70%" runat="server"></asp:DropDownList>
                    </td>
                    <td rowspan="100"> <!-- Árbol -->
                        <label class="control-label">Permisos (un check significa que tiene ese permiso):</label>  
                        <asp:Panel ID="PanelArbolPermisos" runat="server" ScrollBars="Vertical" Height="500px" Width="500px" BorderWidth="1px">
                            <asp:TreeView ID="ArbolPermisos" ShowCheckBoxes="Leaf" runat="server" ImageSet="Simple" ShowLines="True" NodeIndent="30" Enabled="false" OnTreeNodeCollapsed="ArbolPermisos_TreeNodeCollapsed">
                                <HoverNodeStyle Font-Underline="False" ForeColor="Black" />
                                <LeafNodeStyle Font-Size="Medium" />
                                <NodeStyle Font-Names="Tahoma" Font-Size="Medium" ForeColor="Black"/>
                                <ParentNodeStyle Font-Bold="False" />
                                <RootNodeStyle Font-Size="Medium" />
                            </asp:TreeView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            

            <!-- Botones de Guardar/Cancelar para perfiles -->
            <div class="col-lg-12" runat="server">
                <div class="row">
                    <div class="text-center">
                        <button runat="server" type="button" class="btn btn-success-fozkr" onserverclick="botonAceptarCreacionPerfil_ServerClick" id="botonAceptarCreacionPerfil">Guardar</button>
                        <a runat="server" href="#modalCancelar" id="botonCancelarCreacionPerfil" class="btn btn-danger-fozkr" role="button" data-toggle="modal">Cancelar</a>
                    </div>
                </div>
            </div>
        </fieldset>
    </fieldset>

    <!-- Grid de consultas de perfiles -->
    <fieldset id="FieldsetConsultarPerfil" runat="server" class="fieldset">
        <br />
        <div class="col-lg-12">
            <strong><div ID="Div1" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Pefiles en Sistema</div></strong>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridViewConsultaPerfiles" CssClass="table" OnRowCommand="gridViewConsultaPerfiles_RowCommand" OnPageIndexChanging="gridViewConsultaPerfiles_PageIndexChanging" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None" >
                        <Columns>
                            <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                <ControlStyle CssClass="btn btn-default"></ControlStyle>
                            </asp:ButtonField>
                        </Columns>
                        <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                        <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                        <AlternatingRowStyle BackColor="#F8F8F8" />
                        <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gridViewConsultaPerfiles" EventName="RowCommand" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </fieldset>

    <!-- Fieldset de informacion de usuario (crear/consultar/modificar) -->
    <fieldset id="FieldsetUsuario" class="fieldset" runat="server" visible="false">
        <div class="col-lg-9">
            <div class="col-lg-12 row">
                <div class="col-lg-4 form-group">
                    <label for="inputNombre" class= "control-label">Nombre completo:</label>
                    <input id="inputNombre" runat="server"  type="text"  class="form-control"/>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ErrorMessage="Este espacio es requerido." 
                            ControlToValidate="inputNombre"
                            Font-Bold="true"
                            ForeColor="Red"
                            Display="Dynamic"
                            CssClass="ValidationError">
                            </asp:RequiredFieldValidator>
                </div>



                <div class="col-lg-4 form-group">
                    <label for="inputUsuario" class= "control-label">Nombre de usuario:</label>      
                    <input id="inputUsuario" runat="server" type="text" class="form-control" />

                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ErrorMessage="Este espacio es requerido." 
                            ControlToValidate="inputUsuario"
                            Font-Bold="true"
                            ForeColor="Red"
                            Display="Dynamic"
                            CssClass="ValidationError">
                            </asp:RequiredFieldValidator>

                </div>
                <div class="col-lg-4">
                    <label for="inputEstacion" class="control-label">Estación:</label>
                    <asp:DropDownList ID="DropDownListEstacion" OnSelectedIndexChanged="DropDownListEstacion_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-lg-12 row">
                <div class="col-lg-4">
                    <div class= "form-group">
                        <label for="inputFecha" id="labelInputFecha" runat="server" class= "control-label">Fecha de creación:</label>
                        <input id="inputFecha" runat="server"  type="text" class="form-control" />
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class= "form-group">

                        
                        <label for="DropDownListPerfilConsulta"  id="labelDropDownListPerfilConsulta" runat="server" class="control-label">Perfil:</label>
                        <asp:DropDownList ID="DropDownListPerfilConsulta"  OnSelectedIndexChanged="dropDownListCrearPerfilNivel_SelectedIndexChanged" AutoPostBack="true"  runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="form-group">
                        <label for="DropDownListAnfitriona" class="control-label">Anfitriona:</label>
                        <asp:DropDownList ID="DropDownListAnfitriona" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-lg-12 row">
                <div class="col-lg-4">
                    <label for="inputDescripcion" class="control-label">Descripción:</label>
                    <input id="inputDescripcion" runat="server"  type="text" class="form-control"/>
                </div>
                <div class="col-lg-4">
                    <label for="inputDescuentoMaximo" class="control-label">Descuento máximo para ventas:</label>
                    <input id="inputDescuentoMaximo" runat="server" type="text" class="form-control" />
                </div>
                <div class="col-lg-4">
                    <label for="DropDownListEstado" class="control-label">Estado:</label>
                    <asp:DropDownList ID="DropDownListEstado" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-lg-12 row">
                <br />
                <div class="col-lg-4">
                    <label for="inputPassword" id="labelInputPassword" runat="server" class= "control-label">Contraseña:</label>
                    <input id="inputPassword" runat="server" type="password" class="form-control" />
                 </div>
                <div class="col-lg-4">
                    <label for="inputPasswordConfirm" id="labelInputPasswordConfirm" runat="server" class= "control-label">Confirmar contraseña:</label>
                    <input id="inputPasswordConfirm" runat="server"  type="password"  class="form-control"/>
                </div>
                <br />
            </div>
        </div>
        <div class="col-lg-3">
            <!-- Grid de bodegas -->
             <div class="col-lg-12"><strong><div ID="Div3" runat="server" visible="true" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;"> Bodegas</div></strong>
                    <asp:UpdatePanel ID="UpdatePanelBodegas" runat="server">
                        <ContentTemplate>
                                                <asp:Panel ID="PanelBodegas" runat="server" ScrollBars="Vertical" Height="200px">
                            <asp:GridView ID="gridViewBodegas" CssClass="table"  runat="server" AllowPaging="false"  BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None">
						        <Columns>
							        <asp:TemplateField HeaderText="Seleccionar">
								        <ItemTemplate>
									        <asp:CheckBox ID="checkBoxBodegas" OnCheckedChanged="checkBoxBodegas_CheckedChanged" runat="server" AutoPostBack="true"/>
								        </ItemTemplate>
							        </asp:TemplateField>
						        </Columns>                                
                                <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                               <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                               <AlternatingRowStyle BackColor="#F8F8F8" />
                               <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                               <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                          </asp:GridView>
                                                    </asp:Panel>
                     </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridViewBodegas" EventName="RowCommand" />
                     </Triggers>
                  </asp:UpdatePanel>
               </div>  
        </div>
    </fieldset>

        <!-- Grid de consultas -->
        <fieldset id="FieldsetGridCuentas" runat="server" class="fieldset">
            <!-- Gridview de consultar -->
            <div class="col-lg-12">
                <strong><div ID="tituloGridConsulta" runat="server" visible="false" tabindex="" class="control-label" style="text-align:center;font-size:larger; background-color: #C0C0C0;">Cuentas en Sistema</div></strong>
                <asp:UpdatePanel ID="UpdatePanelCuentas" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridViewCuentas" CssClass="table" OnRowCommand="gridViewCuentas_RowCommand" OnPageIndexChanging="gridViewCuentas_PageIndexChanging" runat="server" AllowPaging="True" PageSize="10" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" GridLines="None" >
                            <Columns>
                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default" CommandName="Select" Text="Consultar">
                                    <ControlStyle CssClass="btn btn-default"></ControlStyle>
                                </asp:ButtonField>
                            </Columns>
                            <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                            <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="#F8F8F8" />
                            <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" BackColor="Silver" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridViewCuentas" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>         


    <!-- Fieldset de asociacion de usuario a perfil -->
    <fieldset id="FieldsetAsociarUsuario" class="fieldset" runat="server" visible="false">
        <!-- DropDown Lists que cargan las estaciones disponibles y las bodegas pertenecientes a la estación seleccionada -->
        <div class="row">
            <div class="col-lg-4">
                <label for="DropDownListUsuario" class="control-label">Seleccione al usuario:</label>
                <asp:DropDownList ID="DropDownListUsuario" runat="server" CssClass="form-control" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="col-lg-4">
                <label for="DropDownListPerfil" class="control-label">Seleccione perfil a asignar:</label>
                <asp:DropDownList ID="DropDownListPerfil" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
         </div>
    </fieldset>

    <!-- Fieldset de grid -->
    <fieldset id="FieldsetGrid" class="fieldset" runat="server" visible="false">
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

                           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="Es necesario que confirme su contraseña es un espacio necesario"
                            Font-Bold="true"
                            ControlToValidate="inputPasswordConfirm"
                            ClientValidationFunction="changeColor" 
                            ForeColor="Red"
                            Display="Dynamic"
                            CssClass="ValidationError">
                            </asp:RequiredFieldValidator>

    <!-- Fieldset de botones de aceptar/cancelar -->
    <fieldset id="FieldsetBotones" class="fieldset" runat="server" visible="false">
        <div class="col-lg-12" id="bloqueBotones" runat="server">
            <div class =" row">
                <div class="text-center">
                    <button runat="server" type="button" class="btn btn-success-fozkr" onserverclick="botonAceptarUsuario_ServerClick" id="botonAceptar">Aceptar</button>
                    <a runat="server" href="#modalCancelar" id="botonCancelarEverything" class="btn btn-danger-fozkr" role="button" data-toggle="modal">Cancelar</a>
                </div>
            </div>
        </div>
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
                    <button type="button" id="botonAceptarModalCancelar" class="btn btn-success-fozkr" causesvalidation="false" runat="server" onserverclick="botonAceptarModalCancelar_ServerClick">Aceptar</button>
                    <button type="button" id="botonCancelarModalCancelar" class="btn btn-danger-fozkr" causesvalidation="false" data-dismiss="modal">Cancelar</button>                   
                </div>
            </div>
        </div>
    </div>
    <!-- Javascript -->
    <!-- Modificar tab de site master activo -->
    <script type = "text/javascript">
        function setCurrentTab() {
            document.getElementById("linkFormAdministracion").className = "active";
        }
    </script>
</asp:Content>
