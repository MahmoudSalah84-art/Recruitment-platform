using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Skills.Queries.GetSkillsByJob
{
	public record GetSkillsByJobQuery(string JobId) : ICommand<List<JobSkillResponse>>;
}
