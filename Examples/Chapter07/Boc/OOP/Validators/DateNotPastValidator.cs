using Boc.Commands;
using LaYumba.Functional;
using Boc.Domain;
using Boc.Services.Validation.WithDI;

namespace Boc.Chapter7.OOP
{
   public class DateNotPastValidator : IValidator<MakeTransfer>
   {
      private readonly IDateTimeService clock;

      public DateNotPastValidator(IDateTimeService clock)
      {
         this.clock = clock;
      }

      public Validation<MakeTransfer> Validate(MakeTransfer request)
      {
         if (request.Date.Date <= clock.UtcNow.Date)
            return Errors.TransferDateIsPast;
         return request;
      }
   }
}