using MediatR;

namespace Jobs.Application.Abstractions.Messaging
{
	public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}
