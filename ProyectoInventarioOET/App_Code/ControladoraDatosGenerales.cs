using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoInventarioOET.Modulo_Bodegas;
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
        private DataTable booleanos;                                            // Para construir dropdownlist booleanos
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

            // Crear la tabla de booleanos
            booleanos = new DataTable();
            booleanos.Columns.Add("Valor", typeof(int));
            booleanos.Columns.Add("Representacion",typeof(String));
            booleanos.Rows.Add(0,"No");
            booleanos.Rows.Add(1,"Si");

            //TODO: borrar las instancias de controladorasBD aquí si ya no se volverán a utilizar
        }

        /*
         * Retorna la lista de estados posibles
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
         * Retorna la lista completa de unidades métricas que puede tener un producto
         */
        public DataTable consultarUnidades()
        {
            return unidades;
        }

        /*
         * Retorna la lista de estaciones de la OET
         */
        public DataTable consultarEstaciones()
        {
            return estaciones;
        }

        /*
         * Retorna la lista de compañías con que utilizan el sistema
         */
        public DataTable consultarAnfitriones()
        {
            return anfitrionas;
        }

        /*
         * Retorna la lista de intenciones de uso de una bodega
         * Intenciones de uso siendo si esta es una bodega de un punto de venta, o de almacenamiento
         */
        public DataTable consultarIntenciones()
        {
            return intenciones;
        }

        public DataTable consultarBooleanos()
        {
            return booleanos;
        }

        /*
         * Traduce el codigo interno de un estado a su nombre
         */
        public String traduccionEstado(int valor)
        {
            return estados.Rows[valor-1][0].ToString();
        }

        /*
         * Retorna el impuesto de ventas actual
         */
        public int impuestoVentas()
        {
            return impuesto;
        }

        /*
         * Retorna el precio de compra del dolar
         */
        public int dolarCompra()
        {
            return tipoCambioCompra;
        }

        /*
         * Retorna el precio de venta del dolar
         */
        public int dolarVenta()
        {
            return tipoCambioVenta;
        }
    } 
}