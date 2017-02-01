using System;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   public static class TaskValidationMonad
   {
      //public static Validation<Task<V>> SelectMany<T, U, V>
      //   (this Validation<T> self
      //   , Func<T, Task<U>> bind
      //   , Func<T, U, V> project)
      //   => self.Map(t => bind(t).Map(u => project(t, u)));

      //public static Task<Validation<RR>> SelectMany<T, R, RR>
      //   (this Validation<T> val              // Validation<T>
      //   , Func<T, Task<Validation<R>>> bind  // -> (T -> Task<Validation<R>>)
      //   , Func<T, R, RR> project)
      //   => val.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r))));


      public static Task<Validation<U>> Select<T, U>
         (this Task<Validation<T>> self
         , Func<T, U> mapper)
         => self.Map(x => x.Map(mapper));

      // Task<Validation<T>> `bind` T -> Task<Validation<R>>

      public static Task<Validation<R>> SelectMany<T, R>
         (this Task<Validation<T>> task       // Task<Validation<T>> 
         , Func<T, Task<Validation<R>>> bind) // -> (T -> Task<Validation<R>>)
         => task.Bind(vt => vt.TraverseBind(bind));

      public static Task<Validation<RR>> SelectMany<T, R, RR>
         (this Task<Validation<T>> task       // Task<Validation<T>> 
         , Func<T, Task<Validation<R>>> bind  // -> (T -> Task<Validation<R>>)
         , Func<T, R, RR> project)
         => task
            .Map(vt => vt.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r)))))
            .Unwrap();

      // equivalently:
      //{
      //   var valT = await task;
      //   return await valT.TraverseBind(async t =>
      //   {
      //      var valR = await bind(t);
      //      return valR.Map(r => project(t, r));
      //   });
      //}
   
      
      
      
      // Task<Validation<T>> `bind` T -> Validation<R>

      //public static Task<Validation<RR>> SelectMany<T, R, RR>
      //   (this Task<Validation<T>> task    // Task<Validation<T>> 
      //   , Func<T, Validation<R>> bind     // -> (T -> Validation<R>)
      //   , Func<T, R, RR> project)
      //   => task.Map(vt => vt.Bind(t => bind(t).Map(r => project(t, r))));

      // Task<Validation<T>> `bind` T -> Task<R>

      //public static Task<Validation<RR>> SelectMany<T, R, RR>
      //   (this Task<Validation<T>> task    // Task<Validation<T>> 
      //   , Func<T, Task<R>> bind           // -> (T -> Task<R>)
      //   , Func<T, R, RR> project)
      //   => task.Bind(vt => vt.Traverse(t => bind(t).Map(r => project(t, r))));

      //public static Task<Validation<U>> SelectMany<T, U>
      //   (this Task<Validation<T>> self
      //   , Func<T, Task<U>> bind)
      //   => self.Bind(vt => vt.Traverse(t => bind(t)));

      //public static Task<Validation<V>> SelectMany<T, U, V>
      //   (this Task<T> self
      //   , Func<T, Validation<U>> bind
      //   , Func<T, U, V> project)
      //   => self.Map(t => bind(t).Map(u => project(t, u)));


      //public static Task<Validation<V>> SelectMany<T, U, V>
      //   (this Task<Validation<T>> self
      //   , Func<T, U> bind
      //   , Func<T, U, V> project)
      //   => self.Map(x => x.SelectMany(bind, project));

      //internal static Validation<V> SelectMany<T, U, V>
      //   (this Validation<T> self
      //   , Func<T, U> bind
      //   , Func<T, U, V> project)
      //   => self.Map(t => project(t, bind(t)));
   }
}
