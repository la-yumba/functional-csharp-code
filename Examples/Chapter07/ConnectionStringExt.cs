using System;
using System.Collections.Generic;
using Dapper;
using Examples.Chapter1.DbLogger;

namespace Examples
{
   using static ConnectionHelper;
   
   public static class ConnectionStringExt
   {
      public static Func<SqlTemplate, object, IEnumerable<T>>
      Query<T>(this ConnectionString connString)
         => (sql, param)
         => Connect(connString, conn => conn.Query<T>(sql, param));

      public static void Execute(this ConnectionString connString
         , SqlTemplate sql, object param)
         => Connect(connString, conn => conn.Execute(sql, param));
   }
}
