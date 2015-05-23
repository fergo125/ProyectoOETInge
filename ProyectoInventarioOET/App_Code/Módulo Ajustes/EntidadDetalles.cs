using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Módulo_Ajustes
{
    public class EntidadDetalles
    {
        
        private String nombreProducto;
        private String codigo;
        private double cambio;
        private String idProductoBodega;

            public EntidadDetalles (Object[] datos)
            {
                this.nombreProducto = datos[0].ToString();
                this.codigo = datos[1].ToString();
                this.cambio = Double.Parse(datos[2].ToString());
                this.idProductoBodega = datos[3].ToString();                
            }


            public String NombreProducto
            {
                get { return nombreProducto; }
                set { nombreProducto = value; }
            }

            public String Codigo
            {
                get { return codigo; }
                set { codigo = value; }
            }

            public double Cambio
            {
                get { return cambio; }
                set { cambio = value; }
            }

            public String IdProductoBodega
            {
                get { return idProductoBodega; }
                set { idProductoBodega = value; }
            }

    }
}