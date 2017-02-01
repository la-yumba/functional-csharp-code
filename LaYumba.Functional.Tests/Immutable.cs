using Xunit;

namespace LaYumba.Functional.Tests
{
   using static Assert;

   class A
   {
      public int B { get; }
      public string C { get; }
      public A(int b, string c) { B = b; C = c; }
   }

   
   public class Immutable_With_PropertyName
   {
      A original = new A(123, "hello");

      [Fact] public void ItChangesTheDesiredProperty()
      {
         var @new = original.With("B", 777);

         Assert.Equal(777, @new.B); // updated
         Assert.Equal("hello", @new.C); // same as original
      }

      [Fact] public void ItDoesNotChangesTheOriginal()
      {
         var @new = original.With("B", 777);

         Assert.Equal(123, original.B);
         Assert.Equal("hello", original.C);
      }
   }

   
   public class Immutable_With_PropertyExpression
   {
      A original = new A(123, "hello");

      [Fact]
      public void ItChangesTheDesiredProperty()
      {
         var @new = original.With(a => a.C, "hi");

         Assert.Equal(123, original.B);
         Assert.Equal("hello", original.C);

         Assert.Equal(123, @new.B);
         Assert.Equal("hi", @new.C);
      }

      [Fact]
      public void YouCanChainWith()
      {
         var @new = original
            .With(a => a.B, 345)
            .With(a => a.C, "howdy");

         Assert.Equal(345, @new.B);
         Assert.Equal("howdy", @new.C);      }
   }
}
