using Xunit;
using Unit = System.ValueTuple;

namespace LaYumba.Functional.Tests
{
   
   public class Unit_Test
   {
      [Fact]
      public void ThereCanOnlyBeOneUnit()
      {
         Assert.Equal(new Unit(), F.Unit());
      }
   }
}