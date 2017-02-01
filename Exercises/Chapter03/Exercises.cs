using System;
using System.Collections.Specialized;
//using System.Configuration;
using LaYumba.Functional;

namespace Exercises.Chapter3
{
    public static class Exercises
    {
        // 1.  Write an overload of `GetOrElse`, that returns the wrapped value, or
        // calls a given function to compute an alternative default. What is the
        // benefit of this overload over the implementation we've seen above?

        // 2.  Write `Option.Map` in terms of `Match`.

        // 3.  Write an extension method `Lookup` on `IDictionary<K, T>`, that
        // takes a `K` and returns an `Option<T>`.

        // 4.  Write an extension method `Map` on `IDictionary<K, T>`, that takes a
        // `Func<T, R>` and returns a new `Dictionary<K, R>`, mapping the original
        // keys to the values resulting from applying the function to the values.
    }

    // 6.  Write implementations for the methods in the `AppConfig` class
    // below. (For both methods, a reasonable one-line method body is possible.
    // Assume settings are of type string, numeric or date.) Can this
    // implementation help you to test code that relies on settings in a
    // `.config` file?
    public class AppConfig
    {
      NameValueCollection source;

      //public AppConfig() : this(ConfigurationManager.AppSettings) { }

      public AppConfig(NameValueCollection source)
      {
         this.source = source;
      }

      public Option<T> Get<T>(string name)
        {
            throw new NotImplementedException("your implementation here...");
        }

        public T Get<T>(string name, T defaultValue)
        {
            throw new NotImplementedException("your implementation here...");
        }
    }
}
