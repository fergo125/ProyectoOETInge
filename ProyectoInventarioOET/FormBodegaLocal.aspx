<%@ Page Title="" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="FormBodegaLocal.aspx.cs" Inherits="ProyectoInventarioOET.FormBodegaLocal" %>
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
        <h2 id="TituloBodegaLocal" runat="server">Bodega Local</h2>
        <hr />
    </div>

    <!-- Cuerpo del Form -->
    <fieldset id= "FieldsetBodegas" runat="server" class="fieldset" visible="true">
                <div class="form-horizontal">
                    <h3>Seleccione una bodega local de trabajo.</h3>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2"></div>
                            <div class="col-lg-3">
                                <label for="inputBodega" class="control-label">Seleccione bodega:</label><br />
                                <asp:DropDownList ID="DropDownListBodega" runat="server" CssClass="form-control" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
    </fieldset>
    <br /><br /><br />

</asp:Content>
