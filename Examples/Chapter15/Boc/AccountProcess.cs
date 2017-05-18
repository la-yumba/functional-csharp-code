using Boc.Commands;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Threading.Tasks;
using Boc.Chapter10.Domain;
using Boc.Domain.Events;
using Unit = System.ValueTuple;

namespace Boc.Chapter15
{
   using Result = Validation<(Event Event, AccountState NewState)>;

   public class AccountProcess
   {
      Agent<Command, Result> agent;

      public AccountProcess(AccountState initialState, Func<Event, Task<Unit>> saveAndPublish)
      {
         agent = Agent.Start(initialState
            , async (AccountState state, Command cmd) =>
            {
               Result result = new Pattern
               {
                  (MakeTransfer transfer) => state.Debit(transfer),
                  (FreezeAccount freeze) => state.Freeze(freeze),
               }
               .Match(cmd);

               await result.Traverse(tpl => saveAndPublish(tpl.Event)); // persist within block, so that the agent doesn't process new messages in a non-persisted state

               var newState = result.Map(tpl => tpl.NewState).GetOrElse(state);
               return (newState, result);
            });
      }

      public Task<Result> Handle(Command cmd) => agent.Tell(cmd);
   }
}
