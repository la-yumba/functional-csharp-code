using System;
using Boc.Commands;
using NSubstitute;
using NUnit.Framework;

namespace Boc.Services.Validation.WithDI.Tests
{
   [TestFixture] public class DateNotPastValidatorTest
   {
      IDateTimeService clock = Substitute.For<IDateTimeService>();

      [TestCase(+1, ExpectedResult = true)]
      [TestCase( 0, ExpectedResult = true)]
      [TestCase(-1, ExpectedResult = false)]
      public bool WhenTransferDateIsPast_ThenValidatorFails(int offset)
      {
         var todaysDate = new DateTime(2016, 12, 12);
         clock.UtcNow.Returns(todaysDate);
         var sut = new DateNotPastValidator_Testable(clock);

         var transferDate = todaysDate.AddDays(offset);
         var transfer = new Transfer { Date = transferDate };

         return sut.IsValid(transfer);
      }
   }
}