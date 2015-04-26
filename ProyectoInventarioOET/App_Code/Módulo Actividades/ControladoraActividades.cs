using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Módulo_Actividades
{
    /*
     * ???
     */
    public class ControladoraActividades
    {
        //Atributos
        private ControladoraBDActividades controladoraBDActividades;    //???

        /*
         * Constructor.
         */
        public ControladoraActividades()
        {
            controladoraBDActividades = new ControladoraBDActividades();
        }

        /*
         * Consulta la información de una actividad particular.
         */
        public EntidadActividad consultarActividad(String codigo)
        {
            return controladoraBDActividades.consultarActividad(codigo);
        }

        /*
         * Crea una nueva actividad dado un vector con los datos de la misma.
         */
        public String[] insertarDatos(String descripcion, int estado)
        {
            Object[] datosActividad = new Object[3];
            datosActividad[0] = "";
            datosActividad[1] = descripcion;
            datosActividad[2] = estado;

            EntidadActividad actividad = new EntidadActividad(datosActividad);
            return controladoraBDActividades.insertarActividad(actividad);
        }

        /*
         * Crea una nueva actividad dado un vector con los datos de la misma
         */
        public String[] modificarDatos(EntidadActividad actividadVieja, String descripcionNueva, int estadoNuevo)
        {
            Object[] actividadNueva = new Object[3];
            actividadNueva[0] = actividadVieja.Codigo;
            actividadNueva[1] = descripcionNueva;
            actividadNueva[2] = estadoNuevo;

            EntidadActividad actividad = new EntidadActividad(actividadNueva);
            return controladoraBDActividades.modificarActividad(actividadVieja, actividad);
        }

        /*
         * Desactiva una actividad de la base de datos
         */
        public String[] desactivarActividad(EntidadActividad actividad)
        {
            return controladoraBDActividades.desactivarActividad(actividad);
        }

        /*
         * Consulta la información de todas las bodegas
         */
        public DataTable consultarActividades()
        {
            return controladoraBDActividades.consultarActividades();
        }
    }
}