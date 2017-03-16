using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using static System.Console;
using Examples.Chapter13;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Examples.Agents
{
   using CcyAgents = ImmutableDictionary<string, Agent<string>>;

   public static class CurrencyLookup
   {
      class FxRateRequest
      {
         public string CcyPair { get; set; }
         public string Sender { get; set; } // the sender, to which the response should be sent
      }

      class FxRateResponse
      {
         public string CcyPair { get; set; }
         public decimal Rate { get; set; }
         public string Recipient { get; set; }
      }

      public static void SetUp(MessageBroker broker)
      {
         var sendResponse = Agent.Start(
            (FxRateResponse res) => broker.Send(res.Recipient, res));

         var processRequest = StartReqProcessor(sendResponse);

         broker.Subscribe<FxRateRequest>("FxRateRequests", processRequest.Tell); // when a request is received, pass it to the processing agent
      }                                                                          // here we go from multithreaded to sequential

      static Agent<FxRateRequest> StartReqProcessor(Agent<FxRateResponse> sendResponse)
         => Agent.Start(CcyAgents.Empty, (CcyAgents state, FxRateRequest request) =>
         {
            string ccyPair = request.CcyPair;

            Agent<string> agent = state
               .Lookup(ccyPair)
               .GetOrElse(() => StartAgentFor(ccyPair, sendResponse));

            agent.Tell(request.Sender);
            return state.Add(ccyPair, agent);
         });

      static Agent<string> StartAgentFor
         (string ccyPair, Agent<FxRateResponse> sendResponse)
         => Agent.Start<Option<decimal>, string>(None, async (optRate, recipient) =>
         {
            decimal rate = await optRate.Map(Async)
               .GetOrElse(() => Yahoo.GetRate(ccyPair));

            sendResponse.Tell(new FxRateResponse
            {
               CcyPair = ccyPair,
               Rate = rate,
               Recipient = recipient,
            });

            return Some(rate);
         });


      // test methods

      public static void TestViaCmdLine()
      {
         var sendResponse = Agent.Start((FxRateResponse res)
            => WriteLine($"{res.CcyPair}: {res.Rate}"));

         var rateLookup = StartReqProcessor(sendResponse);

         WriteLine("Enter a currency pair like 'EURUSD' to get a quote, or 'q' to quit");
         for (string input; (input = ReadLine().ToUpper()) != "Q";)
            rateLookup.Tell(new FxRateRequest { CcyPair = input, Sender = "Sender" });
      }

      public static void TestConcurrencyWorksOk()
      {
         var sendResponse = Agent.Start((FxRateResponse res)
            => WriteLine($"{res.CcyPair}: {res.Rate}"));

         var rateLookup = StartReqProcessor(sendResponse);

         Parallel.ForEach(Range(1, 10000)
            , i => rateLookup.Tell(new FxRateRequest { CcyPair = i % 2 == 0 ? "EURUSD" : "GBPUSD", Sender = "Sender" }));
         Task.Delay(10000).Wait();
      }
   }

   // simple implementation using Redis
   public class MessageBroker
   {
      ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

      public void Subscribe<T>(string channel, Action<T> act)
         => redis.GetSubscriber().Subscribe(channel, (_, val) => act(JsonConvert.DeserializeObject<T>(val)));

      public void Send(string channel, object message)
         => redis.GetDatabase(0).PublishAsync(channel, JsonConvert.SerializeObject(message));
   }
}
