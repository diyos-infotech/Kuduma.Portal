using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Collections;

namespace Kuduma.Portal.DAL
{
    /// <summary>
    /// Sets the default configuration.
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppConfiguration()
        {
            command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// Sets the connection string from web.config
        /// </summary>
        string connectionString = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        string CentralconnectionString = ConfigurationManager.ConnectionStrings["CentralConnectionString"].ConnectionString;
        string PocketFameConnectionString = ConfigurationManager.ConnectionStrings["PocketFameConnectionString"].ConnectionString;

        /// <summary>
        /// Command object
        /// </summary>
        public SqlCommand command;

        /// <summary>
        /// Executes the SQL statement
        /// </summary>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync()
        {
            try
            {
                int result = 0;
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync().ConfigureAwait(false); ;
                    }
                    command.CommandTimeout = 80;
                    result = await command.ExecuteNonQueryAsync().ConfigureAwait(false); ;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<int> ExecuteNonQueryWithQueryAsync(string Query)
        {
            try
            {
                int result = 0;
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync().ConfigureAwait(false); ;
                    }
                    command = new SqlCommand(Query, con);
                    command.CommandTimeout = 80;
                    command.CommandType = CommandType.Text;
                    result = await command.ExecuteNonQueryAsync().ConfigureAwait(false); ;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<int> ExecuteNonQueryParamsAsync(string spname,Hashtable ht)
        {
            try
            {
                int result = 0;
                DataTable _table = new DataTable();
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync().ConfigureAwait(false); ;
                    }
                    command.CommandTimeout = 80;
                    command = new SqlCommand(spname, con);
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry de in ht)
                    {
                        command.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }

                    result = await command.ExecuteNonQueryAsync().ConfigureAwait(false); ;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Execute the SQL statement and returns a unique value (First Row , First column value)
        /// </summary>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync()
        {
            try
            {

                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync().ConfigureAwait(false); ;
                    }
                    command.CommandTimeout = 80;
                    return await command.ExecuteScalarAsync().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Executes the SQL statement fast.
        /// </summary>
        /// <returns></returns>
        public async Task<DataTable> ExecuteReaderAsync()
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 80;
                    DataSet ds = new DataSet();
                    Task<SqlDataReader> dr = command.ExecuteReaderAsync();
                    await Task.WhenAll(dr);
                    var reader = dr.Result;
                    ds.Load(reader, LoadOption.OverwriteChanges, "Result");
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> ExecuteReaderWithSPAsync(string SPName)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                    command = new SqlCommand(SPName, con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 80;
                    DataSet ds = new DataSet();
                    Task<SqlDataReader> dr = command.ExecuteReaderAsync();
                    await Task.WhenAll(dr);
                    var reader = dr.Result;
                    ds.Load(reader, LoadOption.OverwriteChanges, "Result");
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> ExecuteReaderWithQueryAsync(string query)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                    command = new SqlCommand(query, con);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 80;
                    DataSet ds = new DataSet();
                    Task<SqlDataReader> dr = command.ExecuteReaderAsync();
                    await Task.WhenAll(dr);
                    var reader = dr.Result;
                    ds.Load(reader, LoadOption.OverwriteChanges, "Result");
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> ExecuteReaderQueryAsync(string query)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                    command = new SqlCommand(query, con);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 200;
                    DataSet ds = new DataSet();
                    Task<SqlDataReader> dr = command.ExecuteReaderAsync();
                    await Task.WhenAll(dr);
                    var reader = dr.Result;
                    ds.Load(reader, LoadOption.OverwriteChanges, "Result");
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> ExecuteReaderCmdTextAsync()
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                    command.CommandTimeout = 80;
                    command.CommandType = CommandType.Text;
                    DataSet ds = new DataSet();
                    Task<SqlDataReader> dr = command.ExecuteReaderAsync();
                    await Task.WhenAll(dr);
                    var reader = dr.Result;
                    ds.Load(reader, LoadOption.OverwriteChanges, "Result");
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fills the dataset
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ExecuteAdaptorAsync(DataSet ds)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 80;

                    SqlDataAdapter adaptor = new SqlDataAdapter(command);
                    adaptor.Fill(ds);
                    return ds;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<DataTable> ExecuteAdaptorAsyncWithQueryParams(string query)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    DataTable _table = new DataTable();
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }

                    command = new SqlCommand(query, con);
                    command.CommandTimeout = 80;
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter adaptor = new SqlDataAdapter(command);
                    adaptor.Fill(_table);
                    return _table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> ExecuteAdaptorAsyncWithParams(string spName, Hashtable HTParms)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    DataTable _table = new DataTable();
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                   
                    command = new SqlCommand(spName, con);
                    command.CommandTimeout = 120;
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry de in HTParms)
                    {
                        command.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }
                    SqlDataAdapter adaptor = new SqlDataAdapter(command);
                    adaptor.Fill(_table);
                    return _table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> ExecuteAdaptorAsyncWithParamsNew(string spName, Hashtable HTParms)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    DataTable _table = new DataTable();
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }

                    command = new SqlCommand(spName, con);
                    command.CommandTimeout = 200;
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry de in HTParms)
                    {
                        command.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }
                    SqlDataAdapter adaptor = new SqlDataAdapter(command);
                    adaptor.Fill(_table);
                    return _table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> ExecuteAdaptorWithParams(string spName, Hashtable HTParms)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    DataTable _table = new DataTable();
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }

                    command = new SqlCommand(spName, con);
                    command.CommandTimeout = 160;
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry de in HTParms)
                    {
                        command.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }
                    SqlDataAdapter adaptor = new SqlDataAdapter(command);
                    adaptor.Fill(_table);
                    return _table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> CentralExecuteReaderWithSPAsync(string SPName)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }
                    command = new SqlCommand(SPName, con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 80;
                    DataSet ds = new DataSet();
                    Task<SqlDataReader> dr = command.ExecuteReaderAsync();
                    await Task.WhenAll(dr);
                    var reader = dr.Result;
                    ds.Load(reader, LoadOption.OverwriteChanges, "Result");
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> PocketFameExecuteAdaptorAsyncWithParams(string spName, Hashtable HTParms)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(PocketFameConnectionString))
                {
                    DataTable _table = new DataTable();
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }

                    command = new SqlCommand(spName, con);
                    command.CommandTimeout = 120;
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry de in HTParms)
                    {
                        command.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }
                    SqlDataAdapter adaptor = new SqlDataAdapter(command);
                    adaptor.Fill(_table);
                    return _table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> PocketFameExecuteNonQueryWithQueryAsync(string Query)
        {
            try
            {
                int result = 0;
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(PocketFameConnectionString))
                {
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync().ConfigureAwait(false); ;
                    }
                    command = new SqlCommand(Query, con);
                    command.CommandTimeout = 80;
                    command.CommandType = CommandType.Text;
                    result = await command.ExecuteNonQueryAsync().ConfigureAwait(false); ;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<DataTable> PocketFameExecuteAdaptorAsyncWithQueryParams(string query)
        {
            try
            {
                using (SqlConnection con = new System.Data.SqlClient.SqlConnection(PocketFameConnectionString))
                {
                    DataTable _table = new DataTable();
                    command.Connection = con;
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();//.ConfigureAwait(false); ;
                    }

                    command = new SqlCommand(query, con);
                    command.CommandTimeout = 80;
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter adaptor = new SqlDataAdapter(command);
                    adaptor.Fill(_table);
                    return _table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}