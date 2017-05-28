using Boc.Commands;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Threading.Tasks;
using Boc.Chapter10.Domain;
using Microsoft.AspNetCore.Mvc;
using Boc.Domain;

namespace Boc.Chapter15
{
   public class MakeTransferController : Controller
   {
      public MakeTransferController(Func<Guid, Task<Option<AccountProcess>>> getAccount
         , Func<MakeTransfer, Validation<MakeTransfer>> validate)
      {
         Validate = validate;
         GetAccount = id => getAccount(id)
            .Map(opt => opt.ToValidation(() => Errors.UnknownAccountId(id)));
      }

      Func<MakeTransfer, Validation<MakeTransfer>> Validate;
      Func<Guid, Task<Validation<AccountProcess>>> GetAccount;
      
      public Task<IActionResult> MakeTransfer([FromBody] MakeTransfer command)
      {
         Task<Validation<AccountState>> outcome =
            from cmd in Async(Validate(command))
            from acc in GetAccount(cmd.DebitedAccountId)
            from result in acc.Handle(cmd)
            select result.NewState;

         return outcome.Map(
            Faulted: ex => StatusCode(500, Errors.UnexpectedError),
            Completed: val => val.Match(
               Invalid: errs => BadRequest(new { Errors = errs }),
               Valid: newState => Ok(new { Balance = newState.Balance }) as IActionResult));
      }
   }   
}
