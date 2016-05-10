using System;
using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace Exercises
{
   public class Program
   {
      public static void Main(string[] args)
      {
         // run the program you've written...
         Chapter2.Solutions.Bmi._main();

         // ... or run all unit tests
         new AutoRun(typeof(Program).GetTypeInfo().Assembly)
             .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
      }
   }
}
