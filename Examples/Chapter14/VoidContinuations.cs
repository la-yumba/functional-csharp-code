using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using LaYumba.Functional;

namespace Examples.Chapter14
{
   public static class VoidContinuations
   {
      public static void Run()
      {
         Action<int> test = i => WriteLine($"Testing {i}...");

         WriteLine("DoNTimes(3, i => WriteLine($\"Testing { i}...\"));");
         DoNTimes(3, test);

         WriteLine();

         WriteLine("Range(1, 10).Where(i => i % 3 == 0)(i => WriteLine($\"Testing { i}...\"));");
         Range(1, 10)
            .Where(i => i % 3 == 0)
            .Map(i => $"Testing {i}...")
               (WriteLine);

         WriteLine();

         WriteLine("Timer(500).Take(5)(i => WriteLine($\"Testing { i}...\"));");
         Timer(500).Take(5)(test);

         Task.Delay(3000).Wait();
      }

      // naive functions for side effects

      static void DoNTimes(int n, Action<int> action)
         => Enumerable.Range(1, 3).ForEach(action);

      static async void DoWithDelay(int millis, Action<int> action)
      {
         int i = 0;
         while (true)
         {
            await Task.Delay(millis);
            action(i);
            i++;
         }
      }

      // reveals that Action<Action<T>> is a continuation

      static Action<Action<int>> Range(int start, int count)
         => action
         => Enumerable.Range(start, count).ForEach(action);

      static Action<Action<int>> Timer(int millis) => async action =>
      {
         int i = 0;
         while (true)
         {
            await Task.Delay(millis);
            action(i);
            i++;
         }
      };

      // operators

      static Action<Action<T>> Where<T>
         (this Action<Action<T>> source, Func<T, bool> pred)
         => action
         => source(t => { if (pred(t)) action(t); });

      static Action<Action<R>> Map<T, R>
         (this Action<Action<T>> source, Func<T, R> f)
         => action
         => source(t => action(f(t)));

      static Action<Action<T>> Take<T>(this Action<Action<T>> source, int take)
         => action =>
      {
         int i = 1;
         source(t =>
         {
            if (take < i) return;
            action(t);
            i++;
         });
      };
   }
}
