using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.CVJobRecommendation.Command.DeactivateRecommendation
{
	public record DeactivateRecommendationCommand(string RecommendationId) : ICommand;

}
