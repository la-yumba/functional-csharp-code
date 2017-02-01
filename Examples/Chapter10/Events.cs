using System;

namespace Boc.Domain.Events
{
   public class Event
   {
      public Guid EntityId { get; set; }
      public DateTime Timestamp { get; set; }
   }

   public class CreatedAccount : Event
   {
      public CurrencyCode Currency { get; set; }
   }

   public class AlteredOverdraft : Event
   {
      public decimal By { get; set; }
   }

   public class FrozeAccount : Event { }

   public class DepositedCash : Event
   {
      public decimal Amount { get; set; }
      public Guid BranchId { get; set; }
   }

   public class DebitedTransfer : Event
   {
      public string Beneficiary { get; set; }
      public string Iban { get; set; }
      public string Bic { get; set; }

      public decimal DebitedAmount { get; set; }
      public string Reference { get; set; }
   }

   class DebitedFee : Event
   {
      public decimal Amount { get; }
      public string Description { get; }
   }
}
