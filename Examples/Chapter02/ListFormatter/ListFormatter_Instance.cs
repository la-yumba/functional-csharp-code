using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static System.Console;

namespace Examples.Purity.ListFormatter.Instance
{
   class ListFormatter
   {
      int counter;

      string PrependCounter(string s) => $"{++counter}. {s}";
      
      public List<string> Format(List<string> list)
         => list
            .Select(StringExt.ToSentenceCase)
            .Select(PrependCounter)
            .ToList();
      
      internal static void _main()
      {
         var shoppingList = new List<string> { "coffee beans", "BANANAS", "Dates" };

         new ListFormatter()
            .Format(shoppingList)
            .ForEach(WriteLine);

         Read();
      }
   }
   
   public class ListFormatter_InstanceTests
   {
      [Test] public void ItWorksOnSingletonList()
      {
         var input = new List<string> { "coffee beans" };
         var output = new ListFormatter().Format(input);
         Assert.AreEqual("1. Coffee beans", output[0]);
      }

      [Test]
      public void ItWorksOnLongerList()
      {
         var input = new List<string> { "coffee beans", "BANANAS" };
         var output = new ListFormatter().Format(input);
         Assert.AreEqual("1. Coffee beans", output[0]);
         Assert.AreEqual("2. Bananas", output[1]);
      }

      [Test]
      public void ItWorksOnAVeryLongList()
      {
         var size = 100000;
         var input = Enumerable.Range(1, size).Select(i => $"item{i}").ToList();
         var output = new ListFormatter().Format(input);
         Assert.AreEqual("100000. Item100000", output[size - 1]);
      }
   }
}
