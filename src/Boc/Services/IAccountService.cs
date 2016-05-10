using Boc.Domain;
using LaYumba.Functional;
using System;

namespace Boc.Services
{
   public interface IAccountService
   {
      Option<AccountDetails> GetAccountDetails(Guid accountId);
   }
}
