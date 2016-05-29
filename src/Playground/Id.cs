using System;

namespace Playground
{
   public struct Id<T>
   {
      internal T Value { get; }

      internal Id(T value)
      {
         Value = value;
      }

      public static implicit operator Id<T>(T value) => new Id<T>(value);
      public static implicit operator T(Id<T> id) => id.Value;
   }

   public static class Id
   {
      public static Id<T> Of<T>(T value) => new Id<T>(value); 

      public static Id<R> Map<T, R>(this Id<T> @this, Func<T, R> func)
         => func(@this.Value); // implicitly lifted

      public static Id<R> Bind<T, R>(this Id<T> @this, Func<T, Id<R>> func)
         => func(@this.Value);

      public static Id<R> Select<T, R>(this Id<T> @this, Func<T, R> func)
          => @this.Map(func);

      public static Id<RR> SelectMany<T, R, RR>(this Id<T> @this
          , Func<T, Id<R>> bind, Func<T, R, RR> project)
      {
         //return @this.Bind(x => bind(x).Map(project.Partial(x)));
         var bound = @this.Bind(bind);
         return project(@this.Value, bound.Value);
      }
   }
}
