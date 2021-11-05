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

        protected void mnuOpciones_MenuItemClick1(object sender, MenuEventArgs e)
        {
            Mensaje(string.Empty);
            switch (this.mnuOpciones.SelectedValue)
            {
                case "opcAgregar":
                    intOpcion = 1;
                    Limpiar();
                    this.txtNit.ReadOnly = true;
                    this.txtNombre.ReadOnly = false;
                    this.txtNombre.Focus();
                    this.txtCorreo.ReadOnly = false;
                    this.txtCelular.ReadOnly = false;
                    this.txtUsuario.ReadOnly = false;
                    this.txtContrasena.ReadOnly = false;
                    this.ddlPerfil.Enabled = true;
                    break;
                /*case "opcModificar":
                    intOpcion = 2;
                    this.imgButtonBuscar.Visible = false;
                    this.txtCodigo.ReadOnly = true;
                    this.txtDescripcion.ReadOnly = false;
                    this.txtDescripcion.Focus();
                    this.txtValorUn.ReadOnly = false;
                    this.txtIva.ReadOnly = false;
                    this.ddlClasificacion.Enabled = true;
                    break;*/
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
                /*case "opcGrabar":
                    Grabar();
                    intOpcion = 0;
                    Limpiar();
                    break;*/
                case "opcCancelar":
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