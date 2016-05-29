using LaYumba.Functional;

namespace Boc.Services.Validation.WithOption
{
   using Data;
   using Commands;
   using Domain;

   using Boc.Services;
   using Boc.Domain.Validator;

   public class SufficientBalanceValidator : IValidator<Transfer>
   {
      private readonly IRepository<Account> accounts;

      public SufficientBalanceValidator(IRepository<Account> accounts)
      {
         this.accounts = accounts;
      }

      public bool IsValid(Transfer request)
         => accounts.Get(request.DebitedAccountId)
            .Map(account => account.CanCover(request.Amount))
            .GetOrElse(false);

      public bool IsValid_WithMatch(Transfer request)
         => accounts.Get(request.DebitedAccountId).Match(
            Some: account => account.CanCover(request.Amount),
            None: () => false);

      public bool IsValid_WithLINQ(Transfer request)
         => (from account in accounts.Get(request.DebitedAccountId)
             select account.CanCover(request.Amount))
            .GetOrElse(false);
   }
}