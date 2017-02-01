using Boc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Boc.Data
{
   public class InMemoryEventStore : IEventStore
   {
      private readonly List<Event> _store = new List<Event>();

      public void Persist(Event e)
      {
         _store.Add(e);
      }

      public void Persist(IEnumerable<Event> e)
      {
         _store.AddRange(e);
      }

      public IEnumerable<Event> GetEvents(Guid id)
      {
         return from stored in _store
                where stored.EntityId.Equals(id)
                orderby stored.Timestamp
                select stored;
      }
      
      public int Count()
      {
         return _store.Count;
      }
   }
}