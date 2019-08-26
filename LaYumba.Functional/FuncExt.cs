using System;
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
   using static F;

   public static class FuncExt
   {
      public static Func<T> ToNullary<T>(this Func<Unit, T> f)
          => () => f(Unit());

      public static Func<T1, R> Compose<T1, T2, R>(this Func<T2, R> g, Func<T1, T2> f)
         => x => g(f(x));

      public static Func<T, bool> Negate<T>(this Func<T, bool> pred) => t => !pred(t);

      public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> func, T1 t1)
          => t2 => func(t1, t2);

      public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T1 t1)
          => (t2, t3) => func(t1, t2, t3);

      public static Func<T2, T3, T4, R> Apply<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> func, T1 t1)
          => (t2, t3, t4) => func(t1, t2, t3, t4);

      public static Func<T2, T3, T4, T5, R> Apply<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> func, T1 t1)
          => (t2, t3, t4, t5) => func(t1, t2, t3, t4, t5);

      public static Func<T2, T3, T4, T5, T6, R> Apply<T1, T2, T3, T4, T5, T6, R>(this Func<T1, T2, T3, T4, T5, T6, R> func, T1 t1)
          => (t2, t3, t4, t5, t6) => func(t1, t2, t3, t4, t5, t6);

      public static Func<T2, T3, T4, T5, T6, T7, R> Apply<T1, T2, T3, T4, T5, T6, T7, R>(this Func<T1, T2, T3, T4, T5, T6, T7, R> func, T1 t1)
          => (t2, t3, t4, t5, t6, t7) => func(t1, t2, t3, t4, t5, t6, t7);

      public static Func<T2, T3, T4, T5, T6, T7, T8, R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, T1 t1)
          => (t2, t3, t4, t5, t6, t7, t8) => func(t1, t2, t3, t4, t5, t6, t7, t8);

      public static Func<T2, T3, T4, T5, T6, T7, T8, T9, R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func, T1 t1)
          => (t2, t3, t4, t5, t6, t7, t8, t9) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9);

      public static Func<I1, I2, R> Map<I1, I2, T, R>(this Func<I1, I2, T> @this, Func<T, R> func)
         => (i1, i2) => func(@this(i1, i2));
   }


   // Env -> T (aka. Reader)
   public static class FuncTRExt
   {
      public static Func<Env, R> Map<Env, T, R>
         (this Func<Env, T> f, Func<T, R> g)
         => x => g(f(x));

      public static Func<Env, R> Bind<Env, T, R>
         (this Func<Env, T> f, Func<T, Func<Env, R>> g)
         => env => g(f(env))(env);

      // same as above, in uncurried form
      public static Func<Env, R> Bind<Env, T, R>
         (this Func<Env, T> f, Func<T, Env, R> g)
         => env => g(f(env), env);


      // LINQ

      public static Func<Env, R> Select<Env, T, R>(this Func<Env, T> f, Func<T, R> g) => f.Map(g);

      public static Func<Env, P> SelectMany<Env, T, R, P>
         (this Func<Env, T> f, Func<T, Func<Env, R>> bind, Func<T, R, P> project)
         => env =>
         {
            var t = f(env);
            var r = bind(t)(env);
            return project(t, r);
         };
   }
}
