using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using static System.Console;
using System.Collections.Generic;

namespace Examples.Chapter7.Apply
{
   public static class MapBinaryFuncExample
   {
      static Func<int, int, int> multiply = (i, j) => i * j;
      static Func<int, int> @double = i => i * 2;

      internal static void MapUnaryFunction()
      {
         Option<int> double3 = Some(3).Map(@double);
         // => Some(6)
         
         IEnumerable<int> doubles = Range(1, 3).Map(@double);
         // => [2, 4, 6]
      }

      internal static void ApplyBinaryFunctionByLowering()
      {
         Option<int> a = Some(3);
         Option<int> b = Some(4);

         var result = a.Match(
            () => None,
            valA => b.Match(
               () => None,
               valB => Some(multiply(valA, valB))
            )
         );
         // => Some(12)

         Some(3).Map(multiply.Curry());
      }

      internal static void _main()
      {
         Option<Func<int, int>> multiplyBy3 = Some(3).Map(multiply);
         // => Some(x => multiply(3, x))
         IEnumerable<Func<int, int>> multiplers = Range(1, 3).Map(multiply);
         // => [x => multiply(1, x), x => multiply(2, x), x => multiply(3, x)]

         //multiplers.Map(f => f(2)).ForEach(Out.WriteLine);
         Some(3).Map(multiply).Apply(Some(2));
      }

      static Option<int> MultiplicationWithBind(string strX, string strY)
         => Int.Parse(strX).Bind(x => Int.Parse(strY).Bind<int, int>(y => multiply(x, y)));
   }
}
