using Jobs.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Command.GenerateEmailConfirmationToken
{
	public record GenerateEmailConfirmationTokenCommand(string UserId) : ICommand;

}
