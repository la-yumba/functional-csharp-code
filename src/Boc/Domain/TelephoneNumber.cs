using System;

namespace Boc.Domain
{
   public enum NumberType { Mobile, Home, Office }

   public class TelephoneNumber
   {
      public static TelephoneNumber Create(NumberType type
         , Country country, int number) => new TelephoneNumber(type, country, number);

      public static Func<Country, Func<int, TelephoneNumber>>
      CreateCurried(NumberType type)
         => country => number => new TelephoneNumber(type, country, number);

      public static Func<NumberType, Func<Country, Func<int, TelephoneNumber>>>
         CreateFunc = type => country => number => new TelephoneNumber(type, country, number);

      public NumberType Type { get; private set; }
      public Country Country { get; }
      public int Number { get; }

      public TelephoneNumber(NumberType type, Country country, int number)
      {
         Type = type;
         Country = country;
         Number = number;
      }

      public T WithType<T>(NumberType type) where T : TelephoneNumber
      {
         T result = (T)MemberwiseClone();
         result.Type = type;
         return result;
      }
   }
}
