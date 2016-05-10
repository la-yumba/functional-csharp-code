using System;
using NUnit.Framework;

namespace LaYumba.Functional.Tests.Option
{
   [TestFixture]
   public class Map
   {
      class Apple { }
      class ApplePie { public ApplePie(Apple apple) { } }

      Func<Apple, ApplePie> makePie = apple => new ApplePie(apple);

      [Test]
      public void GivenSomeApple_WhenMakePie_ThenSomePie()
      {
         var appleOpt = Functional.Option.Of(new Apple());
         var pieOpt = appleOpt.Map(makePie);
         Assert.IsTrue(pieOpt.IsSome);
      }

      [Test]
      public void GivenNoApple_WhenMakePie_ThenNoPie()
      {
         var appleOpt = Functional.Option.Of((Apple)null);
         //Option<Apple> appleOpt = None;
         var pieOpt = appleOpt.Map(makePie);
         Assert.IsTrue(pieOpt.IsNone);
      }
   }
}