using FsCheck;
using FsCheck.Xunit;
using System;
using static LaYumba.Functional.F;
using Xunit;

namespace LaYumba.Functional.Tests
{
   // teach FsCheck to create an arbitrary Option
   static class ArbitraryOption
   {
      public static Arbitrary<Option<T>> Option<T>()
      {
         var gen = from isSome in Arb.Generate<bool>()
                   from val in Arb.Generate<T>()
                   select isSome && val != null ? Some(val) : None;
         return gen.ToArbitrary();
      }
   }

   // illustrates that the applicative laws hold for Option
   // see https://hackage.haskell.org/package/base-4.9.0.0/docs/Control-Applicative.html
   public static class Option_ApplicativeLaws
   {
      // the identity function is still the identity function, in the applicative world
      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      static void IdentityHolds(Option<int> v)
      {
         Func<int, int> id = x => x;
         Assert.Equal(Some(id).Apply(v), v);
      }

      static Func<int, int> minus10 = x => x - 10;
      static Func<int, int> times2 = x => x * 2;

      // composition still works in the applicative world
      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      static void CompositionHolds(Option<int> w, bool uIsSome, bool vIsSome)
      {
         Func<Func<int, int>, Func<int, int>, Func<int, int>> compose
            = (f, g) => x => f(g(x));
         var u = uIsSome ? Some(minus10) : None;
         var v = vIsSome ? Some(times2) : None;

         Assert.Equal(
            Some(compose)
               .Apply(u)
               .Apply(v)
               .Apply(w), 
            u.Apply( v.Apply(w) )
        );
      }

      [Property] static void HomomorphismHolds(int x)
         => Assert.Equal(
               Some(minus10).Apply(Some(x)), 
               Some(minus10(x))
            );

      [Property] static void InterchangeHolds(bool uIsSome, int y)
      {
         var u = uIsSome ? Some(minus10) : None;
         Func<Func<int, int>, int> applyY = f => f(y);

         Assert.Equal(
            u.Apply(Some(y)), 
            Some(applyY).Apply(u)
         );
      }

      static Func<int, int, int> multiply = (i, j) => i * j;

      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      static void _ApplicativeLawHolds(Option<int> a, Option<int> b) 
         => Assert.Equal(
               Some(multiply).Apply(a).Apply(b),
               a.Map(multiply).Apply(b)
            );

      [Property] static void ApplicativeLawHolds(int a, int b)
      {
         var first = Some(multiply)
             .Apply(Some(a))
             .Apply(Some(b));

         var second = Some(a)
             .Map(multiply)
             .Apply(Some(b));

         Assert.Equal(first, second);
      }

      public static Option<R> ApplyInTermsOfBind<T, R>
         (this Option<Func<T, R>> func, Option<T> arg)
         => arg.Bind(t => func.Bind<Func<T, R>, R>(f => f(t)));

      [Property] static void ApplicativeLawHolds_WhenApplyIsDefinedInTermsOfBind(int a, int b)
      {
         Func<int, int, int> multiply = (i, j) => i * j;

         var first = Some(multiply)
             .Apply(Some(a))
             .ApplyInTermsOfBind(Some(b));

         var second = Some(a)
             .Map(multiply)
             .ApplyInTermsOfBind(Some(b));

         Assert.Equal(first, second);
      }
   }
}
