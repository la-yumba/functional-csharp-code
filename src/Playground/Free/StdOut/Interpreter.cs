using System;

namespace Playground.Free
{
   using static Console;

   public static class Interpreter
   {
      public static T Interpret<T>(Free<T> @this)
         => @this.Match(
            Done: t => t,
            More: Interpret
         );

      static T Interpret<T>(StdOut<Free<T>> op)
         => op.Match(
            Ask: (prompt, next) =>
            {
               WriteLine(prompt);
               var s = ReadLine();
               return Interpret(next(s));
            },
            Tell: (message, result) =>
            {
               WriteLine(message);
               return Interpret(result);
            });
   }

   public static class ToString
   {
      static int count;

      public static string Interpret<T>(Free<T> @this)
         => @this.Match(
            Done: t => $"Done({t})",
            More: op => $"More({Interpret(op)})"
         );

      static string Interpret<T>(StdOut<Free<T>> op)
         => op.Match(
            Ask: (prompt, next) => $"Ask('{prompt}', next : s{++count} => {Interpret(next($"s{count}"))})",
            Tell: (message, result) => $"Tell('{message}', {Interpret(result)})"
         );
   }
}
