using System;
using Boc.Commands;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Boc.Services.Validation
{
   public sealed class BicFormatValidator : IValidator<MakeTransfer>
   {
      static readonly Regex regex = new Regex("^[A-Z]{6}[A-Z1-9]{5}$");

      public bool IsValid(MakeTransfer t)
         => regex.IsMatch(t.Bic);
   }
   
   public sealed class BicExistsValidator_Skeleton : IValidator<MakeTransfer>
   {
      readonly IEnumerable<string> validCodes;

      public bool IsValid(MakeTransfer cmd)
         => validCodes.Contains(cmd.Bic);
   }
   
   public sealed class BicExistsValidator : IValidator<MakeTransfer>
   {
      readonly Func<IEnumerable<string>> getValidCodes;

      public BicExistsValidator(Func<IEnumerable<string>> getValidCodes)
      {
         this.getValidCodes = getValidCodes;
      }

      public bool IsValid(MakeTransfer cmd)
         => getValidCodes().Contains(cmd.Bic);
   }

   public class BicExistsValidatorTest
   {
      static string[] validCodes = { "ABCDEFGJ123" };

      [TestCase("ABCDEFGJ123", ExpectedResult = true)]
      [TestCase("XXXXXXXXXXX", ExpectedResult = false)]
      public bool WhenBicNotFound_ThenValidationFails(string bic)
         => new BicExistsValidator(() => validCodes)
            .IsValid(new MakeTransfer { Bic = bic });
   }
}