using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Exercises.Chapter1
{
   static class Solutions
   {
      // 1. 
      static Func<T, bool> Negate<T>(this Func<T, bool> pred) 
         => t => !pred(t);

      // 2.
      static List<int> QuickSort(this List<int> list)
      {
         if (list.Count == 0) return new List<int>();

         var pivot = list[0];
         var rest = list.Skip(1);

         var small = from item in rest where item <= pivot select item;
         var large = from item in rest where pivot < item select item;

         return small.ToList().QuickSort()
            .Append(pivot)
            .Concat(large.ToList().QuickSort())
            .ToList();
      }

      // a more terse solution, using helper methods that will be discussed later in the book
      static List<int> QSort(this List<int> list)
         => list.Match(
               () => List<int>(),
               (pivot, rest) => rest.Where(i => i <= pivot).ToList().QSort()
                  .Append(pivot)
                  .Concat(rest.Where(i => pivot < i).ToList().QSort())
            ).ToList();

      [Test]
      public static void TestQuickSort()
      {
         var list = new List<int> {-100, 63, 30, 45, 1, 1000, -23, -67, 1, 2, 56, 75, 975, 432, -600, 193, 85, 12};
         var expected = new List<int> {-600, -100, -67, -23, 1, 1, 2, 12, 30, 45, 56, 63, 75, 85, 193, 432, 975, 1000};
         var actual = list.QuickSort();
         Assert.AreEqual(expected, actual);
      }

      [Test]
      public static void TestQSort()
      {
         var list = new List<int> {-100, 63, 30, 45, 1, 1000, -23, -67, 1, 2, 56, 75, 975, 432, -600, 193, 85, 12};
         var expected = new List<int> {-600, -100, -67, -23, 1, 1, 2, 12, 30, 45, 56, 63, 75, 85, 193, 432, 975, 1000};
         var actual = list.QSort();
         Assert.AreEqual(expected, actual);
      }

      // 3.
      static List<T> QuickSort<T>(this List<T> list, Comparison<T> compare)
      {
         if (list.Count == 0) return new List<T>();

         var pivot = list[0];
         var rest = list.Skip(1);

         var small = from item in rest where compare(item, pivot) <= 0 select item;
         var large = from item in rest where 0 < compare(item, pivot) select item;

         return small.ToList().QuickSort(compare)
            .Concat(new List<T> { pivot })
            .Concat(large.ToList().QuickSort(compare))
            .ToList();
      }

      // 4.
      static R Using<TDisp, R>(Func<TDisp> createDisposable
         , Func<TDisp, R> func) where TDisp : IDisposable
      {
         using (var disp = createDisposable()) return func(disp);
      }
   }
}
