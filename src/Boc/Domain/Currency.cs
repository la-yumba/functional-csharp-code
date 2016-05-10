using System;

namespace Boc.Domain
{
   public struct Currency : IEquatable<Currency>
   {
      private Currency(string code) { Code = code; }
      public string Code { get;  }
      public static implicit operator Currency(string s) => new Currency(s);

      public bool Equals(Currency other) => this == other;
      public static bool operator ==(Currency left, Currency right) => left.Code == right.Code;
      public static bool operator !=(Currency left, Currency right) => !(left == right);
   }
}