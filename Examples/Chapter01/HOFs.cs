using System;

namespace Examples.Chapter1
{
   using static Console;

   static class HOFs
   {
      internal static void Run()
      {
         Func<double, double, double> divide = (dividend, divisor) => dividend / divisor;
         WriteLine(divide(10, 2)); // => 5

         var divideBy = divide.SwapArgs();
         WriteLine(divideBy(2, 10)); // => 5
      }

      public static Func<T2, T1, R> SwapArgs<T1, T2, R>(this Func<T1, T2, R> func)
         => (t2, t1) => func(t1, t2);
   }
}
