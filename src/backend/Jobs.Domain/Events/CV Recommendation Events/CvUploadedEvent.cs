using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.CV_Recommendation_Events
{

	public record CvUploadedEvent : DomainEvent
	{
		public CvUploadedEvent(string Id) : base(Id)
		{
		}
	}
}
