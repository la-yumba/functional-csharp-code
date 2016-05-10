using System.Collections.Generic;
using System.Linq;

namespace Examples.Purity.ListFormatter.WithZip
{
   using static Enumerable;

   static class ListFormatter
   {
      public static List<string> Format__(List<string> list)
         => list
            .Select(StringExt.ToSentenceCase)
            .Zip(Range(1, list.Count), (s, i) => $"{i}. {s}")
            .ToList();

      public static List<string> Format(List<string> list)
      {
         var left = list.Select(StringExt.ToSentenceCase);
         var right = Range(1, list.Count);
         var zipped = Enumerable.Zip(left, right, (s, i) => $"{i}. {s}");
         return zipped.ToList();
      }
   }
}