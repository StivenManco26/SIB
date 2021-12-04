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
    public class clsEstadoReserva
    {
        #region "Atributos / Propiedades"
        private string strApp;
        public string Error { private set; get; }
        private string strSQL;
        public string codigo { get; set; }
        public string documento { get; set; }
        public int estado { get; set; }
        public DateTime fecha { get; set; }
        private SqlDataReader myReader;
        #endregion
        #region "Constructor"
        public clsEstadoReserva(string Aplicacion)
        {
            strApp = Aplicacion;
            strSQL = string.Empty;
            Error = string.Empty;
        }
        public clsEstadoReserva(string Aplicacion, string codigo, string documento, int estado, DateTime fecha)
        {
            strApp = Aplicacion;
            this.strSQL = string.Empty;
            this.codigo = codigo;
            this.documento = documento;
            this.estado = estado;
            this.fecha = fecha;
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
                strSQL = "EXEC sp_consultar_Reserva_estado;";
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
            if (fecha < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                Error = "Debe ingresar una fecha igual o posterior a la actual";
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
                    Error = "Ingresado correctamente";
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                codigo = Convert.ToString(objCnx.vrUnico);
                objCnx.cerrarCnx();
                if (!codigo.Equals("1"))
                {
                    validarRespuestaBD(objCnx.vrUnico);
                    return false;
                }
                Error = "Ingresado correctamente";
                objCnx = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public void validarRespuestaBD(Object vrUnico)
        {
            int valorUnico = Convert.ToInt32(vrUnico.ToString());
            switch (valorUnico)
            {
                case 0:
                    Error = "Error al ingresar";
                    break;
                case 1:
                    Error = "Ingresado correctamente";
                    break;
                case 2:
                    Error = "El documento ingresado no existe";
                    break;
                case 3:
                    Error = "El material ingresado no existe";
                    break;
                case 4:
                    Error = "El usuario excede la cantidad de prestamos permitidos";
                    break;
                case 5:
                    Error = "El material se encuentra reservado para la fecha ingresada";
                    break;
                case 6:
                    Error = "La fecha no es válida";
                    break;
                case 7:
                    Error = "La fecha no es válida";
                    break;
            }
        }
        public bool grabarMaestro()
        {
            if (!validarDatos())
                return false;
            strSQL = "EXEC sp_ingresar_Reserva '" + codigo + "', '" + documento + "', "
                + estado + ", '" + fecha.ToString("yyyy-MM-dd") + "';";
            return Grabar();
        }
        public bool modificarMaestro()
        {
            strSQL = "EXEC sp_cancelar_Reserva '" + codigo + "', '" + documento + "';";
            return Grabar();
        }
        #endregion

    }
}