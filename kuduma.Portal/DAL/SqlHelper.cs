using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Web;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace KLTS.Data
{

    public static class ConnectionStrings
    {
        static string _Connectionstring = string.Empty;

        static ConnectionStrings()
        {
            _Connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        }

        public static string ConnectionString
        { get; set; }

    }

    public class SqlHelper
    {
        private SqlConnection _Connection = null;

        private static SqlHelper _Instance;

        private SqlHelper()
        {
            string _Connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
            ConnectionStrings.ConnectionString = _Connectionstring;
        }

        public static SqlHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SqlHelper();
                }
                return _Instance;
            }
        }

        public void CreateConnection()
        {
            if (_Connection == null)
            {
                string Connection = ConnectionStrings.ConnectionString;
                _Connection = new SqlConnection(Connection);
                //_Connection.Open();
            }
        }

        public void OpenConnection()
        {
            CreateConnection();
            if (_Connection.State != ConnectionState.Open)
                _Connection.Open();
        }

        public void CloseConnection()
        {
            if (_Connection.State == ConnectionState.Open)
                _Connection.Close();
        }

        public DataTable GetTableByQuery(string Qry)
        {
            CreateConnection();
            DataTable _table = new DataTable();
            SqlDataAdapter _adapter = new SqlDataAdapter(Qry, _Connection);
            using (_adapter)
            {
                //try
                {
                    _adapter.Fill(_table);
                    int count = _table.Rows.Count;
                }
                //catch (System.Exception e) 
                //{
                //    string msg = e.Message;
                //}
            }
            return _table;
        }

        public SqlDataReader GetReader(string Qry)
        {
            SqlDataReader _reader = null;
            //try
            {
                OpenConnection();
                SqlCommand _command = new SqlCommand(Qry, _Connection);
                _reader = _command.ExecuteReader(CommandBehavior.SequentialAccess);
                CloseConnection();
            }
            //catch (System.Exception)
            //{
            //    _Connection.Close();
            //    //_Connection = null;
            //}
            //finally { _Connection.Close(); }

            return _reader;
        }


        public bool IsCheckValidUser(string Qry, string LoginName, string Password)
        {
            bool _Status = false;
            SqlDataReader _reader = GetReader(Qry);
            while (_reader.Read())
            {
                if (_reader.GetString(0) == LoginName && _reader.GetString(1) == Password)
                    _Status = true;
            }
            return _Status;
        }

        public int ExecuteDMLQry(string Qry)
        {
            int status = 0;
            //try
            {
                OpenConnection();
                SqlCommand _command = new SqlCommand(Qry, _Connection);
                status = _command.ExecuteNonQuery();
                CloseConnection();
            }
            //catch (System.Exception e)
            //{
            //    string str = e.Message;
            //    _Connection.Close();
            //    //_Connection = null;
            //}
            //finally { _Connection.Close(); }

            return status;
        }


        public SqlCommand GetSqlCommand(string Qry)
        {
            SqlCommand _command = null;
            //try
            {
                OpenConnection();
                _command = new SqlCommand(Qry, _Connection);
            }
            //catch (System.Exception) { }
            return _command;
        }

        public int ExecuteScalarQry(string Qry)
        {
            int Val = 0;
            //try
            {
                OpenConnection();
                SqlCommand _command = new SqlCommand(Qry, _Connection);
                Val = Convert.ToInt32(_command.ExecuteScalar());
                CloseConnection();
            }
            //catch (System.Exception)
            //{
            //    _Connection.Close();
            //   // _Connection = null;
            //}

            //finally { _Connection.Close(); }
            return Val;
        }

        #region  //BEgin  Code   Related to Stored Procedure

        public int ExecuteQuery(string procName, Hashtable parms)
        {
            // SqlCommand cmd = new SqlCommand();
            SqlCommand cmd = new SqlCommand(procName, _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procName;
            if (parms.Count > 0)
            {
                foreach (DictionaryEntry de in parms)
                {
                    cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                }
            }

            OpenConnection();
            int result = cmd.ExecuteNonQuery();
            CloseConnection();
            return result;
        }

        public DataTable ExecuteSPWithoutParams(string procedureName)
        {
            CreateConnection();
            DataTable _table = new DataTable();
            SqlCommand cmd = new SqlCommand(procedureName, _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
            using (_adapter)
            {
                //try
                {
                    _adapter.Fill(_table);
                    int count = _table.Rows.Count;
                }
                //catch (System.Exception e)
                //{
                //    string msg = e.Message;
                //}
            }
            return _table;
        }

        #endregion//End  code  Related to stored Procedure

        public DataTable ExecuteStoredProcedureWithParams(string procedureName, Hashtable HTParms)
        {

            #region  Begin Old Code As on [20-09-2013]

            CreateConnection();
            DataTable _table = new DataTable();
            SqlCommand cmd = new SqlCommand(procedureName, _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (DictionaryEntry de in HTParms)
            {
                cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value);
            }

            //foreach (List<Object> list in parms)
            //{
            //    SqlParameter p = new SqlParameter();
            //    p.ParameterName = list[0].ToString();
            //    p.SqlDbType = (SqlDbType)list[1];
            //    p.Value = list[2];
            //    cmd.Parameters.Add(p);
            //}



            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
            using (_adapter)
            {
                //try
                {
                    _adapter.Fill(_table);
                    int count = _table.Rows.Count;
                }
                //catch (System.Exception e)
                //{
                //    string msg = e.Message;
                //}
            }
            return _table;

            #endregion  End Old Code As on [20-09-2013]

        }



        #region private utility methods & constructors

        /// <summary>
        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// This method assigns dataRow column values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // Do nothing if we get no data
                return;
            }

            int i = 0;
            // Set the parameters values
            foreach (SqlParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format(
                            "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                            i, commandParameter.ParameterName));
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary>
        /// This method assigns an array of values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary>
        /// This method assigns an array of values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
        private static void UpdateParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                //Update the Return Value
                if (commandParameters[i].Direction == ParameterDirection.ReturnValue)
                {
                    //parameterValues[i] = commandParameters[i].Value;
                }

                if (commandParameters[i].Direction == ParameterDirection.InputOutput)
                    parameterValues[i] = commandParameters[i].Value;
            }
        }

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command
        /// </summary>
        /// <param name="command">The SqlCommand to be prepared</param>
        /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        #endregion private utility methods & constructors

        #region ExecuteDataset

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            DataSet dsReturn;
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                //SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName); -- Original code from sqlHelper
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName, true); // Added Parameter true to support ReturnValues

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                //return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                //Modify code - just store the dataset to dsReturn
                dsReturn = ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);

                //Update the array -  parameterValues from the new CommandParameters that should have the ReturnValue
                UpdateParameterValues(commandParameters, parameterValues);
            }
            else
            {
                // Otherwise we can just call the SP without params
                //return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
                //Modify code
                dsReturn = ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
            //Modify code
            return dsReturn;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                // Return the dataset
                return ds;
            }
        }

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                // Return the dataset
                return ds;
            }
        }

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteDataset

        public string GetCompanyname()
        {
            int check = 1;
            string Cmpname = "FaMS";
            if (System.Configuration.ConfigurationManager.AppSettings["check"] != null)
            {
                check = int.Parse(System.Configuration.ConfigurationManager.AppSettings["check"]);
                if (check == 0)
                {
                    Cmpname = System.Configuration.ConfigurationManager.AppSettings["0"];
                }
                if (check == 1)
                {
                    Cmpname = System.Configuration.ConfigurationManager.AppSettings["1"];
                }
                if (check == 2)
                {
                    Cmpname = System.Configuration.ConfigurationManager.AppSettings["2"];
                }
            }
            return Cmpname;
        }

        public int GetCompanyValue()
        {
            int check = int.Parse(System.Configuration.ConfigurationManager.AppSettings["check"]);
            return check;
        }
    }

    public class GlobalData
    {
        static string appPath = "";
        private GlobalData() { }
        private static GlobalData _Instance;
        public static GlobalData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GlobalData();
                }
                return _Instance;
            }
        }
        AppConfiguration config = new AppConfiguration();


        public DataTable LoadFinancialYears()
        {

            string ProcedureName = "FinancialYears";
            System.Data.DataTable DtHSNNos = config.CentralExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtHSNNos;
        }

        public void GetMonthAndYear(string strMonth, int index, out int month, out int year)
        {
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            month = 0;
            for (int i = 0; i < monthName.Length; i++)
            {
                if (monthName[i].CompareTo(strMonth) == 0)
                {
                    month = i + 1;
                    break;
                }
            }
            year = 2000;
            if (index == 0)//next month
            {
                //year = (DateTime.Now).AddMonths(-1).Year;
                year = (DateTime.Now).AddMonths(0).Year; // changed on 12/9/12
            }
            else if (index == 1)//current month
            {
                year = (DateTime.Now).AddMonths(-1).Year;
            }
            else // previous month
            {
                year = (DateTime.Now).AddMonths(-2).Year;
            }
        }

        public int GetNoOfDaysOfThisMonth(int year, int month)
        {
            int yearv = year;
            int monthv = month;
            int numDays = DateTime.DaysInMonth(yearv, monthv);

            return numDays;
        }

        public int GetIDForNextMonth()
        {
            int year = 2000;

            year = (DateTime.Now).AddMonths(0).Year;

            string mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 0, (year - 2000).ToString());
            int month = Convert.ToInt32(mon);

            return month;
        }

        public int GetIDForThisMonth()
        {
            int year = 2000;

            year = (DateTime.Now).AddMonths(-1).Year;

            int month = DateTime.Now.Month;

            string mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 1, (year - 2000).ToString());

            if (year < DateTime.Now.Year)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 11, (year - 2000).ToString());

            }
            else
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 1, (year - 2000).ToString());
            }

            month = Convert.ToInt32(mon);
            return month;
        }

        public int GetIDForPrviousMonth()
        {
            int year = 2000;

            year = (DateTime.Now).AddMonths(-2).Year;

            string mon = "";
            if (DateTime.Now.Month == 1)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 10, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 2)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 10, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 3)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 4)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 5)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }


            if (DateTime.Now.Month == 6)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 7)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 8)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 9)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }


            if (DateTime.Now.Month == 10)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 11)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 12)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            int month = Convert.ToInt32(mon);

            return month;
        }

        public int GetIDForPrviousoneMonth()
        {
            int year = 2000;

            year = (DateTime.Now).AddMonths(-2).Year;

            string mon = "";
            if (DateTime.Now.Month == 1)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 9, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 2)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 9, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 3)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 4)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 5)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }


            if (DateTime.Now.Month == 6)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 7)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 8)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 9)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }


            if (DateTime.Now.Month == 10)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 11)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            if (DateTime.Now.Month == 12)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            int month = Convert.ToInt32(mon);

            return month;
        }
        public DataTable LoadStaffIDs()
        {

            string Sqlqry = "select   Empid,Empid+' - '+(ISnull(Empfname,'')+' '+ Isnull(empmname,'')+' '+ isnull(Emplname,'') ) AS FullName  From Empdetails where employeetype='S' Order By Right(Empid,6)";
            System.Data.DataTable DtEmpIds = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            return DtEmpIds;
        }


        public int GetNoOfDaysForNextMonth()
        {
            int year = 2000;
            int days = 0;
            year = (DateTime.Now).AddMonths(0).Year;
            days = DateTime.DaysInMonth(year, DateTime.Now.Month + 0);

            return days;
        }

        public int GetNoOfDaysForThisMonthNew(int year, int month)
        {
            #region Begin Old Code As on [23-12-2013]
            //int year = 2000;
            //int days = 0;
            //year = (DateTime.Now).AddMonths(-1).Year;

            //int a = DateTime.Now.Month;
            //if (a != 1)
            //{
            //    days = DateTime.DaysInMonth(year, DateTime.Now.Month - 1);
            //}
            //else
            //{
            //    days = DateTime.DaysInMonth(year, DateTime.Now.Month);
            //}

            //return days;
            #endregion End  Old Code As on [23-12-2013]


            #region Begin New Code As on  [23-12-2013]
            // int year = 2000;
            int days = 0;
            // int month = DateTime.Now.Month;
            //  year = DateTime.Now.Year;
            //if (month == 1)
            //{
            //    days = DateTime.DaysInMonth(year, DateTime.Now.Month + 11);
            //}
            //else
            //{
            //    days = DateTime.DaysInMonth(year, DateTime.Now.Month - 1);
            //}

            days = DateTime.DaysInMonth(year, month);
            return days;
            #endregion End New Code As on  [23-12-2013]


        }

        public int GetNoOfDaysForThisMonth()
        {
            #region Begin Old Code As on [23-12-2013]
            //int year = 2000;
            //int days = 0;
            //year = (DateTime.Now).AddMonths(-1).Year;

            //int a = DateTime.Now.Month;
            //if (a != 1)
            //{
            //    days = DateTime.DaysInMonth(year, DateTime.Now.Month - 1);
            //}
            //else
            //{
            //    days = DateTime.DaysInMonth(year, DateTime.Now.Month);
            //}

            //return days;
            #endregion End  Old Code As on [23-12-2013]


            #region Begin New Code As on  [23-12-2013]
            int year = 2000;
            int days = 0;
            int month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            if (month == 1)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month + 11);
            }
            else
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month - 1);
            }
            return days;
            #endregion End New Code As on  [23-12-2013]


        }

        public int GetNoOfDaysForPrviousMonth()
        {

            #region Begin New Code As on  [23-12-2013]

            // int year = 2000;
            // int days = 0;
            // year = (DateTime.Now).AddMonths(-2).Year;

            //// days = DateTime.DaysInMonth(year, DateTime.Now.Month-2);

            // int a = DateTime.Now.Month;
            // if (a == 1)
            // {
            //     days = DateTime.DaysInMonth(year, DateTime.Now.Month + 10);

            // }
            // if (a ==2)
            // {
            //     days = DateTime.DaysInMonth(year, DateTime.Now.Month + 10);
            // }

            // if (a == 3)
            // {
            //     days = DateTime.DaysInMonth(year, DateTime.Now.Month -1);
            // }

            // return days;


            #endregion End New Code As on  [23-12-2013]


            #region Begin New Code As on  [23-12-2013]
            int year = 2000;
            int days = 0;
            int month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            if (month == 1 || month == 2)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month + 10);
            }
            if (month > 2)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month - 2);
            }
            return days;
            #endregion End New Code As on  [23-12-2013]

        }

        public int GetNoOfDaysForPrviousPrviousMonth()
        {

            #region Begin Old code as on [23-12-2013]
            //int year = 2000;
            //int days = 0;
            //year = (DateTime.Now).AddMonths(-3).Year;
            //days = DateTime.DaysInMonth(year, DateTime.Now.Month - 3);

            //return days;

            #endregion End Old code as on [23-12-2013]



            #region Begin Old code as on [23-12-2013]
            int year = 2000;
            int days = 0;
            int month = DateTime.Now.Month;
            year = DateTime.Now.Year;

            if (month == 1 || month == 2 || month == 3)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month + 9);
            }
            else
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month - 3);
            }



            return days;

            #endregion End Old code as on [23-12-2013]

        }

        public DateTime GetFirstDayMonth(int year, int month)
        {
            DateTime date = new DateTime(year, month, 01);
            return date;
        }

        public DateTime GetLastDayMonth(int year, int month)
        {

            DateTime date = new DateTime(year, month, GetNoOfDaysForThisMonthNew(year, month));
            return date;
        }


        public DateTime GetFirstDayOfNextMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(0).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(0).Month, 01);

            return date;
        }

        public DateTime GetFirstDayOfThisMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(-1).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(-1).Month, 01);

            return date;
        }

        public DateTime GetFirstDayOfPreviousMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(-2).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(-2).Month, 01);

            return date;
        }

        public DateTime GetLastDayOfNextMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(0).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(0).Month, GetNoOfDaysForNextMonth());

            return date;
        }

        public DateTime GetLastDayOfThisMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(-1).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(-1).Month, GetNoOfDaysForThisMonth());

            return date;
        }

        public DateTime GetLastDayOfPreviousMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(-2).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(-2).Month, GetNoOfDaysForPrviousMonth());

            return date;
        }

        public DateTime GetLastDayOfPreviousoneMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(-3).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(-3).Month, GetNoOfDaysForPrviousPrviousMonth());

            return date;
        }

        public void AppendLog(string str)
        {
            if (appPath.Length > 0)
            {
                StreamWriter writer = File.AppendText(appPath);
                writer.WriteLine(str + " - " + DateTime.Now.ToString());
                writer.Flush();
                writer.Close();
            }
            else
            {
                string strpath = Directory.GetParent(HttpContext.Current.Request.PhysicalPath).Parent.FullName;
                string Path = strpath + "/wwwroot/data/Log.txt";//"/klts/data/Log.txt";
                if (!File.Exists(Path))
                    Path = strpath + "/klts/data/Log.txt";
                if (File.Exists(Path))
                {
                    //string Path = "E:/kunden/homepages/37/d358624936/www/App_Data/Log.txt";//"G:/Projects/Product Tracking System/Naidu/PTSystem/PTS.Web/App_Data/Log.txt";//E:/kunden/homepages/37/d358624936/www/App_Data/Log.txt
                    StreamWriter writer;
                    if (!File.Exists(Path))
                    {
                        writer = new StreamWriter(Path);
                    }
                    else
                    {
                        writer = File.AppendText(Path);
                    }
                    //writer.WriteLine("** " + str + " \n" + DateTime.Now.ToString());
                    writer.WriteLine(str + " - " + DateTime.Now.ToString());
                    writer.Flush();
                    writer.Close();
                }
                else
                {
                    Path = "";
                }
            }
        }

        public string GetOPMDesig()
        {
            string desig = "OP. Manager";
            if (System.Configuration.ConfigurationManager.AppSettings["OPMDesig"] != null)
            {
                string opmdesig = System.Configuration.ConfigurationManager.AppSettings["OPMDesig"].ToString();
                if (opmdesig.Length > 0)
                    desig = opmdesig;
            }
            return desig;
        }

        public string GetEmployeeIDPrefix()
        {
            string pref = "";
            if (System.Configuration.ConfigurationManager.AppSettings["Empname"] != null)
            {
                string epref = System.Configuration.ConfigurationManager.AppSettings["Empname"].ToString();
                if (epref.Length > 0)
                    pref = epref;
            }
            return pref;
        }

        public string GetClientIDPrefix()
        {
            string pref = "";
            if (System.Configuration.ConfigurationManager.AppSettings["Clientname"] != null)
            {
                string cpref = System.Configuration.ConfigurationManager.AppSettings["Clientname"].ToString();
                if (cpref.Length > 0)
                    pref = cpref;
            }
            return pref;
        }

        public bool GetContractIDPrefix()
        {
            bool pref = false;
            if (System.Configuration.ConfigurationManager.AppSettings["Contractname"] != null)
            {
                string cpref = System.Configuration.ConfigurationManager.AppSettings["Contractname"].ToString();
                if (cpref.Length > 0)
                {
                    try
                    {
                        int flag = Convert.ToInt32(cpref);
                        if (flag == 0)
                            pref = false;
                        else
                            pref = true;
                    }
                    catch (Exception ex) { }
                }
            }
            return pref;
        }

        public string GetServicetaxno()
        {
            string Servicetaxno = "";
            if (System.Configuration.ConfigurationManager.AppSettings["servicetaxno"] != null)
            {
                string servicetaxno = System.Configuration.ConfigurationManager.AppSettings["servicetaxno"].ToString();
                if (servicetaxno.Length > 0)
                    Servicetaxno = servicetaxno;
            }
            //string strQry = "Select * from 
            return Servicetaxno;
        }

        public string GetFontStyle()
        {
            string pref = "";
            if (System.Configuration.ConfigurationManager.AppSettings["FontStyle"] != null)
            {
                string cpref = System.Configuration.ConfigurationManager.AppSettings["FontStyle"].ToString();
                if (cpref.Length > 0)
                    pref = cpref;
            }
            return pref;
        }

        public string GetCompanyFontStyle()
        {
            string pref = "";
            if (System.Configuration.ConfigurationManager.AppSettings["CompanyFontStyle"] != null)
            {
                string cpref = System.Configuration.ConfigurationManager.AppSettings["CompanyFontStyle"].ToString();
                if (cpref.Length > 0)
                    pref = cpref;
            }
            return pref;
        }

        public string Getpanno()
        {
            string Panno = "";
            if (System.Configuration.ConfigurationManager.AppSettings["pan"] != null)
            {
                string panno = System.Configuration.ConfigurationManager.AppSettings["pan"].ToString();
                if (panno.Length > 0)
                    Panno = panno;
            }
            return Panno;
        }

        public string GetBillPrefix(bool WithST, string Branchid)
        {
            string billPrefix = "";

            string Qry = "Select BillnoWithServicetax,BillNoWithoutServiceTax from Branchdetails where branchid='" + Branchid + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;

            string BillPrefixWST = ""; string BillPrefixWOST = "";

            if (dt.Rows.Count > 0)
            {

                if (WithST)
                {
                    BillPrefixWST = dt.Rows[0]["BillnoWithServicetax"].ToString();

                    if (BillPrefixWST != null)
                    {
                        string pref = BillPrefixWST.ToString();
                        if (pref.Length > 0)
                            billPrefix = pref;
                    }
                }
                else
                {
                    BillPrefixWOST = dt.Rows[0]["BillNoWithoutServiceTax"].ToString();

                    if (BillPrefixWOST != null)
                    {
                        string pref = BillPrefixWOST.ToString();
                        if (pref.Length > 0)
                            billPrefix = pref;
                    }
                }
            }
            return billPrefix;
        }

        public string GetBillStartingNo(bool WithST)
        {
            string billNo = "00001";
            if (WithST)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["BillNoWST"] != null)
                {
                    string no = System.Configuration.ConfigurationManager.AppSettings["BillNoWST"].ToString();
                    if (no.Length > 0)
                        billNo = no;
                }
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["BillNoWOST"] != null)
                {
                    string no = System.Configuration.ConfigurationManager.AppSettings["BillNoWOST"].ToString();
                    if (no.Length > 0)
                        billNo = no;
                }
            }
            return billNo;
        }

        public string GetBillSequence()
        {
            string seq = "";
            string strQry = "Select BillSeq from CompanyInfo";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            if (dt.Rows.Count > 0)
            {
                seq = dt.Rows[0]["BillSeq"].ToString();
            }
            return seq;
        }


        public string GetType(string clientid)
        {
            string type = "";
            string strQry = "Select case isnull(TypeofWork,'0') when '1' then 'CLG' when '2' then 'SEC' when '3' then 'MP' end TypeofWork from contracts where clientid = '" + clientid + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            if (dt.Rows.Count > 0)
            {
                type = dt.Rows[0]["TypeofWork"].ToString();
            }
            return type;
        }

        public bool Checkdesignation(string desgn)
        {
            string Sqlqry = "select * From Designations  Where design='" + desgn + "'";
            System.Data.DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool Checksegment(string segname)
        {
            string Sqlqry = "select * From segments  Where segname='" + segname + "'";
            System.Data.DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public string NYAEmployeeID(string EmpIDPrefix)
        {
            DataTable dtempid;
            string EmployeeId;

            int empid = 1;

            int empidNYA = 0;
            string EmpidPrefixNYA = "NYA";
            string lengthNYA = (EmpidPrefixNYA.Trim().Length + 1 + 3).ToString();
            string length = (EmpIDPrefix.Trim().Length + 3).ToString();

            string selectqueryclientid = " Select  max(cast(substring(Tbl1.NYAEmpid,4, 6) as int)) as Empid FROM EmpDetails as Tbl1 ";
            dtempid = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;

            if (dtempid.Rows.Count > 0)
            {
                if (dtempid.Rows[0]["Empid"].ToString().Length > 0)
                {
                    empid = Convert.ToInt32(dtempid.Rows[0]["Empid"].ToString()) + 1;
                }
            }

            if (dtempid.Rows.Count == 0)
            {
                empid = int.Parse("000001");
                EmployeeId = EmpidPrefixNYA + empid.ToString("000000");
            }
            else
            {
                if (Convert.ToInt32(empid) > Convert.ToInt32(empidNYA))
                {
                    EmployeeId = EmpidPrefixNYA + empid.ToString("000000");
                }
                else
                {
                    EmployeeId = EmpidPrefixNYA + empidNYA.ToString("000000");
                }
            }

            return EmployeeId;

        }


        public string LoadMaxEmpid(string EmpidPrefix, string EmployeeType)
        {

            string SqlqryForEmpid = "select (select (Max(right(empid,8))) From empdetails where Empid like '" + EmpidPrefix + "%' and  EmployeeType='G' )  as EmpidG,(select  (Max(right(empid,8)))  From empdetails where Empid like '" + EmpidPrefix + "%'  and  EmployeeType='S') as EmpidS ";

            System.Data.DataTable dtForEmpid = config.ExecuteAdaptorAsyncWithQueryParams(SqlqryForEmpid).Result;
            int Empid = 1;
            string EmpPrefix = string.Empty;

            if (dtForEmpid.Rows.Count > 0)
            {
                if (EmployeeType == "G")
                {
                    if (String.IsNullOrEmpty(dtForEmpid.Rows[0]["EmpidG"].ToString()) == false)
                    {
                        Empid = int.Parse(dtForEmpid.Rows[0]["EmpidG"].ToString()) + 1;
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(dtForEmpid.Rows[0]["EmpidS"].ToString()) == false)
                    {
                        Empid = int.Parse(dtForEmpid.Rows[0]["EmpidS"].ToString()) + 1;
                    }
                }
            }

            if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Empname"]) == false)
            {
                EmpPrefix = System.Configuration.ConfigurationManager.AppSettings["Empname"].ToString();
            }
            return EmpidPrefix + (Empid).ToString("00000000");
        }


        public string LoadMaxClientid(string Clientidprefix)
        {
            string SqlqryForCId = "select max(right(Clientid,4)) as Clientid From Clients   where clientid like '" + Clientidprefix + "%'";
            System.Data.DataTable dtCId = config.ExecuteAdaptorAsyncWithQueryParams(SqlqryForCId).Result;
            int CId = 1;
            string ClientPrefix = string.Empty;
            if (dtCId.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dtCId.Rows[0]["Clientid"].ToString()) == false)
                {
                    CId = int.Parse(dtCId.Rows[0]["Clientid"].ToString()) + 1;
                }
            }

            if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Clientname"]) == false)
            {
                ClientPrefix = System.Configuration.ConfigurationManager.AppSettings["Clientname"].ToString();
            }

            return Clientidprefix + (CId).ToString("0000");
        }

        public DataTable LoadTaxComponents()
        {

            string ProcedureName = "GetTaxComponents";
            System.Data.DataTable DtTaxComponents = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtTaxComponents;
        }

        public DataTable LoadVATCheckforItem(string ItemID)
        {

            string ProcedureName = "GetVATCheckforItem";
            Hashtable ht = new Hashtable();
            ht.Add("@ItemID", ItemID);
            System.Data.DataTable DtVATComponents = config.ExecuteAdaptorAsyncWithParams(ProcedureName, ht).Result;
            return DtVATComponents;
        }

        public DataTable LoadEmpIds(string EmpIdPrefix)
        {
            string ProcedureName = "GetEmpids";
            Hashtable HtEmpids = new Hashtable();
            HtEmpids.Add("@EmpidPrefix", EmpIdPrefix);
            // System.Data.DataTable DtEmpIds = config.ExecuteReaderWithSPAsync(ProcedureName);
            System.Data.DataTable DtEmpIds = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtEmpids).Result;

            return DtEmpIds;
        }

        public DataTable LoadEmployeeNames(string EmpIdPrefix)
        {
            string ProcedureName = "GetEmployeeNames";
            Hashtable HtEmployeeNames = new Hashtable();
            HtEmployeeNames.Add("@EmpidPrefix", EmpIdPrefix);

            System.Data.DataTable DtEmpNames = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtEmployeeNames).Result;
            return DtEmpNames;
        }

        public DataTable LoadEmpNames(string EmpIdPrefix, DataTable BranchID)
        {
            string ProcedureName = "GetEmpNames";
            Hashtable HtEmpNames = new Hashtable();
            HtEmpNames.Add("@EmpidPrefix", EmpIdPrefix);
            HtEmpNames.Add("@Branch", BranchID);

            System.Data.DataTable DtEmpNames = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtEmpNames).Result;
            return DtEmpNames;
        }

        public DataTable LoadAttendanceEmpNames(string EmpIdPrefix, DataTable BranchID, string strid, string Date, string search)
        {
            string ProcedureName = "AttGetEmpNames";
            Hashtable HtEmpNames = new Hashtable();
            HtEmpNames.Add("@EmpidPrefix", EmpIdPrefix);
            HtEmpNames.Add("@Branch", BranchID);
            HtEmpNames.Add("@strid", strid);
            HtEmpNames.Add("@Date", Date);
            HtEmpNames.Add("@search", search);

            System.Data.DataTable DtEmpNames = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtEmpNames).Result;
            return DtEmpNames;
        }

        public DataTable LoadCIds(string ClientIdPrefix, DataTable BranchID)
        {
            string ProcedureName = "GetClientids";
            Hashtable Htclientids = new Hashtable();
            Htclientids.Add("@clientidprefix", ClientIdPrefix);
            Htclientids.Add("@Branch", BranchID);

            System.Data.DataTable DtCIds = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientids).Result;
            return DtCIds;
        }

        public DataTable LoadCIdsbasedonType(string ClientIdPrefix, DataTable BranchID)
        {
            string ProcedureName = "GetClientIDSbasedonType";
            Hashtable Htclientids = new Hashtable();
            Htclientids.Add("@clientidprefix", ClientIdPrefix);
            Htclientids.Add("@Branch", BranchID);

            System.Data.DataTable DtCIds = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientids).Result;
            return DtCIds;
        }

        public DataTable LoadClientIdBasedonMonth(string ClientIdPrefix, int Month, int type)
        {
            string ProcedureName = "GetClientIdbasedonMonth";
            Hashtable Htclientids = new Hashtable();
            Htclientids.Add("@clientidprefix", ClientIdPrefix);
            Htclientids.Add("@month", Month);
            Htclientids.Add("@Type", type);

            System.Data.DataTable DtCIds = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientids).Result;
            return DtCIds;
        }

        public DataTable LoadActiveClientnames(string ClientIdPrefix, bool SelectAll, string order, bool ExcludeClients, string BillType, int month, DataTable BranchID)
        {


            string ProcedureName = "ActiveClientsForBilling";
            Hashtable Htclientids = new Hashtable();
            Htclientids.Add("@clientidprefix", ClientIdPrefix);
            Htclientids.Add("@SelectAll", SelectAll);
            Htclientids.Add("@order", order);
            Htclientids.Add("@ExcludeClients", ExcludeClients);
            Htclientids.Add("@BIllType", BillType);
            Htclientids.Add("@Month", month);
            Htclientids.Add("@Branch", BranchID);
            System.Data.DataTable DtCIds = SqlHelper.Instance.ExecuteStoredProcedureWithParams(ProcedureName, Htclientids);
            return DtCIds;
        }

        public DataTable LoadCNames(string ClientIdPrefix, DataTable BranchID)
        {
            string ProcedureName = "GetClientNames";
            Hashtable Htclientnames = new Hashtable();
            Htclientnames.Add("@clientidprefix", ClientIdPrefix);
            Htclientnames.Add("@Branch", BranchID);
            System.Data.DataTable DtCNames = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientnames).Result;
            return DtCNames;
        }

        public DataTable LoadCNamesbasedonType(string ClientIdPrefix, DataTable BranchID)
        {
            string ProcedureName = "GetClientNamesbasedonType";
            Hashtable Htclientnames = new Hashtable();
            Htclientnames.Add("@clientidprefix", ClientIdPrefix);
            Htclientnames.Add("@Branch", BranchID);
            System.Data.DataTable DtCNames = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientnames).Result;
            return DtCNames;
        }

        public DataTable LoadMainunitCIds(string ClientIdPrefix)
        {
            string ProcedureName = "ReportForLoadMainUnitIds";
            Hashtable Htclientnames = new Hashtable();
            Htclientnames.Add("@Clientidprefix", ClientIdPrefix);
            System.Data.DataTable DtCNames = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientnames).Result;
            return DtCNames;
        }

        public DataTable LoadMainunitCNames(string ClientIdPrefix)
        {
            string ProcedureName = "ReportForLoadMainUnitNames";
            Hashtable Htclientnames = new Hashtable();
            Htclientnames.Add("@Clientidprefix", ClientIdPrefix);
            System.Data.DataTable DtCNames = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientnames).Result;
            return DtCNames;
        }

        public DataTable LoadOpManagerIds()
        {
            string opmDesig = GlobalData.Instance.GetOPMDesig();
            string Sqlqry = "select  Empid  From Empdetails Where  EmpDesgn = '" + opmDesig + "' " + " Order By Right(Empid,6)";
            System.Data.DataTable DtEmpIds = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            return DtEmpIds;
        }

        public DataTable LoadOpManagerNames()
        {
            string opmDesig = GlobalData.Instance.GetOPMDesig();
            string Sqlqry = "select  Empid,(ISnull(Empfname,'')+' '+ Isnull(empmname,'')+' '+ isnull(Emplname,'') ) AS FullName " +
                           "  From Empdetails   Where  EmpDesgn = '" + opmDesig + "' " + "  Order by  (ISnull(Empfname,'')+' '+ Isnull(empmname,'')+' '+ isnull(Emplname,'') ) ";
            System.Data.DataTable DtEmpNames = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            return DtEmpNames;
        }

        public DataTable LoadDesigns()
        {

            string ProcedureName = "GetDesignations";
            System.Data.DataTable DtDesigns = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtDesigns;
        }

        public DataTable LoadLoanTypes()
        {

            string ProcedureName = "GetLoanTypes";
            System.Data.DataTable DtDesigns = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtDesigns;
        }

        public DataTable LoadItemNames()
        {

            string ProcedureName = "GetItemNames";
            System.Data.DataTable DtDesigns = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtDesigns;
        }

        public DataTable LoadWageCategory()
        {

            string ProcedureName = "GetMinWageCategories";
            System.Data.DataTable DtCategories = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtCategories;
        }
        public DataTable LoadDepartments()
        {

            string ProcedureName = "GetDepartments";
            System.Data.DataTable DtDepartments = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtDepartments;

        }
        public DataTable LoadPFbranches()
        {
            string ProcedureName = "GetPFBranches";
            System.Data.DataTable dtEsibranches = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return dtEsibranches;
        }


        public DataTable LoadAllBranch()
        {

            string ProcedureName = "GetBranch";
            System.Data.DataTable DtAllBranches = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtAllBranches;

        }

        public DataTable LoadLoginBranch(DataTable BranchID)
        {

            string ProcedureName = "GetBranchLoginWise";
            Hashtable Htclientnames = new Hashtable();
            Htclientnames.Add("@Branch", BranchID);
            System.Data.DataTable DtAllBranches = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientnames).Result;
            return DtAllBranches;

        }

        public DataTable LoadBankNames()
        {

            string ProcedureName = "GetBankNames";
            System.Data.DataTable DtBankNames = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtBankNames;

        }

        public DataTable LoadStateNames()
        {

            string ProcedureName = "GetStateNames";
            System.Data.DataTable DtStateNames = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtStateNames;

        }

        public DataTable GetMonthNames()
        {

            string ProcedureName = "GetMonthNames";
            System.Data.DataTable DtStateNames = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtStateNames;

        }

        public DataTable LoadEsibranches()
        {
            string ProcedureName = "GetEsiBranches";
            System.Data.DataTable dtEsibranches = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return dtEsibranches;
        }

        public DataTable LoadSegments()
        {

            string ProcedureName = "GetSegments";
            System.Data.DataTable DtSegments = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtSegments;

        }

        public DataTable LoadDivision()
        {

            string ProcedureName = "GetDivisions";
            System.Data.DataTable DtDivisions = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtDivisions;

        }

        public DataTable LoadCategories()
        {

            string ProcedureName = "GetCategories";
            System.Data.DataTable DtCategories = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtCategories;

        }

        public DataTable LoadResources()
        {

            string ProcedureName = "GetResources";
            System.Data.DataTable DtSegments = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtSegments;

        }

        public DataTable LoadItems()
        {

            string ProcedureName = "GetItems";
            System.Data.DataTable DtItems = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtItems;

        }

        public DataTable LoadBloodGroupNames()
        {

            string ProcedureName = "GetBloodGroup";
            System.Data.DataTable DtBloodGroup = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtBloodGroup;

        }


        public DataTable LoadPreviligers()
        {

            string ProcedureName = "GetPreviligers";
            System.Data.DataTable DtPreviligers = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtPreviligers;

        }


        public DataTable LoadTblOptionsData(string ID)
        {

            string ProcedureName = "GetTblOptions";
            Hashtable HtTblOptions = new Hashtable();
            HtTblOptions.Add("@ID", ID);
            System.Data.DataTable DtItems = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtTblOptions).Result;
            return DtItems;

        }


        public DataTable LoadHSNNumbers()
        {

            string ProcedureName = "GetHSNNos";
            System.Data.DataTable DtHSNNos = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtHSNNos;
        }

        public DataTable LoadGSTNumbers(DataTable BranchID)
        {

            string ProcedureName = "GetGSTNos";
            Hashtable HtTblOptions = new Hashtable();
            HtTblOptions.Add("@Branch", BranchID);
            System.Data.DataTable DtGSTNos = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtTblOptions).Result;
            return DtGSTNos;
        }



        public DataTable LoadProfTaxData()
        {

            string ProcedureName = "GetProfTax";
            System.Data.DataTable DtItems = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtItems;

        }


        public DataTable LoadActiveEmployees(string EmpidPrefix)
        {

            string ProcedureName = "GetActiveEmployees";
            Hashtable HtActiveEmployees = new Hashtable();
            HtActiveEmployees.Add("@empidprefix", EmpidPrefix);
            // System.Data.DataTable DtActiveEmployees = config.ExecuteReaderWithSPAsync(ProcedureName);
            System.Data.DataTable DtActiveEmployees = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtActiveEmployees).Result;

            return DtActiveEmployees;

        }

        public DataTable LoadActiveEmployeesOrderBy(string EmpidPrefix, int OrderBy, DataTable BranchIDs)
        {

            string ProcedureName = "IMActiveEmployeesOrderBy";
            Hashtable HtActiveEmployees = new Hashtable();
            HtActiveEmployees.Add("@empidprefix", EmpidPrefix);
            HtActiveEmployees.Add("@OrderBy", OrderBy);
            HtActiveEmployees.Add("@Branch", BranchIDs);
            System.Data.DataTable DtActiveEmployees = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtActiveEmployees).Result;

            return DtActiveEmployees;

        }


        public DataTable LoadBranchOnUserID(string Branch)
        {
            List<string> list = Branch.Split(',').ToList();
            DataTable dtBranch = new DataTable();
            dtBranch.Columns.Add("Branch");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtBranch.NewRow();
                    row["Branch"] = s;
                    dtBranch.Rows.Add(row);
                }
            }

            return dtBranch;
        }

        public DataTable LoadInActiveEmployees()
        {

            string ProcedureName = "GetInActiveEmployees";
            System.Data.DataTable DtInActiveEmployees = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtInActiveEmployees;

        }


        public DataTable LoadActiveClients()
        {

            string ProcedureName = "GetActiveClients";
            System.Data.DataTable DtActiveClients = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtActiveClients;

        }


        public DataTable LoadInActiveClients()
        {

            string ProcedureName = "GetInActiveClients";
            System.Data.DataTable DtInActiveClients = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtInActiveClients;

        }


        public DataTable LoadEmployeesBasedOnTheSearch()
        {

            string ProcedureName = "GetEmployeesSearchBase";
            System.Data.DataTable DtActiveClients = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtActiveClients;

        }


        public DataTable LoadClientsBasedOnTheSearch()
        {

            string ProcedureName = "GetInActiveClients";
            System.Data.DataTable DtInActiveClients = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtInActiveClients;

        }


        public int CheckEnteredDate(string ReciviedDate)
        {
            var TestDate = 0;

            if (ReciviedDate.Trim().Length != 10)
            {
                return TestDate = 1;
            }
            int NoOfTimes = 0;
            foreach (char ch in ReciviedDate)
            {
                if (ch == '/')
                {
                    NoOfTimes++;
                }
            }
            if (NoOfTimes > 2 || NoOfTimes < 2)
            {
                TestDate = 1;
            }

            return TestDate;

        }

        public int GetMonth(int indexfromddlmonth)
        {
            int month = 0;
            if (indexfromddlmonth == 1)
            {
                month = GlobalData.Instance.GetIDForNextMonth();
            }
            if (indexfromddlmonth == 2)
            {
                month = GlobalData.Instance.GetIDForThisMonth();
            }

            if (indexfromddlmonth == 3)
            {
                month = GlobalData.Instance.GetIDForPrviousMonth();
            }
            return month;
        }

        public int CountSundays(int year, int month)
        {
            var firstDay = new DateTime(year, month, 1);

            var day29 = firstDay.AddDays(28);
            var day30 = firstDay.AddDays(29);
            var day31 = firstDay.AddDays(30);

            if ((day29.Month == month && day29.DayOfWeek == DayOfWeek.Sunday)
            || (day30.Month == month && day30.DayOfWeek == DayOfWeek.Sunday)
            || (day31.Month == month && day31.DayOfWeek == DayOfWeek.Sunday))
            {
                return 5;
            }
            else
            {
                return 4;
            }
        }

        public DataTable LoadExpensesPurpose()
        {
            string ProcedureName = "GetExpensesPurposeNames";
            System.Data.DataTable DtExpensesPurpose = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtExpensesPurpose;
        }

        public DataTable LoadExpensesapprovedBy()
        {
            string ProcedureName = "GetExpensesApprovedBy";
            System.Data.DataTable DtExpensesPurpose = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtExpensesPurpose;
        }

        public DataTable LoadPayment_Mode()
        {
            string ProcedureName = "GetLoadPayment_Mode_Names";
            System.Data.DataTable DtExpensesPurpose = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtExpensesPurpose;
        }

        public DataTable LoadMinimumWagesCategories()
        {

            string ProcedureName = "GetMinimumWagesCategories";
            System.Data.DataTable DtSegments = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtSegments;

        }

        public string CheckDateFormat(string RecivedDate)
        {

            string OutPutDate = "";

            if (RecivedDate.Trim().Length > 0)
            {
                int CSD = int.Parse(RecivedDate.Substring(0, 2));
                if (CSD > 12)
                {
                    OutPutDate = DateTime.Parse(RecivedDate, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    // en-US
                    try
                    {
                        CSD = int.Parse(OutPutDate.Substring(0, 2));
                    }
                    catch
                    {
                        CSD = int.Parse(OutPutDate.Substring(0, 1));
                    }



                    if (CSD > 12)
                    {
                        OutPutDate = DateTime.Parse(OutPutDate).ToString("MM/dd/yyyy");
                    }

                }
                else
                {
                    OutPutDate = RecivedDate;


                    string Day = OutPutDate.Substring(0, 2);
                    string Month = OutPutDate.Substring(3, 2);
                    string Year = OutPutDate.Substring(6, 4);



                    OutPutDate = Month + "/" + Day + "/" + Year;


                }
            }
            else
            {
                OutPutDate = "01/01/1900";

            }

            return OutPutDate;
        }

        public DataTable LoadExpensesVoucherNos(int OperationType)
        {

            string ProcedureName = "GetVoucherNos";
            Hashtable HtActiveEmployees = new Hashtable();
            HtActiveEmployees.Add("@OperationType", OperationType);
            System.Data.DataTable DtSegments = config.ExecuteReaderWithSPAsync(ProcedureName).Result;
            return DtSegments;
        }

        public string LoadMaxContractId(string ClientId)
        {
            #region Begin Variable Declaration
            var SPName = "";
            Hashtable HTContractId = new Hashtable();
            #endregion  End Variable Declaration

            #region Begin Assign Values To the Variables
            SPName = "LoadMaxContractId";
            HTContractId.Add("@Clientid", ClientId);
            #endregion  End  Assign Values To the Variables

            #region Begin Calling Stored PRocedure
            DataTable DtContractId = config.ExecuteAdaptorAsyncWithParams(SPName, HTContractId).Result;
            if (DtContractId.Rows.Count > 0 && DtContractId.Rows[0]["contractid"].ToString().Trim().Length > 0)
            {
                return ClientId + "/" + (int.Parse(DtContractId.Rows[0]["contractid"].ToString()) + 1).ToString("00");
            }
            else
            {
                return ClientId + "/" + "01";
            }
            #endregion  End Calling Stored PRocedure


        }

        public DataTable LoadAllContractIds(string ClientId)
        {
            #region Begin Variable Declaration
            var SPName = "";
            Hashtable HTContractIds = new Hashtable();
            #endregion  End Variable Declaration

            #region Begin Assign Values To the Variables
            SPName = "LoadAllContractIds";
            HTContractIds.Add("@Clientid", ClientId);
            #endregion  End  Assign Values To the Variables

            #region Begin Calling Stored PRocedure
            DataTable DtContractIds = config.ExecuteAdaptorAsyncWithParams(SPName, HTContractIds).Result;
            return DtContractIds;
            #endregion  End Calling Stored PRocedure
        }

        public string LoadMaxId(string ID)
        {
            #region Begin Variable Declaration
            var SPName = "";
            Hashtable HTContractId = new Hashtable();
            #endregion  End Variable Declaration

            #region Begin Assign Values To the Variables
            SPName = "LoadMaxId";
            HTContractId.Add("@empid", ID);
            #endregion  End  Assign Values To the Variables

            #region Begin Calling Stored PRocedure
            DataTable DtContractId = config.ExecuteAdaptorAsyncWithParams(SPName, HTContractId).Result;
            if (DtContractId.Rows.Count > 0 && DtContractId.Rows[0]["id"].ToString().Trim().Length > 0)
            {
                int NewID = int.Parse(DtContractId.Rows[0]["id"].ToString());
                return (NewID + 1).ToString("00");
            }
            else
            {
                return ID + "/" + "1";
            }
            #endregion  End Calling Stored PRocedure


        }

        public DataTable LoadInvBranches(string BranchID)
        {
            string ProcedureName = "GetInvBranch";
            Hashtable Htclientnames = new Hashtable();
            Htclientnames.Add("@Branch", BranchID);
            System.Data.DataTable DtAllBranches = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htclientnames).Result;
            return DtAllBranches;
        }

    }

    public static class ApplicationSessionStateStore
    {
        private static bool _enabled;

        /// <summary>
        /// Enables this class for use.  This is a safety check.  By invoking this method, 
        /// the caller commits to calling <c>RemoveAllItems</c> when sessions terminate.
        /// </summary>
        public static bool IsEnabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public static void AssertEnabled()
        {
            if (!_enabled)
            {
                throw new InvalidOperationException("Use of ApplicationSessionStateStore is not enabled.  See 'IsEnabled' property documentation for proper usage.");
            }
        }

        public static T GetItem<T>(string sessionId, string key) where T : class
        {
            return GetItem<T>(sessionId, key, false);
        }

        public static T GetItem<T>(string key) where T : class
        {
            return GetItem<T>(HttpContext.Current.Session.SessionID, key, false);
        }

        public static T GetAndRemoveItem<T>(string sessionId, string key) where T : class
        {
            return GetItem<T>(sessionId, key, true);
        }

        public static T GetAndRemoveItem<T>(string key) where T : class
        {
            return GetItem<T>(HttpContext.Current.Session.SessionID, key, true);
        }

        public static void SetItem(string key, object value)
        {
            SetItem(HttpContext.Current.Session.SessionID, key, value);
        }

        public static void SetItem(string sessionId, string key, object value)
        {
            AssertEnabled();
            HttpContext context = HttpContext.Current;
            HttpApplicationState appState = context.Application;
            IDictionary<string, object> sessionDic;

            appState.Lock();
            try
            {
                sessionDic = (IDictionary<string, object>)appState.Get(sessionId);
                if (sessionDic == null)
                {
                    sessionDic = new Dictionary<string, object>();
                    appState.Set(sessionId, sessionDic);
                }
            }
            finally
            {
                appState.UnLock();
            }

            lock (sessionDic)
            {
                sessionDic[key] = value;
            }
        }

        public static void RemoveAllItems()
        {
            RemoveAllItems(HttpContext.Current.Session.SessionID);
        }

        public static void RemoveAllItems(string sessionId)
        {
            HttpContext.Current.Application.Remove(sessionId);
        }

        private static T GetItem<T>(string sessionId, string key, bool removeItem) where T : class
        {
            AssertEnabled();
            HttpContext context = HttpContext.Current;
            HttpApplicationState appState = context.Application;
            IDictionary<string, object> sessionDic = (IDictionary<string, object>)appState.Get(sessionId);

            if (sessionDic == null)
            {
                return null;
            }
            else
            {
                object value;
                lock (sessionDic)
                {
                    if (sessionDic.TryGetValue(key, out value))
                    {
                        if (removeItem)
                        {
                            sessionDic.Remove(key);
                        }
                    }
                }
                return value as T;
            }
        }
    }

    public class Timings
    {
        public int GetIdForSelectedMonthPrevious(int SelectedIndex)
        {
            var month = 0;
            if (SelectedIndex == 1)
            {
                month = GetIdForPreviousMonth();
            }

            if (SelectedIndex == 2)
            {
                month = GetIdForPreviousOneMonth();
            }

            if (SelectedIndex == 3)
            {
                month = GetIdForPreviousTwoMonth();
            }
            return month;
        }


        private static Timings _Instance;
        public static Timings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Timings();
                }
                return _Instance;
            }
        }

        public int GetIdForSelectedMonth(int SelectedIndex)
        {
            var month = 0;
            if (SelectedIndex == 1)
            {
                month = GetIdForThisMonth();
            }

            if (SelectedIndex == 2)
            {
                month = GetIdForPreviousMonth();
            }

            if (SelectedIndex == 3)
            {
                month = GetIdForPreviousOneMonth();
            }

            if (SelectedIndex == 4)
            {
                month = GetIdForPreviousTwoMonth();
            }
            return month;
        }

        public DateTime GetDateForSelectedMonth(int SelectedIndex)
        {
            var month = DateTime.Now;
            if (SelectedIndex == 1)
            {
                month = GetFirstDayOfThisMonth();
            }

            if (SelectedIndex == 2)
            {
                month = GetFirstDayOfPreviousMonth();
            }

            if (SelectedIndex == 3)
            {
                month = GetFirstDayOfPreviousOneMonth();
            }

            if (SelectedIndex == 4)
            {
                month = GetLastDayOfPreviousTwoMonth();
            }
            return month;
        }

        public int GetReverseIdForSelectedMonth(int SelectedIndex)
        {
            var month = 0;
            if (SelectedIndex == 1)
            {
                int year = 2000;
                year = (DateTime.Now).AddMonths(0).Year;
                string mon = string.Format("{0:yy}{1}", (year - 2000).ToString(), DateTime.Now.Month + 0);
                month = Convert.ToInt32(mon);
                return month;
            }

            if (SelectedIndex == 2)
            {
                int year = 2000;
                int CMonth = DateTime.Now.Month;
                string mon = "";

                year = (CMonth == 1)
                    ? (DateTime.Now).AddMonths(0).Year - 1
                    : year = (DateTime.Now).AddMonths(0).Year;

                mon = (CMonth == 1)
                         ? string.Format("{0:yy}{1}", (year - 2000).ToString(), DateTime.Now.Month + 11)
                         : mon = string.Format("{0:yy}{1}", (year - 2000).ToString(), DateTime.Now.Month - 1);

                month = Convert.ToInt32(mon);

                return month;
            }

            if (SelectedIndex == 3)
            {
                int year = 2000;
                int CMonth = DateTime.Now.Month;
                string mon = "";
                year = (CMonth == 2 || CMonth == 1)
                  ? (DateTime.Now).AddMonths(0).Year - 1
                  : year = (DateTime.Now).AddMonths(0).Year;

                mon = (CMonth == 2 || CMonth == 1)
                        ? string.Format("{0:yy}{1}", (year - 2000).ToString(), DateTime.Now.Month + 10)
                        : mon = string.Format("{0:yy}{1}", (year - 2000).ToString(), DateTime.Now.Month - 2);

                month = Convert.ToInt32(mon);

                return month;
            }

            return month;
        }
        //New Code

        public DateTime GetDateWithMonthString(string month)
        {
            var yearstring = month.Substring(month.Length - 2);
            var monthstring = month.Replace(yearstring, "");
            var twoDigitYear = int.Parse(yearstring);
            int fourDigitYear = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar.ToFourDigitYear(twoDigitYear);
            var monthdigit = int.Parse(monthstring);
            var relievedate = new DateTime(fourDigitYear, monthdigit, 1);
            return relievedate;
        }

        public int GetIdForEnteredMOnth(DateTime Enterdate)
        {
            var month = 0;

            DateTime Date = Enterdate;
            month = int.Parse(Date.Month.ToString() + Date.Year.ToString().Substring(2, 2));

            return month;
        }

        public int GetIdForThisMonth()
        {
            int year = 2000;

            year = (DateTime.Now).AddMonths(0).Year;

            string mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 0, (year - 2000).ToString());
            int month = Convert.ToInt32(mon);

            return month;
        }

        public int GetIdForPreviousMonth()
        {
            int year = 2000;
            int CMonth = DateTime.Now.Month;
            string mon = "";
            if (CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }

            if (CMonth == 1)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 11, (year - 2000).ToString());
            }
            else
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 1, (year - 2000).ToString());
            }
            int month = Convert.ToInt32(mon);

            return month;
        }

        public int GetIdForPreviousOneMonth()
        {
            int year = 2000;

            int CMonth = DateTime.Now.Month;
            string mon = "";
            if (CMonth == 2 || CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }



            if (CMonth == 2 || CMonth == 1)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 10, (year - 2000).ToString());
            }
            else
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 2, (year - 2000).ToString());
            }

            int month = Convert.ToInt32(mon);

            return month;
        }

        public int GetIdForPreviousTwoMonth()
        {
            int year = 2000;
            string mon = "";
            int CMonth = DateTime.Now.Month;
            if (CMonth == 3 || CMonth == 2 || CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }

            if (CMonth == 3 || CMonth == 2 || CMonth == 1)
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 9, (year - 2000).ToString());
            }
            else
            {
                mon = string.Format("{0}{1:yy}", DateTime.Now.Month - 3, (year - 2000).ToString());
            }

            //string mon = string.Format("{0}{1:yy}", DateTime.Now.Month + 0, (year - 2000).ToString());
            int month = Convert.ToInt32(mon);

            return month;
        }

        public DateTime GetFirstDayOfThisMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(0).Year;
            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(0).Month, 01);
            return date;

        }

        public DateTime GetFirstDayOfPreviousMonth()
        {
            int year = 2000;
            int CMonth = DateTime.Now.Month;
            DateTime date;
            if (CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }
            if (CMonth == 1)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(11).Month, 01);
            }
            else
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(-1).Month, 01);
            }


            return date;

        }

        public DateTime GetFirstDayOfPreviousOneMonth()
        {
            int year = 2000;
            int CMonth = DateTime.Now.Month;
            DateTime date;
            if (CMonth == 2 || CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }
            if (CMonth == 2 || CMonth == 1)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(10).Month, 01);
            }
            else
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(-2).Month, 01);
            }
            return date;

        }

        public DateTime GetFirstDayOfPreviousTwoMonth()
        {
            int year = 2000;
            int CMonth = DateTime.Now.Month;
            DateTime date;
            if (CMonth == 1 || CMonth == 2 || CMonth == 3)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }
            if (CMonth == 1 || CMonth == 2 || CMonth == 3)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(9).Month, 01);
            }
            else
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(-3).Month, 01);
            }
            return date;

        }


        public DateTime GetLastDayOfThisMonth()
        {
            int year = 2000;
            year = (DateTime.Now).AddMonths(0).Year;

            DateTime date = new DateTime(year, (DateTime.Now).AddMonths(0).Month, GetNoOfDaysForThisMonth());
            return date;
        }

        public DateTime GetLastDayOfPreviousMonth()
        {
            int year = 2000;
            int CMonth = DateTime.Now.Month;
            DateTime date;
            if (CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }

            if (CMonth == 1)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(11).Month, GetNoOfDaysForPreviousMonth());
            }
            else
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(-1).Month, GetNoOfDaysForPreviousMonth());
            }



            return date;
        }

        public DateTime GetLastDayOfPreviousOneMonth()
        {
            int year = 2000;
            int CMonth = DateTime.Now.Month;
            DateTime date = DateTime.Now.Date;
            if (CMonth == 2 || CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }

            if (CMonth == 2 || CMonth == 1)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(10).Month, GetNoOfDaysForPreviousOneMonth());
            }

            if (CMonth != 2 && CMonth != 1)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(-2).Month, GetNoOfDaysForPreviousOneMonth());
            }


            return date;
        }

        public DateTime GetLastDayOfPreviousTwoMonth()
        {
            int year = 2000;
            int CMonth = DateTime.Now.Month;
            DateTime date = DateTime.Now.Date;
            if (CMonth == 1 || CMonth == 2 || CMonth == 3)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }

            if (CMonth == 3 || CMonth == 2 || CMonth == 1)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(9).Month, GetNoOfDaysForPreviousTwoMonth());
            }


            if (CMonth != 1 && CMonth != 2 && CMonth != 3)
            {
                date = new DateTime(year, (DateTime.Now).AddMonths(-3).Month, GetNoOfDaysForPreviousTwoMonth());
            }

            return date;
        }

        public int GetNoofDaysForSelectedMonth(int SelectedIndex, int bBillDates)
        {
            var days = 0;
            if (SelectedIndex == 1)
            {
                if (bBillDates == 1 || bBillDates == 2 || bBillDates == 3)
                {
                    days = GetNoOfDaysForPreviousMonth();
                }
                else
                {
                    days = GetNoOfDaysForThisMonth();
                }
            }

            if (SelectedIndex == 2)
            {

                if (bBillDates == 1 || bBillDates == 2 || bBillDates == 3)
                {
                    days = GetNoOfDaysForPreviousOneMonth();
                }
                else
                {
                    days = GetNoOfDaysForPreviousMonth();
                }
            }

            if (SelectedIndex == 3)
            {
                if (bBillDates == 1 || bBillDates == 2 || bBillDates == 3)
                {
                    days = GetNoOfDaysForPreviousTwoMonth();
                }
                else
                {
                    days = GetNoOfDaysForPreviousOneMonth();
                }
            }

            if (SelectedIndex == 4)
            {
                days = GetNoOfDaysForPreviousTwoMonth();
            }
            return days;

        }

        //New Code 

        public int GetNoofDaysForEnteredMonth(DateTime date, int bBillDates)
        {
            var days = 0;
            DateTime Monthdays = date;

            if (bBillDates == 2 || bBillDates == 3)
            {
                days = DateTime.DaysInMonth(Monthdays.Year, Monthdays.Month - 1);
            }
            else
            {
                days = DateTime.DaysInMonth(Monthdays.Year, Monthdays.Month);
            }

            return days;

        }


        public int GetNoOfDaysForThisMonth()
        {
            int year = 2000;
            int days = 0;
            year = (DateTime.Now).AddMonths(0).Year;
            days = DateTime.DaysInMonth(year, DateTime.Now.Month + 0);
            return days;

        }

        public int GetNoOfDaysForPreviousMonth()
        {
            int year = 2000;
            int days = 0;
            int CMonth = DateTime.Now.Month;
            if (CMonth == 1)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }

            if (CMonth == 1)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month + 11);
            }
            else
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month - 1);
            }

            return days;
        }

        public int GetNoOfDaysForPreviousOneMonth()
        {
            int year = 2000;
            int days = 0;
            int CMonth = DateTime.Now.Month;
            if (CMonth == 1 || CMonth == 2)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }

            if (CMonth == 1 || CMonth == 2)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month + 10);
            }

            if (CMonth != 1 && CMonth != 2)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month - 2);
            }

            return days;
        }

        public int GetNoOfDaysForPreviousTwoMonth()
        {
            int year = 2000;
            int days = 0;
            int CMonth = DateTime.Now.Month;
            if (CMonth == 1 || CMonth == 2 || CMonth == 3)
            {
                year = (DateTime.Now).AddMonths(0).Year - 1;
            }
            else
            {
                year = (DateTime.Now).AddMonths(0).Year;
            }


            if (CMonth == 1 || CMonth == 2 || CMonth == 3)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month + 9);
            }

            if (CMonth != 1 && CMonth != 2 && CMonth != 3)
            {
                days = DateTime.DaysInMonth(year, DateTime.Now.Month - 3);
            }

            return days;
        }


        public string CheckDateFormat(string RecivedDate)
        {

            string OutPutDate = "";

            if (RecivedDate.Trim().Length > 0)
            {
                int CSD = int.Parse(RecivedDate.Substring(0, 2));
                if (CSD > 12)
                {
                    OutPutDate = DateTime.Parse(RecivedDate, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    // en-US
                    try
                    {
                        CSD = int.Parse(OutPutDate.Substring(0, 2));
                    }
                    catch
                    {
                        CSD = int.Parse(OutPutDate.Substring(0, 1));
                    }



                    if (CSD > 12)
                    {
                        OutPutDate = DateTime.Parse(OutPutDate).ToString("MM/dd/yyyy");
                    }

                }
                else
                {
                    OutPutDate = RecivedDate;


                    string Day = OutPutDate.Substring(0, 2);
                    string Month = OutPutDate.Substring(3, 2);
                    string Year = OutPutDate.Substring(6, 4);



                    OutPutDate = Month + "/" + Day + "/" + Year;


                }
            }
            else
            {
                OutPutDate = "01/01/1900";

            }

            return OutPutDate;
        }

        public int CheckEnteredDate(string ReciviedDate)
        {
            var TestDate = 0;

            if (ReciviedDate.Trim().Length != 10)
            {
                return TestDate = 1;
            }
            int NoOfTimes = 0;
            foreach (char ch in ReciviedDate)
            {
                if (ch == '/')
                {
                    NoOfTimes++;
                }
            }
            if (NoOfTimes > 2 || NoOfTimes < 2)
            {
                TestDate = 1;
            }

            return TestDate;

        }

        public DateTime GetLastDayForSelectedMonth(int SelectedIndex)
        {
            if (SelectedIndex == 1)
            {
                return Timings.Instance.GetLastDayOfThisMonth();
            }
            if (SelectedIndex == 2)
            {
                return Timings.Instance.GetLastDayOfPreviousMonth();
            }
            if (SelectedIndex == 3)
            {
                return Timings.Instance.GetLastDayOfPreviousOneMonth();
            }
            if (SelectedIndex == 4)
            {
                return Timings.Instance.GetLastDayOfPreviousTwoMonth();
            }
            return DateTime.Now.Date;
        }

        public int Get_GS_Days(int month, int GenDays)
        {

            int mn = 0;
            int yr = 0;

            if (month.ToString().Trim().Length == 3)
            {
                mn = int.Parse(month.ToString().Substring(0, 1));
                yr = int.Parse(month.ToString().Substring(1, 2));
            }
            if (month.ToString().Trim().Length == 4)
            {
                mn = int.Parse(month.ToString().Substring(0, 2));
                yr = int.Parse(month.ToString().Substring(2, 2));
            }
            return GenDays - CountSundays(yr, mn);

        }

        static int CountSundays(int year, int month)
        {
            var firstDay = new DateTime(year, month, 1);

            var day29 = firstDay.AddDays(28);
            var day30 = firstDay.AddDays(29);
            var day31 = firstDay.AddDays(30);

            if ((day29.Month == month && day29.DayOfWeek == DayOfWeek.Sunday)
            || (day30.Month == month && day30.DayOfWeek == DayOfWeek.Sunday)
            || (day31.Month == month && day31.DayOfWeek == DayOfWeek.Sunday))
            {
                return 5;
            }
            else
            {
                return 4;
            }
        }

    }

    public class NumberToEnglish
    {
        private NumberToEnglish() { }

        private static NumberToEnglish _Instance;

        public static NumberToEnglish Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new NumberToEnglish();
                }
                return _Instance;
            }
        }

        public String changeNumericToWords(double numb)
        {
            String num = numb.ToString();
            return changeToWords(num, false);
        }

        public String changeCurrencyToWords(String numb)
        {
            return changeToWords(numb, true);
        }

        public String changeNumericToWords(String numb)
        {
            return changeToWords(numb, false);
        }

        public String changeCurrencyToWords(double numb)
        {
            return changeToWords(numb.ToString(), true);
        }

        private String changeToWords(String numb, bool isCurrency)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = (isCurrency) ? ("Only") : ("");
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = (isCurrency) ? ("Rupees and") : ("point");// just to separate whole numbers from points/cents
                        endStr = (isCurrency) ? ("Paisa " + endStr) : ("");
                        pointStr = translateCents(points);
                    }
                }
                else
                {
                    endStr = (isCurrency) ? ("Rupees Only") : ("");
                }
                val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch {; }
            return val;
        }

        private String translateWholeNumber(String number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = number.StartsWith("0");
                    int numDigits = number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            if (beginsZero == false)
                                place = " Hundred ";
                            break;
                        case 4://thousands' range
                        case 5:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 6:
                        case 7://Lakhs' range
                            pos = (numDigits % 6) + 1;
                            place = " Lakh ";
                            break;
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12://Crore's range
                            pos = (numDigits % 8) + 1;
                            place = " Crore ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }

                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));
                        //check for trailing zeros
                        if (beginsZero) word = " and " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch {; }
            return word.Trim();
        }

        private String tens(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = null;
            switch (digt)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Forty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private String ones(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";
            switch (digt)
            {

                case 0:
                    name = "Zero";
                    break;

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private String translateCents(String cents)
        {
            String cts = "", digit = "", engOne = "";
            for (int i = 0; i < cents.Length; i++)
            {
                digit = cents[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cts += " " + engOne;
            }
            return cts;
        }

        public string NumbersToWords(long inputNumber, bool isIntegral)
        {
            long inputNo = inputNumber;

            if (inputNo == 0)
                return "Zero";

            long[] numbers = new long[4];
            int first = 0;
            long u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0)
                    {
                        if (isIntegral)
                        {
                            sb.Append(" ");
                        }
                        else { sb.Append(""); }
                    }
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
        public string NumbersToWordsDecimal(long inputNumber, bool isIntegral)
        {
            long inputNo = inputNumber;

            if (inputNo == 0)
                return "Zero";

            long[] numbers = new long[4];
            int first = 0;
            long u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0)
                    {
                        if (isIntegral)
                        {
                            sb.Append(" ");
                        }
                        else { sb.Append(""); }
                    }
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
    }

    /// <summary>
    /// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
    /// ability to discover parameters for stored procedures at run-time.
    /// </summary>

    internal sealed class SqlHelperParameterCache
    {
        #region private methods, variables, and constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new SqlHelperParameterCache()"
        private SqlHelperParameterCache() { }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">Whether or not to include their return value parameter</param>
        /// <returns>The parameter array discovered.</returns>
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary>
        /// Deep copy of cached SqlParameter array
        /// </summary>
        /// <param name="originalParameters"></param>
        /// <returns></returns>
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion private methods, variables, and constructors

        #region caching functions

        /// <summary>
        /// Add parameter array to the cache
        /// </summary>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters to be cached</param>
        internal static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve a parameter array from the cache
        /// </summary>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An array of SqlParamters</returns>
        internal static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion caching functions

        #region Parameter Discovery Functions

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of SqlParameters</returns>
        internal static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        internal static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of SqlParameters</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters;

            cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion Parameter Discovery Functions
    }
}
