using FCG.Catalog.Domain.Messaging;
using FCG.Shared.Events;
using MassTransit;

namespace FCG.Catalog.Infrastructure.Messaging;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;   
    }

    public async Task Publish<TEvent>(TEvent @event) where TEvent : class
    {
        await _publishEndpoint.Publish(@event);
    }
}
