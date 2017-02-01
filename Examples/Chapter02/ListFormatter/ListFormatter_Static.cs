using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static System.Console;

namespace Examples.Purity.ListFormatter.Static
{
   public static class ListFormatter
   {
      static int counter;

      static string PrependCounter(string s) => $"{++counter}. {s}";
      
      public static List<string> Format(List<string> list)
         => list
            .Select(StringExt.ToSentenceCase)
            .Select(PrependCounter)
            .ToList();
      
      internal static void _main()
      {
         var shoppingList = new List<string> { "coffee beans", "BANANAS", "Dates" };

         ListFormatter
            .Format(shoppingList)
            .ForEach(WriteLine);

         Read();
      }
   }
   
   // If ListFormatter is static, order of execution matters
   // Tests must not have any order-of-run dependency.
   [Ignore("Tests will fail because they are not isolated")]
   public class ListFormatterTests
   {
      [Test] public void ItWorksOnSingletonList()
      {
         var input = new List<string> { "coffee beans" };
         var output = ListFormatter.Format(input);
         Assert.AreEqual("1. Coffee beans", output[0]);
      }

      [Test]
      public void ItWorksOnLongerList()
      {
         var input = new List<string> { "coffee beans", "BANANAS" };
         var output = ListFormatter.Format(input);
         Assert.AreEqual("1. Coffee beans", output[0]);
         Assert.AreEqual("2. Bananas", output[1]);
      }

      [Test]
      public void ItWorksOnAVeryLongList()
      {
         var size = 100000;
         var input = Enumerable.Range(1, size).Select(i => $"item{i}").ToList();
         var output = ListFormatter.Format(input);
         Assert.AreEqual("100000. Item100000", output[size - 1]);
      }
   }
}
