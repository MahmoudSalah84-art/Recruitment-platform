using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.GetMyApplications
{
	public record GetMyApplicationsQuery : IRequest<Result<List<ApplicationResponse>>>;

}
