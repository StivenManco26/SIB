using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using libGeneralesBD;

namespace webSib.Clases
{
    public class clsPrestamo
    {
        #region "Atributos y propiedades"
        private string strApp;
        public DateTime FechaPres { set; get; }
        public DateTime FechaDevo { set; get; }
        public DateTime FechaRegi { set; get; }
        public string CodigoMat { set; get; }
        public int IdEstadoMaterial { set; get; }
        public string Nit { set; get; }
        public int IdReserva { set; get; }
        public int Numero { set; get; }
        public string Nombre { set; get; }
        public string Material { private set; get; }
        public string Error { private set; get; }
        private int PerfilPersona;
        private string strSQL;
        private DataSet Myds;
        private DataTable Mydt;
        private SqlDataReader MyReader;


        #endregion

        #region "Constructor"

        public clsPrestamo(string Aplicacion)
        {
            strApp = Aplicacion;
            FechaPres = DateTime.Now.Date;
            FechaDevo = DateTime.Now.Date;
            FechaRegi = DateTime.Now.Date;
            CodigoMat = string.Empty;
            IdEstadoMaterial = 1;
            Nit = string.Empty;
            IdReserva = 0;
            Numero = 0;
            Material = string.Empty;
            strSQL = string.Empty;
            Error = string.Empty;

        }


        public clsPrestamo(string Aplicacion, DateTime fechaPres, DateTime fechaDevo, DateTime fechaRegi, string codigoMat, int idEstadoMaterial, string nit, int idReserva, int numero, string material)
        {
            strApp = Aplicacion;
            FechaPres = fechaPres;
            FechaDevo = fechaDevo;
            FechaRegi = fechaRegi;
            CodigoMat = codigoMat;
            IdEstadoMaterial = idEstadoMaterial;
            Nit = nit;
            IdReserva = idReserva;
            Numero = numero;
            Material = material;
            strSQL = string.Empty;
            Error = string.Empty;

        }
        #endregion

        #region "métodos privados"

        private bool ValidarDatos()
        {
            if (FechaPres < DateTime.Now.Date)
            {
                Error = "Fecha de prestamo no válida";
                return false;
            }
            if (FechaPres > FechaDevo)
            {
                Error = "Fecha de Devolucion no válida";
                return false;
            }
            if (FechaRegi > DateTime.Now.Date)
            {
                Error = "Fecha de registro no válida";
                return false;
            }
            if (string.IsNullOrEmpty(CodigoMat))
            {
                Error = "El código del material no es Válido ";
                return false;
            }
            if (IdEstadoMaterial <= 0)
            {
                Error = "El Id del Estado del material no es Válido ";
                return false;
            }
            return true;
        }

        #endregion

        private bool Grabar()
        {
            try
            {
                clsGeneralesBD objCnx = new clsGeneralesBD(strApp);
                objCnx.SQL = strSQL;
                if (!objCnx.consultarValorUnico(false))
                {
                    Error = objCnx.Error;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                Numero = Convert.ToInt32(objCnx.vrUnico);
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

        private bool BuscarPerfilPersona(string NroDocUsuario)
        {
            try
            {
                strSQL = "EXEC sp_buscar_persona '" + NroDocUsuario + "';";
                clsGeneralesBD objCnx = new clsGeneralesBD(strApp);
                objCnx.SQL = strSQL;
                if (!objCnx.Consultar(false))
                {
                    Error = objCnx.Error;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader = objCnx.dataReaderLleno;
                if (!MyReader.HasRows)
                {
                    Error = "No existe registro con el Documento: " + NroDocUsuario;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader.Read();
                PerfilPersona = MyReader.GetInt32(0);
                MyReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        #region "Métodos Públicos"

        public bool BuscarNombreLibro(string CM)
        {
            try
            {
                strSQL = "EXEC sp_consultar_Mat_puntual '" + CM + "';";
                clsGeneralesBD objCnx = new clsGeneralesBD(strApp);
                objCnx.SQL = strSQL;
                if (!objCnx.Consultar(false))
                {
                    Error = objCnx.Error;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader = objCnx.dataReaderLleno;
                if (!MyReader.HasRows)
                {
                    Error = "No se encontró ningún Material: " + CM;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader.Read();
                CodigoMat = MyReader.GetString(0);
                Material = MyReader.GetString(1);
                MyReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        #endregion

        public bool BuscarPrestamo(int nit, GridView grid)  //BuscarFactura
        {
            try
            {
                if (nit <= 0 || grid == null)
                {
                    Error = "Nro. de Factura no Válido";
                    return false;
                }
                strSQL = "EXEC sp_buscar_prestamo_persona " + nit + ";";
                clsGeneralesBD objCnx = new clsGeneralesBD(strApp);
                objCnx.SQL = strSQL;
                if (!objCnx.llenarDataSet(false))
                {
                    Error = objCnx.Error;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                Myds = objCnx.dataSetLleno;
                objCnx = null;
                Mydt = Myds.Tables[0];
                if (Mydt.Rows.Count <= 0)
                {
                    Error = "No existe prestamo para el Documento: " + nit;
                    Myds.Clear();
                    Myds = null;
                    return false;
                }
                DataRow dr = Mydt.Rows[0];
                Numero = Convert.ToInt32(dr["id"]);
                CodigoMat = dr["codigoMat"].ToString();
                IdEstadoMaterial = Convert.ToInt32(dr["Cliente"]);
                Nit = dr["nit"].ToString();
                IdReserva = Convert.ToInt32(dr["idReserva"]);
                FechaPres = Convert.ToDateTime(dr["fechaPrestamo"]);
                FechaDevo = Convert.ToDateTime(dr["fechaDevolucion"]);
                FechaRegi = Convert.ToDateTime(dr["fechaRegistro"]);
                Mydt.Clear();
                Mydt = Myds.Tables[1];
                grid.DataSource = Mydt;
                grid.DataBind();
                grid.GridLines = GridLines.Both;
                grid.CellPadding = 1;
                grid.ForeColor = System.Drawing.Color.Black;
                grid.BackColor = System.Drawing.Color.Beige;
                grid.AlternatingRowStyle.BackColor = System.Drawing.Color.Gainsboro;
                grid.HeaderStyle.BackColor = System.Drawing.Color.Aqua;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }


        public bool BuscarPersona(string n)
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
                MyReader = objCnx.dataReaderLleno;
                if (!MyReader.HasRows)
                {
                    Error = "No existe registro con nit: " + n;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader.Read();
                Nit = MyReader.GetString(0);
                Nombre = MyReader.GetString(1);
                MyReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }


        public bool grabarPrestamo() //grabarMaestro
        {
            if (!ValidarDatos())
                return false;
            //Grabar el encabezado
            strSQL = "EXEC sp_Ingresar_Prestamo '" + CodigoMat + "'," + IdEstadoMaterial + ",'" + Nit + "' , '" + IdReserva + "," + FechaDevo + "';";
            if (!Grabar())
                return false;

            return true;
        }


    }
}