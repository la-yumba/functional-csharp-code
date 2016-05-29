using System;
using System.Collections.Generic;

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

      public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
         => IsLeft ? Left(this.Left) : Right(this.Right);

      public Unit Match(Action<L> Left, Action<R> Right)
         => Match(Left.ToFunc(), Right.ToFunc());

      public IEnumerator<R> AsEnumerable()
      {
         if (IsRight) yield return Right;
      }
   }

   public static class Either
   {
      public static Either<L, RR> Map<L, R, RR>(this Either<L, R> @this
         , Func<R, RR> func) => @this.IsRight ? func(@this.Right) : new Either<L, RR>(@this.Left);

      public static Either<L, Unit> ForEach<L, R>(this Either<L, R> @this, Action<R> act)
         => Map(@this, act.ToFunc());

      public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> @this
         , Func<R, Either<L, RR>> func)
          => @this.IsRight ? func(@this.Right) : new Either<L, RR>(@this.Left);

      // LINQ

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
