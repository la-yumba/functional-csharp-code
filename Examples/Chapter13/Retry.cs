using System;
using System.Net.Http;
using static System.Console;
using System.Threading.Tasks;
using LaYumba.Functional;

namespace Examples.Chapter13
{
   using static F;

   public static class RetryHelper
   {
      public enum RetryStrategy { Exponential, Fixed };

      public static Task<T> _Retry<T>
         (int retries, int delayMillis, Func<Task<T>> start)
         => retries == 0
            ? start()
            : start().OrElse(() =>
               from _ in Task.Delay(delayMillis)
               from t in Retry(retries - 1, delayMillis * 2, start)
               select t);

      public static Task<T> Retry<T>
         (int retries, int delayMillis, Func<Task<T>> start)
         => retries == 0
            ? start()
            : start().OrElse(async () =>
            {
               await Task.Delay(delayMillis);
               return await Retry(retries - 1, delayMillis * 2, start);
            });
      
      public static void _main()
      {
         Retry(10, 100, () => FxApi.GetRate("GBPUSD"));
      }
   }
}
