using System;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   public static class OptionTraversable
   {
      // Exceptional
      public static Exceptional<Option<R>> Traverse<T, R>
         (this Option<T> tr, Func<T, Exceptional<R>> f)
         => tr.Match(
               None: () => Exceptional((Option<R>)None),
               Some: t => f(t).Map(Some)
            );

      // Task
      public static Task<Option<R>> Traverse<T, R>
         (this Option<T> @this, Func<T, Task<R>> func)
         => @this.Match(
               None: () => Async((Option<R>)None),
               Some: t => func(t).Map(Some)
            );

      public static Task<Option<R>> TraverseBind<T, R>(this Option<T> @this
         , Func<T, Task<Option<R>>> func)
         => @this.Match(
               None: () => Async((Option<R>)None),
               Some: t => func(t)
            );
   }
}
