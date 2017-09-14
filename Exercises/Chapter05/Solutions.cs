using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Examples.Chapter3;

namespace Exercises.Chapter5.Solutions
{
   static class Exercises
   {
      // 1. Without looking at any code or documentation (or intllisense), write the function signatures of
      // `OrderByDescending`, `Take` and `Average`, which we used to implement `AverageEarningsOfRichestQuartile`:
      static decimal AverageEarningsOfRichestQuartile(List<Person> population)
         => population
            .OrderByDescending(p => p.Earnings)
            .Take(population.Count/4)
            .Select(p => p.Earnings)
            .Average();

      // OrderByDescending : (IEnumerable<T>, (T -> decimal)) -> IEnumerable<T>
      // particularized for this case:
      // OrderByDescending : (IEnumerable<Person>, (Person -> decimal)) -> IEnumerable<Person>

      // Take : (IEnumerable<T>, int) -> IEnumerable<T>
      // particularized for this case:
      // Take : (IEnumerable<Person>, int) -> IEnumerable<Person>

      // Select : (IEnumerable<T>, (T -> R)) -> IEnumerable<R>
      // particularized for this case:
      // Select : (IEnumerable<Person>, (Person -> decimal)) -> IEnumerable<decimal>

      // Average : IEnumerable<T> -> T
      // particularized for this case:
      // Average : IEnumerable<decimal> -> decimal


      // 2 Check your answer with the MSDN documentation: https://docs.microsoft.com/
      // en-us/dotnet/api/system.linq.enumerable. How is Average different?

      // Average is the only method call that does not return an IEnumerable;
      // this also means that Average is the only greedy method and causes all the
      // previous ones in the chain to be evaluated


      // 3 Implement a general purpose Compose function that takes two unary functions
      // and returns the composition of the two.

      static Func<T1, R> Compose<T1, T2, R>(this Func<T2, R> g, Func<T1, T2> f)
         => x => g(f(x));
   }
}
