using System;
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
   using static F;

   public static class ActionExt
   {
      public static Func<Unit> ToFunc(this Action action)
          => () => { action(); return Unit(); };

      public static Func<T, Unit> ToFunc<T>(this Action<T> action)
          => t => { action(t); return Unit(); };

      public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
          => (T1 t1, T2 t2) => { action(t1, t2); return Unit(); };
   }
}
