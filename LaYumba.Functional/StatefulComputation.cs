using System;
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
   using static F;

   public delegate (T Value, S State) StatefulComputation<S, T>(S state);

   public static class StatefulComputation<S>
   {
        public static StatefulComputation<S, T> Return<T>(T value)
           => state => (value, state);
        
        public static StatefulComputation<S, Unit> Put(S newState)
            => state => (Unit(), newState);

      public static StatefulComputation<S, S> Get
          => state => (state, state);
    }

    public static class StatefulComputation
    {
        public static T Run<S, T>(this StatefulComputation<S, T> f, S state)
           => f(state).Value;

        public static StatefulComputation<S, Unit> Put<S>(S newState)
            => state => (Unit(), newState);

      public static StatefulComputation<S, S> Get<S>()
          => state => (state, state);

        public static StatefulComputation<S, R> Select<S, T, R>
           (this StatefulComputation<S, T> f, Func<T, R> project)
           => state0 =>
           {
               var(t, state1) = f(state0);
               return (project(t), state1);
           };

        public static StatefulComputation<S, R> SelectMany<S, T, R>
           (this StatefulComputation<S, T> StatefulComputation, Func<T, StatefulComputation<S, R>> f)
           => state0 =>
           {
               var(t, state1) = StatefulComputation(state0);
               return f(t)(state1);
           };

        public static StatefulComputation<S, RR> SelectMany<S, T, R, RR>
           (this StatefulComputation<S, T> f
           , Func<T, StatefulComputation<S, R>> bind
           , Func<T, R, RR> project)
           => state0 =>
           {
               var(t, state1) = f(state0);
               var(r, state2) = bind(t)(state1);
               var rr = project(t, r);
               return (rr, state2);
           };
    }
}
