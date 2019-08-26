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

      private readonly Func<int, int, int, int> add3Integers = 
         (a, b, c) => a + b + c;
      private readonly Func<int, int, int, int, int> add4Integers = 
         (a, b, c, d) => a + b + c + d;
      private readonly Func<int, int, int, int, int, int> add5Integers = 
         (a, b, c, d, e) => a + b + c + d + e;
      private readonly Func<int, int, int, int, int, int, int> add6Integers = 
         (a, b, c, d, e, f) => a + b + c + d + e + f;
      private readonly Func<int, int, int, int, int, int, int, int> add7Integers = 
         (a, b, c, d, e, f, g) => a + b + c + d + e + f + g;
      private readonly Func<int, int, int, int, int, int, int, int, int> add8Integers = 
         (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h;
      private readonly Func<int, int, int, int, int, int, int, int, int, int> add9Integers = 
         (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i;

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

      [Fact]
      public void ApplySomeArgs_to_function_requiring_3_args()
      {
         var opt = Some(add3Integers)
             .Apply(Some(1))
             .Apply(Some(2))
             .Apply(Some(3));

         Assert.Equal(Some(6), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_4_args()
      {
         var opt = Some(add4Integers)
             .Apply(Some(1))
             .Apply(Some(2))
             .Apply(Some(3))
             .Apply(Some(4));

         Assert.Equal(Some(10), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_5_args()
      {
         var opt = Some(add5Integers)
             .Apply(Some(1))
             .Apply(Some(2))
             .Apply(Some(3))
             .Apply(Some(4))
             .Apply(Some(5));

         Assert.Equal(Some(15), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_6_args()
      {
         var opt = Some(add6Integers)
             .Apply(Some(1))
             .Apply(Some(2))
             .Apply(Some(3))
             .Apply(Some(4))
             .Apply(Some(5))
             .Apply(Some(6));

         Assert.Equal(Some(21), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_7_args()
      {
         var opt = Some(add7Integers)
             .Apply(Some(1))
             .Apply(Some(2))
             .Apply(Some(3))
             .Apply(Some(4))
             .Apply(Some(5))
             .Apply(Some(6))
             .Apply(Some(7));

         Assert.Equal(Some(28), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_8_args()
      {
         var opt = Some(add8Integers)
             .Apply(Some(1))
             .Apply(Some(2))
             .Apply(Some(3))
             .Apply(Some(4))
             .Apply(Some(5))
             .Apply(Some(6))
             .Apply(Some(7))
             .Apply(Some(8));

         Assert.Equal(Some(36), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_9_args()
      {
         var opt = Some(add9Integers)
             .Apply(Some(1))
             .Apply(Some(2))
             .Apply(Some(3))
             .Apply(Some(4))
             .Apply(Some(5))
             .Apply(Some(6))
             .Apply(Some(7))
             .Apply(Some(8))
             .Apply(Some(9));

         Assert.Equal(Some(45), opt);
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