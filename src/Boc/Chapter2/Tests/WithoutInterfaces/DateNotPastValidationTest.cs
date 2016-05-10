using System;
using Boc.Commands;
using Boc.Services.Validation.WithoutInterfaces;
using NUnit.Framework;

namespace Boc.Services.WithoutInterfaces.Tests
{
   [TestFixture] public class DateNotPastValidatorTest
   {
      [TestCase(+1, ExpectedResult = true)]
      [TestCase( 0, ExpectedResult = true)]
      [TestCase(-1, ExpectedResult = false)]
      public bool WhenTransferDateIsPast_ThenValidatorFails(int offset)
      {
         var now = new DateTime(2016, 12, 12, 11, 11, 00);
         var transferDate = now.AddDays(offset);
         var transfer = new Transfer { Date = transferDate };

         return DateNotPastValidator.IsValid(transfer, () => now);
      }
   }
}