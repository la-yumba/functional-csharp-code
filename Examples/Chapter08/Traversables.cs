using LaYumba.Functional;
using System;
using System.Linq;
using NUnit.Framework;
using Double = LaYumba.Functional.Double;
using String = LaYumba.Functional.String;

namespace Examples.Chapter8
{
   using static F;

   public class Traversable_Validation
   {
      public static void _main()
      {
         var input = Console.ReadLine();
         var result = Process(input);
         Console.WriteLine(result);
      }

      static Validation<double> Validate(string s)
         => Double.Parse(s).Match(
            () => Error($"'{s}' is not a valid number"),
            d => Valid(d));

      static string Process(string input)
         => input.Split(',')     // Array<string>
            .Map(String.Trim)    // IEnumerable<string>
            .Traverse(Validate)  // Validation<IEnumerable<double>>
            .Map(Enumerable.Sum) // Validation<double>
            .Match(
               Invalid: errs => string.Join(", ", errs),
               Valid: sum => $"The sum is {sum}");

      [TestCase("1, 2, 3", ExpectedResult = "The sum is 6")]
      [TestCase("one, two, 3", ExpectedResult = "'one' is not a valid number, 'two' is not a valid number")]
      public string TraversableIEnumerableValidation(string s) => Process(s);
   }

   public class Traversable_Option
   {
      public static void _main()
      {
         var input = Console.ReadLine();
         var result = Process(input);
         Console.WriteLine(result);
      }

      static string Process(string input)
         => input.Split(',')        // IEnumerable<string>
            .Map(String.Trim)       // IEnumerable<string>
            .Traverse(Double.Parse) // Option<IEnumerable<double>>
            .Map(Enumerable.Sum)    // Option<double>
            .Match(
               () => "Some of your inputs could not be parsed",
               sum => $"The sum is {sum}");

      [TestCase("1, 2, 3", ExpectedResult = "The sum is 6")]
      [TestCase("one, two, 3", ExpectedResult = "Some of your inputs could not be parsed")]
      public string TraversableIEnumerableOption(string s) => Process(s);
   }
}
