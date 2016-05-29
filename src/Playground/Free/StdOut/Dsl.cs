using System;

namespace Playground.Free
{
   public abstract class StdOut<T>
   {
      public abstract R Match<R>(Func<string, Func<string, T>, R> Ask, Func<string, T, R> Tell);

      internal class Tell : StdOut<T>
      {
         public string Message { get; }
         public T Result { get; }

         public Tell(string message, T result)
         {
            Message = message;
            Result = result;
         }

         public override R Match<R>(Func<string, Func<string, T>, R> Ask
            , Func<string, T, R> Tell) => Tell(Message, Result);
      }
      
      internal class Ask : StdOut<T>
      {
         internal string Prompt;
         internal Func<string, T> Next;

         public Ask(string prompt, Func<string, T> next)
         {
            Prompt = prompt;
            Next = next;
         }

         public override R Match<R>(Func<string, Func<string, T>, R> Ask
            , Func<string, T, R> Tell) => Ask(Prompt, Next);
      }
   }

   public static class StdOut
   {
      // factory methods to create a StdOut

      static StdOut<T> Ask<T>(string prompt, Func<string, T> func)
         => new StdOut<T>.Ask(prompt, func);

      static StdOut<T> Tell<T>(string message, T result)
         => new StdOut<T>.Tell(message, result);

      // The `StdOut` data type is a functor.
      // This is all that is necessary to provide the grammar (`FreeStdOut`).
      // Note that `FreeStdOut` uses only `Select`
      // (and no other `StdOut` methods) to implement `SelectMany`
      public static StdOut<R> Map<T, R>(this StdOut<T> @this
      , Func<T, R> func)
      => @this.Match(
         Ask: (prompt, next) => Ask(prompt, s => func(next(s))),
         Tell: (message, result) => Tell(message, func(result)));
   }
}
