using Jobs.Application.Abstractions.Messaging;
using MediatR;

namespace Jobs.API.DTOs
{
	public class CreateOrUpdateFileDto : IRequest<Result<object>>
	{
		public string UserId { get; set; }
		public IFormFile File { get; set; }
	}
}
