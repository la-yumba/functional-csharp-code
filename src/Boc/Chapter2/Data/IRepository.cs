using LaYumba.Functional;
using System;

namespace Boc.Chapter1.V1.Data
{
   public interface IRepository<T> where T : IEntity
   {
      T Get(Guid id);
   }
}