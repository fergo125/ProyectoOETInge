using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace ProyectoInventarioOET.Modulo_Categorias
{
    public class ControladoraCategorias
    {

        private ControladoraBDCategorias controladoraBDCategorias;

        public ControladoraCategorias()
        {
            controladoraBDCategorias = new ControladoraBDCategorias();
        }


        public EntidadCategoria consultarCategoria(String id)
        {
            /*consulta la información de una categoria particular*/
            return controladoraBDCategorias.consultarCategoria(id);
        }

        public String[] insertarDatos(Object[] datoscategoria)
        {
            /*crea una nueva categoria dado un vector con los datos de la misma*/
            EntidadCategoria categoria = new EntidadCategoria(datoscategoria);
            return controladoraBDCategorias.insertarCategoria(categoria);
        }

        public String[] modificarDatos(EntidadCategoria categoriaVieja, Object[] datoscategoriaNueva)
        {
            /*modifica los datos de una categoria particular*/
            EntidadCategoria categoriaNueva = new EntidadCategoria(datoscategoriaNueva,categoriaVieja.Nombre);
            return controladoraBDCategorias.modificarCategoria(categoriaVieja, categoriaNueva);
        }

        public String[] desactivarcategoria(EntidadCategoria categoria)
        {
            /*desactiva una categoria de la base de datos*/
            return controladoraBDCategorias.desactivarCategoria(categoria);
        }

        public DataTable consultarCategorias()
        {
            /*consulta la información de todas las categorias*/
            return controladoraBDCategorias.consultarCategorias();
        }
    }
    }