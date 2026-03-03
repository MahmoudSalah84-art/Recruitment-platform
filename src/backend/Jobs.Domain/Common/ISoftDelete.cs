using Jobs.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Common
{
	public interface ISoftDelete  
	{
		bool IsDeleted { get; set; }
		DateTime? DeletedAt { get;  set; }

		void SoftDelete();
	}
}
