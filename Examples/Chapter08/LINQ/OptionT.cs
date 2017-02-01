using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Chapter9.LINQ
{
   using static F;
   using static Console;

   public class Example
   {
      static IEnumerable<Option<int>> GetOptions()
         => Range(1, 5).Map(i => i % 2 == 0 ? None : Some(i));

      static int Double(int i) => i * 2;

      static IEnumerable<T> YieldTwice<T>(T i)
      {
         yield return i;
         yield return i;
      }

      static Option<int> OtherThan5(int i) => i == 5 ? None : Some(i);

      public static void _main()
      {
         // generate a list of options
         var result = GetOptions()
            .Map(o => o.Map(Double)); // unwrap the option

         result = from i in GetOptions()
                  from j in OtherThan5(i)
                  from k in YieldTwice(j)
                  select Double(j);

         WriteLine(string.Join(", ", result.Map(o => o.ToString()).ToArray()));
      }
   }

   public static partial class EnumerableT
   {
      // make IEnumerable<Option<T>> a functor over T
      public static IEnumerable<Option<U>> Select<T, U>
         (this IEnumerable<Option<T>> self, Func<T, U> mapper)
         => self.Map(x => x.Map(mapper));

      public static IEnumerable<Option<V>> SelectMany<T, U, V>
         (this IEnumerable<Option<T>> self
         , Func<T, Option<U>> bind
         , Func<T, U, V> project)
         => self.Map(wt => wt.Bind(t => bind(t).Map(u => project(t, u))));

      public static IEnumerable<Option<V>> SelectMany<T, U, V>
         (this IEnumerable<Option<T>> self
         , Func<T, IEnumerable<U>> bind
         , Func<T, U, V> project)
         => self.Bind(wt => wt.Traverse(t => bind(t).Map(u => project(t, u))));

      public static IEnumerable<Option<U>> SelectMany<T, U>
         (this IEnumerable<Option<T>> self
         , Func<T, IEnumerable<U>> bind)
         => self.Bind(vt => vt.Traverse(t => bind(t)));
      

      //internal static Option<V> SelectMany<T, U, V>
      //   (this Option<T> self
      //   , Func<T, U> bind
      //   , Func<T, U, V> project)
      //   => self.Map(t => project(t, bind(t)));
      
      //public static IEnumerable<Option<V>> SelectMany<T, U, V>
      //   (this IEnumerable<T> self
      //   , Func<T, Option<U>> bind
      //   , Func<T, U, V> project)
      //   => self.Map(t => bind(t).Map(u => project(t, u)));

      //public static IEnumerable<Option<V>> SelectMany<T, U, V>
      //   (this IEnumerable<Option<T>> self
      //   , Func<T, U> bind
      //   , Func<T, U, V> project)
      //   => self.Map(x => x.SelectMany(bind, project));
   }
}
