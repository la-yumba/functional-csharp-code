using System;

namespace Boc.Chapter1.V1.Data
{
   internal class Repository<T> : IRepository<T> where T : IEntity
   {
      public T Get(Guid id)
      {
         throw new NotImplementedException();
      }
   }
}