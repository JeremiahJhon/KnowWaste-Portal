using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Web.Framework.Server
{
    public class MSSQLServer
    {
        private static string ConnectionString;

        public static string Driver { get; set; }

        public static string Name { get; set; }

        public static string UserName { get; set; }

        public static string Password { get; set; }

        public static string Database { get; set; }

        public string Error { get; set; }

        public string Query { get; set; }

        public static void Initialize()
        {
            ConnectionString = String.Format("Server={0};Database={1};User Id={2};Password={3};", Name, Database, UserName, Password );
        }

        public DataTable ExecuteQuery()
        {
            DataTable dReturnValue = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(
                        Query, connection);
                    adapter.Fill(dReturnValue);
                }
                Error = "";
            }
            catch (Exception e)
            {
                Error = e.Message;

                using (StreamWriter sw = File.AppendText(@"D:\Projects\KnowWaste\log.txt"))
                {
                    sw.WriteLine(Error);
                }
            }

            return dReturnValue;
        }

        protected bool ExecuteNonQuery()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(Query, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                Error = "";
                return true;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return false;
            }
        }
    }
}