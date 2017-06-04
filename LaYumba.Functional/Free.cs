using System;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   // free case classes
   public abstract class Free<T>
   {
      public abstract R Match<R>(Func<T, R> Done
         , Func<Coyo<object, Free<T>>, R> More);
   }

   // aka Return
   internal class Done<T> : Free<T>
   {
      public T Value { get; }

      public Done(T value) { Value = value; }

      public override R Match<R>(Func<T, R> Done
         , Func<Coyo<object, Free<T>>, R> More) => Done(Value);
   }

   // aka Bind, Suspend
   internal class More<T> : Free<T>
   {
      public Coyo<object, Free<T>> Next { get; }

      public More(Coyo<object, Free<T>> next) { Next = next; }

      public override R Match<R>(Func<T, R> Done
         , Func<Coyo<object, Free<T>>, R> More) => More(Next);
   }

   public static class Free
   {
      // Lifts a T into a Free<T> that has the given T as its inner value
      public static Free<T> Return<T>(T t) => Done(t);

      // Runs a Free<T> with the given interpreter, returning a T
      public static T Run<T>
         (this Free<T> free, Func<object, object> interpret)
         => free.Match(
            Done: t => t,
            More: coyo => Run(coyo.Func(interpret(coyo.Value)), interpret)
         );

      // Runs a Free<T> with the given asynchronous interpreter, returning a Task<T>
      public static Task<T> Run<T>
         (this Free<T> free, Func<object, Task<object>> interpret)
         => free.Match<Task<T>>(
            Done: t => Async(t),
            More: async coyo => await Run(coyo.Func(await interpret(coyo.Value)), interpret)
         );

      /// Lifts a single instruction into a program.
      /// Given an instruction such as: 
      /// 
      /// Ask( Prompt: "What's your age?" )
      /// 
      /// we get a Coyo such as
      /// 
      /// Coyo(
      ///    Value: Ask ( Prompt: "What's your age?" ),
      ///    Func: age => age // where `age` is the value that will be returned by running the Ask
      /// )
      /// 
      /// and then a program such as:
      ///
      /// More(
      ///    Coyo(
      ///       Value: Ask( Prompt: "What's your age?" ),
      ///       Func: age => Done(
      ///          Value: age
      ///       )
      ///    )
      /// )
      public static Free<T> Of<T>(object op)
         => More(Coyo.Of<object, T>(op).Map(t => Done<T>(t)));

      static Free<T> Done<T>(T t) => new Done<T>(t);
      static Free<T> More<T>(Coyo<object, Free<T>> t) => new More<T>(t);

      // LINQ operators to be able to use Free with monadic syntax
      // Note that in order to make Free a monad we _only_ rely on `Coyo.Map`
      // This is the meaning of "free" monad: just based on Coyo being a functor,
      // we can define the monad for Free

      // this is like a recursive function traversing the list until `f` makes it to the end (Done)
      public static Free<R> Select<T, R>(this Free<T> @this
         , Func<T, R> func)
         => @this.Match(
            Done: t => Done<R>(func(t)),
            More: op => More(op.Map(free => free.Select(func)))
         );

      public static Free<R> SelectMany<T, R>(this Free<T> @this
         , Func<T, Free<R>> func)
         => @this.Match(
            Done: func,
            More: op => More(op.Map(free => free.SelectMany(func)))
         );

      public static Free<RR> SelectMany<T, R, RR>(this Free<T> @this
         , Func<T, Free<R>> bind, Func<T, R, RR> project)
         => SelectMany(@this, t => Select(bind(t), r => project(t, r)));
   }    
}