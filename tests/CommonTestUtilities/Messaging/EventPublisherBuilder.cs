using FCG.Catalog.Domain.Messaging;
using FCG.Catalog.Domain.Repositories;
using FCG.Shared.Events;
using Moq;

namespace CommonTestUtilities.Messaging;

public class EventPublisherBuilder
{
    private readonly Mock<IEventPublisher> _mock;
    public EventPublisherBuilder()
    {
        _mock = new Mock<IEventPublisher>();
    }

    public void VerifyPublishOrderPlacedEventOnce()
    {
        _mock.Verify(
            publisher => publisher.Publish(It.IsAny<OrderPlacedEvent>()),
            Times.Once
        );
    }

    public void VerifyPublishNever()
    {
        _mock.Verify(
            publisher => publisher.Publish(It.IsAny<object>()),
            Times.Never);
    }


    public IEventPublisher Build() => _mock.Object;
}
