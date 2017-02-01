using Boc.Commands;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;
using System;
using Unit = System.ValueTuple;

namespace Boc.Api
{
   public class Chapter6_BookTransferController : Controller
   {
      [HttpPost, Route("api/Chapters6/transfers/future/restful")]
      public IActionResult BookTransfer_v1([FromBody] BookTransfer request)
         => Handle(request).Match<IActionResult>(
            Right: _ => Ok(),
            Left: BadRequest);

      [HttpPost, Route("api/Chapters6/transfers/future/resultDto")]
      public ResultDto<Unit> BookTransfer_v2([FromBody] BookTransfer request)
         => Handle(request).ToResult();

      Either<Error, Unit> Handle(BookTransfer request) { throw new NotImplementedException();  }
   }
}
