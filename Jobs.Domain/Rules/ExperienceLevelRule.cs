using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class ExperienceLevelRule : IBusinessRule
	{
		private readonly int _experienceLevel;

		public ExperienceLevelRule(int experienceLevel)
		{
			_experienceLevel = experienceLevel;
		}

		public bool IsBroken()
		{
			
			return _experienceLevel < 0 || _experienceLevel > 40;
		}

		public string Message =>
			"Experience level must be between 0 and 40 years.";
	}
}
