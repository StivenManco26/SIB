using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webSib
{
    public partial class frmDevolucion: System.Web.UI.Page
    {
        #region "Variables Globales"
        static string codigoUsu, nombreUsu;
        private static string strApp;
        private static int intOpcion;
        private static string strCodigo;
        private static string strDocumento;
        private static int intEstado;
        private static string strNota;
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
                webSib.Clases.clsEstado objXX = new webSib.Clases.clsEstado(strApp);
                if (!objXX.llenarCombo(this.ddlEstado))
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
                this.ddlEstado.SelectedIndex = 0;
                this.txtCodigo.Enabled = false;
                this.txtCodigo.ReadOnly = true;
                this.txtCodigo.Focus();
                this.txtDocumento.Enabled = false;
                this.txtDocumento.ReadOnly = true;
                this.txtNota.Enabled = false;
                this.txtNota.ReadOnly = true;
            }
        }
        private void Limpiar()
        {
            this.txtCodigo.Text = string.Empty;
            this.txtDocumento.Text = string.Empty;
            this.txtNota.Text = string.Empty;
            this.ddlEstado.SelectedIndex = 0;
            this.txtCodigo.Enabled = false;
            this.txtCodigo.ReadOnly = true;
            this.txtCodigo.Focus();
            this.txtDocumento.ReadOnly = true;
            this.ddlEstado.Enabled = false;
            this.txtNota.ReadOnly = true;
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
                strNota = this.txtNota.Text.Trim();
                intEstado = this.ddlEstado.SelectedIndex + 1;
                webSib.Clases.clsDevolucion objXX = new webSib.Clases.clsDevolucion(strApp, strCodigo, strDocumento, 
                    intEstado, strNota);
                if (intOpcion == 1)
                {
                    if (!objXX.grabarMaestro())
                    {
                        Mensaje(objXX.Error);
                        objXX = null;
                        return;
                    }
                }
                strCodigo = objXX.codigo;
                if (strCodigo.Equals("0"))
                {
                    objXX = null;
                    Mensaje("Error al procesar registro, Consultar con el administrador del sistema");
                    return;
                }
                Mensaje("Devolución exitosa. Valor a pagar por deterioro o mora: $" + objXX.monto.Split(',')[0]);
                objXX = null;
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
                case "opcDevolver":
                    intOpcion = 1;
                    Limpiar();
                    this.btnGuardar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.txtCodigo.ReadOnly = false;
                    this.txtCodigo.Enabled = true;
                    this.txtCodigo.Focus();
                    this.txtDocumento.ReadOnly = false;
                    this.txtDocumento.Enabled = true;
                    this.ddlEstado.Enabled = true;
                    this.txtNota.ReadOnly = false;
                    this.txtNota.Enabled = true;
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