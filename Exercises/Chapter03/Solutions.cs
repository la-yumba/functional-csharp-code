using System;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Configuration;
using LaYumba.Functional;

namespace Exercises.Chapter3
{
   using static F;

   public static class Solutions
   {
      // 1.  Write an overload of `GetOrElse`, that returns the wrapped value, or
      // calls a given function to compute an alternative default.
      static T GetOrElse<T>(this Option<T> @this, Func<T> func)
         => @this.Match(
            Some: t => t,
            None: func);

      // What is the benefit of this overload over the implementation we've seen above?
      // 'func' is not evalueated unless necessary, resulting in a performance benefit
      // if 'func' is expensive

      // 2.  Write `Option.Map` in terms of `Match`.
      static Option<R> Map_Alt<T, R>(this Option<T> @this, Func<T, R> func)
          => @this.Match(
              Some: value => F.Some(func(value)),
              None: () => F.None);

      // 3.  Write an extension method `Lookup` on `IDictionary<K, T>`, that
      // takes a `K` and returns an `Option<T>`.
      static Option<T> Lookup<K, T>(this IDictionary<K, T> map, K key)
         => map.ContainsKey(key) ? Some(map[key]) : None;

      // 4.  Write an extension method `Map` on `IDictionary<K, T>`, that takes a
      // `Func<T, R>` and returns a new `Dictionary<K, R>`, mapping the original
      // keys to the values resulting from applying the function to the values.
      // TODO
   }

   public class AppConfig_Solution
   {
      NameValueCollection source;

      //public AppConfig_Solution() : this(ConfigurationManager.AppSettings) { }

      public AppConfig_Solution(NameValueCollection source)
      {
         this.source = source;
      }

      public Option<T> Get<T>(string key)
         => Some((T)Convert.ChangeType(source[key], typeof(T)));

      public T Get<T>(string key, T defaultValue)
         => Get<T>(key).Match(
            Some: value => value,
            None: () => defaultValue);
   }
}
