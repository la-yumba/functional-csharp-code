using Boc.Commands;
using Boc.Services;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Boc.Chapter3.Services
{
   public class TransferHandler_2
   {
      IValidator<Transfer> validator;

      public void Handle(Transfer request)
         => Some(request)
            .Where(validator.IsValid)
            .ForEach(InternalBook);

      public void HandleLINQ(Transfer request) 
         => (from r in Some(request)
             where validator.IsValid(r)
             select r)
            .ForEach(InternalBook);

      private void InternalBook(Transfer request)
      {
         // actually book the transfer...
      }
   }
}
