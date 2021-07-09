using System;
using LaYumba.Functional;

using Unit = System.ValueTuple;

namespace Exercises.Chapter10
{
   public static class Exercises
   {
      // 1. Implement `Apply` for `Either` and `Exceptional`.

      // see LaYumba.Functional/Either.cs and LaYumba.Functional/Exceptional.cs

      // 2. Implement the query pattern for `Either` and `Exceptional`. Try to
      // write down the signatures for `Select` and `SelectMany` without
      // looking at any examples. For the implementation, just follow the
      // types--if it type checks, it’s probably right!

      // see LaYumba.Functional/Either.cs and LaYumba.Functional/Exceptional.cs

      // 3. Come up with a scenario in which various `Either`-returning
      // operations are chained with `Bind`. (If you’re short of ideas, you can
      // use the favorite-dish example from Examples/Chapter08/CookFavouriteFood.)
      // Rewrite the code using a LINQ expression.
   }
}
