using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.Events
{
    public class UserVerifiedEvent : DomainEvent
    {
        
        public User User { get; }

        public UserVerifiedEvent(User user)
        {
            User = user;
			OccurredOn = DateTime.UtcNow;

		}
	}
}
