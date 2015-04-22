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
 * Se hizo de esta forma por la invariabilidad de datos.
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

        public ControladoraDatosGenerales()
        {
            // Crear controladoras BD y traer datos
        }
    }
}