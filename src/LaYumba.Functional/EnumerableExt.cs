using System;
using System.Collections.Generic;
using System.Linq;

namespace LaYumba.Functional
{
   using static F;

   public static class EnumerableExt
   {
      public static IEnumerable<T> Append<T>(this IEnumerable<T> source
         , params T[] ts) => source.Concat(ts);

      public static Option<T> Find<T>(this IEnumerable<T> source, Func<T, bool> predicate)
         => source.FirstOrDefault(predicate);

      public static Unit ForEach<T>(this IEnumerable<T> source, Action<T> action)
      {
         foreach (var t in source) action(t);
         return Unit();
      }

      public static IEnumerable<R> Map<T, R>(this IEnumerable<T> list, Func<T, R> func)
          => list.Select(func);

      public static R Match<T, R>(this IEnumerable<T> list
         , Func<R> Empty, Func<T, IEnumerable<T>, R> Otherwise) 
         => list.SafeHead().Match(
            None: Empty,
            Some: head => Otherwise(head, list.Skip(1)));

      public static Option<T> SafeHead<T>(this IEnumerable<T> list)
      {
         if (list == null) return None;
         var enumerator = list.GetEnumerator();
         return enumerator.MoveNext() ? Some(enumerator.Current) : None;
      }

      //public static IEnumerable<R> Map<T, R>(this IEnumerable<T> list
      //   , Func<T, R> func)
      //{
      //   foreach (var item in list) yield return func(item);
      //}

      //public static IEnumerable<T> Where<T>(this IEnumerable<T> list
      //   , Func<T, bool> predicate)
      //{
      //   foreach (var item in list) if (predicate(item)) yield return item;
      //}

      public static IEnumerable<Func<T2, R>> Map<T1, T2, R>(this IEnumerable<T1> list
         , Func<T1, T2, R> func)
         => list.Map(func.Curry());

      public static IEnumerable<Func<T2, Func<T3, R>>> 
      Map<T1, T2, T3, R>(this IEnumerable<T1> opt, Func<T1, T2, T3, R> func)
         => opt.Map(func.Curry());

      public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, IEnumerable<R>> func)
          => list.SelectMany(func);

      public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func)
          => list.Bind(t => func(t).AsEnumerable());

      // LINQ

      public static IEnumerable<RR> SelectMany<T, R, RR>
         (this IEnumerable<T> source
         , Func<T, Option<R>> bind
         , Func<T, R, RR> project)
         => from t in source
            let opt = bind(t)
            where opt.IsSome
            select project(t, opt.Value);

   }
}