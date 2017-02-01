using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
   using static F;

   public static partial class F
   {
      public static Task<T> Async<T>(T t) => Task.FromResult(t);
   }

   public static class TaskExt
   {
      public static async Task<R> Apply<T, R>
         (this Task<Func<T, R>> f, Task<T> arg)
         //=> (await f)(await arg);
         => (await f.ConfigureAwait(false))(await arg.ConfigureAwait(false));

      public static Task<Func<T2, R>> Apply<T1, T2, R>
         (this Task<Func<T1, T2, R>> f, Task<T1> arg)
         => Apply(f.Map(F.Curry), arg);

      public static Task<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Task<Func<T1, T2, T3, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);


      public static async Task<R> Map<T, R>
         (this Task<T> task, Func<T, R> f)
         //=> f(await task);
         => f(await task.ConfigureAwait(false));

      public static async Task<R> Map<R>
         (this Task task, Func<R> f)
      {
         await task;
         return f();
      }

      public static Task<Func<T2, R>> Map<T1, T2, R>
         (this Task<T1> @this, Func<T1, T2, R> func)
          => @this.Map(func.Curry());

      public static Task<Func<T2, T3, R>> Map<T1, T2, T3, R>
         (this Task<T1> @this, Func<T1, T2, T3, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<R> Map<T, R>
         (this Task<T> task, Func<Exception, R> Faulted, Func<T, R> Completed)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? Faulted(t.Exception)
                  : Completed(t.Result));

      public static Task<Unit> ForEach<T>(this Task<T> @this, Action<T> continuation)
          => @this.ContinueWith(t => continuation.ToFunc()(t.Result)
              , TaskContinuationOptions.OnlyOnRanToCompletion);

      public static async Task<R> Bind<T, R>
         (this Task<T> task, Func<T, Task<R>> f)
          //=> await f(await task);
          => await f(await task.ConfigureAwait(false)).ConfigureAwait(false);


      public static Task<T> OrElse<T>
         (this Task<T> task, Func<Task<T>> fallback)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? fallback()
                  : Task.FromResult(t.Result)
            )
            .Unwrap();


      public static Task<T> Recover<T>
         (this Task<T> task, Func<Exception, T> fallback)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? fallback(t.Exception)
                  : t.Result);

      public static Task<T> RecoverWith<T>
         (this Task<T> task, Func<Exception, Task<T>> fallback)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? fallback(t.Exception)
                  : Task.FromResult(t.Result)
         ).Unwrap();

      // LINQ

      public static async Task<RR> SelectMany<T, R, RR>
         (this Task<T> task, Func<T, Task<R>> bind, Func<T, R, RR> project)
      {
         T t = await task;
         R r = await bind(t);
         return project(t, r);
      }

      public static async Task<RR> SelectMany<R, RR>
         (this Task task, Func<Unit, Task<R>> bind, Func<Unit, R, RR> project)
      {
         await task;
         R r = await bind(Unit());
         return project(Unit(), r);
      }

      public static async Task<R> Select<T, R>(this Task<T> task, Func<T, R> f)
         => f(await task);

      public static async Task<T> Where<T>(this Task<T> source
          , Func<T, bool> predicate)
      {
         T t = await source;
         if (!predicate(t)) throw new OperationCanceledException();
         return t;
      }

      public static async Task<V> Join<T, U, K, V>(
          this Task<T> source, Task<U> inner,
          Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
          Func<T, U, V> resultSelector)
      {
         await Task.WhenAll(source, inner);
         if (!EqualityComparer<K>.Default.Equals(
             outerKeySelector(source.Result), innerKeySelector(inner.Result)))
            throw new OperationCanceledException();
         return resultSelector(source.Result, inner.Result);
      }

      public static async Task<V> GroupJoin<T, U, K, V>(
          this Task<T> source, Task<U> inner,
          Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
          Func<T, Task<U>, V> resultSelector)
      {
         T t = await source;
         return resultSelector(t,
             inner.Where(u => EqualityComparer<K>.Default.Equals(
                 outerKeySelector(t), innerKeySelector(u))));
      }

      //public static async Task<T> Cast<T>(this Task source)
      //{
      //   await source;
      //   return (T)((dynamic)source).Result;
      //}
   }
}
