using System;
using Boc.Commands;
using Boc.Domain;
using NUnit.Framework;

namespace Boc.Services.Validation.WithoutInterfaces
{
   using static SufficientBalanceValidator;

   [TestFixture] public class SufficientBalanceValidatorTest
   {
      Guid accountId = Guid.NewGuid();

      [TestCase(800, 200, ExpectedResult = true)]
      [TestCase(100, 200, ExpectedResult = false)]
      public bool WhenBalanceIsInsufficient_ThenValidatorFails
         (decimal balance, decimal transferAmount)
      {
         Func<Guid, Account> getAccount = id => (id == accountId)
            ? new Account().WithBalance(balance)
            : null;
         return IsValid(TransferOf(transferAmount), getAccount);
      }

      Transfer TransferOf(decimal amount) => new Transfer
      {
         Amount = new Money(amount, "EUR"),
         DebitedAccountId = accountId,
      };
   }
}