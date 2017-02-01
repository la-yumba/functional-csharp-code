using Boc.Commands;
using Boc.Domain;
using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using Boc.Domain.Events;
using AccountState = Boc.Chapter10.Domain.AccountState;
using Microsoft.AspNetCore.Mvc;
using Boc.Chapter10.Domain;

namespace Boc.Chapter10.Transitions
{
   namespace WithValidation
   {
      public class Chapter10_Transfers_WithValidation : Controller
      {
         Func<CreateAccountWithOptions, Validation<CreateAccountWithOptions>> validate;
         Func<Guid> generateId;
         Action<Event> saveAndPublish;
         
         public IActionResult CreateInitialized([FromBody] CreateAccountWithOptions cmd)
            => validate(cmd)
               .Bind(Initialize)
               .Match<IActionResult>(
                  Invalid: errs => BadRequest(new { Errors = errs }),
                  Valid: id => Ok(id));

         Validation<Guid> Initialize(CreateAccountWithOptions cmd)
         {
            Guid id = generateId();
            DateTime now = DateTime.UtcNow;

            var create = new CreateAccount
            {
               AccountId = id,
               Timestamp = now,
               Currency = cmd.Currency,
            };

            var depositCash = new AcknowledgeCashDeposit
            {
               AccountId = id,
               Timestamp = now,
               Amount = cmd.InitialDepositAccount,
               BranchId = cmd.BranchId,
            };

            var setOverdraft = new SetOverdraft
            {
               AccountId = id,
               Timestamp = now,
               Amount = cmd.AllowedOverdraft,
            };

            var transitions =
               from e1 in Account.Create(create)
               from e2 in Account.Deposit(depositCash)
               from e3 in Account.SetOverdraft(setOverdraft)
               select List<Event>(e1, e2, e3);

            return transitions(default(AccountState))
               .Do(t => t.Item1.ForEach(saveAndPublish))
               .Map(_ => id);
         }
      }

      public class CreateAccountWithOptions : Command
      {
         public CurrencyCode Currency { get; set; }
         public decimal InitialDepositAccount { get; set; }
         public decimal AllowedOverdraft { get; set; }
         public Guid BranchId { get; set; }
      }
   }
}
