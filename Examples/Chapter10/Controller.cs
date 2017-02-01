using Boc.Commands;
using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using Boc.Domain.Events;
using AccountState = Boc.Chapter10.Domain.AccountState;
using Boc.Chapter10.Domain;
using Microsoft.AspNetCore.Mvc;
using Boc.Domain;

namespace Boc.Chapter10
{
   namespace Unsafe
   {
      public class Chapter10_Transfers_NoValidation : Controller
      {
         Func<Guid, AccountState> getAccount;
         Action<Event> saveAndPublish;

         public IActionResult MakeTransfer([FromBody] MakeTransfer cmd)
         {
            var account = getAccount(cmd.DebitedAccountId);

            // performs the transfer
            var (evt, newState) = account.Debit(cmd);

            saveAndPublish(evt);

            // returns information to the user about the new state
            return Ok(new { Balance = newState.Balance });
         }
      }

      // unsafe version
      public static class Account
      {
         // handle commands

         public static (Event Event, AccountState NewState) Debit
            (this AccountState @this, MakeTransfer transfer)
         {
            var evt = transfer.ToEvent();
            var newState = @this.Apply(evt);

            return (evt, newState);
         }

         // apply events

         public static AccountState Create(CreatedAccount evt)
            => new AccountState
               (
                  Currency: evt.Currency,
                  Status: AccountStatus.Active
               );

         public static AccountState Apply(this AccountState @this, Event evt)
            => new Pattern<AccountState>
            {
               (DepositedCash e) => @this.Credit(e.Amount),
               (DebitedTransfer e) => @this.Debit(e.DebitedAmount),
               (FrozeAccount e) => @this.WithStatus(AccountStatus.Frozen),
            }
            .Match(evt);
      }
   }

   namespace WithValidation
   {
      public class Chapter10_Transfers_WithValidation : Controller
      {
         Func<MakeTransfer, Validation<MakeTransfer>> validate;
         Func<Guid, AccountState> getAccount;
         Action<Event> saveAndPublish;

         public IActionResult MakeTransfer([FromBody] MakeTransfer cmd)
            => validate(cmd)
               .Bind(t => getAccount(t.DebitedAccountId).Debit(t))
               .Do(result => saveAndPublish(result.Item1))
               .Match<IActionResult>(
                  Invalid: errs => BadRequest(new { Errors = errs }),
                  Valid: result => Ok(new { Balance = result.Item2.Balance }));         
      }
   }
}
