using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
    public static class TaskExt
    {
        public static Task<R> Map<T, R>(this Task<T> @this, Func<T, R> continuation)
            => @this.ContinueWith(t => continuation(t.Result)
                , TaskContinuationOptions.OnlyOnRanToCompletion);

        public static Task<Unit> ForEach<T>(this Task<T> @this, Action<T> continuation)
            => @this.ContinueWith(t => continuation.ToFunc()(t.Result)
                , TaskContinuationOptions.OnlyOnRanToCompletion);

        public static async Task<R> Bind<T, R>(this Task<T> @this, Func<T, Task<R>> func) 
            => await func(await @this);

        public static async Task<V> SelectMany<T, U, V>(this Task<T> source
            , Func<T, Task<U>> selector, Func<T, U, V> resultSelector)
        {
            T t = await source;
            U u = await selector(t);
            return resultSelector(t, u);
        }

        public static async Task<U> Select<T, U>(this Task<T> source
            , Func<T, U> selector)
        {
            T t = await source;
            return selector(t);
        }

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

        public static async Task<T> Cast<T>(this Task source)
        {
            await source;
            return (T)((dynamic)source).Result;
        }
    }
}
