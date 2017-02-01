using System;
using LaYumba.Functional;
using Boc.Commands;
using Unit = System.ValueTuple;

namespace Boc.Chapter7.OOP
{
   using static F;
   using Examples;

   public class BookTransferRepository : IRepository<BookTransfer>
   {
      ConnectionString conn;

      public BookTransferRepository(ConnectionString conn)
      {
         this.conn = conn;
      }

      public Option<BookTransfer> Lookup(Guid id)
      { throw new NotImplementedException("Illustrates violating interface segregation"); }

      public Exceptional<Unit> Save(BookTransfer transfer)
      {
         try { conn.Execute("INSERT ...", transfer); }
         catch (Exception ex) { return ex; }
         return Unit();
      }
   }  
}
