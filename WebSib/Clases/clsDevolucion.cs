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
    public class clsDevolucion
    {
        #region "Atributos / Propiedades"
        private string strApp;
        public string Error { private set; get; }
        private string strSQL;
        public string codigo { get; set; }
        public string documento { get; set; }
        public string nota { get; set; }
        public string monto { get; set; }
        public int estado { get; set; }
        private SqlDataReader myReader;
        #endregion
        #region "Constructor"
        public clsDevolucion(string Aplicacion)
        {
            strApp = Aplicacion;
            strSQL = string.Empty;
            Error = string.Empty;
        }
        public clsDevolucion(string Aplicacion, string codigo, string documento, int estado, string nota)
        {
            strApp = Aplicacion;
            this.strSQL = string.Empty;
            this.codigo = codigo;
            this.documento = documento;
            this.estado = estado;
            this.nota = nota;
            Error = string.Empty;
        }
        #endregion
        #region "Métodos Públicos"
        private bool validarDatos()
        {
            if (string.IsNullOrEmpty(codigo.ToString()))
            {
                Error = "Falta código";
                return false;
            }
            if (string.IsNullOrEmpty(documento.ToString()))
            {
                Error = "Falta el documento";
                return false;
            }
            if (string.IsNullOrEmpty(nota.ToString()))
            {
                Error = "Falta la nota";
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
                    Error = "Error al procesar la devolución";
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                monto = Convert.ToString(objCnx.vrUnico);
                objCnx.cerrarCnx();
                Error = "Error al procesar la devolución";
                objCnx = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool grabarMaestro()
        {
            if (!validarDatos())
                return false;
            strSQL = "EXEC sp_Ingresar_Devolucion '" + codigo + "', " + estado + ", '"
                + documento + "', '" + nota + "';";
            return Grabar();
        }
        #endregion

    }
}