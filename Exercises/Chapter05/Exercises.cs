using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exercises.Chapter5
{
   public static class Exercises
   {
      // 1. Write some tests to illustrate that `IEnumerable` obeys the functor laws
      // (tip: you can take the tests for `Option` in LaYumba.Functional.Tests/Option/FunctorLaws
      // as a starting point).
     
      // 2. Implement `Map` in terms of `Bind` and `Return` for `Container`, `Option`
      // and `IEnumerable`.

      // 3. Write a class `Switch<T, R>` that would enable you to write the code
      // in the snippet below. (Hint: notice how each `.Case` is equivalent to a
      // `KeyValuePair` in the `TransferMappingRules` Dictionary we have used
      // above.)

      //static string DailyComment(DateTime day)
      //{
      //   Predicate<DateTime> always = d => true;
      //   Predicate<DateTime> weekend = d => d.DayOfWeek == DayOfWeek.Saturday
      //                                   || d.DayOfWeek == DayOfWeek.Sunday;
      //   Predicate<DateTime> friday = d => d.DayOfWeek == DayOfWeek.Friday;
      //   Predicate<DateTime> nearEOY = d => d.Month == 12 && d.Day > 25;

      //   var @switch = Switch<DateTime, string>()
      //       .Case(always, d => d.DayOfWeek.ToString() + ":")
      //       .Case(weekend, d => "chilling...")
      //       .Case(friday, d => "thank God it's the weekend!")
      //       .Case(nearEOY, d => "getting ready for a party!");

      //   IEnumerable<string> comments = @switch.MatchAll(day);
      //   return string.Join(" ", comments.ToArray());
      //}

      
      // 4. Write tests to ensure it works as expected.

      // 5. Exhance `Switch<T, R>` so that it can also be used like the `switch`
      // statement in the C# language, by adding an overload taking a constant
      // value to match against `Case(T value, Func<T, R> func)`, and a `Match`
      // method that returns the result of invoking the function of the _first_
      // matching case.

      // Write a simple scenario with those methods, and unit test it

   }
}
