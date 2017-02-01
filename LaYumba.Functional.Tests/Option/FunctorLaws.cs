using FsCheck.Xunit;
using System;
using Xunit;

namespace LaYumba.Functional.Tests
{
   using static F;

   public static class Option_FunctorLaws
   {
      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      static void FirstFunctorLawHolds(Option<object> a)
         => Assert.Equal(a, a.Map(x => x));
      
      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      static void SecondFunctorLawHolds(Option<int> val)
      {
         Func<int, int> f = i => i - 2;
         Func<int, int> g = i => i * 50;
         // h = f `andThen` g
         Func<int, int> h = i => g(f(i));

         Assert.Equal(
            val.Map(f).Map(g),
            val.Map(h)
         );
      }

      public static Option<R> MapInTermsOfApply<T, R>
         (this Option<T> @this, Func<T, R> func)
         => Some(func).Apply(@this);

      [Property]
      static void SecondFunctorLawHolds_WhenMapIsDefinedInTermsOfApply(int input)
      {
         Func<int, int> f = i => i - 2;
         Func<int, int> g = i => i * 50;
         // h = f `andThen` g
         Func<int, int> h = i => g(f(i));

         Assert.Equal(
            Some(input).MapInTermsOfApply(f).MapInTermsOfApply(g),
            Some(input).MapInTermsOfApply(h)
         );
      }
   }
}
