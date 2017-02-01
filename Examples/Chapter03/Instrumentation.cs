using System;
using System.Diagnostics;
using LaYumba.Functional;
using Microsoft.Extensions.Logging;
using Unit = System.ValueTuple;

namespace Examples
{
   public static class Instrumentation
   {
      public static T Time<T>(ILogger log, string op, Func<T> f)
      {
         var sw = new Stopwatch();
         sw.Start();

         T t = f();

         sw.Stop();
         log.LogDebug($"{op} took {sw.ElapsedMilliseconds}ms");
         return t;
      }

      public static T Trace<T>(ILogger log, string op, Func<T> f)
      {
         log.LogTrace($"Entering {op}");
         T t = f();
         log.LogTrace($"Leaving {op}");
         return t;
      }

      public static T Trace<T>(Action<string> log, string op, Func<T> f)
      {
         log($"Entering {op}");
         T t = f();
         log($"Leaving {op}");
         return t;
      }

      public static T Time<T>(string op, Func<T> f)
      {
         var sw = new Stopwatch();
         sw.Start();

         T t = f();

         sw.Stop();
         Console.WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
         return t;
      }

      /* duplication!
      public static void Time(string op, Action act)
      {
         var sw = new Stopwatch();
         sw.Start();

         act();

         sw.Stop();
         Console.WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
      }*/

      public static void Time(string op, Action act)
         => Time<Unit>(op, act.ToFunc());
   }
}
