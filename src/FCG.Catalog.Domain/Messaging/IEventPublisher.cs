namespace FCG.Catalog.Domain.Messaging;

public interface IEventPublisher
{
    Task Publish<TEvent>(TEvent @event) where TEvent : class;
}
