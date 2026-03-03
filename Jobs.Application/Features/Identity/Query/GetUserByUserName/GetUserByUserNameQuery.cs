using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserByUserName
{

	// Get User By UserName Query
	public class GetUserByUserNameQuery : IRequest<Result<UserDto>>
	{
		public string UserName { get; set; }
	}
}








