using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libRnDescuento;

namespace libOpDescuento
{
    public class clsOpDescuento
    {
        #region "Constructor"
        public clsOpDescuento()
        {
            intTipo = 0;
            intCant = 0;
            fltDesc = 0;
            strError = string.Empty;
        }

        public clsOpDescuento(int tipo, int cant)
        {
            intTipo = tipo;
            intCant = cant;
            fltDesc = 0;
            strError = string.Empty;
        }
        #endregion

        #region "Atributos / Propiedades"
        public int intTipo { private get; set; }
        public int intCant { private get; set; }
        public float fltDesc { get; set; }
        public string strError { get; private set; }
        #endregion

        #region "Métodos privados"
        private bool Validar()
        {
            if (intTipo < 1 || intTipo > 2)
            {
                strError = "Tipo de cliente no válido";
                return false;
            }

            if (intCant <= 0)
            {
                strError = "Cantidad no válido";
                return false;
            }

            return true;
        }
        #endregion

        #region "Metodos públicos"
        public bool Descuento()
        {
            try
            {
                clsRnDescuento objRn = new clsRnDescuento();

                if (!Validar())
                    return false;

                //Enviar info
                objRn.intTipo = intTipo;
                objRn.intCanti = intCant;
                //Invocación del método y tratamiento del error
                if (!objRn.Consultar())
                {
                    strError = objRn.strError;
                    return false;
                }

                fltDesc = objRn.fltDesc;
                
                return true;
            }
            catch (Exception Ex)
            {
                strError = Ex.Message;
                return false;
            }


        }
        #endregion
    }
}
