using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using LaYumba.Functional;

//using NUnit.Common;
//using NUnit.Framework;
//using NUnitLite;
using System.Reflection;

namespace Examples
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var cliExamples = new Dictionary<string, Action>
         {
            //["test"] = () => new AutoRun(typeof(Program).GetTypeInfo().Assembly)
            //    .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In),
            ["HOFs"] = Chapter1.HOFs.Run,
            ["Greetings"] = Chapter7.Greetings.Run,
            ["Timer"] = Chapter13.CreatingObservables.Timer.Run,
            ["Subjects"] = Chapter13.CreatingObservables.Subjects.Run,
            ["Create"] = Chapter13.CreatingObservables.Create.Run,
            ["Generate"] = Chapter13.CreatingObservables.Generate.Run,
            ["CurrencyLookup_Unsafe"] = Chapter13.CurrencyLookup_Unsafe.Run,
            ["CurrencyLookup_Safe"] = Chapter13.CurrencyLookup_Safe.Run,
            ["VoidContinuations"] = Chapter13.VoidContinuations.Run,
            ["KeySequences"] = Chapter13.KeySequences.Run,
         };

         if (args.Length > 0)
            cliExamples.Lookup(args[0])
               .Match(
                  None: () => WriteLine($"Unknown option: '{args[0]}'"),
                  Some: (main) => main()
               );

         else StartWebApi();
      }

      static void StartWebApi()
      { 
         var host = new WebHostBuilder()
             .UseKestrel()
             .UseContentRoot(Directory.GetCurrentDirectory())
             .UseIISIntegration()
             .UseStartup<Boc.Startup>()
             .Build();

         host.Run();
      }
   }
}
