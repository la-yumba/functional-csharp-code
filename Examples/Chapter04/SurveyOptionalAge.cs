using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using Examples.Chapter3;
using static System.Console;

namespace Examples.Bind
{
   public static class AskForValidAgeAndPrintFlatteringMessage
   {
      public static void _main()
         => WriteLine($"Only {ReadAge()}! That's young!");

      static Option<Age> ParseAge(string s)
         => Int.Parse(s).Bind(Age.Of);

      static Age ReadAge()
         => ParseAge(Prompt("Please enter your age")).Match(
            () => ReadAge(),
            (age) => age);      

      static string Prompt(string prompt)
      {
         WriteLine(prompt);
         return ReadLine();
      }
   }

   class SurveyOptionalAge
   {
      class Person
      {
         public Option<int> Age { get; set; }
      }

      static IEnumerable<Person> Population => new[]
      {
         new Person { Age = Some(33) },
         new Person { }, // this person did not disclose her age
         new Person { Age = Some(37) },
      };

      internal static void _main()
      {
         var optionalAges = Population.Map(p => p.Age);
         // => [Some(33), None, Some(37)]

         var statedAges = Population.Bind(p => p.Age);
         // => [33, 37]

         var averageAge = statedAges.Average();
         // => 35
      }
   }
}
