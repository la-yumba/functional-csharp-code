using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using Xunit;

namespace LaYumba.Functional.Tests
{
   using static F;

   public class IEnumerable_MonadLaws
   {
      [Property]
      public void RightIdentityHolds(string s)
      {
         Func<string, IEnumerable<string>> Return = i => List(i);

         var a = Return(s);
         var b = a.Bind(Return);
         Assert.Equal(a, b);
      }

      [Property]void LeftIdentityHolds(int x)
      {
         // given a world crossing function f...
         Func<int, IEnumerable<int>> f = i => Range(0, i);

         // then applying f to a value x
         // is the same as lifting x and binding f to it
         Assert.Equal(List(x).Bind(f), f(x));
      }

      [Property]
      public void LeftIdentityHoldsRefType(NonNull<string> nonNull)
      {
         var x = nonNull.Get;

         // given a world crossing function f...
         Func<string, IEnumerable<char>> f = s => s.ToUpper();

         // then applying f to a value x
         // is the same as lifting x and binding f to it
         Assert.Equal(
            List(x).Bind(f), 
            f(x)
         );
      }

      [Property]
      public void AssociativityHolds(NonNull<string> input)
      {
         Func<string, IEnumerable<char>> f = s => s;
         Func<char, IEnumerable<char>> g = c => List(c, c, c);

         // (m `bind` f) `bind` g ==
         //  m `bind` (f `bind` g) -- not correct, but if we expand f we get:
         //  m `bind` (x => f(x) `bind` g)

         Assert.Equal
            ( List(input.Get).Bind(f).Bind(g)
            , List(input.Get).Bind(x => f(x).Bind(g))
         );
      }
   }
}
