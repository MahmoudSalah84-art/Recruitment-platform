using Jobs.Domain.Common;

namespace Jobs.Domain.Events.Events
{


	public record UserRegisteredEvent(string Id) : DomainEvent(Id) ;
	

}
