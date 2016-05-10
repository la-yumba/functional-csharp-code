using System;
using LaYumba.Functional;

namespace Examples.Chapter3
{
   public struct Age
   {
      public int Value { get; }
      private Age(int value) { Value = value; }

      public static Option<Age> Create(int age)
         => 0 < age && age < 120 ? F.Some(new Age(age)) : F.None;
   }

   public static class Bind_Example
   {
      internal static void _main()
      {
         Func<string, Option<Age>> parseAge = s
            => s.ParseInt().Bind(Age.Create);

         parseAge("26");        // => Some(26)
         parseAge("notAnAge");  // => None
         parseAge("11111");     // => None
      }
   }
}
