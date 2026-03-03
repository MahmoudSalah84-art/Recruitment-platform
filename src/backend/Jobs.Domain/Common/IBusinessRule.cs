using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Common
{
	public interface IBusinessRule
	{
		bool IsBroken();
		string Message { get; }
	}
}
