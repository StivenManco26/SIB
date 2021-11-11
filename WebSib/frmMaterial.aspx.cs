using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webSib
{
    public partial class frmMaterial1 : System.Web.UI.Page
    {
        #region "Variables Globales"
        static string codigoUsu, nombreUsu;
        private static string strApp;
        private static int intOpcion;
        private static string strCodigo;
        private static string strNombre;
        private static string strEdicion;
        private static int intCantidad;
        private static int intEstado;
        private static int intAutor;
        private static int intEditorial;

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
        private void llenarComboAutor()
        {
            try
            {
                webSib.Clases.clsAutor objXX = new webSib.Clases.clsAutor(strApp);
                if (!objXX.llenarCombo(this.ddlAutor))
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
        private void llenarComboEditorial()
        {
            try
            {
                webSib.Clases.clsEditorial objXX = new webSib.Clases.clsEditorial(strApp);
                if (!objXX.llenarCombo(this.ddlEditorial))
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
        private void llenarGridMaterial()
        {
            try
            {
                Clases.clsMaterial objXX = new Clases.clsMaterial(strApp);
                if (!objXX.llenarGrid(this.grvDatosMaterial))
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
                llenarComboAutor();
                llenarComboEditorial();
                this.ddlEstado.SelectedIndex = 0;
                this.ddlEditorial.SelectedIndex = 0;
                this.ddlAutor.SelectedIndex = 0;
                this.txtCodigo.Enabled = true;
                this.txtCodigo.ReadOnly = false;
                this.txtCodigo.Focus();
                this.txtNombre.ReadOnly = true;
                this.txtEdicion.ReadOnly = true;
                this.txtCantidad.ReadOnly = true;
                this.ddlEstado.Enabled = false;
                this.ddlAutor.Enabled = false;
                this.ddlEditorial.Enabled = false;
                llenarGridMaterial();
            }
        }
        private void Limpiar()
        {
            this.txtCodigo.Text = string.Empty;
            this.txtNombre.Text = string.Empty;
            this.txtEdicion.Text = string.Empty;
            this.txtCantidad.Text = string.Empty;
            this.ddlEstado.SelectedIndex = 0;
            this.ddlAutor.SelectedIndex = 0;
            this.ddlEditorial.SelectedIndex = 0;
            this.txtCodigo.Enabled = true;
            this.txtCodigo.ReadOnly = false;
            this.txtCodigo.Focus();
            this.txtNombre.ReadOnly = true;
            this.txtEdicion.ReadOnly = true;
            this.txtCantidad.ReadOnly = true;
            this.ddlEstado.Enabled = false;
            Mensaje(string.Empty);
        }

        private void Buscar()
        {
            if (!this.txtCodigo.Text.Trim().Equals(""))
            {
                webSib.Clases.clsMaterial objXX = new webSib.Clases.clsMaterial(strApp);
                strCodigo = this.txtCodigo.Text;
                if (!objXX.Buscar(strCodigo))
                {
                    Limpiar();
                    Mensaje(objXX.Error);
                    objXX = null;
                    return;
                }
                this.txtCodigo.Text = objXX.codigo.ToString();
                this.txtNombre.Text = objXX.nombre.ToString();
                this.txtEdicion.Text = objXX.edicion.ToString();
                this.txtCantidad.Text = objXX.cantidad.ToString();
                this.ddlEstado.SelectedIndex = objXX.estado - 1;
                this.ddlAutor.SelectedIndex = objXX.autor - 1;
                this.ddlEditorial.SelectedIndex = objXX.editorial - 1;
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
                strCodigo = this.txtCodigo.Text;
                strNombre = this.txtNombre.Text.Trim();
                strEdicion = this.txtEdicion.Text.Trim();
                intCantidad = Convert.ToInt32(this.txtCantidad.Text.Trim());
                intEstado = this.ddlEstado.SelectedIndex + 1;
                intAutor = this.ddlAutor.SelectedIndex + 1;
                intEditorial = this.ddlEditorial.SelectedIndex + 1;
                webSib.Clases.clsMaterial objXX = new webSib.Clases.clsMaterial(strApp, strCodigo, strNombre, 
                    strEdicion, intCantidad, intEstado, intAutor, intEditorial);
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
                llenarGridMaterial();
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
                    this.txtCodigo.ReadOnly = false;
                    this.txtCodigo.Focus();
                    this.txtNombre.ReadOnly = false;
                    this.txtEdicion.ReadOnly = false;
                    this.txtCantidad.ReadOnly = false;
                    this.ddlEstado.Enabled = true;
                    this.ddlAutor.Enabled = true;
                    this.ddlEditorial.Enabled = true;
                    this.mnuOpciones.FindItem("opcModificar").Selectable = false;
                    break;
                case "opcModificar":
                    intOpcion = 2;
                    this.btnGuardar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.txtCodigo.ReadOnly = true;
                    this.txtNombre.ReadOnly = false;
                    this.txtNombre.Focus();
                    this.txtEdicion.ReadOnly = false;
                    this.txtCantidad.ReadOnly = false;
                    this.ddlEstado.Enabled = true;
                    this.ddlAutor.Enabled = true;
                    this.ddlEditorial.Enabled = true;
                    break;
                case "opcBuscar":
                    intOpcion = 0;
                    Buscar();
                    this.txtCodigo.Enabled = true;
                    this.txtCodigo.ReadOnly = false;
                    this.txtCodigo.Focus();
                    this.txtNombre.ReadOnly = true;
                    this.txtEdicion.ReadOnly = true;
                    this.txtCantidad.ReadOnly = true;
                    this.ddlEstado.Enabled = false;
                    this.ddlAutor.Enabled = false;
                    this.ddlEditorial.Enabled = false;
                    this.mnuOpciones.FindItem("opcModificar").Selectable=true;
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