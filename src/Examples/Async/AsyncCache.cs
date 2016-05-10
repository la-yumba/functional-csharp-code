using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Examples.Async
{
    // One important aspect of Task is that it may be handed out to multiple consumers, 
    // all of whom may await it, register continuations with it, get its result(in the case of Task<TResult>) 
    // or exceptions, and so on.This makes Task and Task<TResult> perfectly suited to be used 
    // in an asynchronous caching infrastructure.  Here’s a small but powerful asynchronous 
    // cache built on top of Task<TResult>:
    public class AsyncCache<TKey, TValue>
    {
        private readonly Func<TKey, Task<TValue>> factory;
        private readonly ConcurrentDictionary<TKey, Task<TValue>> _map
             = new ConcurrentDictionary<TKey, Task<TValue>>();

        public AsyncCache(Func<TKey, Task<TValue>> factory)
        {
            this.factory = factory;
        }

        public Task<TValue> this[TKey key]
            => _map.GetOrAdd(key, k => Task.Run(() => factory(k)));
    }
}
