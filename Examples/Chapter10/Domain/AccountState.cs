using Boc.Domain;

namespace Boc.Domain
{
   public enum AccountStatus
   { Requested, Active, Frozen, Dormant, Closed }
}

namespace Boc.Chapter10.Domain
{
      public sealed class AccountState
   {
      public AccountStatus Status { get; }
      public CurrencyCode Currency { get; }
      public decimal Balance { get; }
      public decimal AllowedOverdraft { get; }

      public AccountState
         ( CurrencyCode Currency
         , AccountStatus Status = AccountStatus.Requested
         , decimal Balance = 0
         , decimal AllowedOverdraft = 0)
      {
         this.Currency = Currency;
         this.Status = Status;
         this.Balance = Balance;
         this.AllowedOverdraft = AllowedOverdraft;
      }

      public AccountState Debit(decimal amount) => Credit(-amount);

      public AccountState Credit(decimal amount)
         => new AccountState(
               Currency: this.Currency,
               Status: this.Status,
               Balance: this.Balance + amount,
               AllowedOverdraft: this.AllowedOverdraft
            );

      public AccountState WithStatus(AccountStatus newStatus)
         => new AccountState(
               Currency: this.Currency,
               Status: newStatus,
               Balance: this.Balance,
               AllowedOverdraft: this.AllowedOverdraft
            );
   }
}
