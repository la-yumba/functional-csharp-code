using Boc.Domain.Events;
using System;
using System.Collections.Generic;

namespace Boc.Data
{
    public interface IEventStore
    {
        void Persist(Event e);
        void Persist(IEnumerable<Event> e);
        IEnumerable<Event> GetEvents(Guid id);
    }
}