using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Kuduma.Portal.DAL
{
    public class DatabaseMethods
    {
        static SqlConnection con;
        static SqlCommand cmd;

        public DatabaseMethods()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string conection = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;

        public static int ExecuteNonquery(string connectionString, CommandType commandType, string commandText, SqlParameter[] parameters)
        {
            try
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand(commandText, con);
                cmd.CommandType = commandType;
                foreach (SqlParameter p in parameters)
                {
                    if (p.Value == null)
                    {
                    }
                    cmd.Parameters.Add(p);
                }

                con.Open();
                return cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, SqlParameter[] parameters)
        {
            try
            {
                // Create the DataAdapter & DataSet

                con = new SqlConnection(connectionString);

                cmd = new SqlCommand(commandText, con);
                cmd.CommandType = commandType;
                foreach (SqlParameter p in parameters)
                {
                    if (p.Value == null)
                    {
                    }
                    cmd.Parameters.Add(p);
                }
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    // Fill the DataSet using default values for DataTable names, etc
                    da.Fill(ds);

                    // Detach the SqlParameters from the command object, so they can be used again
                    cmd.Parameters.Clear();
                    return ds;

                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}