using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using LaYumba.Functional;

using static System.Console;

namespace Examples.Chapter14
{
   static class CreatingObservables
   {
      public static class Timer
      {
         public static void Run()
         {
            var oneSec = TimeSpan.FromMilliseconds(1000);

            var ticks = Observable.Interval(oneSec);

            ticks.Take(10).Subscribe(Console.WriteLine);

            Task.Delay(12_000).Wait();
         }
      }

      public static class Subjects
      {
         public static void Run()
         {
            WriteLine("Enter some inputs to push them to 'inputs', or 'q' to quit");

            var inputs = new Subject<string>();

            using (inputs.Trace("inputs"))
            {
               for (string input; (input = ReadLine()) != "q";)
                  inputs.OnNext(input);
               inputs.OnCompleted();
            }
         }
      }

      // alternative methods to capture console inputs as a stream

      public static class Create
      {
         public static void Run()
         {
            var inputs = Observable.Create<string>(observer =>
            {
               for (string input; (input = ReadLine()) != "q";)
                  observer.OnNext(input);
               observer.OnCompleted();

               return () => {};
            });

            inputs.Trace("inputs");
         }
      }

      public static class Generate
      {
         public static void Run()
         {
            var inputs = Observable.Generate<string, string>(ReadLine()
               , input => input != "q"
               , _ => ReadLine()
               , input => input);

            inputs.Trace("inputs");
         }
      }
   }
}
