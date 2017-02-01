using LaYumba.Functional;
using LaYumba.Functional.Data.LinkedList;
using static LaYumba.Functional.Data.LinkedList.LinkedList;

using NUnit.Framework;
using System;

namespace Exercises.Chapter9
{
   static class Exercises
   {
      // LISTS: implement functions to work with the singly linked List defined in this chapter:
      // Tip: start by writing the function signature in arrow-notation

      // InsertAt inserts an item at the given index

      // RemoveAt removes the item at the given index

      // TakeWhile takes a predicate, and traverses the list yielding all items until it find one that fails the predicate

      // DropWhile works similarly, but excludes all items at the front of the list


      // complexity:
      // InsertAt: 
      // RemoveAt: 
      // TakeWhile: 
      // DropWhile: 

      // number of new objects required: 

      // TakeWhile and DropWhile are useful when working with a list that is sorted 
      // and you’d like to get all items greater/smaller than some value; write implementations 
      // that take an IEnumerable rather than a List


      // TREES: Implement a LabelTree type, where each node has a label of type string and a list of subtrees; 
      // this could be used to model a typical navigation tree or a cateory tree in a website

      // Imagine you need to add localization to your navigation tree: you're given a `LabelTree` where
      // the value of each label is a key, and a dictionary that maps keys
      // to translations in one of the languages that your site must support
      // (hint: define `Map` for `LabelTree` and use it to obtain the localized navigation/category tree)

   }
}