using LaYumba.Functional;
using static LaYumba.Functional.F;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace Boc.Chapter7
{   
   using static ValidationStrategies;
   
   public static partial class ValidationStrategies
   {
      // runs all validators, accumulating all validation errors
      public static Validator<T> HarvestErrorsTr<T>
         (IEnumerable<Validator<T>> validators)
         => t
         => validators
            .Traverse(validate => validate(t))
            .Map(_ => t);
   }
   
   public static partial class ValidationStrategiesTest
   {
      public class HarvestErrors_WithTraverse_Test
      {
         [Test]
         public void WhenAllValidatorsSucceed_ThenSucceed() => Assert.AreEqual(
            actual: HarvestErrorsTr(List(Success, Success))(1),
            expected: Valid(1)
         );

         [Test]
         public void WhenNoValidators_ThenSucceed() => Assert.AreEqual(
            actual: HarvestErrorsTr(List<Validator<int>>())(1),
            expected: Valid(1)
         );

         [Test]
         public void WhenOneValidatorFails_ThenFail() =>
            HarvestErrorsTr(List(Success, Failure))(1).Match(
               Valid: (_) => Assert.Fail(),
               Invalid: (errs) => Assert.AreEqual(1, errs.Count()));

         [Test]
         public void WhenSeveralValidatorsFail_ThenFail() =>
            HarvestErrorsTr(List(Success, Failure, Failure, Success))(1).Match(
               Valid: (_) => Assert.Fail(),
               Invalid: (errs) => Assert.AreEqual(2, errs.Count())); // all errors are returned
      }
   }
}
