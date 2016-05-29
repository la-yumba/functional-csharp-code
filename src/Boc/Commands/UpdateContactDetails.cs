using System;

namespace Boc.Commands
{
   public class UpdateContactDetails
   {
      public Guid AccountId { get; }
      public string Telephone { get; }
      public string Email { get; }
   }
}
