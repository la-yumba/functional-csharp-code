using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Examples.Chapter13
{
   using static Instrumentation;
   using static Enumerable;

   public class Applicative
   {
      public static void _main()
      {
         Range(1, 2).ForEach(_ => // do a few rounds to get ; the first is always penalized
         {
            Time("a + b", () =>
               (from a in Return(300, "a")
                from b in Return(500, "b")
                select a + b).Result
               );

            Time("c + d", () =>
               Return(300, "c").
               Map<string, string, string>((l, r) => l + r).
               Apply(Return(500, "d")).Result);
         });
         
         Time("Applicative Traverse", () => // takes ~500 ms
            Range(1, 5).TraverseA(async i =>
            {
               await Task.Delay(i * 100);
               return i;
            })
            .Map(xs => xs.Sum())
            .Result);

         Time("Monadic Traverse", () => // takes ~1500 ms
            Range(1, 5).TraverseM(async i =>
            {
               await Task.Delay(i * 100);
               return i;
            })
            .Map(xs => xs.Sum())
            .Result);
      }

      static async Task<string> Return(int delay, string val)
      {
         await Task.Delay(delay);
         return val;
      }

      interface Airline
      {
         Task<Flight> BestFare(string from, string to, DateTime on);
         Task<IEnumerable<Flight>> Flights(string from, string to, DateTime on);
      }

      Airline ryanair;
      Airline easyjet;

      Task<Flight> BestFareM(string @from, string to, DateTime @on)
         => from r in ryanair.BestFare(@from, to, @on)
            from e in easyjet.BestFare(@from, to, @on)
            select r.Price < e.Price ? r : e;

      Task<Flight> BestFareA(string from, string to, DateTime on)
         => Async(PickCheaper)
            .Apply(ryanair.BestFare(from, to, on))
            .Apply(easyjet.BestFare(from, to, on));

      static Func<Flight, Flight, Flight> PickCheaper 
         = (l, r) => l.Price < r.Price ? l : r;

      async Task<IEnumerable<Flight>> Search(IEnumerable<Airline> airlines
         , string from, string to, DateTime on)
      {
         //var results = await airlines.Traverse(a => a.Flights(from, to, on).Recover(ex => Empty<Flight>()));
         var results = await airlines.Traverse(SafelySearch(from, to, on));
         return results.Flatten().OrderBy(f => f.Price);
      }

      Func<Airline, Task<IEnumerable<Flight>>> SafelySearch
         (string from, string to, DateTime on)
         => airline
         => airline.Flights(from, to, on)
                   .Recover(ex => Empty<Flight>());

      Task<IEnumerable<Flight>> _Search(IEnumerable<Airline> airlines
         , string @from, string to, DateTime @on)
         => from results in airlines.Traverse(a => a.Flights(@from, to, @on))
            select results.Flatten().OrderBy(f => f.Price).AsEnumerable();

      class Flight
      {
         public decimal Price { get; set; }
      }
   }
}
