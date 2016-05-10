using System;
using Boc.Commands;
using Boc.Events;

namespace Boc.Domain
{
   public enum AccountType { Personal, Business }

   public class Account : IEntity
   {
      public Guid Id { get; set; }
      public string UserId { get; set; }
      public AccountType Type { get; set; }
      public decimal Balance { get; private set; }
      public decimal Overdraft { get; set; }
      public Currency Currency { get; set; }
      public decimal FreeOverdraft { get; private set; }

      public void Debit(TransferNow transfer)
      {
      }

      public DebitedFee Charge(Fee fee)
      {
         return new DebitedFee(this.Id, fee.Amount, fee.Description);
      }

      public Account WithBalance(decimal balance)
      {
         var result = (Account)MemberwiseClone();
         result.Balance = balance;
         return result;
      }
   }
}