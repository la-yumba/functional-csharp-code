using LaYumba.Functional.Data.LinkedList;
using static LaYumba.Functional.Data.LinkedList.LinkedList;
using static System.Console;

namespace Examples.Chapter10
{
   static class ImmutableList_Example
   {
      static void _main()
      {
         var fruit = List("pineapple", "banana");
         WriteLine(fruit);
         // => ["pineapple", "banana"]

         var tropicalMix = fruit.Add("kiwi");
         WriteLine(tropicalMix);
         // => ["kiwi", "pineapple", "banana"]

         var yellowFruit = fruit.Add("lemon");
         WriteLine(yellowFruit);
         // => ["lemon", "pineapple", "banana"]

         ReadKey();
      }

      public static int Sum(this List<int> @this)
         => @this.Match(
            Empty: () => 0,
            Cons: (head, tail) => head + tail.Sum());
   }
}
