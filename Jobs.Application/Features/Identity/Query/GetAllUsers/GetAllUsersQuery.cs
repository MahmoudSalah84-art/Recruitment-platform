using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetAllUsers
{
	// Get All Users Query
	public class GetAllUsersQuery : IRequest<Result<List<UserDto>>>
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public string SearchTerm { get; set; }
		public bool? IsActive { get; set; }
	}

}
