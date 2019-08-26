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

   public class Task_Apply_Test
   {
      async Task<string> Fail(string message = null) => await Task.FromException<string>(new Exception(message)); 
      async Task<T> Succeed<T>(T t) => await Task.FromResult(t);
      
      Func<int, int, int> add = (a, b) => a + b;
      Func<int, int, int> multiply = (i, j) => i * j;

      private readonly Func<int, int, int, int> add3Integers = 
         (a, b, c) => a + b + c;
      private readonly Func<int, int, int, int, int> add4Integers = 
         (a, b, c, d) => a + b + c + d;
      private readonly Func<int, int, int, int, int, int> add5Integers = 
         (a, b, c, d, e) => a + b + c + d + e;
      private readonly Func<int, int, int, int, int, int, int> add6Integers = 
         (a, b, c, d, e, f) => a + b + c + d + e + f;
      private readonly Func<int, int, int, int, int, int, int, int> add7Integers = 
         (a, b, c, d, e, f, g) => a + b + c + d + e + f + g;
      private readonly Func<int, int, int, int, int, int, int, int, int> add8Integers = 
         (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h;
      private readonly Func<int, int, int, int, int, int, int, int, int, int> add9Integers = 
         (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i;

      [Fact]
      public async void MapAndApplyArg_ReturnsExpectedContent()
      {
         var result = await Succeed(3)
            .Map(add)
            .Apply(Succeed(4));

         Assert.Equal(7, result);
      }

      [Fact]
      public async void MapAndApplyArg_to_function_requiring_3_arguments()
      {
         var result = await Succeed(1)
            .Map(add3Integers)
            .Apply(Succeed(2))
            .Apply(Succeed(3));

         Assert.Equal(6, result);
      }

      [Fact]
      public async void MapAndApplyArg_to_function_requiring_4_arguments()
      {
         var result = await Succeed(1)
            .Map(add4Integers)
            .Apply(Succeed(2))
            .Apply(Succeed(3))
            .Apply(Succeed(4));

         Assert.Equal(10, result);
      }

      [Fact]
      public async void MapAndApplyArg_to_function_requiring_5_arguments()
      {
         var result = await Succeed(1)
            .Map(add5Integers)
            .Apply(Succeed(2))
            .Apply(Succeed(3))
            .Apply(Succeed(4))
            .Apply(Succeed(5));

         Assert.Equal(15, result);
      }

      [Fact]
      public async void MapAndApplyArg_to_function_requiring_6_arguments()
      {
         var result = await Succeed(1)
            .Map(add6Integers)
            .Apply(Succeed(2))
            .Apply(Succeed(3))
            .Apply(Succeed(4))
            .Apply(Succeed(5))
            .Apply(Succeed(6));

         Assert.Equal(21, result);
      }

      [Fact]
      public async void MapAndApplyArg_to_function_requiring_7_arguments()
      {
         var result = await Succeed(1)
            .Map(add7Integers)
            .Apply(Succeed(2))
            .Apply(Succeed(3))
            .Apply(Succeed(4))
            .Apply(Succeed(5))
            .Apply(Succeed(6))
            .Apply(Succeed(7));

         Assert.Equal(28, result);
      }

      [Fact]
      public async void MapAndApplyArg_to_function_requiring_8_arguments()
      {
         var result = await Succeed(1)
            .Map(add8Integers)
            .Apply(Succeed(2))
            .Apply(Succeed(3))
            .Apply(Succeed(4))
            .Apply(Succeed(5))
            .Apply(Succeed(6))
            .Apply(Succeed(7))
            .Apply(Succeed(8));

         Assert.Equal(36, result);
      }

      [Fact]
      public async void MapAndApplyArg_to_function_requiring_9_arguments()
      {
         var result = await Succeed(1)
            .Map(add9Integers)
            .Apply(Succeed(2))
            .Apply(Succeed(3))
            .Apply(Succeed(4))
            .Apply(Succeed(5))
            .Apply(Succeed(6))
            .Apply(Succeed(7))
            .Apply(Succeed(8))
            .Apply(Succeed(9));

         Assert.Equal(45, result);
      }
   }
}
