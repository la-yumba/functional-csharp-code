using Xunit;
using LaYumba.Functional.Data.BinaryTree;

namespace LaYumba.Functional.Tests
{
   using static Tree;

   
   public class BinaryTreeTest
   {
      [Fact]
      public void MapLeaf()
      {
         var tree = Leaf(3);

         var actual = tree.Map(i => i + 1);
         var expected = Leaf(4);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void MapBranch()
      {
         var tree = Branch(
            Branch(Leaf(1), Leaf(2)),
            Leaf(3)
         );

         var actual = tree.Map(i => i + 1);

         var expected = Branch(
            Branch(Leaf(2), Leaf(3)),
            Leaf(4)
         );

         Assert.Equal(expected, actual);
      }
   }
}
