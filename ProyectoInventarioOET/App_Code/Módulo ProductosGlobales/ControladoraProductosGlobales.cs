using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.Modulo_Categorias;

namespace ProyectoInventarioOET.App_Code.Módulo_ProductosGlobales
{
    /* Controladora de productos globales, utilizada por la interfaz del catálogo general de productos.
     * Auspicia los datos generales o globales de los productos que la OET maneja 
     * Ofrece la comunicación entre la Controladora de Base de Datos y la interfaz.
     */
    public class ControladoraProductosGlobales
    {
        private ControladoraBDProductosGlobales controladoraBD; 
        private EntidadProductoGlobal productoActual;           

        /*
         * Constructor de la controladora.
         * Instancia la controladora de base de datos
         */
        public ControladoraProductosGlobales()
        {
            controladoraBD = new ControladoraBDProductosGlobales(); // Instancia de la controladora de base de datos para realizar operaciones allí.
        }

        /*
         * Método que retorna una producto global específico, encapsulado en la entidad 
         * correspondiente, mediante un llamado a la controladora de bases de datos
         */
        public EntidadProductoGlobal consultarProductoGlobal(String id)
        {
            productoActual = controladoraBD.consultarProductoGlobal(id);
            return productoActual;
        }

        /*
         * Método que inserta un producto global con los datos ingresados por el 
         * usuario, encapsulado en la entidad correspondiente, mediante un llamado a la controladora de 
         * bases de datos
         */
        public String[] insertar(Object[] datosProductoGlobal)
        {
            EntidadProductoGlobal productoGlobal = new EntidadProductoGlobal(datosProductoGlobal);
            return controladoraBD.insertarProductoGlobal(productoGlobal);
        }


         /*
         * Método que modifica un producto global con los datos ingresados por el 
         * usuario, encapsulado en la entidad correspondiente, mediante un llamado a la controladora de 
         * bases de datos
         */
        public String[] modificarDatos(EntidadProductoGlobal productoGlobalViejo, Object[] datosProductoGlobalModificado)
        {
            EntidadProductoGlobal productoGlobalModificado = new EntidadProductoGlobal(datosProductoGlobalModificado);
            return controladoraBD.modificarProductoGlobal(productoGlobalViejo, productoGlobalModificado);
        }

        /*
         * Método que desactiva un producto global mediante un llamado a la controladora de 
         * bases de datos
         */
        public String[] desactivarProductoGlobal(EntidadProductoGlobal productoGlobal)
        {
            /*desactiva una bodega de la base de datos*/
            return controladoraBD.desactivarProductoGlobal(productoGlobal);
        }


        /*
         * Método que consulta todos los productos globales mediante un llamado a 
         * la controladora de bases de datos. Los datos retornados solo muestran la información más
         * importante de ellos
         */
        public DataTable consultarProductosGlobales()
        {
            return controladoraBD.consultarProductosGlobales();
        }

        /*
         * Método que consulta las categorias para presentarle al usuario las opciones   
         * con las que puede asociar o categorizar un producto global mediante un llamado a la controladora de categoria
         */
        public DataTable consultarCategorias()
        {
            ControladoraCategorias controladoraCategorias = new ControladoraCategorias();   // Hace un llamado a la controladora de categorias
            return controladoraCategorias.consultarCategorias();                            // para obtener los id y nombres de las categorias
        }


        /*
         * Método que consulta los productos globales con el criterio de busqueda ingresado por el  
         * usuario mediante un llamado a la controladora de bases de datos
         */
        public DataTable consultarProductosGlobales(string query)
        {
            return controladoraBD.consultarProductosGlobales(query); ;
        }
    }
}