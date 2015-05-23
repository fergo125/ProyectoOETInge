using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Módulo_Seguridad;
using ProyectoInventarioOET.Módulo_Bodegas;

namespace ProyectoInventarioOET
{
    public partial class FormBodegaLocal : System.Web.UI.Page
    {
        private static ControladoraBodegas controladoraBodegas;                 // Controladora de Bodegas
        private static Object[] idArray;                                        // Arreglo de ID's de bodega

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mensajeAlerta.Visible = false;

                controladoraBodegas = new ControladoraBodegas();
                cargarBodegas();

            }
        }

        // Muestra el mensaje en pantalla
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje)
        {

            mensajeAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            mensajeAlerta.Visible = true;
        }

        // Carga las bodegas que puede seleccionar el usuario
        void cargarBodegas(){
             try
                {
                    Object[] datos = new Object[4];
                    EntidadUsuario usuarioActual = (this.Master as SiteMaster).Usuario;
                    String idUsuario = usuarioActual.Codigo;
                    String rol = usuarioActual.Perfil;
                    DataTable bodegas = controladoraBodegas.consultarBodegas(idUsuario,rol);
                    int i = 0;
                    if (bodegas.Rows.Count > 0)
                    {
                        idArray = new Object[bodegas.Rows.Count];
                        DropDownListBodega.Items.Clear();
                        foreach (DataRow fila in bodegas.Rows)
                        {
                            idArray[i] = fila[0];
                            datos[0] = fila[1].ToString();
                            DropDownListBodega.Items.Add(datos[0].ToString());
                            i++;
                        }
                    }
                    else
                    {
                        mostrarMensaje("warning", "Atención: ", "No existen bodegas en la base de datos.");
                    }
                }
                catch (Exception e)
                {
                    mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
                }
        }
    }
}