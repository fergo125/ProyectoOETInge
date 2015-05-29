using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProyectoInventarioOET.Modulo_Bodegas;
using ProyectoInventarioOET.Modulo_Productos_Locales;

namespace ProyectoInventarioOET.App_Code.Modulo_Traslados
{
    public class ControladoraTraslado
    {

        private ControladoraBDTraslado controladoraBD;
        

        public ControladoraTraslado() {
            controladoraBD = new ControladoraBDTraslado();
        }


        public DataTable consultarProductosDeBodega(String idBodega)
        {
            ControladoraProductoLocal controladoraProductoLocal = new ControladoraProductoLocal();
            return controladoraProductoLocal.consultarProductosDeBodegaAjustes(idBodega);
        }


        public DataTable consultarTraslados(String idBodega)
        {
            DataTable traslados = controladoraBD.consultaTraslados(idBodega);
            traslados.Columns.Add("Tipo", typeof(string));
            if (traslados.Rows.Count > 0)
            {
                foreach (DataRow fila in traslados.Rows) {
                    fila[5] = "Entrada";
                    if (fila[4].ToString() == idBodega) { //Si el idOrigen es igual al de la bodega que estoy consultado
                        fila[5] = "Salida";
                    }
                }
            }
            return traslados;
        }



    }
}