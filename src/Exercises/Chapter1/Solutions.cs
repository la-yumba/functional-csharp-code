using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercises.Chapter1
{
   static class Solutions
   {
      // 2.
      static List<int> QuickSort(this List<int> list)
      {
         if (list.Count == 0) return new List<int>();
         var pivot = list[0];
         var small = from item in list where item <= pivot select item;
         var large = from item in list where pivot < item select item;
         return small.ToList().QuickSort()
            .Concat(large.ToList().QuickSort())
            .ToList();
      }

      // 3.
      static List<T> QuickSort<T>(this List<T> list, Comparison<T> compare)
      {
         if (list.Count == 0) return new List<T>();
         var pivot = list[0];
         var small = from item in list where compare(item, pivot) <= 0 select item;
         var large = from item in list where 0 < compare(item, pivot) select item;
         return small.ToList().QuickSort(compare)
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
