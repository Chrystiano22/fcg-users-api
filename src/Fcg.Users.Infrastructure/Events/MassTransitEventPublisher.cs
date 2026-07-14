using Fcg.Users.Application.Events;
using MassTransit;

namespace Fcg.Users.Infrastructure.Events;

public sealed class MassTransitEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        return _publishEndpoint.Publish(@event, cancellationToken);
    }
}
