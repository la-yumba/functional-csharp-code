using System;
using Playground.Free;

namespace Playground
{
   using static Console;

   public class Program
   {
      public static void Main(string[] args)
      {
         WriteLine(Interactive.AgeWorkflow.InterpretWith(Free.ToString.Interpret));
         WriteLine(Interactive.NameWorkflow.InterpretWith(Free.ToString.Interpret));
         //Interactive.AgeWorkflow.InterpretWith(Free.Interpreter.Interpret);
      }
   }
}
