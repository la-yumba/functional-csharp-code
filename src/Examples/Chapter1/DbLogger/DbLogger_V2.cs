using Dapper;
using System;
using System.Data;

namespace Examples.Chapter1.DbLogger
{
   using static ConnectionExt;

   class DbLogger_V2
   {
      string connString;

      public void Log(LogMessage message) 
         => Connect(connString, conn 
            => conn.Execute("sp_create_log", message
               , commandType: CommandType.StoredProcedure));

      public void DeleteOldLogs() 
         => Connect(connString, conn 
            => conn.Execute(DeleteOldLogsSql));

      string DeleteOldLogsSql => $@"DELETE [Logs] 
         WHERE [Timestamp] < {DateTime.Now.AddDays(-7)}";
   }
}