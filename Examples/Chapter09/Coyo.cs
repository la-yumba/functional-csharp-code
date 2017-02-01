using LaYumba.Functional;
using LaYumba.Functional.Data.LinkedList;
using System;

using String = LaYumba.Functional.String;
using static LaYumba.Functional.Data.LinkedList.LinkedList;

namespace Examples.Chapter10.Data
{
   public class CoyoExample
   {
      public static void _main()
      {
         var emails = List(" Some@Ema.il ");

         // eager evaluation: 2 passes through the list
         emails.Map(String.Trim)
               .Map(String.ToLower)
               .ForEach(Console.WriteLine);

         // lazy evaluation: coyo just composes the functions to be mapped
         var coyo = Coyo.Of<List<string>, string>(emails)
            .Map(String.Trim)
            .Map(String.ToLower);

         // composed functions are applied only when Run is called
         // with a single pass over the list
         var results = coyo.Run();
         results.ForEach(Console.WriteLine);
      }
   }
}
