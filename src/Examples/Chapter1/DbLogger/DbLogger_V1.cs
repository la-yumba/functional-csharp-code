using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Examples.Chapter1.DbLogger
{
   class DbLogger_V1
   {
      string connString;

      public void Log(LogMessage message)
      {
         using (var conn = new SqlConnection(connString))
         {
            conn.Open();
            conn.Execute("sp_create_log", message
               , commandType: CommandType.StoredProcedure);
         }
      }

      public void DeleteOldLogs()
      {
         using (var conn = new SqlConnection(connString))
         {
            conn.Open();
            conn.Execute("DELETE [Logs] WHERE [Timestamp] < "
                         + DateTime.Now.AddDays(-7));
         }
      }
   }
}