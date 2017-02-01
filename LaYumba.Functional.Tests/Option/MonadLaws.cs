using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using Xunit;
using LaYumba.Functional;

namespace LaYumba.Functional.Tests
{
   using static F;
   
   public class Option_MonadLaws
   {
      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      void RightIdentityHolds(Option<object> m) => Assert.Equal(
         m,
         m.Bind(Some)
      );

      [Property]
      public void LeftIdentityHolds(int x)
      {
         // given a world crossing function f...
         Func<int, Option<int>> f = i 
            => i % 2 == 0 ? Some(i) : None;

         // then applying f to a value x
         // is the same as lifting x and binding f to it
         Assert.Equal(Some(x).Bind(f), f(x));
      }

      [Property] // only works for non-null, given my implementation of Some
      public void LeftIdentityHoldsRefValues(NonNull<string> x)
      {
         // given a world crossing function f...
         Func<string, Option<string>> f = s => Some($"{s} World");

         // then applying f to a value x
         // is the same as lifting x and binding f to it
         Assert.Equal(Some(x.Get).Bind(f), f(x.Get));
      }

      // 3. Associativity

      // (m `bind` f) `bind` g
      //  m `bind` (f `bind` g) -- not correct, but if we expand f we get:
      //  m `bind` (x => f(x) `bind` g)

      Func<double, Option<double>> safeSqrt = d
         => d < 0 ? None : Some(Math.Sqrt(d));

      [Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
      void AssociativityHolds(Option<string> m) => Assert.Equal(
         m.Bind(Double.Parse).Bind(safeSqrt), 
         m.Bind(x => Double.Parse(x).Bind(safeSqrt))
      );
   }
}
