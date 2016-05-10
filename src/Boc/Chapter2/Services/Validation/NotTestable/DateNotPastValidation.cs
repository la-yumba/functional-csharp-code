using System;
using Boc.Commands;

namespace Boc.Services.Validation.NotTestable
{
   public class DateNotPastValidator : IValidator<Transfer>
   {
      public bool IsValid(Transfer request)
         => DateTime.UtcNow.Date <= request.Date.Date;
   }
}