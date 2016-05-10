using System;
using NUnit.Framework;

namespace LaYumba.Functional.Tests.Option
{
   [TestFixture]
   public class FunctorLaws
   {
      [TestCase("Hello"), TestCase(null)]
      public void FirstFunctorLawHolds(string s)
      {
         var a = F.Some(s);
         var b = a.Map(x => x);
         Assert.AreEqual(a, b);
      }

      [TestCase("Hello"), TestCase(null)]
      public void SecondFunctorLawHolds(string input)
      {
         Func<string, string> f = s => s.Substring(0, 3);
         Func<string, string> g = s => s.ToUpper();

         var a = F.Some(input).Map(f).Map(g);
         var b = F.Some(input).Map(x => g(f(x)));

         Assert.AreEqual(a, b);
      }

      [TestCase(5)]
      public void WhenOrderMatters_SecondFunctorLawStillHolds(int input)
      {
         Func<int, int> f = i => i - 2;
         Func<int, int> g = i => i * 50;

         var a = F.Some(input).Map(f).Map(g);
         var b = F.Some(input).Map(x => g(f(x)));

         Assert.AreEqual(a, b);
      }
   }
}