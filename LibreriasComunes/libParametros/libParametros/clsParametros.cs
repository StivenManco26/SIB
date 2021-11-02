using System;

//Usar librería
using System.Xml;

//Referenciar y usar
using System.Windows.Forms;

namespace libParametros
{
    public class clsParametros
    {
    #region "Atributos/Propiedades"
        private string strServidor;
        private string strBaseDatos;
        private string strUsuario;
        private string strClave;
        private string strSegInt;
        private string strPathXml;

        public string cadenaCnx {  get; private set; } //Propiedad de solo salida
        public string Error { get; private set; }      //Propiedad de solo salida

        private XmlDocument objDoc = new XmlDocument();
        private XmlNode objNodo;
    #endregion

    #region "Constructor"
        public clsParametros()
        {
            strServidor  = string.Empty;
            strBaseDatos = string.Empty;
            strUsuario   = string.Empty;
            strClave     = string.Empty;
            strSegInt    = string.Empty;
            strPathXml   = string.Empty;
            cadenaCnx    = string.Empty;  //Propiedad
            Error        = string.Empty;  //Propiedad
        }
    #endregion

    #region "Métodos Públicos"
        public bool generarCadenaCnx ( string strNombreSitio )
        {
            try
            {
                if ( string.IsNullOrEmpty( strNombreSitio ) )
                {
                    Error = "Sin Nombre, consultar admon.";
                    return false;
                }
                strPathXml = Application.StartupPath + "\\CON_" + strNombreSitio + ".xml";
                objDoc.Load( strPathXml );
                objNodo = objDoc.SelectSingleNode("//Servidor");
                strServidor = objNodo.InnerText;    //Recupera el nombre del servidor del archivo xml
                objNodo = objDoc.SelectSingleNode("//BaseDatos");
                strBaseDatos = objNodo.InnerText;    //Recupera el nombre de la BD del archivo xml
                objNodo = objDoc.SelectSingleNode("//Usuario");
                strUsuario = objNodo.InnerText;    //Recupera el nombre del usuario del archivo xml
                objNodo = objDoc.SelectSingleNode("//Clave");
                strClave = objNodo.InnerText;    //Recupera la clave del usuario del archivo xml
                objNodo = objDoc.SelectSingleNode("//SeguridadIntegrada");
                strSegInt = objNodo.InnerText;

                strSegInt = strSegInt.ToLower();
                if ( strSegInt.Equals("no") ) //Autenticación SQL SERVER   
                    cadenaCnx = "Data Source =" + strServidor +
                                "; Initial Catalog =" + strBaseDatos +
                                "; User Id =" + strUsuario +
                                "; Password =" + strClave + ";";
                else if ( strSegInt.ToLower() == "si" )   //Autenticación  Windows
                    cadenaCnx = "Data Source =" + strServidor +
                                "; Initial Catalog =" + strBaseDatos +
                                "; Integrated Security = SSPI;";
                else
                {
                    Error = "Proyecto no válido";
                    objDoc = null;
                    return false;
                }
                objDoc = null;
                return true;
            }
            catch ( Exception ex )
            {
                Error = ex.Message;
                objDoc = null;
                return false;
            }
        }
    #endregion
    }
}
