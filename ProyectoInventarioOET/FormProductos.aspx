<%@ Page Title="Productos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FormProductos.aspx.cs" Inherits="ProyectoInventarioOET.FormProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <div class= " form-control">
        <fieldset id= "">
            Nombre: <input id="txtNombreProducto" type="text" />
            Descripcion: <input id="txtDescripcionProducto" type="text" />
        </fieldset>
        </div>
</asp:Content>