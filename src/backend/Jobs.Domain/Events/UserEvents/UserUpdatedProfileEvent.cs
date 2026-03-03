using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.UserEvents
{
    public class UserUpdatedProfileEvent : DomainEvent
    {
        public User User { get; }

        public UserUpdatedProfileEvent(User user)
        {
            User = user;
			OccurredOn = DateTime.UtcNow;

		}
	}
}
