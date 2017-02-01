using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Examples.Chapter1.DbLogger
{
   using static ConnectionHelper;

   public class DbLogger_V2
   {
      string connString;
      string sqlDeleteLogs = "DELETE [Logs] WHERE [Timestamp] < @upTo";

      public void Log(LogMessage message)
         => Connect(connString, c => c.Execute("sp_create_log"
            , message, commandType: CommandType.StoredProcedure));

      public void DeleteOldLogs() => Connect(connString
         , c => c.Execute(sqlDeleteLogs, new {upTo = 7.Days().Ago()}));

      public IEnumerable<LogMessage> GetLogs(DateTime since)
         => Connect(connString, c => c.Query<LogMessage>(@"SELECT * 
            FROM [Logs] WHERE [Timestamp] > @since", new { since = since }));
   }
}
