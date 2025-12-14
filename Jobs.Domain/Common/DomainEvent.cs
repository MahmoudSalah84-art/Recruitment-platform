using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Common
{
	public abstract class DomainEvent
    {
		public DateTime OccurredOn { get; protected set; }

	}
}
