using System.Collections.Generic;

namespace Examples.Chapter2
{
   public static class Examples
   {
      public static string Get1(IDictionary<string, string> dictionary, string key)
      {
         if (dictionary.ContainsKey(key))
            return dictionary[key];
         return null;
      }

      public static string Get2(IDictionary<string, string> dictionary, string key)
      {
         string result;
         dictionary.TryGetValue(key, out result);
         return result;
      }

   }
}