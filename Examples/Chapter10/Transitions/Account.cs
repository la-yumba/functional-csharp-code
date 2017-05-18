using Boc.Commands;
using Boc.Domain;
using Boc.Domain.Events;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using Boc.Chapter10.Domain;

namespace Boc.Chapter10.Transitions
{
   public static class Account
   {
      // handle commands

      public static Transition<AccountState, CreatedAccount> 
      Create(CreateAccount cmd)
         => _ =>
      {
         var evt = cmd.ToEvent();
         var newState = evt.ToAccount();
         return (evt, newState);
      };


      public static Transition<AccountState, DepositedCash> 
      Deposit(AcknowledgeCashDeposit cmd)
         => account =>
      {
         if (account.Status != AccountStatus.Active)
            return Errors.AccountNotActive;

         var evt = cmd.ToEvent();
         var newState = account.Apply(evt);

         return (evt, newState);
      };

      public static Transition<AccountState, AlteredOverdraft> 
      SetOverdraft(SetOverdraft cmd)
         => account =>
      {
         var evt = cmd.ToEvent(cmd.Amount - account.AllowedOverdraft);
         var newState = account.Apply(evt);

         return (evt, newState);
      };

      public static Validation<FrozeAccount> Freeze
         (this AccountState @this, FreezeAccount cmd)
      {
         if (@this.Status == AccountStatus.Frozen)
            return Errors.AccountNotActive;

         return cmd.ToEvent();
      }
      
      // apply events

      public static AccountState ToAccount(this CreatedAccount evt)
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
