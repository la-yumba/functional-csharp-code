using Xunit;

namespace LaYumba.Functional.Tests
{
   internal static class Assertions
   {
      public static void Succeed() {; }
      public static void Fail() => Assert.True(false);
   }
}
