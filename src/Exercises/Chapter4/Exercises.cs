using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;

namespace Exercises.Chapter4
{
   class Exercises
   {
      // 1. Without looking at any code or documentation (or intllisense), write the function signatures of
      // `OrderByDescending`, `Take` and `Average`, which we used to implement `AverageEarningsOfRichestQuartile`:
      decimal AverageEarningsOfRichestQuartile(List<Person> population)
         => population
            .OrderByDescending(p => p.Earnings)
            .Take(population.Count/4)
            .Select(p => p.Earnings)
            .Average();

      // 2. Check your answer with the msdn documentation: https://msdn.microsoft.com/en-us/library/system.linq.enumerable(v=vs.110).aspx. 
      // How is `Average` different?

      // 3. Implement a general purpose `Compose` function that takes two unary functions
      // and returns the composition of the two.

      // 4. Implement a `Lookup` extension method on `IDictionary<T>` returning `Option<T>`.

      // 5. Use `Bind` and the `Lookup` function from the previous exercise to
      // implement `GetWorkPermit` below.Then enrich the implementation so that `GetWorkPermit`
      // returns `None` if the work permit has expired.

      Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> people, string employeeId)
      {
         throw new NotImplementedException();
      }

      // 6. Use `Bind` to implement `AverageYearsWorkedAtTheCompany` below(only
      // employees who have left should be included).

      double AverageYearsWorkedAtTheCompany(List<Employee> employees)
      {
         // your implementation here...
         throw new NotImplementedException();
      }

      // 7. Write implementations of `Where`, `ForEach` and `Bind` for
      // `Container`. Try doing so without looking at the implementations for
      // `Option` first, and only check them if needed.

      // public static Container<R> Map ...
      // public static Container<T> Where ...
      // public static Container<R> Bind ...
      // public static Unit ForEach ...

   }

   public struct WorkPermit
   {
      public string Number { get; set; }
      public DateTime Expiry { get; set; }
   }

   public class Employee
   {
      public string Id { get; set; }
      public Option<WorkPermit> WorkPermit { get; set; }

      public DateTime JoinedOn { get; }
      public Option<DateTime> LeftOn { get; }
   }
}