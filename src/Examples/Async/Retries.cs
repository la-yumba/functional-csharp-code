using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using static System.Console;

namespace Examples.Async
{
    public class RetryOptions<T>
    {
        public Func<Task<T>> FuncToAttempt { get; } 
        public int AttemptsToMake { get; }
        public int AttemptsMade { get; }

        public TimeSpan NextDelay => TimeSpan.FromSeconds(Math.Pow(2, AttemptsMade));
        public bool GiveUp => AttemptsMade == AttemptsToMake;

        public RetryOptions(Func<Task<T>> funcToAttempt, int attemptsToMake) 
            : this(funcToAttempt, attemptsToMake, 0) { }

        private RetryOptions(Func<Task<T>> funcToAttempt, int attemptsToMake, int attemptsMade)
        {
            FuncToAttempt = funcToAttempt;
            AttemptsToMake = attemptsToMake;
            AttemptsMade = attemptsMade;
        }

        public RetryOptions<T> IncrementAttemptsMade()
            => new RetryOptions<T>(FuncToAttempt, AttemptsToMake, AttemptsMade + 1);
    }

    public static class Retries
    {
        public static async void main()
        {
            var uri = "http://google.com";

            // naive 
            var result = await DownloadStringWithRetries(uri);
            WriteLine(result);

            // with options
            Func<Task<string>> funcToAttempt = () => Using(
                ()     => new HttpClient(), 
                client => client.GetStringAsync(uri));

            result = await DoAnythingWithRetries(3, funcToAttempt);
            WriteLine(result);

            // with state
            var options = new RetryOptions<string>(funcToAttempt, 3);

            //var r = State.Of<string, RetryOptions<string>>(uri);
            //var r2 = r.Bind(RetryStatefulComp);

            ReadKey();
        }

        /// <summary>
        /// desired 
        /// (url) => content
        /// 
        /// (url, retries) => Either<(url, retries), content>
        /// (url, retries) => (content, (url, retries))
        /// </summary>
        /// 
        static Either<int, R> Try<R>(Func<R> func, int triesLeft)
        {
            if (triesLeft == 1) return func();
            try
            {
                return func();
            }
            catch
            {
                Thread.Sleep(1);
                return triesLeft - 1;
            }
        } 

        //static Tuple<Promise<string>, RetryOptions<string>> RetryStatefulComp(string uri, RetryOptions<string> state)
        //{
        //    if (state.GiveUp) return Tuple(state.FuncToAttempt(), state);
        //    return Tuple(input, state).Bin;
        //}


        public static async Task<string> DownloadStringWithRetries(string uri)
        {
            using (var client = new HttpClient())
            {
                var nextDelay = TimeSpan.FromSeconds(1);
                for (int i = 0; i != 3; i++)
                {
                    try { return await client.GetStringAsync(uri); }
                    catch { }

                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }

                // try one last time, allowing the error to propagate
                return await client.GetStringAsync(uri);
            }
        }

        public static async Task<R> DoAnythingWithRetries<R>(int retries, Func<Task<R>> funcToRetry)
        {
            var nextDelay = TimeSpan.FromSeconds(1);
            for (int i = 0; i < retries; i++)
            {
                try { return await funcToRetry(); }
                catch { }

                await Task.Delay(nextDelay);
                nextDelay = nextDelay + nextDelay;
            }

            // try one last time, allowing the error to propagate
            return await funcToRetry();
        }

        //public static Tuple<Promise<string>, RetryOptions> DownloadStringWithRetries(string uri, RetryOptions state)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        if (state.GiveUp) return Tuple(async () => await client.GetStringAsync(uri), state);

        //        var nextDelay = TimeSpan.FromSeconds(1);
        //        for (int i = 0; i != 3; i++)
        //        {
        //            try { return await client.GetStringAsync(uri); }
        //            catch { }

        //            await Promise.Delay(nextDelay);
        //            nextDelay = nextDelay + nextDelay;
        //        }

        //        // try one last time, allowing the error to propagate
        //        return await client.GetStringAsync(uri);
        //    }
        //}
    }
}