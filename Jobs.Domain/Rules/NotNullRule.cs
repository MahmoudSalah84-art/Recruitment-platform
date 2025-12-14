using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class NotNullRule<T> : IBusinessRule
	{
		private readonly T _value;

		public NotNullRule(T value)
		{
			_value = value;
		}

		public string Message => $"{typeof(T).Name} must not be null.";
		public bool IsBroken() => _value is null;



	}

}
