using System;
using System.Net.Http;
using static System.Console;
using System.Threading.Tasks;
using LaYumba.Functional;
using Decimal = LaYumba.Functional.Decimal;
using System.Collections.Immutable;

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Examples.Chapter14
{
   using static F;
   using CcyCache = ImmutableDictionary<string, decimal>;
   
   public static class CurrencyLookup_Unsafe
   {
      public static void Run()
      {
         var inputs = new Subject<string>();

         // var rates =
         //    from pair in inputs
         //    from rate in Observable.FromAsync(() => Yahoo.GetRate(pair))
         //    select rate;

         var rates =
            from pair in inputs
            from rate in Yahoo.GetRate(pair)
            select rate;

         var outputs = from r in rates select r.ToString();

         using (inputs.Trace("inputs"))
         using (rates.Trace("rates"))
         using (outputs.Trace("outputs")) 
            for (string input; (input = ReadLine().ToUpper()) != "Q";)
               inputs.OnNext(input);
      }
   } 

   public static class CurrencyLookup_Safe
   {
      public static void Run()
      {
         var inputs = new Subject<string>();

         var (rates, errors) = inputs.Safely(Yahoo.GetRate);

         var outputs = rates
            .Select(Decimal.ToString)
            .Merge(errors.Select(ex => ex.Message))
            .StartWith("Enter a currency pair like 'EURUSD', or 'q' to quit");

         using (outputs.Subscribe(WriteLine)) 
            for (string input; (input = ReadLine().ToUpper()) != "Q";)
               inputs.OnNext(input);
      }
   }

   public static class CurrencyLookup_Stateful_Async
   {
      public static void Run()
      {
         var pairs = new Subject<string>();

         var results = GetRates(pairs.AsObservable());
         var(succeeded, failed) = results.Partition();

         var outputs = succeeded.Select(t => t.Rate.ToString())
            .Merge(failed.Select(ex => ex.Message))
            .StartWith("Enter a currency pair like 'EURUSD' to get a quote, or 'q' to quit");

         using (outputs.Subscribe(WriteLine))
            for (string input; (input = ReadLine().ToUpper()) != "Q";)
               pairs.OnNext(input);
      }

      public static IObservable<Exceptional<(string Pair, decimal Rate)>> 
         GetRates(IObservable<string> pairs)
      {
         var retrievedRemotely = new Subject<(string Pair, decimal Rate)>();

         var cacheStates = retrievedRemotely
            .Scan(CcyCache.Empty, (cache, result) => cache.Add(result.Pair, result.Rate))
            .StartWith(CcyCache.Empty); // must give it a starting value, otherwise inputs never signals

         var inputs = pairs.WithLatestFrom(cacheStates
            , (pair, cache) => (Pair: pair, Cache: cache));

         var(finds, misses) = inputs.Partition(t => t.Cache.ContainsKey(t.Pair)
            , t => Exceptional((Pair: t.Pair, Rate: t.Cache[t.Pair])) // create a successful Exceptional for pairs whose rate is found in the cache
            , t => t.Pair); // keep the names of the pairs we must look up remotely

         var remoteLookups = misses.SelectMany(pair =>
            Observable.FromAsync(() =>
               Yahoo.GetRate(pair).Map(
                  ex => ex,
                  rate => Exceptional((Pair: pair, Rate: rate)))));

         return remoteLookups
            .Do(exc => exc.ForEach(result => retrievedRemotely.OnNext(result)))
            .Merge(finds);
      }
   }

   public class CurrencyLookup_Stateful_Sync
   {
      public static void _main()
      {
         var inputs = new Subject<string>();

         var accumulator = (Cache: CcyCache.Empty, Message: string.Empty);

         var lookups = inputs.Scan(accumulator
            , (tpl, pair) => tpl.Cache.ContainsKey(pair)
               ? (tpl.Cache, tpl.Cache[pair].ToString())
               : Yahoo.GetRate(pair)
                  .Map(rate => (tpl.Cache.Add(pair, rate), rate.ToString()))
                  .Recover(ex => (tpl.Cache, ex.Message))
                  .Result);

         // verbose version of the above
         //var main = inputs.Scan(Tuple(ImmutableDictionary.Create<string, decimal>(), string.Empty)
         //   , (tpl, pair) =>
         //   {
         //      if (tpl.Item1.ContainsKey(pair))
         //      {
         //         WriteLine("reusing cached value");
         //         return (tpl.Item1, tpl.Item1[pair].ToString());
         //      }
         //      else
         //      {
         //         WriteLine("fetching remotely...");
         //         return Yahoo.GetRate(pair)
         //            .Map(rate => (tpl.Item1.Add(pair, rate), rate.ToString()))
         //            .Recover(ex => (tpl.Item1, ex.Message))
         //            .Result;
         //      }
         //   });

         var outputs = lookups.Select(t => t.Message)
            .StartWith("Enter a currency pair like 'EURUSD' to get a quote, or 'q' to quit");

         using (outputs.Subscribe(WriteLine))
            for (string input; (input = ReadLine().ToUpper()) != "Q";)
               inputs.OnNext(input);
      }
   }

   public static class Yahoo
   {
      public static Task<decimal> GetRate(string ccyPair) =>
         from body in new HttpClient().GetStringAsync(QueryFor(ccyPair))
         select decimal.Parse(body.Trim());

      static string QueryFor(string ccyPair)
         => $"http://finance.yahoo.com/d/quotes.csv?f=l1&s={ccyPair}=X";
   }
}
