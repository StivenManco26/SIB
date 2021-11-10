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
    public class clsMaterial
    {
        #region "Atributos/Propiedades"
        private string strApp;
        private string strSQL;
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string edicion { get; set; }
        public int cantidad { get; set; }
        public int estado { get; set; }
        public int autor { get; set; }
        public int editorial { get; set; }
        public string Error { get; private set; }
        private SqlDataReader myReader;

        #endregion
        #region "Constructor"
        public clsMaterial(string Aplicacion)
        {
            strApp = Aplicacion;
            strSQL = string.Empty;
            codigo = string.Empty;
            nombre = string.Empty;
            edicion = string.Empty;
            cantidad = 0;
            Error = string.Empty;
            estado = 0;
            autor = 0;
            editorial = 0;
        }

        public clsMaterial(string Aplicacion, string codigo, string nombre, string edicion, int cantidad, int estado, int autor, int editorial)
        {
            strApp = Aplicacion;
            this.strSQL = string.Empty;
            this.codigo = codigo;
            this.nombre = nombre;
            this.edicion = edicion;
            this.cantidad = cantidad;
            this.estado = estado;
            this.autor = autor;
            this.editorial = editorial;
            Error = string.Empty;
        }

        #endregion
        private bool validarDatos()
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
            if (string.IsNullOrEmpty(edicion.ToString()))
            {
                Error = "Falta el edición";
                return false;
            }
            if (string.IsNullOrEmpty(cantidad.ToString()))
            {
                Error = "Falta el cantidad";
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
                codigo = Convert.ToString(objCnx.vrUnico);
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
        
        public bool Buscar(string c)
        {
            try
            {
                strSQL = "EXEC sp_consultar_Mat_puntual '" + c + "';";
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
                    Error = "No existe registro con código: " + c;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                myReader.Read();
                codigo = myReader.GetString(0);
                nombre = myReader.GetString(1);
                edicion = myReader.GetString(2);
                cantidad = myReader.GetInt32(3);
                estado = myReader.GetInt32(4);
                autor = myReader.GetInt32(5);
                editorial = myReader.GetInt32(6);
                myReader.Close();
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
            strSQL = "EXEC sp_ingresar_Material '" + codigo + "', '" + nombre + "', '"
                + edicion + "', " + cantidad + ", " + estado + ", " + autor + ", " + editorial + ";";
            return Grabar();
        }
        public bool modificarMaestro()
        {
            if (!validarDatos())
                return false;
            strSQL = "EXEC sp_actualizar_Material '" + codigo + "', '" + nombre + "', '"
                + edicion + "', " + cantidad + ", " + estado + ", " + autor + ", " + editorial + ";";
            return Grabar();
        }
    }
}