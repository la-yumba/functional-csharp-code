using System.Net.Http;
using System.Threading.Tasks;
using LaYumba.Functional;
using static System.Console;

namespace Examples.Async
{
    using static F;

    class SimpleTaskExample
    {
        internal static void _main()
        {
            var ccyPair = "EURUSD";
            Task<string> content = GetQuote(ccyPair);

            content
                .Map(c => c.Split(',')[1])
                .ForEach(WriteLine);
            WriteLine("The request has been sent!");

            (from r in content select r.Split(',')[1])
               .ForEach(WriteLine);

            ReadKey();
        }

        private static async Task<string> GetQuote(string ccyPair)
        {
            var url = $"http://finance.yahoo.com/d/quotes.csv?f=nl1&s={ccyPair}=X";
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }
    }
}
