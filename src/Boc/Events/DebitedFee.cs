using System;

namespace Boc.Events
{
   public class DebitedFee : IEvent
   {
      public DebitedFee(Guid entityId, decimal amount, string description)
      {
         EntityId = entityId;
         Amount = amount;
         Description = description;
      }

      public Guid EntityId { get; }
      public string EntityType { get; } = "Account";
      public decimal Amount { get; }
      public string Description { get; }

      public DateTime Timestamp
      {
         get
         {
            throw new NotImplementedException();
         }
      }
   }
}