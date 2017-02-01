using LaYumba.Functional;
using NUnit.Framework;
using System;

namespace Exercises.Chapter7.Solutions
{
   static class Solutions
   {
      // 1. Partial application with a binary arithmethic function:
      // Write a function `Remainder`, that calculates the remainder of 
      // integer division(and works for negative input values!). 

      public static Func<int, int, int> Remainder = (dividend, divisor)
         => dividend - ((dividend / divisor) * divisor);

      [TestCase(8, 5, ExpectedResult = 3)]
      [TestCase(-8, 5, ExpectedResult = -3)]
      public static int TestRemainder(int dividend, int divisor)
         => Remainder(dividend, divisor);

      // Notice how the expected order of parameters is not the
      // one that is most likely to be required by partial application
      // (you are more likely to partially apply the divisor).

      // Write an `ApplyR` function, that gives the rightmost parameter to
      // a given binary function (try to write it without looking at the implementation for `Apply`).
      // Write the signature of `ApplyR` in arrow notation, both in curried and non-curried form

      // ApplyR : (((T1, T2) -> R), T2) -> T1 -> R
      // ApplyR : (T1 -> T2 -> R) -> T2 -> T1 -> R (curried form)
      static Func<T1, R> ApplyR<T1, T2, R>(this Func<T1, T2, R> func, T2 t2)
          => t1 => func(t1, t2);

      // Use `ApplyR` to create a function that returns the
      // remainder of dividing any number by 5. 

      static Func<int, int> RemainderWhenDividingBy5 = Remainder.ApplyR(5);

      // Write an overload of `ApplyR` that gives the rightmost argument to a ternary function
      static Func<T1, T2, R> ApplyR<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T3 t3)
          => (t1, t2) => func(t1, t2, t3);


      // 2. Let's move on to ternary functions. Define a class `PhoneNumber` with 3
      // fields: number type(home, mobile, ...), country code('it', 'uk', ...), and number.
      // `CountryCode` should be a custom type with implicit conversion to and from string.

      enum NumberType { Mobile, Home, Office }

      class CountryCode
      {
         string Value { get; }
         public CountryCode(string value) { Value = value; }
         public static implicit operator string(CountryCode c) => c.Value;
         public static implicit operator CountryCode(string s) => new CountryCode(s);
         public override string ToString() => Value;
      }

      class PhoneNumber
      {
         public NumberType Type { get; }
         public CountryCode Country { get; }
         public string Number { get; }

         public PhoneNumber(NumberType type, CountryCode country, string number)
         {
            Type = type;
            Country = country;
            Number = number;
         }
      }

      // Now define a ternary function that creates a new number, given values for these fields.
      // What's the signature of your factory function? 

      // CreatePhoneNumber : CountryCode -> NumberType -> string -> PhoneNumber
      static Func<CountryCode, NumberType, string, PhoneNumber> 
      CreatePhoneNumber = (country, type, number) 
         => new PhoneNumber(type, country, number);

      // Use partial application to create a binary function that creates a UK number, 
      // and then again to create a unary function that creates a UK mobile

      static Func<NumberType, string, PhoneNumber>
      CreateUkNumber = CreatePhoneNumber.Apply((CountryCode)"uk");

      static Func<string, PhoneNumber>
      CreateUkMobileNumber = CreateUkNumber.Apply(NumberType.Mobile);


      // 3. Functions everywhere. You may still have a feeling that objects are ultimately 
      // more powerful than functions. Surely, a logger object should expose methods 
      // for related operations such as Debug, Info, Error? 
      // To see that this is not necessarily so, challenge yourself to write 
      // a very simple logging mechanism that does not involve any classes or structs. 
      // You should still be able to inject a Log value into a consumer class/function, 
      // exposing operations like Debug, Info, and Error

      enum Level { Debug, Info, Error }

      delegate void Log(Level level, string message);

      static Log consoleLogger = (Level level, string message) 
         => Console.WriteLine($"[{level}]: {message}");

      static void Debug(this Log log, string message)
         => log(Level.Debug, message);

      static void Info(this Log log, string message)
         => log(Level.Info, message);

      static void Error(this Log log, string message)
         => log(Level.Error, message);

      public static void _main() 
         => ConsumeLog(consoleLogger);

      static void ConsumeLog(Log log) 
         => log.Info("this is an info message");
   }
}
