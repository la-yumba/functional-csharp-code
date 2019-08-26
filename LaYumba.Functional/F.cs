using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
   public static partial class F
   {
      public static Unit Unit() => default(Unit);

      // function manipulation 

      public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func)
          => t1 => t2 => func(t1, t2);

      public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func)
          => t1 => t2 => t3 => func(t1, t2, t3);

      public static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>
         (this Func<T1, T2, T3, R> @this) => t1 => (t2, t3) => @this(t1, t2, t3);

      public static Func<T1, Func<T2, T3, T4, R>> CurryFirst<T1, T2, T3, T4, R>
         (this Func<T1, T2, T3, T4, R> @this) => t1 => (t2, t3, t4) => @this(t1, t2, t3, t4);

      public static Func<T1, Func<T2, T3, T4, T5, R>> CurryFirst<T1, T2, T3, T4, T5, R>
         (this Func<T1, T2, T3, T4, T5, R> @this) => t1 => (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);

      public static Func<T1, Func<T2, T3, T4, T5, T6, R>> CurryFirst<T1, T2, T3, T4, T5, T6, R>
         (this Func<T1, T2, T3, T4, T5, T6, R> @this) => t1 => (t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);

      public static Func<T1, Func<T2, T3, T4, T5, T6, T7, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, R>
         (this Func<T1, T2, T3, T4, T5, T6, T7, R> @this) => t1 => (t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);

      public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);

      public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);

      public static Func<T, T> Tap<T>(Action<T> act) 
         => x => { act(x); return x; };
      
      public static R Pipe<T, R>(this T @this, Func<T, R> func) => func(@this);
      
      /// <summary>
      /// Pipes the input value in the given Action, i.e. invokes the given Action on the given value.
      /// returning the input value. Not really a genuine implementation of pipe, since it combines pipe with Tap.
      /// </summary>
      public static T Pipe<T>(this T input, Action<T> func) => Tap(func)(input);

      // DATA STRUCTURES

      public static KeyValuePair<K, T> Pair<K, T>(K key, T value)
         => new KeyValuePair<K, T>(key, value);

      public static IEnumerable<T> List<T>(params T[] items) => items.ToImmutableList();

      public static Func<T, IEnumerable<T>> SingletonList<T>() => (item) => ImmutableList.Create(item);

      public static IEnumerable<T> Cons<T>(this T t, IEnumerable<T> ts)
         => List(t).Concat(ts);

      public static Func<T, IEnumerable<T>, IEnumerable<T>> Cons<T>()
         => (t, ts) => t.Cons(ts);

      public static IDictionary<K, T> Map<K, T>(params KeyValuePair<K, T>[] pairs)
         => pairs.ToImmutableDictionary();

      // misc

      // Using
      public static R Using<TDisp, R>(TDisp disposable
         , Func<TDisp, R> func) where TDisp : IDisposable
      {
         using (var disp = disposable) return func(disp);
      }

      public static Unit Using<TDisp>(TDisp disposable
         , Action<TDisp> act) where TDisp : IDisposable 
         => Using(disposable, act.ToFunc());
      
      public static R Using<TDisp, R>(Func<TDisp> createDisposable
         , Func<TDisp, R> func) where TDisp : IDisposable
      {
         using (var disp = createDisposable()) return func(disp);
      }

      public static Unit Using<TDisp>(Func<TDisp> createDisposable
         , Action<TDisp> action) where TDisp : IDisposable
         => Using(createDisposable, action.ToFunc());

      // Range
      public static IEnumerable<char> Range(char from, char to)
      {
         for (var i = from; i <= to; i++) yield return i;
      }

      public static IEnumerable<int> Range(int from, int to)
      {
         for (var i = from; i <= to; i++) yield return i;
      }
   }
}


