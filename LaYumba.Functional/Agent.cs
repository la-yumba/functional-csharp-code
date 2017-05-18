using static LaYumba.Functional.F;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
   public static class Agent
   {
      public static Agent<Msg> Start<Msg>(Action<Msg> action)
         => new StatelessAgent<Msg>(action);

      public static Agent<Msg> Start<State, Msg>
         ( Func<State> initState
         , Func<State, Msg, State> process)
         => new StatefulAgent<State, Msg>(initState(), process);

      public static Agent<Msg> Start<State, Msg>
         (State initialState
         , Func<State, Msg, State> process)
         => new StatefulAgent<State, Msg>(initialState, process);

      public static Agent<Msg> Start<State, Msg>
         (State initialState
         , Func<State, Msg, Task<State>> process)
         => new StatefulAgent<State, Msg>(initialState, process);

      public static Agent<Msg, Reply> Start<State, Msg, Reply>
         (State initialState
         , Func<State, Msg, (State, Reply)> process)
         => new TwoWayAgent<State, Msg, Reply>(initialState, process);

      public static Agent<Msg, Reply> Start<State, Msg, Reply>
         (State initialState
         , Func<State, Msg, Task<(State, Reply)>> process)
         => new TwoWayAgent<State, Msg, Reply>(initialState, process);
   }

   public interface Agent<in Msg>
   {
      void Tell(Msg message);
   }

   public interface Agent<in Msg, Reply>
   {
      Task<Reply> Tell(Msg message);
   }

   class StatelessAgent<Msg> : Agent<Msg>
   {
      private readonly ActionBlock<Msg> actionBlock;

      public StatelessAgent(Action<Msg> process)
      {
         actionBlock = new ActionBlock<Msg>(process);
      }

      public StatelessAgent(Func<Msg, Task> process)
      {
         actionBlock = new ActionBlock<Msg>(process);
      }

      public void Tell(Msg message) => actionBlock.Post(message);
   }

   class StatefulAgent<State, Msg> : Agent<Msg>
   {
      private State state;
      private readonly ActionBlock<Msg> actionBlock;

      public StatefulAgent(State initialState
         , Func<State, Msg, State> process)
      {
         state = initialState;

         actionBlock = new ActionBlock<Msg>(
            msg => state = process(state, msg)); // process the message with the current state, and store the resulting new state as the current state of the agent
      }

      public StatefulAgent(State initialState
         , Func<State, Msg, Task<State>> process)
      {
         state = initialState;

         actionBlock = new ActionBlock<Msg>(
            async msg => state = await process(state, msg));
      }

      public void Tell(Msg message) => actionBlock.Post(message);
   }

   class TwoWayAgent<State, Msg, Reply> : Agent<Msg, Reply>
   {
      private State state;
      private readonly ActionBlock<(Msg, TaskCompletionSource<Reply>)> actionBlock;

      public TwoWayAgent(State initialState, Func<State, Msg, (State, Reply)> process)
      {
         state = initialState;

         actionBlock = new ActionBlock<(Msg, TaskCompletionSource<Reply>)>(
            t =>
            {
               var result = process(state, t.Item1);
               state = result.Item1;
               t.Item2.SetResult(result.Item2);
            });
      }

      // creates a 2-way agent with an async processing func
      public TwoWayAgent(State initialState, Func<State, Msg, Task<(State, Reply)>> process)
      {
         state = initialState;

         actionBlock = new ActionBlock<(Msg, TaskCompletionSource<Reply>)>(
            async t => await process(state, t.Item1)
               .ContinueWith(task =>
               {
                  if (task.Status == TaskStatus.Faulted)
                     t.Item2.SetException(task.Exception);
                  else
                  {
                     state = task.Result.Item1;
                     t.Item2.SetResult(task.Result.Item2);
                  }
               }));
      }

      public Task<Reply> Tell(Msg message)
      {
         var tcs = new TaskCompletionSource<Reply>();
         actionBlock.Post((message, tcs));
         return tcs.Task;
      }
   }
}
