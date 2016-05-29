using System;
using static LaYumba.Functional.F;
using NUnit.Framework;

namespace LaYumba.Functional.Tests
{
   [TestFixture] class OptionApply
   {
      Func<int, int, int> add = (a, b) => a + b;
      Func<int, int, int> Multiply = (i, j) => i * j;

      [Test]
      public void MapAndApplySomeArg_ReturnsSome()
      {
         var opt = Some(3)
             .Map(Multiply)
             .Apply(Some(4));

         Assert.AreEqual(Some(12), opt);
      }

      [Test]
      public void MapAndApplyNoneArg_ReturnsNone()
      {
         var opt = Some(3)
             .Map(Multiply)
             .Apply(None);

         var opt2 = ((Option<int>)None)
             .Map(Multiply)
             .Apply(Some(4));

         Assert.AreEqual(None, opt);
         Assert.AreEqual(None, opt2);
      }

      [Test]
      public void ApplySomeArgs()
      {
         var opt = Some(add)
             .Apply(Some(3))
             .Apply(Some(4));

         Assert.AreEqual(Some(7), opt);
      }

      [Test]
      public void ApplyNoneArgs()
      {
         var opt = Some(add)
             .Apply(None)
             .Apply(Some(4));

         Assert.AreEqual(None, opt);
      }

      [Test] public void ApplicativeLawHolds()
      {
         var first = Some(add)
             .Apply(Some(3))
             .Apply(Some(4));

         var second = Some(3)
             .Map(add)
             .Apply(Some(4));

         Assert.AreEqual(first, second);
      }
   }
}
