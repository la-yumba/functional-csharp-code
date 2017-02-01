using System.Threading;
using static System.Console;
using LaYumba.Functional;
using System.Threading.Tasks;

namespace Examples.Agents
{
   public class PingPongAgents
   {
      public static void main()
      {
         Agent<string> logger, ping, pong = null;

         logger = Agent.Start((string msg) => WriteLine(msg));

         ping = Agent.Start((string msg) =>
         {
            if (msg == "STOP") return;

            logger.Tell($"Received '{msg}'; Sending 'PING'");
            Task.Delay(500).Wait();
            pong.Tell("PING");
         });

         pong = Agent.Start(0, (int count, string msg) =>
         {
            int newCount = count + 1;
            string nextMsg = (newCount < 5) ? "PONG" : "STOP"; 

            logger.Tell($"Received '{msg}' #{newCount}; Sending '{nextMsg}'");
            Task.Delay(500).Wait();
            ping.Tell(nextMsg);

            return newCount;
         });

         ping.Tell("START");

         Thread.Sleep(10000);
      }
   }
}
