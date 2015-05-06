using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.Módulo_Actividades
{
    /*
     * Comunicación entre la Controladora de Base de Datos y la que maneja las operaciones de la interfaz.
     */
    public class ControladoraActividades
    {
        //Atributos
        private ControladoraBDActividades controladoraBDActividades;    // Instancia de la controladora de base
                                                                        // de datos para realizar operaciones allí.

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
         * Consulta la información de una actividad particular a partir de su nombre.
         */
        public EntidadActividad consultarActividadPorNombre(String nombre)
        {
            return controladoraBDActividades.consultarActividadPorNombre(nombre);
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
         * Consulta la información de todas las actividades
         */
        public DataTable consultarActividades()
        {
            return controladoraBDActividades.consultarActividades();
        }
    }
}