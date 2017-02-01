namespace Boc.Domain
{
   public struct CurrencyCode
   {
      string Value { get; }
      public CurrencyCode(string value) { Value = value; }

      public static implicit operator string(CurrencyCode c)
         => c.Value;
      public static implicit operator CurrencyCode(string s)
         => new CurrencyCode(s);

      public override string ToString() => Value;
   }
}
