using MediatR;

namespace Heus.Ddd.Domain;
public interface IDomainEventDispatcher
{
    Task Dispatch(params IDomainEvent[] events);
}
public interface IDomainEventDispatcher<in TEvent>
    : INotificationHandler<TEvent> where TEvent : IDomainEvent { }