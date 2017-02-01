using LaYumba.Functional;
using System;
using Unit = System.ValueTuple;

namespace Examples.Chapter5
{
   class CookFavouriteDish
   {
      Func<Either<Reason, Unit>> WakeUpEarly;
      Func<Unit, Either<Reason, Ingredients>> ShopForIngredients;
      Func<Ingredients, Either<Reason, Food>> CookRecipe;

      Action<Food> EnjoyTogether;
      Action<Reason> ComplainAbout;
      Action OrderPizza;

      void Start()
      {
         WakeUpEarly()
            .Bind(ShopForIngredients)
            .Bind(CookRecipe)
            .Match(
               Right: dish => EnjoyTogether(dish),
               Left: reason =>
               {
                  ComplainAbout(reason);
                  OrderPizza();
               });
      }
   }

   class Reason { }
   class Ingredients { }
   class Food { }
}