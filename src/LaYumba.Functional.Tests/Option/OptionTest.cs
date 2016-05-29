using NUnit.Framework;
using System;

namespace LaYumba.Functional.Tests.Chapter1
{
   using static F;
   using static Assert;

   [TestFixture]
   public class OptionTest
   {
      [TestCase("Hello", ExpectedResult = true)]
      [TestCase(null, ExpectedResult = false)]
      public bool ValidOption_IsSome(string s)
         => Some(s).IsSome;

      [TestCase("John", ExpectedResult = "hello, John")]
      [TestCase(null, ExpectedResult = "sorry, who?")]
      public string MatchCallsAppropriateFunc(string name)
         => Some(name).Match(
            Some: n => $"hello, {n}",
            None: () => "sorry, who?");

      [TestCase(ExpectedResult = true)]
      public bool NoneField_IsNone() => Option<string>.None.IsNone;
   }

   [TestFixture]
   public class Option_Map_Test
   {
      class Apple { }
      class ApplePie { public ApplePie(Apple apple) { } }

      Func<Apple, ApplePie> makePie = apple => new ApplePie(apple);

      [Test]
      public void GivenSomeApple_WhenMakePie_ThenSomePie()
      {
         var appleOpt = Option.Of(new Apple());
         var pieOpt = appleOpt.Map(makePie);
         Assert.IsTrue(pieOpt.IsSome);
      }

      [Test]
      public void GivenNoApple_WhenMakePie_ThenNoPie()
      {
         var appleOpt = Option.Of((Apple)null);
         //Option<Apple> appleOpt = None;
         var pieOpt = appleOpt.Map(makePie);
         Assert.IsTrue(pieOpt.IsNone);
      }
   }

   [TestFixture]
   public class Option_FunctorLaws
   {
      [TestCase("Hello"), TestCase(null)]
      public void FirstFunctorLawHolds(string s)
      {
         var a = Some(s);
         var b = a.Map(x => x);
         AreEqual(a, b);
      }

      [TestCase("Hello"), TestCase(null)]
      public void SecondFunctorLawHolds(string input)
      {
         Func<string, string> f = s => s.Substring(0, 3);
         Func<string, string> g = s => s.ToUpper();

         var a = Some(input).Map(f).Map(g);
         var b = Some(input).Map(x => g(f(x)));

         AreEqual(a, b);
      }

      [TestCase(5)]
      public void WhenOrderMatters_SecondFunctorLawStillHolds(int input)
      {
         Func<int, int> f = i => i - 2;
         Func<int, int> g = i => i * 50;

         var a = Some(input).Map(f).Map(g);
         var b = Some(input).Map(x => g(f(x)));

         AreEqual(a, b);
      }
   }

   [TestFixture]
   public class Option_MonadLaws
   {
      [TestCase("Hello"), TestCase(null)]
      public void RightIdentityHolds(string s)
      {
         var a = Some(s);
         var b = a.Bind(Some);
         AreEqual(a, b);
      }

      [TestCase("Hello")]
      [TestCase(null), Ignore("does not hold for null")]
      public void LeftIdentityHolds(string x)
      {
         Func<string, Option<string>> f = s => Some($"{s} World");

         AreEqual(Some(x).Bind(f), f(x));
      }
   }
}

