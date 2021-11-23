using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Referenciar y Usar
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using libGeneralesBD;


namespace webSib.Clases
{
    public class clsEditorial
    {
        #region "Atributos / Propiedades"
        private string strApp;
        public string Error { private set; get; }
        private string strSQL;
        public int codigo { get; set; }
        public string nombre { get; set; }
        private SqlDataReader myReader;
        #endregion
        #region "Constructor"
        public clsEditorial(string Aplicacion)
        {
            strApp = Aplicacion;
            strSQL = string.Empty;
            Error = string.Empty;
        }
        public clsEditorial(string Aplicacion, int codigo, string nombre)
        {
            strApp = Aplicacion;
            this.strSQL = string.Empty;
            this.codigo = codigo;
            this.nombre = nombre;
            Error = string.Empty;
        }
        #endregion
        #region "Métodos Públicos"
        public bool llenarCombo(DropDownList Combo)
        {
            try
            {
                if (Combo == null)
                {
                    Error = "Error";
                    return false;
                }
                strSQL = "EXEC sp_consultar_Mat_Productor;";
                clsGenerales objGles = new clsGenerales();
                if (!objGles.llenarCombo(strApp, Combo, strSQL, "Clave", "Dato"))
                {
                    Error = objGles.Error;
                    objGles = null;
                    return false;
                }
                objGles = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        private bool validarDatosModificar()
        {
            if (string.IsNullOrEmpty(codigo.ToString()))
            {
                Error = "Falta código";
                return false;
            }
            if (string.IsNullOrEmpty(nombre.ToString()))
            {
                Error = "Falta el nombre";
                return false;
            }
            return true;
        }
        private bool validarDatosIngresar()
        {
            if (string.IsNullOrEmpty(nombre.ToString()))
            {
                Error = "Falta el nombre";
                return false;
            }
            return true;
        }
        private bool Grabar()
        {
            try
            {
                if (string.IsNullOrEmpty(strApp))
                {
                    Error = "Falta el nombre de la aplicación";
                    return false;
                }
                clsGeneralesBD objCnx = new clsGeneralesBD(strApp);
                objCnx.SQL = strSQL;
                if (!objCnx.consultarValorUnico(false))
                {
                    Error = objCnx.Error;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                codigo = Convert.ToInt32(objCnx.vrUnico);
                objCnx.cerrarCnx();
                objCnx = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool Buscar(int c)
        {
            try
            {
                strSQL = "EXEC sp_consultar_Mat_Productor_puntual '" + c + "';";
                clsGeneralesBD objCnx = new clsGeneralesBD(strApp);
                objCnx.SQL = strSQL;
                if (!objCnx.Consultar(false))
                {
                    Error = objCnx.Error;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                myReader = objCnx.dataReaderLleno;
                if (!myReader.HasRows)
                {
                    Error = "No existe registro con id: " + c;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                myReader.Read();
                codigo = myReader.GetInt32(0);
                nombre = myReader.GetString(1);
                myReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool llenarGrid(GridView Grid)
        {
            strSQL = "EXEC sp_consultar_Mat_Productor_general;";
            clsGenerales objGles = new clsGenerales();
            if (!objGles.llenarGrid(strApp, Grid, strSQL))
            {
                Error = objGles.Error;
                objGles = null;
                return false;
            }
            objGles = null;
            return true;
        }
        public bool grabarMaestro()
        {
            if (!validarDatosIngresar())
                return false;
            strSQL = "EXEC sp_ingresar_Mat_productor '" + nombre + "';";
            return Grabar();
        }
        public bool modificarMaestro()
        {
            if (!validarDatosModificar())
                return false;
            strSQL = "EXEC sp_actualizar_productor " + codigo + ", '" + nombre + "';";
            return Grabar();
        }
        #endregion

    }
}