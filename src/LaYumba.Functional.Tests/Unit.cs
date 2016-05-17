using NUnit.Framework;

namespace LaYumba.Functional.Tests
{
   [TestFixture]
   public class Unit_Test
   {
      [Test]
      public void ThereCanOnlyBeOneUnit()
      {
         Assert.AreEqual(new Unit(), F.Unit());
      }
   }
}