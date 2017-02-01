using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;

namespace Examples.Chapter6
{
   using static F;

   public class Aggregate
   {
      public static void _main()
      {
         var sum = Range(1, 5).Aggregate(0, (acc, i) => acc + i);
         // => 15

         var count = Range(1, 5).Aggregate(0, (acc, _) => acc + 1);
         // => 5
      }
   }
}
