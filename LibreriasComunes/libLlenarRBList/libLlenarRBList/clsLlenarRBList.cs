using System;

//Referenciar y Usar desde ensamblados del Framework, mirar vídeo sobre referenciar esta librería
using System.Web.UI.WebControls;

//Referenciar y usar de la clase compilada en clase anterior
using libGeneralesBD;

namespace libLlenarRBList
{
    public class clsLlenarRBList
    {
        #region "Atributos/Propiedades"
        private string strApp;
        public string SQL { private get; set; }
        public string campoID { private get; set; }
        public string campoTexto { private get; set; }
        public string Error { get; private set; }
        #endregion

        #region "Constructor"
        public clsLlenarRBList(string nombreSitio)
        {
            strApp = nombreSitio;
            SQL = string.Empty;
            campoID = string.Empty;
            campoTexto = string.Empty;
            Error = string.Empty;
        }
        #endregion

        #region "Métodos Privados"
        private bool Validar()
        {
            if (string.IsNullOrEmpty(SQL))
            {
                Error = "Debe definir una instrucción SQL";
                return false;
            }
            if (string.IsNullOrEmpty(campoID))
            {
                Error = "Debe definir el nombre del Campo con la PK(Id)";
                return false;
            }
            if (string.IsNullOrEmpty(campoTexto))
            {
                Error = "Debe definir el nombre del Campo con valores Texto";
                return false;
            }
            return true;
        }
        #endregion

        #region "Métodos Públicos"
        public bool llenarRadioBLWeb(RadioButtonList Generico)
        {
            if (!Validar())
                return false;
            clsGeneralesBD objGralBd = new clsGeneralesBD(strApp);
            try
            {
                objGralBd.SQL = SQL;
                if (!objGralBd.llenarDataSet(false))
                {
                    Error = objGralBd.Error;
                    objGralBd.cerrarCnx();
                    objGralBd = null;
                    return false;
                }
                Generico.DataSource = objGralBd.dataSetLleno.Tables[0];
                Generico.DataValueField = campoID;
                Generico.DataTextField = campoTexto;
                Generico.DataBind();
                objGralBd.cerrarCnx();
                objGralBd = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        #endregion
    }
}