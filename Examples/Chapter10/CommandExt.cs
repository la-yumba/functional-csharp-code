using Boc.Commands;
using Boc.Domain.Events;

namespace Boc.Chapter10
{
   public static class CommandExt
   {
      public static DebitedTransfer ToEvent(this MakeTransfer cmd)
         => new DebitedTransfer
         {
            Beneficiary = cmd.Beneficiary,
            Bic = cmd.Bic,
            DebitedAmount = cmd.Amount,
            EntityId = cmd.DebitedAccountId,
            Iban = cmd.Iban,
            Reference = cmd.Reference,
            Timestamp = cmd.Timestamp
         };
   }
}
