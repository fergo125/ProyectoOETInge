using System;
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
            EntidadFactura factura = new EntidadFactura(datosFactura);
            return controladoraBDVentas.insertarFactura(factura);
        }

        /*
         * Invocada por la capa de interfaz para revisar si un producto que está intentando agregarse a una factura,
         * de verdad existe en la base de datos.
         */
        public String verificarExistenciaProductoLocal(String idBodega, String productoEscogido)
        {
            //Primero, obtener la llave desde el catálogo global usando esos dos valores (es necesario revisar ambos
            //valores para asegurarse de que el usuario no cambio ninguno antes de dar click al botón). 
            String codigoProductoEscogido = productoEscogido.Substring(productoEscogido.LastIndexOf('(') + 1);  //el código sin el primer paréntesis
            codigoProductoEscogido = codigoProductoEscogido.TrimEnd(')');                                       //el código
            productoEscogido = productoEscogido.Remove(productoEscogido.LastIndexOf('(') - 1);                  //nombre del producto (con un -1 al final por el espacio)
            String llaveProducto = controladoraBDVentas.verificarExistenciaProductoGlobal(productoEscogido, codigoProductoEscogido);

            //Luego, si el producto se existe, se usa su llave para verificar que existe en el catálogo local de la bodega
            //que se está usando como punto de venta.
            if (controladoraBDVentas.verificarExistenciaProductoLocal(llaveProducto, idBodega))
                return llaveProducto; //Finalmente, se retorna nulo si el producto no se encuentra, y la llave del produto si sí se encuentra
            return null;
        }

        /*
         * ???
         */
        public DataTable consultarFacturas(String perfil, String idUsuario, String idBodega, String idEstacion)
        {
            return controladoraBDVentas.consultarFacturas(perfil, idUsuario, idBodega, idEstacion);
        }

        /*
         * ???
         */
        public EntidadFactura consultarFactura(String id)
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
    }
}