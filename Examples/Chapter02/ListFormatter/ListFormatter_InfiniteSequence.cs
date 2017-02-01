using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;
using NUnit.Framework;
using static System.Console;

namespace Examples.Purity.ListFormatter.Parallel.InfiniteSequence
{
   static class ListFormatter
   {
      static IEnumerable<int> Naturals(int startingWith = 0)
      {
         while (true) yield return startingWith++;
      }

      public static IEnumerable<string> Format(IEnumerable<string> list)
         => list.AsParallel()
            .Select(StringExt.ToSentenceCase)
            .Zip(Naturals(startingWith: 1).AsParallel()
               , (s, i) => $"{i}. {s}");

      internal static void _main()
      {
         var size = 100000;
         var shoppingList = Enumerable.Range(1, size).Select(i => $"item{i}");

         ListFormatter.Format(shoppingList).ForEach(WriteLine);

         ReadKey();
      }
   }

   
   public class ZipListFormatterTests
   {
      [Test]
      public void ItWorksOnSingletonList()
      {
         var input = new[] { "coffee beans" };
         var output = ListFormatter.Format(input).ToList();
         Assert.AreEqual("1. Coffee beans", output[0]);
      }

      [Test]
      public void ItWorksOnAVeryLongList()
      {
         var size = 100000;
         var input = Enumerable.Range(1, size).Select(i => $"item{i}");
         var output = ListFormatter.Format(input).ToList();
         Assert.That(output[size - 1].StartsWith("100000. Item"));
      }
   }
}
