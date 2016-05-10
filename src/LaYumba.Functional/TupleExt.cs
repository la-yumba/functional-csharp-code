using System;

namespace LaYumba.Functional
{
    public static class TupleExt
   {
      public static R Match<T1, T2, R>(this Tuple<T1, T2> @this
          , Func<T1, T2, R> func) => func(@this.Item1, @this.Item2);

      public static Unit Match<T1, T2>(this Tuple<T1, T2> @this
          , Action<T1, T2> func) => func.ToFunc()(@this.Item1, @this.Item2);
   }
}