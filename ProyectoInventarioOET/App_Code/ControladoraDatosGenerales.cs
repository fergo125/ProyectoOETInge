using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

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
        private static ControladoraDatosGenerales instancia = null;
        private DataTable estaciones;
        private DataTable anfitriones;
        private DataTable unidades;
        private DataTable estados;
        

        public static ControladoraDatosGenerales Instanciar
        {
            get
            {
                if (instancia == null)
                    instancia = new ControladoraDatosGenerales();
                return instancia;
            }
        }

        private ControladoraDatosGenerales()
        {
            ControladoraBDAnfitriones controladoraAnfitriones = new ControladoraBDAnfitriones();
            ControladoraBDEstaciones controladoraEstaciones = new ControladoraBDEstaciones();
            ControladoraBDEstados controladoraEstados = new ControladoraBDEstados();
            ControladoraBDUnidades controladoraUnidades = new ControladoraBDUnidades();

            anfitriones = controladoraAnfitriones.consultarAnfitriones();
            estaciones = controladoraEstaciones.consultarEstaciones();
            estados = controladoraEstados.consultarEstados();
            unidades = controladoraUnidades.consultarUnidades();
        }

        public DataTable consultarAnfitriones()
        {
            return anfitriones;
        }

        public DataTable consultarEstaciones()
        {
            return estaciones;
        }

        public DataTable consultarEstados()
        {
            return estados;
        }

        public DataTable consultarUnidades()
        {
            return unidades;
        }
    }
}