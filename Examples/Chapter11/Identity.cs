using System;
using LaYumba.Functional;
using Unit = System.ValueTuple;

namespace Examples.Chapter11.Cache
{
   using static Console;
   using static F;

   interface IRepository<T>
   {
      Option<T> Get(Guid id);
   }

   class CachingRepository<T> : IRepository<T>
   {
      private IRepository<T> cache;
      private IRepository<T> db;

      public Option<T> Get(Guid id)
         //=> cache.Get(id).OrElse(db.Get(id)); // eagerly gets from DB, defeating the purpose of having the cache
         => cache.Get(id).OrElse(() => db.Get(id));
   }

   static class Identity_Example
   {
      internal static void _main()
      {
         string value = "Anton";
         Func<string, string> toUpper = s => s.ToUpper();
         Func<string, Unit> print = s => { WriteLine(s); return Unit(); };

         Identity(value)
            .Map(toUpper)
            .Map(print)();
         
         ReadKey();
      }
   }
}
