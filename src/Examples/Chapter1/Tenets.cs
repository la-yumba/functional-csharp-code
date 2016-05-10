using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Examples.Chapter0.Introduction
{
   [TestFixture] class Tenets
   {
      static void _main()
      {
         var ints = new List<int> { 1, 2, 3, 4, 5 };

         foreach (var i in ints)
            Console.WriteLine(i);

         ints.ForEach(Console.WriteLine);
      }

      [Test]
      public void NoInPlaceUpdates()
      {
         var original = new[] { 5, 7, 1 };
         var sorted = original.OrderBy(x => x).ToList();

         Assert.AreEqual(new[] { 5, 7, 1 }, original);
         Assert.AreEqual(new[] { 1, 5, 7 }, sorted);
      }

      [Test]
      public void InPlaceUpdates()
      {
         var original = new List<int> { 5, 7, 1 };
         original.Sort();
         Assert.AreEqual(new[] { 1, 5, 7 }, original);
      }
   }
}