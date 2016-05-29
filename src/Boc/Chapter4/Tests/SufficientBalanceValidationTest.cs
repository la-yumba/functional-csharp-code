using LaYumba.Functional;

using NSubstitute;
using NUnit.Framework;
using System;

namespace Boc.Chapter3.V2.Tests
{
   using Data;
   using Commands;
   using Domain;
   using Services.Validation;

   using static F;

   [TestFixture]
   public class SufficientBalanceValidatorTest
   {
      private Guid accountId = Guid.NewGuid();
      private IRepository<Account> accounts = Substitute.For<IRepository<Account>>();

      [Test]
      public void WhenRequestIsNull_ThenValidatorFails()
      {
         accounts.Get(accountId).Returns(new Account().WithBalance(1000));
         var sut = new SufficientBalanceValidator(accounts);
         var result = sut.IsValid(null);

         Assert.IsFalse(result);
      }

      [Test] public void WhenNoAccountIsFound_ThenValidatorFails()
      {
         accounts.Get(accountId).Returns(None);
         var sut = new SufficientBalanceValidator(accounts);

         var result = sut.IsValid(TransferOf(200));

         Assert.IsFalse(result);
      }

      private Transfer TransferOf(decimal amount)
          => new Transfer
          {
             Amount = new Money(amount, "EUR"),
             DebitedAccountId = accountId,
          };
   }
}
