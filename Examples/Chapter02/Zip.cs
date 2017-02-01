using System;
using System.Linq;

namespace Examples.Purity
{
   using static Console;

   public static class Zip
   {
      internal static void _main()
      {
         var sentences = Enumerable.Zip(
            new[] {1, 2, 3}, 
            new[] {"ichi", "ni", "san"}, 
            (number, name) => $"In Japanese, {number} is: {name}");

         foreach (var sentence in sentences)
            WriteLine(sentence);
      }
   }
}