using System;

namespace LaYumba.Functional
{
   public delegate T Identity<T>();

   public static class Identity
   {
      public static Identity<R> Map<T, R>(this Identity<T> @this
         , Func<T, R> func) => () => func(@this());

      public static Identity<R> Bind<T, R>(this Identity<T> @this
         , Func<T, Identity<R>> func) => () => func(@this())();
   }
}
