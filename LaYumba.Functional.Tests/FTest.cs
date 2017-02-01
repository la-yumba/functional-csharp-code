using System;
using Xunit;

namespace LaYumba.Functional.Tests
{
   using static F;

   
   public class FTest
   {
      private const string HELLO = "hello";
      readonly Func<string, bool> isHello = s => s == HELLO;

      [Fact]
      public void NegateTest()
      {
         Assert.True(isHello(HELLO));
         Assert.False(isHello.Negate()(HELLO));
      }

      [Fact]
      public void TestListReturnFunction()
      {
         var emptyList = List<string>();
         Assert.Equal(new string[] { }, emptyList);

         var singletonList = List("Andrej");
         Assert.Equal(new[] { "Andrej" }, singletonList);

         var multiList = List("Andrej", "Natasha");
         Assert.Equal(new[] { "Andrej", "Natasha" }, multiList);
      }
   }
}
