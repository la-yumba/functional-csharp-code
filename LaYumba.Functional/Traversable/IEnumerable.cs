using System;
using System.Collections.Generic;
using System.Linq;

namespace LaYumba.Functional
{
   using System.Threading.Tasks;
   using static F;

   public static class IEnumerableTraversable
   {
      static Func<IEnumerable<T>, T, IEnumerable<T>> Append<T>()
         => (ts, t) => ts.Append(t);

      // Exceptional

      public static Exceptional<IEnumerable<R>> Traverse<T, R>(this IEnumerable<T> list
         , Func<T, Exceptional<R>> func)
         => list.Aggregate(
            seed: Exceptional.Of(Enumerable.Empty<R>()),
            // Exceptional<[R]> -> T -> Exceptional<[R]>
            func: (optRs, t) => from rs in optRs
                                from r in func(t)
                                select rs.Append(r));

      // Option

      // applicative traverse, recursive (reference only)
      static Option<IEnumerable<R>> TraverseARec<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> func)
         => list.Match(
            Empty: () => Some(Enumerable.Empty<R>()),
            Otherwise: (t, ts) => Some(Cons<R>())
               .Apply(func(t))
               .Apply(ts.TraverseARec(func)));

      // applicative traverse
      public static Option<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> f)
         => list.Aggregate(
            seed: Some(Enumerable.Empty<R>()),
            func: (optRs, t) => Some(Append<R>())
                                   .Apply(optRs)
                                   .Apply(f(t)));

      // monadic traverse, recursive (reference only)
      static Option<IEnumerable<R>> TraverseMRec<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> func)
         => list.Match(
            Empty: () => Some(Enumerable.Empty<R>()),
            Otherwise: (t, ts) => from r in func(t)
                                  from rs in TraverseMRec(ts, func)
                                  select List(r).Concat(rs));

      // monadic traverse
      public static Option<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> ts, Func<T, Option<R>> func) => Traverse(ts, func);

      public static Option<IEnumerable<R>> Traverse<T, R>
         (this IEnumerable<T> ts, Func<T, Option<R>> func)
         => ts.Aggregate(
            seed: Some(Enumerable.Empty<R>()),
            // Option<[R]> -> T -> Option<[R]>
            func: (optRs, t) => from rs in optRs
                                from r in func(t)
                                select rs.Append(r));


      // Validation

      // monadic traverse

      public static Validation<IEnumerable<R>> TraverseM<T, R>
         (this IEnumerable<T> ts, Func<T, Validation<R>> func)
         => ts.Aggregate(
            seed: Valid(Enumerable.Empty<R>()),
            // Validation<[R]> -> T -> Validation<[R]>
            func: (optRs, t) => from rs in optRs
                                from r in func(t)
                                select rs.Append(r));

      // applicative traverse
      public static Validation<IEnumerable<R>> Traverse<T, R>
         (this IEnumerable<T> ts, Func<T, Validation<R>> f) 
         => TraverseA(ts, f);

      public static Validation<IEnumerable<R>> TraverseA<T, R>
         (this IEnumerable<T> ts, Func<T, Validation<R>> f)
         => ts.Aggregate(
            seed: Valid(Enumerable.Empty<R>()),
            func: (valRs, t) => Valid(Append<R>())
                                   .Apply(valRs)
                                   .Apply(f(t)));

      // Task
      
      // applicative traverse
      public static Task<IEnumerable<R>> TraverseA<T, R>
         (this IEnumerable<T> ts, Func<T, Task<R>> f)
         => ts.Aggregate(
            seed: Task.FromResult(Enumerable.Empty<R>()),
            func: (rs, t) => Task.FromResult(Append<R>())
                                   .Apply(rs)
                                   .Apply(f(t)));

      // by default use applicative traverse (parallel, hence faster)
      public static Task<IEnumerable<R>> Traverse<T, R>(this IEnumerable<T> list, Func<T, Task<R>> func) => TraverseA(list, func);

      // monadic traverse
      public static Task<IEnumerable<R>> TraverseM<T, R>
         (this IEnumerable<T> ts, Func<T, Task<R>> func)
         => ts.Aggregate(
            seed: Task.FromResult(Enumerable.Empty<R>()),
            // Task<[R]> -> T -> Task<[R]>
            func: (taskRs, t) => from rs in taskRs
                                 from r in func(t)
                                 select rs.Append(r));
   }
}