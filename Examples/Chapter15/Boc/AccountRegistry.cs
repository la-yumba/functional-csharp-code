using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Threading.Tasks;
using Boc.Chapter10.Domain;
using Boc.Domain.Events;
using Unit = System.ValueTuple;
using System.Collections.Immutable;

namespace Boc.Chapter15
{
   using AccountsCache = ImmutableDictionary<Guid, AccountProcess>;

   public class AccountRegistry_Naive
   {
      Agent<Guid, Option<AccountProcess>> agent;

      public AccountRegistry_Naive(Func<Guid, Task<Option<AccountState>>> loadAccount
         , Func<Event, Task<Unit>> saveAndPublish)
      {
         agent = Agent.Start(AccountsCache.Empty
            , async (AccountsCache cache, Guid id) =>
            {
               AccountProcess account;
               if (cache.TryGetValue(id, out account))
                  return (cache, Some(account));

               var optAccount = await loadAccount(id);

               return optAccount.Map(accState =>
               {
                  var process = new AccountProcess(accState, saveAndPublish);
                  return (cache.Add(id, process), Some(process));
               })
               .GetOrElse(() => (cache, (Option<AccountProcess>)None));
            });
      }

      public Task<Option<AccountProcess>> Lookup(Guid id) => agent.Tell(id);
   }

   public class AccountRegistry
   {
      Agent<Msg, Option<AccountProcess>> agent;
      Func<Guid, Task<Option<AccountState>>> loadAccount;

      class Msg { public Guid Id { get; set; } }
      class LookupMsg : Msg { }
      class RegisterMsg : Msg
      {
         public AccountState AccountState { get; set; }
      }

      public AccountRegistry(Func<Guid, Task<Option<AccountState>>> loadAccount
         , Func<Event, Task<Unit>> saveAndPublish)
      {
         this.loadAccount = loadAccount;

         this.agent = Agent.Start(AccountsCache.Empty, (AccountsCache cache, Msg msg) =>
            new Pattern<(AccountsCache, Option<AccountProcess>)>
            {
               (LookupMsg m) => (cache, cache.Lookup(m.Id)),

               (RegisterMsg m) => cache.Lookup(m.Id).Match(
                  Some: acc => (cache, Some(acc)),
                  None: () =>
                  {
                     var account = new AccountProcess(m.AccountState, saveAndPublish);
                     return (cache.Add(m.Id, account), Some(account));
                  })
            }
            .Match(msg));
      }

      public Task<Option<AccountProcess>> Lookup(Guid id)
         => agent
         .Tell(new LookupMsg { Id = id })
         .OrElse(() => 
            from state in loadAccount(id) // loading the state is done in the calling thread
            from account in agent.Tell(new RegisterMsg
            {
               Id = id,
               AccountState = state
            })
            select account);
   }
}
