using System;
using Boc.Chapter1.V1.Data;
using Boc.Chapter1.V1.Services.Validation;
using Boc.Commands;
using Boc.Domain;
using NSubstitute;
using NUnit.Framework;

namespace Boc.Services.Validation.Tests.Failing
{
    [TestFixture]
    public class SufficientBalanceValidatorTest
    {
        Guid accountId = Guid.NewGuid();
        IRepository<Account> accounts = Substitute.For<IRepository<Account>>();

        [Ignore("demonstrates NullReferenceExeption")]
        [TestCase(ExpectedResult = false)]
        public bool WhenNoAccountIsFound_ThenValidatorFails()
        {
            accounts.Get(accountId).Returns((Account)null);
            var sut = new SufficientBalanceValidator(accounts);

            return sut.IsValid(TransferOf(200));
        }

        Transfer TransferOf(decimal amount) => new Transfer
        {
            Amount = new Money(amount, "EUR"),
            DebitedAccountId = accountId,
        };
    }
}