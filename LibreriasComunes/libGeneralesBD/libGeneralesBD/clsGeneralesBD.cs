using System;

//Referenciar y Usar
using libParametros;

//Usar
using System.Data;   //No Conectado  DataSet, DataTable
using System.Data.SqlClient; //Conectada: Connection, Command, DAtaREader y Datadapter

namespace libGeneralesBD
{
    public class clsGeneralesBD
    {
    #region "Atríbutos/Propiedades"
        private string strApp;       //Para el nombre de la aplicación
        private string strCadenaCnx; //Para la cadena de conexón a la BD
        private bool blnHayCnx;      //Para saber si hay o no Cnx a la BD

        public string SQL { private get; set; } //Para la sentencia SQL a ejecutarse en la BD
        public object vrUnico { get; private set; } //Para la captura y retorno del Vr. único(método: ConsultarValorUnico)) 
        public string Error { get; private set; } //Para el mensaje de error
        public SqlDataReader dataReaderLleno { get; private set; } //Para el objeto DataReader (contenedor de info)

        private SqlConnection objCnx;     //Para el objeto Conexión
        private SqlCommand objCmd;     //Para el objeto Command (realiza la transacción)
        private SqlDataAdapter objAdapter; //Para el objeto DataAdapter (para llenar el DataSet, entre otros)
        public DataSet dataSetLleno { get; private set; } //Para el objeto DataSet (contenedor de info)
    #endregion

    #region "Constructor"
        public clsGeneralesBD ( string nombreSitio )
        {
            strApp = nombreSitio;
            blnHayCnx = false;
            objCnx = new SqlConnection ( );      //Para la Conexión
            objCmd = new SqlCommand ( );         //Para la Transacción
            objAdapter = new SqlDataAdapter ( ); //Para la llenar el DataSet
            dataSetLleno = new DataSet ( );      //Para el DataSet
        }
    #endregion

    #region "Métodos Privados"
        private bool generarCadenaCnx ()
        {
            try
            {
                if ( string.IsNullOrEmpty ( strApp ) )
                {
                    Error = "Sin Nombre de la aplicación";
                    return false;
                }
                clsParametros objParams = new clsParametros();
                if ( ! objParams.generarCadenaCnx ( strApp ) )
                {
                    Error = objParams.Error;
                    objParams = null;
                    return false;
                }
                strCadenaCnx = objParams.cadenaCnx;
                objParams = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        private bool abrirCnx ()
        {
            if ( ! generarCadenaCnx ( ) )
                return false;

            objCnx.ConnectionString = strCadenaCnx;
            try
            {
                objCnx.Open();
                blnHayCnx = true;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                blnHayCnx = false;
                return false;
            }
        }
    #endregion

    #region "Métodos Públicos"
        public void cerrarCnx()
        {
            try
            {
                objCnx.Close();
                objCnx = null;
                blnHayCnx = false;
            }
            catch (Exception ex)
            {
                Error = "No se cerró o liberó la conexión, " + ex.Message;
            }
        }
        public bool Consultar( bool blnParametros ) //false
        {
            try
            {
                if ( string.IsNullOrEmpty ( SQL ) )
                {
                    Error = "No definió la instrucción SQL";
                    return false;
                }
                if ( ! blnHayCnx )
                    if ( ! abrirCnx ( ) )
                        return false;
                //Preparar el Comando para ejecutar la transacción SQL en la BD
                objCmd.Connection = objCnx;
                objCmd.CommandText = SQL;
                if ( blnParametros ) 
                    objCmd.CommandType = CommandType.StoredProcedure;
                else
                    objCmd.CommandType = CommandType.Text;
                dataReaderLleno = objCmd.ExecuteReader();  //Realizar la transacción en la BD
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool consultarValorUnico ( bool blnParametros )
        {
            try
            {
                if ( string.IsNullOrEmpty( SQL ))
                {
                    Error = "No definió la instrucción SQL";
                    return false;
                }
                if ( ! blnHayCnx )
                    if ( ! abrirCnx() )
                        return false;
                //Preparar el Comando para ejecutar la transacción SQL en la BD
                objCmd.Connection = objCnx;
                objCmd.CommandText = SQL;
                if (blnParametros)
                    objCmd.CommandType = CommandType.StoredProcedure;
                else
                    objCmd.CommandType = CommandType.Text;
                vrUnico = objCmd.ExecuteScalar();  //Realizar la transacción en la BD
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool ejecutarSentencia( bool blnParametros )
        {
            try
            {
                if ( string.IsNullOrEmpty( SQL ))
                {
                    Error = "No definió la instrucción SQL";
                    return false;
                }
                if ( ! blnHayCnx )
                    if ( ! abrirCnx( ) )
                        return false;
                //Preparar el Comando para ejecutar la transacción SQL en la BD
                objCmd.Connection = objCnx;
                objCmd.CommandText = SQL;
                if ( blnParametros )
                    objCmd.CommandType = CommandType.StoredProcedure;
                else
                    objCmd.CommandType = CommandType.Text;
                objCmd.ExecuteNonQuery();   //Realizar la transacción en la BD
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool llenarDataSet( bool blnParametros )
        {
            try
            {
                if ( string.IsNullOrEmpty( SQL ) )
                {
                    Error = "No definió la instrucción SQL";
                    return false;
                }
                if ( ! blnHayCnx )
                    if ( ! abrirCnx() )
                        return false;
                //Preparar el Comando para el DataAdapter
                objCmd.Connection = objCnx;
                objCmd.CommandText = SQL;
                if ( blnParametros )
                    objCmd.CommandType = CommandType.StoredProcedure;
                else
                    objCmd.CommandType = CommandType.Text;
                //Preparar el DataAdapter para el uso del comando en la BD
                //El DataAdapter Utiliza el Command para la transacción
                objAdapter.SelectCommand = objCmd;
                //Realizar la transacción en la BD y el llenado del DataSet/Datatable
                objAdapter.Fill( dataSetLleno );
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
