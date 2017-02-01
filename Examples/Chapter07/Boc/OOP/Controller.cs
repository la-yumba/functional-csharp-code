using Boc.Commands;
using Boc.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using LaYumba.Functional;
using Unit = System.ValueTuple;

namespace Boc.Chapter7
{
   // option 1. use MVC, and inject an IValidator and IRepository as dependencies
   namespace OOP
   {
      public interface IValidator<T>
      {
         Validation<T> Validate(T request);
      }

      public interface IRepository<T>
      {
         Option<T> Lookup(Guid id);
         Exceptional<Unit> Save(T entity);
      }

      public class BookTransferController : Controller
      {
         IValidator<BookTransfer> validator;
         IRepository<BookTransfer> repository;

         public BookTransferController
            (IValidator<BookTransfer> validator, IRepository<BookTransfer> repository)
         {
            this.validator = validator;
            this.repository = repository;
         }

         //[HttpPost, Route("api/Chapters7/transfers/future")]
         public IActionResult BookTransfer([FromBody] BookTransfer cmd)
            => validator.Validate(cmd)
               .Map(repository.Save)
               .Match(
                  Invalid: BadRequest,
                  Valid: result => result.Match<IActionResult>(
                     Exception: _ => StatusCode(500, Errors.UnexpectedError),
                     Success: _ => Ok()));
      }
   }
}
