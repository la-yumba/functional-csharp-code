using System;

namespace Examples.Chapter3
{
   public static class MethodChaining
   {
      static string AbbreviateName(this Person p)
         => Abbreviate(p.FirstName) + Abbreviate(p.LastName);

      static string AppendDomain(this string localPart)
         => $"{localPart}@manning.com";

      static string Abbreviate(string s)
         => s.Substring(0, 2).ToLower();

      internal static void _main()
      {
         var joe = new Person("Joe", "Bloggs");
         var email = joe.AbbreviateName().AppendDomain(); 
         // => jobl@manning.com

         Console.WriteLine(email);
         Console.ReadKey();
      }
   }
}