//using LaYumba.Functional;
//using static System.Configuration.ConfigurationManager;

//namespace Examples.Chapter2
//{
//   static class ParseInt_Example
//   {
//      static RedisClient CreateRedisClient()
//      {
//         var host = AppSettings["Redis.Host"] ?? "localhost";

//         int port;
//         if (!int.TryParse(AppSettings["Redis.Port"], out port))
//            port = 6359;

//         int db;
//         if (!int.TryParse(AppSettings["Redis.Db"], out db))
//            db = 0;

//         return RedisClient.Create(host, port, db);
//      }

//      static string RedisConnString(string host, int port, int db)
//         => $"redis://{host}:{port}?db={db}";
//   }

//   static class ParseInt_Example_V2
//   {
//      static RedisClient CreateRedisClient()
//         => RedisClient.Create(
//            AppSettings["Redis.Host"] ?? "localhost",
//            AppSettings["Redis.Port"].ParseInt().GetOrElse(6359),
//            AppSettings["Redis.Db"].ParseInt().GetOrElse(0));
//   }

//   public class RedisClient
//   {
//      private int db;
//      private string host;
//      private int port;

//      public RedisClient(string host, int port, int db)
//      {
//         this.host = host;
//         this.port = port;
//         this.db = db;
//      }

//      public static RedisClient Create(string host, int port, int db)
//         => new RedisClient(host, port, db);
//   }
//}
