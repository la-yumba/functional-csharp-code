using System;
using LaYumba.Functional;

namespace Examples.Chapter11
{
   class Tap_Example
   {
      IRepository<ViewModel> cache;
      IRepository<Model> models;

      Model GetOrCreateModel(Guid id)
         => models.Get(id)
            .GetOrElse(Model.Create(id).Pipe(models.Save));
      
      ViewModel CreateAndCacheViewModel(Guid id)
         => GetOrCreateModel(id).ToViewModel().Pipe(cache.Save);

      ViewModel GetOrCreate(Guid id)
         => cache.Get(id)
            .GetOrElse(() => CreateAndCacheViewModel(id));
   }

   interface IRepository<T>
   {
      Option<T> Get(Guid id);
      void Save(T obj);
   }
}
