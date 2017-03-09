using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
   using System.Threading.Tasks;
   using static F;

   public static partial class F
   {
      // The None value
      public static Option.None None => Option.None.Default;

      // Wraps the given value into a Some
      public static Option<T> Some<T>(T value) => new Option.Some<T>(value);

      // NOTE: in the book text, the Some function appears as:
      //
      // public static Option.Some<T> Some<T>(T value) => new Option.Some<T>(value);
      //
      // that is, it returns a Some, whereas in the implementation given here it returns an Option.
      // The reason for the implementation as it appears in the book is that I want to emphasize
      // that None and Some(T) are different types. Hence the Some function should construct a Some.
      // So, on a theoretical level, this version is more consistent with the way I present
      // my approach to modelling Option in a language that doesn't have "sum types".
      //
      // Naturally, according to the definition:
      //
      //   Option<T> = Some(T) | None
      //
      // both are (implicitly convertible to) an Option<T>.
      //
      // The implementation that appears here performs this conversion immediately,
      // since when given a T you can create an Option<T> directly - unlike with None,
      // where you don't yet know the type of T.
      //
      // This has the advantage that (among other things) you can write stuff like:
      //
      // public static Option<DateTime> Parse(string s)
      //    => DateTime.TryParse(s, out DateTime d) ? Some(d) : None;
      //
      // If Some returns an Option, the above compiles (since None is convertible to Option).
      // If Some returns a Some, the above fails (since C# cannot figure out that it should
      // convert both the Some and the None to an Option), and you would have to add noise by
      // explicitly converting one of the two values to Option.
      // So, on a purely pragmatic level, the implementation given here is somewhat preferable.
   }

   public struct Option<T> : IEquatable<Option.None>, IEquatable<Option<T>>
   {
      readonly T value;
      readonly bool isSome;
      bool isNone => !isSome;

      private Option(T value)
      {
         if (value == null)
            throw new ArgumentNullException();
         this.isSome = true;
         this.value = value;
      }

      public static implicit operator Option<T>(Option.None _) => new Option<T>();
      public static implicit operator Option<T>(Option.Some<T> some) => new Option<T>(some.Value);

      public static implicit operator Option<T>(T value)
         => value == null ? None : Some(value);

      public R Match<R>(Func<R> None, Func<T, R> Some)
          => isSome ? Some(value) : None();

      public IEnumerable<T> AsEnumerable()
      {
         if (isSome) yield return value;
      }

      public bool Equals(Option<T> other)
         => this.isSome == other.isSome
         && (this.isNone || this.value.Equals(other.value));

      public bool Equals(Option.None _) => isNone;

      public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
      public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

      public override string ToString() => isSome ? $"Some({value})" : "None";
   }

   namespace Option
   {
      public struct None
      {
         internal static readonly None Default = new None();
      }

      public struct Some<T>
      {
         internal T Value { get; }

         internal Some(T value)
         {
            if (value == null)
               throw new ArgumentNullException(nameof(value)
                  , "Cannot wrap a null value in a 'Some'; use 'None' instead");
            Value = value;
         }
      }
   }

   public static class OptionExt
   {
      public static Option<R> Apply<T, R>
         (this Option<Func<T, R>> @this, Option<T> arg)
         => @this.Match(
            () => None,
            (func) => arg.Match(
               () => None,
               (val) => Some(func(val))));

      public static Option<Func<T2, R>> Apply<T1, T2, R>
         (this Option<Func<T1, T2, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.Curry), arg);

      public static Option<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Option<Func<T1, T2, T3, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<R> Bind<T, R>
         (this Option<T> optT, Func<T, Option<R>> f)
          => optT.Match(
             () => None,
             (t) => f(t));

      public static IEnumerable<R> Bind<T, R>
         (this Option<T> @this, Func<T, IEnumerable<R>> func)
          => @this.AsEnumerable().Bind(func);

      public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
         => Map(@this, action.ToFunc());

      public static Option<R> Map<T, R>
         (this Option.None _, Func<T, R> f)
         => None;

      public static Option<R> Map<T, R>
         (this Option.Some<T> some, Func<T, R> f)
         => Some(f(some.Value));

      public static Option<R> Map<T, R>
         (this Option<T> optT, Func<T, R> f)
         => optT.Match(
            () => None,
            (t) => Some(f(t)));

      public static Option<Func<T2, R>> Map<T1, T2, R>
         (this Option<T1> @this, Func<T1, T2, R> func)
          => @this.Map(func.Curry());

      public static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>
         (this Option<T1> @this, Func<T1, T2, T3, R> func)
          => @this.Map(func.CurryFirst());

      public static IEnumerable<Option<R>> Traverse<T, R>(this Option<T> @this
         , Func<T, IEnumerable<R>> func)
         => @this.Match(
            () => List((Option<R>)None),
            (t) => func(t).Map(r => Some(r)));

      // utilities

      public static Unit Match<T>(this Option<T> @this, Action None, Action<T> Some)
          => @this.Match(None.ToFunc(), Some.ToFunc());

      internal static bool IsSome<T>(this Option<T> @this)
         => @this.Match(
            () => false,
            (_) => true);

      internal static T ValueUnsafe<T>(this Option<T> @this)
         => @this.Match(
            () => { throw new InvalidOperationException(); },
            (t) => t);

      public static T GetOrElse<T>(this Option<T> opt, T defaultValue)
         => opt.Match(
            () => defaultValue,
            (t) => t);

      public static T GetOrElse<T>(this Option<T> opt, Func<T> fallback)
         => opt.Match(
            () => fallback(),
            (t) => t);

      public static Task<T> GetOrElse<T>(this Option<T> opt, Func<Task<T>> fallback)
         => opt.Match(
            () => fallback(),
            (t) => Async(t));

      public static Option<T> OrElse<T>(this Option<T> left, Option<T> right)
         => left.Match(
            () => right,
            (_) => left);

      public static Option<T> OrElse<T>(this Option<T> left, Func<Option<T>> right)
         => left.Match(
            () => right(),
            (_) => left);


      public static Validation<T> ToValidation<T>(this Option<T> opt, Func<Error> error)
         => opt.Match(
            () => Invalid(error()),
            (t) => Valid(t));

      // LINQ

      public static Option<R> Select<T, R>(this Option<T> @this, Func<T, R> func)
         => @this.Map(func);

      public static Option<T> Where<T>
         (this Option<T> optT, Func<T, bool> predicate)
         => optT.Match(
            () => None,
            (t) => predicate(t) ? optT : None);

      public static Option<RR> SelectMany<T, R, RR>
         (this Option<T> @this, Func<T, Option<R>> bind, Func<T, R, RR> project)
         => @this.Match(
            () => None,
            (t) => @this.Bind(bind).Match(
               () => None,
               (r) => Some(project(t, r))));
   }
}
