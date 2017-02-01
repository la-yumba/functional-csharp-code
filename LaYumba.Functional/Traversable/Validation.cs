using System;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   public static class ValidationTraversable
   {
      // Exceptional
      public static Exceptional<Validation<R>> Traverse<T, R>
         (this Validation<T> tr, Func<T, Exceptional<R>> f)
         => tr.Match(
               Invalid: reasons => Exceptional(Invalid<R>(reasons)),
               Valid: t => f(t).Map(Valid)
            );

      // Task
      public static Task<Validation<R>> Traverse<T, R>
         (this Validation<T> @this, Func<T, Task<R>> func)
         => @this.Match(
               Invalid: reasons => Async(Invalid<R>(reasons)),
               Valid: t => func(t).Map(Valid)
            );

      public static Task<Validation<R>> TraverseBind<T, R>(this Validation<T> @this
         , Func<T, Task<Validation<R>>> func)
         => @this.Match(
               Invalid: reasons => Async(Invalid<R>(reasons)),
               Valid: t => func(t)
            );
   }

   public static class TaskTraversable
   {
      public static Validation<Task<R>> Traverse<T, R>(this Task<T> @this
         , Func<T, Validation<R>> func)
      { throw new NotImplementedException(); }
   }

   public static class ExceptionalTraversable
   {
      public static Validation<Exceptional<R>> Traverse<T, R>
         (this Exceptional<T> tr, Func<T, Validation<R>> f)
         => tr.Match(
            Exception: e => Valid((Exceptional<R>)e),
            Success: t => from r in f(t) select Exceptional(r));
   }
}
