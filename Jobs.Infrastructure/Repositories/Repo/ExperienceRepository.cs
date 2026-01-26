using Jobs.Domain.Entities;
using Jobs.Domain.IRepository;
using Jobs.Domain.Specifications;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
    public class ExperienceRepository : Repository<UserExperience>, IExperienceRepository
    {
        public ExperienceRepository(JobDbContext context) : base(context) { }  
    }
    
}
