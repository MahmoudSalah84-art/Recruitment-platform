using Jobs.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserClaims
{

	// Get User Claims Query
	public class GetUserClaimsQuery : IRequest<Result<List<ClaimDto>>>
	{
		public Guid UserId { get; set; }
	}
}
