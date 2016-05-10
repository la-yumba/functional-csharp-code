using System;

namespace Boc
{
   public interface IEvent
   {
      Guid EntityId { get; }
      string EntityType { get; }
      
      //int Version { get; }
      DateTime Timestamp { get; }
      //string UserName { get; }
   }
}