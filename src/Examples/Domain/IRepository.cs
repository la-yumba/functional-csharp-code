using System;
using LaYumba.Functional;

namespace Examples.Domain
{
   public interface IRepository<T> where T: IEntity
   {
      Option<T> Get(Guid id);
   }
}