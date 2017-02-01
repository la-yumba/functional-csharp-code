using Xunit;
using System;
using static LaYumba.Functional.F;

namespace LaYumba.Functional.Tests
{
   using static TestUtils;

   public class TryTests
   {
      Try<Uri> CreateUri(string uri) => () => new Uri(uri);

      [Fact]
      public void SuccessfulTry()
      {
         var uriTry = CreateUri("http://github.com");

         uriTry.Run().Match(
             Success: uri => Assert.NotNull(uri),
             Exception: ex => Fail()
         );
      }

      [Fact]
      public void FailingTry()
      {
         var uriTry = CreateUri("rubbish");

         uriTry.Run().Match(
             Success: uri => Fail(),
             Exception: ex => Assert.NotNull(ex)
         );
      }

      [Fact]
      public void ItIsLazy()
      {
         bool tried = false;

         Func<string, Try<Uri>> createUri = (uri) => Try(() =>
         {
            tried = true;
            return new Uri(uri);
         });

         var uriTry = createUri("http://github.com");
         Assert.False(tried, "creating a Try should not run it");

         var schemeTry = uriTry.Map(uri => uri.Scheme);
         Assert.False(tried, "mapping onto a try should not run it");

         uriTry.Run().Match(
             Success: uri => Assert.NotNull(uri),
             Exception: ex => Assert.True(false, "should have succeeded")
         );
         Assert.True(tried, "matching should run the Try");
      }
   }
}
