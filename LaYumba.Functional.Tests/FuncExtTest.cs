using System;
using System.Linq;
using Xunit;

namespace LaYumba.Functional.Tests
{
   public class FuncExtTest
   {
      Func<int, string> toString = i => i.ToString();

      [Fact]
      public void FirstFunctorLawHolds()
      {
         var mapped = toString.Map(x => x);
         AreEquivalent(toString, mapped); // Equality doesn't really work well with Func
      }

      [Fact]
      public void SecondFunctorLawHolds()
      {
         Func<string, string> f = s => s.Substring(0, 3);
         Func<string, string> g = s => s.ToUpper();

         var a = toString.Map(f).Map(g);
         var b = toString.Map(x => g(f(x)));

         AreEquivalent(a, b);
      }

      private static void AreEquivalent(Func<int, string> a, Func<int, string> b)
         => Assert.Equal(true, Enumerable.Range(1000, 1010)
            .Map(i => a(i) == b(i))
            .All(eq => eq));
   }

   public class Func_Apply_Test
   {
      Func<int, int, int> add = (a, b) => a + b;

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
      public void ApplyArgs()
      {
         var add1 = add.Apply(1);
         var actual = add1(1);

         Assert.Equal(2, actual);
      }

      [Fact]
      public void ApplyArgs_to_function_requiring_3_args()
      {
         var f = add3Integers.Apply(1)
            .Apply(2);

         var result = f(3);
         
         Assert.Equal(6, result);
      }

      [Fact]
      public void ApplyArgs_to_function_requiring_4_args()
      {
         var f = add4Integers.Apply(1)
            .Apply(2)
            .Apply(3);

         var result = f(4);
         
         Assert.Equal(10, result);
      }

      [Fact]
      public void ApplyArgs_to_function_requiring_5_args()
      {
         var f = add5Integers.Apply(1)
            .Apply(2)
            .Apply(3)
            .Apply(4);

         var result = f(5);
         
         Assert.Equal(15, result);
      }

      [Fact]
      public void ApplyArgs_to_function_requiring_6_args()
      {
         var f = add6Integers.Apply(1)
            .Apply(2)
            .Apply(3)
            .Apply(4)
            .Apply(5);

         var result = f(6);
         
         Assert.Equal(21, result);
      }

      [Fact]
      public void ApplyArgs_to_function_requiring_7_args()
      {
         var f = add7Integers.Apply(1)
            .Apply(2)
            .Apply(3)
            .Apply(4)
            .Apply(5)
            .Apply(6);

         var result = f(7);
         
         Assert.Equal(28, result);
      }

      [Fact]
      public void ApplyArgs_to_function_requiring_8_args()
      {
         var f = add8Integers.Apply(1)
            .Apply(2)
            .Apply(3)
            .Apply(4)
            .Apply(5)
            .Apply(6)
            .Apply(7);

         var result = f(8);
         
         Assert.Equal(36, result);
      }

      [Fact]
      public void ApplyArgs_to_function_requiring_9_args()
      {
         var f = add9Integers.Apply(1)
            .Apply(2)
            .Apply(3)
            .Apply(4)
            .Apply(5)
            .Apply(6)
            .Apply(7)
            .Apply(8);

         var result = f(9);
         
         Assert.Equal(45, result);
      }
   }
}
