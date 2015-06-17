using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace ProyectoInventarioOET.Modulo_Categorias
{
    /*
     * ???
     */
    public class ControladoraCategorias
    {
        //Atributos
        private ControladoraBDCategorias controladoraBDCategorias;  //???

        /*
         * Constructor.
         */
        public ControladoraCategorias()
        {
            controladoraBDCategorias = new ControladoraBDCategorias();
        }

        /*
         * Consulta la información de una categoria particular.
         */
        public EntidadCategoria consultarCategoria(String id)
        {
            return controladoraBDCategorias.consultarCategoria(id);
        }

        /*
         * Crea una nueva categoria dado un vector con los datos de la misma.
         */
        public String[] insertarDatos(string descripcion,string estado)
        {
            EntidadCategoria categoria = new EntidadCategoria(descripcion, estado);
            return controladoraBDCategorias.insertarCategoria(categoria);
        }

        /*
         * Modifica los datos de una categoria particular.
         */
        public String[] modificarDatos(EntidadCategoria categoriaVieja, Object[] datoscategoriaNueva)
        {
            EntidadCategoria categoriaNueva = new EntidadCategoria(datoscategoriaNueva);
            return controladoraBDCategorias.modificarCategoria(categoriaVieja, categoriaNueva);
        }

        /*
         * Consulta la información de todas las categorias.
         */
        public DataTable consultarCategorias()
        {
            return controladoraBDCategorias.consultarCategorias();
        }
    }
}