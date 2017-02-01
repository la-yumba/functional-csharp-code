using System;
using static LaYumba.Functional.F;

namespace LaYumba.Functional
{
   public delegate Exceptional<T> Try<T>();

   public static partial class F
   {
      public static Try<T> Try<T>(Func<T> f) => () => f();
   }

   public static class TryExt
   {
      public static Exceptional<T> Run<T>(this Try<T> @try)
      {
         try { return @try(); }
         catch (Exception ex) { return ex; }
      }
      
      public static Try<R> Map<T, R>
         (this Try<T> @try, Func<T, R> f)
         => () 
         => @try.Run()
               .Match<Exceptional<R>>(
                  ex => ex,
                  t => f(t));

      public static Try<Func<T2, R>> Map<T1, T2, R>
         (this Try<T1> @try, Func<T1, T2, R> func) 
         => @try.Map(func.Curry());

      public static Try<R> Bind<T, R>
         (this Try<T> @try, Func<T, Try<R>> f)
         => ()
         => @try.Run().Match(
               Exception: ex => ex,
               Success: t => f(t).Run());
      
      // LINQ

      public static Try<R> Select<T, R>(this Try<T> @this, Func<T, R> func) => @this.Map(func);
      
      public static Try<RR> SelectMany<T, R, RR>
         (this Try<T> @try, Func<T, Try<R>> bind, Func<T, R, RR> project)
         => () 
         => @try.Run().Match(
               ex => ex,
               t => bind(t).Run()
                        .Match<Exceptional<RR>>(
                           ex => ex,
                           r => project(t, r))
                        );
   }
}
