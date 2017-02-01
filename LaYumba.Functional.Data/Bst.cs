using System;

namespace LaYumba.Functional.Data.Bst
{
   using System.Collections.Generic;
   using static Tree;

   public abstract class Tree<T> where T : IComparable<T>
   {
      public abstract R Match<R>(Func<R> Empty, Func<Tree<T>, T, Tree<T>, R> Node);

      public abstract bool IsEmpty { get; }
      public abstract bool Contains(T value);
      public abstract Tree<T> Insert(T value);
      public abstract IEnumerable<T> AsEnumerable();

      public bool Equals(Tree<T> other) => this.ToString() == other.ToString(); // hack
      public override bool Equals(object obj) => Equals((Tree<T>)obj);
   }

   public class Empty<T> : Tree<T> where T : IComparable<T>
   {
      public override R Match<R>(Func<R> Empty, Func<Tree<T>, T, Tree<T>, R> Node)
         => Empty();

      public override bool IsEmpty => true;
      public override bool Contains(T value) => false;
      public override Tree<T> Insert(T value)
         => Node(Empty<T>(), value, Empty<T>());

      public override string ToString() => string.Empty;
      public override IEnumerable<T> AsEnumerable() { yield break; }
   }

   public class Node<T> : Tree<T> where T : IComparable<T>
   {
      public Tree<T> Left { get; }
      public Tree<T> Right { get; }
      public T Value { get; }

      public Node(Tree<T> Left, T Value, Tree<T> Right)
      {
         this.Left = Left;
         this.Right = Right;
         this.Value = Value;
      }

      public override R Match<R>
         (Func<R> Empty, Func<Tree<T>, T, Tree<T>, R> Node)
         => Node(Left, Value, Right);

      public override bool IsEmpty => false;

      public override string ToString()
         => $"Node(Left:({Left}), Value:{Value}, Right:({Right}))";

      public override bool Contains(T value)
      {
         var comparison = value.CompareTo(this.Value);
         if (comparison == 0) return true;
         else if (comparison < 0) return Left.Contains(value);
         else return Right.Contains(value);
      }

      public override Tree<T> Insert(T value)
      {
         var comparison = value.CompareTo(this.Value);
         if (comparison == 0) return this;
         else if (comparison < 0)
            return Node(Left.Insert(value), this.Value, this.Right);
         else return Node(this.Left, this.Value, Right.Insert(value));
      }

      public override IEnumerable<T> AsEnumerable()
      {
         foreach (var item in this.Left.AsEnumerable())
            yield return item;

         yield return this.Value;

         foreach (var item in this.Right.AsEnumerable())
            yield return item;
      }
   }

   public static class Tree
   {
      public static Tree<T> Empty<T>() where T : IComparable<T>
         => new Empty<T>();

      public static Tree<T> Node<T>(Tree<T> Left, T Value, Tree<T> Right)
         where T : IComparable<T>
         => new Node<T>(Left, Value, Right);

      // This implementation looks right but is dangerous, since the resulting
      // tree is not necessarily sorted
      public static Tree<R> Map<T, R>(this Tree<T> tree, Func<T, R> func)
         where T : IComparable<T>
         where R : IComparable<R> 
         => tree.Match(
            Empty: () => Empty<R>(),
            Node: (left, value, right) => Node
               (
                  Left: left.Map(func),
                  Value: func(value),
                  Right: right.Map(func)
               )
         );
   }
}
