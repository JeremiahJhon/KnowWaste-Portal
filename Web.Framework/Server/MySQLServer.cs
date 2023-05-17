using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Framework.Server
{
    public class MySQLServer
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
            ConnectionString = String.Format("Driver={0}; Server={1}; User={2}; password={3}; Database={4}; Option=3;", Driver, Name, UserName, Password, Database);
        }

        public DataTable ExecuteQuery()
        {
            //Reference: http://msdn.microsoft.com/en-us/library/ms998569.aspx
            //When Using DataReaders, Specify CommandBehavior.CloseConnection
            DataTable dReturnValue = new DataTable();

            try
            {
                //Open and Close the Connection in the Method. See Reference.
                OdbcDataAdapter MySQLAdapter = new OdbcDataAdapter(Query, ConnectionString);

                //Explicitly Close Connections. See Reference.
                MySQLAdapter.SelectCommand.Connection.Close();

                //Do Not Explicitly Open a Connection if You Use Fill or Update for a Single Operation
                MySQLAdapter.Fill(dReturnValue);

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
            OdbcConnection MySQLConnection = new OdbcConnection(ConnectionString);
            try
            {
                //Open and Close the Connection in the Method. See Reference.
                MySQLConnection.Open();
                OdbcCommand MySQLCommand = new OdbcCommand(Query, MySQLConnection);
                MySQLCommand.ExecuteNonQuery();

                Error = "";
                return true;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return false;
            }
            finally
            {
                //Open and Close the Connection in the Method. See Reference.
                MySQLConnection.Close();
            }
        }
    }
}