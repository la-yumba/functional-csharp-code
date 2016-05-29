using System;
using LaYumba.Functional;

// inspired by Tony Morris https://github.com/tonymorris

namespace Playground.Free
{
   using static F;

   // convenience constructors for creating Free StdOut operations
   public static class FreeStdOutFactory
   {
      public static Free<string> Ask(string prompt)
         => new StdOut<string>.Ask(prompt, s => s).Lift();

      public static Free<Unit> Tell(string message)
         => new StdOut<Unit>.Tell(message, Unit()).Lift();
   }

   // free case classes
   public abstract class Free<T>
   {
      public abstract R Match<R>(Func<T, R> Done
         , Func<StdOut<Free<T>>, R> More);
   }

   // aka Return
   internal class Done<T> : Free<T>
   {
      public T Value { get; }

      public Done(T value) { Value = value; }

      public override R Match<R>(Func<T, R> Done
         , Func<StdOut<Free<T>>, R> More) => Done(Value);
   }

   // aka Bind, Suspend
   internal class More<T> : Free<T>
   {
      public StdOut<Free<T>> Next { get; }

      public More(StdOut<Free<T>> next) { Next = next; }

      public override X Match<X>(Func<T, X> Done
         , Func<StdOut<Free<T>>, X> More) => More(Next);
   }

   public static class FreeStdOut
   {
      public static R InterpretWith<T, R>(this Free<T> @this
         , Func<Free<T>, R> interpreter) => interpreter(@this);

      /// Lift a StdOut into a FreeStdOut
      /// you can think of this as lifting a single instruction into a program.
      /// Given an instruction such as: 
      /// 
      /// Ask(
      ///    Prompt: "What's your age?",
      ///    Next: age => age
      /// )
      /// 
      /// you end up with a program such as:
      ///
      /// More(
      ///    Ask(
      ///       Prompt: "What's your age?",
      ///       Next: age => Done(
      ///          Value: age
      ///       )
      ///    )
      /// )
      internal static Free<T> Lift<T>(this StdOut<T> @this)
         => More(@this.Map(Done));

      static Free<T> Done<T>(T t) => new Done<T>(t);
      static Free<T> More<T>(StdOut<Free<T>> t) => new More<T>(t);

      // LINQ operators to be able to use FreeStdOut with monadic syntax
      // Note that in order to make FreeStdOut a monad we _only_ rely on `StdOut.Map`
      // This is the meaning of "free" monad: as long as StdOut is a functor,
      // we can define the monad FreeStdOut

      public static Free<R> Select<T, R>(this Free<T> @this
         , Func<T, R> func)
         => @this.Match(
            Done: t => Done(func(t)),
            More: op => More(op.Map(free => free.Select(func)))
         );

      public static Free<B> SelectMany<A, B>(this Free<A> @this
         , Func<A, Free<B>> func)
         => @this.Match(
            Done: func,
            More: op => More(op.Map(free => free.SelectMany(func)))
         );

      public static Free<RR> SelectMany<T, R, RR>(this Free<T> @this
         , Func<T, Free<R>> bind, Func<T, R, RR> project)
         => SelectMany(@this, t => Select(bind(t), r => project(t, r)));
   }
}
