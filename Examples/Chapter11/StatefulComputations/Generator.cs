using System;
using LaYumba.Functional;
using System.Collections.Generic;
using System.Linq;

namespace Examples.RandomState
{
   using System.Text;
   using static F;

   public delegate (T Value, int Seed) Generator<T>(int seed);

   public static class Gen
   {
      public static Generator<int> NextInt = (seed) =>
      {
         seed ^= seed >> 13;
         seed ^= seed << 18;
         int result = seed & 0x7fffffff;
         return (result, result);
      };

      static Generator<(int, int)> _PairOfInts = (seed0) =>
      {
         var (a, seed1) = NextInt(seed0);
         var (b, seed2) = NextInt(seed1);
         return ((a, b), seed2);
      };

      public static Generator<(int, int)> PairOfInts =>
         from a in NextInt
         from b in NextInt
         select (a, b);

      public static Generator<Option<int>> OptionInt =>
         from some in NextBool
         from i in NextInt
         select some ? Some(i) : None;

      public static Generator<bool> NextBool
         => from i in NextInt select i % 2 == 0;

      public static Generator<char> NextChar
         => from i in NextInt select (char)(i % (char.MaxValue + 1));

      public static Generator<IEnumerable<int>> IntList
         => from empty in NextBool
            from list in empty ? Empty : NonEmpty
            select list;

      public static Generator<IEnumerable<int>> Empty
         => Gen.Of(Enumerable.Empty<int>());

      public static Generator<IEnumerable<int>> NonEmpty
         => from head in NextInt
            from tail in IntList
            select List(head).Concat(tail);

      public static Generator<string> NextString
         => from ints in IntList
            select IntsToString(ints);

      static char IntToChar(int i) => (char)(i % (char.MaxValue + 1));

      static string IntsToString(this IEnumerable<int> ints)
      {
         var sb = new StringBuilder();
         foreach (var c in ints) sb.Append(IntToChar(c));
         return sb.ToString();
      }

      // helpers

      static Generator<T> Of<T>(T value)
         => seed => (value, seed);

      static Generator<int> Between(int low, int high)
         => from i in NextInt select low + i % (high - low);

      static Generator<T> OneOf<T>(params T[] values)
         => from i in Between(0, values.Length) select values[i];
   }

   public static class GenExt
   {
      public static T Run<T>(this Generator<T> gen, int seed)
         => gen(seed).Value;

      public static T Run<T>(this Generator<T> gen)
         => gen(Environment.TickCount).Value;

      // LINQ

      public static Generator<R> Select<T, R>
         (this Generator<T> gen, Func<T, R> f)
         => seed0 =>
         {
            var (t, seed1) = gen(seed0);
            return (f(t), seed1);
         };

      public static Generator<R> SelectMany<T, R>
         (this Generator<T> gen, Func<T, Generator<R>> f)
         => seed0 =>
         {
            var (t, seed1) = gen(seed0);
            return f(t)(seed1);
         };

      public static Generator<RR> SelectMany<T, R, RR>
         (this Generator<T> gen
         , Func<T, Generator<R>> bind
         , Func<T, R, RR> project)
         => seed0 =>
         {
            var (t, seed1) = gen(seed0);
            var (r, seed2) = bind(t)(seed1);
            var rr = project(t, r);
            return (rr, seed2);
         };
   }
}
