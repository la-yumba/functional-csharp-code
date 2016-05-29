using Boc.Services;


using Boc.Commands;
using LaYumba.Functional;

namespace Boc.Chapter2.Services
{
   using static F;
   public class TransferHandler_1
   {
      IValidator<Transfer> validator;

      public void Handle(Transfer request) 
         => Validate(request).Map(Book);

      Option<Transfer> Validate(Transfer request)
         => request != null && validator.IsValid(request)
            ? Some(request) : None;

      Unit Book(Transfer request)
      {
         // actually book the transfer...
         return Unit();
      }
   }
}