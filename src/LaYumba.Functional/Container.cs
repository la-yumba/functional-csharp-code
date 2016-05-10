using System;

namespace LaYumba.Functional
{
   public struct Container<T>
   {
      internal T Value { get; }
      internal Container(T value) { Value = value; }
      public override string ToString() => $"Container({Value})";
   }

   public static class Container
   {
      public static Container<T> Of<T>(T value)
         => new Container<T>(value);

      public static Container<R> Map<T, R>(this Container<T> @this
         , Func<T, R> func) => Container.Of(func(@this.Value));
   }
}
