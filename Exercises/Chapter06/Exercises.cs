using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;

namespace Exercises.Chapter6
{
   static class Exercises
   {
      // 1. Write a `ToOption` extension method to convert an `Either` into an
      // `Option`. Then write a `ToEither` method to convert an `Option` into an
      // `Either`, with a suitable parameter that can be invoked to obtain the
      // appropriate `Left` value, if the `Option` is `None`. (Tip: start by writing
      // the function signatures in arrow notation)
      
      // 2. Take a workflow where 2 or more functions that return an `Option`
      // are chained using `Bind`.

      // Then change the first one of the functions to return an `Either`.

      // This should cause compilation to fail. Since `Either` can be
      // converted into an `Option` as we have done in the previous exercise,
      // write extension overloads for `Bind`, so that
      // functions returning `Either` and `Option` can be chained with `Bind`,
      // yielding an `Option`.
      

      // 3. Write a function `Safely` of type ((() → R), (Exception → L)) → Either<L, R> that will
      // run the given function in a `try/catch`, returning an appropriately
      // populated `Either`.

      // 4. Write a function `Try` of type (() → T) → Exceptional<T> that will
      // run the given function in a `try/catch`, returning an appropriately
      // populated `Exceptional`.
   }
}
