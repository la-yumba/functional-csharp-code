using System;

namespace LaYumba.Functional
{
   using static F;

   // S -> (T, S)
   public delegate Tuple<T, S> State<S, T>(S state);
   
   public class StateResult<T, S>
   {
      public T Value { get; }
      public S State { get; }

      internal StateResult(T value, S state)
      {
         Value = value;
         State = state;
      }

      public static implicit operator StateResult<T, S>(Tuple<T, S> tuple)
         => tuple.Match((t, s) => new StateResult<T, S>(t, s));

      public static implicit operator T(StateResult<T, S> value) 
         => value.Value;
   }

   public static class State<S>
   {
      // Return
      public static State<S, T> Of<T>(T value)
          => state => Tuple(value, state);
   }

   public static class State
   {
      // put allows you to update the state
      public static State<S, Unit> Put<S>(S newState)
          => state => Tuple(Unit(), newState);

      // get allows you to read the state
      public static State<S, S> Get<S>()
          => state => Tuple(state, state);

      // without tuple.Match it would look like this
      //public static State<R, S> Bind<T, R, S>(this State<T, S> @this // signatures wrong
      //    , Func<T, State<R, S>> func) => state =>
      //    {
      //        var r = @this(state);
      //        return func(r.Item1)(r.Item2);
      //    };

      //public static State<R, S> Bind<T, R, S>(this State<T, S> @this
      //    , Func<T, S, Tuple<R, S>> func) => state =>
      //    {
      //        var r = @this(state);
      //        return func(r.Item1, r.Item2);
      //    };

      public static State<S, R> Map<T, R, S>(this State<S, T> @this
          , Func<T, R> func)
          => state => @this(state).Match((t, s) => Tuple(func(t), s)); // really, that's it?


      public static State<S, R> Bind<T, R, S>(this State<S, T> @this
          , Func<T, State<S, R>> func)
          => state => @this(state).Match((t, s) => func(t)(s));

      public static State<S, R> Bind<T, R, S>(this State<S, T> @this
          , Func<T, S, Tuple<R, S>> func)
          => state => @this(state).Match(func);

      // LINQ

      public static State<S, R> Select<T, R, S>(this State<S, T> @this
         , Func<T, R> func) => @this.Map(func);

      public static State<S, V> SelectMany<S, T, U, V>(this State<S, T> @this
         , Func<T, State<S, U>> bind
         , Func<T, U, V> project) 
         => state =>
         {
            var resT = @this(state);
            var resU = bind(resT.Item1)(resT.Item2);
            var resV = project(resT.Item1, resU.Item1);
            return Tuple(resV, resU.Item2);
         };
   }
}
