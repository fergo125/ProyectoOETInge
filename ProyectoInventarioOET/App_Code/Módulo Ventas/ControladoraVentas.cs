using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ProyectoInventarioOET.App_Code.Modulo_Ventas
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
    }
}