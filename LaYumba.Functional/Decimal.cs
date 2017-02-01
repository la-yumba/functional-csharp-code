using System;
using static LaYumba.Functional.F;

namespace LaYumba.Functional
{
   public static class Decimal
   {
      public static Option<decimal> Parse(string s)
      {
         decimal result;
         return decimal.TryParse(s, out result)
            ? Some(result) : None;
      }

      public static bool IsOdd(decimal i) => i % 2 == 1;

      public static bool IsEven(decimal i) => i % 2 == 0;

      public static new Func<decimal, string> ToString = d => d.ToString();
   }
}
