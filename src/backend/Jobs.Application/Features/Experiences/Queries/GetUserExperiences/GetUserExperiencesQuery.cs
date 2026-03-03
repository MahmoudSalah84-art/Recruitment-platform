using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Experiences.Queries.GetUserExperiences
{
	public record GetUserExperiencesQuery() : IQuery<List<ExperienceDto>>;
}
