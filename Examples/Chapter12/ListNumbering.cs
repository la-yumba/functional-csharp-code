using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;

namespace Examples.ReaderEx
{
   using static Enumerable;
   using static F;
   using static Console;
   using static StatefulComputation;

   public class State_Number_List
   {
      internal static void _main()
      {
         var list = List('a', 'b', 'c', 'd');
         
         WriteLine("recursively number list:");

         var numberedList = new State_Number_List().NumberList(list)(0).Item1;
         numberedList.ForEach(WriteLine);

         WriteLine();
         WriteLine("Using Zip");
         var altNumberedList = list.Zip(Naturals(), Numbered<char>.Create);
         altNumberedList.ForEach(WriteLine);

         ReadKey();
      }

      private static StatefulComputation<int, Numbered<T>> NumberItem<T>(T item)
         => count => (new Numbered<T>(item, count), count + 1);
      
      // alternatively
      private static StatefulComputation<int, Numbered<T>> NumberItemLINQ<T>(T item)
         => from count in Get<int>()
            from _     in Put(count + 1)
            select new Numbered<T>(item, count);

      StatefulComputation<int, IEnumerable<Numbered<T>>> NumberList<T>(IEnumerable<T> list)
         => list.Match(
            Empty: () => StatefulComputation<int>.Return(Empty<Numbered<T>>()),
            Otherwise: (x, xs) => from head in NumberItem(x)
                                  from tail in NumberList(xs)
                                  select List(head).Concat(tail));

      static IEnumerable<int> Naturals()
      {
         int i = 0;
         while (true) yield return i++;
      } 
   }
}