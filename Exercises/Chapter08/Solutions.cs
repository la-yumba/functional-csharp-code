using System;
using LaYumba.Functional;

using Unit = System.ValueTuple;
using Reason = System.String;

using static System.Console;

namespace Exercises.Chapter10.Solutions
{
   public static class Solutions
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
      // use the favorite-dish example from <<ch_error>>.) Rewrite the code using a
      // LINQ expression.

      static Either<Reason, Food> PrepareFavoriteDish
         => from _ in WakeUpEarly()
            from ingredients in ShopForIngredients()
            from dish in CookRecipe(ingredients)
            select dish;

      static void ConsumeFavoriteDish()
         => PrepareFavoriteDish.Match
         (
            Left: ComplainAbout,
            Right: Enjoy
         );

      static Either<Reason, Unit> WakeUpEarly() => throw new NotImplementedException();
      static Either<Reason, Ingredients> ShopForIngredients() => throw new NotImplementedException();
      static Either<Reason, Food> CookRecipe(Ingredients ingredients) => throw new NotImplementedException();

      static void Enjoy(Food food) => WriteLine("Mmmm... yummy!");
      static void ComplainAbout(Reason reason) => WriteLine($"Oh boy, {reason}");

      class Ingredients { }
      class Food { }
   }
}
