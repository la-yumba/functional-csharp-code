using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Examples.Chapter7
{
   class TypeInference_Map
   {
      // 1. method
      int AddMethod(int x, int y) => x + y;

      // 2. field
      Func<int, int, int> AddField = (x, y) => x + y;

      // 3. factory
      Func<int, int, int> AddFactory() => (x, y) => x + y;

      public void Demonstrate()
      {
         // the line below does NOT compile
         //Some(3).Map(AddMethod).Apply(Some(4));

         Some(3).Map<int, int, int>(AddMethod).Apply(Some(4));
         Some(3).Map(AddField).Apply(Some(4));
         Some(3).Map(AddFactory()).Apply(Some(4));
      }
   }
}

