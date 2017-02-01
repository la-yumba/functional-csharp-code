using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Examples.Bind
{
   class PetsInNeighbourhood
   {
      internal static void _main_1()
      {
         var neighbours = new[]
         {
            new {Name = "John", Pets = new Pet[] {"Fluffy", "Thor"}},
            new {Name = "Tim", Pets = new Pet[] {}},
            new {Name = "Carl", Pets = new Pet[] {"Sybil"}},
         };

         IEnumerable<IEnumerable<Pet>> nested = neighbours.Map(n => n.Pets);
         IEnumerable<Pet> flat = neighbours.Bind(n => n.Pets);
      }

      class Neighbour
      {
         public string Name { get; set; }
         public IEnumerable<Pet> Pets { get; set; } = new Pet[] { };
      }
   }

   internal class Pet
   {
      private readonly string name;

      private Pet(string name)
      {
         this.name = name;
      }

      public static implicit operator Pet(string name) 
         => new Pet(name);
   }


}
