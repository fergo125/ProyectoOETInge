using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoInventarioOET.Modulo_Seguridad;
using ProyectoInventarioOET.Modulo_Bodegas;

namespace ProyectoInventarioOET
{
    public partial class FormBodegaLocal : System.Web.UI.Page
    {
        private static ControladoraBodegas controladoraBodegas;                 // Controladora de Bodegas
        private static ControladoraSeguridad controladoraSeguridad;             // Controladora para obtener datos de usuario
        private static Object[] idArray;                                        // Arreglo de ID's de bodega

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mensajeAlerta.Visible = false;
                controladoraSeguridad = new ControladoraSeguridad();
                controladoraBodegas = new ControladoraBodegas();
                controladoraSeguridad.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
                controladoraBodegas.NombreUsuarioLogueado = (this.Master as SiteMaster).Usuario.Usuario;
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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        }

        // Carga las bodegas que puede seleccionar el usuario
        void cargarBodegas()
        {
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

        // Guarda la bodega seleccionada para utiliza localmente
        protected void EnviarBodegaLocal_Click(object sender, EventArgs e)
        {
            if (idArray != null)
            {
                (this.Master as SiteMaster).LlaveBodegaSesion = idArray[DropDownListBodega.SelectedIndex].ToString();
                (this.Master as SiteMaster).NombreBodegaSesion = DropDownListBodega.SelectedValue.ToString();
            }
            Response.Redirect("Default.aspx");
        }

        protected void CerrarSesion_Click(object sender, EventArgs e)
        {
            (this.Master as SiteMaster).cerrarSesion(null,null);
            Response.Redirect("Default.aspx");
        }

        // Aceptar cambio de contraseña
        protected void botonAceptarModalCambiar_Click(object sender, EventArgs e)
        {
            EntidadUsuario usuario = controladoraSeguridad.consultarUsuario((this.Master as SiteMaster).Usuario.Usuario,inputActual.Text.ToString());
            String[] mensaje = new String[3];
            mensaje[0] = "warning";
            if (usuario != null)
            {
                if(controladoraSeguridad.contrasenaEsValida(inputNueva.Text.ToString())){
                    if (inputNueva.Text.ToString().Equals(inputNuevaConfirmacion.Text.ToString()))
                    {
                        mensaje = controladoraSeguridad.modificarContrasena((this.Master as SiteMaster).Usuario.Codigo,inputNueva.Text.ToString());
                    }
                    else
                    {
                        mensaje[1] = "Error";
                        mensaje[2] = "La contraseña nueva y su confirmación no son idénticas";
                    }
                }
                else
                {
                    mensaje[1] = "Alerta";
                    mensaje[2] = "Contraseña nueva inválida, debe contener al menos una mayúscula, una minúscula, un número y tener un tamaño mínimo de 8 caracteres";
                }
            }
            else
            {
                mensaje[1] = "Alerta";
                mensaje[2] = "Contraseña de usuario incorrecta";
            }
            mostrarMensaje(mensaje[0],mensaje[1],mensaje[2]);
        }

    }
}