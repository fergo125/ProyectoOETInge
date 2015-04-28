using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventarioOET.Módulo_Seguridad
{
    /*
     * Entidad Usuario, clase encargada de encapsulador la información acerca de cuentas de usuario
     */
    public class EntidadUsuario
    {
        //Atributos
        private String codigo;              //???
        private String usuario;             //???
        private String clave;               //???
        private String descripcion;         //???
        private String idEstacion;          //???
        private String anfitriona;          //???
        private String nombre;              //???
        private String perfil;              //???
        private String codigoPerfil;       //???
        private DateTime fechaCreacion;     //???
        private int estado;                 //???

        /*
         * Constructor.
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
         * ???
         */
        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        /*
         * ???
         */
        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        /*
         * ???
         */
        public String Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        /*
         * ???
         */
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        /*
         * ???
         */
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        /*
         * ???
         */
        public String IdEstacion
        {
            get { return idEstacion; }
            set { idEstacion = value; }
        }

        /*
         * ???
         */
        public String Anfitriona
        {
            get { return anfitriona; }
            set { anfitriona = value; }
        }

        /*
         * ???
         */
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        /*
         * ???
         */
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        /*
         * ???
         */
        public String Perfil
        {
            get { return perfil; }
            set { perfil = value; }
        }
        
        /*
         * ???
         */
        public String CodigoPerfil
        {
            get { return codigoPerfil; }
            set { codigoPerfil = value; }
        }
    }
}