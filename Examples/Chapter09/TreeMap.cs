using System;
using LaYumba.Functional.Data.BinaryTree;

namespace Examples
{
   using static Tree;

   public static class TreePatternMatching
   {
      internal static void _main()
      {
         var tree = Branch
         (
            Left: Leaf("one"),
            Right: Branch
            (
               Left: Leaf("two"),
               Right: Leaf("three")
            )
         );

         Console.WriteLine(tree.Map(s => s.ToUpper()));
         Console.ReadKey();
      }
   }
}
