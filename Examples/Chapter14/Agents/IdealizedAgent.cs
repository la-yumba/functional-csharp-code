using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;

namespace LaYumba.Functional
{
   // idealized implementation of an agent
   sealed class IdealizedAgent<State, Msg>
   {
      BlockingCollection<Msg> inbox = new BlockingCollection<Msg>(new ConcurrentQueue<Msg>());

      public void Tell(Msg message) => inbox.Add(message);

      public IdealizedAgent(State initialState
         , Func<State, Msg, State> process)
      {
         void Loop(State state)
         {
            Msg message = inbox.Take(); // dequeue a message as soon as it's available
            State newState = process(state, message); // process the message
            Loop(newState); // loop with the new state
         }

         Task.Run(() => Loop(initialState)); // the actor runs in its own process
      }
   }
}
