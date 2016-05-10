using Boc.Commands;
using Boc.Services;

namespace Boc.Services.Validation.WithDI
{
   public class DateNotPastValidator_Testable : IValidator<Transfer>
   {
      private readonly IDateTimeService clock;

      public DateNotPastValidator_Testable(IDateTimeService clock)
      {
         this.clock = clock;
      }
      
      public bool IsValid(Transfer request)
         => clock.UtcNow.Date <= request.Date.Date;
   }
}