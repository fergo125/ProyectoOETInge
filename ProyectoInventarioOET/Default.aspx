<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProyectoInventarioOET._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>SIO: Sistema de Inventarios OET</h1>
        <p class="lead">Este sistema permite realizar diversas labores, como facturación, mantenimiento de inventario, generación de reportes contables, e incluso tareas administrativas.</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Inventarios</h2>
            <p>
                Confirme la entrada de productos comprados recientemente a proveedores, maneje solicitudes de movimiento entre bodegas, agregue entradas extraordinarias o ajustes por toma física, entre otros.
                Todas las tareas relacionadas con el inventario de su bodega pueden encontrarse en esta sección.
            </p>
        </div>
        <div class="col-md-4">
            <h2>Ventas</h2>
            <p>
                Cree nuevas facturas rápidamente en un ambiente ágil para el usuario, busque productos, aplique descuentos, y no se preocupe por aspectos como el tipo de cambio o la información que ya debería de generarse automáticamente.
            </p>
        </div>
        <div class="col-md-4">
            <h2>Reportes</h2>
            <p>
                Reporte de compras, reporte de ventas, los productos más vendidos, el costo promedio de los productos, incluso generación de batches a MIP.
                Toda la contabilidad del sistema comprimida en un sólo lugar.
            </p>
        </div>
    </div>

</asp:Content>
