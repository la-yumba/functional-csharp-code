using System;
using System.Linq;
using NUnit.Framework;

namespace LaYumba.Functional.Tests
{
   [TestFixture] public class FuncExtTest
   {
      Func<int, string> toString = i => i.ToString();

      [Test]
      public void FirstFunctorLawHolds()
      {
         var mapped = toString.Map(x => x);
         AreEquivalent(toString, mapped); // Equality doesn't really work well with Func
      }

      [Test]
      public void SecondFunctorLawHolds()
      {
         Func<string, string> f = s => s.Substring(0, 3);
         Func<string, string> g = s => s.ToUpper();

         var a = toString.Map(f).Map(g);
         var b = toString.Map(x => g(f(x)));

         AreEquivalent(a, b);
      }

      private static void AreEquivalent(Func<int, string> a, Func<int, string> b)
         => Assert.AreEqual(true, Enumerable.Range(1000, 1010)
            .Map(i => a(i) == b(i))
            .All(eq => eq));
   }
}
