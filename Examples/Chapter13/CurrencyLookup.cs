using System.Net.Http;
using static System.Console;
using System.Threading.Tasks;
using LaYumba.Functional;

namespace Examples.Chapter13
{
   using Newtonsoft.Json.Linq;
   using static F;

   public class CurrencyLookup_Stateless
   {
      public static void _main()
      {
         WriteLine("Enter a currency pair like 'EURUSD' to get a quote, or 'q' to quit");
         for (string input; (input = ReadLine().ToUpper()) != "Q";)
            WriteLine(FxApi.GetRate(input).Map(Decimal.ToString)
               .Recover(ex => ex.Message).Result); // what to do in case of failure
      }
   }

   static class FxApi_1
   {
      public static async Task<decimal> GetRate(string ccyPair)
      {
         WriteLine($"fetching rate...");
         var s = await new HttpClient().GetStringAsync(QueryFor(ccyPair));
         return decimal.Parse(s.Trim());
      }

      static string QueryFor(string ccyPair)
         => $"http://finance.yahoo.com/d/quotes.csv?f=l1&s={ccyPair}=X";
   }

   static class FxApi_2
   {
      public static Task<decimal> GetRate(string ccyPair) =>
         from s in new HttpClient().GetStringAsync(QueryFor(ccyPair))
         select decimal.Parse(s.Trim());

      static string QueryFor(string ccyPair)
         => $"http://finance.yahoo.com/d/quotes.csv?f=l1&s={ccyPair}=X";
   }



   static class FxApi
   {
      public static Task<decimal> GetRate(string ccyPair) =>
         CurrencyLayer.GetRate(ccyPair)
            .OrElse(() => Yahoo.GetRate(ccyPair));
   }


   public static class Yahoo
   {
      public static Task<decimal> GetRate(string ccyPair)
      {
         WriteLine($"Fetching rate for {ccyPair} ...");
         return from body in new HttpClient().GetStringAsync(QueryFor(ccyPair))
                select decimal.Parse(body.Trim());
      }

      static string QueryFor(string ccyPair)
         => $"http://finance.yahoo.com/d/quotes.csv?f=l1&s={ccyPair}=X";
   }


   /// <summary>
   /// Note that:
   /// 1. you need to get an API key from https://currencylayer.com/ (free, registration required)
   /// 2. the free plan only allows you to query rates with USD as base currency
   /// </summary>
   public static class CurrencyLayer
   {
      static string key = "4772f4e46027c9047c9a2f7444c95c60";

      public static Task<decimal> GetRate(string ccyPair) =>
         from body in new HttpClient().GetStringAsync(QueryFor(ccyPair))
         select (decimal)JObject.Parse(body)["quotes"][ccyPair];

      static string QueryFor(string pair)
         => $"http://www.apilayer.net/api/live?access_key={key}";
   }
}
