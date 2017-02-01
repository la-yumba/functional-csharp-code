using System;
using System.Linq;
using Xunit;
using static LaYumba.Functional.F;

namespace LaYumba.Functional.Tests
{
   class Valid_Test
   {
      Validation<int> Invalid(string m = "Some error") => new Error(m);

      Func<int, int, int> add = (a, b) => a + b;
      Func<int, int, int> Multiply = (i, j) => i * j;

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
