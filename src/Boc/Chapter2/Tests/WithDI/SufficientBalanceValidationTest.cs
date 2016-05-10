using System;
using Boc.Chapter1.V1.Data;
using Boc.Chapter1.V1.Services.Validation;
using Boc.Commands;
using Boc.Domain;
using NSubstitute;
using NUnit.Framework;

namespace Boc.Services.Validation.Tests.WithDI
{
   [TestFixture]
   public class SufficientBalanceValidatorTest
   {
      private Guid accountId = Guid.NewGuid();
      private IRepository<Account> accounts = Substitute.For<IRepository<Account>>();

      [Test]
      public void WhenBalanceIsSufficient_ThenValidatorSucceeds()
      {
         // arrange
         accounts.Get(accountId).Returns(new Account().WithBalance(1000));

         var request = new Transfer
         {
            Amount = new Money(200, "EUR"),
            DebitedAccountId = accountId,
         };

         var sut = new SufficientBalanceValidator(accounts);

         // act
         var result = sut.IsValid(request);

         // assert
         Assert.IsTrue(result);
      }

      [Ignore("left as an exercise to the reader"), Test]
      public void WhenBalanceIsInsufficient_ThenValidatorFails()
      {
         // same as above, but with a low balance on the account
      }
   }
}