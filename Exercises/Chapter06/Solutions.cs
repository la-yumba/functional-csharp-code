using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using Examples.Chapter3;

namespace Exercises.Chapter6.Solutions
{
   static class Exercises
   {
      // 1. Write a `ToOption` extension method to convert an `Either` into an
      // `Option`. Then write a `ToEither` method to convert an `Option` into an
      // `Either`, with a suitable parameter that can be invoked to obtain the
      // appropriate `Left` value, if the `Option` is `None`.

      // ToOption : Either<L, R> -> Option<R>
      static Option<R> ToOption<L, R>(this Either<L, R> @this)
         => @this.Match(l => None, r => Some(r));

      // ToEither : (Option<R>, Func<L>) -> Either<L, R>
      static Either<L, R> ToEither<L, R>(this Option<R> @this, Func<L> left)
         => @this.Match<Either<L, R>>(
            None: () => left(), 
            Some: r => r);

      // 2. Take a workflow where 2 or more functions that return an `Option`
      // are chained using `Bind`. 

      // example taken from chapter 3
      static Func<string, Option<Age>> parseAge = s
         => Int.Parse(s).Bind(Age.Of);

      // Then change the first one of the functions to return an `Either`.

      static Either<string, int> ParseIntVerbose(this string s)
         => Int.Parse(s).ToEither(() => $"'{s}' is not a valid representation of an int");

      // This should cause compilation to fail. Since `Either` can be
      // converted into an `Option` as we have done in the previous exercise,
      // write extension overloads for `Bind`, so that
      // functions returning `Either` and `Option` can be chained with `Bind`,
      // yielding an `Option`.

      public static Option<RR> Bind<L, R, RR>(this Either<L, R> @this, Func<R, Option<RR>> func)
          => @this.Match(
             Left: _ => None,
             Right: r => func(r));

      public static Option<RR> Bind<L, R, RR>(this Option<R> @this, Func<R, Either<L, RR>> func)
          => @this.Match(
             None: () => None,
             Some: v => func(v).ToOption());

      static Func<string, Option<Age>> parseAge2 = s
         => s.ParseIntVerbose().Bind(Age.Of);


      // 3. Write a function `Safely` of type ((() → R), (Exception → L)) → Either<L, R> that will
      // run the given function in a `try/catch`, returning an appropriately
      // populated `Either`.

      // Safely : ((() → R), (Exception → L)) → Either<L, R>
      static Either<L, R> Safely<L, R>(Func<R> func, Func<Exception, L> left)
      {
         try { return func(); }
         catch(Exception ex) { return left(ex); }
      }

      // 4. Write a function `Try` of type (() → T) → Exceptional<T> that will
      // run the given function in a `try/catch`, returning an appropriately
      // populated `Exceptional`.

      // Try : (() → T) → Exceptional<T>
      static Exceptional<T> Try<T>(Func<T> func)
      {
         try { return func(); }
         catch (Exception ex) { return ex; }
      }
   }
}
