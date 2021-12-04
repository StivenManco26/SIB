using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webSib
{
    public partial class frmReserva : System.Web.UI.Page
    {
        #region "Variables Globales"
        static string codigoUsu, nombreUsu;
        private static string strApp;
        private static int intOpcion;
        private static string strCodigo;
        private static string strDocumento;
        private static int intEstado;
        private static DateTime dttmFecha;
        #endregion
        #region "Métodos propios"

        private void Mensaje(string Texto)
        {
            this.lblMsj.Text = Texto.Trim();
        }

        private void llenarComboEstado()
        {
            try
            {
                webSib.Clases.clsEstadoReserva objXX = new webSib.Clases.clsEstadoReserva(strApp);
                if (!objXX.llenarCombo(this.ddlEstadoReserva))
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
                llenarComboEstado();
                this.ddlEstadoReserva.SelectedIndex = 0;
                this.txtCodigo.Enabled = false;
                this.txtCodigo.ReadOnly = true;
                this.txtCodigo.Focus();
                this.txtDocumento.Enabled = false;
                this.txtDocumento.ReadOnly = true;
                this.ddlEstadoReserva.Enabled = false;
                this.txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        private void Limpiar()
        {

            this.txtCodigo.Text = string.Empty;
            this.txtDocumento.Text = string.Empty;
            this.ddlEstadoReserva.SelectedIndex = 0;
            this.txtCodigo.Enabled = false;
            this.txtCodigo.ReadOnly = true;
            this.txtCodigo.Focus();
            this.txtDocumento.ReadOnly = true;
            this.ddlEstadoReserva.Enabled = false;
            this.txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
                strCodigo = this.txtCodigo.Text.Trim();
                strDocumento = this.txtDocumento.Text.Trim();
                dttmFecha = Convert.ToDateTime(this.txtFecha.Text.Trim());
                intEstado = this.ddlEstadoReserva.SelectedIndex + 1;
                webSib.Clases.clsEstadoReserva objXX = new webSib.Clases.clsEstadoReserva(strApp, strCodigo, strDocumento, 
                    intEstado, dttmFecha);
                if (intOpcion == 1) // Agregar
                {
                    if (!objXX.grabarMaestro())
                    {
                        Mensaje(objXX.Error);
                        objXX = null;
                        return;
                    }
                    strCodigo = objXX.codigo;
                }
                else // Modificar
                if (!objXX.modificarMaestro())
                {
                    Mensaje(objXX.Error);
                    return;
                }
                strCodigo = objXX.codigo;
                objXX = null;
                if (strCodigo.Equals("0"))
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
                    this.txtCodigo.ReadOnly = false;
                    this.txtCodigo.Enabled = true;
                    this.txtCodigo.Focus();
                    this.txtDocumento.ReadOnly = false;
                    this.txtDocumento.Enabled = true;
                    this.ddlEstadoReserva.SelectedIndex = 0;
                    this.ddlEstadoReserva.Enabled = false;

                    break;
                case "opcCancelar":
                    intOpcion = 2;
                    this.btnGuardar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.txtCodigo.ReadOnly = false;
                    this.txtCodigo.Enabled = true;
                    this.txtCodigo.Focus();
                    this.txtDocumento.ReadOnly = false;
                    this.txtDocumento.Enabled = true;
                    this.ddlEstadoReserva.SelectedIndex = 1;
                    this.ddlEstadoReserva.Enabled = false;
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