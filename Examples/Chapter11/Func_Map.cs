using System;

using LaYumba.Functional;

namespace Examples.Chapter11
{
   class Model
   {
      public Guid Id { get; }
      public static Model Create(Guid id) => new Model();
   }
   class ViewModel { public Guid Id { get; } }

   static class Mapper
   {
      public static ViewModel ToViewModel(this Model model) => new ViewModel();
   }

   class Func_Map_Example
   {
      IRepository<ViewModel> cache;
      IRepository<Model> models;

      Func<Model> ModelFactory(Guid id) => () 
         => models.Get(id).GetOrElse(Model.Create(id));

      ViewModel GetOrCreate(Guid id) 
         => cache.Get(id)
            .GetOrElse(ModelFactory(id).Map(Mapper.ToViewModel));
   }

   class Func_Map_Example_More_Reasonable
   {
      IRepository<ViewModel> cache;
      IRepository<Model> models;

      Model ModelFactory(Guid id)
         => models.Get(id).GetOrElse(Model.Create(id));

      ViewModel GetOrCreate(Guid id)
         => cache.Get(id)
            .GetOrElse(() => ModelFactory(id).ToViewModel());
   }
}
