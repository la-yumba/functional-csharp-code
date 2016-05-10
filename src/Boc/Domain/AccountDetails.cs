using System;

namespace Boc.Domain
{
   public class AccountDetails : IEntity
   {
      public Guid Id { get; }
      public string Telephone { get; }
      public string Email { get; }

      public AccountDetails(Guid id, string email, string telephone)
      {
         this.Id = id;
         this.Email = email;
         this.Telephone = telephone;
      }

      public AccountDetails WithEmail(string email)
         => new AccountDetails(Id, email, Telephone);

      public AccountDetails WithTelephone(string telephone)
         => new AccountDetails(Id, Email, telephone);
   }
}