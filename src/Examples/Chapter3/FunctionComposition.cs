using System;

namespace Examples.Chapter3
{
   using static Console;

   public static class FunctionComposition
   {
      static string AbbreviateName(Person p)
         => Abbreviate(p.FirstName) + Abbreviate(p.LastName);

      static string AppendDomain(string localPart)
         => $"{localPart}@manning.com";

      static string Abbreviate(string s)
         => s.Substring(0, 2).ToLower();

      internal static void _main()
      {
         Func<Person, string> emailFor =
            p => AppendDomain(AbbreviateName(p));

         var joe = new Person("Joe", "Bloggs");
         var email = emailFor(joe); // => jobl@manning.com

         WriteLine(email);
         ReadKey();
      }
   }
}