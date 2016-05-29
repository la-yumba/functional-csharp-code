using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LaYumba.Functional.Tests
{
   using static F;

   [TestFixture]
   public class FTest
   {
      private const string HELLO = "hello";
      readonly Predicate<string> isHello = s => s == HELLO;

      [Test]
      public void NegateTest()
      {
         Assert.IsTrue(isHello(HELLO));
         Assert.IsFalse(isHello.Negate()(HELLO));
      }

      [Test]
      public void TestListReturnFunction()
      {
         var emptyList = List<string>();
         Assert.AreEqual(new string[] { }, emptyList);

         var singletonList = List("Andrej");
         Assert.AreEqual(new[] { "Andrej" }, singletonList);

         var multiList = List("Andrej", "Natasha");
         Assert.AreEqual(new[] { "Andrej", "Natasha" }, multiList);
      }
   }
}
