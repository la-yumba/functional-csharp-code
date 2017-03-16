using System;
using static System.Console;
using System.Reactive.Linq;

namespace Examples.Chapter14
{
   public static class ObservableExt
   {
      public static IDisposable Trace<T>(this IObservable<T> source, string name)
         => source.Subscribe(
            onNext: val => WriteLine($"{name} -> {val}"),
            onError: ex => WriteLine($"{name} ERROR: {ex.Message}"),
            onCompleted: () => WriteLine($"{name} END"));
   }
}
