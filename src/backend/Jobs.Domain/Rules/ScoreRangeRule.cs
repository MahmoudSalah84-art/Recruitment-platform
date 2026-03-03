using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class ScoreRangeRule : IBusinessRule
	{
		private readonly int _score;

		public ScoreRangeRule(int score)
		{
			_score = score;
		}

		public bool IsBroken() => _score < 0 || _score > 100;

		public string Message => "Recommendation score must be between 0 and 100.";
	}
}
