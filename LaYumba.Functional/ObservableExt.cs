using System;
using System.Threading.Tasks;
using LaYumba.Functional;
using static LaYumba.Functional.F;

using System.Reactive.Linq;

namespace LaYumba.Functional
{
   public static class ObservableExt
   {
      // Safely performs a Task-returning function for each t in ts,
      // and returns a stream of results for the completed tasks, 
      // and a stream of exceptions
      public static (IObservable<R> Completed, IObservable<Exception> Faulted) 
      Safely<T, R>(this IObservable<T> ts, Func<T, Task<R>> f)
         => ts
            .SelectMany(t =>
               Observable.FromAsync(() =>
                  f(t).Map(
                     Faulted: ex => ex,
                     Completed: r => Exceptional(r))))
            .Partition();

      public static (IObservable<T> Successes, IObservable<Exception> Exceptions) 
      Partition<T>(this IObservable<Exceptional<T>> excTs)
      {
         bool IsSuccess(Exceptional<T> ex) 
            => ex.Match(_ => false, _ => true);

         T ValueOrDefault(Exceptional<T> ex)
            => ex.Match(exc => default(T), t => t);

         Exception ExceptionOrDefault(Exceptional<T> ex)
            => ex.Match(exc => exc, _ => default(Exception));
         
         return (excTs.Where(IsSuccess).Select(ValueOrDefault)
            , excTs.Where(e => !IsSuccess(e)).Select(ExceptionOrDefault));
      }

      public static (IObservable<T> Passed, IObservable<T> Failed) Partition<T>
         (this IObservable<T> source, Func<T, bool> predicate)
         => (Passed: (from t in source where predicate(t) select t)
            , Failed: (from t in source where !predicate(t) select t));

      public static (IObservable<RTrue> Passed, IObservable<RFalse> Failed) Partition<T, RTrue, RFalse>
         (this IObservable<T> source
         , Func<T, bool> If
         , Func<T, RTrue> Then
         , Func<T, RFalse> Else)
         => (source.Where(t => If(t)).Select(Then)
            , source.Where(t => !If(t)).Select(Else));
      
      public static IObservable<(T Previous, T Current)> 
         PairWithPrevious<T>(this IObservable<T> source)
         => source
            .Scan((Previous: default(T), Current: default(T))
               , (prev, current) => (prev.Current, current))
            .Skip(1);
   }
}
