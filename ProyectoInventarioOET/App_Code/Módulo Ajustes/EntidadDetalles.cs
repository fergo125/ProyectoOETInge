using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.App_Code.Modulo_Ajustes
{
    public class EntidadDetalles
    {
        
        private String nombreProducto;
        private String codigo;
        private double cambio;
        private String idProductoBodega;
        private double final;
        private String unidades;

            public EntidadDetalles (Object[] datos)
            {
                this.nombreProducto = datos[0].ToString();
                this.codigo = datos[1].ToString();
                this.cambio = Double.Parse(datos[2].ToString());
                this.idProductoBodega = datos[3].ToString();
                if( datos.Count() > 4 )
                    this.final = Double.Parse(datos[4].ToString());
            }

            public EntidadDetalles(Object[] datos, bool traslado)
            {
                this.nombreProducto = datos[0].ToString();
                this.codigo = datos[1].ToString();
                this.cambio = Double.Parse(datos[2].ToString());
                this.unidades = datos[3].ToString();
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


            public String Unidades
            {
                get { return unidades; }
                set { unidades = value; }
            }



    }
}