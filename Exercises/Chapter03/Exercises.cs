using System;
using System.Collections.Specialized;
//using System.Configuration;
using LaYumba.Functional;

namespace Exercises.Chapter3
{
   public static class Exercises
   {
      // 1 Write a generic function that takes a string and parses it as a value of an enum. It
      // should be usable as follows:

      // Enum.Parse<DayOfWeek>("Friday") // => Some(DayOfWeek.Friday)
      // Enum.Parse<DayOfWeek>("Freeday") // => None

      // 2 Write a Lookup function that will take an IEnumerable and a predicate, and
      // return the first element in the IEnumerable that matches the predicate, or None
      // if no matching element is found. Write its signature in arrow notation:

      // bool isOdd(int i) => i % 2 == 1;
      // new List<int>().Lookup(isOdd) // => None
      // new List<int> { 1 }.Lookup(isOdd) // => Some(1)

      // 3 Write a type Email that wraps an underlying string, enforcing that it’s in a valid
      // format. Ensure that you include the following:
      // - A smart constructor
      // - Implicit conversion to string, so that it can easily be used with the typical API
      // for sending emails
      
      // 4 Take a look at the extension methods defined on IEnumerable inSystem.LINQ.Enumerable.
      // Which ones could potentially return nothing, or throw some
      // kind of not-found exception, and would therefore be good candidates for
      // returning an Option<T> instead?
   }

   // 5.  Write implementations for the methods in the `AppConfig` class
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
