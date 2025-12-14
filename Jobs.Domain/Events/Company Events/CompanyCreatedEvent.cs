using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.Company_Events
{
    public class CompanyCreatedEvent : DomainEvent
    {
        public Company Company { get; }
        

        public CompanyCreatedEvent(Company company)
        {
            Company = company;
            OccurredOn = DateTime.UtcNow;
		}
    }

}
