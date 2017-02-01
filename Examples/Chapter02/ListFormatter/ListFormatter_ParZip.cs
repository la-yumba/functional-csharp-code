using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Examples.Purity.ListFormatter.Parallel.WithRange
{
   using static ParallelEnumerable;

   static class ListFormatter
   {
      public static List<string> Format(List<string> list) 
         => list.AsParallel()
            .Select(StringExt.ToSentenceCase)
            .Zip(Range(1, list.Count), (s, i) => $"{i}. {s}")
            .ToList();
   }

   
   public class RangeListFormatterTests
   {
      [Test]
      public void ItWorksOnSingletonList()
      {
         var input = new List<string> { "coffee beans" };
         var output = ListFormatter.Format(input).ToList();
         Assert.AreEqual("1. Coffee beans", output[0]);
      }

      [Test]
      public void ItWorksOnAVeryLongList()
      {
         var size = 100000;
         var input = Enumerable.Range(1, size).Select(i => $"item{i}").ToList();
         var output = ListFormatter.Format(input);
         Assert.That(output[size - 1].StartsWith("100000. Item"));
      }
   }
}