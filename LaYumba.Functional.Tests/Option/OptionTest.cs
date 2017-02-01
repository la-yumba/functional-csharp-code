using Xunit;
using System;
using FsCheck.Xunit;

namespace LaYumba.Functional.Tests
{
   using static F;
   
   public class OptionTest
   {
      [Fact]
      public void MatchCallsAppropriateFunc()
      {
         Assert.Equal("hello, John", Greet(Some("John")));
         Assert.Equal("sorry, who?", Greet(None));
      }

      private string Greet(Option<string> name) 
         => name.Match(
               Some: n => $"hello, {n}",
               None: () => "sorry, who?");

      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      public void SingleClauseLINQExpr(Option<string> opt)
         => Assert.Equal(
               from x in opt select String.ToUpper(x),
               opt.Map(String.ToUpper));

      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      public void TwoClauseLINQExpr(Option<string> optA, Option<string> optB)
         => Assert.Equal(
             from a in optA
             from b in optB
             select a + b,
             optA.Bind(a => optB.Map(b => a + b)));

      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      public void ThreeClauseLINQExpr(Option<string> optA, Option<string> optB, Option<string> optC)
         => Assert.Equal(
             from a in optA
             from b in optB
             from c in optC
             select a + b + c,
             optA.Bind(a => optB.Bind(b => optC.Map(c => a + b + c))));
   }


   public class Option_Map_Test
   {
      class Apple { }
      class ApplePie { public ApplePie(Apple apple) { } }

      Func<Apple, ApplePie> makePie = apple => new ApplePie(apple);

      [Fact]
      public void GivenSomeApple_WhenMakePie_ThenSomePie()
      {
         var appleOpt = Some(new Apple());
         var pieOpt = appleOpt.Map(makePie);
         Assert.True(pieOpt.IsSome());
      }

      [Fact]
      public void GivenNoApple_WhenMakePie_ThenNoPie()
      {
         Option<Apple> appleOpt = None;
         var pieOpt = appleOpt.Map(makePie);
         Assert.False(pieOpt.IsSome());
      }
   }

   public class Option_Apply_Test
   {
      Func<int, int, int> add = (a, b) => a + b;
      Func<int, int, int> multiply = (i, j) => i * j;

      [Fact]
      public void MapAndApplySomeArg_ReturnsSome()
      {
         var opt = Some(3)
             .Map(multiply)
             .Apply(Some(4));

         Assert.Equal(Some(12), opt);
      }

      [Property]
      public void MapAndApplyNoneArg_ReturnsNone(int i)
      {
         var opt = Some(i)
             .Map(multiply)
             .Apply(None);

         var opt2 = ((Option<int>)None)
             .Map(multiply)
             .Apply(i);

         Assert.Equal(None, opt);
         Assert.Equal(None, opt2);
      }

      [Fact]
      public void ApplySomeArgs()
      {
         var opt = Some(add)
             .Apply(Some(3))
             .Apply(Some(4));

         Assert.Equal(Some(7), opt);
      }

      [Property]
      public void ApplyNoneArgs(int i)
      {
         var opt = Some(add)
             .Apply(None)
             .Apply(Some(i));

         Assert.Equal(None, opt);
      }
   }
}