using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using LaYumba.Functional;

using System.Reflection;
using Microsoft.AspNetCore;

namespace Examples
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var cliExamples = new Dictionary<string, Action>
         {
            ["HOFs"] = Chapter1.HOFs.Run,
            ["Greetings"] = Chapter7.Greetings.Run,
            ["Timer"] = Chapter14.CreatingObservables.Timer.Run,
            ["Subjects"] = Chapter14.CreatingObservables.Subjects.Run,
            ["Create"] = Chapter14.CreatingObservables.Create.Run,
            ["Generate"] = Chapter14.CreatingObservables.Generate.Run,
            ["CurrencyLookup_Unsafe"] = Chapter14.CurrencyLookup_Unsafe.Run,
            ["CurrencyLookup_Safe"] = Chapter14.CurrencyLookup_Safe.Run,
            ["VoidContinuations"] = Chapter14.VoidContinuations.Run,
            ["KeySequences"] = Chapter14.KeySequences.Run,
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
         var host = WebHost.CreateDefaultBuilder()
            .UseStartup<Boc.Startup>()
            .Build();

         host.Run();
      }
   }
}
