using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Módulo_Actividades
{
    public class ControladoraActividades
    {

 
        private ControladoraBDActividades controladoraBDActividades;

        public ControladoraActividades()
        {
            controladoraBDActividades = new ControladoraBDActividades();
        }


        public EntidadActividad consultarActividad(String codigo)
        {
            /*consulta la información de una actividad particular*/
            return controladoraBDActividades.consultarActividad(codigo);
        }

        public String[] insertarDatos(String codigo, String descripcion, int estado)
        {
            /*crea una nueva actividad dado un vector con los datos de la misma*/
            Object[] datosActividad = new Object[3];
            datosActividad[0] = codigo;
            datosActividad[1] = descripcion;
            datosActividad[2] = estado;

            EntidadActividad actividad = new EntidadActividad(datosActividad);
            return controladoraBDActividades.insertarActividad(actividad);
        }

        public String[] modificarDatos(EntidadActividad actividadVieja, String descripcionNueva, int estadoNuevo)
        {
            /*crea una nueva actividad dado un vector con los datos de la misma*/
            Object[] actividadNueva = new Object[3];
            actividadNueva[0] = actividadVieja.Codigo;
            actividadNueva[1] = descripcionNueva;
            actividadNueva[2] = estadoNuevo;

            EntidadActividad actividad = new EntidadActividad(actividadNueva);
            return controladoraBDActividades.modificarActividad(actividadVieja, actividad);
        }

        public String[] desactivarActividad(EntidadActividad actividad)
        {
            /*desactiva una actividad de la base de datos*/
            return controladoraBDActividades.desactivarActividad(actividad);
        }

        public DataTable consultarActividades()
        {
            /*consulta la información de todas las bodegas*/
            return controladoraBDActividades.consultarActividades();
        }
    }
}