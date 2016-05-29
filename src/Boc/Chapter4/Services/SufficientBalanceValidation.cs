using LaYumba.Functional;

namespace Boc.Chapter3.V2.Services.Validation
{
   using Data;
   using Commands;
   using Domain;
   using Boc.Services;
   using static F;

   using Boc.Domain.Validator;

   public class SufficientBalanceValidator : IValidator<Transfer>
   {
      private readonly IRepository<Account> accounts;

      public SufficientBalanceValidator(IRepository<Account> accounts)
      {
         this.accounts = accounts;
      }

      public bool IsValid(Transfer request)
         => Some(request)
            .Bind(r => accounts.Get(r.DebitedAccountId))
            .Map(account => account.CanCover(request.Amount))
            .GetOrElse(false);
   }
}