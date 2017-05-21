using NUnit.Framework;
using System;

using LaYumba.Functional;
using LaYumba.Functional.Data.LinkedList;
using static LaYumba.Functional.Data.LinkedList.LinkedList;

using LaYumba.Functional.Data.BinaryTree;
using static LaYumba.Functional.Data.BinaryTree.Tree;

namespace Exercises.Chapter9.Solutions
{
   static class Solutions
   {
      // LISTS: implement functions to work with the singly linked List defined in this chapter:

      // InsertAt inserts an item at the given index
      static List<T> InsertAt<T>(this List<T> @this, int m, T value)
         => m == 0 
            ? List(value, @this) 
            : List(@this.Head, @this.Tail.InsertAt(m - 1, value));

      // RemoveAt removes the item at the given index
      static List<T> RemoveAt<T>(this List<T> @this, int m)
         => m == 0 
            ? @this.Tail 
            : List(@this.Head, @this.Tail.RemoveAt(m - 1));

      // TakeWhile takes a predicate, and traverses the list yielding all items until it find one that fails the predicate
      static List<T> TakeWhile<T>(this List<T> @this, Func<T, bool> pred)
         => @this.Match(
            () => @this,
            (head, tail) => pred(head) 
               ? List(head, tail.TakeWhile(pred))
               : List<T>());

      // DropWhile works similarly, but excludes all items at the front of the list
      static List<T> DropWhile<T>(this List<T> @this, Func<T, bool> pred)
         => @this.Match(
            Empty: () => @this,
            Cons: (head, tail) => pred(head)
               ? tail.DropWhile(pred)
               : @this);


      // complexity:
      // InsertAt: O(m) - where m is the index of insertion
      // RemoveAt: O(m) - where m is the index of deletion
      // TakeWhile: O(m) - where m is the lehgth of the resulting list
      // DropWhile: O(m) - where m is the number of elements dropped

      // number of new objects required: same as above, except for DropWhile,
      // which allocates no new objects

      // TakeWhile and DropWhile are useful when working with a list that is sorted 
      // and you’d like to get all items greater/smaller than some value; write implementations 
      // that take an IEnumerable rather than a List
      
      // Answer: see LaYumba.Functional.EnumerableExt


      // Is it possible to define `Bind` for the binary tree implementation shown in this
      // chapter? If so, implement `Bind`:

      static Tree<R> Bind<T, R>(this Tree<T> tree, Func<T, Tree<R>> f)
         => tree.Match(
               Leaf: f,
               Branch: (l, r) => Branch(l.Bind(f), r.Bind(f))
            );

   }

   // Implement a LabelTree type, where each node has a label of type string and a list of subtrees; 
   // this could be used to model a typical navigation tree or a cateory tree in a website

   class LabelTree<T>
   {
      public T Label { get; }
      public List<LabelTree<T>> Children { get; }

      public LabelTree(T label, List<LabelTree<T>> children)
      {
         Label = label;
         Children = children;
      }

      public override string ToString() => $"{Label}: {Children}";
      public override bool Equals(object other) => this.ToString() == other.ToString();
   }

   static class LabelTreeExample
   {
      public static LabelTree<R> Map<T, R>(this LabelTree<T> @this, Func<T, R> f)
         => new LabelTree<R>(f(@this.Label), @this.Children.Map(tree => tree.Map(f))); 

      static LabelTree<string> Tree(string label, List<LabelTree<string>> children = null)
         => new LabelTree<string>(label, children ?? List<LabelTree<string>>());

      [Test] public static void LabelTreeLocalize()
      {
         // a category tree
         var tree = Tree("root", List(
            Tree("footwear", List(
               Tree("shoes_cycling"),
               Tree("overshoes")               
            )),
            Tree("accessories", List(
               Tree("pumps"),
               Tree("sunglasses")               
            ))
         ));

         // mapping of keys to translations in German
         var localizations = new System.Collections.Generic.Dictionary<string, string> 
         {
            ["root"] = "Alle Kategorien",
            ["footwear"] = "Shuhe",
            ["shoes_cycling"] = "Fahrradschuhe",
            ["overshoes"] = "Schuhüberzüge",
            ["accessories"] = "Zubehör",
            ["pumps"] = "Pumpen",
            ["sunglasses"] = "Sonnenbrillen",            
         };

         // localize the category tree
         var localizedTree = tree.Map(s => localizations[s]);
         
         // expected result of Map
         var expected = Tree("Alle Kategorien", List(
            Tree("Shuhe", List(
               Tree("Fahrradschuhe"),
               Tree("Schuhüberzüge")               
            )),
            Tree("Zubehör", List(
               Tree("Pumpen"),
               Tree("Sonnenbrillen")               
            ))
         ));

         Assert.AreEqual(expected, localizedTree);
      }
   } 
}
