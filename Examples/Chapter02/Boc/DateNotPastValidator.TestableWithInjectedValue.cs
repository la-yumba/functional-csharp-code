using System;
using Boc.Commands;

namespace Boc.Services.Validation.InjectValue
{
   public class DateNotPastValidator : IValidator<MakeTransfer>
   {
      DateTime Today { get; }

      public DateNotPastValidator(DateTime today)
      {
         this.Today = today;
      }

      public bool IsValid(MakeTransfer cmd)
         => Today <= cmd.Date.Date;
   }
}