using Xunit;
using LaYumba.Functional.Data.BinaryTree;

namespace LaYumba.Functional.Tests
{
   using static Tree;

   
   public class CoyoTest
   {
      [Fact]
      public void Check()
      {
         var tree = Branch(
            Branch(Leaf(1), Leaf(2)),
            Leaf(3)
         );

         var coyoTree = Coyo.Of<Tree<int>, int>(tree)
            .Map(i => i + 1)
            .Map(i => i * 10);

         var expected = Branch(
            Branch(Leaf(20), Leaf(30)),
            Leaf(40)
         );

         Assert.Equal(expected, coyoTree.Run());
      }
   }
}
