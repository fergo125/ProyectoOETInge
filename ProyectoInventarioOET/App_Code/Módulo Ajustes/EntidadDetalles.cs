using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{
    public class EntidadDetalles
    {
        private String idAjuste;
            private String idProductoBodega;
            private double cambio;

            public EntidadDetalles (String idAjuste, String idProductoBodega, double cambio)
            {
                this.idAjuste = idAjuste;
                this.idProductoBodega = idProductoBodega;
                this.cambio = cambio;
            }

            public String IdAjuste
            {
                get { return idAjuste; }
                set { idAjuste = value; }
            }


            public String IdProductoBodega
            {
                get { return idProductoBodega; }
                set { idProductoBodega = value; }
            }

            public double Cambio
            {
                get { return cambio; }
                set { cambio = value; }
            }




    }
}