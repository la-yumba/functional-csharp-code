using System;
using System.Collections.Immutable;
using System.Net.Http;
using static System.Console;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace StatefulComputations
{
   using Rates = ImmutableDictionary<string, decimal>;

   public class CurrencyLookup_Stateless
   {
      public static void _main()
      {
         WriteLine("Enter a currency pair like 'EURUSD', or 'q' to quit");
         for (string input; (input = ReadLine().ToUpper()) != "Q";)
            WriteLine(FxApi.GetRate(input));
      }
   }

   public class CurrencyLookup
   {
      public static void _main()
      {
         WriteLine("Enter a currency pair like 'EURUSD', or 'q' to quit");
         MainRec(Rates.Empty);
      }

      static void MainRec(Rates cache)
      {
         var input = ReadLine().ToUpper();
         if (input == "Q") return;

         var (rate, newState) = GetRate(input, cache);
         WriteLine(rate);
         MainRec(newState); // recursively calls itself with the new state
      }

      // non-recursive version
      public static void MainNonRec()
      {
         WriteLine("Enter a currency pair like 'EURUSD', or 'q' to quit");
         var state = Rates.Empty;

         for (string input; (input = ReadLine().ToUpper()) != "Q";)
         {
            var (rate, newState) = GetRate(input, state);
            state = newState;
            WriteLine(rate);
         }
      }

      static (decimal, Rates) GetRate(string ccyPair, Rates cache)
      {
         if (cache.ContainsKey(ccyPair))
            return (cache[ccyPair], cache);

         var rate = FxApi.GetRate(ccyPair);
         return (rate, cache.Add(ccyPair, rate));
      }
   }

   public class CurrencyLookup_MoreTestable
   {
      public static void _main()
      {
         WriteLine("Enter a currency pair like 'EURUSD', or 'q' to quit");
         MainRec(Rates.Empty);
      }

      static void MainRec(Rates cache)
      {
         var input = ReadLine().ToUpper();
         if (input == "Q") return;

         var (rate, newState) = GetRate(FxApi.GetRate, input, cache);
         WriteLine(rate);
         MainRec(newState); // recursively calls itself with the new state
      }

      static (decimal, Rates) GetRate
         (Func<string, decimal> getRate, string ccyPair, Rates cache)
      {
         if (cache.ContainsKey(ccyPair))
            return (cache[ccyPair], cache);

         var rate = getRate(ccyPair);
         return (rate, cache.Add(ccyPair, rate));
      }
   }

   public class CurrencyLookup_ErrorHandling
   {
      public static void _main()
         => MainRec("Enter a currency pair like 'EURUSD', or 'q' to quit"
            , Rates.Empty);

      static void MainRec(string message, Rates cache)
      {
         WriteLine(message);
         var input = ReadLine().ToUpper();
         if (input == "Q") return;

         GetRate(pair => () => FxApi.GetRate(pair), input, cache).Run().Match(
            ex => MainRec($"Error: {ex.Message}", cache),
            result => MainRec(result.Quote.ToString(), result.NewState));
      }

      static Try<(decimal Quote, Rates NewState)> GetRate
        (Func<string, Try<decimal>> getRate, string ccyPair, Rates cache)
      {
         if (cache.ContainsKey(ccyPair))
            return Try(() => (cache[ccyPair], cache));

         else return from rate in getRate(ccyPair)
            select (rate, cache.Add(ccyPair, rate));
      }
   }

   static class FxApi
   {
      public static decimal GetRate(string ccyPair)
      {
         WriteLine($"fetching rate...");
         var uri = $"http://finance.yahoo.com/d/quotes.csv?f=l1&s={ccyPair}=X";
         var request = new HttpClient().GetStringAsync(uri);
         return decimal.Parse(request.Result.Trim());
      }

      public static Try<decimal> TryGetRate(string ccyPair)
         => () => GetRate(ccyPair);
   }
}
