using Boc.Commands;
using Boc.Domain;
using Boc.Domain.Events;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Boc.Chapter10.Domain
{
   public static class Account
   {
      // handle commands

      public static Validation<(Event Event, AccountState NewState)> Debit
         (this AccountState @this, MakeTransfer cmd)
      {
         if (@this.Status != AccountStatus.Active)
            return Errors.AccountNotActive;

         if (@this.Balance - cmd.Amount < @this.AllowedOverdraft)
            return Errors.InsufficientBalance;

         var evt = cmd.ToEvent();
         var newState = @this.Apply(evt);

         return (evt as Event, newState);
      }

      public static Validation<(Event Event, AccountState NewState)> Freeze
         (this AccountState @this, FreezeAccount cmd)
      {
         if (@this.Status == AccountStatus.Frozen)
            return Errors.AccountNotActive;

         var evt = cmd.ToEvent();
         var newState = @this.Apply(evt);

         return (evt as Event, newState);
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

      // hydrate

      public static Option<AccountState> From(IEnumerable<Event> history)
         => history.Match(
            Empty: () => None,
            Otherwise: (createdEvent, otherEvents) => Some(
               otherEvents.Aggregate(
                  seed: Account.Create((CreatedAccount)createdEvent),
                  func: (state, evt) => state.Apply(evt))));
   }
}
