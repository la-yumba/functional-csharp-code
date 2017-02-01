using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System;

namespace Examples.Chapter1.DbLogger
{
   public class DbLogger_V1
   {
      string connString;

      public void Log(LogMessage msg)
      {
         using (var conn = new SqlConnection(connString))
         {
            conn.Open();
            int affectedRows = conn.Execute("sp_create_log"
               , msg, commandType: CommandType.StoredProcedure);
         }
      }

      public void DeleteOldLogs()
      {
         using (var conn = new SqlConnection(connString))
         {
            conn.Open();
            //conn.Execute($@"DELETE [Logs] WHERE [Timestamp] < 
            //   '{DateTime.Now.AddDays(-7).ToString("s")}'");
            conn.Execute("DELETE [Logs] WHERE [Timestamp] < @upTo"
               , param: new { upTo = 7.Days().Ago() });
         }
      }

      public IEnumerable<LogMessage> GetLogs(DateTime since)
      {
         var sqlGetLogs = "SELECT * FROM [Logs] WHERE [Timestamp] > @since";
         using (var conn = new SqlConnection(connString))
         {
            return conn.Query<LogMessage>(sqlGetLogs, new { since = since });
         }
      }
   }
}