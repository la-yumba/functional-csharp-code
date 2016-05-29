using System;
using System.Collections.Generic;
using Boc.Domain;
using LaYumba.Functional;

namespace Boc.Data
{
   public interface IReadRepository<T> where T : IEntity
   {
      Option<T> Get(Guid id);
   }

   public interface IRepository<T> : IReadRepository<T> 
      where T : IEntity
   {
      void Save(T request);
      IEnumerable<T> GetAll();
   }
}