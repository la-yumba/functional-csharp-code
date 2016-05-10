using System;
using Boc.Commands;
using Boc.Domain;
using Boc.Domain.Validator;
using Boc.Services;

namespace Boc.Chapter1.V1.Services.Validation
{
   using Data;

   public class SufficientBalanceValidator : IValidator<Transfer>
   {
      private readonly IRepository<Account> accounts;

      public SufficientBalanceValidator(IRepository<Account> accounts)
      {
         this.accounts = accounts;
      }

      public bool IsValid(Transfer request)
      {
         var account = accounts.Get(request.DebitedAccountId);
         return request.Amount <= account.Balance + account.Overdraft;
      }
   }

   public class SufficientBalanceValidator_MoreFunctional : IValidator<Transfer>
   {
      private readonly IRepository<Account> accounts;

      public bool IsValid(Transfer request)
      {
         var account = accounts.Get(request.DebitedAccountId);
         return account.CanCover(request.Amount);
      }
   }
}