using System;
using static System.Console;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Examples.Chapter14
{
   static class KeySequences
   {
      public static void Run()
      {
         WriteLine("Enter some inputs to push them to 'inputs', or 'q' to quit");

         var keys = new Subject<ConsoleKeyInfo>();
         var halfSec = TimeSpan.FromMilliseconds(500);

         var keysAlt = keys
            .Where(key => key.Modifiers.HasFlag(ConsoleModifiers.Alt));

         var twoKeyCombis =
            from first in keysAlt
            from second in keysAlt.Take(halfSec).Take(1)
            select (First: first, Second: second);

         var altKB =
            from tpl in twoKeyCombis
            where tpl.First.Key == ConsoleKey.K
               && tpl.Second.Key == ConsoleKey.B
            select Unit();

         using (keys.Select(k => Environment.NewLine + k.KeyChar).Trace("keys"))
         using (twoKeyCombis.Select(ks => $"{ks.Item1.KeyChar}-{ks.Item2.KeyChar}").Trace("twoKeyCombis"))
         using (altKB.Trace("altKB"))
            for (ConsoleKeyInfo key; (key = ReadKey()).Key != ConsoleKey.Q;)
               keys.OnNext(key);
      }
   }

   static class Operators
   {
      public static void ScanEx()
      {
         IObservable<Transaction> transactions = null;

         decimal initialBalance = 0;
         IObservable<decimal> balance = transactions.Scan(initialBalance
            , (bal, trans) => bal + trans.Amount);

         IObservable<decimal> dipsIntoTheRed =
            from bal in balance.PairWithPrevious()
            where bal.Previous >= 0
               && bal.Current < 0
            select bal.Current;


         var dipsIntoRed = transactions
            .GroupBy(t => t.AccountId)
            .Select(DipsIntoTheRed)
            .Merge();
      }

      public static IObservable<Guid> DipsIntoTheRed
         (IGroupedObservable<Guid, Transaction> transactions)
      {
         Guid accountId = transactions.Key;
         decimal initialBalance = 0;

         var balance = transactions.Scan(initialBalance
            , (bal, trans) => bal + trans.Amount);

         return from bal in balance.PairWithPrevious()
                where bal.Previous >= 0
                   && bal.Current < 0
                select accountId;
      }

      public static IObservable<T> MergeAll<T>
         (this IObservable<IObservable<T>> source)
         => source.SelectMany(x => x);
   }

   internal class Transaction
   {
      public Guid AccountId { get; }
      public decimal Amount { get; }
   }
}
