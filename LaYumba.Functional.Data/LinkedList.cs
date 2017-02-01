using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;

namespace LaYumba.Functional.Data.LinkedList
{
   using static F;

   // List<T> = Empty | Otherwise(T, List<T>)

   public sealed class List<T>
   {
      readonly bool isEmpty;
      readonly T head;
      readonly List<T> tail;

      // the empty list
      internal List() { isEmpty = true; }

      // the non empty list
      internal List(T head, List<T> tail)
      {
         this.head = head;
         this.tail = tail;
      }

      public R Match<R>(Func<R> Empty, Func<T, List<T>, R> Cons)
         => isEmpty ? Empty() : Cons(head, tail);

      // for convenience
      public T Head => Match(
         () => { throw new IndexOutOfRangeException(); },
         (head, _) => head);

      public List<T> Tail => Match(
         () => { throw new IndexOutOfRangeException(); },
         (_, tail) => tail);

      // not really required, but hey...
      public T this[int index] => Match(
         () => { throw new IndexOutOfRangeException(); },
         (head, tail) => index == 0 ? head : tail[index - 1]);
          
      public IEnumerable<T> AsEnumerable()
      {
         if (isEmpty) yield break;
         yield return head;
         foreach (T t in tail.AsEnumerable()) yield return t;
      }

      public override string ToString() => Match(
         () => "{ }",
         (_, __) => $"{{ {string.Join(", ", AsEnumerable().Map(v => v.ToString()))} }}");
   }

   public static class LinkedList
   {
      // factory functions
      public static List<T> List<T>(T h, List<T> t) => new List<T>(h, t);

      public static List<T> List<T>(params T[] items)
         => items.Reverse().Aggregate(new List<T>()
            , (tail, head) => List(head, tail));

      public static int Length<T>(this List<T> @this) => @this.Match(
         () => 0,
         (t, ts) => 1 + ts.Length());

      public static List<T> Add<T>(this List<T> @this, T value)
         => List(value, @this);

      public static List<T> Append<T>(this List<T> @this, T value)
         => @this.Match(
            () => List(value, List<T>()),
            (head, tail) => List(head, tail.Append(value)));

      public static List<T> InsertAt<T>(this List<T> @this, int m, T value)
         => m == 0 ? List(value, @this) : @this.Match(
            () => { throw new IndexOutOfRangeException(); },
            (head, tail) => List(head, tail.InsertAt(m - 1, value)));

      public static List<R> Map<T, R>(this List<T> @this, Func<T, R> f)
         => @this.Match(
            () => List<R>(),
            (head, tail) => List(f(head), tail.Map(f)));

      public static Unit ForEach<T>(this List<T> @this, Action<T> action)
      {
         @this.Map(action.ToFunc());
         return Unit();
      }

      public static List<R> Bind<T, R>(this List<T> @this, Func<T, List<R>> f)
         => @this.Map(f).Join();

      public static List<T> Join<T>(this List<List<T>> @this) => @this.Match(
         () => List<T>(),
         (xs, xss) => concat(xs, Join(xss)));

      public static Acc Aggregate<T, Acc>(this List<T> @this, Acc acc, Func<Acc, T, Acc> f)
         => @this.Match(
            () => acc,
            (head, tail) => Aggregate(tail, f(acc, head), f));

      static List<T> concat<T>(List<T> l, List<T> r) => l.Match(
         () => r,
         (h, t) => List(h, concat(t, r)));

      public static List<R> Run<T, R>(this Coyo<List<T>, R> @this) 
         => @this.Value.Map(t => @this.Func(t));
   }
}
