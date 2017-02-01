using Boc.Domain;
using Boc.Domain.Events;
using System;

namespace Boc.Commands
{
   public class CreateAccount : Command
   {
      public Guid AccountId { get; set; }
      public CurrencyCode Currency { get; set; }

      public CreatedAccount ToEvent() => new CreatedAccount
      {
         EntityId = this.AccountId,
         Timestamp = this.Timestamp,
         Currency = this.Currency,
      };
   }

   public class AcknowledgeCashDeposit : Command
   {
      public Guid AccountId { get; set; }
      public decimal Amount { get; set; }
      public Guid BranchId { get; set; }

      public DepositedCash ToEvent() => new DepositedCash
      {
         EntityId = this.AccountId,
         Timestamp = this.Timestamp,
         Amount = this.Amount,
         BranchId = this.BranchId,
      };
   }

   public class SetOverdraft : Command
   {
      public Guid AccountId { get; set; }
      public decimal Amount { get; set; }

      public AlteredOverdraft ToEvent(decimal by) => new AlteredOverdraft
      {
         EntityId = this.AccountId,
         Timestamp = this.Timestamp,
         By = by,
      };
   }

   public class FreezeAccount : Command
   {
      public Guid AccountId { get; set; }

      public FrozeAccount ToEvent() => new FrozeAccount
      {
         EntityId = this.AccountId,
         Timestamp = this.Timestamp,
      };
   }
}