using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Applications.Commands.WithdrawApplication
{
	public record WithdrawApplicationCommand(Guid ApplicationId) : IRequest<Result>;

}
