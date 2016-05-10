using System;

namespace LaYumba.Functional
{
   public static class FuncExt
   {
      public static Func<T1, R> Compose<T1, T2, R>(this Func<T2, R> g, Func<T1, T2> f)
         => x => g(f(x));

      public static Func<I1, I2, R> Map<I1, I2, T, R>(this Func<I1, I2, T> @this, Func<T, R> func)
         => (i1, i2) => func(@this(i1, i2));
   }

   // () -> T (aka. Identity)
   public static class FuncTExt
   {
      public static Func<R> Map<T, R>(this Func<T> @this
         , Func<T, R> func) => () => func(@this());

      public static Func<R> Bind<T, R>(this Func<T> @this
         , Func<T, Func<R>> func) => () => func(@this())();

      // LINQ

      public static Func<R> Select<T, R>(this Func<T> @this
         , Func<T, R> func) => @this.Map(func);

      public static Func<P> SelectMany<T, R, P>(this Func<T> @this
         , Func<T, Func<R>> bind, Func<T, R, P> project)
         => () =>
         {
            var t = @this();
            var r = bind(t)();
            return project(t, r);
         };
   }

   // Env -> T (aka. Reader)
   public static class FuncTRExt
   {
      public static Func<Env, R> Map<Env, T, R>(this Func<Env, T> @this, Func<T, R> func)
         => x => func(@this(x));

      public static Func<Env, R> Bind<Env, T, R>(this Func<Env, T> @this, Func<T, Func<Env, R>> func)
         => env => func(@this(env))(env);

      public static Func<Env, R> Bind<Env, T, R>(this Func<Env, T> @this, Func<T, Env, R> func)
         => env => func(@this(env), env);


      // LINQ

      public static Func<Env, R> Select<Env, T, R>(this Func<Env, T> @this
         , Func<T, R> func) => @this.Map(func);

      public static Func<Env, P> SelectMany<Env, T, R, P>(this Func<Env, T> @this
         , Func<T, Func<Env, R>> bind, Func<T, R, P> project)
         => env =>
         {
            var t = @this(env);
            var r = bind(t)(env);
            return project(t, r);
         };
   }
}
