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
    public class clsUsuario
    {
        #region "Atributos/Propiedades"
        private string strApp;
        private string strSQL;
        public string nit { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public int perfil { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string Error { get; private set; }
        private SqlDataReader myReader;

        #endregion
        #region "Constructor"
        public clsUsuario(string Aplicacion)
        {
            strApp = Aplicacion;
            strSQL = string.Empty;
            nit = string.Empty;
            nombre = string.Empty;
            correo = string.Empty;
            celular = string.Empty;
            Error = string.Empty;
            perfil = 0;
            usuario = string.Empty;
            contrasena = string.Empty;
        }

        public clsUsuario(string Aplicacion, string nit, string nombre, string correo, string celular, int perfil, string usuario, string contrasena)
        {
            strApp = Aplicacion;
            this.strSQL = string.Empty;
            this.nit = nit;
            this.nombre = nombre;
            this.correo = correo;
            this.celular = celular;
            this.perfil = perfil;
            this.usuario = usuario;
            this.contrasena = contrasena;
            Error = string.Empty;
        }

        #endregion
        private bool validarDatos()
        {
            if (string.IsNullOrEmpty(nit.ToString()))
            {
                Error = "Falta nit";
                return false;
            }
            if (string.IsNullOrEmpty(nombre.ToString()))
            {
                Error = "Falta el nombre";
                return false;
            }
            if (string.IsNullOrEmpty(correo.ToString()))
            {
                Error = "Falta el correo";
                return false;
            }
            if (string.IsNullOrEmpty(celular.ToString()))
            {
                Error = "Falta el celular";
                return false;
            }
            if (string.IsNullOrEmpty(usuario.ToString()))
            {
                Error = "Falta el usuario";
                return false;
            }
            if (string.IsNullOrEmpty(contrasena.ToString()))
            {
                Error = "Falta la contraseña";
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
                nit = Convert.ToString(objCnx.vrUnico);
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
        
        public bool Buscar(string n)
        {
            try
            {
                strSQL = "EXEC sp_buscar_persona '" + n + "';";
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
                    Error = "No existe registro con nit: " + n;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                myReader.Read();
                nit = myReader.GetString(0);
                nombre = myReader.GetString(1);
                correo = myReader.GetString(2);
                celular = myReader.GetString(3);
                perfil = myReader.GetInt32(4);
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
            strSQL = "EXEC sp_ingresar_persona_usuario '" + nit + "', '" + nombre + "', '"
                + correo + "', '" + celular + "', " + perfil + ", '" + usuario + "', '" + contrasena + "';";
            return Grabar();
        }
        /*public bool modificarMaestro()
        {
            if (!validarDatos())
                return false;
            strSQL = "EXEC USP_Prod_Modificar " + codigo + ", " + "'" + descripcion + "', " + 
                Math.Truncate(valorUnitario) + ", " + iva.ToString().Replace(',', '.') + ", " + clasificacion + ", " + codEmpleado + ";";
            return Grabar();
            //EXEC USP_Prod_Modificar 100,'LG SMARTV 32', 2000000, 0.19, 1, 1111
        }*/
    }
}