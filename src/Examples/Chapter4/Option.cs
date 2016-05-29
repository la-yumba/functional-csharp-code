using LaYumba.Functional;

namespace Examples.Chapter1
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Specialized;
   using static System.Console;

   class Option_Match_Example
   {
      internal static void _main()
      {
         string _ = null, john = "John";

         Greet(john); // prints: hello, John
         Greet(_); // prints: sorry, who?

         ReadKey();
      }

      static void Greet(string name)
         => WriteLine(GreetingFor(name));

      static string GreetingFor(string name)
         => Option.Of(name).Match(
            Some: n => $"hello, {n}",
            None: () => "sorry, who?");
   }

   class Option_Map_Example
   {
      internal static void _main()
      {
         Func<string, string> greet = name => $"hello, {name}";
         string _ = null, john = "John";

         Option.Of(john).Map(greet); // => Some("hello, John")
         Option.Of(_).Map(greet); // => None
      }
   }

   class Person
   {
      public string Name;
      public Option<Relationship> Relationship;      
   }

   class Relationship
   {
      public string Type;
      public Person Partner;
   }

   static class Option_Match_Example2
   {
      internal static void _main()
      {
         Person grace = new Person { Name = "Grace" }
            , dimitry = new Person { Name = "Dimitry" }
            , armin = new Person { Name = "Armin" };

         grace.Relationship = new Relationship {
            Type = "going out", Partner = dimitry };

         WriteLine(grace.RelationshipStatus());
         // prints: Grace is going out with Dimitry

         WriteLine(armin.RelationshipStatus());
         // prints: Armin is single

         ReadKey();
      }

      static string RelationshipStatus(this Person p)
         => p.Relationship.Match(
            Some: r => $"{p.Name} is {r.Type} with {r.Partner.Name}",
            None: () => $"{p.Name} is single");
   }
}
