using LaYumba.Functional;
using NSubstitute;
using NUnit.Framework;
using System;
using Boc.Data;
using Boc.Commands;
using Boc.Domain;
using Boc.Services.Validation.WithOption;

namespace Boc.Services.Validation.Tests.WithOption
{
   using static F;

   [TestFixture]
   public class SufficientBalanceValidatorTest
   {
      private Guid accountId = Guid.NewGuid();
      private IRepository<Account> accounts = Substitute.For<IRepository<Account>>();
      
      [Test] public void WhenNoAccountIsFound_ThenValidatorFails()
      {
         accounts.Get(accountId).Returns(None);
         var sut = new SufficientBalanceValidator(accounts);

         var result = sut.IsValid(TransferOf(200));

         Assert.IsFalse(result);
      }

      [TestCase(1000, 200, ExpectedResult = true)]
      [TestCase(100, 200, ExpectedResult = false)]
      public bool WhenBalanceIsInsufficient_ThenValidatorFails
          (decimal balance, decimal transferAmount)
      {
         accounts.Get(accountId).Returns(new Account().WithBalance(balance)); // implicitly converted to Option<Account>
         var sut = new SufficientBalanceValidator(accounts);

         return sut.IsValid(TransferOf(transferAmount));
      }

      private Transfer TransferOf(decimal amount)
          => new Transfer
          {
             Amount = new Money(amount, "EUR"),
             DebitedAccountId = accountId,
          };
   }
}
