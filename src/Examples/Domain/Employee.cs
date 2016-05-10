using System;

namespace Examples.Domain
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