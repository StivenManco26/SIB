using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace webSib
{
    public partial class Formulario_web23 : System.Web.UI.Page
    {
        #region "Variables Globales"
        static string codigoUsu, nombreUsu;
        private static string strApp;
        private static int intOpcion;
        private static string strCodigoLibro;
        private static string strNit;
        private static int intNit;
        private static int intIdReserva;
        private static int intCantPrestamo;
        private DateTime FechaDevolucion;
        private DateTime FechaDe;
        private static int intIdEstadoMaterial;
        private static string strEstadoMaterial;
        private static DateTime Nuevafecha;
        #endregion

        #region "Métodos propios"

        private void Mensaje(string Texto)
        {
            this.lblMsj.Text = Texto.Trim();
        }

        private void llenarComboMaterialEstado()
        {
            try
            {
                WebSib.Clases.clsEstadoMaterial objXX = new WebSib.Clases.clsEstadoMaterial(strApp);
                if (!objXX.llenarCombo(this.ddlMaterialEstado))
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
                llenarComboMaterialEstado();
                this.ddlMaterialEstado.SelectedIndex = 0;
                lblHoraPres.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblFechaDevo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblFechaRegistro.Text = DateTime.Now.ToString("dd/MM/yyyy");             
                //llenarGridProductos();
            }
        }
        private void Limpiar()
        {
            this.txtCodigoLibro.Text = string.Empty;
            this.txtNit.Text = string.Empty;
            this.ddlMaterialEstado.SelectedIndex = 0;
            this.lblFechaRegistro.Text = string.Empty;
            this.lblFechaDevo.Text = string.Empty;
            Mensaje(string.Empty);
        }
        private void LimpiarGrid()
        {
            grvDatosPrestamo.Visible = false;
            Mensaje(string.Empty);
        }


        private void BuscarPersonas()
        {
            webSib.Clases.clsPrestamo objXX = new webSib.Clases.clsPrestamo(strApp);
            strNit = this.txtNit.Text;
            if (!objXX.BuscarPersona(strNit))
            {
                Limpiar();
                Mensaje(objXX.Error);
                objXX = null;
                return;
            }
            this.txtNombre.Text = objXX.Nombre.ToString();
            objXX = null;
        }

        private void BuscarLibro()
        {
            webSib.Clases.clsPrestamo objXX = new webSib.Clases.clsPrestamo(strApp);
            strCodigoLibro = this.txtCodigoLibro.Text;
            if (!objXX.BuscarNombreLibro(strCodigoLibro))
            {
                Limpiar();
                Mensaje(objXX.Error);
                objXX = null;
                return;
            }
            this.lblNombreLibro.Text = objXX.Material.ToString();
            objXX = null;
        }

        private void DiasPrestamo()
        {
            webSib.Clases.clsPrestamo objXX = new webSib.Clases.clsPrestamo(strApp);
            strNit = this.txtNit.Text;
            if (!objXX.BuscarPerfilPersona(strNit))
            {
                Limpiar();
                Mensaje(objXX.Error);
                objXX = null;
                return;
            }
            this.lblDiasPrestamo.Text = objXX.DiasPrestamo.ToString();
            this.lblMaxCantPrestamo.Text = objXX.MaxCantPres.ToString();
            objXX = null;
        }

        private void BuscarPrestamo()
        {

            grvDatosPrestamo.Visible = true;
            if (intOpcion == 2)
                this.grvDatosPrestamo.Columns[0].Visible = true;
            webSib.Clases.clsPrestamo objXX = new webSib.Clases.clsPrestamo(strApp);
            intNit = Convert.ToInt32( this.txtNit.Text);
            if(!objXX.BuscarPrestamo(intNit, this.grvDatosPrestamo))
            {
                Mensaje(objXX.Error);
                objXX = null;
                return;
            }
            lblMsj.Text = string.Empty;

            objXX = null;
        }
        private void BuscarCantidad()
        {
            webSib.Clases.clsPrestamo objXX = new webSib.Clases.clsPrestamo(strApp);
            strNit = this.txtNit.Text;
            if (!objXX.BuscarCantidad(strNit))
            {
                Limpiar();
                Mensaje(objXX.Error);
                objXX = null;
                return;
            }
            intCantPrestamo = objXX.CantPrestamo;
            objXX = null;
        }

        private void IngresarPrestamo()
        {
            LimpiarGrid();
            try
            {
                if (intOpcion != 0 && intOpcion != 1)
                {
                    Mensaje("Opción no válida");
                    return;
                }
                DateTime FechaDe = DateTime.Now;
                FechaDevolucion.Date.ToShortTimeString();
                strNit = this.txtNit.Text;
                strCodigoLibro = this.txtCodigoLibro.Text;
                intIdEstadoMaterial = this.ddlMaterialEstado.SelectedIndex+1;
                intIdReserva = 1;                
                FechaDevolucion = Convert.ToDateTime(lblFechaDevo.Text.ToString());
                webSib.Clases.clsPrestamo objXX = new webSib.Clases.clsPrestamo(strApp, strCodigoLibro,
                    intIdEstadoMaterial, strNit, intIdReserva, FechaDevolucion= Convert.ToDateTime(lblFechaDevo.Text.ToString()));
                BuscarCantidad();           
                if(intCantPrestamo>=Convert.ToInt32(lblMaxCantPrestamo.Text))
                {
                    Mensaje("Error, El usuario llegó al maximo de prestamos permitidos");                    
                    return;
                }
                if (intOpcion == 0) // Agregar
                {
                    if (!objXX.grabarPrestamo())
                    {
                        Mensaje(objXX.Error);
                        objXX = null;
                        return;
                    }
                    strNit = objXX.Nit;
                }
                else // Modificar
                if (!objXX.modificarMaestro())
                {
                    Mensaje(objXX.Error);
                    return;
                }
                strNit = objXX.Nit;
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

        protected void tmrHoraActual_Tick(object sender, EventArgs e)
        {

        }

        protected void btnBuscarNombre_Click(object sender, ImageClickEventArgs e)
        {

            BuscarPersonas();
            this.txtNombre.ReadOnly = true;
        }

        protected void btnBuscarLibro_Click(object sender, ImageClickEventArgs e)
        {
            BuscarLibro();

        }

        protected void btnBuscarprestamo_Click(object sender, EventArgs e)
        {
            BuscarPrestamo();
        }

        protected void btnIngresarPrestamo_Click(object sender, EventArgs e)
        {
            IngresarPrestamo();
        }

        protected void mnuOpciones_MenuItemClick1(object sender, MenuEventArgs e)
        {
            Mensaje(string.Empty);
            switch (this.mnuOpciones.SelectedValue)
            {
                case "opcAgregar":
                    intOpcion = 0;
                    DiasPrestamo();
                    Nuevafecha = Convert.ToDateTime(lblFechaDevo.Text);
                    lblFechaDevo.Text = Nuevafecha.AddDays(Convert.ToInt32(lblDiasPrestamo.Text)).ToString("dd/MM/yyyy");

                    break;
                case "opcLimpiarGrid":
                    intOpcion = 1;
                    LimpiarGrid();

                    break;
                ///*case "opcGrabar":
                //    Grabar();
                //    intOpcion = 0;
                //    Limpiar();
                //    break;*/
                //case "opcCancelar":
                //    Limpiar();
                //    if (intOpcion == 1 || intOpcion == 2)
                //    {
                //        Limpiar();
                //    }
                //    intOpcion = 0;
                //    break;
                default:
                        break;
            }
        }
        #endregion
    }
}