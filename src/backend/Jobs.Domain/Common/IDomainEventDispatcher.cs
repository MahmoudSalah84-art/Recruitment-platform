

namespace Jobs.Domain.Common
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(DomainEvent domainEvent);
    }

}
