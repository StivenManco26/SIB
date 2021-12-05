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
        public string FechaPres { set; get; }
        public DateTime FechaDevo { set; get; }
        public DateTime FechaRegi { set; get; }
        public DateTime FechaReserva { set; get; }
        public string CodigoMat { set; get; }
        public int IdEstadoMaterial { set; get; }
        public string EstadoMaterial { set; get; }
        public string EstadoReserva { set; get; }
        public int RespuestaRes { set; get; }
        public int RespuestaPres { set; get; }
        public string Nit { set; get; }
        public int IdReserva { set; get; }
        public int Numero { set; get; }
        public string Nombre { set; get; }
        public string Material { private set; get; }
        public int CantPrestamo { private set; get; }
        public int MaxCantPres { private set; get; }
        public int DiasPrestamo { private set; get; }
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
            FechaPres = string.Empty;
            FechaDevo = DateTime.Now.Date;
            FechaRegi = DateTime.Now.Date;
            FechaReserva = DateTime.Now.Date;
            CodigoMat = string.Empty;
            IdEstadoMaterial = 1;
            Nit = string.Empty;
            IdReserva = 0;
            Numero = 0;
            Nombre = string.Empty;
            CantPrestamo = 0;
            MaxCantPres = 0;
            DiasPrestamo = 0;
            Material = string.Empty;
            strSQL = string.Empty;
            Error = string.Empty;

        }


        public clsPrestamo(string Aplicacion, /*DateTime fechaPres, DateTime fechaRegi, */string codigoMat, int idEstadoMaterial, string nit, int idReserva, DateTime fechaDevo /*int numero, string material,int CantPres,int diasPrest,string nombre*/)
        {
            strApp = Aplicacion;
            FechaPres = "yyyy-mm-dd";
            FechaDevo = fechaDevo;
            FechaRegi = DateTime.Now.Date;
            CodigoMat = codigoMat;
            IdEstadoMaterial = idEstadoMaterial;
            Nit = nit;
            IdReserva = idReserva;
            Numero = 0;
            Material = string.Empty;
            CantPrestamo = 0;
            MaxCantPres = 0;
            DiasPrestamo = 0;
            Nombre = string.Empty;
            strSQL = string.Empty;
            Error = string.Empty;

        }

        #endregion

        #region "métodos privados"

        private bool ValidarDatos()
        {
            if (Convert.ToDateTime(FechaPres)<  DateTime.Now.Date)
            {
                Error = "Fecha de prestamo no válida";
                return false;
            }
            if (Convert.ToDateTime(FechaPres) > Convert.ToDateTime(FechaDevo))
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
            if (IdEstadoMaterial < 0)
            {
                Error = "El Id del Estado del material no es Válido ";
                return false;
            }
            if (CantPrestamo>MaxCantPres)
            {
                Error = "Maximo de prestamos permitido ";
                return false;
            }
            if(RespuestaRes==0)
            {
                Error = "El codigo "+CodigoMat+" Ya tiene una reserva activa";
                return false;
            }
            return true;
        }

        private bool ValidarDatosReserva()
        {
            if (Convert.ToDateTime(FechaPres) < DateTime.Now.Date)
            {
                Error = "Fecha de prestamo no válida";
                return false;
            }
            if (Convert.ToDateTime(FechaPres) > Convert.ToDateTime(FechaDevo))
            {
                Error = "Fecha de Devolucion no válida";
                return false;
            }
            if (FechaRegi > DateTime.Now.Date)
            {
                Error = "Fecha de registro no válida";
                return false;
            }

            if (CantPrestamo > MaxCantPres)
            {
                Error = "Maximo de prestamos permitido";
                return false;
            }
            //if (RespuestaPres == 0)
            //{
            //    Error = "El codigo " + CodigoMat + " Ya tiene un Prestamo activo";
            //    return false;
            //}
            return true;
        }






        private bool ValidarDatosMaestro()
        {
            if (Convert.ToDateTime(FechaPres) < DateTime.Now.Date)
            {
                Error = "Fecha de prestamo no válida";
                return false;
            }
            if (Convert.ToDateTime(FechaPres) > Convert.ToDateTime(FechaDevo))
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
            if (IdEstadoMaterial < 0)
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
                Nit = Convert.ToString(objCnx.vrUnico);
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

        public bool BuscarPerfilPersona(string n)
        {
            try
            {
                strSQL = "EXEC sp_buscar_persona_perfil '" + n + "';";
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
                    Error = "No existe registro con el Documento: " + n;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader.Read();
                Nit = MyReader.GetString(0);
                MaxCantPres = MyReader.GetInt32(1);
                DiasPrestamo = MyReader.GetInt32(2);
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
        //realizar la ejecucion del procedimiento almacenado con un buscar cantdad prestamo
        public bool BuscarCantidad(string n)
        {
            try
            {
                strSQL = "EXEC sp_Contar_Prestamo '" + n + "';";
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
                    Error = "No existe registro con el Documento: " + n;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader.Read();
                CantPrestamo = MyReader.GetInt32(0);
                MyReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }

        }


        public bool BuscarPrestamo(string nit, GridView grid)  //BuscarPrestamo
        {
            try
            {
                if (nit == "" || grid == null)
                {
                    Error = "Nro. de Prestamo no Válido o nit vacío";
                    return false;
                }
                strSQL = "EXEC sp_buscar_prestamo_persona '" + nit + "';";
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
                EstadoMaterial = dr["estado"].ToString();
                Nit = dr["nit"].ToString();
                IdReserva = Convert.ToInt32(dr["idReserva"]);
                FechaPres = dr["fechaPrestamo"].ToString();                
                FechaDevo = Convert.ToDateTime(dr["fechaDevolucion"]);
                FechaRegi = Convert.ToDateTime(dr["fechaRegistro"]);
                //Mydt.Clear();
                //Mydt = Myds.Tables[1];            
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

        public bool BuscarReservaPersona(/*string codigoMat,*/ string nit, GridView grid)  //BuscarPrestamo
        {
            try
            {
                if (nit == "" || grid == null)
                {
                    Error = "Nro. de Reserva no Válido o nit vacío";
                    return false;
                }
                strSQL = "EXEC sp_cargar_reserva_persona '" /* + codigoMat +"','"*/+ nit + "';";
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
                    Error = "No existe Reserva para el Documento: " + nit /*+ " y el libro" + codigoMat*/;
                    Myds.Clear();
                    Myds = null;
                    return false;
                }
                DataRow dr = Mydt.Rows[0];
                IdReserva = Convert.ToInt32(dr["Reserva"]);
                Material = dr["Material"].ToString();
                FechaReserva = Convert.ToDateTime(dr["Fecha reserva"]);
                EstadoReserva = dr["Estado"].ToString();
  
                //Mydt.Clear();
                //Mydt = Myds.Tables[1];            
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

        public bool BuscarReservaMaterial(string codigoMaterial)
        {
            try
            {
                strSQL = "EXEC sp_validar_reserva_por_Material '" + codigoMaterial + "';";
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
                    Error = "No existe registro con codigo: " + codigoMaterial;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader.Read();
                RespuestaRes = Convert.ToInt32(MyReader.GetString(0));
                MyReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        public bool BuscarPrestamoMaterial(string codigoMaterial)
        {
            try
            {
                strSQL = "EXEC sp_validar_prestamo '" + codigoMaterial + "';";
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
                    Error = "No existe registro con codigo: " + codigoMaterial;
                    objCnx.cerrarCnx();
                    objCnx = null;
                    return false;
                }
                MyReader.Read();
                RespuestaPres = Convert.ToInt32(MyReader.GetString(0));
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
            
            strSQL = "EXEC sp_Ingresar_Prestamo '" + CodigoMat + "'," + IdEstadoMaterial + ",'" + Nit + "' ," + IdReserva + ",'" + "" + FechaDevo.ToString("yyyy-MM-dd") + "';";
            if (!Grabar())
                return false;

            return true;
        }

        public bool grabarReservaPrestamo() //grabarMaestro
        {
            if (!ValidarDatosReserva())
                return false;

            strSQL = "EXEC sp_Ingresar_Prestamo_con_Reserva " + IdReserva + "," + IdEstadoMaterial + ",'" + FechaDevo.ToString("yyyy-MM-dd") + "';";
            if (!Grabar())
                return false;

            return true;
        }

        public bool modificarMaestro() //grabarMaestro
        {
            if (!ValidarDatos())
                return false;
            strSQL = "EXEC sp_Ingresar_Prestamo '" + CodigoMat + "'," + IdEstadoMaterial + ",'" + Nit + "' , '" + IdReserva + "," + FechaDevo + "';";
            if (!Grabar())
                return false;

            return true;
        }


    }
}