﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.Modulo_Ventas
{
    /*
     * Clase controladora del módulo de Ventas, encargada de la comunicación entre la capa de interfaz y la capa de datos.
     * Toda operación de base de datos solicitada por la interfaz pasa por esta clase, realizando los arreglos, adaptaciones,
     * y encapsulamientos necesarios.
     */
    public class ControladoraVentas
    {
        //Atributos
        private ControladoraBDVentas controladoraBDVentas;  //Usada para interactuar con la base de datos

        /*
         * Constructor.
         */
        public ControladoraVentas()
        {
            controladoraBDVentas = new ControladoraBDVentas();
        }

        /*
         * ???
         */
        public String[] insertarFactura(Object[] datosFactura)
        {
            EntidadFacturaVenta factura = new EntidadFacturaVenta(datosFactura);
            return controladoraBDVentas.insertarFactura(factura);
        }

        /*
         * Invocada por la capa de interfaz para revisar si un producto que está intentando agregarse a una factura,
         * de verdad existe en la base de datos.
         */
        public String verificarExistenciaProductoLocal(String idBodega, String nombreProductoEscogido, String codigoProductoEscogido)
        {
            String llaveProducto = controladoraBDVentas.verificarExistenciaProductoGlobal(nombreProductoEscogido, codigoProductoEscogido);
            //Luego, si el producto se existe, se usa su llave para verificar que existe en el catálogo local de la bodega
            //que se está usando como punto de venta.
            if (controladoraBDVentas.verificarExistenciaProductoLocal(llaveProducto, idBodega))
                return llaveProducto; //Finalmente, se retorna nulo si el producto no se encuentra, y la llave del produto si sí se encuentra
            return null;
        }

        /*
         * ???
         */
        public DataTable consultarFacturas(String idUsuario, String idBodega, String idEstacion)
        {
            return controladoraBDVentas.consultarFacturas(idUsuario, idBodega, idEstacion);
        }

        /*
         * ???
         */
        public EntidadFacturaVenta consultarFactura(String id)
        {
            return controladoraBDVentas.consultarFactura(id);
        }

        /*
         * ???
         */
        public DataTable asociadosABodegas(String idBodega, String idEstacion)
        {
            if (idBodega == "All")
                return controladoraBDVentas.asociadosAEstacion(idEstacion); //Si se pregunta por todas ("All"), se consulta a nivel de estación
            else
                return controladoraBDVentas.asociadosABodega(idBodega); 
        }

        /*
         * Obtiene el máximo de descuento aplicable a la venta de un producto específico por parte de un empleado específico 
         */
        public int maximoDescuentoAplicable(String idProducto, String idVendedor)
        {
            return controladoraBDVentas.maximoDescuentoAplicable(idProducto,idVendedor);
        }

        /*
         * Obtiene la venta del tipo de cambio más reciente en la base de datos.
         */
        public double consultarTipoCambio()
        {
            return controladoraBDVentas.consultarTipoCambio();
        }

        /*
         * Obtiene el precio unitario, en colones o dólares, de un producto.
         */
        public double consultarPrecioUnitario(String llaveProducto, String tipoMoneda)
        {
            return Math.Round((controladoraBDVentas.consultarPrecioUnitario(llaveProducto, tipoMoneda)), 2, MidpointRounding.AwayFromZero);
            //return controladoraBDVentas.consultarPrecioUnitario(llaveProducto, tipoMoneda);
        }

        /*
         * ???
         */
        public String[] anularFactura(EntidadFacturaVenta entidadFactura)
        {
            return controladoraBDVentas.anularFactura(entidadFactura);
        }

        /*
         * Obtiene las posibles formas de pago.
         */
        public DataTable consultarMetodosPago()
        {
            return controladoraBDVentas.consultarMetodosPago();
        }

        /*
         * Obtiene los posibles clientes para las ventas, dentro de los empleados de la OET.
         */
        public DataTable consultarPosiblesClientes()
        {
            return controladoraBDVentas.consultarClientes();
        }
    }
}