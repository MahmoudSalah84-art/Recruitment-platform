using Jobs.Application.Abstractions.Messaging;
using MediatR;

namespace Jobs.Application.Abstractions.Interfaces
{
	public interface IBackgroundJobService
	{
		void Enqueue<TCommand>(TCommand command) where TCommand : IBaseRequest;
	}
}
