using System;
using LaYumba.Functional;

using Name = System.String;
using Greeting = System.String;
using PersonalizedGreeting = System.String;

namespace Examples.Chapter7
{
   using static Console;

   public static class Greetings
   {
      internal static void Run()
      {
         Func<Greeting, Name, PersonalizedGreeting> greet 
            = (gr, name) => $"{gr}, {name}";

         Func<Greeting, Func<Name, PersonalizedGreeting>> greetWith 
            = gr => name => $"{gr}, {name}";

         var names = new Name[] { "Tristan", "Ivan" };

         WriteLine("Greet - with 'normal', multi-argument application");
         names.Map(g => greet("Hello", g)).ForEach(WriteLine);
         // prints: Hello, Tristan
         //         Hello, Ivan

         WriteLine("Greet formally - with partial application, manual");
         var greetFormally = greetWith("Good evening");
         names.Map(greetFormally).ForEach(WriteLine);
         // prints: Good evening, Tristan
         //         Good evening, Ivan

         WriteLine("Greet informally - with partial application, general");
         var greetInformally = greet.Apply("Hey");
         names.Map(greetInformally).ForEach(WriteLine);
         // prints: Hey, Tristan
         //         Hey, Ivan

         WriteLine("Greet nostalgically - with partial application, currying");
         var greetNostalgically = greet.Curry()("Arrivederci");
         names.Map(greetNostalgically).ForEach(WriteLine);
         // prints: Arrivederci, Tristan
         //         Arrivederci, Ivan
      }
   }
}