using System;
using System.Collections.Generic;
using System.Linq;

namespace LaYumba.Functional
{
   using static F;

   public struct Option<T> : IEquatable<NoneType>, IEquatable<Option<T>>
   {
      public static readonly Option<T> None = new Option<T>();

      internal T Value { get; }
      public bool IsSome { get; }
      public bool IsNone => !IsSome;

      internal Option(T value, bool isSome)
      {
         IsSome = isSome;
         Value = value;
      }

      public static implicit operator Option<T>(T value) => Some(value);
      public static implicit operator Option<T>(NoneType _) => None;
      
      public R Match<R>(Func<T, R> Some, Func<R> None)
          => IsSome ? Some(Value) : None();

      public IEnumerable<T> AsEnumerable()
      {
         if (IsSome) yield return Value;
      }

      public bool Equals(Option<T> other) 
         => this.IsSome == other.IsSome 
         && (this.IsNone || this.Value.Equals(other.Value));

      public bool Equals(NoneType other) => IsNone;

      public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
      public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

      public override string ToString() => IsSome ? $"Some({Value})" : "None";
   }

   public static class Option
   {
      public static Option<T> Of<T>(T value)
         => new Option<T>(value, value != null);
      
      public static Option<R> Apply<T, R>(this Option<Func<T, R>> opt, Option<T> arg)
         => opt.IsSome && arg.IsSome
            ? Some(opt.Value(arg.Value))
            : None;

      public static Option<Func<T2, R>> Apply<T1, T2, R>(this Option<Func<T1, T2, R>> opt, Option<T1> arg)
         => opt.IsSome && arg.IsSome
            ? Some(opt.Value.Curry()(arg.Value))
            : None;

      public static Option<R> Bind<T, R>(this Option<T> @this, Func<T, Option<R>> func)
          => @this.IsSome
              ? func(@this.Value)
              : None;

      public static IEnumerable<R> Bind<T, R>(this Option<T> @this
         , Func<T, IEnumerable<R>> func)
          => @this.AsEnumerable().Bind(func);

      public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
         => Map(@this, action.ToFunc());

      public static T GetOrElse<T>(this Option<T> opt, T defaultValue) 
         => opt.Match(
            Some: value => value,
            None: () => defaultValue);

      public static T GetOrElse<T>(this Option<T> @this, Func<T> fallback) 
         => @this.Match(
            Some: value => value,
            None: fallback);

      public static Option<R> Map<T, R>(this Option<T> @this, Func<T, R> func)
          => @this.IsSome
              ? Some(func(@this.Value))
              : None;

      public static Option<Func<T2, R>> Map<T1, T2, R>(this Option<T1> opt, Func<T1, T2, R> func)
          => opt.Map(func.Curry());

      public static Option<Func<T2, Func<T3, R>>> Map<T1, T2, T3, R>(this Option<T1> opt, Func<T1, T2, T3, R> func)
          => opt.Map(func.Curry());

      // applicative traverse
      public static Option<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> func)
         => list.Match(
            Empty: () => Some(Enumerable.Empty<R>()),
            Otherwise: (t, ts) => Some(Cons<R>())
               .Apply(func(t))
               .Apply(ts.TraverseA(func)));

      // monadic traverse
      public static Option<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> func)
         => list.Match(
            Empty: () => Some(Enumerable.Empty<R>()),
            Otherwise: (t, ts) => from r  in func(t)
                                  from rs in TraverseM(ts, func)
                                  select List(r).Concat(rs));

      // non-recursive monadic traverse
      public static Option<IEnumerable<R>> Traverse<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> func)
         => list.Aggregate(
            seed: Some(Enumerable.Empty<R>()), 
            func: (optRs, t) => from rs in optRs
                                from r in func(t)
                                select rs.Append(r));
      // LINQ

      public static Option<R> Select<T, R>(this Option<T> opt, Func<T, R> func)
          => opt.Map(func);

      public static Option<T> Where<T>(this Option<T> opt, Func<T, bool> predicate)
          => opt.IsSome && predicate(opt.Value) ? opt : None;

      public static Option<RR> SelectMany<T, R, RR>(this Option<T> @this
          , Func<T, Option<R>> bind, Func<T, R, RR> project)
      {
         var bound = @this.Bind(bind);
         return bound.IsSome
            ? Some(project(@this.Value, bound.Value))
            : None;
      }
   }
}
