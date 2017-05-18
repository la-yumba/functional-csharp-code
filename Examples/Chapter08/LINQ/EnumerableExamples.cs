using System;
using System.Linq;
using LaYumba.Functional;

namespace Examples.Chapter8.Linq
{
   using static Console;
   using static F;

   class EnumerableExamples
   {
      internal static void _main()
      {
         SimpleSelectExample();
         WriteLine();
         SimpleSelectManyExample();

         ReadKey();
      }

      public static void SimpleSelectExample()
      {
         var a =
            from x in Range(1, 4)
            select x * 2;

         var b =
            Range(1, 4)
               .Map(x => x * 2);

         a.ForEach(WriteLine);
         WriteLine();
         b.ForEach(WriteLine);
         WriteLine();
      }
      
      public static void SimpleSelectManyExample()
      {
         var a =
            from c in Range('a', 'c')
            from i in Range(2, 3)
            select (c, i);

         var b =
            Range('a', 'c')
               .SelectMany(c => Range(2, 3)
                  .Select(i => (c, i)));

         var d = Range('a', 'c')
            .SelectMany(c => Range(2, 3)
               , (c, i) => (c, i));



         a.ForEach(t => WriteLine($"({t.Item1}, {t.Item2})"));
         WriteLine();
         b.ForEach(t => WriteLine($"({t.Item1}, {t.Item2})"));
         WriteLine();
         d.ForEach(t => WriteLine($"({t.Item1}, {t.Item2})"));
         WriteLine();
      }
   }
}
