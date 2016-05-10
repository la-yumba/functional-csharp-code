using LaYumba.Functional;
using System;

namespace Boc.Chapter1.V2.Data
{
   public interface IRepository<T> where T : IEntity
   {
      Option<T> Get(Guid id);
   }
}