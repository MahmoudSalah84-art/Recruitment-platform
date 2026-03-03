using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserByEmail
{

	// Get User By Email Query
	public class GetUserByEmailQuery : IRequest<Result<UserDto>>
	{
		public string Email { get; set; }
	}
}
