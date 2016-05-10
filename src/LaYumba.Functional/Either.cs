using System;
using System.Collections.Generic;
using static LaYumba.Functional.F;

namespace LaYumba.Functional
{
   public struct Either<L, R>
   {
      internal L Left { get; }
      internal R Right { get; }

      public bool IsRight { get; }
      public bool IsLeft => !IsRight;

      internal Either(L left)
      {
         IsRight = false;
         Left = left;
         Right = default(R);
      }

      internal Either(R right)
      {
         IsRight = true;
         Right = right;
         Left = default(L);
      }

      public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
      public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

      public TResult Match<TResult>(Func<R, TResult> Right, Func<L, TResult> Left)
          => IsRight ? Right(this.Right) : Left(this.Left);

      public Unit Match(Action<R> isRight, Action<L> isLeft)
         => IsRight ? isRight.ToFunc()(this.Right) : isLeft.ToFunc()(this.Left);

      public Either<L, TResult> Map<TResult>(Func<R, TResult> func)
          => IsRight ? func(this.Right) : new Either<L, TResult>(this.Left);

      public Either<L, Unit> ForEach(Action<R> act)
      {
         if (IsLeft) { return Left; }
         else { act(Right); return Unit(); }
      }

      public Either<L, TResult> Bind<TResult>(Func<R, Either<L, TResult>> func)
          => IsRight ? func(this.Right) : new Either<L, TResult>(this.Left);

      public IEnumerator<R> AsEnumerable()
      {
         if (IsRight) yield return Right;
      }
   }

   public static class Either
   {

      //    /// <summary>
      //    /// Monadic bind function
      //    /// https://en.wikipedia.org/wiki/Monad_(functional_programming)
      //    /// </summary>
      //    /// <typeparam name="L">Left</typeparam>
      //    /// <typeparam name="R">Right</typeparam>
      //    /// <typeparam name="Ret"></typeparam>
      //    /// <param name="self"></param>
      //    /// <param name="binder"></param>
      //    /// <returns>Bound Either</returns>
      //    public static Either<L, Ret> Bind<L, R, Ret>(this Either<L, R> self, Func<R, Either<L, Ret>> binder) =>
      //        self.IsBottom
      //            ? new Either<L, Ret>(true)
      //            : self.IsRight
      //                ? binder(self.RightValue)
      //                : Either<L, Ret>.Left(self.LeftValue);

      //    /// <summary>
      //    /// Filter the Either
      //    /// This may give unpredictable results for a filtered value.  The Either won't
      //    /// return true for IsLeft or IsRight.  IsBottom is True if the value is filterd and that
      //    /// should be checked.
      //    /// </summary>
      //    /// <typeparam name="L">Left</typeparam>
      //    /// <typeparam name="R">Right</typeparam>
      //    /// <param name="self">Either to filter</param>
      //    /// <param name="pred">Predicate function</param>
      //    /// <returns>If the Either is in the Left state it is returned as-is.  
      //    /// If in the Right state the predicate is applied to the Right value.
      //    /// If the predicate returns True the Either is returned as-is.
      //    /// If the predicate returns False the Either is returned in a 'Bottom' state.  IsLeft will return True, but the value 
      //    /// of Left = default(L)</returns>
      //    public static Either<L, R> Filter<L, R>(this Either<L, R> self, Func<R, bool> pred) =>
      //        self.IsBottom
      //            ? self
      //            : match(self,
      //                Right: t => pred(t) ? Either<L, R>.Right(t) : new Either<L, R>(true),
      //                Left: l => Either<L, R>.Left(l));

      public static Either<L, R> Select<L, T, R>(this Either<L, T> @this
         , Func<T, R> map) => @this.Map(map);

      public static Either<L, RR> SelectMany<L, T, R, RR>(this Either<L, T> @this
         , Func<T, Either<L, R>> bind, Func<T, R, RR> project)
      {
         if (@this.IsLeft) return new Either<L, RR>(@this.Left);
         var bound = bind(@this.Right);
         return bound.IsLeft 
            ? new Either<L, RR>(bound.Left) 
            : project(@this.Right, bound.Right);
      }
   }
}
