using Boc.Commands;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;
using System;
using Unit = System.ValueTuple;

namespace Boc.EitherImpl.Services.Skeleton
{
   class BookTransferController_Skeleton : Controller
   {
      [HttpPost, Route("transfers/book/skeleton")]
      public void BookTransfer([FromBody] BookTransfer request)
         => Handle(request);

      Either<Error, Unit> Handle(BookTransfer request)
         => Validate(request)
            .Bind(Save);

      Either<Error, BookTransfer> Validate(BookTransfer request)
      { throw new NotImplementedException(); }

      Either<Error, Unit> Save(BookTransfer request)
      { throw new NotImplementedException(); }
   }
}