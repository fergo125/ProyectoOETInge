using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

/*
 * Controladora de datos generales, utilizada entre varios módulos.
 * Utiliza patrón singleton.
 * Auspicia datos sobre estaciones, compañías, unidades métricas, y estados.
 * 
 * Al ser instanciada trae todos los datos desde la base de datos, almacenándolos en memoria.
 * Por esto, para que se actualicen es necesario cerrar el explorador y volver a abrirlo.
 * Se hizo de esta forma tomando en cuenta la invariabilidad de estos datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraDatosGenerales
    {
        private static ControladoraDatosGenerales instanciaSingleton = null;
        private DataTable estados;
        private DataTable unidades;
        private DataTable estaciones;
        private DataTable anfitrionas;
        

        public static ControladoraDatosGenerales Instanciar
        {
            get
            {
                if (instanciaSingleton == null)
                    instanciaSingleton = new ControladoraDatosGenerales();
                return instanciaSingleton;
            }
        }

        private ControladoraDatosGenerales()
        {
            ControladoraBDEstados controladoraEstados = new ControladoraBDEstados();
            ControladoraBDUnidades controladoraUnidades = new ControladoraBDUnidades();
            ControladoraBDEstaciones controladoraEstaciones = new ControladoraBDEstaciones();
            ControladoraBDAnfitrionas controladoraAnfitriones = new ControladoraBDAnfitrionas();

            estados = controladoraEstados.consultarEstados();
            unidades = controladoraUnidades.consultarUnidades();
            estaciones = controladoraEstaciones.consultarEstaciones();
            anfitrionas = controladoraAnfitriones.consultarAnfitriones();
        }

        public DataTable consultarEstados()
        {
            return estados;
        }

        public DataTable consultarUnidades()
        {
            return unidades;
        }

        public DataTable consultarEstaciones()
        {
            return estaciones;
        }

        public DataTable consultarAnfitriones()
        {
            return anfitrionas;
        }
    }
}