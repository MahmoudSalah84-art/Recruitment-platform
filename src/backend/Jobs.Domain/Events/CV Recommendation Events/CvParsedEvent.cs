using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.CV_Recommendation_Events
{
    public class CvParsedEvent : DomainEvent
    {
		public string CvId { get; }
		public string UserId { get; }


        public CvParsedEvent(CV cv)
        {
			CvId = cv.Id;
			UserId = cv.UserId;
			OccurredOn = DateTime.UtcNow;
		}
		
		
	}

}
