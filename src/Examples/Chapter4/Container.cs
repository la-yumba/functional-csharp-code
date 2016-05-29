using LaYumba.Functional;
using System;

namespace Examples.Chapter1
{
   class Container_Example
   {
      internal static void _main()
      {
         var x = Container.Of(4d);
         // => Container(4d)

         var y = x.Map(Math.Sqrt);
         // => Container(2d)
      }

      internal static void _main2()
      {
         Func<int, int> plus3 = x => x + 3;

         var a = Container.Of(2);
         // => Container(2)

         var b = a.Map(plus3);
         // => Container(5)
      }
   }
}
