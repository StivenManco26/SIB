using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webSib
{
    public partial class Formulario_web22 : System.Web.UI.Page
    {
        #region "Variables Globales"
        static string codigoUsu, nombreUsu;
        private static string strApp;
        private static int intOpcion;
        private static string strNit;
        private static string strNombre;
        private static string strCorreo;
        private static string strCelular;
        private static int intPerfil;
        private static string strUsuario;
        private static string strContrasena;

        #endregion

        #region "Métodos propios"

        private void Mensaje(string Texto)
        {
            this.lblMsj.Text = Texto.Trim();
        }

        private void llenarComboPerfil()
        {
            try
            {
                webSib.Clases.clsPerfil objXX = new webSib.Clases.clsPerfil(strApp);
                if (!objXX.llenarCombo(this.ddlPerfil))
                {
                    Mensaje(objXX.Error);
                    objXX = null;
                    return;
                }
                objXX = null;
            }
            catch (Exception ex)
            {
                Mensaje(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                codigoUsu = Session["nit"].ToString();
                if (string.IsNullOrEmpty(codigoUsu))
                    Response.Redirect("frmLogin.aspx");
                nombreUsu = Session["usuario"].ToString();
                this.lblUsu.Text = "Usuario: " + nombreUsu;
                strApp = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                intOpcion = 0;
                llenarComboPerfil();
                this.ddlPerfil.SelectedIndex = 0;
                //llenarGridProductos();
            }
        }
        private void Limpiar()
        {
            this.txtNit.Text = string.Empty;
            this.txtNombre.Text = string.Empty;
            this.txtCorreo.Text = string.Empty;
            this.txtCelular.Text = string.Empty;
            this.ddlPerfil.SelectedIndex = 0;
            this.txtUsuario.Text = string.Empty;
            this.txtContrasena.Text = string.Empty;
            Mensaje(string.Empty);
        }

        private void Buscar()
        {
            webSib.Clases.clsUsuario objXX = new webSib.Clases.clsUsuario(strApp);
            strNit = this.txtNit.Text;
            if (!objXX.Buscar(strNit))
            {
                Limpiar();
                Mensaje(objXX.Error);
                objXX = null;
                return;
            }
            this.txtNit.Text = objXX.nit.ToString();
            this.txtNombre.Text = objXX.nombre.ToString();
            this.txtCorreo.Text = objXX.correo.ToString();
            this.txtCelular.Text = objXX.celular.ToString();
            this.ddlPerfil.SelectedIndex = objXX.perfil - 1;
            this.txtUsuario.Text = string.Empty;
            this.txtContrasena.Text = string.Empty;
            objXX = null;
        }
        private void Grabar()
        {
            try
            {
                if (intOpcion != 1 && intOpcion != 2)
                {
                    Mensaje("Opción no válida");
                    return;
                }
                strNit = this.txtNit.Text;
                //strNit = (intOpcion == 1) ? "" : this.txtNit.Text;
                strNombre = this.txtNombre.Text.Trim();
                strCorreo = this.txtCorreo.Text.Trim();
                strCelular = this.txtCelular.Text.Trim();
                intPerfil = this.ddlPerfil.SelectedIndex + 1;
                strUsuario = this.txtUsuario.Text.Trim();
                strContrasena = this.txtContrasena.Text.Trim();
                webSib.Clases.clsUsuario objXX = new webSib.Clases.clsUsuario(strApp, strNit, strNombre, 
                    strCorreo, strCelular, intPerfil, strUsuario, strContrasena);
                if (intOpcion == 1) // Agregar
                {
                    if (!objXX.grabarMaestro())
                    {
                        Mensaje(objXX.Error);
                        objXX = null;
                        return;
                    }
                    strNit = objXX.nit;
                }
                else // Modificar
                if (!objXX.modificarMaestro())
                {
                    Mensaje(objXX.Error);
                    return;
                }
                strNit = objXX.nit;
                objXX = null;
                if (strNit.Equals("0"))
                {
                    Mensaje("Error al procesar registro, Consultar con el administrador del sistema");
                    return;
                }
                Mensaje("Registro Grabado con éxito");
            }
            catch (Exception ex)
            {
                Mensaje(ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
            if (intOpcion == 1 || intOpcion == 2)
            {
                Limpiar();
            }
            intOpcion = 0;
            this.btnGuardar.Visible = false;
            this.btnCancelar.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Grabar();
            intOpcion = 0;
            Limpiar();
            this.btnGuardar.Visible = false;
            this.btnCancelar.Visible = false;
        }

        protected void mnuOpciones_MenuItemClick1(object sender, MenuEventArgs e)
        {
            Mensaje(string.Empty);
            switch (this.mnuOpciones.SelectedValue)
            {
                case "opcAgregar":
                    intOpcion = 1;
                    Limpiar();
                    this.btnGuardar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.txtNit.ReadOnly = false;
                    this.txtNit.Focus();
                    this.txtNombre.ReadOnly = false;
                    this.txtCorreo.ReadOnly = false;
                    this.txtCelular.ReadOnly = false;
                    this.txtUsuario.ReadOnly = false;
                    this.txtContrasena.ReadOnly = false;
                    this.ddlPerfil.Enabled = true;
                    break;
                case "opcModificar":
                    intOpcion = 2;
                    Limpiar();
                    this.btnGuardar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.txtNit.ReadOnly = false;
                    this.txtNombre.ReadOnly = false;
                    this.txtNombre.Focus();
                    this.txtCorreo.ReadOnly = false;
                    this.txtCelular.ReadOnly = false;
                    this.txtUsuario.ReadOnly = true;
                    this.txtContrasena.ReadOnly = true;
                    this.ddlPerfil.Enabled = true;
                    break;
                case "opcBuscar":
                    intOpcion = 0;
                    Buscar();
                    this.txtNit.Enabled = true;
                    this.txtNit.ReadOnly = false;
                    this.txtNit.Focus();
                    this.txtNombre.ReadOnly = true;
                    this.txtCorreo.ReadOnly = true;
                    this.txtCelular.ReadOnly = true;
                    this.txtUsuario.ReadOnly = true;
                    this.txtContrasena.ReadOnly = true;
                    this.ddlPerfil.Enabled = false;
                    break;
                case "opcLimpiar":
                    Limpiar();
                    if (intOpcion == 1 || intOpcion == 2)
                    {
                        Limpiar();
                    }
                    intOpcion = 0;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}