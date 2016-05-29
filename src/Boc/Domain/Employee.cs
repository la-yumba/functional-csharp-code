using System;

namespace Boc.Domain
{
   public class Employee : IEntity
   {
      public Guid Id
      {
         get;
      }

       public string LastName { get; }
   }
}