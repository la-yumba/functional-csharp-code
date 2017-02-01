using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;

namespace Exercises.Chapter4.Solutions
{
   using static F;

   static class Solutions
   {
      // 1. Without looking at any code or documentation (or intllisense), write the function signatures of
      // `OrderByDescending`, `Take` and `Average`, which we used to implement `AverageEarningsOfRichestQuartile`:

      // decimal AverageEarningsOfRichestQuartile(List<Person> population)
      //    => population
      //       .OrderByDescending(p => p.Earnings)
      //       .Take(population.Count/4)
      //       .Select(p => p.Earnings)
      //       .Average();

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



      // 2. Check your answer with the msdn documentation: https://msdn.microsoft.com/en-us/library/system.linq.enumerable(v=vs.110).aspx. 
      // How is `Average` different?

      // Average is the only method call that does not return an IEnumerable;
      // this also means that Average is the only greedy method and causes all the
      // previous ones in the chain to be evaluated



      // 3. Implement a general purpose `Compose` function that takes two unary functions
      // and returns the composition of the two.

      static Func<T1, R> Compose<T1, T2, R>(this Func<T2, R> g, Func<T1, T2> f)
         => x => g(f(x));



      // 4. Implement a `Lookup` extension method on `IDictionary<T>` returning `Option<T>`.

      static Option<T> Lookup<K, T>(this IDictionary<K, T> dict, K key)
      {
         T value;
         return dict.TryGetValue(key, out value) ? Some(value) : None;
      }



      // 5. Use `Bind` and the `Lookup` function from the previous exercise to
      // implement `GetWorkPermit` below.

      static Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> employees, string employeeId)
         => employees.Lookup(employeeId).Bind(e => e.WorkPermit);

      // Then enrich the implementation so that `GetWorkPermit`
      // returns `None` if the work permit has expired.

      static Option<WorkPermit> GetValidWorkPermit(Dictionary<string, Employee> employees, string employeeId)
         => employees
            .Lookup(employeeId)
            .Bind(e => e.WorkPermit)
            .Where(HasExpired.Negate());

      static Func<WorkPermit, bool> HasExpired => permit => permit.Expiry < DateTime.Now.Date;



      // 6. Use `Bind` to implement `AverageYearsWorkedAtTheCompany` below(only
      // employees who have left should be included).

      static double AverageYearsWorkedAtTheCompany(List<Employee> employees)
         => employees
            .Bind(e => e.LeftOn.Map(leftOn => YearsBetween(e.JoinedOn, leftOn)))
            .Average();

      // a more elegant solution, which will become clear in Chapter 9
      static double AverageYearsWorkedAtTheCompany_LINQ(List<Employee> employees)
         => (from e in employees
             from leftOn in e.LeftOn
             select YearsBetween(e.JoinedOn, leftOn)
            ).Average();

      static double YearsBetween(DateTime start, DateTime end)
         => (end - start).Days / 365d;


      // 7. Write implementations of `Where`, `ForEach` and `Bind` for
      // `Container`. Try doing so without looking at the implementations for
      // `Option` first, and only check them if needed.

      // please see the implementation of Container
      // NOTE: `Where` cannot be implemented: since Container contains exactly
      // one value, you cannot "filter out" that value
   }
}