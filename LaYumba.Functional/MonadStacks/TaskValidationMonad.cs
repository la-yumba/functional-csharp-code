using System;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   public class TaskValidation<T>
   {
      public Task<Validation<T>> Value { get; }

      public TaskValidation(Task<Validation<T>> value) 
         => Value = value;

      public static implicit operator Task<Validation<T>>(TaskValidation<T> tv) 
         => tv.Value;
   }

   public static class TaskValidationMonad
   {
      public static TaskValidation<T> Stack<T>(this Task<Validation<T>> @this)
         => new TaskValidation<T>(@this);

      public static Task<Validation<U>> Select<T, U>
         (this Task<Validation<T>> self
         , Func<T, U> mapper)
         => self.Map(x => x.Map(mapper));

      // public static Task<Validation<R>> SelectMany<T, R>
      //    (this Task<Validation<T>> task       // Task<Validation<T>> 
      //    , Func<T, Task<Validation<R>>> bind) // -> (T -> Task<Validation<R>>)
      //    => task.Bind(vt => vt.TraverseBind(bind));

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

      public static TaskValidation<RR> SelectMany<T, R, RR>
         (this TaskValidation<T> tv       // Task<Validation<T>> 
         , Func<T, Task<Validation<R>>> bind  // -> (T -> Task<Validation<R>>)
         , Func<T, R, RR> project)
         => tv
            .Value
            .Map(vt => vt.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r)))))
            .Unwrap()
            .Stack();
   }
}
