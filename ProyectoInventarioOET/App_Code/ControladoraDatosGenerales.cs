using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.Módulo_Bodegas;
namespace ProyectoInventarioOET.App_Code
{
    /*
     * Controladora de datos generales, utilizada entre varios módulos.
     * Utiliza patrón singleton.
     * Auspicia datos sobre estaciones, compañías, unidades métricas, estados, entre otros de interés general.
     * 
     * Al ser instanciada trae todos los datos desde la base de datos, almacenándolos en memoria.
     * Por esto, para que se actualicen es necesario cerrar el explorador y volver a abrirlo.
     * Se hizo de esta forma tomando en cuenta la invariabilidad de estos datos.
     */
    public class ControladoraDatosGenerales
    {
        private static ControladoraDatosGenerales instanciaSingleton = null;    //Instancia que será usada por todos
        private DataTable estadosActividad;                                     //???
        private DataTable anfitrionas;                                          //Información de las compañías (OET/ESINTRO)
        private DataTable intenciones;                                          //Posibles intenciones de uso para los productos
        private DataTable estaciones;                                           //Estaciones de la OET existentes
        private DataTable unidades;                                             //Posibles unidades métricas para los productos
        private DataTable estados;                                              //Posibles estados para todas las entidades del sistema
        private int tipoCambioCompra;                                           //Tipo de cambio colones a dólares
        private int tipoCambioVenta;                                            //Tipo de cambio dólares a colones
        private int impuesto;                                                   //Impuesto de venta (13%)
        
        /*
         * Método usado para instanciar el objeto singleton que será usado por todas las controladoras que lo necesiten.
         * Sólo debe ser creada una vez por cada ejecución del sistema en cada servidor.
         */
        public static ControladoraDatosGenerales Instanciar
        {
            get
            {
                if (instanciaSingleton == null)
                    instanciaSingleton = new ControladoraDatosGenerales();
                return instanciaSingleton;
            }
        }

        /*
         * Constructor, inicializa las controladoras de datos generales individualmente para servir de interfaz entre
         * las mismas y las demás controladoras del sistema. A la vez carga las tablas de interés.
         */
        private ControladoraDatosGenerales()
        {
            ControladoraBDGeneral controladoraGeneral = new ControladoraBDGeneral();
            ControladoraBDEstados controladoraEstados = new ControladoraBDEstados();
            ControladoraBDBodegas controladoraBDBodegas = new ControladoraBDBodegas();
            ControladoraBDUnidades controladoraUnidades = new ControladoraBDUnidades();
            ControladoraBDEstaciones controladoraEstaciones = new ControladoraBDEstaciones();
            ControladoraBDAnfitrionas controladoraAnfitriones = new ControladoraBDAnfitrionas();

            estados = controladoraEstados.consultarEstados();
            impuesto = controladoraGeneral.consultarImpuesto();
            unidades = controladoraUnidades.consultarUnidades();
            estaciones = controladoraEstaciones.consultarEstaciones();
            intenciones = controladoraBDBodegas.consultarIntenciones();
            anfitrionas = controladoraAnfitriones.consultarAnfitriones();
            estadosActividad = controladoraEstados.consultarEstadosActividad();

            DataTable tipoCambio = controladoraGeneral.consultarTipoCambio();
            tipoCambioCompra = Convert.ToInt32(tipoCambio.Rows[0][0]);
            tipoCambioVenta = Convert.ToInt32(tipoCambio.Rows[0][1]);

            //TODO: borrar las instancias de controladorasBD aquí si ya no se volverán a utilizar
        }

        /*
         * ???
         */
        public DataTable consultarEstados()
        {
            return estados;
        }

        /*
         * Retorna un subgrupo de los posibles estados: "Activo" e "Inactivo".
         */
        public DataTable consultarEstadosActividad()
        {
            return estadosActividad;
        }

        /*
         * ???
         */
        public DataTable consultarUnidades()
        {
            return unidades;
        }

        /*
         * ???
         */
        public DataTable consultarEstaciones()
        {
            return estaciones;
        }

        /*
         * ???
         */
        public DataTable consultarAnfitriones()
        {
            return anfitrionas;
        }

        /*
         * ???
         */
        public DataTable consultarIntenciones()
        {
            return intenciones;
        }

        /*
         * ???
         */
        public String traduccionEstado(int valor)
        {
            return estados.Rows[valor-1][0].ToString();
        }

        /*
         * ???
         */
        public int impuestoVentas()
        {
            return impuesto;
        }

        /*
         * ???
         */
        public int dolarCompra()
        {
            return tipoCambioCompra;
        }

        /*
         * ???
         */
        public int dolarVenta()
        {
            return tipoCambioVenta;
        }
    }
}