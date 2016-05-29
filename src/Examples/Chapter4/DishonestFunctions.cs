using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using static System.Console;

namespace Examples.Chapter3.Option
{
   class DishonestFunctions
   {
      /// <summary>
      /// prints:
      /// green!
      /// KeyNotFoundException
      /// </summary>
      internal static void _main()
      {
         try
         {
            var empty = new NameValueCollection();
            var green = empty["green"];
            WriteLine("green!");

            var alsoEmpty = new Dictionary<string, string>();
            var blue = alsoEmpty["blue"];
            WriteLine("blue!");
         }
         catch (Exception ex)
         {
            WriteLine(ex.GetType().Name);
         }
      }
   }
}
