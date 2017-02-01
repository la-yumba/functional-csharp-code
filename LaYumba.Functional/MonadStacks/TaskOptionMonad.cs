using System;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   public static class TaskOptionMonad
   {
      public static Task<Option<T>> OrElse<T>
         (this Task<Option<T>> task, Func<Task<Option<T>>> fallback)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? fallback()
                  : t.Result.Match(
                     None: fallback,
                     Some: val => Async(t.Result))
            )
            .Unwrap();

      public static Task<Option<U>> Select<T, U>
         (this Task<Option<T>> self
         , Func<T, U> mapper)
         => self.Map(x => x.Map(mapper));

      public static Task<Option<R>> SelectMany<T, R>
         (this Task<Option<T>> task       // Task<Option<T>> 
         , Func<T, Task<Option<R>>> bind) // -> (T -> Task<Option<R>>)
         => task.Bind(vt => vt.TraverseBind(bind));
      //=> task.Bind(vt => vt.Traverse(bind).Map(vvr => vvr.Bind(vr => vr)));

      public static Task<Option<RR>> SelectMany<T, R, RR>
         (this Task<Option<T>> task       // Task<Option<T>> 
         , Func<T, Task<Option<R>>> bind  // -> (T -> Task<Option<R>>)
         , Func<T, R, RR> project)
         => task
            .Map(vt => vt.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r)))))
            .Unwrap();
   }
}
