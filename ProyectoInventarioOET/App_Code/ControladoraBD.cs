using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client; //para conectarse a la base de datos manualmente con strings

namespace ProyectoInventarioOET
{
    /*
     * Clase no instanciable usada sólo para heredar el código necesario para conectarse a la base de datos y para generar
     * el ID de las tuplas de la base de datos.
     */
    public abstract class ControladoraBD
    {
        //Atributos
        protected static OracleConnection conexionBD;   // Atributo estático compartido por todas las ControladorasBD para conectarse
        protected static int consecutivo;               // Contador de # de transacciones ocurridas de inserción de tuplas a la base de datos entre 000 y 999
        protected static String nombreUsuarioLogueado;  // Usado para insertar en la tabla de réplica

        /*
         * Constructor, crea y abre la conexión sólo la primera vez que es necesario.
         */
        public ControladoraBD()
        {
        }

        /*
         * Destructor, cierra la conexión cuando se termina la ejecución general de la sesión.
         */
        ~ControladoraBD()
        {
            //conexionBD.Close(); //por ahora no sirve de nada intentar cerrarla aquí porque los destructores no son invocados
        }

        /*
         * Método usado para generar el ID de las tuplas en la base de datos a partir de la fecha y hora del momento exacto,
         * siguiendo la convención de la OET con la única diferencia de omitir el nombre del servidor que crea este ID.
         */
        protected String generarID()
        {
            consecutivo = (consecutivo + 1) % 999;
            return ""
            + DateTime.Now.Day.ToString("D2")
            + DateTime.Now.Month.ToString("D2")
            + DateTime.Now.Year.ToString()
            + DateTime.Now.Hour.ToString("D2")
            + DateTime.Now.Minute.ToString("D2")
            + DateTime.Now.Second.ToString("D2")
            + DateTime.Now.Millisecond.ToString("D3")
            + consecutivo.ToString("D3");
        }

        /*
         * Método que a partir de cualquier instrucción SQL, abre una conexión a la base de datos, crea el comando a partir de la string SQL y ejecuta el comando.
         * Absolutamente todas las instrucciones SQL pasan por aquí y son ejecutadas aquí, por lo que la inserción en las tablas de réplica se hace aquí.
         */
        protected DataTable ejecutarComandoSQL(String comandoSQL, bool esperaResultado)
        {
            //Primero, intentar abrir la conexión
            conexionBD = new OracleConnection();
            conexionBD.ConnectionString = "Data Source=10.1.4.93;User ID=inventarios;Password=inventarios"; //en el futuro se podría leer esta string desde un archivo
            conexionBD.Open();
            if (conexionBD.State == ConnectionState.Closed) //si no se logró abrir
                return null; //retornar nulo de una vez para que se detecte el error en la invocación

            //Segundo, intentar ejecutar la instrucción SQL
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = comandoSQL;
                insertarReplicaSQL(comandoSQL); //Insertar en la tabla de réplica aquí
                OracleDataReader reader = command.ExecuteReader();
                if (esperaResultado) //Si se trata de una consulta, se debe cargar lo que devuelva, de lo contrario, no
                    resultado.Load(reader);
            }
            catch(Exception e) //error
            {
                resultado = null;
            }
            //Último, siempre cerrar la conexión
            conexionBD.Close();
            return resultado;
        }

        /*
         * Se inserta una réplica de cada comando SQL que se ejecuta en la base de datos.
         */
        protected bool insertarReplicaSQL(String comandoSQL)
        {
            //Primero prepara la consulta para obtener los servidores en los que debe insertar
            String esquema = "Inventarios.";
            String comandoSQLservidores = "SELECT * FROM " + esquema + "SERVIDORES WHERE REPLICAR = 'Y'";
            DataTable resultado = new DataTable();
            try
            {
                OracleCommand command = conexionBD.CreateCommand();
                command.CommandText = comandoSQLservidores;
                OracleDataReader reader = command.ExecuteReader();
                resultado.Load(reader);
                //Luego por cada servidor, inserta una réplica del comando SQL
                foreach (DataRow fila in resultado.Rows)
                {
                    String comandoSQLreplica = "INSERT INTO " + esquema + "REPLICA_SALIDA (TIRA_SQL, ESTADO, FECHA, USUARIO, SERVER) VALUES ("
                        + "'" + comandoSQL + "', "
                        + "'N', "
                        + "'" + DateTime.Now.Date.ToString("dd/MM/yyyy") + "', "
                        + "'" + nombreUsuarioLogueado + "', "
                        + "'" + fila[0] + "'";
                    command = conexionBD.CreateCommand();
                    command.CommandText = comandoSQLreplica;
                    reader = command.ExecuteReader();
                }
                return true; //retorna true si todo salió bien
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /*
         * Setter y getter de nombreUsuarioLogueado.
         */
        public String NombreUsuarioLogueado
        {
            get { return nombreUsuarioLogueado; }
            set { nombreUsuarioLogueado = value; }
        }
    }
}