using System;

namespace Boc.Commands
{
   public abstract class Command
   {
      public DateTime Timestamp { get; set; }

      public T WithTimestamp<T>(DateTime timestamp)
         where T : Command
      {
         T result = (T)MemberwiseClone();
         result.Timestamp = timestamp;   
         return result;
      }
   }

   public class MakeTransfer : Command
   {
      public Guid DebitedAccountId { get; set; }

      public string Beneficiary { get; set; }
      public string Iban { get; set; }
      public string Bic { get; set; }

      public DateTime Date { get; set; }
      public decimal Amount { get; set; }
      public string Reference { get; set; }
   }
}