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
 * Se hizo de esta forma tomando en cuenta la invariabilidad de estos datos.
 */
namespace ProyectoInventarioOET.App_Code
{
    public class ControladoraDatosGenerales
    {
        private static ControladoraDatosGenerales instanciaSingleton = null;
        private DataTable estados;
        private DataTable estadosActividad;
        private DataTable unidades;
        private DataTable estaciones;
        private DataTable anfitrionas;
        private int tipoCambioCompra;
        private int tipoCambioVenta;
        private int impuesto;
        

        public static ControladoraDatosGenerales Instanciar
        {
            get
            {
                if (instanciaSingleton == null)
                    instanciaSingleton = new ControladoraDatosGenerales();
                return instanciaSingleton;
            }
        }

        private ControladoraDatosGenerales()
        {
            ControladoraBDEstados controladoraEstados = new ControladoraBDEstados();
            ControladoraBDUnidades controladoraUnidades = new ControladoraBDUnidades();
            ControladoraBDEstaciones controladoraEstaciones = new ControladoraBDEstaciones();
            ControladoraBDAnfitrionas controladoraAnfitriones = new ControladoraBDAnfitrionas();
            ControladoraBDGeneral controladoraGeneral = new ControladoraBDGeneral();

            estados = controladoraEstados.consultarEstados();
            estadosActividad = controladoraEstados.consultarEstadosActividad();
            unidades = controladoraUnidades.consultarUnidades();
            estaciones = controladoraEstaciones.consultarEstaciones();
            anfitrionas = controladoraAnfitriones.consultarAnfitriones();
            impuesto = controladoraGeneral.consultarImpuesto();

            DataTable tipoCambio = controladoraGeneral.consultarTipoCambio();
            tipoCambioCompra = Convert.ToInt32(tipoCambio.Rows[0][0]);
            tipoCambioVenta = Convert.ToInt32(tipoCambio.Rows[0][1]);
        }

        public DataTable consultarEstados()
        {
            return estados;
        }

        // Activo e inactivo
        public DataTable consultarEstadosActividad()
        {
            return estadosActividad;
        }

        public DataTable consultarUnidades()
        {
            return unidades;
        }

        public DataTable consultarEstaciones()
        {
            return estaciones;
        }

        public DataTable consultarAnfitriones()
        {
            return anfitrionas;
        }

        public String traduccionEstado(int valor)
        {
            return estados.Rows[valor-1][0].ToString();
        }

        public int impuestoVentas()
        {
            return impuesto;
        }

        public int dolarCompra()
        {
            return tipoCambioCompra;
        }

        public int dolarVenta()
        {
            return tipoCambioVenta;
        }
    }
}