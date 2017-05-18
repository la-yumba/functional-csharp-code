using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exercises.Chapter5.Solutions
{
   static class Exercises
   {
      // 1. Write some tests to illustrate that `IEnumerable` obeys the functor laws
      // (tip: you can take the tests for `Option` in LaYumba.Functional.Tests/Option/FunctorLaws
      // as a starting point).

      // Answers in LaYumba.Functional.Tests/IEnumerable/FunctorLaws

      // 2. Implement `Map` in terms of `Bind` and `Return` for `Option`
      // and `IEnumerable`.
      
      static Option<R> Map<T, R>(this Option<T> @this
         , Func<T, R> func)
         => @this.Bind(v => Some(func(v)));

      static IEnumerable<R> Map<T, R>(this IEnumerable<T> @this
         , Func<T, R> func)
         => @this.Bind(v => List(func(v)));

      // 3. Write a class `Switch<T, R>` that would enable you to write the code
      // in the snippet below. (Hint: notice how each `.Case` is equivalent to a
      // `KeyValuePair` in the `TransferMappingRules` Dictionary we have used
      // above.)

      static string DailyComment(DateTime day)
      {
         Predicate<DateTime> always = d => true;
         Predicate<DateTime> weekend = d => d.DayOfWeek == DayOfWeek.Saturday
                                         || d.DayOfWeek == DayOfWeek.Sunday;
         Predicate<DateTime> friday = d => d.DayOfWeek == DayOfWeek.Friday;
         Predicate<DateTime> nearEOY = d => d.Month == 12 && d.Day > 25;

         var @switch = Switch<DateTime, string>()
             .Case(always, d => d.DayOfWeek.ToString() + ":")
             .Case(weekend, d => "chilling...")
             .Case(friday, d => "thank God it's the weekend!")
             .Case(nearEOY, d => "getting ready for a party!");

         IEnumerable<string> comments = @switch.MatchAll(day);
         return string.Join(" ", comments.ToArray());
      }

      // note that here I use Tuple instead of KeyValuePair, but that's a minor detail
      static IEnumerable<ValueTuple<Predicate<T>, Func<T, R>>> Switch<T, R>()
         => Enumerable.Empty<ValueTuple<Predicate<T>, Func<T, R>>>();
      
      static IEnumerable<ValueTuple<Predicate<T>, Func<T, R>>> Case<T, R>(
         this IEnumerable<ValueTuple<Predicate<T>, Func<T, R>>> rules
         , Predicate<T> pred, Func<T, R> func)
            => rules.Append((pred, func));

      static IEnumerable<R> MatchAll<T, R>(this IEnumerable<ValueTuple<Predicate<T>, Func<T, R>>> rules, T t)
            => rules.Where(rule => rule.Item1(t))
               .Map(rule => rule.Item2(t));
      
      // 4. Write tests to ensure it works as expected.

      [TestCase("24 Jun 2016", ExpectedResult = "Friday: thank God it's the weekend!")]
      [TestCase("27 Dec 2016", ExpectedResult = "Tuesday: getting ready for a party!")]
      public static string TestDateComment(string date)
         => DailyComment(DateTime.Parse(date));

      // 5. Exhance `Switch<T, R>` so that it can also be used like the `switch`
      // statement in the C# language, by adding an overload taking a constant
      // value to match against `Case(T value, Func<T, R> func)`, and a `Match`
      // method that returns the result of invoking the function of the _first_
      // matching case.

      static IEnumerable<ValueTuple<Predicate<T>, Func<T, R>>> Case<T, R>(
         this IEnumerable<ValueTuple<Predicate<T>, Func<T, R>>> rules
         , T value, Func<T, R> func)
            => rules.Append((new Predicate<T>(v => v.Equals(value)), func));

      static R Match<T, R>(this IEnumerable<ValueTuple<Predicate<T>, Func<T, R>>> rules, T t)
         => rules.Where(rule => rule.Item1(t))
            .First().Item2(t);

      // Write a simple scenario with those methods, and unit test it

      static string NumberComment(int number)
      {
         Predicate<int> isOdd = i => i % 2 == 1;

         var @switch = Switch<int, string>()
             .Case(33, _ => "33 is my favourite number!") // match on specific value
             .Case(isOdd, i => $"{i} is a bit of an odd one...") // match on a condition
             .Case(_ => true, i => $"{i} is just a boring, even number"); // catch all

         return @switch.Match(number);
      }

      
      [TestCase(33, ExpectedResult = "33 is my favourite number!")]
      [TestCase(27, ExpectedResult = "27 is a bit of an odd one...")]
      [TestCase(-2, ExpectedResult = "-2 is just a boring, even number")]
      public static string TestNumberComment(int number) => NumberComment(number);
   }
}
