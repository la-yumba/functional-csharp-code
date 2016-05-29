using Boc.Commands;
using Boc.Services;

namespace Boc.Chapter2.Services
{
   public class TransferHandler_Skeleton
   {
      private readonly IValidator<Transfer> validator;

      public void Handle(Transfer request)
      {
         if (request != null)
            if (Validate(request))
               InternalBook(request);
      }

      private bool Validate(Transfer request)
         => validator.IsValid(request);

      private void InternalBook(Transfer request)
      {
         // actually book the transfer...
      }
   }
}