using System;
using Boc.Chapter1.V1.Data;
using Boc.Chapter1.V1.Services.Validation;
using Boc.Commands;
using Boc.Domain;
using NSubstitute;
using NUnit.Framework;

namespace Boc.Services.Tests.WithTestCases
{
    [TestFixture] public class SufficientBalanceValidatorTest
    {
        Guid accountId = Guid.NewGuid();
        IRepository<Account> accounts = Substitute.For<IRepository<Account>>();

        [TestCase(800, 200, ExpectedResult = true)]
        [TestCase(100, 200, ExpectedResult = false)]
        public bool WhenBalanceIsInsufficient_ThenValidatorFails
           (decimal balance, decimal transferAmount)
        {
            accounts.Get(accountId).Returns(new Account().WithBalance(balance));
            var sut = new SufficientBalanceValidator(accounts);

            return sut.IsValid(TransferOf(transferAmount));
        }

        Transfer TransferOf(decimal amount) => new Transfer
        {
            Amount = new Money(amount, "EUR"),
            DebitedAccountId = accountId,
        };
    }
}