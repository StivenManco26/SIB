using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Referenciar y usar ensamblados de FrameWorw
using System.Windows.Forms;
using System.Web.UI.WebControls;

//Ferenciar y usar librería propia
using libGeneralesBD;


namespace libLlenarGrids
{
    public class clsLlenarGrids
    {
    #region "Atributos/Propiedades"
        private string strApp;
        public string SQL { private get; set; }
        public string Error { get; private set; }
    #endregion

    #region "Constructor"
        public clsLlenarGrids ( string nombreSitio )
        {
            strApp = nombreSitio;
            SQL = string.Empty;
            Error = string.Empty;
        }
    #endregion

    #region "Métodos Privados"
        private bool Validar ( )
        {
            if (string.IsNullOrEmpty( SQL ) )
            {
                Error = "Debe definir la instrucción SQL";
                return false;
            }
            return true;
        }
    #endregion

    #region "Métodos Públicos"
        public bool llenarGridWindows ( DataGridView Generico )
        {
            if ( ! Validar ( ) )
                return false;

            try
            {
                clsGeneralesBD objGralBd = new clsGeneralesBD(strApp);
                objGralBd.SQL = SQL;

                if ( ! objGralBd.llenarDataSet( false ))
                {
                    Error = objGralBd.Error;
                    objGralBd.cerrarCnx();
                    objGralBd = null;
                    return false;
                }
                Generico.DataSource = objGralBd.dataSetLleno.Tables[0];
                Generico.Refresh();
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
        public bool llenarGridWeb     ( GridView Generico )
        {
            if ( ! Validar( ) )
                return false;
            try
            {
                clsGeneralesBD objGralBd = new clsGeneralesBD(strApp);
                objGralBd.SQL = SQL;
                if ( ! objGralBd.llenarDataSet( false ) )
                {
                    Error = objGralBd.Error;
                    objGralBd.cerrarCnx();
                    objGralBd = null;
                    return false;
                }
                Generico.DataSource = objGralBd.dataSetLleno.Tables[0];
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
