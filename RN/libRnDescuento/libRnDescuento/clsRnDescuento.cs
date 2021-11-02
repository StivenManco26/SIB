using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace libRnDescuento
{
    public class clsRnDescuento
    {
        #region "Constructor"
        public clsRnDescuento()
        {
            intTipo = 0;
            fltDesc = 0;
            intCanti = 0;
            strError = string.Empty;
        }
        #endregion

        #region "Atributos/Propiedades"
        public int intTipo { private get; set; }
        public int intCanti { private get; set; }
        public float fltDesc { get; set; }
        public string strError { get; private set; }
        #endregion

        #region "Métodos Privados"
        private bool leerArchivo()
        {
            try
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + @"DescuentoCasual.txt";
                int intCant = 0;// Para la cantidad de líneas que tiene el archivo
                int intCantD = 0; //Para la cantidad de productos a comparar
                string strLinea;// Para la línea leída del archivo
                string[] vectorLinea;// Vector para almacenar la línea del archivo
                string strCodigo;
                intCant = File.ReadAllLines(strPath).Length;// Lee la cantidad de líneas que tiene el archivo
                if (intCant <= 0)
                {
                    strError = "Sin registros";
                    return false;
                }
                StreamReader Archivo = new StreamReader(@strPath); //Crear objeto para leer el archivo
                while ((strLinea = Archivo.ReadLine()) != null)   //Leer línea * línea el archivo
                {
                    vectorLinea = strLinea.Split(';');
                    strCodigo = vectorLinea[0]; //Nombre Dato
                    intCantD = Convert.ToInt32(vectorLinea[1]);//Cantidad
                    if (strCodigo == intTipo.ToString().Trim() && intCanti>= intCantD)
                    {
                        fltDesc = Convert.ToSingle(vectorLinea[2]); //Descuento
                        fltDesc = fltDesc / 100f;
                        break;
                    }
                }
                Archivo.Close();
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }
        #endregion

        #region "Métodos Públicos"
        public bool Consultar()
        {
            return leerArchivo();
        }
        #endregion
    }
}
