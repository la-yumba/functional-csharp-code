using System;
using System.Threading.Tasks;
using Xunit;

namespace LaYumba.Functional.Tests
{
   public class TaskTests
   {
      async Task<string> Fail(string message = null) => await Task.FromException<string>(new Exception(message)); 
      async Task<T> Succeed<T>(T t) => await Task.FromResult(t);

      [Fact]
      public async void WhenTSucceeds_ThenMapSucceeds()
      {
         var actual = await Succeed("value").Map(String.ToUpper);
         Assert.Equal("VALUE", actual);
      }

      [Fact]
      public async void TaskFailsWithGivenException() =>
         await Assert.ThrowsAsync<Exception>(() => Fail());

      // note that the type of the Exception should be preserved
      [Fact]
      public async void WhenTFails_ThenMapFails() =>
         await Assert.ThrowsAsync<Exception>(() => Fail().Map(String.ToUpper));

      [Fact]
      public void MapThrowsNoExceptions() =>
         Fail().Map(String.ToUpper).Map(String.Trim);

      [Fact]
      public void BindThrowsNoExceptions() =>
         Fail().Bind(_ => Fail());

      [Fact]
      public async void BindSuccess()
      {
         var result = await Succeed("value").Bind(s => Succeed("next value"));
         Assert.Equal("next value", result);
      }

      [Fact]
      public async void WhenTFails_ThenBindFails()
      {
         var task = Fail().Bind(s => Succeed("next value"));
         await Assert.ThrowsAsync<Exception>(async () => await task);
      }

      [Fact]
      public async void WhenFFails_ThenBindFails()
      {
         var task = Succeed("value").Bind(s => Fail());
         await Assert.ThrowsAsync<Exception>(async () => await task);
      }
   }
}
