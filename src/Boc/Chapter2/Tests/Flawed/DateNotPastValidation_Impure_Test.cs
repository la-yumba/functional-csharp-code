using System;
using Boc.Commands;
using Boc.Services.Validation.NotTestable;
using NUnit.Framework;

namespace Boc.Services.Validation.Tests.Flawed
{
   [TestFixture]
   public class DateNotPastValidator_Impure_Test
   {
      [Test]
      [Ignore("Demonstrates a test that is not repeatable")]
      public void WhenTransferDateIsFuture_ThenValidatorPasses()
      {
         var sut = new DateNotPastValidator();
         var transfer = new Transfer { Date = new DateTime(2016, 12, 12) };

         var actual = sut.IsValid(transfer);
         Assert.AreEqual(true, actual);
      }
   }
}