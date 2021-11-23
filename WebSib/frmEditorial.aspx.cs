using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webSib
{
    public partial class frmEditorial : System.Web.UI.Page
    {
        #region "Variables Globales"
        static string codigoUsu, nombreUsu;
        private static string strApp;
        private static int intOpcion;
        private static int strCodigo;
        private static string strNombre;
        #endregion
        #region "Métodos propios"
        private void Mensaje(string Texto)
        {
            this.lblMsj.Text = Texto.Trim();
        }
        private void llenarGridEditorial()
        {
            try
            {
                Clases.clsEditorial objXX = new Clases.clsEditorial(strApp);
                if (!objXX.llenarGrid(this.grvDatosEditorial))
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
                this.txtCodigo.Enabled = true;
                this.txtCodigo.ReadOnly = false;
                this.txtCodigo.Focus();
                this.txtNombre.ReadOnly = true;
                llenarGridEditorial();
            }
        }
        private void Limpiar()
        {
            this.txtCodigo.Text = string.Empty;
            this.txtNombre.Text = string.Empty;
            this.txtCodigo.Enabled = true;
            this.txtCodigo.ReadOnly = false;
            this.txtCodigo.Focus();
            this.txtNombre.ReadOnly = true;
            Mensaje(string.Empty);
        }

        private void Buscar()
        {
            if (!this.txtCodigo.Text.Trim().Equals(""))
            {
                webSib.Clases.clsEditorial objXX = new webSib.Clases.clsEditorial(strApp);
                strCodigo = Convert.ToInt32(this.txtCodigo.Text);
                if (!objXX.Buscar(strCodigo))
                {
                    Limpiar();
                    Mensaje(objXX.Error);
                    objXX = null;
                    return;
                }
                this.txtCodigo.Text = objXX.codigo.ToString();
                this.txtNombre.Text = objXX.nombre.ToString();
                this.mnuOpciones.FindItem("opcModificar").Selectable = true;
                this.txtCodigo.ReadOnly = true;
                objXX = null;
            }
            else
            {
                Mensaje("Debe ingresar el Código a buscar");
            }
            
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
                if (this.txtCodigo.Text.Equals(""))
                {
                    strCodigo = 0;
                }
                else
                {
                    strCodigo = Convert.ToInt32(this.txtCodigo.Text);
                }
                strNombre = this.txtNombre.Text.Trim();
                webSib.Clases.clsEditorial objXX = new webSib.Clases.clsEditorial(strApp, strCodigo, strNombre);
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
                llenarGridEditorial();
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
            this.mnuOpciones.FindItem("opcModificar").Selectable = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Grabar();
            intOpcion = 0;
            Limpiar();
            this.btnGuardar.Visible = false;
            this.btnCancelar.Visible = false;
            this.mnuOpciones.FindItem("opcModificar").Selectable = false;
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
                    this.txtCodigo.ReadOnly = true;
                    this.txtCodigo.Focus();
                    this.txtNombre.ReadOnly = false;
                    this.mnuOpciones.FindItem("opcModificar").Selectable = false;
                    break;
                case "opcModificar":
                    intOpcion = 2;
                    this.btnGuardar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.txtCodigo.ReadOnly = true;
                    this.txtNombre.ReadOnly = false;
                    this.txtNombre.Focus();
                    break;
                case "opcBuscar":
                    intOpcion = 0;
                    Buscar();
                    this.txtCodigo.Focus();
                    this.txtNombre.ReadOnly = true;
                    break;
                case "opcLimpiar":
                    Limpiar();
                    if (intOpcion == 1 || intOpcion == 2)
                    {
                        Limpiar();
                    }
                    intOpcion = 0;
                    this.mnuOpciones.FindItem("opcModificar").Selectable = false;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}