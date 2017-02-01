using System;

namespace LaYumba.Functional
{
   public delegate T Reader<in Env, out T>(Env env);

   public static class Reader
   {
      // standard monad methods

      public static Reader<Env, R> Map<Env, T, R>
         (this Reader<Env, T> f, Func<T, R> g) 
         => x => g(f(x));

      public static Reader<Env, R> Bind<Env, T, R>
         (this Reader<Env, T> f, Func<T, Reader<Env, R>> g)
         => env => g(f(env))(env);

      public static Reader<Env, R> Bind<Env, T, R>
         (this Reader<Env, T> f, Func<T, Env, R> g)
         => env => g(f(env), env);

      // Reader specific

      public static Reader<Env, Env> Ask<Env>() => env => env;

      // LINQ

      public static Reader<Env, R> Select<Env, T, R>(this Reader<Env, T> @this
         , Func<T, R> func) => @this.Map(func);

      public static Reader<Env, RR> SelectMany<Env, T, R, RR>(this Reader<Env, T> @this
         , Func<T, Reader<Env, R>> bind, Func<T, R, RR> project) 
         => env =>
         {
            var t = @this(env);
            var r = bind(t)(env);
            return project(t, r);
         };
   }
}
