using System;
using Examples.Chapter9.Try;
using LaYumba.Functional;

namespace Examples.Domain
{
   public interface IRepository<T> where T: IEntity
   {
      Option<T> Get(Guid id);
   }
}