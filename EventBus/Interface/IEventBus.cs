using EventBus.Events;
using EventBus.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Interface
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
        void Subscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>;
    }
}
