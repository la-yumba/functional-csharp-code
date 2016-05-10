using System;

namespace Boc.Events
{
   public class Event : IEvent
   {
      protected Event(string entityType, Guid entityId, int version, string userName, DateTime timestamp)
      {
         EntityType = entityType;
         EntityId = entityId;
         Version = version;
         Timestamp = timestamp;
         UserName = userName;
      }

      public string EntityType { get; }
      public Guid EntityId { get; }
      public int Version { get; }
      public DateTime Timestamp { get; }
      public string UserName { get; }
   }
}
