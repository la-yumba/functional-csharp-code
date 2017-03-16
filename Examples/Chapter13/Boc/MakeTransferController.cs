using Boc.Commands;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Threading.Tasks;
using Boc.Chapter10.Domain;
using Boc.Domain.Events;
using Microsoft.AspNetCore.Mvc;
using Boc.Domain;
using Unit = System.ValueTuple;

namespace Boc.Chapter10.Services
{
   public class TransferNowController : Controller
   {
      Func<MakeTransfer, Validation<MakeTransfer>> validate;
      Func<Guid, Task<Option<AccountState>>> getAccount;
      Func<Event, Task> saveAndPublish;

      Func<Guid, Task<Validation<AccountState>>> GetAccount 
         => id 
         => getAccount(id)
            .Map(opt => opt.ToValidation(() => Errors.UnknownAccountId(id)));

      Func<Event, Task<Unit>> SaveAndPublish => async e =>
      {
         await saveAndPublish(e);
         return Unit();
      };

      public Task<IActionResult> Transfer([FromBody] MakeTransfer command)
      {
         Task<Validation<AccountState>> outcome =
            from cmd in Async(validate(command))
            from acc in GetAccount(cmd.DebitedAccountId)
            from result in Async(Account.Debit(acc, cmd))
            from _ in SaveAndPublish(result.Item1).Map(Valid)
            select result.Item2;
         
         //Task<Validation<AccountState>> a = validate(command)
         //   .Traverse(cmd => getAccount(cmd.DebitedAccountId)
         //      .Bind(acc => Account.Debit(acc, cmd)
         //         .Traverse(result => saveAndPublish(result.Item1)
         //            .Map(_ => result.Item2))))
         //   .Map(vva => vva.Bind(va => va)); // flatten the nested validation inside the task

         return outcome.Map(
            Faulted: ex => StatusCode(500, Errors.UnexpectedError),
            Completed: val => val.Match(
               Invalid: errs => BadRequest(new { Errors = errs }),
               Valid: newState => Ok(new { Balance = newState.Balance }) as IActionResult));
      }
   }   
}
