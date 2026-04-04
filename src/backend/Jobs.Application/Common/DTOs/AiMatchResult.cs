using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Common.DTOs
{
	public sealed record AiMatchResult(
	int MatchScore,
	string Decision,
	IReadOnlyList<string> MissingSkills,
	string Explanation);
}
