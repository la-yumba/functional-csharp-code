using Boc.Commands;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using Unit = System.ValueTuple;

namespace Boc.EitherImpl.Services.ValidationMethods
{
   using Domain;
   using static F;

   class BookTransferController_Skeleton : Controller
   {
      DateTime now;
      Regex bicRegex = new Regex("[A-Z]{11}");

      Either<Error, Unit> Handle(BookTransfer request)
         => Right(request)
            .Bind(ValidateBic)
            .Bind(ValidateDate)
            .Bind(Save);

      Either<Error, BookTransfer> ValidateBic(BookTransfer request)
      {
         if (!bicRegex.IsMatch(request.Bic))
            return Errors.InvalidBic;
         else return request;
      }

      Either<Error, BookTransfer> ValidateDate(BookTransfer request)
      {
         if (request.Date.Date <= now.Date)
            return Errors.TransferDateIsPast;
         else return request;
      }

      Either<Error, Unit> Save(BookTransfer request)
      { throw new NotImplementedException(); }
   }
}
