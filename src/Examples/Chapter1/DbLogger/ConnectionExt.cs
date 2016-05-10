using System;
using System.Data;
using System.Data.SqlClient;
using static LaYumba.Functional.F;

namespace Examples.Chapter1.DbLogger
{
   public static class ConnectionExt
   {
      public static R Connect<R>(string connString
         , Func<IDbConnection, R> func)
      {
         using (var conn = new SqlConnection(connString))
         {
            conn.Open();
            return func(conn);
         }
      }
   }

   public static class ConnectionExt_V2
   {
      public static R Connect<R>(string connString, Func<IDbConnection, R> func)
         => Using(new SqlConnection(connString)
            , conn => { conn.Open(); return func(conn); });
   }
}