using System;
using LaYumba.Functional;

namespace Examples.ReaderEx
{
   using static Console;

   // using the custom delegate Reader<Env, T> to indicate a function 
   // that "reads" an Env and computes a T
   public static class Reader_Example
   {
      public static Reader<string, string> FirstThing()
         => from name in Reader.Ask<string>()
            select $"First, {name} looked up";

      public static Reader<string, string> SecondThing()
         => from name in Reader.Ask<string>()
            select $"Then, {name} turned to me...";

      internal static void _main()
      {
         var reader = from first in FirstThing()
                      from second in SecondThing()
                      select first + "\n" + second;

         var story = reader("Tamerlano");

         WriteLine(story);
      }
   }

   // same, but just using Func<Env, T>
   public static class Func_As_Reader_Example
   {
      internal static void _main()
      {
         Func<string, string> firstThing
            = name => $"First, {name} looked up";

         Func<string, string> secondThing
            = name => $"Then, {name} turned to me...";

         var reader = from first in firstThing
                      from second in secondThing
                      select $"{first}\n{second}";

         // same as above, but using explicit arguments, instead of LINQ
         var reader1 = firstThing
            .Bind(first => secondThing
               .Map(second => $"{first}\n{second}"));


         var story = reader("Tamerlano");
         // => First, Tamerlano looked up
         //    Then, Tamerlano turned to me...

         WriteLine(story);
      }
   }
}