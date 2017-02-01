using System;
using LaYumba.Functional;
using Boc.Commands;
using Unit = System.ValueTuple;

namespace Boc.Chapter7
{
   using static F;
   using Examples;
   
   namespace Delegate
   {
      public static class Sql
      {
         public static class Queries
         {
            public static readonly SqlTemplate InsertTransferOn = "INSERT ...";
         }

         public static Func< ConnectionString
                           , SqlTemplate
                           , object
                           , Exceptional<Unit> >
         TryExecute => (conn, sql, t) =>
         {
            try { conn.Execute(sql, t); }
            catch (Exception ex) { return ex; }
            return Unit();
         };
      }
   }
}
