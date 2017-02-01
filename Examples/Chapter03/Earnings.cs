using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;
using NUnit.Framework;

namespace Examples.Chapter3
{
   using static F;
   
   public static class PopulationStatistics
   {
      public static decimal AverageEarnings(this IEnumerable<Person> population)
         => population
            .Average(p => p.Earnings);

      public static IEnumerable<Person> RichestQuartile(this List<Person> population)
         => population
            .OrderByDescending(p => p.Earnings)
            .Take(population.Count / 4);

      public static decimal AverageEarningsOfRichestQuartile(List<Person> population)
         => population
            .OrderByDescending(p => p.Earnings)
            .Take(population.Count / 4)
            .Select(p => p.Earnings)
            .Average();
   }

   
   public class TestEarnings
   {
      [TestCase(ExpectedResult = 75000)]
      public decimal AverageEarningsOfRichestQuartile()
         => SamplePopulation
            .RichestQuartile()
            .AverageEarnings();

      private static List<Person> SamplePopulation
         => Range(1, 8).Map(i => new Person { Earnings = i * 10000 }).ToList();

      [TestCase(ExpectedResult = 75000)]
      public decimal AverageEarningsOfRichestQuartile_()
      {
         var population = Range(1, 8)
            .Select(i => new Person { Earnings = i * 10000 })
            .ToList();

         return PopulationStatistics
            .AverageEarningsOfRichestQuartile(population);
      }
   }
}