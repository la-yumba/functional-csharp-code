using System;
using static LaYumba.Functional.F;

namespace LaYumba.Functional
{
   public delegate TryResult<T> Try<T>();

   public struct TryResult<T>
   {
      internal readonly T Value;
      internal Exception Exception;

      public TryResult(T value)
      {
         Value = value;
         Exception = null;
      }

      public TryResult(Exception e)
      {
         Exception = e;
         Value = default(T);
      }

      public static implicit operator TryResult<T>(T value) =>
          new TryResult<T>(value);

      internal bool IsFaulted => Exception != null;

      public override string ToString() =>
          IsFaulted
              ? Exception.ToString()
              : Value.ToString();
   }

   public static class TryExt
   {
      public static R Match<T, R>(this Try<T> self, Func<T, R> Succ, Func<Exception, R> Fail)
      {
         var res = self.Try();
         return res.IsFaulted
             ? Fail(res.Exception)
             : Succ(res.Value);
      }

      public static Unit Match<T>(this Try<T> self, Action<T> Succ, Action<Exception> Fail)
      {
         var res = self.Try();

         if (res.IsFaulted)
            Fail(res.Exception);
         else
            Succ(res.Value);

         return Unit();
      }

      public static Option<T> ToOption<T>(this Try<T> self)
      {
         var res = self.Try();
         return res.IsFaulted
             ? None
             : Some(res.Value);
      }

      public static TryResult<T> Try<T>(this Try<T> self)
      {
         try { return self(); }
         catch (Exception e) { return new TryResult<T>(e); }
      }
      
      public static Try<R> Map<T, R>(this Try<T> self, Func<T, R> mapper) => () =>
      {
         var res = self.Try();
         return res.IsFaulted
             ? new TryResult<R>(res.Exception)
             : mapper(res.Value);
      };

      public static Try<R> Map<T, R>(this Try<T> self, Func<T, R> Succ, Func<Exception, R> Fail) => () =>
      {
         var res = self.Try();
         return res.IsFaulted
             ? Fail(res.Exception)
             : Succ(res.Value);
      };

      public static Try<Func<T2, R>> Map<T1, T2, R>(this Try<T1> self, Func<T1, T2, R> func) =>
          self.Map(func.Curry());

      public static Try<Func<T2, Func<T3, R>>> Map<T1, T2, T3, R>(this Try<T1> self, Func<T1, T2, T3, R> func) =>
          self.Map(func.Curry());

      public static Try<R> Bind<T, R>(this Try<T> self, Func<T, Try<R>> binder) => () =>
      {
         var res = self.Try();
         return res.IsFaulted
             ? new TryResult<R>(res.Exception)
             : binder(res.Value).Try();
      };

      public static Try<R> Bind<T, R>(this Try<T> self, Func<T, Try<R>> Succ, Func<Exception, Try<R>> Fail) => () =>
      {
         var res = self.Try();
         return res.IsFaulted
             ? Fail(res.Exception).Try()
             : Succ(res.Value).Try();
      };
   }
}
