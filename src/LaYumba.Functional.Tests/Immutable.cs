using NUnit.Framework;

namespace LaYumba.Functional.Tests
{
   using static Assert;

   class A
   {
      public int B { get; }
      public string C { get; }
      public A(int b, string c) { B = b; C = c; }
   }

   [TestFixture]
   public class Immutable_With_PropertyName
   {
      A original = new A(123, "hello");

      [Test] public void ItChangesTheDesiredProperty()
      {
         var @new = original.With("B", 777);

         AreEqual(777, @new.B); // updated
         AreEqual("hello", @new.C); // same as original
      }

      [Test] public void ItDoesNotChangesTheOriginal()
      {
         var @new = original.With("B", 777);

         AreEqual(123, original.B);
         AreEqual("hello", original.C);
      }
   }

   [TestFixture]
   public class Immutable_With_PropertyExpression
   {
      A original = new A(123, "hello");

      [Test]
      public void ItChangesTheDesiredProperty()
      {
         var @new = original.With(a => a.C, "hi");

         AreEqual(123, original.B);
         AreEqual("hello", original.C);

         AreEqual(123, @new.B);
         AreEqual("hi", @new.C);
      }

      [Test]
      public void YouCanChainWith()
      {
         var @new = original
            .With(a => a.B, 345)
            .With(a => a.C, "howdy");

         AreEqual(345, @new.B);
         AreEqual("howdy", @new.C);      }
   }
}
