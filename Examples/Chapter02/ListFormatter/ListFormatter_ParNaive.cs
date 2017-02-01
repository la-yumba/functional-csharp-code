using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Examples.Purity.ListFormatter.Parallel.Naive
{
   class ListFormatter
   {
      int counter;

      Numbered<T> ToNumberedValue<T>(T t) => new Numbered<T>(t, ++counter);

      // possible fix, using lock
      // Numbered<T> ToNumberedValue<T>(T t) => new Numbered<T>(t, Interlocked.Increment(ref counter));

      string Render(Numbered<string> s) => $"{s.Number}. {s.Value}";

      public List<string> Format(IEnumerable<string> list)
         => list.AsParallel()
            .Select(StringExt.ToSentenceCase)
            .Select(ToNumberedValue)
            .OrderBy(n => n.Number)
            .Select(Render)
            .ToList();

      public void Main()
      {
         var size = 100000;
         var shoppingList = Enumerable.Range(1, size).Select(i => $"item{i}");

         new ListFormatter()
            .Format(shoppingList)
            .ForEach(Console.WriteLine);

         Console.Read();
      }

   }

   
   public class ParallelListFormatterTests
   {
      [Test]
      public void ItWorksOnSingletonList()
      {
         var input = new[] { "coffee beans" };
         var output = new ListFormatter().Format(input);
         Assert.That("1. Coffee beans" == output[0]);
      }

      //Expected string length 20 but was 19. Strings differ at index 0.
      //Expected: "1000000. Item1000000"
      //But was:  "956883. Item1000000"
      //-----------^
      [Ignore("Tests will fail because of lost updates")]
      [Test]
      public void ItWorksOnAVeryLongList()
      {
         var size = 100000;
         var input = Enumerable.Range(1, size).Select(i => $"item{i}");
         var output = new ListFormatter().Format(input);
         Assert.AreEqual("100000. Item100000", output[size - 1]);
      }
   }
}