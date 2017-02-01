using Boc.Commands;
using Boc.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using NUnit.Framework;
using Unit = System.ValueTuple;

namespace Boc.Chapter7
{
   public delegate Validation<T> Validator<T>(T t);

   // option 2. use MVC, and inject a handler function as a dependency
   namespace Delegate
   {
      public static class Validation
      {
         public static Validator<BookTransfer> DateNotPast(Func<DateTime> clock)
            => cmd 
            => cmd.Date.Date < clock().Date
               ? Errors.TransferDateIsPast
               : Valid(cmd);
      }

      public class BookTransferController_FunctionDependencies : Controller
      {
         Validator<BookTransfer> validate;
         Func<BookTransfer, Exceptional<Unit>> save;

         public BookTransferController_FunctionDependencies(Validator<BookTransfer> validate
            , Func<BookTransfer, Exceptional<Unit>> save)
         {
            this.validate = validate;
            this.save = save;
         }

         //[HttpPut, Route("api/TransferOn")]
         public IActionResult BookTransfer([FromBody] BookTransfer cmd)
            => validate(cmd).Map(save).Match( 
               Invalid: BadRequest,
               Valid: result => result.Match<IActionResult>(
                  Exception: _ => StatusCode(500, Errors.UnexpectedError),
                  Success: _ => Ok()));
      }

      public class Tests
      {
         [Test]
         public void WhenValid_AndSaveSucceeds_ThenResponseIsOk()
         {
            var controller = new BookTransferController_FunctionDependencies(
               validate: cmd => Valid(cmd),
               save: cmd => Exceptional(Unit()));

            var result = controller.BookTransfer(new BookTransfer());

            Assert.AreEqual(typeof(OkResult), result.GetType());
         }
      }
   }

   // option 3. dont use MVC; just use a function, and inject it into 
   // the pipeline in the Startup class
   namespace FunctionsEverywhere
   {
      using static ActionResultFactory;

      public class UseCases
      {
         public static Func<ILogger
            , Func<BookTransfer, Validation<Exceptional<Unit>>>
            , BookTransfer
            , IActionResult>
         TransferOn => (logger, handle, cmd) =>
         {
            Func<Exception, IActionResult> onFaulted = ex =>
            {
               logger.LogError(ex.Message);
               return InternalServerError(ex);
            };

            return handle(cmd).Match(
               Invalid: BadRequest,
               Valid: result => result.Match(
                  Exception: onFaulted,
                  Success: _ => Ok()));
         };
      }

      static class ActionResultFactory
      {
         public static IActionResult Ok() => new OkResult();
         public static IActionResult Ok(object value) => new OkObjectResult(value);
         public static IActionResult BadRequest(object error) => new BadRequestObjectResult(error);
         public static IActionResult InternalServerError(object value)
         {
            var result = new ObjectResult(value);
            result.StatusCode = 500;
            return result;
         }
      }
   }
}
