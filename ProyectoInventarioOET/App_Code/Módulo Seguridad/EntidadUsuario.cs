using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Modulo_Seguridad
{
    /*
     * Entidad Usuario, clase encargada de encapsulador la información acerca de cuentas de usuario
     */
    public class EntidadUsuario
    {
        //Atributos
        private String codigo;              // Codigo interno de la base de datos
        private String usuario;             // Nombre de usuario, utilizado para iniciar sesión
        private String clave;               // Contraseña de seguridad, para inicio de sesión y cambios de perfil
        private String descripcion;         // Rol (Opcional) de una persona dentro de la OET
        private String idEstacion;          // Estación en la que trabaja, importante si es de permisos limitados
        private String anfitriona;          // Compañía para la que trabaja, importante si es de permisos limitados
        private String nombre;              // Nombre real de la persona
        private String perfil;              // Perfil de permisos de la persona
        private String llavePerfil;         // Llave de BD del perfil usada para averiguar permisos
        private String codigoPerfil;        // Código interno de dicho perfil usado para operaciones de interfaz
        private DateTime fechaCreacion;     // Fecha en la que el usuario fue creado
        private int estado;                 // Estado de la cuenta, una cuenta desactivada no debería poder utilizarse

        /*
         * Constructor de la clase
         * Este método toma datos y los encapsula en la Entidad Usuario
         */
        public EntidadUsuario(Object[] datos)
        {
            this.codigo = datos[0].ToString();
            this.usuario = datos[1].ToString();
            this.clave = datos[2].ToString();
            this.fechaCreacion = Convert.ToDateTime(datos[3].ToString());
            this.descripcion = datos[4].ToString();
            this.idEstacion = datos[5].ToString();
            this.anfitriona = datos[6].ToString();
            this.nombre = datos[7].ToString();
            this.estado = Convert.ToInt32(datos[8].ToString());
        }

        /*
         * Métodos de acceso a datos
         * Permiten obtener o manipular los atributos encapsulados en Entidad Usuario
         * Cada método se refiere al atributo del mismo nombre
         */
        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public String Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public String IdEstacion
        {
            get { return idEstacion; }
            set { idEstacion = value; }
        }

        public String Anfitriona
        {
            get { return anfitriona; }
            set { anfitriona = value; }
        }

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public String Perfil
        {
            get { return perfil; }
            set { perfil = value; }
        }

        public String LlavePerfil
        {
            get { return llavePerfil; }
            set { llavePerfil = value; }
        }

        public String CodigoPerfil
        {
            get { return codigoPerfil; }
            set { codigoPerfil = value; }
        }

        // Fin de metodos de acceso a datos
    }
}