using LaYumba.Functional;
using Enum = LaYumba.Functional.Enum;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Chapter7
{
   using static F;

   class Example_WithOption
   {
      ISet<string> ValidCountryCodes = new HashSet<string> { "ch", "uk" };

      // string -> Option<CountryCode>
      Func<string, Option<CountryCode>> optCountryCode 
         => CountryCode.Create.Apply(ValidCountryCodes);

      // string -> Option<NumberType>
      Func<string, Option<PhoneNumber.NumberType>> optNumberType
         = Enum.Parse<PhoneNumber.NumberType>;

      // applicative
      Option<PhoneNumber> CreatePhoneNumberA
         (string typeStr, string countryStr, string numberStr)
         => Some(PhoneNumber.Create)
            .Apply(optNumberType(typeStr))
            .Apply(optCountryCode(countryStr))
            .Apply(PhoneNumber.Number.Create(numberStr));
      
      // monadic
      Option<PhoneNumber> CreatePhoneNumberM
         (string typeStr, string countryStr, string numberStr)
         => optCountryCode(countryStr)
            .Bind(country => optNumberType(typeStr)
               .Bind(type => PhoneNumber.Number.Create(numberStr)
                  .Bind<PhoneNumber.Number, PhoneNumber>(number
                     => PhoneNumber.Create(type, country, number))));

      // monadic with LINQ
      Option<PhoneNumber> CreatePhoneNumber_LINQ
         (string typeStr, string countryStr, string numberStr)
         => from country in optCountryCode(countryStr)
            from type in optNumberType(typeStr)
            from number in PhoneNumber.Number.Create(numberStr)
            select PhoneNumber.Create(type, country, number);
   }

   class Example_WithValidation
   {
      ISet<string> ValidCountryCodes = new HashSet<string> { "ch", "uk" };

      // string -> Validation<CountryCode>
      Func<string, Validation<CountryCode>> validCountryCode 
         => s => CountryCode.Create(ValidCountryCodes, s).Match(
            None: () => Error($"{s} is not a valid country code"),
            Some: c => Valid(c));

      // string -> Validation<PhoneNumber.NumberType>
      Func<string, Validation<PhoneNumber.NumberType>> validNumberType
         = s => Enum.Parse<PhoneNumber.NumberType>(s).Match(
            None: () => Error($"{s} is not a valid number type"),
            Some: n => Valid(n));

      // string -> Validation<PhoneNumber.Number>
      Func<string, Validation<PhoneNumber.Number>> validNumber
         = s => PhoneNumber.Number.Create(s).Match(
            None: () => Error($"{s} is not a valid number"),
            Some: n => Valid(n));

      Validation<PhoneNumber> CreateValidPhoneNumber
         (string type, string countryCode, string number)
         => Valid(PhoneNumber.Create)
            .Apply(validNumberType(type))
            .Apply(validCountryCode(countryCode))
            .Apply(validNumber(number));

      Validation<PhoneNumber> CreateValidPhoneNumberM
         (string typeStr, string countryStr, string numberStr)
         => from type in validNumberType(typeStr) 
            from country in validCountryCode(countryStr)
            from number in validNumber(numberStr)
            select PhoneNumber.Create(type, country, number);

      [TestCase("Mobile", "ch", "123456", ExpectedResult = "Valid(Mobile: (ch) 123456)")]
      [TestCase("Mobile", "xx", "123456", ExpectedResult = "Invalid([xx is not a valid country code])")]
      [TestCase("Mobile", "xx", "1", ExpectedResult = "Invalid([xx is not a valid country code, 1 is not a valid number])")]
      [TestCase("rubbish", "xx", "1", ExpectedResult = "Invalid([rubbish is not a valid number type, xx is not a valid country code, 1 is not a valid number])")]
      public string ValidTelephoneNumberTest(string type, string country, string number)
         => CreateValidPhoneNumber(type, country, number).ToString();

      [TestCase("Mobile", "ch", "123456", ExpectedResult = "Valid(Mobile: (ch) 123456)")]
      [TestCase("Mobile", "xx", "123456", ExpectedResult = "Invalid([xx is not a valid country code])")]
      [TestCase("Mobile", "xx", "1", ExpectedResult = "Invalid([xx is not a valid country code])")]
      [TestCase("rubbish", "xx", "1", ExpectedResult = "Invalid([rubbish is not a valid number type])")]
      public string ValidTelephoneNumber_Monadic(string type, string country, string number)
         => CreateValidPhoneNumberM(type, country, number).ToString();
   }

   class CountryCode
   {
      // ISet<string> -> string -> Option<CountryCode>
      public static Func<ISet<string>, string, Option<CountryCode>>
      Create = (validCodes, code)
         => validCodes.Contains(code) 
            ? Some(new CountryCode(code)) 
            : None;
      
      string Value { get; }
      // private ctor so that no invalid instances may be created
      private CountryCode(string value) { Value = value; }
      public override string ToString() => Value;
   }

   class PhoneNumber
   {
      public enum NumberType { Mobile, Home, Office }

      public struct Number
      {
         // ISet<string> -> string -> Option<_Number>
         public static Func<string, Option<Number>>
         Create = s => Long.Parse(s)
            .Map(_ => s)
            .Where(_ => 5 < s.Length && s.Length < 11)
            .Map(_ => new Number(s));

         string Value { get; }
         private Number(string value) { Value = value; }
         public static implicit operator string(Number c) => c.Value;
         public static implicit operator Number(string s) => new Number(s);
         public override string ToString() => Value;
      }

      public NumberType Type { get; }
      public CountryCode Country { get; }
      public Number Nr { get; }

      public static Func<NumberType, CountryCode, Number, PhoneNumber> 
      Create = (type, country, number)
         => new PhoneNumber(type, country, number);

      PhoneNumber(NumberType type, CountryCode country, Number number)
      {
         Type = type;
         Country = country;
         Nr = number;
      }

      public override string ToString() => $"{Type}: ({Country}) {Nr}";
   }
}
