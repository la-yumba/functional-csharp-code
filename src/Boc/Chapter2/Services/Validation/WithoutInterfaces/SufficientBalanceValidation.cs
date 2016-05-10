using System;
using Boc.Chapter1.V1.Data;
using Boc.Commands;
using Boc.Domain;
using Boc.Domain.Validator;

namespace Boc.Services.Validation.WithoutInterfaces
{
   public class SufficientBalanceValidator : IValidator<Transfer>
   {
      private readonly Repository<Account> accounts;

      public bool IsValid(Transfer request)
         => IsValid(request, id => accounts.Get(id));

      internal static bool IsValid(Transfer request
         , Func<Guid, Account> getAccount)
      {
         var account = getAccount(request.DebitedAccountId);
         return account.CanCover(request.Amount);
      }
   }
}