using System;
using NUnit.Framework;

namespace LaYumba.Functional.Tests.Option
{
   [TestFixture]
   public class MonadLaws
   {
      [TestCase("Hello"), TestCase(null)]
      public void RightIdentityHolds(string s)
      {
         var a = F.Some(s);
         var b = a.Bind(F.Some);
         Assert.AreEqual(a, b);
      }

      [TestCase("Hello")]
      [TestCase(null), Ignore("does not hold for null")]
      public void LeftIdentityHolds(string x)
      {
         Func<string, Option<string>> f = s => F.Some($"{s} World");

         Assert.AreEqual(F.Some(x).Bind(f), f(x));
      }
   }
}

