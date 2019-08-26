using System;
using System.Linq;
using Xunit;
using static LaYumba.Functional.F;

namespace LaYumba.Functional.Tests
{
   public class Valid_Test
   {
      Validation<int> Invalid(string m = "Some error") => new Error(m);

      Func<int, int, int> add = (a, b) => a + b;
      Func<int, int, int> Multiply = (i, j) => i * j;

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

      Func<string, Validation<int>> parseInt => 
         s => Int.Parse(s).Match(
            None: () => new Error($"{s} is not an int"),
            Some: (i) => Valid(i));
      
      // test that errors are accumulated

      [Fact]
      public void ItTracksErrors() => Assert.Equal(
         actual: Valid(add)
             .Apply(parseInt("4"))
             .Apply(parseInt("x")),
         expected: Invalid("x is not an int")
      );

      [Fact]
      public void ItAccumulatesErrors() => Assert.Equal(
         actual: Valid(add)
            .Apply(parseInt("y"))
            .Apply(parseInt("x"))
            .Errors.Count(),
         expected: 2
      );

      [Fact]
      public void TraversableA_HappyPath() => Assert.Equal(
         actual: Range(1, 4).Map(i => i.ToString())
            .Traverse(parseInt)
            .Map(list => list.Sum()),
         expected: Valid(10)
      );

      [Fact]
      public void TraversableA_UnhappyPath() => Assert.Equal(
         actual: List("1", "2", "rubbish", "4", "more rubbish")
            .Traverse(parseInt)
            .Map(list => list.Sum())
            .Errors.Count(),
         expected: 2
      );

      // standard applicative tests

      [Fact]
      public void MapAndApplySomeArg_ReturnsSome() => Assert.Equal(
         actual: Valid(3)
                  .Map(Multiply)
                  .Apply(Valid(4)),
         expected: Valid(12)
      );

      [Fact]
      public void MapAndApplyNoneArg_ReturnsNone()
      {
         var opt = Valid(3)
             .Map(Multiply)
             .Apply(Invalid());

         var opt2 = (Invalid())
             .Map(Multiply)
             .Apply(Valid(4));

         Assert.Equal(Invalid(), opt);
         Assert.Equal(Invalid(), opt2);
      }

      [Fact]
      public void ApplySomeArgs()
      {
         var opt = Valid(add)
             .Apply(Valid(3))
             .Apply(Valid(4));

         Assert.Equal(Valid(7), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_3_args()
      {
         var opt = Valid(add3Integers)
            .Apply(Valid(1))
            .Apply(Valid(2))
            .Apply(Valid(3));

         Assert.Equal(Valid(6), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_4_args()
      {
         var opt = Valid(add4Integers)
            .Apply(Valid(1))
            .Apply(Valid(2))
            .Apply(Valid(3))
            .Apply(Valid(4));

         Assert.Equal(Valid(10), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_5_args()
      {
         var opt = Valid(add5Integers)
            .Apply(Valid(1))
            .Apply(Valid(2))
            .Apply(Valid(3))
            .Apply(Valid(4))
            .Apply(Valid(5));

         Assert.Equal(Valid(15), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_6_args()
      {
         var opt = Valid(add6Integers)
            .Apply(Valid(1))
            .Apply(Valid(2))
            .Apply(Valid(3))
            .Apply(Valid(4))
            .Apply(Valid(5))
            .Apply(Valid(6));

         Assert.Equal(Valid(21), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_7_args()
      {
         var opt = Valid(add7Integers)
            .Apply(Valid(1))
            .Apply(Valid(2))
            .Apply(Valid(3))
            .Apply(Valid(4))
            .Apply(Valid(5))
            .Apply(Valid(6))
            .Apply(Valid(7));

         Assert.Equal(Valid(28), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_8_args()
      {
         var opt = Valid(add8Integers)
            .Apply(Valid(1))
            .Apply(Valid(2))
            .Apply(Valid(3))
            .Apply(Valid(4))
            .Apply(Valid(5))
            .Apply(Valid(6))
            .Apply(Valid(7))
            .Apply(Valid(8));

         Assert.Equal(Valid(36), opt);
      }

      [Fact]
      public void ApplySomeArgs_to_function_requiring_9_args()
      {
         var opt = Valid(add9Integers)
            .Apply(Valid(1))
            .Apply(Valid(2))
            .Apply(Valid(3))
            .Apply(Valid(4))
            .Apply(Valid(5))
            .Apply(Valid(6))
            .Apply(Valid(7))
            .Apply(Valid(8))
            .Apply(Valid(9));

         Assert.Equal(Valid(45), opt);
      }
      
      [Fact]
      public void ApplyNoneArgs()
      {
         var opt = Valid(add)
             .Apply(Invalid())
             .Apply(Valid(4));

         Assert.Equal(Invalid(), opt);
      }

      [Fact] public void ApplicativeLawHolds()
      {
         var first = Valid(add)
             .Apply(Valid(3))
             .Apply(Valid(4));

         var second = Valid(3)
             .Map(add)
             .Apply(Valid(4));

         Assert.Equal(first, second);
      }
   }
}
