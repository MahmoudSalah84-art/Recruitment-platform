using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserById
{
	// Get User By Id Query
	public class GetUserByIdQuery : IRequest<Result<UserDto>>
	{
		public Guid UserId { get; set; }
	}
}
