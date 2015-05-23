using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{
    public class EntidadDetalles
    {
        
        private String idProductoBodega;
        private String nombreProducto;
        private double cambio;

            public EntidadDetalles (String idProductoBodega, String nombreProducto, double cambio)
            {
                this.idProductoBodega = idProductoBodega;
                this.nombreProducto = nombreProducto;
                this.cambio = cambio; 
            }


            public String IdProductoBodega
            {
                get { return idProductoBodega; }
                set { idProductoBodega = value; }
            }

            public String NombreProducto
            {
                get { return nombreProducto; }
                set { nombreProducto = value; }
            }

            public double Cambio
            {
                get { return cambio; }
                set { cambio = value; }
            }
    }
}