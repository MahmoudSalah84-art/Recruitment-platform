using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Experiences.Commands.DeleteUserExperience
{
	public record DeleteExperienceCommand(Guid Id) : ICommand;

}
