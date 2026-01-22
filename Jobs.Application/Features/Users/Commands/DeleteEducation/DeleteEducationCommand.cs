using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.DeleteEducation
{
	public record DeleteEducationCommand(Guid Id) : IRequest<Result>;

}
