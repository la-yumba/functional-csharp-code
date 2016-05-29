using System;
using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace TestRunner
{
   public class Program
   {
      public static void Main(string[] args)
      {
         Func<Type, int> runTestsInAssemlyOf = t => new AutoRun(t.GetTypeInfo().Assembly)
                .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);

         var result
            = runTestsInAssemlyOf(typeof(LaYumba.Functional.Tests.Unit_Test))
            + runTestsInAssemlyOf(typeof(Boc.Program));

         Console.WriteLine($"status: {result}");
         Console.WriteLine(result > 0 ? "ERROR: failing tests" : "SUCCESS: all tests pass");
      }
   }
}
