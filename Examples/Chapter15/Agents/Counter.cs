using System.Threading;
using static System.Console;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System.Threading.Tasks;

namespace Examples.Agents
{
   public class Counter
   {
      public static void PrintThreadOf(string op)
         => WriteLine($"{op} on thread: {Thread.CurrentThread.ManagedThreadId}");

      private readonly Agent<int, int> counter = 
         Agent.Start(0, (int state, int msg) =>
         {
            var newState = state + msg;
            PrintThreadOf("Counter");
            return (newState, newState);
         });

      // public interface of the Counter
      public Task<int> IncrementBy(int by) => counter.Tell(by);

      public static void main()
      {
         PrintThreadOf("Main");
         var counter = new Counter();

         for (string input; (input = ReadLine().ToUpper()) != "Q";)
         {
            var newCount = counter.IncrementBy(int.Parse(input)).Result;
            PrintThreadOf("received result");
            WriteLine(newCount);
         }
      }
   }
}
