using System.Diagnostics.Contracts;
using Boc.Domain;

namespace Boc
{
   public struct Money
   {
      public Money(decimal amount, Currency currency)
      {
         Amount = amount;
         Currency = currency;
      }

      public decimal Amount { get; }
      public Currency Currency { get; }

      public static Money operator +(Money left, Money right)
      {
         Contract.Requires(left.Currency == right.Currency);
         return new Money(left.Amount + right.Amount, left.Currency);
      }

      public static bool operator >(Money left, Money right)
      {
         Contract.Requires(left.Currency == right.Currency);
         return left.Amount > right.Amount;
      }

      public static bool operator <(Money left, Money right)
      {
         Contract.Requires(left.Currency == right.Currency);
         return left.Amount < right.Amount;
      }

      public static bool operator >=(Money left, Money right)
      {
         return !(left.Amount < right.Amount);
      }

      public static bool operator <=(Money left, Money right)
      {
         return !(left.Amount > right.Amount);
      }

      public static bool operator <=(Money left, decimal right)
      {
         return !(left.Amount > right);
      }

      public static bool operator >=(Money left, decimal right)
      {
         return !(left.Amount < right);
      }

      public static bool operator ==(Money left, Money right)
      {
         Contract.Requires(left.Currency == right.Currency);
         return left.Amount == right.Amount;
      }

      public static bool operator !=(Money left, Money right)
      {
         return !(left == right);
      }
   }
}
