using Boc.Commands;
using NUnit.Framework;
using System;

namespace Boc.Services.Validation.WithDI
{
   // interface
   public interface IDateTimeService
   {
      DateTime UtcNow { get; }
   }

   // "real" implementation
   public class DefaultDateTimeService : IDateTimeService
   {
      public DateTime UtcNow => DateTime.UtcNow;
   }

   // testable class depends on interface
   public class DateNotPastValidator_Testable : IValidator<MakeTransfer>
   {
      private readonly IDateTimeService clock;

      public DateNotPastValidator_Testable(IDateTimeService clock)
      {
         this.clock = clock;
      }
      
      public bool IsValid(MakeTransfer request)
         => clock.UtcNow.Date <= request.Date.Date;
   }

   // can be tested using fakes
   public class DateNotPastValidatorTest
   {
      static DateTime presentDate = new DateTime(2016, 12, 12);

      // "fake" implementation
      private class FakeDateTimeService : IDateTimeService
      {
         public DateTime UtcNow => presentDate;
      }

      // example-based unit test
      [Test]
      public void WhenTransferDateIsPast_ThenValidatorFails()
      {
         var sut = new DateNotPastValidator_Testable(new FakeDateTimeService());
         var transfer = new MakeTransfer { Date = presentDate.AddDays(-1) };
         Assert.AreEqual(false, sut.IsValid(transfer));
      }

      // parameterized unit test
      [TestCase(+1, ExpectedResult = true)]
      [TestCase(0, ExpectedResult = true)]
      [TestCase(-1, ExpectedResult = false)]
      public bool WhenTransferDateIsPast_ThenValidationFails(int offset)
      {
         var sut = new DateNotPastValidator_Testable(new FakeDateTimeService());
         var transferDate = presentDate.AddDays(offset);
         var transfer = new MakeTransfer { Date = transferDate };

         return sut.IsValid(transfer);
      }
   }
}